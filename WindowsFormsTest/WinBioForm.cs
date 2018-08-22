using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.ServiceProcess;
using WinBioNET;
using WinBioNET.Enums;

namespace WindowsFormsTest
{
    public partial class WinBioForm
        : Form
    {
        private static readonly Guid DatabaseId = Guid.Parse("BC7263C3-A7CE-49F3-8EBF-D47D74863CC6");
        private WinBioSessionHandle _session;
        private int _unitId;

        public WinBioForm()
        {
            InitializeComponent();
        }

        protected void Log(string message)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke(new Action<string>(Log), message);
                return;
            }
            richTextBox.AppendText(message + "\n");
        }

        protected void Log(WinBioException exception)
        {
            Log(exception.Message);
        }

        protected override void OnLoad(EventArgs e)
        {
            var units = WinBio.EnumBiometricUnits(WinBioBiometricType.Fingerprint);
            Log(string.Format("Found {0} units", units.Length));

            // Check if we have a connected fingerprint sensor
            if (units.Length == 0)
            {
                MessageBox.Show("Error: Fingerprint sensor not found! Exiting...", "Error", MessageBoxButtons.OK);
                Application.Exit();
                return;
            }
            
            var unit = units[0];
            _unitId = unit.UnitId;
            Log(string.Format("Using unit id: {0}", _unitId));
            Log(string.Format("Device instance id: {0}", unit.DeviceInstanceId));
            Log(string.Format("Using database: {0}", DatabaseId));

            // Check if we need to create a new database
            if (WinBioConfiguration.DatabaseExists(DatabaseId) == false)
            {
                InitialStart();
            }
            else
            {
                OpenBiometricSession();
            }
        }

        private void OpenBiometricSession()
        {
            _session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.System, WinBioSessionFlag.Raw, null, WinBioDatabaseId.Default);
            Log("Session opened: " + _session.Value);
        }

        private void WinBioForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_session.IsValid) return;
            WinBio.CloseSession(_session);
            _session.Invalidate();
            Log("Session closed");
        }

        private void buttonLocateSensor_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Log("Locating sensor...");
                try
                {
                    var unitId = WinBio.LocateSensor(_session);
                    Log(string.Format("Sensor located: unit id {0}", unitId));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonIdentify_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Log("Identifying user...");
                try
                {
                    WinBioIdentity identity;
                    WinBioBiometricSubType subFactor;
                    WinBioRejectDetail rejectDetail;
                    WinBio.Identify(_session, out identity, out subFactor, out rejectDetail);
                    Log(string.Format("Identity: {0}", identity));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonEnroll_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                try
                {
                    var identity = AddEnrollment(_session, _unitId, WinBioBiometricSubType.RhIndexFinger);
                    Log(string.Format("Identity: {0}", identity));
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            WinBio.Cancel(_session);
        }

        private WinBioIdentity AddEnrollment(WinBioSessionHandle session, int unitId, WinBioBiometricSubType subType)
        {
            Log(string.Format("Beginning enrollment of {0}:", subType));
            WinBio.EnrollBegin(session, subType, unitId);
            var code = WinBioErrorCode.MoreData;
            for (var swipes = 1; code != WinBioErrorCode.Ok; swipes++)
            {
                WinBioRejectDetail rejectDetail;
                code = WinBio.EnrollCapture(session, out rejectDetail);
                switch (code)
                {
                    case WinBioErrorCode.MoreData:
                        Log(string.Format("Swipe {0} was good", swipes));
                        break;
                    case WinBioErrorCode.BadCapture:
                        Log(string.Format("Swipe {0} was bad: {1}", swipes, rejectDetail));
                        break;
                    case WinBioErrorCode.Ok:
                        Log(string.Format("Enrollment complete with {0} swipes", swipes));
                        break;
                    default:
                        throw new WinBioException(code, "WinBioEnrollCapture failed");
                }
            }
            Log(string.Format("Committing enrollment.."));
            WinBioIdentity identity;
            var isNewTemplate = WinBio.EnrollCommit(session, out identity);
            Log(string.Format(isNewTemplate ? "New template committed." : "Template already existing."));
            return identity;
        }

        private void btnRebuildDatabase_Click(object sender, EventArgs e)
        {
            // Close existing session
            if (_session.IsValid)
            {
                WinBio.Cancel(_session);
            }

            InitialStart();
        }

        private void InitialStart()
        {
            // Stop Windows Biometric Service to apply changes
            RestartService("WbioSrvc", 5000, ServiceMode.Stop);

            var databases = WinBio.EnumDatabases(WinBioBiometricType.Fingerprint);
            Console.WriteLine("Found {0} databases", databases.Length);
            for (var i = 0; i < databases.Length; i++)
            {
                Console.WriteLine("DatabaseId {0}: {1}", i, databases[i].DatabaseId);
            }
            if (WinBioConfiguration.DatabaseExists(DatabaseId))
            {
                Console.WriteLine("Removing database: {0}", DatabaseId);
                WinBioConfiguration.RemoveDatabase(DatabaseId);
            }

            // Start Windows Biometric Service to apply changes
            RestartService("WbioSrvc", 5000, ServiceMode.Start);

            Console.WriteLine("Creating database: {0}", DatabaseId);
            WinBioConfiguration.AddDatabase(DatabaseId, _unitId);
            Console.WriteLine("Adding sensor to the pool: {0}", _unitId);
            WinBioConfiguration.AddUnit(DatabaseId, _unitId);

            // Restart Windows Biometric Service to apply changes
            RestartService("WbioSrvc", 5000, ServiceMode.Restart);

            Log("Successfully recreated database.");

            OpenBiometricSession();
        }

        public enum ServiceMode
        {
            Restart,
            Stop,
            Start
        }

        private void RestartService(string serviceName, int timeoutMilliseconds, ServiceMode serviceMode)
        {
            ServiceController service = new ServiceController(serviceName);
            try
            {
                int millisec1 = Environment.TickCount;
                TimeSpan timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds);

                if (serviceMode == ServiceMode.Start)
                    goto start;

                service.Stop();
                service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);

                if (serviceMode == ServiceMode.Stop)
                    return;

                service.Start();

                start:
                // count the rest of the timeout
                int millisec2 = Environment.TickCount;
                timeout = TimeSpan.FromMilliseconds(timeoutMilliseconds - (millisec2 - millisec1));

                service.WaitForStatus(ServiceControllerStatus.Running, timeout);
            }
            catch
            {
                MessageBox.Show("Error restarting Windows Biometric Service", "Error", MessageBoxButtons.OK);
            }
        }

        private void richTextBox_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox.SelectionStart = richTextBox.Text.Length;

            // scroll it automatically
            richTextBox.ScrollToCaret();
        }

        private void buttonCaptureSample_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                Bitmap image;
                WinBioRejectDetail rejectDetail;
                Log("Capturing sample...");
                try
                {
                    WinBio.CaptureSample(_session, WinBioBirPurpose.NoPurposeAvailable, WinBioBirDataFlags.Raw, out rejectDetail, out image);
                    if (rejectDetail != WinBioRejectDetail.None)
                    {
                        Log(string.Format("CaptureSample failed! Reject detail: {0}", rejectDetail));
                    }
                    else
                    {
                        Log("Captured sample successfully!");
                        this.fingerprintPictureBox.BackgroundImage = image;
                    }
                }
                catch (WinBioException ex)
                {
                    Log(ex);
                }
            });
        }
    }
}

using System;
using System.Threading;
using System.Windows.Forms;
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
            if (units.Length == 0) return;
            var unit = units[0];
            _unitId = unit.UnitId;
            Log(string.Format("Using unit id: {0}", _unitId));
            Log(string.Format("Device instance id: {0}", unit.DeviceInstanceId));
            Log(string.Format("Using database: {0}", DatabaseId));
            _session = WinBio.OpenSession(WinBioBiometricType.Fingerprint, WinBioPoolType.Private, WinBioSessionFlag.Basic, new[] { _unitId }, DatabaseId);
            //_session = WinBio.OpenSession(WinBioBiometricType.Fingerprint);
            Log("Session opened: " + _session.Value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
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
    }
}

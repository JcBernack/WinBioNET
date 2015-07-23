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
            Log("Session opened: " + _session.Value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
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
                    Log(string.Format("Identity: {0} ({1})", identity, subFactor));
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
    }
}

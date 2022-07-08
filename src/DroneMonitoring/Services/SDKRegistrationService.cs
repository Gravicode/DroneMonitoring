using DJI.WindowsSDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneMonitoring.Services
{
    public class SDKRegistrationEventArgs : EventArgs
    {
        public SDKError Result { get; set; }
        public SDKRegistrationState State { set; get; }
    }
    public class SDKRegistrationService
    {
        public EventHandler<SDKRegistrationEventArgs> StateChanged;
        public bool IsActive { get; set; }
        public string ActivationInfo { get; set; }
        public SDKRegistrationService()
        {
            DJISDKManager.Instance.SDKRegistrationStateChanged += Instance_SDKRegistrationEvent;
        }


        private void Instance_SDKRegistrationEvent(SDKRegistrationState state, SDKError resultCode)
        {

            IsActive = state == SDKRegistrationState.Succeeded;
            ActivationInfo = resultCode == SDKError.NO_ERROR ? "Register success" : resultCode.ToString();
            StateChanged?.Invoke(this, new SDKRegistrationEventArgs { Result = resultCode, State = state });
        }

        public void Register(string AppKey)
        {
            DJISDKManager.Instance.RegisterApp(AppKey);
        }

    }
}

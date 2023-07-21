using System;
using UnityEngine;

namespace PrimePeaks.InstallReferrer {
    
    internal class InstallReferrerStateListener : AndroidJavaProxy {
        
        private readonly Action<int> onSetupFinished;
        private readonly Action onServiceDisconnected;
        
        public InstallReferrerStateListener(Action<int> onSetupFinished, Action onServiceDisconnected) : base("com.android.installreferrer.api.InstallReferrerStateListener") {
            this.onSetupFinished = onSetupFinished;
            this.onServiceDisconnected = onServiceDisconnected;
        }

        public void onInstallReferrerSetupFinished(int responseCode) {
            onSetupFinished?.Invoke(responseCode);
        }

        public void onInstallReferrerServiceDisconnected() {
            onServiceDisconnected?.Invoke();
        }
        
    }
    
}

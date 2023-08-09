using System;
using System.Threading.Tasks;
using UnityEngine;

namespace PrimePeaks.InstallReferrer {
    
    public class InstallReferrerClient : IDisposable {
        
        private readonly AndroidJavaObject client;
        
        public InstallReferrerClient() {
            var ctx = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            var clientClass = new AndroidJavaClass("com.android.installreferrer.api.InstallReferrerClient");
            client = clientClass.CallStatic<AndroidJavaObject>("newBuilder", ctx).Call<AndroidJavaObject>("build");
        }
        
        public Task<InstallReferrerDetails> GetInstallReferrerAsync() {
            var tsc = new TaskCompletionSource<InstallReferrerDetails>();
            
            client.Call("startConnection", new InstallReferrerStateListener(
                code => {
                    var responseCode = (InstallReferrerResponse) code;
                    if (responseCode == InstallReferrerResponse.Ok) {
                        var details = client.Call<AndroidJavaObject>("getInstallReferrer");
                        tsc.SetResult(new InstallReferrerDetails {
                            InstallReferrer = details.Call<string>("getInstallReferrer"),
                            ReferrerClickTimestampSeconds = details.Call<long>("getReferrerClickTimestampSeconds"),
                            InstallBeginTimestampSeconds = details.Call<long>("getInstallBeginTimestampSeconds"),
                            ReferrerClickTimestampServerSeconds = details.Call<long>("getReferrerClickTimestampServerSeconds"),
                            InstallBeginTimestampServerSeconds = details.Call<long>("getInstallBeginTimestampServerSeconds"),
                            InstallVersion = details.Call<string>("getInstallVersion"),
                            GooglePlayInstantParam = details.Call<bool>("getGooglePlayInstantParam")
                        });
                    } else {
                        tsc.SetException(new InstallReferrerException(responseCode));
                    }
                    client.Call("endConnection");
                },
                () => {
                    if (tsc.Task.Result != null) return;
                    tsc.SetException(new InstallReferrerException(InstallReferrerResponse.ServiceDisconnected));
                }
            ));
            
            return tsc.Task;
        }

        public void Dispose() {
            client?.Dispose();
        }
    }
    
}

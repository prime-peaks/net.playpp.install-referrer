using System;
using UnityEngine;

namespace PrimePeaks.InstallReferrer {
    
    public class InstallReferrerClient : IDisposable {
        
        private readonly AndroidJavaObject client;
        
        private InstallReferrerRequest request;
        
        public InstallReferrerClient() {
            var ctx = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            var clientClass = new AndroidJavaClass("com.android.installreferrer.api.InstallReferrerClient");

            request = new InstallReferrerRequest();
            
            client = clientClass.CallStatic<AndroidJavaObject>("newBuilder", ctx).Call<AndroidJavaObject>("build");
            client.Call("startConnection", new InstallReferrerStateListener(
                code => {
                    var responseCode = (InstallReferrerResponse) code;
                    if (responseCode == InstallReferrerResponse.Ok) {
                        var details = client.Call<AndroidJavaObject>("getInstallReferrer");
                        request.InstallReferrerDetails = new InstallReferrerDetails {
                            InstallReferrer = details.Call<string>("getInstallReferrer"),
                            ReferrerClickTimestampSeconds = details.Call<long>("getReferrerClickTimestampSeconds"),
                            InstallBeginTimestampSeconds = details.Call<long>("getInstallBeginTimestampSeconds"),
                            ReferrerClickTimestampServerSeconds = details.Call<long>("getReferrerClickTimestampServerSeconds"),
                            InstallBeginTimestampServerSeconds = details.Call<long>("getInstallBeginTimestampServerSeconds"),
                            InstallVersion = details.Call<string>("getInstallVersion"),
                            GooglePlayInstantParam = details.Call<bool>("getGooglePlayInstantParam")
                        };
                    }
                    request.ResponseCode = responseCode;
                },
                () => {
                    if (request.IsDone) return;
                    request.ResponseCode = InstallReferrerResponse.ServiceDisconnected;
                }
            ));
        }
        
        public InstallReferrerRequest GetInstallReferrerAsync() {
            return request;
        }

        public void Dispose() {
            client.Call("endConnection");
            client.Dispose();
        }
        
    }
}

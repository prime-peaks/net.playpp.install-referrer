using System;
using System.Threading;
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
            var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
            
            client.Call("startConnection", new InstallReferrerStateListener(
                code => {
                    var responseCode = (InstallReferrerResponse) code;
                    if (responseCode == InstallReferrerResponse.Ok) {
                        GetInstallReferrer(tsc, scheduler);
                    } else {
                        tsc.SetException(new InstallReferrerException(responseCode));
                        EndConnection();
                    }
                },
                () => {
                    if (tsc.Task.Result != null) return;
                    tsc.SetException(new InstallReferrerException(InstallReferrerResponse.ServiceDisconnected));
                }
            ));
            
            return tsc.Task;
        }

        private void GetInstallReferrer(TaskCompletionSource<InstallReferrerDetails> tsc, TaskScheduler scheduler) {
            Task.Run(() => {
                try {
                    AndroidJNI.AttachCurrentThread();
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
                } finally {
                    AndroidJNI.DetachCurrentThread();
                }
            }).ContinueWith(task => {
                EndConnection();
                if (!task.IsFaulted) return;

                tsc.SetException(task.Exception is { InnerException: {} }
                    ? task.Exception.InnerException
                    : new InstallReferrerException(InstallReferrerResponse.Unknown));
            }, scheduler);
        }

        private void EndConnection() {
            try {
                client.Call("endConnection");
            } catch (Exception e) {
                Debug.LogError($"Failed to end connection with Install Referrer service: {e.Message}");
            }
        }

        public void Dispose() {
            client?.Dispose();
        }
    }
    
}

# Play Install Referrer Wrapper for Unity

This wrapper is for the [Play Install Referrer Library](https://developer.android.com/google/play/installreferrer/library). It provides a C# API for accessing install referrer details from a Unity app running on Android.

## Usage

Add this repository URL to the Unity Package Manager.

```csharp
var installReferrerClient = new InstallReferrerClient();
var installReferrerTask = installReferrerClient.GetInstallReferrerAsync();
await installReferrerTask.ContinueWith(task => {
    if (!task.IsCompletedSuccessfully) return;

    var details = task.Result;
    Debug.Log($"Install referrer: {details.InstallReferrer}");
    Debug.Log($"Referrer click timestamp: {details.ReferrerClickTimestampSeconds}");
    Debug.Log($"Install begin timestamp: {details.InstallBeginTimestampSeconds}");
    Debug.Log($"Referrer click timestamp server: {details.ReferrerClickTimestampServerSeconds}");
    Debug.Log($"Install begin timestamp server: {details.InstallBeginTimestampServerSeconds}");
    Debug.Log($"Install version: {details.InstallVersion}");
    Debug.Log($"Google play instant param: {details.GooglePlayInstantParam}");
});
```

## How to Modify This Package

These instructions are for macOS. Additional research is required for other platforms.

### Steps

1. Check out the package from the remote repository. For convenience, use the package name as the folder name.
2. Open Terminal.
3. Change the current working directory to the `Packages` folder in the Unity project where you've used this package.
```
cd /path/to/UnityProject/Packages
```
4. Create a symbolic link to the package folder.
```
ln -s /path/to/net.playpp.install-referrer .
```
5. Open the Unity project to allow Unity to recognize the changes.
6. Modify the package and test it in the Unity project.
7. Commit and push the changes to the remote repository.
8. Remove the symbolic link.
```
rm net.playpp.install-referrer
```
9. Open the Unity project again so Unity can recognize the changes.
10. Commit the updated `packages-lock.json` in the Unity project.

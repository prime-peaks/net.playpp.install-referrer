namespace PrimePeaks.InstallReferrer {
    
    public enum InstallReferrerResponse {
        Ok = 0,
        ServiceUnavailable = 1,
        FeatureNotSupported = 2,
        DeveloperError = 3,
        ServiceDisconnected = -1,
        Unknown = int.MinValue, 
    }
    
}

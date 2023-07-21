namespace PrimePeaks.InstallReferrer {
    
    public class InstallReferrerDetails {
        
        public string InstallReferrer { get; internal set; }
        public long ReferrerClickTimestampSeconds { get; internal set; }
        public long InstallBeginTimestampSeconds { get; internal set; }
        public long ReferrerClickTimestampServerSeconds { get; internal set; }
        public long InstallBeginTimestampServerSeconds { get; internal set; }
        public string InstallVersion { get; internal set; }
        public bool GooglePlayInstantParam { get; internal set; }
        
    }
    
}

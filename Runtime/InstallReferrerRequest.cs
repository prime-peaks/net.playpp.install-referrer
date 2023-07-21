using UnityEngine;

namespace PrimePeaks.InstallReferrer {
    
    public class InstallReferrerRequest : CustomYieldInstruction {
        
        public bool IsDone => ResponseCode != InstallReferrerResponse.Unknown;
        
        public override bool keepWaiting => !IsDone;

        public InstallReferrerResponse ResponseCode { get; internal set; } = InstallReferrerResponse.Unknown;
        
        public InstallReferrerDetails InstallReferrerDetails { get; internal set; }
        
    }
    
}

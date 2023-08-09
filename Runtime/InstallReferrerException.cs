namespace PrimePeaks.InstallReferrer {

    public class InstallReferrerException : System.Exception {

        public InstallReferrerResponse ResponseCode { get; private set; }

        internal InstallReferrerException(InstallReferrerResponse code) : base($"Failed to connect to Install Referrer service: {code}") {
            ResponseCode = code;
        }

    }

}

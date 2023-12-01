using aow3.Debugging;

namespace aow3
{
    public class aow3Consts
    {
        public const string LocalizationSourceName = "aow3";

        public const string ConnectionStringName = "Default";

        public const bool MultiTenancyEnabled = true;


        /// <summary>
        /// Default pass phrase for SimpleStringCipher decrypt/encrypt operations
        /// </summary>
        public static readonly string DefaultPassPhrase =
            DebugHelper.IsDebug ? "gsKxGZ012HLL3MI5" : "7e5144411b504f89a63c500b23864240";
    }
}

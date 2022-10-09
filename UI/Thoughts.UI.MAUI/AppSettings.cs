namespace Thoughts.UI.MAUI
{
    /// <summary>
    /// General application settings.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Api address for work on devices.
        /// </summary>
        public string DeviceWebAPI { get; set; }

        /// <summary>
        /// Api address for debug except Android.
        /// </summary>
        public string DebugWebAPI { get; set; }

        /// <summary>
        /// Api address for debug on emulator Android only.
        /// </summary>
        public string DebugWebAPIAndroid { get; set; }
    }
}

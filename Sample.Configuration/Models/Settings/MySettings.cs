namespace Sample.Configuration.Models.Settings
{
    public class MySettings
    {
        public MySettings()
        {
            SubSettings = new SubSettings();
        }

        public string Id { get; set; }

        public SubSettings SubSettings { get; set; }
    }
}
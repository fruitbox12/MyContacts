namespace Settings
{
    //interfaces are abstract classes
    public interface IAppSettings
    {
        string CurrentConnectionName { get; set; }
        string CurrentConnectionString { get; set; }
        public bool Trace { get; set; }

    }
}
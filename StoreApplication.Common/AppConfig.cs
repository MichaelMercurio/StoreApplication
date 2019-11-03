namespace StoreApplication.Common
{
    // class for accessing config entries, populated during Startup.cs
    public class AppConfig
    {
        public static string ConnectionString { get; set; }
        public static string JwtSecret { get; set; }
        public static string Salt { get; set; }
    }
}

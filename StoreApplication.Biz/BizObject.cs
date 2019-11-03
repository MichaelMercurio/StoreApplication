using StoreApplication.Common;

namespace StoreApplication.Biz
{
    public class BizObject
    {
        // database connection string, available to all biz objects
        public string ConnStr { get; set; }

        // constructor
        public BizObject()
        {
            // retrieve connection string from web.config
            ConnStr = AppConfig.ConnectionString;
        }
    }
}

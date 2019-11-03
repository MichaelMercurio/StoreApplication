using System.Data.SqlClient;

namespace StoreApplication.Data
{
    /// <summary>
    /// DataObject is the class from which all classes in the Data Services
    /// Tier inherit.
    /// </summary>
    public abstract class DataObject
    {
        protected string Dsn { get; set; }
        protected SqlTransaction Trans { get; set; }

        public DataObject(string newDsn)
        {
            Dsn = newDsn;
        }

        public DataObject(string newDsn, SqlTransaction newTrans)
        {
            Dsn = newDsn;
            if (newTrans != null)
            {
                Trans = newTrans;
            }
        }
    }
}

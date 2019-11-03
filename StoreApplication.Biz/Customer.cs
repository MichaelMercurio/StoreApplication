using System.Data;
using System.Data.SqlClient;

using StoreApplication.Data;

namespace StoreApplication.Biz
{
    public class Customer : BizObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        // create blank object and set defaults
        public Customer()
        {
            ResetProperties();
        }

        // create object and retrieve values from data layer
        public Customer(int id)
        {
            ResetProperties();
            Id = id;

            LoadFromID();
        }

        public Customer(string emailAddress)
        {
            ResetProperties();
            Id = GetCustomerIdForEmail(emailAddress);

            LoadFromID();
        }

        // reset the properties
        private void ResetProperties()
        {
            Id = -1;
            Name = "";
            Email = "";
            Password = "";
        }

        // retrieve values from database
        private void LoadFromID()
        {
            CustomerData dao = new CustomerData(ConnStr);
            CustomerDetails details = dao.RetrieveDetails(Id);

            Name = details.Name;
            Email = details.Email;
            Password = details.Password;
        }

        // create a row in the table using the current values
        public int CreateFromCurrent(SqlTransaction trans)
        {
            CustomerData dao = new CustomerData(ConnStr, trans);
            Id = dao.Create(Name, Email, Password);
            return Id;
        }

        // update database row using the current values
        public void UpdateFromCurrent(SqlTransaction trans)
        {
            CustomerData dao = new CustomerData(ConnStr, trans);
            dao.Update(Id, Name, Email, Password);
        }

        // delete row from database
        public void Delete(SqlTransaction trans, int id)
        {
            CustomerData dao = new CustomerData(ConnStr, trans);
            dao.Delete(id);
            ResetProperties();
        }

        // list all rows from table
        public DataSet List()
        {
            CustomerData dao = new CustomerData(ConnStr);
            return dao.List();
        }

        // get specific customer by email address
        public int GetCustomerIdForEmail(string emailAddress)
        {
            CustomerData dao = new CustomerData(ConnStr);
            return dao.GetCustomerIdForEmail(emailAddress);
        }
    }
}

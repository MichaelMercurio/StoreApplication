using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace StoreApplication.Data
{
    public class CustomerDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CustomerData : DataObject
    {
        public CustomerData(string newDsn) : base(newDsn)
        {
            
        }

        public CustomerData(string newDsn, SqlTransaction newTrans) : base(newDsn, newTrans)
        {
            
        }

        // return only the record with the specified table ID
        public CustomerDetails RetrieveDetails(int id)
        {
            // create blank details object
            CustomerDetails details = new CustomerDetails();

            // populate the primary key
            details.Id = id;

            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("RetrieveCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            // add output parameters
            SqlParameter name = cmd.Parameters.Add("@name", SqlDbType.VarChar, 255);
            name.Direction = ParameterDirection.Output;
            SqlParameter email = cmd.Parameters.Add("@email", SqlDbType.VarChar, 255);
            email.Direction = ParameterDirection.Output;
            SqlParameter password = cmd.Parameters.Add("@password", SqlDbType.VarChar, 100);
            password.Direction = ParameterDirection.Output;

            // execute the proc
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // results are in output variables
                details.Name = Convert.IsDBNull(name.Value) ?
                    "" : name.Value.ToString();
                details.Email = Convert.IsDBNull(email.Value) ?
                    "" : email.Value.ToString();
                details.Password = Convert.IsDBNull(password.Value) ?
                    "" : password.Value.ToString();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            finally
            {
                conn.Close();
            }

            return details;
        }

        // create a new row in table and return id
        public int Create(string name, string email, string password)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("CreateCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 255).Value = name;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 255).Value = email;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = password;

            // add output parameters
            SqlParameter id = cmd.Parameters.Add("@id", SqlDbType.Int, 4);
            id.Direction = ParameterDirection.Output;

            // execute the proc
            try
            {
                if (Trans == null)
                {
                    conn.Open();
                }

                cmd.ExecuteNonQuery();

                // results are in output variables
                return Convert.ToInt32(id.Value);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            finally
            {
                if (Trans == null)
                {
                    conn.Close();
                }
            }
        }

        // update row in the table
        public void Update(int id, string name, string email, string password)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("UpdateCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 255).Value = name;
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 255).Value = email;
            cmd.Parameters.Add("@password", SqlDbType.VarChar, 100).Value = password;

            // execute the proc
            try
            {
                if (Trans == null)
                {
                    conn.Open();
                }

                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            finally
            {
                if (Trans == null)
                {
                    conn.Close();
                }
            }
        }

        // delete row from table
        public void Delete(int id)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("DeleteCustomer", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            // execute the proc
            try
            {
                if (Trans == null)
                {
                    conn.Open();
                }

                cmd.ExecuteNonQuery();
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            finally
            {
                if (Trans == null)
                {
                    conn.Close();
                }
            }
        }

        // ****************************************************
        // Parameterized Queries
        // ****************************************************

        // list all rows in table
        public DataSet List()
        {
            // define the sql command
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT id");
            sb.AppendLine("      ,name");
            sb.AppendLine("      ,email");
            sb.AppendLine("      ,password");
            sb.AppendLine("FROM Customer");

            // define command
            SqlConnection conn = new SqlConnection(Dsn);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);

            // fill dataset and return
            SqlDataAdapter sa = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            try
            {
                sa.Fill(ds);
            }
            catch (SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }

            return ds;
        }

        // get specific customer by email address
        public int GetCustomerIdForEmail(string emailAddress)
        {
            // define the sql command
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT id");
            sb.AppendLine("FROM Customer");
            sb.AppendLine("WHERE email = @email");

            // define command
            SqlConnection conn = new SqlConnection(Dsn);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);

            // add input parameters
            cmd.Parameters.Add("@email", SqlDbType.VarChar, 255).Value = emailAddress;

            // execute scalar and return
            try
            {
                conn.Open();

                object result = cmd.ExecuteScalar();

                return result == null ? -1 : Convert.ToInt32(result);
            }
            catch(SqlException sqlEx)
            {
                throw new Exception(sqlEx.Message, sqlEx);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

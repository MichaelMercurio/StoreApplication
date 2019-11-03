using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace StoreApplication.Data
{
    public class PurchaseDetails
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
    }

    public class PurchaseData : DataObject
    {
        public PurchaseData(string newDsn) : base(newDsn)
        {
            
        }

        public PurchaseData(string newDsn, SqlTransaction newTrans) : base(newDsn, newTrans)
        {
            
        }

        // return only the record with the specified table ID
        public PurchaseDetails RetrieveDetails(int id)
        {
            // create blank details object
            PurchaseDetails details = new PurchaseDetails();

            // populate the primary key(s)
            details.Id = id;

            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("RetrievePurchase", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            // add output parameters
            SqlParameter userid = cmd.Parameters.Add("@userid", SqlDbType.Int, 4);
            userid.Direction = ParameterDirection.Output;
            SqlParameter productid = cmd.Parameters.Add("@productid", SqlDbType.Int, 4);
            productid.Direction = ParameterDirection.Output;

            // execute the proc
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // results are in output variables
                details.UserId = Convert.IsDBNull(userid.Value) ?
                    -1 : Convert.ToInt32(userid.Value);
                details.ProductId = Convert.IsDBNull(productid.Value) ?
                    -1 : Convert.ToInt32(productid.Value);
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
        public int Create(int userid, int productid)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("CreatePurchase", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@userid", SqlDbType.Int, 4).Value = userid;
            cmd.Parameters.Add("@productid", SqlDbType.Int, 4).Value = productid;

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
        public void Update(int id, int userid, int productid)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("UpdatePurchase", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;
            cmd.Parameters.Add("@userid", SqlDbType.Int, 4).Value = userid;
            cmd.Parameters.Add("@productid", SqlDbType.Int, 4).Value = productid;

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
            SqlCommand cmd = new SqlCommand("DeletePurchase", conn);
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
            sb.AppendLine("      ,userid");
            sb.AppendLine("      ,productid");
            sb.AppendLine("FROM Purchase");

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

        // list rows for specific userid
        public DataSet ListForUserId(int userId)
        {
            // define the sql command
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT id");
            sb.AppendLine("      ,userid");
            sb.AppendLine("      ,productid");
            sb.AppendLine("FROM Purchase");
            sb.AppendLine("WHERE userid = @userid");
            sb.AppendLine("ORDER BY id DESC");

            // define command
            SqlConnection conn = new SqlConnection(Dsn);
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);

            // add input parameters
            cmd.Parameters.Add("@userid", SqlDbType.Int, 4).Value = userId;

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
    }
}

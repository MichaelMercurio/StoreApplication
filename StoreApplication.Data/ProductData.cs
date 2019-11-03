using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace StoreApplication.Data
{
    public class ProductDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public class ProductData : DataObject
    {
        public ProductData(string newDsn) : base(newDsn)
        {
            
        }

        public ProductData(string newDsn, SqlTransaction newTrans) : base(newDsn, newTrans)
        {
            
        }

        // return only the record with the specified table ID
        public ProductDetails RetrieveDetails(int id)
        {
            // create blank details object
            ProductDetails details = new ProductDetails();

            // populate the primary key(s)
            details.Id = id;

            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("RetrieveProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;

            // add output parameters
            SqlParameter name = cmd.Parameters.Add("@name", SqlDbType.VarChar, 255);
            name.Direction = ParameterDirection.Output;
            SqlParameter price = cmd.Parameters.Add("@price", SqlDbType.Decimal, 9);
            price.Direction = ParameterDirection.Output;
            price.Precision = 15;
            price.Scale = 2;

            // execute the proc
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();

                // results are in output variables
                details.Name = Convert.IsDBNull(name.Value) ?
                    "" : name.Value.ToString();
                details.Price = Convert.IsDBNull(price.Value) ?
                    0M : Convert.ToDecimal(price.Value);
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
        public int Create(string name, decimal price)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("CreateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 255).Value = name;
            cmd.Parameters.Add("@price", SqlDbType.Decimal, 9).Value = price;

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
        public void Update(int id, string name, decimal price)
        {
            // define stored procedure
            SqlConnection conn = (Trans == null) ? new SqlConnection(Dsn) : Trans.Connection;
            SqlCommand cmd = new SqlCommand("UpdateProduct", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (Trans != null)
            {
                cmd.Transaction = Trans;
            }

            // add input parameters
            cmd.Parameters.Add("@id", SqlDbType.Int, 4).Value = id;
            cmd.Parameters.Add("@name", SqlDbType.VarChar, 255).Value = name;
            cmd.Parameters.Add("@price", SqlDbType.Decimal, 9).Value = price;

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
            SqlCommand cmd = new SqlCommand("DeleteProduct", conn);
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
            sb.AppendLine("      ,price");
            sb.AppendLine("FROM Product");

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
    }
}

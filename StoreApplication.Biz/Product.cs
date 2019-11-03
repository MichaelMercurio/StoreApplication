using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using StoreApplication.Data;

namespace StoreApplication.Biz
{
    public class Product : BizObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // create blank object and set defaults
        public Product()
        {
            ResetProperties();
        }

        // create object and retrieve values from data layer
        public Product(int id)
        {
            ResetProperties();
            Id = id;

            LoadFromID();
        }

        // reset the properties
        private void ResetProperties()
        {
            Id = -1;
            Name = "";
            Price = 0M;
        }

        // retrieve values from database
        private void LoadFromID()
        {
            ProductData dao = new ProductData(ConnStr);
            ProductDetails details = dao.RetrieveDetails(Id);

            Name = details.Name;
            Price = details.Price;
        }

        // create a row in the table using the current values
        public int CreateFromCurrent(SqlTransaction trans)
        {
            ProductData dao = new ProductData(ConnStr, trans);
            Id = dao.Create(Name, Price);
            return Id;
        }

        // update database row using the current values
        public void UpdateFromCurrent(SqlTransaction trans)
        {
            ProductData dao = new ProductData(ConnStr, trans);
            dao.Update(Id, Name, Price);
        }

        // delete row from database
        public void Delete(SqlTransaction trans, int id)
        {
            ProductData dao = new ProductData(ConnStr, trans);
            dao.Delete(id);
            ResetProperties();
        }

        // list all rows from table
        public DataSet List()
        {
            ProductData dao = new ProductData(ConnStr);
            return dao.List();
        }

        // list all rows in a List object
        public List<Product> ListProducts()
        {
            List<Product> result = new List<Product>();

            DataSet ds = List();
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                result.Add(new Product
                {
                    Id = Convert.ToInt32(dr["id"]),
                    Name = dr["name"].ToString(),
                    Price = Convert.ToDecimal(dr["price"])
                });
            }

            return result;
        }
    }
}

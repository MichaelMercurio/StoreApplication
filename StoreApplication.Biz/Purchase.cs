using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

using StoreApplication.Data;

namespace StoreApplication.Biz
{
    public class Purchase : BizObject
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }

        // non-table values
        // helper property to return the name of the associated product
        public string ProductName
        {
            get
            {
                if (ProductId > -1)
                {
                    Product p = new Product(ProductId);
                    return p.Name;
                }
                else
                {
                    return "";
                }
            }
        }

        // create blank object and set defaults
        public Purchase()
        {
            ResetProperties();
        }

        // create object and retrieve values from data layer
        public Purchase(int id)
        {
            ResetProperties();
            Id = id;

            LoadFromID();
        }

        // reset the properties
        private void ResetProperties()
        {
            Id = -1;
            UserId = -1;
            ProductId = -1;
        }

        // retrieve values from database
        private void LoadFromID()
        {
            PurchaseData dao = new PurchaseData(ConnStr);
            PurchaseDetails details = dao.RetrieveDetails(Id);

            UserId = details.UserId;
            ProductId = details.ProductId;
        }

        // create a row in the table using the current values
        public int CreateFromCurrent(SqlTransaction trans)
        {
            PurchaseData dao = new PurchaseData(ConnStr, trans);
            Id = dao.Create(UserId, ProductId);
            return Id;
        }

        // update database row using the current values
        public void UpdateFromCurrent(SqlTransaction trans)
        {
            PurchaseData dao = new PurchaseData(ConnStr, trans);
            dao.Update(Id, UserId, ProductId);
        }

        // delete row from database
        public void Delete(SqlTransaction trans, int id)
        {
            PurchaseData dao = new PurchaseData(ConnStr, trans);
            dao.Delete(id);
            ResetProperties();
        }

        // list all rows from table
        public DataSet List()
        {
            PurchaseData dao = new PurchaseData(ConnStr);
            return dao.List();
        }

        // list rows for specific userid
        public List<Purchase> ListForUserId(int userId)
        {
            List<Purchase> result = new List<Purchase>();
            PurchaseData dao = new PurchaseData(ConnStr);
            DataSet ds = dao.ListForUserId(userId);

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                result.Add(new Purchase
                {
                    Id = Convert.ToInt32(dr["id"]),
                    UserId = Convert.ToInt32(dr["userid"]),
                    ProductId = Convert.ToInt32(dr["productid"])
                });
            }

            return result;
        }
    }
}

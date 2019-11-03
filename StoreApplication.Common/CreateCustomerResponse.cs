using System;
using System.Collections.Generic;
using System.Text;

namespace StoreApplication.Common
{
    // helper class for passing back the created customer from CustomersController
    public class CreateCustomerResponse
    {
        public string Token { get; set; }
        public string Id { get; set; }
    }
}

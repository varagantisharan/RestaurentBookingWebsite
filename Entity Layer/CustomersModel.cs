﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
    public class CustomersModel
    {
        public int customer_id { get; set; }
        public string first_name { get; set; }
        public string last_name{ get; set; }
        public string address { get; set; }
        public string password { get; set; }
        public string confirm_password { get; set; }
        public string phone_number { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public string userid { get; set; }

    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity_Layer
{
    public class BookingsModel
    {
        public int customer_id { get; set; }

        [DataType(DataType.Date)]
        public DateTime booking_date { get; set; }
        public string slot_Time { get; set; }
        public string Status { get; set; }
        public DateTime creation_time { get; set; }
    }
                        
       
}

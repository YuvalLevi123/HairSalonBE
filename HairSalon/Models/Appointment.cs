using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HairSalon.Models
{
    public class Appointment
    {
        private int customerId=50;
        private DateTime date;

        public DateTime Date
        {
            get => date;
            set => date = value;
        }

        public int CustomerId 
        {
            get => customerId;
            set => customerId = value;
        }
    }
}
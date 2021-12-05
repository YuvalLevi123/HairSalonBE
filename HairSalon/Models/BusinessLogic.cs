using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Npgsql;

namespace HairSalon.Models
{
    public class BusinessLogic
    {
        public static bool createAppointment (string appointmentInfo)
        {
            bool haveAvailableSlot;
            System.Diagnostics.Debug.WriteLine(appointmentInfo);
            Appointment appointmentToCheck = JsonConvert.DeserializeObject<Appointment>(appointmentInfo, new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy HH:mm:ss" });
            string date = appointmentToCheck.Date.ToString("yyyy-MM-dd HH:mm:ss");
            haveAvailableSlot = DataAccess.checkAvailableSlot(date);
            if (haveAvailableSlot)
            {
                DataAccess.saveAppointmentToDb(date, appointmentToCheck.CustomerId);
                return true;
            }

            return false;
        }

        public static List<Appointment> getAvaliablSlotsInDay(DateTime dayToCheck)
        {
            List<Appointment> AvailableSlots = new List<Appointment>();
            DateTime dateToAdd=dayToCheck.AddHours(10); // opening hours: 10:00 - 20:00
            Appointment app = new Appointment();
            app.Date = dateToAdd;

            AvailableSlots.Add(app);
            for (int i = 0; i < 10; i++)
            {
                DateTime dateToAddTemp= new DateTime();
                dateToAddTemp = dateToAdd.AddHours(i+1); 
                app = new Appointment();
                app.Date = dateToAddTemp;
                AvailableSlots.Add(app);
            }

            return AvailableSlots;
        }
        public static List<Appointment> getAppointmentsByDate(string date)
        {
            DateTime dayToCheck = DateTime.Parse(date);

            List<Appointment> notAvailableSlots =DataAccess.getAppointmentsByDateFromDb(date);
            List<Appointment> AllSlots= getAvaliablSlotsInDay(dayToCheck);
            List<Appointment> AvailableSlots = new List<Appointment>(AllSlots);
            foreach (Appointment currAppointment in AllSlots)
            {
                if (notAvailableSlots.Any(item => item.Date.Equals(currAppointment.Date)))
                {
                    AvailableSlots.Remove(currAppointment);
                }
            }

            return AvailableSlots;

        }


        public static List<Appointment> getCustomerAppointments(int customerId)
        {

          return DataAccess.getCustomerAppointmentsFromDb(customerId);
        }

   

    }
}
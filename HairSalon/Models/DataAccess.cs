using System;
using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using Npgsql;

namespace HairSalon.Models
{
    public class DataAccess
    {
        
        public static void saveAppointmentToDb(string date, int id)
        {
            string ConnectionString =
                "Server=tai.db.elephantsql.com;Port=5432;UserId=bmgvxcqr;Password=tQi114V55nnm-IvIwc6qyW_KyDMyCxYl;Database=bmgvxcqr;";
            string customerAppointments =
                "INSERT INTO Appointments(customerId, DateAndTime) VALUES (" + id + ",'" + date + "')";

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(customerAppointments, conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                }
            }
        }

        public static bool checkAvailableSlot(string date)
        {
            string ConnectionString =
                "Server=tai.db.elephantsql.com;Port=5432;UserId=bmgvxcqr;Password=tQi114V55nnm-IvIwc6qyW_KyDMyCxYl;Database=bmgvxcqr;";

            string checkAvailableSlotStr = "select dateandtime from appointments where (dateandtime='" + date + "')";

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(checkAvailableSlotStr, conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // if (reader.HasRows==true) not have available time slot
                        return false;
                    else
                        return true;
                }
            }

            //close
        }

        public static List<Appointment> getCustomerAppointmentsFromDb(int cutomerId)
        {
            List<Appointment> listOfCustomerAppointments = new List<Appointment>();
            string ConnectionString =
                "Server=tai.db.elephantsql.com;Port=5432;UserId=bmgvxcqr;Password=tQi114V55nnm-IvIwc6qyW_KyDMyCxYl;Database=bmgvxcqr;";

            string customerAppointments = "select dateAndTime from Appointments where customerId = " + cutomerId;
            System.Diagnostics.Debug.WriteLine(customerAppointments);

            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(customerAppointments, conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string dateStr = reader[0].ToString(); // read data from db
                        Appointment tempAppointmentForList = new Appointment();
                        tempAppointmentForList.Date = DateTime.Parse(dateStr);
                        tempAppointmentForList.CustomerId = cutomerId;
                        listOfCustomerAppointments.Add(tempAppointmentForList);
                    }

                    //close
                }
            }

            return listOfCustomerAppointments;
        }

        public static List<Appointment> getAppointmentsByDateFromDb(string date)
        {
            List<Appointment> listOfCustomerAppointments = new List<Appointment>();
            string ConnectionString =
                "Server=tai.db.elephantsql.com;Port=5432;UserId=bmgvxcqr;Password=tQi114V55nnm-IvIwc6qyW_KyDMyCxYl;Database=bmgvxcqr;";

            DateTime tempDateForConnectionToDb = DateTime.Parse(date);
            string tempStrForConnectionToDb = tempDateForConnectionToDb.ToString("MM-dd-yyyy");

            string allAppointments = "select * from appointments where (dateandtime::date='" + tempStrForConnectionToDb + "')";
            using (NpgsqlConnection conn = new NpgsqlConnection(ConnectionString))
            {
                conn.Open();
                using (NpgsqlCommand command = new NpgsqlCommand(allAppointments, conn))
                {
                    NpgsqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        string dateStr = reader[0].ToString(); // read data from db
                        Appointment tempAppointment = new Appointment();
                        tempAppointment.CustomerId = int.Parse(reader[0].ToString());
                        tempAppointment.Date = DateTime.Parse(reader[1].ToString());
                        listOfCustomerAppointments.Add(tempAppointment);

                    }
                }
            }
            return listOfCustomerAppointments;
        }
    }
}
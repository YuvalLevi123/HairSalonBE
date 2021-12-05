using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using HairSalon.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Npgsql.Replication;

namespace HairSalon.Controllers
{
    public class AppointmentsController : ApiController
    {
        [Route("api/Appointments/GetAvailableSlots/{date}")]
        [HttpGet]
        // GET: api/Appointments
        public string Get(string date)
        {
            List<Appointment> availableAppointments= BusinessLogic.getAppointmentsByDate(date);
            string jsonAvailableAppointments = JsonConvert.SerializeObject(availableAppointments, Formatting.Indented);
            return jsonAvailableAppointments;
        }

        [Route("api/Appointments/GetCustomerAppointments/{id:int}")]
        [HttpGet]
        // GET: api/Appointments/5
        public string Get(int id)
        {
            List<Appointment> customerAppointment = BusinessLogic.getCustomerAppointments(id);
            string jsonCustomerAppointments = JsonConvert.SerializeObject(customerAppointment, Formatting.Indented);
            return jsonCustomerAppointments;
        }

        [Route("api/Appointments/Create/")]
        [HttpPost]
        // POST: api/Appointments
        public string Post(HttpRequestMessage data)
        {
            string appointmentInfo =data.Content.ReadAsStringAsync().Result;
            if (BusinessLogic.createAppointment(appointmentInfo))
            {
                return "Successfully booked!";
            }
            else
            {
                return "Appointment is already booked";
            }
        }

        // PUT: api/Appointments/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Appointments/5
        public void Delete(int id)
        {
        }
    }
}

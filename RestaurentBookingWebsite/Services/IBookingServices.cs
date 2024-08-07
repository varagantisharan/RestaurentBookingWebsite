﻿using Entity_Layer;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Ocsp;
using RestaurentBookingWebsite.DbModels;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace RestaurentBookingWebsite.Services

{                                                                              
    public class IBookingServices : IBooking
    {
        private RestaurantContext db;
        private readonly IMail mailService;

        public IBookingServices(RestaurantContext db, IMail mailService)
        {
            this.db = db;
            this.mailService = mailService; 
        }
        public BookingsModel  Register(BookingsModel model)
        { 
            Booking MyBooking = new Booking();
           
            MyBooking.CustomerId= model.customer_id;
           

            string formattedDate = model.booking_date.ToShortDateString() + " " + model.slot_Time;

            MyBooking.BookingDate = Convert.ToDateTime(formattedDate);

            if(model.slot_Time == "09:00:00")
            {
                MyBooking.SlotTime = 1;
            }
            else if (model.slot_Time == "13:00:00")
            {
                MyBooking.SlotTime = 2;
            }
            else if (model.slot_Time == "17:00:00")
            {
                MyBooking.SlotTime = 3;
            }
            else
            {
                MyBooking.SlotTime = 4;
            }

            //MyBooking.SlotTime = model.slot_Time;

            BookingsModel user = new BookingsModel();
            try
            {
                db.Bookings.Add(MyBooking);
                db.SaveChanges();              

                return user;
               
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    throw new Exception(e.InnerException.Message);
                }
                throw new Exception(e.Message);
            }


        }
        public Customer GetCustomerDetails(int id)
        {
            var IsValidCustomer = db.Customers.Find(id);
            if(IsValidCustomer!=null)
            {
                return IsValidCustomer;
            }
            else
            {
                throw new Exception("Customer not Found");
            }
        }
        public List<Admin> GetAllAdminDetails()
        {

            List<Admin> GetAdmins = new List<Admin>();
            
            GetAdmins = db.Admins.ToList();

            if(GetAdmins!=null)
            {
                return GetAdmins;
            }
            else
            {
                throw new Exception("No records found in Admin Database");
            }
        }
        public Booking GetBookingDetails(int custId)
        {
            var bookingDetails = db.Bookings.OrderByDescending(p => p.BookingId).FirstOrDefault();
            if(bookingDetails!=null)
            {
                return bookingDetails;
            }
            else
            { 
                throw new Exception("No booking details found for this customer");
            }
        }

        public List<Booking> GetCustomerBookingDetails(int custId)
        {
            var bookingDetails = db.Bookings.OrderByDescending(p => p.BookingId).ToList();
            if (bookingDetails != null)
            {
                return bookingDetails;
            }
            else
            {
                throw new Exception("No booking details found for this customer");
            }
        }

        public int CancelBooking(int bookingId)
        {
            if(bookingId!=0)
            {
                var bookingDetails = db.Bookings.Find(bookingId);
                if(bookingDetails!=null)
                {
                    DateTime currentTime = DateTime.Now;
                    var hrs = bookingDetails.BookingDate.Subtract(currentTime);
                    TimeSpan time = new TimeSpan(24,0,0);
                    if (hrs > time)
                    {
                        bookingDetails.Status = "Cancelled";
                        db.Entry(bookingDetails).State = EntityState.Modified;
                        db.SaveChanges();

                        var customerdetails = db.Customers.Find(bookingDetails.CustomerId);
                        var adminDetails = GetAllAdminDetails();


                        if (customerdetails!=null)
                        {
                            MailRequest mail = new MailRequest();

                            string message = "Dear " + customerdetails.FirstName + " " + customerdetails.LastName + " .<br>" +
                            "Your booking has been Cancelled successfully.<br>" +
                             "Booking Id :" + bookingDetails.BookingId +
                            "<br>Slot Date and time :" + bookingDetails.BookingDate +
                            "<br>Thank You." +
                            "<br>Best regards," +
                            "<br>Sharan";
                            string subject = "Cancellation has been confirmed";

                            mail.Body = message;
                            mail.Subject = subject;
                            mail.ToEmail = customerdetails.Email;

                            mailService.SendEmail(mail);
                        }
                        if (adminDetails != null)
                        {
                            foreach(Admin admin in adminDetails)
                            {
                                MailRequest mail = new MailRequest();
                                string message = "Dear Admin " + admin.FirstName + " " + admin.LastName +
                                                 ",<br>A new Cancelletion has been confirmed by the Customer: " + customerdetails.FirstName + " " + customerdetails.LastName +
                                                 " with the booking Id: " + bookingDetails.BookingId +
                                                 "<br>Slot Date and time :" + bookingDetails.BookingDate +
                                                 "<br>Thank You.";

                                string subject = "Cancellation has been confirmed";

                                mail.Body = message;
                                mail.Subject = subject;
                                mail.ToEmail = admin.Email;

                                mailService.SendEmail(mail);
                            }
                        }

                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    throw new Exception("No booking details found.");
                }
                
            }
            else
            {
                throw new Exception("No booking details found with the given Booking Id.");
            }
        }
    }



}


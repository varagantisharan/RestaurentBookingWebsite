using DocumentFormat.OpenXml.Bibliography;
using Entity_Layer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurentBookingWebsite.DbModels;

namespace RestaurentBookingWebsite.Services
{
    public class AdminServices : IAdmin
    {
        private RestaurantContext db;
        public AdminServices(RestaurantContext db) 
        {
            this.db = db;
        }
        public List<Customer> CustRegisteredInSevenDays()
        {
            DateTime minsSevenDays = DateTime.Now.AddDays(-7);
            DateTime currentDay = DateTime.Now;

            var customers = db.Customers.Where(c => c.DateOfRegistration<=currentDay && c.DateOfRegistration>=minsSevenDays).ToList();
            if(customers!=null)
            {
                return customers;
            }
            else
            {
                throw new Exception("No details found");
            }
        }

        public List<Booking> UpcomingThreeDaysBookings()
        {
            DateTime nextthreedays = DateTime.Now.AddDays(3);
            DateTime currentDay = DateTime.Now;

            var bookings = db.Bookings.Where(b => b.BookingDate>= currentDay && b.BookingDate <= nextthreedays && b.Status=="Booked").ToList();
            if (bookings != null)
            {
                return bookings;
            }
            else
            {
                throw new Exception("No details found");
            }
        }

        public List<Booking> BookingsAsPerDateRange(DateTime from, DateTime to)
        {
            var bookings = db.Bookings.Where(b => b.BookingDate >= from && b.BookingDate <= to).ToList();
            if (bookings != null)
            {
                return bookings;
            }
            else
            {
                throw new Exception("No details found");
            }
        }

        public List<Booking> CancellationForNextThreedays()
        {
            DateTime nextthreedays = DateTime.Now.AddDays(3);
            DateTime currentDay = DateTime.Now;

            var Cancellations = db.Bookings.Where(b => b.BookingDate >= currentDay && b.BookingDate <= nextthreedays && b.Status == "Cancelled").ToList();
            if (Cancellations != null)
            {
                return Cancellations;
            }
            else
            {
                throw new Exception("No details found");
            }
        }

        public List<Customer> GetBookedCustomerDetails(List<Booking> bookingmodel)
        {
            return db.Customers.ToList();
        }

        public List<CheckIn> GetAllCheckIns()
        {
            return db.CheckIns.ToList();    
        }

        public int UpdateCheckInDetails(int BookingId)
        {
            try
            {
                if (BookingId > 0)
                {
                    var BookingExists = db.Bookings.Find(BookingId);
                    if (BookingExists != null)
                    {
                        var checkInExists = db.CheckIns.Where(c => c.BookingId == BookingId);
                        if (checkInExists == null)
                        {
                            CheckIn checkInModel = new CheckIn();
                            checkInModel.BookingId = BookingId;
                            db.CheckIns.Add(checkInModel);
                            db.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        throw new Exception("Booking Id does not exist in the checkin table");
                    }
                }
                else
                {
                    throw new Exception("Invalid Booking Id");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public int UpdateCheckOutDetails(int checkinId)
        {
            if (checkinId > 0)
            {
                var checkOutExists = db.CheckIns.Find(checkinId);
                if(checkOutExists != null)
                {
                    CheckIn checkInModel = new CheckIn();
                    checkInModel.CheckOutTime = DateTime.Now;
                    checkInModel.GrossAmount = 1000;

                    db.Entry(checkInModel).State = EntityState.Modified; ;
                    db.SaveChanges();

                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
    }
}

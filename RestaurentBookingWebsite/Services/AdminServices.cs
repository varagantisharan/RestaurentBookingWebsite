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


    }
}

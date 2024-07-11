using RestaurentBookingWebsite.DbModels;

namespace RestaurentBookingWebsite.Services
{
    public interface IAdmin
    {
        public List<Customer> CustRegisteredInSevenDays();
        public List<Booking> UpcomingThreeDaysBookings();

    }
}

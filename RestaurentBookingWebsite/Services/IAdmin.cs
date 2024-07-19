using Entity_Layer;
using RestaurentBookingWebsite.DbModels;

namespace RestaurentBookingWebsite.Services
{
    public interface IAdmin
    {
        public List<Customer> CustRegisteredInSevenDays();
        public List<Booking> UpcomingThreeDaysBookings();
        public List<Booking> CancellationForNextThreedays();
        public List<Booking> BookingsAsPerDateRange(DateTime from, DateTime to);
        public List<Customer> GetBookedCustomerDetails(List<Booking> bookingmodel);
        public List<CheckIn> GetAllCheckIns();
        public int UpdateCheckInDetails(int BookingId);
        public int UpdateCheckOutDetails(int checkinId);

    }
}

using  Entity_Layer;
using RestaurentBookingWebsite.DbModels;

namespace RestaurentBookingWebsite.Services
{
    public interface IBooking
    {
        public BookingsModel Register(BookingsModel model);
        public Customer GetCustomerDetails(int id);
        public List<Admin> GetAllAdminDetails();
        public Booking GetBookingDetails(int custId);
        
    }
}

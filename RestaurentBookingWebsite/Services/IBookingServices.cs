using Entity_Layer;
using RestaurentBookingWebsite.DbModels;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;



namespace RestaurentBookingWebsite.Services

{                                                                              
    public class IBookingServices : IBooking
    {
        private RestaurantContext db;

        public IBookingServices(RestaurantContext db)
        {
            this.db = db;
        }
        public BookingsModel Register(BookingsModel model)
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
    }



}


using Entity_Layer;
using Microsoft.AspNetCore.Mvc;
using RestaurentBookingWebsite.Services;

namespace RestaurentBookingWebsite.Controllers

{
    public class CustomerDashboardController : Controller
    {
        private ILoginService _loginService;
        private IBookingServices _bookingsServices;
        private readonly string senderEmail;
        private readonly string senderName;
      
        public CustomerDashboardController(ILoginService loginService,IBookingServices bookingServices, IConfiguration configuration)
        {
            _loginService = loginService;
            _bookingsServices = bookingServices;
            this.senderEmail = configuration["BrevoApi:SenderEmail"]!;
            this.senderName = configuration["BrevoApi:SenderName"]!;
        }

        public IActionResult CustDashboard(int id)
        {
            string Name = _loginService.GetUserName(id);

            TempData["CustId"] = id;
            TempData["UserName"] = Name;

            DateTime current_day = DateTime.Now;

            TempData["minDate"] = current_day.AddDays(1).ToString("yyyy-MM-dd");
            TempData["maxDate"] = current_day.AddDays(3).ToString("yyyy-MM-dd");

            //DateTime current_day = DateTime.Now.Date;
            //List<DateTime> nextdays = new List<DateTime>(); 

            //for(int i=0; i<3; i++)
            //{
            //    nextdays.Add(current_day.AddDays(1));
            //}

            //TempData["slots"] = nextdays;
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        public IActionResult Register(BookingsModel model)
        {
            model.customer_id = Convert.ToInt32(TempData["CustId"]);
            var res= _bookingsServices.Register(model);
            var adminDetails = _bookingsServices.GetAllAdminDetails();
            if (res != null)
            {
                var IsValidCustomer = _bookingsServices.GetCustomerDetails(model.customer_id);
                var IsValidBooking = _bookingsServices.GetBookingDetails(model.customer_id);

                if(IsValidCustomer!=null && IsValidBooking!=null) 
                {
                    string message = "Dear " + IsValidCustomer.FirstName + " " + IsValidCustomer.LastName + " .\n" +
                    "Your booking has been completed successfully.\n" +
                    "Booking Id :" + IsValidBooking.BookingId + "\n" +
                    "Slot Date and time :" + IsValidBooking.BookingDate + "\n" +
                    "Thank You.\n" +
                    "Best regards,\n" +
                    "Sharan";
                    string subject = "Booking has been confirmed";

                    EmailSender.SendEmail(senderName, senderEmail, IsValidCustomer.Email, IsValidCustomer.FirstName + IsValidCustomer.LastName, subject, message);
                }
                if(adminDetails!=null)
                {
                    foreach (var admin in adminDetails)
                    {
                        string message = "Dear Admin " + admin.FirstName + " " + admin.LastName +
                                         ", A new booking has been confirmed by the Customer: " + IsValidCustomer.FirstName + " " + IsValidCustomer.LastName +
                                         " with the booking Id: " + IsValidBooking.BookingId + "\n" +
                                         "Thank You.\n";
                                         
                        string subject = "Booking has been confirmed";

                        EmailSender.SendEmail(senderName, senderEmail, admin.Email, admin.FirstName + admin.LastName, subject, message);
                    }
                    
                }

                return RedirectToAction("CustDashboard", "CustomerDashboard", new { @id = model.customer_id });

            }
            else
            {
                throw new Exception("Signup is not successful");
            }

            
        }


    }
}

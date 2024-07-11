using Entity_Layer;
using Microsoft.AspNetCore.Mvc;
using RestaurentBookingWebsite.Services;

namespace RestaurentBookingWebsite.Controllers

{
    public class CustomerDashboardController : Controller
    {
        private ILoginService _loginService;
        private IBookingServices _bookingsServices;
        private readonly IMail mailService;

        public CustomerDashboardController(ILoginService loginService,IBookingServices bookingServices, IConfiguration configuration, IMail mailService)
        {
            _loginService = loginService;
            _bookingsServices = bookingServices;
            this.mailService = mailService;
        }

        public IActionResult CustDashboard(int id)
        {
            TempData["CustId"] = id;

            int custid = Convert.ToInt32(TempData["CustId"]);

            string Name = _loginService.GetUserName(id,"Customer");



            TempData["UserName"] = Name;

            DateTime current_day = DateTime.Now;

            TempData["minDate"] = current_day.ToString("yyyy-MM-dd");
            //TempData["minDate"] = current_day.AddDays(1).ToString("yyyy-MM-dd");
            TempData["maxDate"] = current_day.AddDays(2).ToString("yyyy-MM-dd");
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
            int custid = Convert.ToInt32(TempData["CustId"]);
            int cid = Convert.ToInt32(TempData["CustomerId"]);
            model.customer_id = cid ;
            var res= _bookingsServices.Register(model);
            var adminDetails = _bookingsServices.GetAllAdminDetails();
            if (res != null)
            {
                var IsValidCustomer = _bookingsServices.GetCustomerDetails(model.customer_id);
                var IsValidBooking = _bookingsServices.GetBookingDetails(model.customer_id);

                if(IsValidCustomer!=null && IsValidBooking!=null) 
                {
                    MailRequest mail = new MailRequest();

                    string message = "Dear " + IsValidCustomer.FirstName + " " + IsValidCustomer.LastName + " .<br>" +
                    "Your booking has been completed successfully.<br>" +
                    "Booking Id :" + IsValidBooking.BookingId +
                    "<br>Slot Date and time :" + IsValidBooking.BookingDate +
                    "<br>Thank You." +
                    "<br>Best regards," +
                    "<br>Sharan";
                    string subject = "Booking has been confirmed";

                    mail.Body = message;
                    mail.Subject = subject;
                    mail.ToEmail = IsValidCustomer.Email;

                    mailService.SendEmail(mail);
                }
                if(adminDetails!=null)
                {
                    foreach (var admin in adminDetails)
                    {
                        MailRequest mail = new MailRequest();
                        string message = "Dear Admin " + admin.FirstName + " " + admin.LastName +
                                         ",<br>A new booking has been confirmed by the Customer: " + IsValidCustomer.FirstName + " " + IsValidCustomer.LastName +
                                         " with the booking Id: " + IsValidBooking.BookingId +
                                         "<br>Thank You.";

                        string subject = "Booking has been confirmed";

                        mail.Body = message;
                        mail.Subject = subject;
                        mail.ToEmail = admin.Email;

                        mailService.SendEmail(mail);

                    }

                }

                return RedirectToAction("CustDashboard", "CustomerDashboard", new { @id = model.customer_id });

            }
            else
            {
                throw new Exception("Signup is not successful");
            }

            
        }

        [HttpGet]
        public IActionResult MyBookings(int id)
        {
            var bookingDetails = _bookingsServices.GetCustomerBookingDetails(Convert.ToInt32(TempData["CustId"]));
            if (bookingDetails != null)
            {

                return View(bookingDetails);
            }
            else
            {
                return View(null); 
            }
        }

        //[HttpDelete]

        //public IActionResult CancelBooking()
        //{
        //    return View();
        //}


        //[HttpPost]//, ActionName("CancelBooking")]
        public IActionResult CancelBooking(int id) 
        {
            int res=_bookingsServices.CancelBooking(id);

            if(res==1)
            {
                return RedirectToAction("MyBookings", "CustomerDashboard", new { @id = Convert.ToInt32(TempData["CustomerId"]) });
            }
            else
            {
                return RedirectToAction("MyBookings", "CustomerDashboard", new { @id = Convert.ToInt32(TempData["CustomerId"]) });
                throw new Exception("Cannot cancel booking");
            }
            

            
        }

    }
}

using Microsoft.AspNetCore.Mvc;
using RestaurentBookingWebsite.Services;

namespace RestaurentBookingWebsite.Controllers
{
    public class AdminDashboardController : Controller
    {
        private ILogin _loginService;
        private IAdmin _adminService;
        public AdminDashboardController(ILogin loginService, IAdmin adminService)
        {
            _loginService = loginService;
            _adminService = adminService;
        }
        public IActionResult AdmnDashboard(int id)
        {
            string Name = _loginService.GetUserName(id, "Admin");

            TempData["UserName"] = Name;
            ViewBag.Bookings = _adminService.UpcomingThreeDaysBookings();
            ViewBag.Customers = _adminService.CustRegisteredInSevenDays();

            return View();
        }

        //public IActionResult UpcomingBookings()
        //{
            
        //    return bookings;
        //}
    }
}

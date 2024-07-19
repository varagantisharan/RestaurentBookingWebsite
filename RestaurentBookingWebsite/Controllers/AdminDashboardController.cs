using Microsoft.AspNetCore.Mvc;
using RestaurentBookingWebsite.Services;
using System.IO;
using System.Data;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Http;
using RestaurentBookingWebsite.DbModels;
using Entity_Layer;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Diagnostics.CodeAnalysis;
using Humanizer;
using DocumentFormat.OpenXml.Bibliography;
using RestaurentBookingWebsite.Models;


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
            ViewBag.Cancellations = _adminService.CancellationForNextThreedays();
            return View();

        }

        [HttpPost]
        public ActionResult AdmnDashboard(DateTime From, DateTime To)
        {
            ViewBag.Bookings = _adminService.UpcomingThreeDaysBookings();
            ViewBag.Customers = _adminService.CustRegisteredInSevenDays();
            ViewBag.Cancellations = _adminService.CancellationForNextThreedays();
            var bookings = _adminService.BookingsAsPerDateRange(From,To);
            TempData["FromDate"] = From;
            TempData["ToDate"] = To;
            ViewBag.DateRangeBookings = bookings;
            return View(bookings);
        }

        public IActionResult ExportToExcel()
        {
            DateTime From = (DateTime)TempData["FromDate"];
            DateTime To = (DateTime)TempData["ToDate"];
            //var bookings = _adminService.BookingsAsPerDateRange(dateRange);
            var bookings = _adminService.BookingsAsPerDateRange(From, To);
            //Create an Instance of ExcelFileHandling
            ExcelFileHandling excelFileHandling = new ExcelFileHandling();
            //Call the CreateExcelFile method by passing the list of Employee
            var stream = excelFileHandling.CreateExcelFile(bookings);
            //Give a Name to your Excel File
            string excelName = $"Bookings.xlsx";
            // 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' is the MIME type for Excel files
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }


        public IActionResult CustomerBookingDetails()
        {
            
            List<Booking> bookings = _adminService.UpcomingThreeDaysBookings();
            List<Customer> customers = _adminService.GetBookedCustomerDetails(bookings);

            var customerBookings = from c in customers
                                   join b in bookings on c.CustomerId equals b.CustomerId into table1
                                   from b in table1.ToList()
                                   select new CustomerBookingModel
                                   {
                                       Customer = c,
                                       Booking = b,
                                   };
            return View(customerBookings);
        }

        [HttpPost]
        public IActionResult CustomerBookingDetails(String UserId)
        {

            List<Booking> bookings = _adminService.UpcomingThreeDaysBookings();
            List<Customer> customers = _adminService.GetBookedCustomerDetails(bookings).Where(c => c.UserId==UserId).ToList();
            List<CheckIn> checkins = _adminService.GetAllCheckIns();

            var customerBookings = from c in customers
                                   join b in bookings on c.CustomerId equals b.CustomerId into table1
                                   from b in table1
                                   join ch in checkins on b.BookingId equals ch.BookingId into table2
                                   from ch in table2.DefaultIfEmpty(). ToList()    
                                   select new CustomerBookingModel
                                   {
                                       Customer = c,
                                       Booking = b,
                                       CheckIn = ch,
                                   };
            return View(customerBookings);
        }

        public IActionResult CheckedIn(int id)
        {
            if(id>0)
            {
                int res=_adminService.UpdateCheckInDetails(id);
                 
            }
            return RedirectToAction("CustomerBookingDetails", "AdminDashboard");
        }

        public IActionResult CheckedOut(int id)
        {
            if (id > 0)
            {
                int res = _adminService.UpdateCheckOutDetails(id);

            }
            return RedirectToAction("CustomerBookingDetails", "AdminDashboard");
        }

        //var result = db.Clients.Select(x => new
        //{
        //    CustomerID = x.CustomerID,
        //    UserID = x.UserID,
        //    FullName = x.FullName,
        //    EmailAdd = string.IsNullOrEmpty(x.emailadd) ? "No Email" : x.emailadd,

        }
}

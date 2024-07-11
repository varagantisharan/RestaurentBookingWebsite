using Entity_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using RestaurentBookingWebsite.Services;
using RestaurentBookingWebsite.Controllers;
using MailKit;

namespace RestaurentBookingWebsite.Controllers
{
    public class LoginPageController : Controller
    {
        private ILogin _loginser;
        private readonly IMail mailService;
        public LoginPageController(ILogin loginser, IMail mailService)
        {
            _loginser = loginser;
            this.mailService = mailService;
        }
        public IActionResult SigninUser()
        {
            return View();
        }

        [HttpPost]


        public IActionResult SigninUser(SignInModel model)
        {
            try
            {
                var user = _loginser.SignIn(model);
                if (user!=null)
                {

                    if (user.Role == "Customer")
                    {
                        //TempData["AdmnId"] = user.admin_id;
                        TempData["CustomerId"] = user.admin_id;
                        return RedirectToAction("CustDashboard", "CustomerDashboard", new { @id = user.admin_id });
                        
                    }
                    else if(user.Role == "Admin")
                    {
                        TempData["AdminId"] = user.admin_id;
                        return RedirectToAction("AdmnDashboard","AdminDashboard", new { @id = user.admin_id });
                    }
                    else
                    {
                        return RedirectToAction("SigninUser");
                    }
                }
                else
                {
                    /*  ViewBag.Alert = AlertService.ShowAlert(Alerts.Danger, "Invalid Credentials");*/
                    ViewBag.Alert = "Invalid Credentials";
                    throw new Exception("Invalid user details");
                }

            }

            catch (Exception e)
            {

                return View();
            }
        }

        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(AdminsModel newuser)
        {
            SignInModel user=new SignInModel();
            try
            {
                if(newuser.password == newuser.confirm_password)
                {           
                    if (newuser.Role == "Admin")
                    {
                        user = _loginser.AdminSignUp(newuser);
                    }
                    else if (newuser.Role == "Customer")
                    {
                        var customer = new CustomersModel();
                        customer.userid = newuser.userid;
                        customer.first_name = newuser.first_name;
                        customer.last_name = newuser.last_name;
                        customer.address = newuser.address;
                        customer.password = newuser.password;
                        customer.phone_number = newuser.phone_number;
                        customer.email = newuser.email;
                        customer.confirm_password = newuser.confirm_password;
                        customer.role = newuser.Role;
                        user = _loginser.CustomerSignUp(customer);
                    }
                    if (user != null)
                    {
                        MailRequest mail = new MailRequest(); 
                        // send configuration mail
                        string receiverEmail = newuser.email;
                        string receiverName = newuser.first_name + " " + newuser.last_name;
                        string message = "Dear " + receiverName + ".<br>" +
                        "Here is your userId please login with this userId " + user.UserId + "." +
                        "<br>Thank You." +
                        "<br>Best Regards";
                        string subject = "Account has been created";
                        mail.Body = message;
                        mail.Subject = subject;
                        mail.ToEmail = newuser.email;

                        mailService.SendEmail(mail);

                        return RedirectToAction("SigninUser");

                    }
                    else
                    {
                        throw new Exception("Signup is not successful");
                    }
                }
                else
                {
                    throw new Exception("Password and Confirm Password must be same");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Signup is not successful");
            }
        }

        public IActionResult Signout()
        {
            return RedirectToAction("SigninUser");
        }

    }
}

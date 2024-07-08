using Entity_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using RestaurentBookingWebsite.Services;
using RestaurentBookingWebsite.Controllers;

namespace RestaurentBookingWebsite.Controllers
{
    public class LoginPageController : Controller
    {
        private ILogin _loginser;
        private readonly string senderEmail;
        private readonly string senderName;
        public LoginPageController(ILogin loginser, IConfiguration configuration)
        {
            _loginser = loginser;
            this.senderEmail = configuration["BrevoApi:SenderEmail"]!;
            this.senderName = configuration["BrevoApi:SenderName"]!;
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
                var role = _loginser.SignIn(model);
                if (role!=null)
                {

                    if (role == "Admin")
                    {
                        return RedirectToAction("SigninUser");
                    }
                    else
                    {
                        return RedirectToAction("CustDashboard", "CustomerDashboard", new { @id = model.UserId });
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
                        // send configuration mail
                        string receiverEmail = newuser.email;
                        string receiverName = newuser.first_name + " " + newuser.last_name;
                        string message = "Dear " + receiverName + ".\n" +
                        "Here is your userId please login with this userId " + user.UserId + ".\n" +
                        "Thank You.\n" +
                        "best regards";
                        string subject = "Account has been created";

                        EmailSender.SendEmail(senderName, senderEmail, receiverEmail, receiverName, subject, message);
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

    }
}

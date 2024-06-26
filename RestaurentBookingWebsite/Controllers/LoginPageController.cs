using Entity_Layer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using DALayer;
using DALayer.Model;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace RestaurentBookingWebsite.Controllers
{
    public class LoginPageController : Controller
    {
        private ILogin _loginser;
        public LoginPageController(ILogin loginser)
        {
            _loginser = loginser;
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
                        return RedirectToAction("ShowCustById", "Emp", new { @id = model.UserId });
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
                if(newuser.Role == "Admin")
                {
                    user = _loginser.AdminSignUp(newuser);
                }
                else if(newuser.Role == "Customer")
                {
                    var customer = new CustomersModel();
                    customer.first_name = newuser.first_name;
                    customer.last_name = newuser.last_name;
                    customer.address = newuser.address;
                    customer.password = newuser.password;
                    customer.phone_number = newuser.phone_number;
                    customer.email = newuser.email;
                    user = _loginser.CustomerSignUp(customer);
                }
                if (user!=null)
                {
                    return RedirectToAction("SigninUser");
                }
                else
                {
                    throw new Exception("Signup is not successful");
                }
            }
            catch (Exception e)
            {
                throw new Exception("Signup is not successful");
            }
        }

        //public IActionResult CustomerSignup()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult CustomerSignup(CustomersModel newuser)
        //{
        //    try
        //    {
        //        var user = _loginser.CustomerSignUp(newuser);
        //        if (user != null)
        //        {
        //            return RedirectToAction("SigninUser");
        //        }
        //        else
        //        {
        //            throw new Exception("Signup is not successful");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        throw new Exception("Signup is not successful");
        //    }
        //}


    }
}

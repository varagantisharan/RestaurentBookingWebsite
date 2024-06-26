using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Identity;
using DALayer;
using Entity_Layer;
using Microsoft.EntityFrameworkCore;
using DALayer.Model;

namespace DALayer
{

    public class ILoginService : ILogin
    {
        private RestaurantContext db;
        public ILoginService(RestaurantContext db)
        {
            this.db = db;
        }
        public SignInModel AdminSignUp(AdminsModel register)
        {
            Admin newuser = new Admin();
            newuser.FirstName = register.first_name;
            newuser.LastName = register.last_name;
            newuser.Password = register.password;
            newuser.Email = register.email;
            newuser.Address = register.address;
            newuser.PhoneNumber = register.phone_number;


            SignInModel user = new SignInModel();
            try
            {
                if (register.password == register.confirm_password)
                {

                    db.Admins.Add(newuser);
                    db.SaveChanges();

                    user.UserId = register.admin_id;
                    user.Password = register.password;

                    return user;
                }
                else
                {
                    throw new Exception("Enter valid details");
                }

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
        public SignInModel CustomerSignUp(CustomersModel register)
        {
            Customer newuser = new Customer();
            newuser.FirstName = register.first_name;
            newuser.LastName = register.last_name;
            newuser.Address = register.address;
            newuser.Password = register.password;
            newuser.PhoneNumber = register.phone_number;
            newuser.Email = register.email;

            SignInModel user = new SignInModel();
            try
            {
                if (register.password == register.confirm_password)
                {

                    db.Customers.Add(newuser);
                    db.SaveChanges();

                    user.UserId = register.customer_id;
                    user.Password = register.password;

                    return user;
                }
                else
                {
                    throw new Exception("Enter valid details");
                }

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
        public String SignIn(SignInModel login)
        {
            SignInModel model = new SignInModel();
            try
            {
                var IsCustomer = db.Customers.Find(login.UserId);
                var IsAdmin = db.Admins.Find(login.UserId);
                if (IsCustomer != null)
                {
                    if ((IsCustomer.CustomerId == login.UserId) & (IsCustomer.Password == login.Password))
                    {
                        model.UserId = IsCustomer.CustomerId;
                        model.Password = IsCustomer.Password;
                        return "Customer";
                    }
                    else
                    {
                        throw new Exception("Invalid username or password");
                    }
                }
                else if (IsAdmin != null)
                {
                    if ((IsAdmin.AdminId == login.UserId) & (IsAdmin.Password == login.Password))
                    {
                        model.UserId = IsAdmin.AdminId;
                        model.Password = IsAdmin.Password;
                        return "Admin";
                    }
                    else
                    {
                        throw new Exception("Invalid username or password");
                    }

                }
                else
                {
                    throw new Exception("Enter valid details");
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}

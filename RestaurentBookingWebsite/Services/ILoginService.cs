using Entity_Layer;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using RestaurentBookingWebsite.DbModels;
using System.Text;

namespace RestaurentBookingWebsite.Services
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
            string Encypted_Password = Encryptdata(register.password);

            Admin newuser = new Admin();
            //newuser.AdminId = register.admin_id;
            newuser.FirstName = register.first_name;
            newuser.LastName = register.last_name;
            newuser.Password = Encypted_Password;
            newuser.Email = register.email;
            newuser.Address = register.address;
            newuser.PhoneNumber = register.phone_number;


            SignInModel user = new SignInModel();
            try
            {
                //if (register.password == register.confirm_password)
                //{

                    db.Admins.Add(newuser);
                    db.SaveChanges();

                    var CreatedUser = db.Admins.OrderBy(p => p.AdminId).Last();

                    user.UserId = CreatedUser.AdminId;
                    user.Password = register.password;

                    return user;
                //}
                //else
                //{
                //    throw new Exception("Enter valid details");
                //}

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
            string Encypted_Password = Encryptdata(register.password);

            Customer newuser = new Customer();
            newuser.FirstName = register.first_name;
            newuser.LastName = register.last_name;
            newuser.Address = register.address;
            newuser.Password = Encypted_Password;
            newuser.PhoneNumber = register.phone_number;
            newuser.Email = register.email;

            SignInModel user = new SignInModel();
            try
            {
                //if (register.password == register.confirm_password)
                //{

                    db.Customers.Add(newuser);
                    db.SaveChanges();

                    var CreatedUser = db.Customers.OrderBy(p => p.CustomerId).Last();

                    user.UserId = CreatedUser.CustomerId;
                    user.Password = register.password;

                    return user;
                //}
                //else
                //{
                //    throw new Exception("Enter valid details");
                //}

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
                    string Decrypted_Password = Decryptdata(IsCustomer.Password);
                    if ((IsCustomer.CustomerId == login.UserId) & (login.Password == Decrypted_Password))
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
                    string Decrypted_Password = Decryptdata(IsAdmin.Password);
                    if ((IsAdmin.AdminId == login.UserId) & (login.Password == Decrypted_Password))
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


        public string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            strmsg = Convert.ToBase64String(encode);
            return strmsg;
        }

        public string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return decryptpwd;
        }

        public string GetUserName(int id)
        {
            var IsCustomer = db.Customers.Find(id);
            var IsAdmin = db.Admins.Find(id);

            if (IsCustomer != null)
            {
                return IsCustomer.FirstName + " " + IsCustomer.LastName;
            }
            else if (IsAdmin != null)
            {
                return IsAdmin.FirstName + " " + IsAdmin.LastName;
            }
            else
            {
                throw new Exception("Enter valid details");
            }
        }
    }
}

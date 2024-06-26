using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity_Layer;
//using RestaurentBookingWebsite;

namespace DALayer
{
    public interface ILogin
    {
        public SignInModel AdminSignUp(AdminsModel register);
        public SignInModel CustomerSignUp(CustomersModel register);
        public String SignIn(SignInModel login);
        //public List<Admin> GetAdmins();
        //public List<Customer> GetCustomer();
    }
}

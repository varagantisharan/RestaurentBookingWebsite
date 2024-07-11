using Entity_Layer;
namespace RestaurentBookingWebsite.Services
{
    public interface IMail
    {
        public void SendEmail(MailRequest mailRequest);
        //public int SendWelcomeEmail(WelcomeRequest request);
    }
}

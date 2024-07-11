
using Entity_Layer;
using RestaurentBookingWebsite.DbModels;
using RestaurentBookingWebsite.Services;


var builder = WebApplication.CreateBuilder(args);


ConfigurationManager configuration = builder.Configuration;

builder.Services.Configure<MailSettings>(configuration.GetSection("MailSettings"));


// Add services to the container.
builder.Services.AddTransient<IMail, RestaurentBookingWebsite.Services.MailServices>();
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddTransient(typeof(RestaurantContext));
builder.Services.AddTransient(typeof(ILoginService));
builder.Services.AddTransient(typeof(ILogin), typeof(ILoginService));
builder.Services.AddTransient(typeof(IBookingServices));
builder.Services.AddTransient(typeof(IBooking), typeof(IBookingServices));
builder.Services.AddTransient(typeof(MailServices));
builder.Services.AddTransient(typeof(IMail), typeof(MailServices));
builder.Services.AddTransient(typeof(AdminServices));
builder.Services.AddTransient(typeof(IAdmin), typeof(AdminServices));
builder.Services.AddSwaggerGen();

var app = builder.Build(); 

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSwagger();

app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=LoginPage}/{action=SigninUser}/{id?}");

app.Run();

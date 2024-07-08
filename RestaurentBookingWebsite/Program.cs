
using RestaurentBookingWebsite.DbModels;
using RestaurentBookingWebsite.Services;
using sib_api_v3_sdk.Client;

var builder = WebApplication.CreateBuilder(args);

Configuration.Default.ApiKey.Add("api-key", builder.Configuration["BrevoApi:ApiKey"]);


// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();
builder.Services.AddTransient(typeof(RestaurantContext));
builder.Services.AddTransient(typeof(ILoginService));
builder.Services.AddTransient(typeof(ILogin), typeof(ILoginService));
builder.Services.AddTransient(typeof(IBookingServices));
builder.Services.AddTransient(typeof(IBooking), typeof(IBookingServices));
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

using AspNetCore;
using UnquotedJsonExample.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;

services.AddControllersWithViews()
    .AddApplicationPart(typeof(LoggerController).Assembly)
    ;
UnquotedJsonDependencyInjection.AddUnquotedJson(services);

// In production, the js files will be served from this directory
services.AddSpaStaticFiles(configuration => {
    configuration.RootPath = "ClientApp/dist";
});

// Configure the HTTP request pipeline.
var app = builder.Build();
var env = app.Environment;

//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//}

//app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

//app.MapRazorPages();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    //app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//app.UseAuthorization();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.UseSpa(spa => {
    spa.Options.SourcePath = "ClientApp";

    if (env.IsDevelopment())
    {
        spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
    }
});

app.Run();

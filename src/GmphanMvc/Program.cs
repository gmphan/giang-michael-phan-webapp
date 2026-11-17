using Gmphan.BusinessAccessLib;
using GmphanMvc.Injectors;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// inject Serilog
builder.InjectSerilogSetup();

// Trust the X-Forwarded-* headers set by the Cloudflare Tunnel / proxy.
// Cloudflare terminates TLS at the edge and forwards to this origin over HTTP,
// so without this the app sees every request as "http" and UseHttpsRedirection()
// would loop. The tunnel connector reaches the app from the LAN, not loopback,
// so the default loopback-only restriction is cleared.
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;

    options.KnownNetworks.Clear();
    options.KnownProxies.Clear();
});

// inject Services
builder.Services.InjectServicesFromAssemblies(builder.Configuration);

var app = builder.Build();

// MUST be the first middleware, before HTTPS redirection / auth / routing
app.UseForwardedHeaders();

// seed a default admin user
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    try
    {
        // Resolve AdminSeederInjector and call SeedAdminUser
        var adminSeeder = serviceProvider.GetRequiredService<ISeedAdminIdentityServ>();
        await adminSeeder.SeedAdminUser(serviceProvider);
    }
    catch (Exception ex)
    {
        // Log errors (optional)
        Log.Error(ex, "An error occurred while seeding the admin user.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Visitor}/{controller=Home}/{action=Index}/{id?}");

app.Run();

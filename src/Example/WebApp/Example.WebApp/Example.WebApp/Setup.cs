using Example.ReadModel.Context;
using Example.WebApp.Client;
using Example.WebApp.Components;
using Example.WebApp.Components.Account;
using Example.WebApp.Data;
using Example.WebApp.Services;
using Example.WebApp.Shared.Services;
using Example.WebApp.WebApi;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Modular.Framework.Infrastructure.Mediator;
using Serilog;
using Serilog.Formatting.Compact;
using ILogger = Serilog.ILogger;

namespace Example.WebApp;

public static class Setup
{
    private static ILogger _logger;
    private static ILogger _webApiLogger { get; set; }
    private static ConnectionString _connectionString { get; set; }

    public static WebApplicationBuilder RegisterLogger(this WebApplicationBuilder builder)
    {
        _logger = new LoggerConfiguration()
                 .MinimumLevel.Error()
                 .Enrich.FromLogContext()
                 .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] [{Module}] [{Context}] {Message:lj}{NewLine}{Exception}")
                 .WriteTo.File(new CompactJsonFormatter(), "logs/logs")
                 .CreateLogger();

        _webApiLogger = _logger.ForContext("Module", "API");
        _webApiLogger.Information("Logger configured");

        builder.Host.UseSerilog(_webApiLogger);

        return builder;
    }

    public static WebApplicationBuilder RegisterConnectionString(this WebApplicationBuilder builder)
    {
        var config = builder.Configuration.AddEnvironmentVariables();
        _connectionString = ConnectionString.FromString(config.Build().GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found."));
        _webApiLogger.Information($"Connection string: {_connectionString}");
        builder.Services.AddSingleton(_connectionString);
        return builder;
    }

    public static WebApplicationBuilder RegisterMediator(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IMediator, Mediator>();
        builder.Services.AddSingleton<IMessageHandlerMap, MessageHandlerMap>();
        builder.Services.AddSingleton<IMessageHandlerResolver, MessageHandlerResolver>();
        return builder;
    }
    
    public static WebApplicationBuilder RegisterDefaultAspNetServices(this WebApplicationBuilder builder)
    {
        // Add services to the container.
        builder.Services.AddRazorComponents()
               .AddInteractiveServerComponents()
               .AddInteractiveWebAssemblyComponents();

        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, PersistingServerAuthenticationStateProvider>();

        builder.Services.AddAuthorization();
        builder.Services.AddAuthentication(options =>
                                           {
                                               options.DefaultScheme       = IdentityConstants.ApplicationScheme;
                                               options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                                           })
               .AddIdentityCookies();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
               .AddEntityFrameworkStores<ApplicationDbContext>()
               .AddSignInManager()
               .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddControllers(c => c.Filters.Add(typeof(AppContextFilter)));

        return builder;
    }

    public static WebApplicationBuilder RegisterApiService(this WebApplicationBuilder builder)
    {
        builder.Services.AddHttpClient("WebApi", (c, client) =>
                                                 {
                                                     var httpContextAccessor = c.GetRequiredService<IHttpContextAccessor>();
                                                     var httpContext = httpContextAccessor.HttpContext;
                                                     var baseAddress = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.PathBase}";
                                                     client.BaseAddress = new Uri(baseAddress);
                                                 })
               .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
                                                         {
                                                             // это для того чтобы когда предотвартить ситуацию, когда с пререндеринга натыкаемся на unathorized,
                                                             // он автоматически перевод на страницу логина и возвращается 200, хотя толжно быть 401
                                                             AllowAutoRedirect = false
                                                         });
        builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebApi"));
        builder.Services.AddTransient<IApiService, ApiService>();
        return builder;
    }

    public static WebApplicationBuilder RegisterServices(this WebApplicationBuilder builder)
    {
        builder.RegisterLogger();
        builder.RegisterConnectionString();
        builder.RegisterDefaultAspNetServices();
        builder.RegisterApiService();
        builder.RegisterMediator();

        builder.Services.AddReadDbContext(_connectionString.Value);
        builder.Services.AddHostedService<ModulesHostedService>(c => new ModulesHostedService(c.GetRequiredService<ConnectionString>(), _logger).Configure());
        builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_connectionString.Value));
        
        return builder;
    }

    public static WebApplication Configure(this WebApplication app)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
            app.UseMigrationsEndPoint(); //:: ??? 
        }
        else
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
           .AddInteractiveServerRenderMode()
           .AddInteractiveWebAssemblyRenderMode()
           .AddAdditionalAssemblies(typeof(IWASM).Assembly);

        // Add additional endpoints required by the Identity /Account Razor components.
        app.MapAdditionalIdentityEndpoints();

        app.MapControllers();

        return app;
    }
}
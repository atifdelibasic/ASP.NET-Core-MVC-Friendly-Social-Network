using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer.EntityModels;
using FriendlyRS1.Repository;
using FriendlyRS1.Repository.RepostorySetup;
using FriendlyRS1.Services;
using FriendlyRS1.SignalRChat;
using FriendlyRS1.SignalRChat.Hubs;
using FriendlyRS1.SignalRChat.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReflectionIT.Mvc.Paging;

namespace FriendlyRS1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
           {
                options.UseSqlServer(Configuration.GetConnectionString("PleskBaza"));
            });



            services.AddIdentity<ApplicationUser, IdentityRole<int>>
                (options =>
                {
                    options.SignIn.RequireConfirmedEmail = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;

                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);

                    options.Tokens.EmailConfirmationTokenProvider = "CustomEmailConfirmation";
                    options.Tokens.PasswordResetTokenProvider = "CustomPasswordReset";
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddTokenProvider<CustomEmailConfirmationTokenProvider<ApplicationUser>>("CustomEmailConfirmation")
                .AddTokenProvider<CustomPasswordTokenProvider<ApplicationUser>>("CustomPasswordReset")
                .AddPasswordValidator<CommonPasswordValidator<ApplicationUser>>();

            services.Configure<EmailConfirmationTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromDays(3));
            services.Configure<PasswordResetTokenProviderOptions>(o => o.TokenLifespan = TimeSpan.FromHours(3));

            //services.Configure

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "MyCookie";
                config.LoginPath = "/Account/Login";
                config.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });

            services.AddTransient<IEmailSender, SendGridEmailSender>();
            services.AddSingleton<IUserConnectionManager, UserConnectionManager>();
            services.Configure<EmailSenderOptions>(Configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();


            services.AddMemoryCache();

            services.AddAutoMapper(typeof(Startup));

            // add toastnotify
            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation()
                .AddNToastNotifyNoty(new NToastNotify.NotyOptions()
                {
                    ProgressBar = true,
                    Timeout = 5000,
                    Theme = "sunset"
                });


            services.AddRazorPages();
            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseNToastNotify();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<ChatHub>("/chathub");
                //endpoints.MapHub<NotificationHub>("/NotificationHub");
                endpoints.MapHub<NotificationHubUser>("/NotificationUserHub");
            });
        }
    }
}

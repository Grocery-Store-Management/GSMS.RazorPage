using CorePush.Apple;
using CorePush.Google;
using GSMS.API.PRM;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace GsmsRazor
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
            services.AddTransient<INotificationService, NotificationService>();
            services.AddHttpClient<FcmSender>();
            services.AddHttpClient<ApnSender>();
            var appSettingsSection = Configuration.GetSection("FcmNotification");
            services.Configure<FcmNotificationSetting>(appSettingsSection);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors();
            services.AddRepository(Configuration);
            services.AddRazorPages();
            services.AddSignalR();
            services.AddAntiforgery(o => o.HeaderName = "XSRF-TOKEN");
            //session
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromHours(1);
            });

            services.AddMemoryCache();

            services.AddAuthorization(options =>
                options.AddPolicy("ACTIVE", policy => policy.RequireClaim("ACTIVE", "True"))
            );

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(config =>
                {
                    config.Cookie.Name = "GSMSUserLoginCookie";
                    config.LoginPath = "/";
                    config.LogoutPath = "/Logout";
                    config.AccessDeniedPath = "/Error";
                    config.ExpireTimeSpan = TimeSpan.FromHours(1);
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseCors(options =>
            {
                options.WithOrigins("*").AllowAnyMethod();
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<SignalRHub>("/signalRHub");
            });
        }
    }
}

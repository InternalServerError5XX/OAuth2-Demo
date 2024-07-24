using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OAuth2_Demo.Data;

namespace OAuth2_Demo
{
    public static class Configurator
    {
        public static void ConfigureDb(this IServiceCollection services)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Data", "oauth2.db");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite($"Data Source={path}"));
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            });
        }

        public static void ConfigureAuth(this WebApplicationBuilder builder)
        {
            var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
            var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

            if (googleClientId == null || googleClientSecret == null)
                throw new NullReferenceException("Google auth options are null");

            var microsoftClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
            var microsoftClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];

            if (microsoftClientId == null || microsoftClientSecret == null)
                throw new NullReferenceException("Microsoft auth options are null");

            var facebookAppId = builder.Configuration["Authentication:Facebook:AppId"];
            var facebookAppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];

            if (facebookAppId == null || facebookAppSecret == null)
                throw new NullReferenceException("Facebook auth options are null");

            var twitterConsumerKey = builder.Configuration["Authentication:Twitter:ApiKey"];
            var twitterConsumerSecret = builder.Configuration["Authentication:Twitter:ApiSecretKey"];

            if (twitterConsumerKey == null || twitterConsumerSecret == null)
                throw new NullReferenceException("Twitter auth options are null");

            builder.Services.AddAuthentication()
                .AddGoogle(googleOptions =>
                {
                    googleOptions.ClientId = googleClientId;
                    googleOptions.ClientSecret = googleClientSecret;
                })
             .AddMicrosoftAccount(microsoftOptions =>
             {
                 microsoftOptions.ClientId = microsoftClientId;
                 microsoftOptions.ClientSecret = microsoftClientSecret;
             })
             .AddFacebook(facebookOptions =>
             {
                 facebookOptions.ClientId = facebookAppId;
                 facebookOptions.ClientSecret = facebookAppSecret;
             })
            .AddTwitter(twitterOptions =>
            {
                twitterOptions.ConsumerKey = twitterConsumerKey;
                twitterOptions.ConsumerSecret = twitterConsumerSecret;
                twitterOptions.RetrieveUserDetails = true;
            });
        }
    }
}

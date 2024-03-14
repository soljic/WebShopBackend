using API.Middleware;
using API.Services;
using API.SignalR;
using Application.Activity;
using Domain;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Application.Behaviors;
using Application.Infrastructure.CachingService;
using Application.Infrastructure.Photos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using  Application.Infrastructure.Email;
using Application.Interfaces;
using Application.Movies;
using AutoMapper;
using Infrastructure.Security;
using Application.Infrastructure.Photos;
using FluentValidation;
using StackExchange.Redis;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Newtonsoft.Json;

namespace API
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
          services.AddControllers(opt => 
            {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            }).AddNewtonsoftJson(options=>
              options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
          
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();

            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(connectionString)
            .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)

 );
            services.AddSingleton<MovieService>();
            services.AddSingleton<MovieHttpClient>();
            services.AddHttpClient();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DateNightApp", Version = "v1" });
            });

            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy", policy  =>
                {
                    policy.AllowAnyHeader().AllowCredentials().WithExposedHeaders("www-authenticate", "Pagination").AllowAnyMethod().WithOrigins("http://localhost:3000","http://localhost:5000");
                });
            });

            services.AddMemoryCache();
            services.AddSingleton<ICachedService, CacheService>();
            
            services.AddMediatR(cfg => 
            {
                cfg.RegisterServicesFromAssemblyContaining<Program>();
                cfg.RegisterServicesFromAssembly(typeof(Create).Assembly);
                cfg.AddOpenBehavior(typeof(Application.Behaviors.QueryCachingPipelineBehavior<,>));

            });
              //services.AddAutoMapper(typeof(Application.Core.MappingProfiles).Assembly);
              services.AddIdentityCore<AppUser>(opt => {
                  opt.Password.RequireNonAlphanumeric = false;
                  opt.SignIn.RequireConfirmedEmail = true;
              })
              .AddEntityFrameworkStores<DataContext>()
              .AddSignInManager<SignInManager<AppUser>>()
              .AddDefaultTokenProviders();
              var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["TokenKey"]));
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme; 
                })
                .AddCookie()
                .AddGoogle(options =>
                {
                    var googleConfig = Configuration.GetSection("Google");
                    options.ClientId = googleConfig["ClientId"];
                    options.ClientSecret = googleConfig["ClientSecret"];
                })
                .AddJwtBearer(opt => 
                {
                    // Postavke za JwtBearer autentikaciju
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                    opt.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chat") || !string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/notification"))))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            services.AddAuthorization(opt =>
            {
                opt.AddPolicy("IsActivityHost", policy =>
                {
                    policy.Requirements.Add(new IsHostRequirement());
                });
            });
            services.AddSingleton<IConnectionMultiplexer>(c => 
            {
                var configuration = ConfigurationOptions.Parse(Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
           
            //services.Configure<MediatRServiceConfiguration>(cfg => {
            //    // Ovdje mo�ete postaviti razne postavke za MediatR
            //    cfg.Lifetime = ServiceLifetime.Scoped;
            //    cfg.RegisterServicesFromAssemblyContaining<List.Handler>();
            //});


            services.AddAutoMapper(typeof(API.Helpers.MappingProfiles).Assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<Create>();
            services.AddHttpContextAccessor();
            services.AddScoped<EmailSender>();
            services.AddScoped<TokenService>();
            services.AddScoped<IUserAccessor, UserAccessor>();
            services.AddScoped<IPhotoAccessor, PhotoAccessor>();
            services.Configure<CloudinarySettings>(Configuration.GetSection("Cloudinary"));
            services.AddSignalR();
            // services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();
            // app.UseHttpsRedirection();
            // app.UseMvc(); 
            if (env.IsDevelopment()) 
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();

            app.UseCors("CorsPolicy");


            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("v1/swagger.json", "DateNightApp");
            //});

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<NotificatonHub>("/notification");
                endpoints.MapHub<NotificationResponseHub>("/notification-response");
            });
             


           
        }
    }
}

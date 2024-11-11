using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OMA_DuendeIdentityServer.Entity;
using OMA_DuendeIdentityServer.Models;
using System.Reflection;

namespace OMA_DuendeIdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigureDatabaseConnection(builder);
            RegisterCoreServices(builder);
            ConfigureIdentityAndIdentityServer(builder);
            ConfigureAuthenticationAndAuthorization(builder);
            ConfigureSwagger(builder);

            var app = builder.Build();
            ConfigurePipeline(app);
            InitializeDbData(app);

            app.Run();
        }


        private static void ConfigureDatabaseConnection(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("IdentityServerDatabase")
                                ?? throw new InvalidOperationException("The connection string 'IdentityServerDatabase' was not found.");
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)));
        }


        private static void RegisterCoreServices(WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();
            builder.Services.AddControllersWithViews();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins("https://localhost:7123")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });
        }


        private static void ConfigureIdentityAndIdentityServer(WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("IdentityServerDatabase");
            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddIdentityServer(options =>
            {
                options.Authentication.CookieLifetime = TimeSpan.FromHours(8);
                options.Authentication.CookieSlidingExpiration = true;
            })
            .AddAspNetIdentity<User>()
            .AddOperationalStore(options =>
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
            .AddConfigurationStore(options =>
                options.ConfigureDbContext = db => db.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(migrationsAssembly)))
            .AddProfileService<ProfileService>();
        }


        private static void ConfigureAuthenticationAndAuthorization(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = builder.Configuration["IdentityServer:Authority"];
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "name"
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
                options.AddPolicy("HotlineUserOnly", policy => policy.RequireRole("Hotline-User"));
            });
        }

   
        private static void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "OMA User API",
                    Version = "v1",
                    Description = "This is the OMA User API, designed to manage user operations and roles",
                    
                    
                    
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter 'Bearer' followed by your JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                try
                {
                    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                    c.IncludeXmlComments(xmlPath);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Swagger XML comments file error: {ex.Message}");
                }
            });
        }

    
        private static void ConfigurePipeline(WebApplication app)
        {
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseCors("AllowSpecificOrigin");

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "OMA API v1");
                c.RoutePrefix = "swagger";
            });
#endif

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

#if DEBUG
            app.Use(async (context, next) =>
            {
                var token = context.Request.Headers["Authorization"];
                Console.WriteLine($"Authorization Header: {token} time: {DateTime.Now}");
                await next();
            });
#endif

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }


        private static void InitializeDbData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
            MigrateDatabases(serviceScope);
            SeedIdentityServerData(serviceScope);
        }

      
        private static void MigrateDatabases(IServiceScope serviceScope)
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();
            serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.Migrate();
        }

    
        private static void SeedIdentityServerData(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            if (!context.Clients.Any())
            {
                foreach (var client in Clients.Get())
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Resources.GetIdentityResources())
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var scope in Resources.GetApiScopes())
                {
                    context.ApiScopes.Add(scope.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Resources.GetApiResources())
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}

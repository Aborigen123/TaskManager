using DataLayer;
using DataLayer.Repositories.Implementations;
using DataLayer.Repositories.Interfaces;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.MemoryStorage;
using InventorSoftTestTask.Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Service.Implementations;
using Service.Interfaces;
using Service.ResponseImpl;
using System.Data;
using System.Reflection;
using System.Text;

namespace InventorSoftTestTask
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection service)
        {
            service.AddDbContext<DatabaseContext>(options =>
                options.UseInMemoryDatabase("TaskManager"));

            service.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Task Manager", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            service.AddControllers();
            InjectRepositories(service);
            InjectServices(service);

            service.AddScoped<IdentitySeed>();

            service.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"])),
                };
            });

            service.AddSingleton<IHangfireJobScheduler, HangfireJobScheduler>()
                .AddHangfireServer()
                .AddHangfire(c => c.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                    .UseMemoryStorage());
        }

        public void Configure(
           IApplicationBuilder app, IHangfireJobScheduler hangfireJobScheduler)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            hangfireJobScheduler.ScheduleRecurringJobs();
            app.UseHangfireServer();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions()
            {
                DisplayStorageConnectionString = false
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Manager");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        public void InjectServices(IServiceCollection services)
        {
            services.AddScoped<MethodResultFactory>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ITaskService, TaskService>();
        }

        public void InjectRepositories(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IAssignmentHistoryRepository, AssignmentHistoryRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
        }
    }
}
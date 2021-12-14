using System.Text;
using Aplication.AppoinmentsApp;
using Aplication.ClinicHistoryApp;
using Aplication.ClinicHistoryApp.OralRadiographyApp;
using Aplication.ClinicHistoryApp.PatientEvolutionApp;
using Aplication.ClinicHistoryApp.TreamentPlanApp;
using Aplication.Interfaces;
using Aplication.Interfaces.Contracts;
using Aplication.OdontoApp;
using Aplication.Security;
using Aplication.Security.Forget_and_Reset;
using AutoMapper;
using Domine;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using persistence;
using Security.Token;
using WebApi.Middleware;

namespace WebApi
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
            services.AddCors(o => o.AddPolicy("corsApp", builder => {
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddDbContext<OntoSoftContext>(opt => {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddControllers( opt => {
                var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
            services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddControllers().AddFluentValidation(fv => {
                fv.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                fv.RegisterValidatorsFromAssemblyContaining<PostClinicHistory>();
                fv.RegisterValidatorsFromAssemblyContaining<putClinicHistory>();
                fv.RegisterValidatorsFromAssemblyContaining<UserRegister>();
                fv.RegisterValidatorsFromAssemblyContaining<UserPut>();
                fv.RegisterValidatorsFromAssemblyContaining<Login>();
                fv.RegisterValidatorsFromAssemblyContaining<PostOdontogram>();
                fv.RegisterValidatorsFromAssemblyContaining<putOdontogram>();
                fv.RegisterValidatorsFromAssemblyContaining<deleteOdontogram>();
                fv.RegisterValidatorsFromAssemblyContaining<addRol>();
                fv.RegisterValidatorsFromAssemblyContaining<addUsersRoles>();
                fv.RegisterValidatorsFromAssemblyContaining<deleteRol>();
                fv.RegisterValidatorsFromAssemblyContaining<ObtainRolUser>();
                fv.RegisterValidatorsFromAssemblyContaining<ObtainUserRol>();
                fv.RegisterValidatorsFromAssemblyContaining<PostAppoinment>();
                fv.RegisterValidatorsFromAssemblyContaining<putAppoinments>();
                fv.RegisterValidatorsFromAssemblyContaining<deleteAppoinments>();
                fv.RegisterValidatorsFromAssemblyContaining<PostOralRadiography>();
                fv.RegisterValidatorsFromAssemblyContaining<PostPatientEvolution>();
                fv.RegisterValidatorsFromAssemblyContaining<PostTreamentPlan>();
                fv.RegisterValidatorsFromAssemblyContaining<deleteUsersRoles>();
                fv.RegisterValidatorsFromAssemblyContaining<ResetPassword>();


            });
            
            var builder = services.AddIdentityCore<User>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddRoles<IdentityRole>();
            identityBuilder.AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<User, IdentityRole>>();
            identityBuilder.AddEntityFrameworkStores<OntoSoftContext>();
            identityBuilder.AddSignInManager<SignInManager<User>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();
            builder.AddDefaultTokenProviders();              
            //identityBuilder.AddSignInManager<SignInManager<User>>(); linea encargada de cargar la data al login del usuario de la bd local
            // services.TryAddSingleton<ISystemClock, SystemClock>(); Servicio para iniciar sesion y trae el sistema de reloj
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Clave ultra secreta OntoSoft"));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
                opt.TokenValidationParameters = new TokenValidationParameters{
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
            });
            services.AddScoped<IJwtGenerator, JwtGenerator>();
            services.AddAutoMapper(typeof(GetAppoinment.Manager));
            services.AddAutoMapper(typeof(getClinicHistoryWithUser.Manager));
            services.AddMediatR(typeof(UserRegister.Manager).Assembly);
            services.AddTransient<IMailService, SendGridMailService>();
            services.AddScoped<IUserSesion, UserSesion>();
            services.AddScoped<IForgetPassword, ForgetService>();
            services.AddScoped<IMailCreateAppoinment, createAppoinmentMail>();
            services.AddScoped<IMailEditAppoinment, editAppoinmentMail>();
            services.AddScoped<IMailDeleteAppoinment, SendMailDeleteAppoinment>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("corsApp");
            app.UseMiddleware<ManagerErrorMiddleware>();
            if (env.IsDevelopment())
            {
                // app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RestEase;
using TicketManagement.DataAccess.RepositoryInjection;
using TicketManagement.EventAPI.Dto;
using TicketManagement.EventAPI.ImportThirdPartyEvent;
using TicketManagement.EventAPI.Interfaces;
using TicketManagement.EventAPI.Services;
using TicketManagement.EventAPI.Settings;
using TicketManagement.EventAPI.Validations;

namespace TicketManagement.EventAPI
{
    /// <summary>
    /// Sets configuration for application.
    /// </summary>
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var tokenSettings = Configuration.GetSection(nameof(JwtTokenSettings));

            services.Configure<JwtTokenSettings>(tokenSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = tokenSettings[nameof(JwtTokenSettings.JwtIssuer)],
                        ValidateAudience = true,
                        ValidAudience = tokenSettings[nameof(JwtTokenSettings.JwtAudience)],
                        ValidateLifetime = false,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenSettings[nameof(JwtTokenSettings.JwtSecretKey)])),
                        ValidateIssuerSigningKey = true,
                    };
                });
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketManagement.EventAPI", Version = "v1" });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    Description = "Jwt Token is required to access the endpoints",
                    In = ParameterLocation.Header,
                    Name = "JWT Authentication",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme,
                    },
                };

                options.AddSecurityDefinition("Bearer", jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() },
                });
            });
            services.AddRepository(Configuration);

            services.AddScoped<IValidator<EventAreaDto>, EventAreaValidation>();
            services.AddScoped<IValidator<EventSeatDto>, EventSeatValidation>();
            services.AddScoped<IValidator<EventDto>, EventValidation>();
            services.AddScoped<IService<EventAreaDto>, EventAreaService>();
            services.AddScoped<IService<EventSeatDto>, EventSeatService>();
            services.AddScoped<IService<EventDto>, EventService>();
            services.AddScoped<IValidator<LayoutDto>, LayoutValidation>();
            services.AddScoped<IService<LayoutDto>, LayoutService>();
            services.AddScoped<IVenueService, VenueService>();
            services.AddScoped<IThirdPartyEventService, ThirdPartyEventService>();
            services.AddScoped<IThirdPartyEventRepository, ThirdPartyEventRepository>();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketManagement.EventAPI v1"));
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

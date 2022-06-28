using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnlineStore.Domain;
using OnlineStore.Middlewares;
using OnlineStore.Validators;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore
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
            services.AddScoped<ITestService, TestService>();

            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "OnlineStore", Version = "v1" });
                c.ResolveConflictingActions(c => c.Last()); // takes latest version of API if there's conflict due to same action names while API versioning
            });

            // POLICY BASED AUTH
            services.AddAuthorization(options =>
            {
                options.AddPolicy("AtLeast18", policy =>
                    policy.Requirements.Add(new AuthAgeRequirement(18)));
            });
            services.AddSingleton<IAuthorizationHandler, AuthAgeHandler>();

            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
            //});

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.SaveToken = true;
                var Key = Encoding.UTF8.GetBytes(Configuration["JWT:Key"]);
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["JWT:Issuer"],
                    ValidAudience = Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Key)
                };
            });

            services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();

            // FOR MEMORY CACHING
            services.AddMemoryCache();

            // RESPONSE CACHING
            //services.AddResponseCaching();

            services.AddApiVersioning(x =>
            {
                x.DefaultApiVersion = new ApiVersion(1, 0);
                x.AssumeDefaultVersionWhenUnspecified = true; //If we haven't set this flag to true and client hit the API without mentioning the version then UnsupportedApiVersion exception occurs.
                x.ReportApiVersions = true; //To return the API version in response header.
                x.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "OnlineStore v1"));
            }
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseRouting();

            // UseCors must be called before UseResponseCaching
            //app.UseCors();

            // RESPONSE CACHING
            //app.UseResponseCaching(options =>
            //{
            //    options.MaximumBodySize = 1024;
            //    options.UseCaseSensitivePaths = true;
            //});

            // RESPONSE CACHING MIDDLEWARE
            //app.Use(async (context, next) =>
            //{
            //    context.Response.GetTypedHeaders().CacheControl =
            //        new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            //        {
            //            Public = true,
            //            MaxAge = TimeSpan.FromSeconds(10)
            //        };
            //    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] =
            //        new string[] { "Accept-Encoding" };

            //    await next();
            //});
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseShortCircuitMiddleware();

            app.UseRequestCulture();

            //using (StreamReader iisUrlRewriteStreamReader =
            //File.OpenText("IISUrlRewrite.xml"))
            //{
            //    var options = new RewriteOptions()
            //        .AddRedirect("WeatherForecast/(.*)", "redirected/$1")
            //        .AddRewrite(@"^rewrite-rule/(\d+)/(\d+)", "rewritten?var1=$1&var2=$2",
            //            skipRemainingRules: true)
            //        //.AddApacheModRewrite(apacheModRewriteStreamReader)
            //        .AddIISUrlRewrite(iisUrlRewriteStreamReader)
            //        .Add(MethodRules.RedirectXmlFileRequests)
            //        .Add(MethodRules.RewriteTextFileRequests)
            //        .Add(new RedirectImageRequests(".png", "/png-images"))
            //        .Add(new RedirectImageRequests(".jpg", "/jpg-images"));

            //    app.UseRewriter(options);
            //}



            app.Use(async (context, next) =>
            {
                /// YOUR MIDDLEWAER
                await next.Invoke();
            });

            app.UseMiddleware<Test1Middleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMapWhen(IApplicationBuilder app)
        {
            app.MapWhen(context => {
                return context.Request.Query.ContainsKey("somekey");
            }, HandleBranch);
        }

        private static void HandleBranch(IApplicationBuilder app)
        {
            app.UseRequestCulture();
        }

    }
}

using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;
using System.Text;
using testeef;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace testef
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
            services.AddCors();

            services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("DataBaseInMemory"));
            //entender mais a fundo sobre
            services.AddScoped<DataContext, DataContext>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "testef", Version = "v1" });
            });

            #region JWT Configuration
            var key = Encoding.ASCII.GetBytes(Settings.Secret);
            services.AddAuthentication(x =>
            {	
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
                x.RequireHttpsMetadata= false;
                x.SaveToken= true;
                x.TokenValidationParameters= new TokenValidationParameters(){
                    ValidateIssuerSigningKey= true,
                    IssuerSigningKey= new SymmetricSecurityKey(key),
                    ValidateIssuer= false,
                    ValidateAudience= false
                };
            });
#endregion        
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "testef v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();
        
        #region JWT Configuration
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthentication();
            app.UseAuthorization();
        #endregion
            app.CreateMockDataEntityInMemory(context);
            // CreateMockDataInMemory(context);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        private void CreateMockDataInMemory(DataContext context){
            var categories= new List<Category>();
            var products= new List<Product>();
            for(int i= 1; i<3; i++) {
                var category= new Category(){
                   Title="Categoria " + i 
                };    
                var product= new Product(){
                    CategoryId= i,
                    Description= "DSC Produto "+ i,
                    Price= (decimal)10.2 * i,
                    Title= "Produto "+ i
                };

                products.Add(product);
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.Products.AddRange(products);
            context.SaveChanges();
        }               
    }
    public static class IApplicationBuilderExtension{
        public static void CreateMockDataEntityInMemory(this IApplicationBuilder IApplicationBuilder, DataContext context){
            var categories= new List<Category>();
            var products= new List<Product>();
            for(int i= 1; i<3; i++) {
                var category= new Category(){
                   Title="Categoria " + i 
                };    
                var product= new Product(){
                    CategoryId= i,
                    Description= "DSC Produto "+ i,
                    Price= (decimal)10.2 * i,
                    Title= "Produto "+ i
                };

                products.Add(product);
                categories.Add(category);
            }
            context.Categories.AddRange(categories);
            context.Products.AddRange(products);
            context.SaveChanges();
        }   
    }
}

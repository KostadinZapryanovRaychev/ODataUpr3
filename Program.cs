using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using OdataSolution.Data;
using OdataSolution.Models;
using OdataSolution.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddIdentity<IdentityUser, IdentityRole>()
         .AddEntityFrameworkStores<ApplicationDbContext>()
         .AddDefaultTokenProviders();

        var odataBuilder = new ODataConventionModelBuilder();
        odataBuilder.EntitySet<Student>("Student");

        builder.Services.AddControllers()
            .AddOData(opt => opt
                .AddRouteComponents("odata", odataBuilder.GetEdmModel())
                .Select()
                .Filter()
                .OrderBy()
                .Expand()
                .SetMaxTop(100));

        builder.Services.AddTransient<IStudentService, StudentService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication(); // <== Identity
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
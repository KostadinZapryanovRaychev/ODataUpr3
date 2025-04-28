using Microsoft.AspNetCore.OData;
using Microsoft.OData.ModelBuilder;
using OdataSolution.Models;
using OdataSolution.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var odataBuilder = new ODataConventionModelBuilder();
odataBuilder.EntitySet<Student>("Student");

builder.Services.AddControllers()
    .AddOData(opt => opt
        .AddRouteComponents("odata", odataBuilder.GetEdmModel())
        .Select()
        .Filter()
        .OrderBy()
        .Expand()
        .SetMaxTop(100)
    );
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

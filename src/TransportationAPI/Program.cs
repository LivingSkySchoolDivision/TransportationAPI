using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

#pragma warning disable ASP0016 // Do not return a value from RequestDelegate
app.MapGet("/", context =>
{
    context.Response.Redirect("./swagger/index.html", permanent: false);
    return Task.FromResult(0);
});
#pragma warning restore ASP0016 // Do not return a value from RequestDelegate

app.MapGet("/OpenWorkOrders", () =>
    {
        WorkOrderRepository WORepo = new WorkOrderRepository(builder.Configuration.GetConnectionString("FleetVision") ?? string.Empty);
        return WORepo.GetOpenWorkOrders().OrderBy(x => x.Created);
    })
    .WithName("GetOpenWorkOrders")
    .WithOpenApi();

app.MapGet("/BusInspections", () =>
    {
        BusInspectionRepository BIRepo = new BusInspectionRepository(builder.Configuration.GetConnectionString("VersaTrans") ?? string.Empty);
        return BIRepo.GetAllBusInspections().OrderBy(x => x.Expiration);
    })
    .WithName("GetBusInspections")
    .WithOpenApi();

app.MapGet("/BusInspections/{year}/{month}",(int year, int month) =>
    {
        BusInspectionRepository BIRepo = new BusInspectionRepository(builder.Configuration.GetConnectionString("VersaTrans") ?? string.Empty);

        if (
            (year > 0)
            && (year < DateTime.MaxValue.Year)
            && (month > 0)
            && (month <= 12)
            )
        {
            return BIRepo.GetAllBusInspections().Where(x => x.Expiration.Month == month && x.Expiration.Year == year).OrderByDescending(x => x.Expiration).ToList<BusInspection>();
        } else {
            return new List<BusInspection>();
        }
    })
    .WithName("GetBusInspectionsByMonth")
    .WithOpenApi();

app.MapGet("/BusInspections/overdue/{year}/{month}", (int year, int month) =>
    {
        BusInspectionRepository BIRepo = new BusInspectionRepository(builder.Configuration.GetConnectionString("VersaTrans") ?? string.Empty);

        if (
            (year > 0)
            && (year < DateTime.MaxValue.Year)
            && (month > 0)
            && (month <= 12)
            )
        {
            DateTime comparisonDate = new DateTime(year, month, 1);
            return BIRepo.GetAllBusInspections().Where(x => x.Expiration < comparisonDate).OrderBy(x => x.Expiration).ToList<BusInspection>();
        } else {
            return new List<BusInspection>();
        }
    })
    .WithName("GetOverdueBusInspections")
    .WithOpenApi();

app.Run();
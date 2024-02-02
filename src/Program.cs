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

app.Run();

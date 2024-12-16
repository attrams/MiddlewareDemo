var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseWhen(context => context.Request.Query.ContainsKey("branch"), app =>
{
    app.Use(async (context, next) =>
    {
        var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("From UseWhen(): Branch used = {Branch}", context.Request.Query["branch"].FirstOrDefault() ?? "");

        await next();
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync("Hello world!");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

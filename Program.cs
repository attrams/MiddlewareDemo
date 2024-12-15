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

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Request Host: {Host}", context.Request.Host);
    logger.LogInformation("My Middleware - Before");

    await next(context);

    logger.LogInformation("My Middleware - After");
    logger.LogInformation("Response StatusCode: {StatusCode}", context.Response.StatusCode);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

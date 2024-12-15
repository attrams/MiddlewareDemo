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
    logger.LogInformation("ClientName HttpHeader in Middleware 1: {ClientName}", context.Request.Headers["ClientName"].FirstOrDefault() ?? "");
    logger.LogInformation("Add a ClientName HttpHeader in Middleware 1");

    context.Request.Headers.TryAdd("ClientName", "Ubuntu");

    logger.LogInformation("My Middleware 1 - Before");

    await next(context);

    logger.LogInformation("My Middleware 1 - After");
    logger.LogInformation("Response StatusCode in Middleware 1: {StatusCode}", context.Response.StatusCode);
});

app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("ClientName HttpHeader in Middleware 2: {ClientName}", context.Request.Headers["ClientName"].FirstOrDefault());

    logger.LogInformation("My Middleware 2 - Before");

    context.Response.StatusCode = StatusCodes.Status202Accepted;
    await next(context);

    logger.LogInformation("My Middleware 2 - After");
    logger.LogInformation("Response StatusCode in Middleware 2: {StatusCode}", context.Response.StatusCode);
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

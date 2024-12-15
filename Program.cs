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

app.Map("/lottery", app =>
{
    var random = new Random();
    var luckyNumber = random.Next(1, 6);

    app.UseWhen(context => context.Request.QueryString.Value == $"?{luckyNumber}", app =>
    {
        app.Run(async context =>
        {
            await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
        });
    });

    app.UseWhen(context => string.IsNullOrWhiteSpace(context.Request.QueryString.Value), app =>
    {
        app.Use(async (context, next) =>
        {
            var number = random.Next(1, 6);
            context.Request.Headers.TryAdd("number", number.ToString());

            await next(context);
        });

        app.UseWhen(context => context.Request.Headers["number"] == luckyNumber.ToString(), app =>
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"You win! You got the lucky number {luckyNumber}!");
            });
        });
    });

    app.Run(async context =>
    {
        var number = "";

        if (context.Request.QueryString.HasValue)
        {
            number = context.Request.QueryString.Value?.Replace("?", "");
        }
        else
        {
            number = context.Request.Headers["number"];
        }

        await context.Response.WriteAsync($"Your number is {number}. Try again!");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"Use the /lottery URL to play. You can choose your number with the format /lottery?1.");

});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

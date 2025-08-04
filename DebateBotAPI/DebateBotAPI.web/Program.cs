using DebateBotAPI.web.Services;

var builder = WebApplication.CreateBuilder(args);
var apiKey = builder.Configuration["OpenAI:ApiKey"];
builder.Services.AddSingleton(new DebateService(apiKey));


// Add services to the container.

builder.Services.AddControllers();
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

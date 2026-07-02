using Delhivery.Data.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Read Connection String
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Register ShipmentRepository
builder.Services.AddSingleton(new ShipmentRepository(connectionString));

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors("AllowAll");
app.UseAuthorization();



app.UseDefaultFiles();
app.UseStaticFiles();


app.MapControllers();

app.Run();
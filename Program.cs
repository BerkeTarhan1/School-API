using SchoolAPI.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
	c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
	{
		Title = "School API",
		Version = "v1",
		Description = "API for managing school students"
	});
});

// Configure MongoDB settings
var mongoDBSettings = builder.Configuration.GetSection("MongoDB");
builder.Services.AddSingleton<StudentService>(provider =>
	new StudentService(new SchoolAPI.Models.MongoDBSettings
	{
		ConnectionString = mongoDBSettings["ConnectionString"] ?? "mongodb://localhost:27017",
		DatabaseName = mongoDBSettings["DatabaseName"] ?? "SchoolDB",
		CollectionName = mongoDBSettings["CollectionName"] ?? "Students"
	}));

// Add CORS for frontend applications
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

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "School API v1");
	});
}

app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
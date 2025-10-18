using ClipNchic.Business.Services;
using ClipNchic.DataAccess.Data;
using ClipNchic.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text.Json.Serialization;
using CloudinaryDotNet;

var builder = WebApplication.CreateBuilder(args);

// Configure Cloudinary
var cloudinarySettings = builder.Configuration.GetSection("Cloudinary");
var cloudinary = new Cloudinary(new Account(
    cloudinarySettings["CloudName"],
    cloudinarySettings["ApiKey"],
    cloudinarySettings["ApiSecret"]
));
builder.Services.AddSingleton(cloudinary);

// Configure JSON serialization to handle circular references
builder.Services.AddControllers().AddJsonOptions(options => {
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Version = "v1",
        Title = "API"

    });

    // Tùy chỉnh Swagger để hỗ trợ TimeOnly dưới dạng chuỗi
    c.MapType<TimeOnly>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "time",
        Example = new OpenApiString("00:00:00")
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
    // Thêm JWT Bearer Token vào Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Authorization header sử dụng scheme Bearer.",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Name = "Authorization",
        Scheme = "bearer"
    });
    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });

    c.OrderActionsBy((apiDesc) =>
    {
        if (apiDesc.HttpMethod == "POST") return "3";
        if (apiDesc.HttpMethod == "GET") return "1";
        if (apiDesc.HttpMethod == "PUT") return "2";
        if (apiDesc.HttpMethod == "DELETE") return "4";
        return "5";
    });
});
        

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<UserRepo>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ClipNchic.DataAccess.Repositories.CartRepo>();
builder.Services.AddScoped<ClipNchic.Business.Services.CartService>();

// DataAccess Repositories
builder.Services.AddScoped<ImageRepo>();
builder.Services.AddScoped<BlindBoxRepo>();
builder.Services.AddScoped<CharmProductRepo>();
builder.Services.AddScoped<CharmRepo>();
builder.Services.AddScoped<CollectionRepo>();
builder.Services.AddScoped<ShipRepo>();
builder.Services.AddScoped<ProductRepo>();
builder.Services.AddScoped<OrderRepo>();
builder.Services.AddScoped<ModelRepo>();
builder.Services.AddScoped<BaseRepo>();

// Business Services
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<BlindBoxService>();
builder.Services.AddScoped<CharmProductService>();
builder.Services.AddScoped<CharmService>();
builder.Services.AddScoped<CollectionService>();
builder.Services.AddScoped<ShipService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<ModelService>();
builder.Services.AddScoped<BaseService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    c.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

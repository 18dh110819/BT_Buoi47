using System.Security.Claims;
using System.Text;
using Cybersoft_store.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

//DI Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //Viết doc cho swagger api 
    // Nạp file XML chứa chú thích (summary, response...) để hiển thị trên Swagger UI
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }


    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "My API",
        Version = "v1",
        Description = "API documentation for .NET 10"
    });
    // Khai báo scheme Bearer -> tạo nút "Authorize" + ô nhập token trong Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Nhập token JWT vào ô dưới đây"
    });

    // Áp scheme cho toàn bộ endpoint -> hiện icon ổ khóa và tự gắn header Authorization khi gọi API
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});

//DI authentication - authorization = jwt
var key = builder.Configuration["Jwt:Key"];           // Khóa bí mật để ký token
var issuer = builder.Configuration["Jwt:Issuer"];     // Issuer (bên phát hành token)
var audience = builder.Configuration["Jwt:Audience"]; // Audience (người nhận token)
// 2. Cấu hình Authentication sử dụng JWT Bearer
builder.Services.AddAuthentication("Bearer").AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateIssuerSigningKey = true, // Xác thực key bí mật của token
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        ValidateIssuer = true,// Xác thực Issuer 
        ValidIssuer = issuer, // Phải khớp với Issuer trong token
        ValidateAudience = true,    // Xác thực Audience
        ValidAudience = audience, // Phải khớp với Audience trong token
        ValidateLifetime = true, // Xác thực thời gian hết hạn của token
        ClockSkew = TimeSpan.Zero, // Bỏ qua độ trễ thời gian giữa server và client (ngăn lỗi thời gian)
        RoleClaimType = ClaimTypes.Role, // Ánh xạ claim role
        NameClaimType = "UserName",
    };
});

//DI jwt service
// builder.Services.AddScoped<JwtAuthService>();

builder.Services.AddDbContext<CybersoftMarketPlaceContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBConnectionString"));
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartItemRepository, CartItemRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomer1Repository, Customer1Repository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductImageRepository, ProductImageRepository>();
builder.Services.AddScoped<IProductVariantRepository, ProductVariantRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IShopRepository, ShopRepository>();
builder.Services.AddScoped<IUserRolesRepository, UserRolesRepository>();

//DI UnitOfWork
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//DI Service
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//Cors for http://localhost:5018
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", builder =>
    {
        builder.WithOrigins("http://localhost:5018")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

//Sử dụng middleware controller 
app.MapControllers();

app.UseStaticFiles();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
    options.RoutePrefix = "swagger";
});

app.UseAuthentication(); //Xác thực (đăng nhập)
app.UseAuthorization(); //Phân quyền

app.Run();

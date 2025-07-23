using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentsProjectAPI.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
     /*.NET اضافة نظام تسجيل الدخول والهوية الجاهز من */   /*يخليك توصل وتستخدم قاعدة بياناتك */    /*يعطيك مزايا مثل توليد توكنات إعادة تعيين كلمة المرور والتحقق من الايميل*/
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<StudentDbContext>().AddDefaultTokenProviders();

// appsettings.json من ملف JWT ياخذ اعدادات  
var jwtSettings = builder.Configuration.GetSection("Jwt");
//كطريقة تحقق اساسية JWS يقول للنظام استخدم  
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => /*JWT ضبط خصائص */
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,    //(your-app تأكد أن التوكن جاي من مصدر معروف (زي
        ValidateAudience = true,  //( your-app-users تأكد أن التوكن موجه للجمهور المقصود (زي 
        ValidateLifetime = true,  //تأكد أن التوكن ما انتهت صلاحيته
        ValidateIssuerSigningKey = true, //تأكد أن التوكن تم توقيعه بمفتاح صحيح
        ValidIssuer = jwtSettings["Issuer"], //من وين التوكن صادر
        ValidAudience = jwtSettings["Audience"], //لمن التوكن موجه
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"])) //المفتاح اللي يُستخدم لتوقيع وتحقق التوكن. لازم يكون نفسه اللي استخدمته عند الإنشاء
    };
});



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StudentDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("EFCoreDBConnection")));

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

using ShoppingAutomation.Api.Automation;
using ShoppingAutomation.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// תיקון: הגדרת Kestrel לאפשר גמישות בפורטים ומניעת קריסה כשהכתובת תפוסה
builder.WebHost.ConfigureKestrel(options =>
{
    // הגדרה זו מאפשרת ל-Kestrel להשתמש בפורטים שהוגדרו ב-launchSettings.json
    // כולל fallback לפורט 0 (פורט דינמי) במקרה של התנגשות
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddSingleton<ShopAutomation>();
builder.Services.AddSingleton<SearchService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Shopping Automation API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
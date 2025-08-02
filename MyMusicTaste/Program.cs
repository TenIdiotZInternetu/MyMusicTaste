using MyMusicTaste.Database.Contexts.MongoDb;
using MyMusicTaste.Startup;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string? dbConnectionString = builder.Configuration["MONGODB_URI"];
MongoDbContext.Connect(dbConnectionString);

builder.Services.AddHttpContextAccessor();
builder.Services.InjectDependencies();
builder.Services.AddControllers();

MongoIdentity.Configure(builder.Services, dbConnectionString!);
IdentitySettings.Setup(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

MiddlewareSetup.Setup(app);
app.Run();
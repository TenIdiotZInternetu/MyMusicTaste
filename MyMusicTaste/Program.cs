using MyMusicTaste;
using MyMusicTaste.Components;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Contexts.MongoDb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string? dbConnectionString = builder.Configuration["MONGODB_URI"];
MongoDbContext.Connect(dbConnectionString);

builder.Services.InjectDependencies();
MongoIdentity.Configure(builder.Services, dbConnectionString!);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
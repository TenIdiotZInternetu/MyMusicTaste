using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MyMusicTaste.Components;
using MyMusicTaste.Database;
using MyMusicTaste.Database.Connections;
using MyMusicTaste.Database.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

string? dbConnectionString = builder.Configuration["MONGODB_URI"];
MongoDbConnection.Connect(dbConnectionString);

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddMongoDbStores<ApplicationUser, ApplicationRole, ObjectId>(dbConnectionString, "Security");

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

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
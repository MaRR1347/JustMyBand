using BlazorApp1;
using BlazorApp1.Components;
using Microsoft.AspNetCore.Components.Server;
using BlazorApp1.DBContext;
using BlazorApp1.Hubs;
using BlazorApp1.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSignalR();

builder.Services.AddSingleton<JSONService>();
builder.Services.AddSingleton<QueueService>();
builder.Services.AddSingleton<SongService>();
builder.Services.AddSingleton<MidiService>();
builder.Services.AddSingleton<MidiCommunicationService>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
{
    var context = new AppDBContext();

    context.Database.ExecuteSqlRaw("DELETE FROM queue");
    context.Database.ExecuteSqlRaw("DELETE FROM sqlite_sequence WHERE name='queue'");

});

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseMiddleware<SettingsMiddleware>();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapHub<QueueHub>("/queuehub");

app.Run();

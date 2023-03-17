using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebSharper.AspNetCore;
using WebSharper.UI.CSharp.Routing.Tests;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddWebSharper()
                .AddAuthentication("WebSharper")
                .AddCookie("WebSharper", options => { });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

//Enable if you want to make RPC calls to server
//app.UseWebSharperRemoting();

app.Run();

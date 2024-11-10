using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using DataAccesLayer.Concrete;
using ServiceLayer.Container;
using FastEndpoints;
using FastEndpoints.Swagger;
using AuthenticationLayer.Extensions;
using AuthenticationLayer.Middleware;
using AuthenticationLayer.Extensions;
using ServiceLayer.Container;
using ECommerceView.Endpoints.Interfaces;
using ECommerceView.Endpoints.Baskets;
using ECommerceView.Endpoints.Orders;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleyin
builder.Services.ContainerDependencies(builder.Configuration);
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthenticationServices();
builder.Services.AddAuthenticationServices();
builder.Services.AddAuthentication("login")
    .AddCookie("login", options =>
    {
        options.LoginPath = "/api/login"; // Giri� yolu
        options.LogoutPath = "/api/logout"; // ��k�� yolu
    });
builder.Services.AddScoped<ISyncCartToDatabaseEndpoint, SyncCartToDatabaseEndpoint>();
builder.Services.AddScoped<ICompleteOrderEndpoint, CompleteOrderEndpoint>();
builder.Services.AddScoped<IRemoveFromCartEndpoint, RemoveFromCartEndpoint>();
// FastEndpoints'i ekleyin
builder.Services.AddFastEndpoints();

// Swagger'� ekleyin
builder.Services.SwaggerDocument();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Session servislerini ekleyin
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// DataAccessLayer Context'ini ekleyin (gerekirse yorum sat�r�n� kald�r�n)
//builder.Services.AddDbContext<Context>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

// Token authentication middleware'ini ekleyin
app.UseTokenAuthentication();

app.UseAuthentication();
app.UseAuthorization();

// FastEndpoints middleware'ini ekleyin
app.UseFastEndpoints();

// Swagger UI'� ekleyin
app.UseSwaggerGen();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pages}/{action=Home}/{id?}");

app.Run();
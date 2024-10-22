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

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleyin
builder.Services.ContainerDependencies();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddAuthenticationServices();
builder.Services.AddAuthenticationServices();

// FastEndpoints'i ekleyin
builder.Services.AddFastEndpoints();

// Swagger'ý ekleyin
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

// DataAccessLayer Context'ini ekleyin (gerekirse yorum satýrýný kaldýrýn)
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

// Swagger UI'ý ekleyin
app.UseSwaggerGen();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pages}/{action=Home}/{id?}");

app.Run();
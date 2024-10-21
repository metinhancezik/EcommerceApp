using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.EntityFrameworkCore;
using DataAccesLayer.Concrete;
using ServiceLayer.Container;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

// Servisleri ekleyin
builder.Services.ContainerDependencies();
builder.Services.AddAutoMapper(typeof(Program));

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
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

app.UseRouting();

// FastEndpoints middleware'ini ekleyin
app.UseFastEndpoints();

// Swagger UI'ý ekleyin
app.UseSwaggerGen();

// IdentityServer middleware'ini ekleyin
//app.UseIdentityServer();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Pages}/{action=Home}/{id?}");

app.Run();
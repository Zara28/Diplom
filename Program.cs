using Goldev.Core.Extensions;
using Goldev.Core.MediatR.Extensions;
using Goldev.Core.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using OfficeTime.DBModels;
using OfficeTime.Logic.Integrations.Refit.Intefaces;
using OfficeTime.Logic.Integrations.YandexTracker;
using OfficeTime.Logic.Integrations.YandexTracker.Cache;
using OfficeTime.Logic.Integrations.YandexTracker.Models;
using OfficeTime.Mapper;
using Refit;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Login", "");
});
//builder.Services.AddRazorPages();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<diplom_adminkaContext>();
builder.Services.AddSingleton<MemoryCache<ResponseModel<List<YandexTask>>>>();
builder.Services.AddMediatR<Program>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
        

builder.Configuration.AddConstants();

builder.Services.ConfigureWithEnv<YandexTrackerConfiguration>(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.UseSession();


app.Run();

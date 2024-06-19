using BManagerAPi.Repositories;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Supabase;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Hosting;

DotNetEnv.Env.Load();
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
    
// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

IConfiguration Configuration = builder.Configuration;

// Initialize Supabase client
var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");

var options = new SupabaseOptions
{
    AutoConnectRealtime = true
};

var supabase = new Client(url, key, options);
await supabase.InitializeAsync();
try
{
    supabase = new Client(url, key, options);
    await supabase.InitializeAsync();
}
catch (Exception ex)
{
    throw new Exception("Failed to initialize Supabase client.", ex);
}

// Add Supabase client as a singleton service
builder.Services.AddSingleton(supabase);

builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UserOrderService>();
builder.Services.AddScoped<NoteService>();
builder.Services.AddControllers(options => options.SuppressAsyncSuffixInActionNames = false);
// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowSpecificOrigin");
app.UseAuthorization();

// Map controllers
app.MapControllers();


app.Run();

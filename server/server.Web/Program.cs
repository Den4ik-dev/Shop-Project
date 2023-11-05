using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using server.Application.Interfaces;
using server.Domain;
using server.Domain.Dto;
using server.Infrastructure.Services;
using server.Infrastructure.Services.Customers;
using server.Infrastructure.Validation;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// cors
builder.Services.AddCors();

// @database
string connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<ApplicationContext>(options =>
  options.UseSqlite(connectionString));

builder.Services.AddControllers();

// @authentication & authorization
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(options => options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
  {
    ValidateIssuer = true,
    ValidIssuer = builder.Configuration["JWT:ISSUER"],
    ValidateAudience = true,
    ValidAudience = builder.Configuration["JWT:AUDIENCE"],
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:KEY"]))
  });
builder.Services.AddAuthorization();

// @validation
builder.Services.AddTransient<IValidator<RegisteredCustomerDto>, RegisteredCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<LoginCustomerDto>, LoginCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedCustomerDto>, ChangedCustomerDtoValidator>();
builder.Services.AddTransient<IValidator<TokenDto>, TokenDtoValidator>();
builder.Services.AddTransient<IValidator<AddedProductDto>, AddedProductDtoValidator>();
builder.Services.AddTransient<IValidator<AddedDeliveryDto>, AddedDeliveryDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedDeliveryDto>, ChangedDeliveryDtoValidator>();
builder.Services.AddTransient<IValidator<AddedPriceChangeDto>, AddedPriceChangeDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedPriceChangeDto>, ChangedPriceChangeDtoValidator>();
builder.Services.AddTransient<IValidator<AddedCommentDto>, AddedCommentDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedCommentDto>, ChangedCommentDtoValidator>();
builder.Services.AddTransient<IValidator<ChangedProductDto>, ChangedProductDtoValidator>();
builder.Services.AddTransient<IValidator<AddedBasketItemDto>, AddedBasketItemDtoValidator>();

// @services
builder.Services.AddScoped<ICustomersService, CustomersService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
builder.Services.AddScoped<IBalanceService, BalanceService>();
builder.Services.AddScoped<IProductCategoriesService, ProductCategoriesService>();
builder.Services.AddScoped<IProductManufacturersService, ProductManufacturersService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IProductImagesService, ProductImagesService>();
builder.Services.AddScoped<IProductVideosService, ProductVideosService>();
builder.Services.AddScoped<IProductDeliveriesService, ProductDeliveriesService>();
builder.Services.AddScoped<IPriceChangesService, PriceChangesService>();
builder.Services.AddScoped<IProductCommentsService, ProductCommentsService>();
builder.Services.AddScoped<ICustomerBasketsService, CustomerBasketsService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(options => options
  .WithOrigins("http://localhost:5173")
  .AllowCredentials()
  .AllowAnyMethod()
  .AllowAnyHeader()
  .WithExposedHeaders("x-total-count"));

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
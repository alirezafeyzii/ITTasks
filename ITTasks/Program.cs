using ITTasks.DataLayer;
using ITTasks.Infrastructure.Migrate;
using ITTasks.Infrastructure.Seeding.Roles;
using ITTasks.Infrastructure.Seeding.SeedingConfig;
using ITTasks.Repositories.Roles;
using ITTasks.Repositories.Sprints;
using ITTasks.Repositories.Tasks;
using ITTasks.Repositories.Tasks.TasksType;
using ITTasks.Repositories.Users;
using ITTasks.Services.Roles;
using ITTasks.Services.Sprints;
using ITTasks.Services.Tasks;
using ITTasks.Services.Tasks.TasksType;
using ITTasks.Services.Users;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ITDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ITTaskConnection"));
//});

builder.Services.AddCors();

builder.Services.AddDbContext<ITDbContext>(option =>
{
	option.UseSqlite("Data Source=ittask.db");
});

builder.Services.AddTransient<RolesDataSeeding>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
builder.Services.AddScoped<ITaskTypeService, TaskTypeService>();

builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<ISprintService, SprintService>();

builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();

//builder.Services.AddAuthentication(options =>
//{
//	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//	.AddJwtBearer(options =>
//	{
//		options.SaveToken = true;
//		options.RequireHttpsMetadata = false;
//		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//		{
//			ValidateIssuer = true,
//			ValidateAudience = true,
//			ValidAudience = builder.Configuration["JWT:ValidAudience"],
//			ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
//			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
//		};
//	});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie();

//builder.Services.AddAuthentication(option =>
//{
//	option.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//	option.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
//	option.DefaultAuthenticateScheme = OpenIdConnectDefaults.AuthenticationScheme;
//}).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
//.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
//{
//	options.Authority = "http://192.168.100.100:1234";
//	options.ClientId = "mvc";
//	options.ClientSecret = "secret";
//	options.SaveTokens = true;
//	options.GetClaimsFromUserInfoEndpoint = true;
//	options.RequireHttpsMetadata = false;
//});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}


app.UseCors(builder => builder
				.WithOrigins("https://localhost:7180/").AllowAnyMethod()
				.AllowAnyHeader()
				.AllowAnyMethod()
				.SetIsOriginAllowed((host) => true)
				.AllowCredentials()
			);


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseRoleDataSeed();

app.UseMigration();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Tasks}/{action=CreateTask}/{id?}");

app.Run();

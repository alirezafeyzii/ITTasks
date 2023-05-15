using ITTasks.DataLayer;
using ITTasks.Repositories.Sprints;
using ITTasks.Repositories.Tasks;
using ITTasks.Repositories.Tasks.TasksType;
using ITTasks.Repositories.Users;
using ITTasks.Services.Sprints;
using ITTasks.Services.Tasks;
using ITTasks.Services.Tasks.TasksType;
using ITTasks.Services.Users;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<ITDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("ITTaskConnection"));
//});

builder.Services.AddDbContext<ITDbContext>(option =>
{
    option.UseSqlite("Data Source=ittask.db");
});


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ITaskService, TaskService>();

builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
builder.Services.AddScoped<ITaskTypeService, TaskTypeService>();

builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<ISprintService, SprintService>();


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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

using var scope = app.Services.CreateScope();

var context = scope.ServiceProvider.GetRequiredService<ITDbContext>();

context?.Database.Migrate();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Tasks}/{action=CreateTask}/{id?}");

app.Run();

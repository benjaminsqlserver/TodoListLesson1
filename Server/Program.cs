using Radzen;
using ToDoList.Server.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using Microsoft.AspNetCore.OData;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddRazorComponents().AddInteractiveServerComponents().AddHubOptions(options => options.MaximumReceiveMessageSize = 10 * 1024 * 1024).AddInteractiveWebAssemblyComponents();
builder.Services.AddControllers();
builder.Services.AddRadzenComponents();
builder.Services.AddRadzenCookieThemeService(options =>
{
    options.Name = "ToDoListTheme";
    options.Duration = TimeSpan.FromDays(365);
});
builder.Services.AddHttpClient();
builder.Services.AddScoped<ToDoList.Server.ConDataService>();
builder.Services.AddDbContext<ToDoList.Server.Data.ConDataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConDataConnection"));
});
builder.Services.AddControllers().AddOData(opt =>
{
    var oDataBuilderConData = new ODataConventionModelBuilder();
    oDataBuilderConData.EntitySet<ToDoList.Server.Models.ConData.PriorityLevel>("PriorityLevels");
    oDataBuilderConData.EntitySet<ToDoList.Server.Models.ConData.Status>("Statuses");
    oDataBuilderConData.EntitySet<ToDoList.Server.Models.ConData.ToDoList>("ToDoLists");
    opt.AddRouteComponents("odata/ConData", oDataBuilderConData.GetEdmModel()).Count().Filter().OrderBy().Expand().Select().SetMaxTop(null).TimeZone = TimeZoneInfo.Utc;
});
builder.Services.AddScoped<ToDoList.Client.ConDataService>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.MapControllers();
app.UseStaticFiles();
app.UseAntiforgery();
app.MapRazorComponents<App>().AddInteractiveServerRenderMode().AddInteractiveWebAssemblyRenderMode().AddAdditionalAssemblies(typeof(ToDoList.Client._Imports).Assembly);
app.Run();
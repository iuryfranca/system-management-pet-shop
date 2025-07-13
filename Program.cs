using SystemManagementPetshop.Context;
using SystemManagementPetshop.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AnimalService>();
builder.Services.AddScoped<FuncionarioService>();
builder.Services.AddScoped<ProdutoService>();
builder.Services.AddScoped<ServicoService>();
builder.Services.AddScoped<ServicoProdutoService>();

builder.Services.AddScoped<SideMenuService>();
builder.Services.AddScoped<IFlowbiteService, FlowbiteService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<AuthService>();

string mySqlConnectionStr = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ContextDB>(options =>
    options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr))
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/layout/_Host");

app.Run();

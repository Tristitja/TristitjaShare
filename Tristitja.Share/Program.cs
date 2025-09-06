using FluentValidation;
using Microsoft.AspNetCore.Components;
using Tristitja.Auth.Local;
using Tristitja.Auth.Local.Dto;
using Tristitja.Share;
using Tristitja.Share.Authorization;
using Tristitja.Share.Components;
using Tristitja.Share.Configuration;
using Tristitja.Share.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddOptions<DatabaseOptions>()
    .Bind(builder.Configuration.GetSection(DatabaseOptions.Key))
    .ValidateDataAnnotations()
    .ValidateOnStart();

ValidatorOptions.Global.LanguageManager.Enabled = false;

builder.Services.AddHttpContextAccessor();
builder.Services.AddDbContext<AppDbContext>();

// TODO: only temp
builder.Services.AddValidatorsFromAssemblyContaining<CreateInitialUserRequest>();

builder.Services.AddTristitjaAuthLocal<AppDbContext>();
builder.Services.AddTristitjaShareAuthorization();

builder.Services.AddScoped<CounterState>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapAuthEndpoints();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

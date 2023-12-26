using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "NomeDaSuaAPI", Version = "v1" });
    // Adicione os comentários XML do seu projeto, se houverem
    // c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "NomeDoSeuProjeto.xml"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "NomeDaSuaAPI v1");
    c.RoutePrefix = "swagger";
});


app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

//Forma de Autenticação
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";

})
//Parametros de validação do Token
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //Valida quem está solicitando
        ValidateIssuer = true,
        //Valida quem está recebendo
        ValidateAudience = true,
        //Define se o campo de expiração será validado
        ValidateLifetime = true,
        //Criptograia e validação da chave de autenticação
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        //Valida o tempo de expiração do Token
        ClockSkew = TimeSpan.FromMinutes(60),
        //Nome do Issuer, da origem
        ValidIssuer = "exoapi.webapi",
        //Nome do audience, para o destino
        ValidAudience = "exoapi.webapi"

    };
});


builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Exo.WebApi", Version = "v1" });
    // Adicione os comentários XML do seu projeto, se houverem
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "projeto.xml"));
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Exo.WebApi v1");
    c.RoutePrefix = "swagger";
});


app.UseRouting();

app.UseAuthentication(); 
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

//Forma de Autentica��o
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";

})
//Parametros de valida��o do Token
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //Valida quem est� solicitando
        ValidateIssuer = true,
        //Valida quem est� recebendo
        ValidateAudience = true,
        //Define se o campo de expira��o ser� validado
        ValidateLifetime = true,
        //Criptograia e valida��o da chave de autentica��o
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        //Valida o tempo de expira��o do Token
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
    // Adicione os coment�rios XML do seu projeto, se houverem
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

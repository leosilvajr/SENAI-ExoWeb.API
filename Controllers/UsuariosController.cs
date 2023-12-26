#pragma warning disable 1591
using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.SymbolStore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Exo.WebApi.Controllers
{
    [Produces("application/json")]
    [Route("api/[Controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuariosController(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        //get /api/usuarios
        [HttpGet]
        public IActionResult Listar()

        {
            return Ok(_usuarioRepository.Listar());
        }

        //[HttpPost]
        //public IActionResult Cadastrar(Usuario usuario)

        //{
        //    _usuarioRepository.Cadastrar(usuario);
        //    return StatusCode(201);
        //}

        [HttpPost()]
        public IActionResult Post(Usuario usuario)
        {
            Usuario usuarioBuscado = _usuarioRepository.Login(usuario.Email, usuario.Senha);
            if (usuarioBuscado == null) // Caso nao encontrar usuario no Login, retornar um Erro
            {
                return NotFound("E-mail ou Senha inválidos.");
            }

            //Caso Usuario for encontrar vamos criar o Token para autenticação.

            //Define os dados que serão fornecidos no token - Payload.
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email),
                new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.Id.ToString())
            };

            //Define a chave de acesso ao token
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao"));

            //Define as credenciais do token.
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            //Gera o Token
            var token = new JwtSecurityToken(
                issuer: "exoapi.webapi", //Emissor do Token
                audience: "exoapi.webapi", //Destinatario do Token
                claims: claims, //Dados definidos acima
                expires: DateTime.Now.AddMinutes(30), // Tempo de Expiraçao
                signingCredentials: creds //Credenciais do Token
                );

            //Retorna ok com o token
            return Ok(
                new { token = new JwtSecurityTokenHandler().WriteToken(token) }
                );
        }


        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)

        {
             Usuario usuario = _usuarioRepository.BuscaPorId(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return Ok(usuario);
        }


        [Authorize] // Exige o Token
        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Usuario usuario)

        {
            _usuarioRepository.Atualizar(id, usuario);
            return StatusCode(204);
        }

        [Authorize] // Exige o Token
        [HttpDelete]
        public IActionResult Deletar (int id) 
        {
            try
            {
                _usuarioRepository.Deletar(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);  
            }
        }

    }
}

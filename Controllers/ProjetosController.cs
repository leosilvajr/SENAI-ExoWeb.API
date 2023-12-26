#pragma warning disable 1591
using Exo.WebApi.Models;
using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exo.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProjetosController : Controller
    {

        private readonly ProjetoRepository _projetoRepository;

        public ProjetosController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet]
        public IActionResult Listar()

        {
            return Ok(_projetoRepository.Listar());
        }

        [HttpPost]
        public IActionResult Cadastrar(Projeto projeto)
        {
            _projetoRepository.Cadastrar(projeto);
            return StatusCode(201);
        }

        [HttpGet("{id}")]
        public IActionResult BuscarPorId(int id)
        {
            Projeto projeto = _projetoRepository.BuscarPorId(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return Ok(projeto);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Projeto projeto)
        {
            try
            {
                _projetoRepository.Atualizar(id, projeto);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                _projetoRepository.Deletar(id);
                return StatusCode(204);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

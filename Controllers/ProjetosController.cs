using Exo.WebApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Exo.WebApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class ProjetosController : Controller
    {

        private readonly ProjetoRepository  _projetoRepository;

        public ProjetosController(ProjetoRepository projetoRepository)
        {
            _projetoRepository = projetoRepository;
        }

        [HttpGet]
        public IActionResult Listar()
        
        {
            return Ok(_projetoRepository.Listar());
        }
    }
}

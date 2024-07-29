using Barbearia.API.Data;
using Barbearia.API.DTO;
using Barbearia.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Barbearia.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class ServicoController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public ServicoController(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Lcol_Servicos = await _dbcontext.Servicos.ToListAsync();
            return Ok(Lcol_Servicos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Servico>> GetById(int id)
        {
            var Lobj_Servico = await _dbcontext.Servicos.FindAsync(id);

            if (Lobj_Servico == null)
            {
                return NotFound();
            }

            return Lobj_Servico;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServicoDTO Pobj_ServicoDTO)
        {
            if (ModelState.IsValid)
            {
                Servico Lobj_Servico = new Servico();
                Lobj_Servico.NomeServico = Pobj_ServicoDTO.NomeServico;
                Lobj_Servico.Descricao = Pobj_ServicoDTO.Descricao;
                Lobj_Servico.Preco = Pobj_ServicoDTO.Preco;
                Lobj_Servico.DuracaoMin = Pobj_ServicoDTO.DuracaoMin;

                _dbcontext.Servicos.Add(Lobj_Servico);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = Lobj_Servico.Id }, Lobj_Servico);

            }
            return BadRequest();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ServicoDTO Pobj_Servico)
        {
            if (ModelState.IsValid)
            {
                var Lobj_Servico = _dbcontext.Servicos.Find(id);
                if (Lobj_Servico == null)
                {
                    return NotFound();
                }

                Lobj_Servico.NomeServico = Pobj_Servico.NomeServico;
                Lobj_Servico.Descricao = Pobj_Servico.Descricao;
                Lobj_Servico.DuracaoMin = Pobj_Servico.DuracaoMin;
                Lobj_Servico.Preco = Pobj_Servico.Preco;

                _dbcontext.Servicos.Update(Lobj_Servico);
                await _dbcontext.SaveChangesAsync();
                return Ok(Lobj_Servico);
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Lobj_Servico = _dbcontext.Servicos.Find(id);
            if (Lobj_Servico == null)
            {
                return NotFound();
            }
            _dbcontext.Servicos.Remove(Lobj_Servico);
            await _dbcontext.SaveChangesAsync();
            return Ok(Lobj_Servico);
        }
    }
}

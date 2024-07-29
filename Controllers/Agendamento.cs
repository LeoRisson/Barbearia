using Barbearia.API.Data;
using Barbearia.API.DTO;
using Barbearia.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Barbearia.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AgendamentoController : ControllerBase
    {
        private readonly ApplicationDbContext _dbcontext;

        public AgendamentoController(ApplicationDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Lobj_Agendamentos = await _dbcontext
                                                    .Agendamentos
                                                    .Include(c => c.cliente)
                                                    .Include(s => s.Servicos)
                                                    .ToListAsync();
            return Ok(Lobj_Agendamentos);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Agendamento>> GetById(int id)
        {
            var Lobj_Agentamento = await _dbcontext.Agendamentos.FindAsync(id);

            if (Lobj_Agentamento == null)
            {
                return NotFound();
            }

            return Lobj_Agentamento;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AgendamentoDTO Pobj_AgendamentoDTO)
        {
            if (ModelState.IsValid)
            {
                Agendamento Lobj_Agendamento = new Agendamento();
                Lobj_Agendamento.ClienteID = Pobj_AgendamentoDTO.ClienteID;
                Lobj_Agendamento.Observacoes = Pobj_AgendamentoDTO.Observacoes;
                Lobj_Agendamento.DataHora = Pobj_AgendamentoDTO.DataHora;
                Lobj_Agendamento.Status = Pobj_AgendamentoDTO.Status;
                Lobj_Agendamento.Servicos = new List<Servico>();
                Lobj_Agendamento.cliente = _dbcontext.Clientes.Find(Lobj_Agendamento.ClienteID);

                foreach (var item in Pobj_AgendamentoDTO.Servicos)
                {
                    var Pobj_Servico = _dbcontext.Servicos.Find(item.Id);
                    if(Pobj_Servico == null)
                    {
                        return BadRequest();
                    }

                    Lobj_Agendamento.Servicos.Add(Pobj_Servico);
                }

                _dbcontext.Agendamentos.Add(Lobj_Agendamento);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { id = Lobj_Agendamento.AgendamentoID }, Lobj_Agendamento);

            }
            return BadRequest();
     
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, AgendamentoDTO Pobj_AgendamentoDTO)
        {

            if (ModelState.IsValid)
            {
                var Lobj_agendamento = _dbcontext
                                                 .Agendamentos
                                                 .Include(s => s.Servicos)
                                                 .Include(c => c.cliente)
                                                 .FirstOrDefault(x => x.AgendamentoID == id);


                Lobj_agendamento.ClienteID = Pobj_AgendamentoDTO.ClienteID;
                Lobj_agendamento.Observacoes = Pobj_AgendamentoDTO.Observacoes;
                Lobj_agendamento.DataHora = Pobj_AgendamentoDTO.DataHora;
                Lobj_agendamento.Status = Pobj_AgendamentoDTO.Status;
                Lobj_agendamento.Servicos = new List<Servico>();
                Lobj_agendamento.cliente = _dbcontext.Clientes.Find(Lobj_agendamento.ClienteID);

                foreach (var item in Pobj_AgendamentoDTO.Servicos)
                {
                    var Pobj_Servico = _dbcontext.Servicos.Find(item.Id);
                    if (Pobj_Servico == null)
                    {
                        return BadRequest();
                    }

                    Lobj_agendamento.Servicos.Add(Pobj_Servico);
                }

                _dbcontext.Agendamentos.Update(Lobj_agendamento);
                await _dbcontext.SaveChangesAsync();
                return Ok(Lobj_agendamento);
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var Lobj_Agendamento = _dbcontext
                                            .Agendamentos
                                            .Include(s => s.Servicos)
                                            .Include(c => c.cliente)
                                            .FirstOrDefault(x => x.AgendamentoID == id);
            if (Lobj_Agendamento == null)
            {
                return NotFound();
            }
            _dbcontext.Agendamentos.Remove(Lobj_Agendamento);
            await _dbcontext.SaveChangesAsync();
            return Ok(Lobj_Agendamento);
        }
    }
}

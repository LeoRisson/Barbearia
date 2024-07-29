using Barbearia.API.Data;
using Barbearia.API.DTO;
using Barbearia.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Barbearia.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class ClienteController : ControllerBase
    {
        public readonly ApplicationDbContext _dbcontext;

        public ClienteController(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var Lcol_Clientes = await _dbcontext.Clientes.ToListAsync();
            return Ok(Lcol_Clientes);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClienteDTO Pobj_clienteDTO)
        {
            if (ModelState.IsValid)
            {
                Cliente Lobj_Cliente = new Cliente();
                Lobj_Cliente.Nome = Pobj_clienteDTO.Nome;
                Lobj_Cliente.Telefone = Pobj_clienteDTO.Telefone;
                Lobj_Cliente.DataNascimento = Pobj_clienteDTO.DataNascimento;
                Lobj_Cliente.Email = Pobj_clienteDTO.Email;

                _dbcontext.Clientes.Add(Lobj_Cliente);
                await _dbcontext.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById),new {id = Lobj_Cliente.ClienteID}, Lobj_Cliente);

            }
            return BadRequest();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Cliente>> GetById(int id)
        {
            var Lobj_Cliente = await _dbcontext.Clientes.FindAsync(id);

            if (Lobj_Cliente == null)
            {
                return NotFound();
            }

            return Lobj_Cliente;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ClienteDTO Pobj_clienteDTO)
        {

            if (ModelState.IsValid)
            {
                var Lobj_Cliente = _dbcontext.Clientes.Find(id);
                if(Lobj_Cliente == null)
                {
                    return NotFound();
                }

                Lobj_Cliente.Nome = Pobj_clienteDTO?.Nome;
                Lobj_Cliente.Telefone = Pobj_clienteDTO.Telefone;
                Lobj_Cliente.Email = Pobj_clienteDTO.Email;
                Lobj_Cliente.DataNascimento = Pobj_clienteDTO.DataNascimento;

                _dbcontext.Clientes.Update(Lobj_Cliente);
                await _dbcontext.SaveChangesAsync();
                return Ok(Lobj_Cliente);
            }
            return BadRequest();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) {
            var Lobj_Clinte = _dbcontext.Clientes.Find(id);
            if(Lobj_Clinte == null)
            {
                return NotFound();
            }
            _dbcontext.Clientes.Remove(Lobj_Clinte);
            await _dbcontext.SaveChangesAsync();
            return Ok(Lobj_Clinte);
        }
    }
}

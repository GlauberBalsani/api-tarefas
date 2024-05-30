using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APITarefa.Context;
using APITarefa.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace APITarefa.Controller
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            _context.Add(tarefa);
            _context.SaveChanges();


            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);


        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            try
            {
                var tarefa = await _context.Tarefas
                    .Where(t => t.Id == id)
                    .ToListAsync();
                return Ok(tarefa);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] Tarefa tarefa)
        {
            var tarefaBanco = await _context.Tarefas.FindAsync(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Data = tarefa.Data;

            _context.Entry(tarefaBanco).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();

        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {

            var tarefas = await _context.Tarefas.Where(x => x.Status == status).ToListAsync();

            if (tarefas == null || !tarefas.Any())
            {
                return NotFound("Nenhuma tarefa encontrada com o status fornecido.");
            }

            return Ok(tarefas);
        }


        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var tarefa = await _context.Tarefas.FirstOrDefaultAsync(t => t.Titulo == titulo);

            if (tarefa == null)
            {
                return NotFound();
            }

            return Ok(tarefa);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var tarefas = await _context.Tarefas.Where(x => x.Data.Date == data.Date).ToListAsync();

            if (tarefas == null || !tarefas.Any())
            {
                return NotFound("Nenhuma tarefa encontrada para a data fornecida.");
            }

            return Ok(tarefas);
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            // Remover a tarefa encontrada através do EF
            _context.Tarefas.Remove(tarefaBanco);

            // Salvar as mudanças
            _context.SaveChanges();

            return NoContent();
        }





        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            var tarefas = await _context.Tarefas.ToListAsync();

            return Ok(tarefas);
        }
    }
}
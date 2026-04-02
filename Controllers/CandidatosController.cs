using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EleicaoBrasilApi.Data;
using EleicaoBrasilApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EleicaoBrasilApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CandidatosController : ControllerBase
    {
    private readonly AppDbContext _context;
        public CandidatosController(AppDbContext context)
        {
            _context = context;    
        }

        [HttpGet]

        public IActionResult Get()
        {
            var candidatos = _context.Candidatos.ToList();
            return Ok(candidatos);
        }
        
        [HttpGet("{partido}")]

        public IActionResult Get(string partido)
        {
            var candidatos = _context.Candidatos.Where(c => c.Partido == partido).ToList();
            if (candidatos.Count == 0)
            {
                return NotFound();
            }
            return Ok(candidatos);
        }

        [HttpPost]

        public IActionResult Post(Candidato candidato)
        {
            
            bool validaNumero = _context.Candidatos.Any(c => c.Numero == candidato.Numero);
            if (validaNumero)
            {
                return BadRequest("Número de candidato já existe.");
            }
            _context.Candidatos.Add(candidato);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = candidato.Id}, candidato);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, Candidato candidato, string viceNome)
        {
            if (id !=candidato.Id)
            {
                return BadRequest();
            }

            candidato.ViceNome = viceNome;
            _context.Candidatos.Update(candidato);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var candidato = _context.Candidatos.Find(id);
            if (candidato == null)
            {
                return NotFound();
            }
            _context.Candidatos.Remove(candidato);
            _context.SaveChanges();
            return NoContent();
        }

    }
}
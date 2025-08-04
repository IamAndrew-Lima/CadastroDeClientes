using CadastroClientes.Models;
using CadastroClientes.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CadastroClientes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly ClientesRepository _clientesRepository;

        public ClientesController(ClientesRepository clientesRepository)
        {
            _clientesRepository = clientesRepository;
        }

        [HttpPost("Salvar")]
        public async Task<IActionResult> Salvar([FromBody] Clientes cadastro) 
        {
            if (cadastro == null)
            {
                return BadRequest(new { message = "Dados do cliente não podem ser nulos." });
            }

            try
            {
                await _clientesRepository.SalvarAsync(cadastro); 
                return Ok(new { message = "Cliente salvo com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpPost("Alterar")]
        public async Task<IActionResult> Alterar([FromBody] Clientes cadastro)
        {
            if (cadastro == null)
            {
                return BadRequest(new { message = "Dados do cliente não podem ser nulos." });
            }

            try
            {
                // Use 'await' para aguardar a conclusão da tarefa
                bool sucesso = await _clientesRepository.AlterarAsync(cadastro);
                if (sucesso)
                {
                    return Ok(new { message = "Cliente alterado com sucesso!" });
                }
                return NotFound(new { message = "Cliente não encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }



        [HttpGet("Listar")]
        public async Task<IActionResult> Listar()
        {
            try
            {
                var clientes = await _clientesRepository.ListarAsync();
                var clienteList = clientes.Select(c => new Clientes
                {
                    IdCliente = c.IdCliente,
                    Documento = c.Documento,
                    Nome = c.Nome,
                    Sexo = c.Sexo,
                    Email = c.Email,
                    Telefone = c.Telefone,
                    Fax = c.Fax,
                    UF = c.UF
                }).ToList();

                return Ok(clienteList);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


        [HttpDelete("Deletar")]
        public async Task<IActionResult> Deletar(string documento)
        {
            try
            {
                // Use 'await' para aguardar a conclusão da tarefa
                bool retornoDelete = await _clientesRepository.DeletarAsync(documento);
                if (retornoDelete)
                {
                    return Ok(new { message = "Cliente deletado com sucesso!" });
                }
                return NotFound(new { message = "Cliente não encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }

        [HttpGet("GetCliente")]
        public async Task<IActionResult> GetCliente(string documento)
        {
            try
            {
                var cliente = await _clientesRepository.GetClienteAsync(documento);
                if (cliente != null)
                {
                    return Ok(cliente);
                }
                return NotFound(new { message = "Cliente não encontrado." });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = ex.Message });
            }
        }


    }
}

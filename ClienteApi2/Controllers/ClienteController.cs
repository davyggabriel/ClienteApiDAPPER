using ClienteApi2.Models;
using ClienteApi2.Repositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace ClienteApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly RepositorioClienteDapper _repository;
        public ClienteController(IConfiguration configuration)
        { 
            _repository = new RepositorioClienteDapper(configuration);
        }

        [HttpPost("cadastrar")]
        public ActionResult<Cliente> Cadastrar([FromBody] Cliente cliente)
        {
            cliente.Endereco.Id = _repository.InserirEndereco(cliente.Endereco);
            cliente.IdEndereco = cliente.Endereco.Id;
            cliente.Id = _repository.InserirCliente(cliente);
            cliente.Telefone.Id = _repository.InserirTelefone(cliente.Telefone,cliente.Id);

            return Created("", cliente);
        }

        [HttpGet("listar")]
        public IActionResult Listar()
        {
            var clientes = _repository.ListarTodos();
            return Ok(clientes);
        }

        [HttpGet("busca-por-cpf/{cpf}")]
        public ActionResult<Cliente> BuscaPorCpf(string cpf)
        {
            return Ok(_repository.BuscarPorCpf(cpf));  
        }

        [HttpPut("alteracao-endereco/{id}")]
        public ActionResult AlteracaoEndereco(int id,[FromBody] Endereco endereco)
        {
           _repository.AtualizaEndereco(id,endereco);
            return Accepted();
        }

        [HttpPut("alteracao-cliente/{cpf}")]
        public ActionResult AlteracaoCliente(string cpf,[FromBody] ClienteAlternativo cliente)
        {
            _repository.AtualizaCliente(cpf,cliente);
            return Accepted();
        }

        [HttpPut("alteracao-telefone/{id}")]
        public ActionResult AlteracaoTelefone(int id,[FromBody] Telefone telefone)
        {
            _repository.AtualizaTelefone(id,telefone);
            return Accepted();
        }

        [HttpDelete("deletar-cadastro/{cpf}")]
        public ActionResult Deletar(string cpf)
        {
            var cliente = _repository.BuscarPorCpf(cpf);
            _repository.DeletarTelefone(cliente.Telefone);
            _repository.DeletarCliente(cliente);
            _repository.DeletarEndereco(cliente.Endereco);
            return Accepted();
        }
    }
}
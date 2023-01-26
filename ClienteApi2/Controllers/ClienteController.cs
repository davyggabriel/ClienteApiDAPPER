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
        public static List<Cliente> clientes = new List<Cliente>();
        private string _connectionString;
        public ClienteController(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:ClienteApi").Value;
        }

        [HttpPost("cadastrar")]
        public ActionResult<Cliente> Cadastrar([FromBody] Cliente cliente)
        {
            cliente.IdEndereco = cliente.Endereco.Id = RepositorioCliente.InserirEndereco(_connectionString, cliente);
            cliente.Id = RepositorioCliente.InserirCliente(_connectionString, cliente);
            cliente.Telefone.Id = RepositorioCliente.InserirTelefone(_connectionString, cliente);   
           
            return Created("", cliente);
        }

        [HttpGet("listar")]
        public IActionResult Listar()
        {
            var cliente = RepositorioCliente.Selecionar(_connectionString, "select cliente.*, telefone.id idTelefone , telefone.numero numeroTelefone, telefone.idCliente, endereco.id enderecoId, endereco.rua, endereco.numero, endereco.bairo, endereco.complemento, endereco.cidade, endereco.estado, endereco.pais \r\nfrom cliente\r\njoin telefone\r\non cliente.id = telefone.idcliente\r\njoin endereco\r\non endereco.id = cliente.idendereco", null);
            return Ok(cliente);
        }

        [HttpGet("busca-por-cpf/{cpf}")]
        public ActionResult<Cliente> BuscaPorCpf(string cpf)
        {
            var parametros = new List<SqlParameter>() {
                new SqlParameter("@cpf", cpf)
            };
            var cliente = RepositorioCliente.Selecionar(_connectionString, "select cliente.*, telefone.id idTelefone , telefone.numero numeroTelefone, telefone.idCliente, endereco.id enderecoID, endereco.rua, endereco.numero, endereco.bairo, endereco.complemento, endereco.cidade, endereco.estado, endereco.pais \r\nfrom cliente\r\njoin telefone\r\non cliente.id = telefone.idcliente\r\njoin endereco\r\non endereco.id = cliente.idendereco where cpf = @cpf", parametros);

            return cliente.FirstOrDefault();
        }

        [HttpPut("alteracao-cadastro/{id}")]
        public void AlteracaoDeCadastro(int id, [FromBody] Cliente cliente)
        {
            var parametros = new List<SqlParameter>() {
                new SqlParameter("@id", id),
                new SqlParameter("@rua",cliente.Endereco.Rua),
                new SqlParameter("@numero",cliente.Endereco.Numero),
                new SqlParameter("@complemento",cliente.Endereco.Complemento),
                new SqlParameter("@bairo",cliente.Endereco.Bairro),
                new SqlParameter("@cidade",cliente.Endereco.Cidade),
                new SqlParameter("@estado",cliente.Endereco.Estado),
                new SqlParameter("@pais",cliente.Endereco.Pais),
            };
            var sql = @"update endereco set rua = @rua where id = @id
                    update endereco set numero = @numero where id = @id
                    update endereco set complemento = @complemento where id = @id
                    update endereco set bairo = @bairo where id = @id
                    update endereco set cidade = @cidade where id = @id
                    update endereco set estado = @estado where id = @id
                    update endereco set pais = @pais where id = @id";
            RepositorioCliente.Executar(_connectionString, sql, parametros);

            parametros = new List<SqlParameter>() {
                new SqlParameter("@id", id),
                new SqlParameter("@cpf", cliente.Cpf),
                new SqlParameter("@nome",cliente.Nome),
                new SqlParameter("@filiacao",cliente.Filiacao),
            };
            sql = @"update cliente set cpf = @cpf where id = @id
                    update cliente set nome = @nome where id = @id
                    update cliente set filiacao = @filiacao where id = @id";
            RepositorioCliente.Executar(_connectionString, sql, parametros);

            parametros = new List<SqlParameter>() {
                new SqlParameter("@id", id),
                new SqlParameter("@numero",cliente.Telefone.Numero),
            };
            sql = @"update telefone set numero = @numero where id = @id";
            RepositorioCliente.Executar(_connectionString, sql, parametros);
        }

        [HttpDelete("deletar-cadastro/{cpf}")]
        public void Deletar(string cpf)
        {
            var parametros = new List<SqlParameter>() {
                new SqlParameter("@cpf", cpf)
            };
            var cliente = RepositorioCliente.Selecionar(_connectionString, "select cliente.*, telefone.id idTelefone , telefone.numero numeroTelefone, telefone.idCliente, endereco.id enderecoID, endereco.rua, endereco.numero, endereco.bairo, endereco.complemento, endereco.cidade, endereco.estado, endereco.pais \r\nfrom cliente\r\njoin telefone\r\non cliente.id = telefone.idcliente\r\njoin endereco\r\non endereco.id = cliente.idendereco where cpf = @cpf", parametros);
            parametros = new List<SqlParameter>() {
                new SqlParameter("@idTelefone", cliente.FirstOrDefault().Telefone.Id)
            };
            var sql = "delete from telefone where Id = @idTelefone";
            RepositorioCliente.Executar(_connectionString, sql, parametros);
            parametros = new List<SqlParameter>() {
                new SqlParameter("@idCliente", cliente.FirstOrDefault().Id)
            };
            sql = "delete from cliente where Id = @idCliente";
            RepositorioCliente.Executar(_connectionString, sql, parametros);
            parametros = new List<SqlParameter>() {
                new SqlParameter("@idEndereco",cliente.FirstOrDefault().Endereco.Id)
            };
            sql = "delete from endereco where Id = @idEndereco";
            RepositorioCliente.Executar(_connectionString, sql, parametros);         
        }
    }
}
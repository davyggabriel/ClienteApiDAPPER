using ClienteApi2.Models;
using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace ClienteApi2.Repositorio
{
    public class RepositorioClienteDapper
    {
        public string _connectionString;
        public RepositorioClienteDapper(IConfiguration configuration)
        {
            _connectionString = configuration.GetSection("ConnectionStrings:ClienteApi").Value;
        }

        public int BuscarId(string tabela)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = connection.QueryFirst<int?>(Scripts.BuscarMaiorId(tabela));
                if (result == null)
                {
                    return 1;
                }
                return (int)result + 1;
            }
        }

        public int InserirEndereco(Endereco endereco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                endereco.Id = BuscarId("Endereco");
                var result = connection.Execute(Scripts.InserirEndereco(), endereco);

                return endereco.Id;
            }
        }

        public int InserirCliente(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                cliente.Id = BuscarId("Cliente");
                var result = connection.Execute(Scripts.InserirCliente(), cliente);

                return cliente.Id;
            }
        }

        public int InserirTelefone(Telefone telefone, int idCliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                telefone.Id = BuscarId("Telefone");
                var result = connection.Execute(Scripts.InserirTelefone(), new { id = telefone.Id, numero = telefone.Numero, idCliente = idCliente });

                return telefone.Id;
            }
        }

        public List<Cliente> ListarTodos()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var clientes = connection.Query<Cliente, Telefone, Endereco, Cliente>(
                    Scripts.BuscarCliente(),
                    (cliente, telefone, endereco) =>
                    {
                        cliente.Endereco = endereco;
                        cliente.Telefone = telefone;
                        return cliente;
                    },
                    splitOn: "id,id"
                );
                return clientes.ToList();
            }
        }

        public Cliente BuscarPorCpf(string cpf)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var sql = $"{Scripts.BuscarCliente()} where cpf = @cpf";
                var clientes = connection.Query<Cliente, Telefone, Endereco, Cliente>(
                    sql,
                    (cliente, telefone, endereco) =>
                    {
                        cliente.Endereco = endereco;
                        cliente.Telefone = telefone;
                        return cliente;
                    },
                    new { cpf = cpf },
                    splitOn: "id,id"
                );
                return clientes.FirstOrDefault();
            }
        }

        public void DeletarCliente(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.DeletarCliente(), cliente);
            }
        }

        public void DeletarTelefone(Telefone telefone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.DeletarTelefone(), telefone);
            }
        }

        public void DeletarEndereco(Endereco endereco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.DeletarEndereco(), endereco);
            }
        }

        public void AtualizaCliente(string cpf,ClienteAlternativo cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.AtualizaCliente(), 
                new { Nome = cliente.Nome,
                      Cpf = cliente.Cpf, 
                      Filiacao = cliente.Filiacao, 
                      CpfAntigo = cpf 
                });
            }
        }

        public void AtualizaTelefone(int id,Telefone telefone)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.AtualizaTelefone(), 
                new { Id = id, 
                      Numero = telefone.Numero
                });
            }
        }

        public void AtualizaEndereco(int id,Endereco endereco)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(Scripts.AtualizaEndereco(), 
                new { Id = id, 
                      Rua = endereco.Rua, 
                      Numero = endereco.Numero, 
                      Bairro = endereco.Bairro,
                      complemento = endereco.Complemento,
                      Cidade = endereco.Cidade,
                      Estado = endereco.Estado,
                      Pais = endereco.Pais
                });
            }
        }
    }
}

using ClienteApi2.Models;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace ClienteApi2.Repositorio
{
    public class RepositorioCliente
    {
        public static SqlConnection Conectar(string connectionString)
        { 
            var sqlConnection = new SqlConnection(connectionString);
            if (sqlConnection.State != ConnectionState.Open)
                sqlConnection.Open();

            return sqlConnection;
        }

        public static void Executar(string connectionString, string sql, IEnumerable<SqlParameter> parametros)
        {
            using (var connection = Conectar(connectionString))
            {
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;

                if (parametros != null)
                    command.Parameters.AddRange(parametros.ToArray());

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static int BuscarId(string connectionString,string tabela)
        {
            using (var connection = Conectar(connectionString))
            {
                var id = 0;
                var command = new SqlCommand($"select max(id) id from {tabela}", connection);
                command.CommandType = CommandType.Text;

                var dados = command.ExecuteReader();
                while (dados.Read())
                {
                    var retorno = dados["id"];
                    if(!dados.IsDBNull(0))
                        id = Convert.ToInt32(retorno);
                }
                connection.Close();
                return id+1;
            }
        }

        public static List<Cliente> Selecionar(string connectionString,string sql, IEnumerable<SqlParameter> parametros)
        {
            using (var connection = Conectar(connectionString))
            {
                var clientes = new List<Cliente>();
                var command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.Text;

                if (parametros != null)
                    command.Parameters.AddRange(parametros.ToArray());

                var dados = command.ExecuteReader();
                while (dados.Read())
                {
                    var telefone = new Telefone(Convert.ToInt32(dados["idTelefone"]), dados["numero"].ToString());
                    var endereco = new Endereco(
                        Convert.ToInt32(dados["enderecoId"]),
                        dados["rua"].ToString(),
                        Convert.ToInt32(dados["numero"]),
                        dados["bairo"].ToString(),
                        dados["complemento"].ToString(),
                        dados["cidade"].ToString(),
                        dados["estado"].ToString(),
                        dados["pais"].ToString());
                    var cliente = new Cliente(
                        Convert.ToInt32(dados["id"]),
                        dados["nome"].ToString(),
                        dados["cpf"].ToString(),
                        dados["filiacao"].ToString(),
                        endereco,
                        telefone);
                    clientes.Add(cliente);
                }
                connection.Close();
                return clientes;
            }
        }

        public static int InserirEndereco(string connectionString, Cliente cliente)
        {
            var idEndereco = RepositorioCliente.BuscarId(connectionString, "endereco");
            var sql = @"INSERT INTO [dbo].[Endereco]
           ([Id]
           ,[Rua]
           ,[Numero]
           ,[Bairo]
           ,[Complemento]
           ,[Cidade]
           ,[Estado]
           ,[Pais])
     VALUES(
            @id,
            @rua,
            @numero,
            @bairro,
            @complemento,
            @cidade,
            @estado,
            @pais)";
            var parametros = new List<SqlParameter>() {
                new SqlParameter("@id", idEndereco),
                new SqlParameter("@rua", cliente.Endereco.Rua),
                new SqlParameter("@numero", cliente.Endereco.Numero),
                new SqlParameter("@bairro", cliente.Endereco.Bairro),
                new SqlParameter("@complemento", cliente.Endereco.Complemento),
                new SqlParameter("@cidade", cliente.Endereco.Cidade),
                new SqlParameter("@estado", cliente.Endereco.Estado),
                new SqlParameter("@pais", cliente.Endereco.Pais),
            };
            RepositorioCliente.Executar(connectionString, sql, parametros);
            return idEndereco;
        }

        public static int InserirCliente(string connectionString, Cliente cliente)
        {
            var idCliente = RepositorioCliente.BuscarId(connectionString, "cliente");
            var sql2 = @"INSERT INTO[dbo].[Cliente]
           ([Id]
           ,[Nome]
           ,[Cpf]
           ,[IdEndereco]
           ,[Filiacao])
     VALUES(
           @id,
           @nome,
           @cpf,
           @idEndereco,
           @filiacao)";

            var parametros2 = new List<SqlParameter>()
           {
               new SqlParameter("id",idCliente),
               new SqlParameter("nome",cliente.Nome),
               new SqlParameter("cpf", cliente.Cpf),
               new SqlParameter("idEndereco",cliente.Endereco.Id),
               new SqlParameter("filiacao",cliente.Filiacao)
           };
            RepositorioCliente.Executar(connectionString, sql2, parametros2);
            return idCliente;   
        }

        public static int InserirTelefone(string connectionString, Cliente cliente)
        {
            var idTelefone = RepositorioCliente.BuscarId(connectionString, "telefone");
            var sql3 = @"INSERT INTO[dbo].[Telefone]
           ([Id]
           ,[IdCliente]
           ,[Numero])
     VALUES(
           @id,
           @idCliente,
           @numero)";

            var parametros3 = new List<SqlParameter>()
            {
                new SqlParameter("id",idTelefone),
                new SqlParameter("idCliente",cliente.Id),
                new SqlParameter("numero",cliente.Telefone.Numero)
            };
            RepositorioCliente.Executar(connectionString, sql3, parametros3);
            return idTelefone;
        }
    }
}

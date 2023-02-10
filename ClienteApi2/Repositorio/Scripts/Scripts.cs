using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ClienteApi2.Repositorio
{
    public class Scripts
    {
        public static string BuscarMaiorId(string tabela)
        {
            return $"select max(id) id from {tabela}";
        }

        public static string BuscarCliente()
        {
            return @"select 
                cliente.*, 
                telefone.id, telefone.numero, telefone.idCliente, 
                endereco.id, endereco.rua, endereco.numero, endereco.bairro, endereco.complemento, 
                endereco.cidade, endereco.estado, endereco.pais 
            from cliente
            join telefone
                on cliente.id = telefone.idcliente
            join endereco
                on endereco.id = cliente.idendereco";
        }

        public static string InserirEndereco()
        {
            return @"INSERT INTO [dbo].[Endereco]
           ([Id]
           ,[Rua]
           ,[Numero]
           ,[bairro]
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
        }

        public static string InserirCliente()
        {
            return @"INSERT INTO[dbo].[Cliente]
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
        }

        public static string InserirTelefone()
        {
           return @"INSERT INTO[dbo].[Telefone]
           ([Id]
           ,[IdCliente]
           ,[Numero])
     VALUES(
           @id,
           @idCliente,
           @numero)";
        }

        public static string AtualizaEndereco()
        {
            return @"update endereco set rua = @rua where id = @id
                     update endereco set numero = @numero where id = @id
                     update endereco set complemento = @complemento where id = @id
                     update endereco set bairro = @bairro where id = @id
                     update endereco set cidade = @cidade where id = @id
                     update endereco set estado = @estado where id = @id
                     update endereco set pais = @pais where id = @id";
        }

        public static string AtualizaCliente()
        {
            return @"update cliente set nome = @nome where cpf = @cpfAntigo
                     update cliente set filiacao = @filiacao where cpf = @cpfAntigo               
                     update cliente set cpf = @cpf where cpf = @cpfAntigo";            
        }

        public static string AtualizaTelefone()
        {
            return @"update telefone set numero = @numero where id = @id";
        }

        public static string DeletarTelefone()
        {
            return "delete from telefone where id = @id";
        }

        public static string DeletarCliente()
        {
           return "delete from cliente where id = @id";
        }

        public static string DeletarEndereco()
        {
            return "delete from endereco where id = @id";
        }
    }
}

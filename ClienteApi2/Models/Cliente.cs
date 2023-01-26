using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;

namespace ClienteApi2.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Nome { get; set; }

        [MinLength(11, ErrorMessage = "Cpf mínimo 11 caracteres")]
        [MaxLength(11, ErrorMessage = "Cpf máximo 11 caracteres")]
        public string Cpf { get; set; }

        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Filiacao { get; set; }
        public int IdEndereco { get; set; }

        public Endereco Endereco { get; set; }

        public Telefone Telefone { get; set; }

        public Cliente(int id, string nome, string cpf, string filiacao, Endereco endereco, Telefone telefone)
        {
            Id = id;
            Nome = nome;
            Cpf = cpf;
            Filiacao = filiacao;
            Endereco = endereco;
            Telefone = telefone;
        }
    }
}
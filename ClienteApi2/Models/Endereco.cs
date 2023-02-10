using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClienteApi2.Models
{
    public class Endereco
    {
        public Endereco(int id, string rua, int numero, string bairro, string complemento, string cidade, string estado, string pais)
        {
            Id = id;
            Rua = rua;
            Numero = numero;
            Bairro = bairro;
            Complemento = complemento;
            Cidade = cidade;
            Estado = estado;
            Pais = pais;
        }
        public Endereco()
        {

        }

        public int Id { get; set; }

        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Rua { get; set; }
        public int Numero { get; set; }

        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Bairro { get; set; }

        [MaxLength(255, ErrorMessage = "Máximo 255 caracteres")]
        public string Complemento { get; set; }
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Cidade { get; set; }
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Estado { get; set; }
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres")]
        public string Pais { get; set; }   
    }
}
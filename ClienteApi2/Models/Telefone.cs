using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ClienteApi2.Models
{
    public class Telefone
    {
        public int Id { get; set; }
        [MaxLength(15, ErrorMessage = "Máximo 15 caracteres")]
        public string Numero { get; set; } 
        public Telefone(int id, string numero)
        {
            Id = id;
            Numero = numero;
        }    
        public Telefone()
        {

        }
    }
}
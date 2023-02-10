using System.Globalization;
using System.Text.Json.Serialization;

namespace ClienteApi2.Models
{
    public class ClienteAlternativo 
    {   
        public string Nome { get; set; }   
        public string Cpf { get; set; } 
        public string Filiacao { get; set; }    
    }
}

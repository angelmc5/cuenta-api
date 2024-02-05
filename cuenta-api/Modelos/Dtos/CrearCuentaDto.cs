using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos.Dtos
{
    public class CrearCuentaDto
    {
     
        [Required(ErrorMessage = "El número de cuenta es obligatorio")]
        [MaxLength(10, ErrorMessage = "El número máximo de caracteres es 10")]
        public string Numero { get; set; }

        public string Tipo { get; set; }

        public float Saldo { get; set; }

        public bool Estado { get; set; }
        public int ClienteId { get; set; }
    }
}

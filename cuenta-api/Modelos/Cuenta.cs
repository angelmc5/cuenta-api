using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos
{
    public class Cuenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Numero { get; set; }

        [Required]
        public string Tipo { get; set; }

        [Required]
        public float Saldo { get; set; }

        [Required]
        public bool Estado { get; set; }

        [Required]
        public int ClienteId { get; set; }

    }
}

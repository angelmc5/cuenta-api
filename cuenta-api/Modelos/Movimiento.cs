using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace cuenta_api.Modelos
{
    public class Movimiento
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }

        public Double Valor { get; set; }
        public Double Saldo { get; set; }

        [ForeignKey("cuentaId")]
        public int cuentaId { get; set; }
        public Cuenta Cuenta { get; set; }
    }
}

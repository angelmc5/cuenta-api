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

        public enum TipoMovimientos
        {
            Deposito,
            Retiro
        }

        public TipoMovimientos TipoMovimiento { get; set; }

        public float Valor { get; set; }
        public float Saldo { get; set; }

        [ForeignKey("cuentaId")]
        public int cuentaId { get; set; }
        public Cuenta Cuenta { get; set; }
    }
}

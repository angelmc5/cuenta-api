using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos.Dtos
{
    public class MovimientoDto
    {
        public int Id { get; set; }

        public DateTime Fecha { get; set; }

        public enum TipoMovimientos
        {
            Deposito,
            Retiro
        }

        public TipoMovimientos TipoMovimiento { get; set; }

        public float Valor { get; set; }

        public float Saldo { get; set; }

        public int cuentaId { get; set; }
    }
}

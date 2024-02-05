using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos.Dtos
{
    public class CrearMovimientoDto
    {

        public DateTime Fecha { get; set; }
        public string TipoMovimiento { get; set; }

        public Double Valor { get; set; }

        public Double Saldo { get; set; }

        public int cuentaId { get; set; }
    }
}

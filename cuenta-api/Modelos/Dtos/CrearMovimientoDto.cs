using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos.Dtos
{
    public class CrearMovimientoDto
    {

        public enum TipoMovimientosCrear
        {
            Deposito,
            Retiro
        }

        [Required(ErrorMessage = "El tipo del movimiento es obligatorio")]
        public TipoMovimientosCrear TipoMovimiento { get; set; }

        [Required(ErrorMessage = "El valor del movimiento es obligatorio")]
        public float Valor { get; set; }

        [Required(ErrorMessage = "La cuenta del movimiento es obligatoria")]
        public int cuentaId { get; set; }
    }
}

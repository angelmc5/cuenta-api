using System.ComponentModel.DataAnnotations;

namespace cuenta_api.Modelos
{
    public class Reporte
    {
        [Key]
        public int Id { get; set; }
        public string Fecha { get; set; }

        public String Cliente { get; set; }

        public String NumeroCuenta { get; set; }

        public String Tipo { get; set; }

        public float SaldoInicial { get; set; }

        public bool Estado { get; set; }

        public float Movimiento { get; set; }

        public float SaldoDisponible { get; set; }
    }
}

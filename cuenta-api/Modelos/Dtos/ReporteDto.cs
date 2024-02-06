namespace cuenta_api.Modelos.Dtos
{
    public class ReporteDto
    {
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

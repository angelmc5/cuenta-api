using cuenta_api.Data;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;
using cuenta_api.Repositorio.IRepositorio;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace cuenta_api.Repositorio
{
    public class ReporteRepositorio : IReporteRepositorio
    {
        private readonly ApplicationDbContext _bdd;
        public ReporteRepositorio(ApplicationDbContext bdd)
        {
            _bdd = bdd;
        }
        public ICollection<Reporte> GetReporte(DateTime inicioFecha, DateTime finFecha, string cuenta)
        {
            var sqlQuery = "Select c.id, convert(varchar,  m.Fecha, 103) as Fecha, p.Nombre as Cliente, c.Numero as NumeroCuenta, c.Tipo, m.Saldo as SaldoInicial, c.Estado, m.Valor as Movimiento, (m.Saldo + m.Valor) as SaldoDisponible from Persona p inner join cuenta c on c.ClienteId=p.Id inner join Movimiento m on c.Id=m.cuentaId where m.Fecha >= @startDate and m.Fecha <= @endDate and c.numero = @numeroCta";

            var startDateParam = new SqlParameter("@startDate", inicioFecha);
            var endDateParam = new SqlParameter("@endDate", finFecha);
            var numeroCtaParam = new SqlParameter("@numeroCta", cuenta);

            return _bdd.Reporte.FromSqlRaw(sqlQuery, startDateParam, endDateParam, numeroCtaParam).ToList();

        }
    }
}

using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;

namespace cuenta_api.Repositorio.IRepositorio
{
    public interface IReporteRepositorio
    {
        ICollection<Reporte> GetReporte(DateTime inicioFecha, DateTime finFecha, string cuenta);
    }
}

using cuenta_api.Modelos;

namespace cuenta_api.Repositorio.IRepositorio
{
    public interface IMovimientoRepositorio
    {
        ICollection<Movimiento> GetMovimientos();
        Movimiento GetMovimiento(int id);
        bool ExisteMovimiento(DateTime fecha);
        bool ExisteMovimiento(int id);
        bool CrearMovimiento (Movimiento movimiento);
        bool ActualizarMovimiento(Movimiento movimiento);
        bool BorrarMovimiento(Movimiento movimiento);
        bool Guardar();
    }
}

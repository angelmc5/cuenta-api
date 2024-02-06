using cuenta_api.Data;
using cuenta_api.Modelos;
using cuenta_api.Repositorio.IRepositorio;

namespace cuenta_api.Repositorio
{
    public class MovimientoRepositorio : IMovimientoRepositorio
    {
        private readonly ApplicationDbContext _bdd;
        public MovimientoRepositorio(ApplicationDbContext bdd)
        {
            _bdd = bdd;
        }

        public bool ActualizarMovimiento(Movimiento movimiento)
        {
            _bdd.Movimiento.Update(movimiento);
            return Guardar();
        }

        public bool BorrarMovimiento(Movimiento movimiento)
        {
            _bdd.Movimiento.Remove(movimiento);
            return Guardar();
        }

        public bool CrearMovimiento(Movimiento movimiento)
        {
            movimiento.Fecha = DateTime.Now;
            _bdd.Movimiento.Add(movimiento);
            return Guardar();
        }

        public bool ExisteMovimiento(DateTime fecha)
        {
            bool valor = _bdd.Movimiento.Any(c => c.Fecha == fecha);
            return valor;
        }

        public bool ExisteMovimiento(int id)
        {
            return _bdd.Movimiento.Any(c => c.Id == id);
        }

        public Movimiento GetMovimiento(int id)
        {
            return _bdd.Movimiento.FirstOrDefault(c => c.Id == id);
        }

        public ICollection<Movimiento> GetMovimientos()
        {
            return _bdd.Movimiento.OrderBy(c => c.cuentaId).ToList();
        }

        public bool Guardar()
        {
            try
            {
                return _bdd.SaveChanges() >= 0 ? true : false;
            }
            catch (Exception ex)
            {
                //guardar excepcion en logs
                return false;
            }
        }
    }
}

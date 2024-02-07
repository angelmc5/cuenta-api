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
            try
            {
                _bdd.Movimiento.Update(movimiento);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool BorrarMovimiento(Movimiento movimiento)
        {
            try
            {
                _bdd.Movimiento.Remove(movimiento);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool CrearMovimiento(Movimiento movimiento)
        {
            try
            {
                movimiento.Fecha = DateTime.Now;
                _bdd.Movimiento.Add(movimiento);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool ExisteMovimiento(DateTime fecha)
        {
            try
            {
                bool valor = _bdd.Movimiento.Any(c => c.Fecha == fecha);
                return valor;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool ExisteMovimiento(int id)
        {
            try
            {
                return _bdd.Movimiento.Any(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public Movimiento GetMovimiento(int id)
        {
            try
            {
                return _bdd.Movimiento.FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public ICollection<Movimiento> GetMovimientos()
        {
            try
            {
                return _bdd.Movimiento.OrderBy(c => c.cuentaId).ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool Guardar()
        {
            try
            {
                return _bdd.SaveChanges() >= 0 ? true : false;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }
    }
}

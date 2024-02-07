using cuenta_api.Data;
using cuenta_api.Modelos;
using cuenta_api.Repositorio.IRepositorio;

namespace cuenta_api.Repositorio
{
    public class CuentaRepositorio : ICuentaRepositorio
    {
        private readonly ApplicationDbContext _bdd;

        public CuentaRepositorio(ApplicationDbContext bdd)
        {
            _bdd = bdd;
        }

        public bool ActualizarCuenta(Cuenta cuenta)
        {
            try
            {
                _bdd.Cuenta.Update(cuenta);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool BorrarCuenta(Cuenta cuenta)
        {
            try
            {

                _bdd.Cuenta.Remove(cuenta);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool CrearCuenta(Cuenta cuenta)
        {
            try
            {
                cuenta.Numero = cuenta.Numero.Trim().ToString();
                _bdd.Cuenta.Add(cuenta);
                return Guardar();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool ExisteCuenta(string cuenta)
        {
            try
            {
                bool valor = _bdd.Cuenta.Any(c => c.Numero.ToLower().Trim() == cuenta.ToLower().Trim());
                return valor;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool ExisteCuenta(string cuenta,int id)
        {
            try
            {
                bool valor = _bdd.Cuenta.Any(c => c.Numero.ToLower().Trim() == cuenta.ToLower().Trim() && c.Id != id);
                return valor;
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public bool ExisteCuenta(int id)
        {
            try
            {
                return _bdd.Cuenta.Any(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public ICollection<Cuenta> GetCuentas()
        {
            try
            {
                return _bdd.Cuenta.OrderBy(c => c.Numero).ToList();
            }
            catch (Exception ex)
            {
                throw new RepositoryException($"An error occurred while retrieving the entity: {ex.Message}");
            }
        }

        public Cuenta GetCuenta(int id)
        {
            try
            {
                return _bdd.Cuenta.FirstOrDefault(c => c.Id == id);
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

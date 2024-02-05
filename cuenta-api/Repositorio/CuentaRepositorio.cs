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
            _bdd.Cuenta.Update(cuenta);
            return Guardar();
        }

        public bool BorrarCuenta(Cuenta cuenta)
        {
            _bdd.Cuenta.Remove(cuenta);
            return Guardar();
        }

        public bool CrearCuenta(Cuenta cuenta)
        {
            cuenta.Numero = cuenta.Numero.Trim().ToString();
            _bdd.Cuenta.Add(cuenta);
            return Guardar();
        }

        public bool ExisteCuenta(string cuenta)
        {
            bool valor = _bdd.Cuenta.Any(c => c.Numero.ToLower().Trim() == cuenta.ToLower().Trim());
            return valor;
        }

        public bool ExisteCuenta(string cuenta,int id)
        {
            bool valor = _bdd.Cuenta.Any(c => c.Numero.ToLower().Trim() == cuenta.ToLower().Trim() && c.Id != id);
            return valor;
        }

        public bool ExisteCuenta(int id)
        {
            return _bdd.Cuenta.Any(c => c.Id == id);
        }

        public ICollection<Cuenta> GetCuentas()
        {
            return _bdd.Cuenta.OrderBy(c => c.Numero).ToList();
        }

        public Cuenta GetCuenta(int id)
        {
            return _bdd.Cuenta.FirstOrDefault(c => c.Id == id);
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

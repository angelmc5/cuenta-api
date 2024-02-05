using cuenta_api.Modelos;

namespace cuenta_api.Repositorio.IRepositorio
{
    public interface ICuentaRepositorio
    {
        ICollection<Cuenta> GetCuentas();
        Cuenta GetCuenta(int id);
        bool ExisteCuenta(string cuenta);
        bool ExisteCuenta(string cuenta, int id);
        bool ExisteCuenta(int id);
        bool CrearCuenta(Cuenta cuenta);
        bool ActualizarCuenta(Cuenta cuenta);
        bool BorrarCuenta(Cuenta cuenta);
        bool Guardar();
    }
}

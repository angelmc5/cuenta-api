using cuenta_api.Modelos;
using Microsoft.EntityFrameworkCore;

namespace cuenta_api.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        //Agregar los modelos del api, a fin de generarlos a nivel de base de datos y mapear la tabla con la clase
        public DbSet<Cuenta> Cuenta { get; set; }
        public DbSet<Movimiento> Movimiento { get; set; }
        public DbSet<Reporte> Reporte { get; set; }
    }
}

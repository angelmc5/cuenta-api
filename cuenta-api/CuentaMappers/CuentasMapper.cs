using AutoMapper;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;

namespace cuenta_api.CuentaMappers
{
    public class CuentasMapper : Profile
    {
        public CuentasMapper()
        {
            CreateMap<Cuenta, CuentaDto>().ReverseMap();
            CreateMap<Cuenta, CrearCuentaDto>().ReverseMap();
            CreateMap<Movimiento, MovimientoDto>().ReverseMap();
            CreateMap<Movimiento, CrearMovimientoDto>().ReverseMap();
        }
    }
}

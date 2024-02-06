using AutoMapper;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;
using cuenta_api.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cuenta_api.Controllers
{
    [Route("api/cuentas")]
    [ApiController]
    public class CuentasController : ControllerBase
    {
        private readonly ICuentaRepositorio _ctaRepo;
        private readonly IMapper _mapper;

        public CuentasController(ICuentaRepositorio ctaRepo, IMapper mapper)
        {
            _ctaRepo = ctaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCuentas()
        {
            var listaCuentas = _ctaRepo.GetCuentas();

            var listaCuentasDto = new List<CuentaDto>();

            foreach (var item in listaCuentas)
            {
                listaCuentasDto.Add(_mapper.Map<CuentaDto>(item));
            }
            return Ok(listaCuentasDto);
        }

        [HttpGet("{id:int}", Name = "GetCuenta")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetCuenta(int id)
        {
            var itemCuenta = _ctaRepo.GetCuenta(id);

            if (itemCuenta == null)
            {
                return NotFound();
            }
            var itemCuentaDto = _mapper.Map<CuentaDto>(itemCuenta);
            return Ok(itemCuentaDto);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCuenta([FromBody] CrearCuentaDto crearCuentaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (crearCuentaDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_ctaRepo.ExisteCuenta(crearCuentaDto.Numero))
            {
                ModelState.AddModelError("", "La cuenta ya existe");
                return StatusCode(404, ModelState);
            }
            var cuenta = _mapper.Map<Cuenta>(crearCuentaDto);
            if (!_ctaRepo.CrearCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Error guardando el registro {cuenta.Numero}!");
                return StatusCode(500, ModelState);

            }
            return CreatedAtRoute("GetCuenta", new { id = cuenta.Id }, cuenta);
        }

        [HttpPatch("{id:int}", Name = "ActualizarPatchCuenta")]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarPatchCuenta(int id, [FromBody] CuentaDto cuentaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (cuentaDto == null || id != cuentaDto.Id)
            {
                return BadRequest(ModelState);
            }

            if (_ctaRepo.ExisteCuenta(cuentaDto.Numero,cuentaDto.Id))
            {
                ModelState.AddModelError("", "La cuenta ya existe");
                return StatusCode(404, ModelState);
            }

            var cuenta = _mapper.Map<Cuenta>(cuentaDto);
            if (!_ctaRepo.ActualizarCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Error actualizando el registro {cuenta.Numero}!");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }

        [HttpDelete("{id:int}", Name = "EliminarCuenta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EliminarCuenta(int id)
        {
            if (!_ctaRepo.ExisteCuenta(id))
            {
                return NotFound();
            }

            var cuenta = _ctaRepo.GetCuenta(id);

            if (!_ctaRepo.BorrarCuenta(cuenta))
            {
                ModelState.AddModelError("", $"Error al eliminar el registro {cuenta.Numero}!");
                return StatusCode(500, ModelState);

            }
            return NoContent();
        }
    }
}

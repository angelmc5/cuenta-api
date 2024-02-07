using AutoMapper;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;
using cuenta_api.Repositorio;
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCuentas()
        {
            try
            {
                var listaCuentas = _ctaRepo.GetCuentas();
                var listaCuentasDto = new List<CuentaDto>();

                foreach (var item in listaCuentas)
                {
                    listaCuentasDto.Add(_mapper.Map<CuentaDto>(item));
                }
                return Ok(listaCuentasDto);
            }
            catch (RepositoryException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id:int}", Name = "GetCuenta")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetCuenta(int id)
        {
            try
            {
                var itemCuenta = _ctaRepo.GetCuenta(id);

                if (itemCuenta == null)
                //if (itemCuenta.Equals(null))
                {
                    return NotFound();
                }
                var itemCuentaDto = _mapper.Map<CuentaDto>(itemCuenta);
                return Ok(itemCuentaDto);
            }
            catch (RepositoryException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCuenta([FromBody] CrearCuentaDto crearCuentaDto)
        {
            try
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
            catch (RepositoryException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpPatch("{id:int}", Name = "ActualizarPatchCuenta")]
        [ProducesResponseType(201, Type = typeof(CuentaDto))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarPatchCuenta(int id, [FromBody] CuentaDto cuentaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (cuentaDto == null || id != cuentaDto.Id)
                {
                    return BadRequest(ModelState);
                }

                if (_ctaRepo.ExisteCuenta(cuentaDto.Numero, cuentaDto.Id))
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
            catch (RepositoryException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }

        }

        [HttpDelete("{id:int}", Name = "EliminarCuenta")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EliminarCuenta(int id)
        {
            try
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
            catch (RepositoryException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Manejo de otras excepciones
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}

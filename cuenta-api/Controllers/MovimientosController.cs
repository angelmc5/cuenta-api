using AutoMapper;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;
using cuenta_api.Repositorio;
using cuenta_api.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Eventing.Reader;

namespace cuenta_api.Controllers
{
    [Route("api/movimientos")]
    [ApiController]
    public class MovimientosController : ControllerBase
    {
        private readonly IMovimientoRepositorio _movsRepo;
        private readonly ICuentaRepositorio _ctaRepo;
        private readonly IMapper _mapper;
        public MovimientosController(IMovimientoRepositorio movsRepo,ICuentaRepositorio ctaRepo, IMapper mapper)
        {
            _movsRepo = movsRepo;
            _ctaRepo = ctaRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMovimientos()
        {
            try
            {
                var listaMovimientos = _movsRepo.GetMovimientos();

                var listaMovimientosDto = new List<MovimientoDto>();

                foreach (var item in listaMovimientos)
                {
                    listaMovimientosDto.Add(_mapper.Map<MovimientoDto>(item));
                }
                return Ok(listaMovimientosDto);
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

        [HttpGet("{id:int}", Name = "GetMovimiento")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetMovimiento(int id)
        {
            try
            {
                var itemMovimiento = _movsRepo.GetMovimiento(id);

                if (itemMovimiento == null)
                {
                    return NotFound();
                }
                var itemMovientoDto = _mapper.Map<MovimientoDto>(itemMovimiento);
                return Ok(itemMovientoDto);
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
        [ProducesResponseType(201, Type = typeof(MovimientoDto))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearMovimiento([FromBody] CrearMovimientoDto crearMovimientoDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (crearMovimientoDto == null)
                {
                    return BadRequest(ModelState);
                }

                var movimiento = _mapper.Map<Movimiento>(crearMovimientoDto);

                //Validar que exista la cuenta para registrar el movimiento
                if (_ctaRepo.ExisteCuenta(crearMovimientoDto.cuentaId))
                {
                    //Obtener la cuenta
                    var cuenta = _ctaRepo.GetCuenta(crearMovimientoDto.cuentaId);

                    //Validar que la cuenta esté activa
                    if (!cuenta.Estado.Equals(true))
                    {
                        ModelState.AddModelError("", "La cuenta no está activa");
                        return StatusCode(404, ModelState);
                    }
                    else
                    {
                        //Registrar movimiento y actualizar cuenta
                        switch (crearMovimientoDto.TipoMovimiento)
                        {
                            case CrearMovimientoDto.TipoMovimientosCrear.Deposito:
                                //No validar saldo
                                movimiento.Saldo = cuenta.Saldo;
                                cuenta.Saldo = cuenta.Saldo + float.Abs(crearMovimientoDto.Valor);
                                break;
                            case CrearMovimientoDto.TipoMovimientosCrear.Retiro:
                                //Validar Saldo y actualizar saldo
                                if (cuenta.Saldo >= float.Abs(crearMovimientoDto.Valor))
                                {
                                    movimiento.Saldo = cuenta.Saldo;
                                    cuenta.Saldo = cuenta.Saldo - float.Abs(crearMovimientoDto.Valor);
                                    movimiento.Valor = (crearMovimientoDto.Valor < 0) ? crearMovimientoDto.Valor : crearMovimientoDto.Valor * -1;
                                }
                                else
                                {
                                    ModelState.AddModelError("", "Saldo no disponible");
                                    return StatusCode(404, ModelState);
                                }
                                break;
                            default:
                                ModelState.AddModelError("", "Tipo de movimiento invalido");
                                return StatusCode(404, ModelState);

                        }
                    }
                    if (!_ctaRepo.ActualizarCuenta(cuenta))
                    {
                        ModelState.AddModelError("", $"Error actualizando el registro {cuenta.Numero}!");
                        return StatusCode(500, ModelState);
                    }

                    if (!_movsRepo.CrearMovimiento(movimiento))
                    {
                        ModelState.AddModelError("", $"Error al crear registro {movimiento.TipoMovimiento}!");
                        return StatusCode(500, ModelState);

                    }
                }
                else
                {
                    ModelState.AddModelError("", "La cuenta del movimiento no existe");
                    return StatusCode(404, ModelState);
                }

                return CreatedAtRoute("GetMovimiento", new { id = movimiento.Id }, movimiento);
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

        [HttpDelete("{id:int}", Name = "EliminarMovimiento")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult EliminarMovimiento(int id)
        {
            try
            {
                if (!_movsRepo.ExisteMovimiento(id))
                {
                    return NotFound();
                }

                var movimiento = _movsRepo.GetMovimiento(id);

                if (!_movsRepo.BorrarMovimiento(movimiento))
                {
                    ModelState.AddModelError("", $"Error al eliminar el registro {movimiento.Id} de la cuenta {movimiento.Cuenta.Numero}!");
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

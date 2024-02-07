using AutoMapper;
using cuenta_api.Data;
using cuenta_api.Modelos;
using cuenta_api.Modelos.Dtos;
using cuenta_api.Repositorio;
using cuenta_api.Repositorio.IRepositorio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cuenta_api.Controllers
{
    [Route("api/reportes")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteRepositorio _repoRepo;
        private readonly IMapper _mapper;
        public ReportesController(IReporteRepositorio repoRepo, IMapper mapper)
        {
            _repoRepo = repoRepo;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult GetReporte(DateTime inicioFecha, DateTime finFecha, string cuenta)
        {
            try
            {
                var listaReporte = _repoRepo.GetReporte(inicioFecha, finFecha, cuenta);
                var listaReporteDto = new List<ReporteDto>();

                foreach (var item in listaReporte)
                {
                    listaReporteDto.Add(_mapper.Map<ReporteDto>(item));
                }

                // Retorna el reporte como una respuesta HTTP en formato JSON.
                return Ok(listaReporteDto);
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

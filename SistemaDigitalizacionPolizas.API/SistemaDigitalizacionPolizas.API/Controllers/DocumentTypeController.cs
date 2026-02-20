using Microsoft.AspNetCore.Authorization;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.CreateDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Commands.UpdateStatusDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetAllDocumentType;
using SistemaDigitalizacionPolizas.Application.Services.DocumentType_Service.Queries.GetByNameDocumentType;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar las operaciones relacionadas
    /// con los Tipos de Documento dentro del sistema.
    /// 
    /// Este controlador sigue el patrón CQRS utilizando MediatR,
    /// delegando la lógica de negocio a la capa de Application.
    /// 
    /// Responsabilidades:
    /// - Crear tipos de documento
    /// - Consultar (paginado y por nombre)
    /// - Actualizar información
    /// - Activar / Desactivar (Soft status update)
    /// </summary>
    [Authorize] // Se puede habilitar para proteger los endpoints

    [ApiController]
    [Route("api/[controller]")]
    public class DocumentTypeController : ControllerBase
    {
        /// <summary>
        /// Mediador que permite desacoplar el controlador de la lógica
        /// de negocio (patrón CQRS).
        /// </summary>
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor del controlador.
        /// </summary>
        /// <param name="mediator">Instancia de MediatR inyectada por DI</param>
        public DocumentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // ============================================================
        // CREATE
        // ============================================================

        /// <summary>
        /// Crea un nuevo tipo de documento.
        /// </summary>
        /// <param name="command">Datos necesarios para crear el tipo de documento</param>
        /// <returns>
        /// 201 Created si se crea correctamente.
        /// 400 BadRequest si el modelo es inválido.
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] CreateDocumentTypeCommand command)
        {
            // Validación automática del modelo
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Se envía el comando al Handler correspondiente
            var id = await _mediator.Send(command);

            // Se retorna 201 Created (semánticamente correcto en REST)
            return Created(string.Empty, new
            {
                id,
                message = "Tipo de documento creado correctamente"
            });
        }

        // ============================================================
        // READ - PAGINADO
        // ============================================================

        /// <summary>
        /// Obtiene tipos de documento de forma paginada.
        /// </summary>
        /// <param name="pageNumber">Número de página (por defecto 1)</param>
        /// <param name="pageSize">Cantidad de registros por página (máx 100)</param>
        /// <returns>Lista paginada de tipos de documento</returns>
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            // Normalización de parámetros
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0 || pageSize > 100)
                pageSize = 10;

            var query = new GetPagedDocumentTypeQuery(pageNumber, pageSize);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        // ============================================================
        // READ - POR NOMBRE
        // ============================================================

        /// <summary>
        /// Obtiene un tipo de documento por su nombre.
        /// </summary>
        /// <param name="documentName">Nombre del documento</param>
        /// <returns>
        /// 200 OK si existe.
        /// 404 NotFound si no existe.
        /// 400 BadRequest si el parámetro es inválido.
        /// </returns>
        [HttpGet("by-name")]
        public async Task<IActionResult> GetByName(
            [FromQuery] string documentName)
        {
            if (string.IsNullOrWhiteSpace(documentName))
                return BadRequest("El nombre del documento es obligatorio.");

            var result = await _mediator.Send(
                new GetByNameDocumentTypeQuery(documentName));

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // ============================================================
        // UPDATE COMPLETO (PUT)
        // ============================================================

        /// <summary>
        /// Actualiza completamente un tipo de documento.
        /// </summary>
        /// <param name="id">Id del tipo de documento (desde la URL)</param>
        /// <param name="command">Datos actualizados</param>
        /// <returns>
        /// 204 NoContent si se actualiza correctamente.
        /// 400 BadRequest si el Id no coincide.
        /// 404 NotFound si no existe.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] UpdateDocumentTypeCommand command)
        {
            // Se valida que el Id de la URL sea la fuente de verdad
            if (id != command.IdDocumentType && command.IdDocumentType != 0)
                return BadRequest("El id no coincide.");

            // Se sobreescribe el Id del command
            command = command with { IdDocumentType = id };

            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // ============================================================
        // UPDATE PARCIAL (PATCH - ESTADO)
        // ============================================================

        /// <summary>
        /// Activa o desactiva un tipo de documento según su nombre.
        /// 
        /// Esta operación representa una actualización parcial
        /// (solo modifica la propiedad Active).
        /// </summary>
        /// <param name="documentName">Nombre del documento</param>
        /// <param name="active">Nuevo estado (true = activo, false = inactivo)</param>
        /// <returns>
        /// 204 NoContent si se actualiza correctamente.
        /// 404 NotFound si no existe.
        /// </returns>
        [HttpPatch("status/{documentName}")]
        public async Task<IActionResult> UpdateStatus(
            string documentName,
            [FromBody] bool active)
        {
            var result = await _mediator.Send(
                new UpdateStatusDocumentTypeCmd(documentName, active));

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}

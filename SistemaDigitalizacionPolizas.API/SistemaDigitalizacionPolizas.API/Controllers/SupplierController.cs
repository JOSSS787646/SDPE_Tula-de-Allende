using Microsoft.AspNetCore.Authorization;
using SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.CreateSupplier;
using SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplier;
using SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Commands.UpdateSupplierStatus;
using SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Queries.GetAllSuppliersQuery;
using SistemaDigitalizacionPolizas.Application.Services.Supplier_Service.Queries.GetSupplierByRfc;
using SistemaDigitalizacionPolizas.Domain.Dtos.Supplier;

namespace SistemaDigitalizacionPolizas.API.Controllers
{
    /// <summary>
    /// Gestiona proveedores (crear, consultar, actualizar y cambiar estatus).
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Crea un nuevo proveedor.
        /// </summary>
        /// <param name="command">Datos del proveedor a registrar.</param>
        /// <returns>Id del proveedor creado.</returns>
        /// <response code="201">Proveedor creado correctamente.</response>
        /// <response code="400">Datos inválidos.</response>
        /// <response code="409">Ya existe un proveedor con el mismo RFC o teléfono.</response>
        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateSupplierCommand command)
        {
            var id = await _mediator.Send(command);

            if (id == 0)
                return Conflict("No se pudo crear el proveedor.");

            return StatusCode(StatusCodes.Status201Created, id);
        }

        /// <summary>
        /// Obtener proveedores paginados.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1)
        {
            var result = await _mediator.Send(new GetAllSuppliersQuery(page));
            return Ok(result);
        }

        /// <summary>
        /// Obtener proveedor por RFC.
        /// </summary>
        [HttpGet("by-rfc/{rfc}")]
        public async Task<IActionResult> GetByRfc(string rfc)
        {
            var supplier = await _mediator.Send(new GetSupplierByRfcQuery(rfc));

            if (supplier is null)
                return NotFound($"No existe proveedor con RFC {rfc}");

            return Ok(supplier);
        }

        /// <summary>
        /// Actualizar proveedor.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] SupplierDto dto)
        {
            dto.IdSupplier = id;

            var success = await _mediator.Send(new UpdateSupplierCommand(dto));

            if (!success)
                return NotFound($"No existe proveedor con id {id}");

            return NoContent();
        }

        /// <summary>
        /// Cambiar estatus (activo/inactivo) por RFC.
        /// </summary>
        [HttpPatch("by-rfc/{rfc}/status")]
        public async Task<IActionResult> UpdateStatusByRfc(string rfc, [FromBody] bool active)
        {
            var success = await _mediator.Send(new UpdateSupplierStatusCommand(rfc, active));

            if (!success)
                return NotFound($"No existe proveedor con RFC {rfc}");

            return NoContent();
        }
    }
}
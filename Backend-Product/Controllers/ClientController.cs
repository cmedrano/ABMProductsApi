using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Dtos;
using Products.Application.IServices;
using Swashbuckle.AspNetCore.Annotations;

namespace Backend_Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Get all clients", Description = "Retrieve a list of all clients")]
        [ProducesResponseType(typeof(IEnumerable<ClientDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetAllClients()
        {
            try
            {
                var clients = await _clientService.GetAllAsync();

                return Ok(clients);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving clients: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get client by ID", Description = "Retrieve a client by their unique ID")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> GetClientById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest($"The ID: {id} is invalid.");
                }

                var client = await _clientService.GetByIdAsync(id);

                if (client == null)
                {
                    return NotFound();
                }
                return Ok(client);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error retrieving client: {ex.Message}");
            }
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Create a new client", Description = "Create a new client with the provided details")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateClient([FromBody] CreateClientDto clientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdClient = await _clientService.CreateAsync(clientDto);

                return CreatedAtAction(nameof(GetClientById), new { id = createdClient.Id }, createdClient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error creating client: {ex.Message}");
            }

        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update an existing client", Description = "Update the details of an existing client")]
        [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto clientDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedClient = await _clientService.UpdateAsync(id, clientDto);

                return Ok(updatedClient);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error updating client: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a client", Description = "Delete a client by their unique ID")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

        public async Task<IActionResult> DeleteClient(int id)
        {
            try
            {
                await _clientService.DeleteAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error deleting client: {ex.Message}");
            }
        }


    }
}
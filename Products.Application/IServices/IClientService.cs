using Products.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.IServices
{
    public interface IClientService
    {
        Task<IEnumerable<ClientDto>> GetAllAsync();
        Task<ClientDto> GetByIdAsync(int id);
        Task<ClientDto> CreateAsync(CreateClientDto clientDto);
        Task<ClientDto> UpdateAsync(int id, UpdateClientDto clientDto);
        Task DeleteAsync(int id);
    }
}

using AutoMapper;
using Products.Application.Dtos;
using Products.Application.IServices;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Application.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IMapper _mapper;

        public ClientService(IMapper mapper, IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientDto>> GetAllAsync()
        {
            var clients = await _clientRepository.GetAllClientsAsync();
            return _mapper.Map<IEnumerable<ClientDto>>(clients);
        }

        public async Task<ClientDto> GetByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);

            if (client == null)
                throw new Exception($"El cliente con ID {id} no existe");

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> CreateAsync(CreateClientDto clientDto)
        {
            var client = _mapper.Map<Client>(clientDto);
            var existingClient = await _clientRepository.GetClientByNameAsync(client.name);

            if (existingClient != null)
                throw new Exception($"El cliente con Nombre {client.name} no existe");

            await _clientRepository.AddClientAsync(client);

            return _mapper.Map<ClientDto>(client);
        }

        public async Task<ClientDto> UpdateAsync(int id, UpdateClientDto clientDto)
        {
            var existingClient = await _clientRepository.GetByIdAsync(id);

            if (existingClient == null)
                throw new Exception($"El cliente con email {clientDto.Email} no existe");

            _mapper.Map(clientDto, existingClient);
            await _clientRepository.UpdateClientAsync(existingClient);

            return _mapper.Map<ClientDto>(existingClient);
        }

        public async Task DeleteAsync(int id)
        {
            var existingClient = await _clientRepository.GetByIdAsync(id);

            if (existingClient == null)
                throw new Exception($"El cliente con ID {id} no existe");

            await _clientRepository.DeleteClientAsync(id);
        }
    }
}

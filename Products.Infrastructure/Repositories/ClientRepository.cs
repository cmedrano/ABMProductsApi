using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Products.Application.Dtos;
using Products.Domain.Entities;
using Products.Domain.Interfaces;
using Products.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Products.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly ApplicationDbContext _context;

        public ClientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Client.ToListAsync();
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Client.FirstOrDefaultAsync(c => c.id == id);
        }

        public async Task<Client> GetClientByNameAsync(string name)
        {
            return await _context.Client.FirstOrDefaultAsync(c => c.name == name);
        }

        public async Task<Client> AddClientAsync(Client client)
        {
            await _context.Client.AddAsync(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task UpdateClientAsync(Client client)
        {
            _context.Client.Update(client);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteClientAsync(int id)
        {
            var existingClient = await GetByIdAsync(id);
            if (existingClient != null)
            {
                _context.Client.Remove(existingClient);
                await _context.SaveChangesAsync();
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankIBM.Datas;
using BankIBM.Models;
using BankIBM.Services;
using Microsoft.AspNetCore.Authorization;

namespace BankIBM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly BankIBMContext _context;

        public ClientsController(BankIBMContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            Client validClient = await _context
                                    .Clients
                                    .Where(x => x.Email == client.Email)
                                    .FirstOrDefaultAsync();

            if (validClient != null)
                return NotFound(new { message = $"O e-mail {client.Email} já esta em uso" });

            client.NumberAccount = String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
            client.Password = HashService.Hash(client.Password);
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { id = client.Id }, client);
        }
    }
}

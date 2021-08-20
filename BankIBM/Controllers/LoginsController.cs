using BankIBM.Datas;
using BankIBM.Models;
using BankIBM.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BankIBM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginsController : ControllerBase
    {
        private readonly BankIBMContext _context;

        private IConfiguration _configuration;
        private readonly string ErroLogin = "Usuário ou senha inválidos";

        public LoginsController(BankIBMContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> PostLogin(Client client)
        {
            Client clientFind = await _context
                                    .Clients
                                    .Where(x => x.Email == client.Email)
                                    .FirstOrDefaultAsync();

            if (clientFind == null)
                return NotFound(new { message = ErroLogin });

            var Hash = HashService.Check(hash: clientFind.Password, password: client.Password);

            if (!Hash.verified)
                return NotFound(new { message = ErroLogin });

            var token = JwtService.GenerateToken(clientFind, _configuration);
            clientFind.Password = "";
            return new
            {
                clientFind,
                token
            };
        }
    }
}

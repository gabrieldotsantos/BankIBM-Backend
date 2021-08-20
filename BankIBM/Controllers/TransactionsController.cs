using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankIBM.Datas;
using BankIBM.Models;
using System.Globalization;
using Microsoft.AspNetCore.Authorization;

namespace BankIBM.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly BankIBMContext _context;

        public TransactionsController(BankIBMContext context)
        {
            _context = context;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.ToListAsync();
            transaction.Find(x => x.ClientSourceID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            transaction.ForEach(x =>
            {
                Client source = _context.Clients.Find(x.ClientSourceID);
                Client destiny = _context.Clients.Find(x.ClientDestinyID);
                x.NameAndAccountSource = String.Concat(source.Name, " - ", source.NumberAccount);
                x.NameAndAccountDestiny = String.Concat(destiny.Name, " - ", destiny.NumberAccount);
                x.DateInString = x.TransactionDate.ToString("dd/MM/yyyy HH:mm:ss");
            });

            return transaction;
        }

        // POST: api/Transactions/Date
        [Route("Date")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Transaction>>> PostTransactionDate(Transaction transaction)
        {
            var transactions = await _context.Transactions.ToListAsync();
            transactions = transactions.FindAll(x => x.ClientSourceID == transaction.ClientSourceID
                                                     && x.TransactionDate.Date == transaction.TransactionDate.Date);

            if (transactions == null)
            {
                return NotFound();
            }

            transactions.ForEach(x =>
            {
                Client source = _context.Clients.Find(x.ClientSourceID);
                Client destiny = _context.Clients.Find(x.ClientDestinyID);
                x.NameAndAccountSource = String.Concat(source.Name, " - ", source.NumberAccount);
                x.NameAndAccountDestiny = String.Concat(destiny.Name, " - ", destiny.NumberAccount);
                x.DateInString = x.TransactionDate.ToString("dd/MM/yyyy HH:mm:ss");
            });

            return transactions;
        }

        // POST: api/Transactions
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Transaction>> PostTransaction(Transaction transaction)
        {
            Client clientSource = _context.Clients.FindAsync(transaction.ClientSourceID).Result;

            if (transaction.TransactionType == "Transferencia")
            {
                if (clientSource.Balance < transaction.Value)
                    return NotFound(new { message = $"O valor de transfêrencia R$ {transaction.Value} é maior que o saldo" });
                
                Client clientDestiny = _context.Clients.FindAsync(transaction.ClientDestinyID).Result;
                clientDestiny.Balance += transaction.Value; 
                CreatedAtAction("GetClient", new { id = clientDestiny.Id }, clientDestiny);

                clientSource.Balance -= transaction.Value;
            }
            else
                clientSource.Balance += transaction.Value;

            CreatedAtAction("GetClient", new { id = clientSource.Id }, clientSource);

            transaction.TransactionDate = DateTime.Now;
            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTransaction", new { id = transaction.Id }, transaction);
        }
    }
}

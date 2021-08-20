using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankIBM.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        public string NumberAccount { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [StringLength(int.MaxValue, ErrorMessage = "O campo 'senha' deve ser maior que 6 e menor/igual que 30 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [DefaultValue(0)]
        public decimal Balance { get; set; }

        [InverseProperty(nameof(Transaction.ClientSource))]
        public virtual ICollection<Transaction> ClientSource { get; set; }

        [InverseProperty(nameof(Transaction.ClientDestiny))]
        public virtual ICollection<Transaction> ClientDestiny { get; set; }
    }
}

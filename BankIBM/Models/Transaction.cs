using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankIBM.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(ClientSource)), Column(Order = 1)]
        public int ClientSourceID { get; set; }

        [ForeignKey(nameof(ClientDestiny)), Column(Order = 1)]
        public int ClientDestinyID { get; set; }

        public decimal Value { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }

        public virtual Client ClientSource { get; set; }

        public virtual Client ClientDestiny { get; set; }

        [NotMapped]
        public string NameAndAccountSource { set; get; }

        [NotMapped]
        public string NameAndAccountDestiny { set; get; }

        [NotMapped]
        public string DateInString { set; get; }
    }
}

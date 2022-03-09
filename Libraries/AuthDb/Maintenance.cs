using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AuthDb
{
    public class Maintenance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }

        public DateTime Start { get; init; }

        public DateTime End { get; init; }

        [AllowNull]
        public string Reason { get; init; }

        public override string ToString()
        {
            return $"Id: {Id}, Start: {Start}, End: {End}, Reason: {Reason}";
        }
    }
}
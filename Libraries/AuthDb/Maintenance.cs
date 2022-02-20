using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace AuthDb
{
    public class Maintenance
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        [AllowNull]
        public string Reason { get; set; }

        public override string ToString()
        {
            return $"Id: {Id}, Start: {Start}, End: {End}, Reason: {Reason}";
        }
    }
}
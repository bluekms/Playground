using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace AuthDb
{
    public sealed class Foo
    {
        [Key]
        public long Seq { get; set; }

        [AllowNull]
        public string AccountId { get; set; }

        [AllowNull]
        public string Command { get; set; }

        public int Value { get; set; }

        public enum FooCommand
        {
            Addition,
            Subtraction,
            Multiplication,
            Division,
            Squared,
            Merge,
        }
    }
}
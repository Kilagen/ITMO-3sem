using Banks.Exceptions;

namespace Banks.Models
{
    public class Passport
    {
        public Passport(string series, string number)
        {
            if (string.IsNullOrWhiteSpace(series))
                throw new ClientException("Passport series cannot be null or empty");
            if (string.IsNullOrWhiteSpace(number))
                throw new ClientException("Passport number cannot be null or empty");
            Series = series;
            Number = number;
        }

        private Passport()
        {
            Series = string.Empty;
            Number = string.Empty;
        }

        public static Passport Empty => new Passport();
        public string Series { get; }
        public string Number { get; }
        public override string ToString() => $"Passport: {Series} {Number}";
        public override bool Equals(object? obj)
        {
            return (obj is Passport other) && other.Series == Series && other.Number == Number;
        }

        public override int GetHashCode()
        {
            return Series.GetHashCode() ^ Number.GetHashCode();
        }
    }
}

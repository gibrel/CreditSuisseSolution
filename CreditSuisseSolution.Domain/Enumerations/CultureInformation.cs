using CreditSuisseSolution.Domain.Helpers;

namespace CreditSuisseSolution.Domain.Enumerations
{
    public class CultureInformation : Enumeration
    {
        public static readonly CultureInformation en_US = new(0, nameof(en_US));
        public static readonly CultureInformation pt_BR = new(1, nameof(pt_BR));

        public CultureInformation(int id, string name) : base(id, name)
        {
        }
    }
}
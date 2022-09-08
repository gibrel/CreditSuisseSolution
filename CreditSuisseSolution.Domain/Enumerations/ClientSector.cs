using CreditSuisseSolution.Domain.Helpers;

namespace CreditSuisseSolution.Domain.Enumerations
{
    public class ClientSector : Enumeration
    {
        public static readonly ClientSector Public = new(0, nameof(Public));
        public static readonly ClientSector Private = new(1, nameof(Private));

        public ClientSector(int id, string name) : base(id, name)
        {
        }
    }
}

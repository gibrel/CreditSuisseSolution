using CreditSuisseSolution.Domain.Enumerations;
using CreditSuisseSolution.Domain.Interfaces;

namespace CreditSuisseSolution.Domain.Entities
{
    public class TradeTransaction : ITrade
    {
        public double Value { get; private set; }

        public string ClientSector { get; private set; }

        public DateTime NextPaymentDate { get; private set; }

        //public bool IsPoliticallyExposed { get; private set; }

        public TradeTransaction(double value, ClientSector clientSector, DateTime nextPaymentDate)
        //public TradeTransaction(double value, ClientSector clientSector, DateTime nextPaymentDate, bool isPoliricallyExposed)
        {
            Value = value;
            ClientSector = clientSector.Name;
            NextPaymentDate = nextPaymentDate;
        }
    }
}

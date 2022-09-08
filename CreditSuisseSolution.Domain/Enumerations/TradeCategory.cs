using CreditSuisseSolution.Domain.Helpers;

namespace CreditSuisseSolution.Domain.Enumerations
{
    public class TradeCategory : Enumeration
    {
        public static readonly TradeCategory NONE = new(0, nameof(NONE)); //No other category - used for visual clarification
        public static readonly TradeCategory EXPIRED = new(1, nameof(EXPIRED)); //payment if overdue by more than 30 days from date reference
        public static readonly TradeCategory HIGHRISK = new(2, nameof(HIGHRISK)); //trade with private sector client with more than 1M value
        public static readonly TradeCategory MEDIUMRISK = new(3, nameof(MEDIUMRISK)); //trade with public sector client with more than 1M
        public static readonly TradeCategory PEP = new(4, nameof(PEP)); //trade with politically exposed person

        public TradeCategory(int id, string name) : base(id, name)
        {
        }
    }
}
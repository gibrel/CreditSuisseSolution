using CreditSuisseSolution.Domain.Enumerations;
using CreditSuisseSolution.Domain.Entities;
using CreditSuisseSolution.Domain.Interfaces;
using System.Globalization;
using CreditSuisseSolution.Domain.Helpers;
using System.Text;

namespace CreditSuisseSolution.Service.Services
{
    public class ComputeRiskService
    {
        public DateTime ReferenceDate { get; private set; }
        public List<ITrade> Trades { get; private set; }
        public List<List<TradeCategory>> AnalisedTrades { get; private set; }

        const double _1M = 1000000;

        public ComputeRiskService()
        {
            Trades = new List<ITrade>();
            AnalisedTrades = new List<List<TradeCategory>>();
        }

        public bool ImportReferenceDate(string referenceDateString)
        {
            var validConversion = DateTime.TryParseExact(referenceDateString, "MM/dd/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime referenceDate);
            if(validConversion) ReferenceDate = referenceDate;
            return validConversion;
        }

        public bool ImportTrade(List<string> tradeInfo) {
            if(tradeInfo == null || tradeInfo?.Count < 4) return false;

            string ti_value = tradeInfo?[0] ?? "",
                   ti_ClientSector = tradeInfo?[1] ?? "",
                   ti_nextPaymentDate = tradeInfo?[2] ?? "",
                   ti_isPoliticallyExposed = tradeInfo?[3].ToUpperInvariant() ?? "";

            var isValidValue = double.TryParse(ti_value, out double tradeValue);
            var tradeClientSector = Enumeration.GetByName<ClientSector>(ti_ClientSector);
            var isValidNextPaymentDate = DateTime.TryParseExact(ti_nextPaymentDate, "MM/dd/yyyy",
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime nextPaymentDate);
            (var isValidPoliticallyExposed, var isPoliticallyExposed) = ConvertStringToBool(ti_isPoliticallyExposed);

            if (isValidValue 
                && tradeClientSector.ToString().Equals(ti_ClientSector)
                && isValidNextPaymentDate
                && isValidPoliticallyExposed
                )
            {
                Trades.Add(new TradeTransaction(tradeValue, tradeClientSector, nextPaymentDate, isPoliticallyExposed));
                return true;
            }

            return false;
        }

        public StringBuilder AnalyseTrades()
        {
            var sb = new StringBuilder();

            foreach (var trade in Trades)
            {
                var tradeCategories = new List<TradeCategory>();

                if (IsExpiredTrade(trade)) tradeCategories.Add(TradeCategory.EXPIRED); //trades with 30 days of expired
                if (IsHighRiskTrade(trade)) tradeCategories.Add(TradeCategory.HIGHRISK); //value over 1M of private client
                if (IsMediumRiskTrade(trade)) tradeCategories.Add(TradeCategory.MEDIUMRISK); //value over 1M of private client
                if (IsPepTrade(trade)) tradeCategories.Add(TradeCategory.PEP); //politically exposed client

                if (tradeCategories.Count == 0) tradeCategories.Add(TradeCategory.NONE); //no other category

                AnalisedTrades.Add(tradeCategories);

                foreach(var tradeCategory in tradeCategories)
                {
                    sb.Append(tradeCategory.ToString() + " ");
                }
                sb.Append('\n');
            }

            return sb;
        }

        private bool IsExpiredTrade(ITrade trade ) {
            if( (ReferenceDate - trade.NextPaymentDate).TotalDays > 30) return true;
            return false;
        }

        private bool IsHighRiskTrade(ITrade trade)
        {
            if (Enumeration.GetByName<ClientSector>(trade.ClientSector).Equals(ClientSector.Private)
                && trade.Value > _1M) return true;
            return false;
        }

        private bool IsMediumRiskTrade(ITrade trade)
        {
            if (Enumeration.GetByName<ClientSector>(trade.ClientSector).Equals(ClientSector.Public)
                && trade.Value > _1M) return true;
            return false;
        }

        private bool IsPepTrade(ITrade trade)
        {
            return trade.IsPoliticallyExposed;
        }

        private (bool success, bool converted) ConvertStringToBool (string inputString)
        {
            if (string.Equals(inputString.ToUpperInvariant(), "YES"))
                return (true, true);
            if (string.Equals(inputString.ToUpperInvariant(), "NO"))
                return (true, false);
            return (false, false);
        }
    }
}

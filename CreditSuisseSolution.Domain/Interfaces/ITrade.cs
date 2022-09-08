namespace CreditSuisseSolution.Domain.Interfaces
{
    public interface ITrade
    {
        double Value { get; } //indicated the transaction amount in dollars
        string ClientSector { get; } //indicate the client's sector which can be "Public" or "Private"
        DateTime NextPaymentDate { get; } //indicates when the next payment from the client to the bank is expected
        //bool IsPoliticallyExposed { get; } //indicates if this transaction's client is politically exposed
    }
}

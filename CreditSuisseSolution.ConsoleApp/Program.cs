using System.Resources;
using System.Globalization;
using System.Reflection;
using CreditSuisseSolution.Domain.Enumerations;
using CreditSuisseSolution.Domain.Helpers;
using CreditSuisseSolution.Domain.Interfaces;
using CreditSuisseSolution.Service.Services;

ResourceManager resourceMgr = new ("CreditSuisseSolution.ConsoleApp.Resources.Resource",
    Assembly.GetExecutingAssembly());

CultureInfo ci = GetCulture();

Console.WriteLine(@$"{resourceMgr.GetString("WELCOME_TEXT", ci)}");

var crs = new ComputeRiskService();

GetReferenceDate();

int numberOfTrades = GetNumberOfTrades();

GetTrades(numberOfTrades);

var results = crs.AnalyseTrades();

Console.WriteLine(@$"{resourceMgr.GetString("ANALYSIS_RESULTS_INTRO", ci)}{results}");

CultureInfo GetCulture()
{
    var culture = args.Length > 0 ? args[0].Replace('-', '_') : CultureInformation.en_US.ToString();

    var cultureInfo = Enumeration.GetByName<CultureInformation>(culture) ?? CultureInformation.en_US;

    return new CultureInfo (cultureInfo.Name.Replace('_', '-'));
}

void GetReferenceDate()
{
    var validConversion = false;

    do
    {
        Console.WriteLine($"{resourceMgr.GetString("INFORM_REFERENCE_DATE", ci)} " +
            $"({resourceMgr.GetString("DATE_FORMAT", ci)})");

        var input = Console.ReadLine();
        
        if (input != null)
        {
            validConversion = crs.ImportReferenceDate(input);
            if (validConversion) return;
        }

        Console.WriteLine($"{resourceMgr.GetString("INVALID_REFERENCE_DATE", ci)}");

    } while (!validConversion);

    return;
}

int GetNumberOfTrades()
{
    var validConversion = false;

    do
    {
        Console.WriteLine($"{resourceMgr.GetString("INFORM_NUMBER_TRANSACTIONS", ci)}");

        var input = Console.ReadLine();

        if (input != null)
        {
            validConversion = int.TryParse(input, out int numberOfTrades);
            if (validConversion) return numberOfTrades;
        }

        Console.WriteLine($"{resourceMgr.GetString("INVALID_NUMBER_TRANSACTIONS", ci)}");

    } while (!validConversion);
    return 0;
}

void GetTrades(int numberOfTrades)
{
    for (int sucessfullImportedTrades = 0; sucessfullImportedTrades < numberOfTrades; sucessfullImportedTrades++)
    {
        GetTradeInput(sucessfullImportedTrades + 1, numberOfTrades);
    }
}

void GetTradeInput(int tradeImportIndex, int totalImports)
{
    bool validConversion = false;

    do
    {
        Console.WriteLine($"{resourceMgr.GetString("INFORM_TRADE", ci)}  {tradeImportIndex}/{totalImports} (" +
            $"{resourceMgr.GetString("EXPECTED_TRADE_FORMAT", ci)})");

        var input = Console.ReadLine();
        var tradeInfos = input?.Split(' ').ToList() ?? new List<string>();

        if (input != null)
        {
            validConversion = crs.ImportTrade(tradeInfos);
            if (validConversion) return;
        }

        Console.WriteLine($"{resourceMgr.GetString("INVALID_TRADE", ci)}");
    } while (!validConversion);

    return;
}
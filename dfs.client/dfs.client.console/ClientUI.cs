using dfs.core.common.models;

namespace dfs.client.console;

public static class ClientUI
{
    public static void DisplayDocuments(IEnumerable<Document> documents)
    {
        Console.WriteLine("Documents Available: ");
        foreach (var document in documents)
        {
            Console.WriteLine($"{document.Name} - {document.Cost.ToString("C0")} - {document.QuantityAvailable}");
        }
    }
}
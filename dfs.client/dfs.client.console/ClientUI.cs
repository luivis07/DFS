using dfs.core.common.models;

namespace dfs.client.console;

public static class ClientUI
{
    public static Document? DisplayDocuments(IEnumerable<Document> documents)
    {
        Console.WriteLine("Documents Available: ");
        foreach (var document in documents)
        {
            Console.WriteLine($"{document.Name} - {document.Cost.ToString("C0")} - {document.QuantityAvailable}");
        }
        Console.Write("Select (by name): ");
        var choice = Console.ReadLine();
        //var choice = "A";
        var selection = documents.FirstOrDefault(f => string.Equals(f.Name, choice, StringComparison.OrdinalIgnoreCase));
        return selection;
    }
}
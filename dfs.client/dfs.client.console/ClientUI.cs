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
        var selection = documents.FirstOrDefault(f => string.Equals(f.Name, choice, StringComparison.OrdinalIgnoreCase));
        return selection;
    }

    public static IAdminMessage? DisplayOptions()
    {
        Console.WriteLine("Admin Options");
        Console.WriteLine("(a) Add document");
        Console.WriteLine("(r) Remove document");
        var choice = Console.ReadLine();
        if (string.Equals(choice, MessageType.ADMIN_ADD_DOCUMENT_SELECTION, StringComparison.OrdinalIgnoreCase))
        {
            return AddAdminDocument();
        }
        if (string.Equals(choice, MessageType.ADMIN_REMOVE_DOCUMENT_SELECTION, StringComparison.OrdinalIgnoreCase))
        {
            return RemoveAdminDocument();
        }
        return null;
    }

    private static IAdminMessage RemoveAdminDocument()
    {
        throw new NotImplementedException();
    }

    private static IAdminMessage AddAdminDocument()
    {
        Console.Write("Enter document path: ");
        var documentPath = Console.ReadLine();
        Console.Write("Enter quantity available: ");
        var quantity = int.Parse(Console.ReadLine() ?? "0");
        var result = new ProcessAdminAddDocument();
        result.Document = new Document();
        if (File.Exists(documentPath))
        {
            var fileInfo = new FileInfo(documentPath);
            var bytes = File.ReadAllBytes(documentPath);
            result.Document.NameWithExtension = fileInfo.Name;
            result.Document.Cost = bytes.Length / 1024;
            result.Document.QuantityAvailable = quantity;
            result.Document.Size = bytes.Length;
            result.FollowUpContent = bytes;
        }
        return result;
    }
}
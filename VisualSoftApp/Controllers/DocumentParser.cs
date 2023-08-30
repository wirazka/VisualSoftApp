namespace VisualSoftApp.Controllers;
public class DocumentParser
{
    public List<Document> ParseLines(string[] lines)
	{
        var documents = new List<Document>();

        foreach (var line in lines)
        {
            if (line.StartsWith("H"))
            {
                var header = ParseHeader(line);
                documents.Add(new Document() { Header = header });
            }
            else if (line.StartsWith("C"))
            {
                var comment = ParseComment(line);
                var docTmp = documents.Last();
                docTmp.Comments.Add(comment);
            }
            else if (line.StartsWith("B"))
            {
                var position = ParsePosition(line);
                var docTmp = documents.Last();
                docTmp.Positions.Add(position);
            }
        }

        return documents;
    }

    private Header ParseHeader(string line)
    {
        var parts = line.Split(';');
        if (parts.Length < 15)
        {
            throw new Exception("Niepoprawny format nagłówka.");
        }

        return new Header
        {
            CodeBA = parts[1],
            Type = parts[2],
            DocumentNumber = parts[3],
            OperationDate = DateTime.Parse(parts[4]),
            DayNumber = parts[5],
            ContractorCode = parts[6],
            ContractorName = parts[7],
            ExternalDocumentNumber = parts[8],
            ExternalDocumentDate = DateTime.Parse(parts[9]),
            NetAmount = decimal.Parse(parts[10]),
            VatAmount = decimal.Parse(parts[11]),
            GrossAmount = decimal.Parse(parts[12]),
            F1 = parts[13],
            F2 = parts[14],
            F3 = parts[15]
        };
    }

    private Comment ParseComment(string line)
    {
        var content = line.Substring(1);
        return new Comment { Content = content };
    }

    private Position ParsePosition(string line)
    {
        var parts = line.Split(';');
        if (parts.Length < 9)
        {
            throw new Exception("Niepoprawny format pozycji.");
        }

        return new Position
        {
            ProductCode = parts[1],
            ProductName = parts[2],
            NetPrice = decimal.Parse(parts[3]),
            NetValue = decimal.Parse(parts[4]),
            Vat = decimal.Parse(parts[5]),
            QuantityBefore = int.Parse(parts[6]),
            AvgBefore = decimal.Parse(parts[7]),
            QuantityAfter = int.Parse(parts[8]),
            AvgAfter = decimal.Parse(parts[9]),
            Group = parts[10]
        };
    }
}

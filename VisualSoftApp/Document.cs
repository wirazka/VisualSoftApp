namespace VisualSoftApp
{
    public class Document
    {
        public Header Header { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Position> Positions { get; set; }
    }

    public class Header
    {
        public string CodeBA { get; set; }
        public string Type { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime OperationDate { get; set; }
        public string DayNumber { get; set; }
        public string ContractorCode { get; set; }
        public string ContractorName { get; set; }
        public string ExternalDocumentNumber { get; set; }
        public DateTime ExternalDocumentDate { get; set; }
        public decimal NetAmount { get; set; }
        public decimal VatAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public string F1 { get; set; }
        public string F2 { get; set; }
        public string F3 { get; set; }
    }

    public class Comment
    {
        public string Content { get; set; }
    }

    public class Position
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal NetPrice { get; set; }
        public decimal NetValue { get; set; }
        public decimal Vat { get; set; }
        public int QuantityBefore { get; set; }
        public decimal AvgBefore { get; set; }
        public int QuantityAfter { get; set; }
        public decimal AvgAfter { get; set; }
        public string Group { get; set; }
    }
}

namespace OfficePass.Domain.Entities
{
    public class DocumentType
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<Guest> Guests { get; set; }
    }
}

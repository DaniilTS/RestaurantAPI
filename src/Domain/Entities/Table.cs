namespace Domain.Entities
{
    public class Table
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Size { get; set; }
        
        public int FreeSpace { get; set; }

        public List<ClientsGroup> ClientsGroups { get; set; } = new List<ClientsGroup>();
    }
}

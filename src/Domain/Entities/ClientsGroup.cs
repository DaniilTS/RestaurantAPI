namespace Domain.Entities
{
    public class ClientsGroup
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Size { get; set; }

        public int BoredIndex { get; set; }

        public int MaxBoaredIndex { get; set; }

        public int LunchTimeInSeconds { get; set; }

        public string Status { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Table Table { get; set; }

        public Guid? TableId { get; set; }


        public void IncreaseBoredIndex() => BoredIndex += 1;
    }
}

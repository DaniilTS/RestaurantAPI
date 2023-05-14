namespace Application.DTO
{
    public class ClientsGroupDTO
    {
        public Guid Id { get; set; }

        public int Size { get; set; }

        public int BoredIndex { get; set; }

        public int MaxBoaredIndex { get; set; }

        public int LunchTimeInSeconds { get; set; }

        public string Status { get; set; }

        public DateTime ArrivalTime { get; set; }

        public Guid? TableId { get; set; }
    }
}

namespace BookingTravelApi.DTO
{
    public class RestDTO<T>
    {
        public T Data { get; set; } = default!;
    }
}

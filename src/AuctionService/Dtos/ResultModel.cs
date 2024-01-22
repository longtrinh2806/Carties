namespace AuctionService.Dtos
{
    public class ResultModel
    {
        public object? Data { get; set; }
        public bool IsSucceed { get; set; }
        public string? Message { get; set; }
    }
}

namespace DungeonCrawlerAPI.DTO_s
{
    public class AuctionDTO
    {
        public string Id { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string SellerCharacterId { get; set; }
        public string SellerCharacterName { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? BuyoutPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
        public List<BidDTO> Bids { get; set; }
    }

    public class CreateAuctionDTO
    {
        public string ItemId { get; set; }
        public decimal StartingPrice { get; set; }
        public decimal? BuyoutPrice { get; set; }
        public int DurationInHours { get; set; }
    }

    public class BidDTO
    {
        public string BidderCharacterName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
    }

    public class CreateBidDTO
    {
        public decimal Amount { get; set; }
    }
}
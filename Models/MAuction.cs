using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DungeonCrawlerAPI.Models
{
    [Table("Auction")]
    public class MAuction : BaseEntity
    {
        [Required]
        [Column("Item_Id")]
        public string ItemId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public MItems mItem { get; set; }

        [Column("Seller_Character")]
        public string SellerCharacterId { get; set; }

        [ForeignKey(nameof(SellerCharacterId))]
        public MCharacter character { get; set; }

        [Column("Starting_Price")]
        public decimal StartingPrice { get; set; }
        
        [Column("Buyout_Price")]
        public decimal? BuyoutPrice { get; set; }
        
        [Column("Start_Time")]
        public DateTime StartTime { get; set; }
        
        [Column("End_Time")]
        public DateTime EndTime { get; set; }

        [Column("Auction_Status")]
        public AuctionStatus AuctionStatus { get; set; }
        public ICollection<MBid> Bids { get; set; }

    }

    [Table("Bids")]
    public class MBid : BaseEntity
    {
        [Column("Auction_Id")]
        public string AuctionId { get; set; }

        [ForeignKey(nameof(AuctionId))]
        public MAuction auction { get; set; }

        [Column("Bidder_Character")]
        public string BidderCharacter {  get; set; }

        [ForeignKey(nameof(BidderCharacter))]
        public MCharacter character { get; set; }

        [Column("Amount")]
        public decimal Amount { get; set; }
        public DateTime BidTime { get; set; }
    }

    public enum AuctionStatus
    {
        Active,
        Sold,
        Expired
    }
}

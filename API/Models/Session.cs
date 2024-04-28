
namespace API.Models {
    public class Session
    {
        public required string sessionId { get; set; }
        public required string Address { get; set; }
        public required List<string> Players { get; set; }
    }
}
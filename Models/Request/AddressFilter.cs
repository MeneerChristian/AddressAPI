namespace AddressAPI.Models.Request
{
    public class AddressFilter
    {
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        
        public bool Exact { get; set; }

        // sort by
        public string SortBy { get; set; }
        public string SortOrder { get; set; } 
    }
}
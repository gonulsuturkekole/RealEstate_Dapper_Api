namespace RealEstate_Dapper_UI.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        public string Title { get; set; }
        public int Price { get; set; }
        public string City { get; set; }
        public string CoverImage { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int ProductCategory { get; set; }
        public int CreatorUserId { get; set; }
    }
}

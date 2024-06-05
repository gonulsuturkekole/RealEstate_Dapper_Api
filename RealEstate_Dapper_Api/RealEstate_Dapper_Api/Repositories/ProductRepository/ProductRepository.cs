using Dapper;
using RealEstate_Dapper_Api.Dtos.CategoryDtos;
using RealEstate_Dapper_Api.Dtos.EmployeeDtos;
using RealEstate_Dapper_Api.Dtos.ProductDetailDtos;
using RealEstate_Dapper_Api.Dtos.ProductDtos;
using RealEstate_Dapper_Api.Models.DapperContext;

namespace RealEstate_Dapper_Api.Repositories.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;

        public ProductRepository(Context context)
        {
            _context = context;
        }

        public async Task CreateProduct(CreateProductDto createProductDto)
        {
            int productId = -1;
            string query = "insert into Product (AdvertisementDate,Description,Title,Price,City,CoverImage,Address,Type,ProductCategory,CreatorUserId) values (@AdvertisementDate,@Description,@Title,@Price,@City,@CoverImage,@Address,@Type,@ProductCategory,@CreatorUserId); SELECT CAST(SCOPE_IDENTITY() as int)";
            var parameters = new DynamicParameters();
            parameters.Add("@AdvertisementDate", DateTime.Now);
            parameters.Add("@Description", createProductDto.Description);
            parameters.Add("@Title", createProductDto.Title);
            parameters.Add("@Price", createProductDto.Price);
            parameters.Add("@City", createProductDto.City);
            parameters.Add("@CoverImage", createProductDto.CoverImage);
            parameters.Add("@Address", createProductDto.Address);
            parameters.Add("@Type", createProductDto.Type);
            parameters.Add("@ProductCategory", createProductDto.ProductCategory);
            parameters.Add("@CreatorUserId", createProductDto.CreatorUserId);
            using (var connection = _context.CreateConnection())
            {
                productId = await connection.QuerySingleAsync<int>(query, parameters);
            }

            Random rnd = new Random();
            var productSize = rnd.Next(50, 150);
            var bedRoomCount = rnd.Next(1, 14);
            var bathRoomCount = rnd.Next(1, 5);
            var roomCount = rnd.Next(1, 30);
            var garageSize = rnd.Next(10, 50);
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            var buildYear = start.AddDays(rnd.Next(range));

            query = "insert into ProductDetails (ProductID, ProductSize, BedroomCount, BathroomCount, RoomCount, GarageSize, BuildYear, Price, Location, VideoUrl) values (@ProductId,@ProductSize,@BedroomCount,@BathroomCount,@RoomCount,@GarageSize,@BuildYear,@Price,@Location,@VideoUrl)";
            parameters = new DynamicParameters();
            parameters.Add("@ProductId", productId);
            parameters.Add("@ProductSize", productSize);
            parameters.Add("@BedroomCount", bedRoomCount);
            parameters.Add("@BathroomCount", bathRoomCount);
            parameters.Add("@RoomCount", roomCount);
            parameters.Add("@GarageSize", garageSize);
            parameters.Add("@BuildYear", buildYear);
            parameters.Add("@Price", createProductDto.Price);
            parameters.Add("@Location", "https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d752.3402720069224!2d28.99977205997432!3d41.0392320642639!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x14cab7761a3b7de3%3A0xdcd33e38cf3b830b!2zRG9sbWFiYWjDp2UgU2FyYXnEsQ!5e0!3m2!1str!2str!4v1717452702992!5m2!1str!2str");
            parameters.Add("@VideoUrl", "https://www.youtube.com/embed/XmIoYlTUAjQ");
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }

        public async Task<List<ResultProductDto>> GetAllProductAsync()
        {
            string query = "Select * From Product";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductDto>(query);
                return values.ToList();
            }
        }
        public async Task<List<ResultProductWithCategoryDto>> GetAllProductWithCategoryAsync()
        {
            string query = "Select c.CategoryName, ProductID,Title,Price,City,CoverImage,Type,Address From Product AS p inner join Category AS c on p.ProductCategory=c.CategoryID";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithCategoryDto>(query);
                return values.ToList();
            }
        }

        public async Task<List<ResultLast3ProductWithCategoryDto>> GetLast3ProductAsync()
        {
            string query = "Select Top(3) ProductID,Title,Price,City,District,ProductCategory,CategoryName,AdvertisementDate,CoverImage,Description From Product Inner Join Category On Product.ProductCategory=Category.CategoryID  Order By ProductID Desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultLast3ProductWithCategoryDto>(query);
                return values.ToList();
            }
        }
        public async Task<List<ResultLast5ProductWithCategoryDto>> GetLast5ProductAsync()
        {
            string query = "Select Top(5) ProductID,Title,Price,City,District,ProductCategory,CategoryName,AdvertisementDate From Product Inner Join Category On Product.ProductCategory=Category.CategoryID Where Type='Kiralık' Order By ProductID Desc";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultLast5ProductWithCategoryDto>(query);
                return values.ToList();
            }
        }
        public async Task<List<ResultProductAdvertListWithCategoryByEmployeeDto>> GetProductAdvertListByEmployeeAsyncByFalse(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName,CoverImage,Type,Address,DealOfTheday From Product inner join Category on Product.ProductCategory=Category.CategoryID where EmployeeId=@employeeId and ProductStatus=0";
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductAdvertListWithCategoryByEmployeeDto>(query, parameters);
                return values.ToList();
            }
        }
        public async Task<List<ResultProductAdvertListWithCategoryByEmployeeDto>> GetProductAdvertListByEmployeeAsyncByTrue(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName,CoverImage,Type,Address,DealOfTheday From Product inner join Category on Product.ProductCategory=Category.CategoryID where EmployeeId=@employeeId and ProductStatus=1";
            var parameters = new DynamicParameters();
            parameters.Add("@employeeId", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductAdvertListWithCategoryByEmployeeDto>(query, parameters);
                return values.ToList();
            }
        }

        public async Task<List<ResultProductWithCategoryDto>> GetProductByDealOfTheDayTrueWithCategoryAsync()
        {
            string query = "Select ProductID,Title,Price,City,District,CategoryName,CoverImage,Type,Address,DealOfTheday From Product inner join Category on Product.ProductCategory=Category.CategoryID where DealOfTheDay=1";
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithCategoryDto>(query);
                return values.ToList();
            }
        }
        public async Task<GetProductByProductIdDto> GetProductByProductId(int id)
        {
            string query = "Select ProductID,Title,Price,City,District,Description,CategoryName,CoverImage,Type,Address,DealOfTheday,AdvertisementDate From Product inner join Category on Product.ProductCategory=Category.CategoryID where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<GetProductByProductIdDto>(query, parameters);
                return values.FirstOrDefault();
            }

        }
        public async Task<GetProductDetailByIdDto> GetProductDetailByProductId(int id)
        {
            string query = "Select * From ProductDetails Where ProductId=@productId";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<GetProductDetailByIdDto>(query, parameters);
                return values.FirstOrDefault();
            }
        }
        public async Task ProductDealOfTheDayStatusChangeToFalse(int id)
        {
            string query = "Update Product Set DealOfTheDay=0 where ProductID=@productID";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task ProductDealOfTheDayStatusChangeToTrue(int id)
        {
            string query = "Update Product Set DealOfTheDay=1 where ProductID=@productID";
            var parameters = new DynamicParameters();
            parameters.Add("@productID", id);
            using (var connection = _context.CreateConnection())
            {
                await connection.ExecuteAsync(query, parameters);
            }
        }
        public async Task<List<ResultProductWithSearchListDto>> ResultProductWithSearchList(string searchKeyValue, int propertyCategoryId, string city)
        {
            string query = "Select * From Product Where Title like '%" + searchKeyValue + "%' And ProductCategory=@propertyCategoryId And City=@city";
            var parameters = new DynamicParameters();
            //   parameters.Add("@searchKeyValue", searchKeyValue);
            parameters.Add("@propertyCategoryId", propertyCategoryId);
            parameters.Add("@city", city);
            using (var connection = _context.CreateConnection())
            {
                var values = await connection.QueryAsync<ResultProductWithSearchListDto>(query, parameters);
                return values.ToList();
            }
        }
    }
}
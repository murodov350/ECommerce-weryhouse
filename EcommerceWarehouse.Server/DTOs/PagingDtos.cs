namespace EcommerceWarehouse.Server.DTOs
{
    public class PagedResult<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)System.Math.Ceiling((double)TotalCount / PageSize);
        public List<T> Items { get; set; } = new();
    }

    public class ProductQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public Guid? CategoryId { get; set; }
        public string? Sort { get; set; } // name, price, quantity
        public bool Desc { get; set; } = false;
    }

    public class BasicQuery
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
        public string? Sort { get; set; } // name
        public bool Desc { get; set; } = false;
    }
}

namespace EshopApiAlza.Models
{
    public class PaginatedResponse<T>
    {
        public int TotalProducts { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public IEnumerable<T> Data { get; set; }

        public PaginatedResponse(int totalProducts, int totalPages, int currentPage, int pageSize, IEnumerable<T> data)
        {
            TotalProducts = totalProducts;
            TotalPages = totalPages;
            CurrentPage = currentPage;
            PageSize = pageSize;
            Data = data;
        }
    }
}

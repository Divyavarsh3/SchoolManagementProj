namespace SchoolManagement.Model.Common
{
    public class ApiResponse<T>
    {
        // HTTP style status code for consistency
        public int StatusCode { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }

    // Generic paged result to support pagination metadata
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}

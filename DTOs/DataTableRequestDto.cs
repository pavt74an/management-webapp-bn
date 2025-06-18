namespace management_webapp_bn.DTOs
{
    public class DataTableRequestDto
    {
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public string? PageNumber { get; set; }
        public string? PageSize { get; set; }
        public string? Search { get; set; }
    }
}

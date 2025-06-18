namespace management_webapp_bn.DTOs
{
    public class UserDataTableRequest
    {
        public string? OrderBy { get; set; }
        public string? OrderDirection { get; set; }
        public int? PageNumber { get; set; }
        public int? PageSize { get; set; }
        public string? Search { get; set; }
    }
}

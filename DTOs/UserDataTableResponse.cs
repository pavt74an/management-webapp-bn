namespace management_webapp_bn.DTOs
{
    public class UserDataTableResponse
    {
        public List<UserDto> Data { get; set; } = new List<UserDto>();
        
        public int Page{ get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}

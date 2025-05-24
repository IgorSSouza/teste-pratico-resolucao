using Questao2.Domain.Entities;

namespace Questao2.Shared.Dtos
{
    public class ApiResponse
    {
        public int Page { get; set; }
        public int Per_Page { get; set; }
        public int Total { get; set; }
        public int Total_Pages { get; set; }
        public Match[] Data { get; set; }
    }
}

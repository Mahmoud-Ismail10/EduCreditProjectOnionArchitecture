using EduCredit.Core.Enums;
using System.Text.Json.Serialization;
namespace EduCredit.Service.DTOs.UserDTOs
{
    public class GetUserInfoDto
    {
        [JsonPropertyOrder(2)]
        public string Name { get; set; }
        [JsonPropertyOrder(3)]
        public string PhoneNumber { get; set; }
        [JsonPropertyOrder(4)]
        public Gender Gender { get; set; }
        [JsonPropertyOrder(5)]
        public string Email { get; set; }
        [JsonPropertyOrder(6)]
        public DateOnly BirthDate  { get; set; }
        [JsonPropertyOrder(7)]
        public string Address { get; set; }
        [JsonPropertyOrder(8)]
        public string NationalId { get; set; }
        
    }
}

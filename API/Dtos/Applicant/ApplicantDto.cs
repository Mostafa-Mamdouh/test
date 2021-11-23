using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ApplicantDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        [MinLength(5,ErrorMessage = "Name Should be at least 5 Characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Family Name Is Required")]
        [MinLength(5, ErrorMessage = "Family Name Should be at least 5 Characters")]
        public string FamilyName { get; set; }
        [Required(ErrorMessage = "Address Is Required")]
        [MinLength(5, ErrorMessage = "Address Should be at least 5 Characters")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Country Is Required")]
        public string CountryOfOrigin { get; set; }
        [Required(ErrorMessage = "EMail Is Required")]
        [EmailAddress(ErrorMessage = "Please add a valid email")]
        public string EMailAdress { get; set; }
        [Required(ErrorMessage = "Age Is Required")]
        [Range(20,60,ErrorMessage = "Age Should be between 20 and 60 years old")]
        public int Age { get; set; }
        public bool Hired { get; set; }
    }
}
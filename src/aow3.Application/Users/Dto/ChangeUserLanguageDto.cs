using System.ComponentModel.DataAnnotations;

namespace aow3.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
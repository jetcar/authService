using System.ComponentModel.DataAnnotations;

namespace Dto
{
    public class EditUserDto
    {
        [Required]
        [StringLength(16, ErrorMessage = "{0} length must be between {2} and {1}.", MinimumLength = 6)]
        public string NewPassword { get; set; }

        public string PersonalId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.DbModel
{
    public class UserDb : BaseEntity
    {
        public int Id { get; set; }

        [MaxLength(64)]
        [Required]
        public string Username { get; set; }

        [MaxLength(128)]
        [Required]
        public string PasswordHash { get; set; }

        [MaxLength(36)]
        [Required]
        public string PasswordSalt { get; set; }

        public void SetPassword(string password)
        {
            using (var algorithm = new SHA512CryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password + PasswordSalt);
                var hashedBytes = algorithm.ComputeHash(inputBytes);
                PasswordHash = Convert.ToBase64String(hashedBytes);
            }
        }

        public bool ValidatePassword(string password)
        {
            using (var algorithm = new SHA512CryptoServiceProvider())
            {
                var inputBytes = Encoding.UTF8.GetBytes(password + PasswordSalt);
                var hashedBytes = algorithm.ComputeHash(inputBytes);
                var base64String = Convert.ToBase64String(hashedBytes);
                return PasswordHash == base64String;
            }

        }
    }
}

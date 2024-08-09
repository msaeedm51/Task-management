using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using TaskManagementSystem.Database.Models;
using TaskManagementSystem.DTOs;

namespace TaskManagementSystem.AuthenticationHelper
{
    public static class PasswordManager
    {
        public static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }


        public static byte[] HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA1))
            {
                return pbkdf2.GetBytes(20);
            }

        }


        public static bool ValidatePassword(string enteredPassword, byte[] storedSalt, byte[] storedHash)
        {
            byte[] enteredHash = HashPassword(enteredPassword, storedSalt);

            // Constant-time comparison to mitigate timing attacks
            uint differences = (uint)storedHash.Length ^ (uint)enteredHash.Length;
            for (int i = 0; i < storedHash.Length && i < enteredHash.Length; i++)
            {
                differences |= (uint)(storedHash[i] ^ enteredHash[i]);
            }

            return differences == 0;
        }


        public static string GenerateJwtToken(User loggedInUser, JWTSettings _jWTSettings)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, loggedInUser.Username),
                    new Claim(ClaimTypes.Email, loggedInUser.EmailAddress),
                    new Claim("User_Id", loggedInUser.Id.ToString())
                };

            foreach (var role in loggedInUser.UserRoles!)
            {
                // We only add user roles if they are active
                if (role.Active == true)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role.Role!.Role));
                }
            }

            // Use envorinment variables instead
            var token = new JwtSecurityToken
            (
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(30),
                notBefore: DateTime.UtcNow,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jWTSettings.JWT_Key)),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return tokenString;
        }

        public static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber)
                                .TrimEnd('=')
                                .Replace('+', '-')
                                .Replace('/', '-')
                                .Replace('"', '-');
            }
        }


        public static bool ValidateRefreshToken(RefreshTokens storedRefreshToken, string receivedRefreshToken)
        {
            return storedRefreshToken.RefreshToken == receivedRefreshToken && storedRefreshToken.RefreshTokenExpiry > DateTime.Now && storedRefreshToken.Active == true;  // Ideally constant time comparison
        }
    }
}

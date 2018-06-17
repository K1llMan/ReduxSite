using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

using Common;

using Microsoft.IdentityModel.Tokens;

namespace Redux
{
    public class JWTUser
    {
        public string Name;
        public string Role;
    }

    public class JWTControl
    {
        #region Поля

        private Database db;
        private SecurityKey key;

        #endregion Поля

        #region Основные функции

        public JWTUser IsValidUser(string userName, string password)
        {
            var row = db.Query(
                "select *" +
                " from redux_users" +
                $" where \"Username\" = '{userName}'")
                .FirstOrDefault();

            if (row == null || row.Password.ToString() != password)
                return null;

            return new JWTUser { Name = row.Username.ToString(), Role = row.Role.ToString() };
        }

        public JwtSecurityToken GenerateToken(string userName, string password)
        {
            if (key == null)
                return null;

            JWTUser user = IsValidUser(userName, password);
            if (user == null)
                return null;

            Claim[] claims = {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(JwtRegisteredClaimNames.Exp, $"{new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}")
            };

            return new JwtSecurityToken(new JwtHeader(new SigningCredentials(key, SecurityAlgorithms.HmacSha256)), new JwtPayload(claims));
        }

        public JWTControl(Database database, string secretKey)
        {
            if (database != null)
                db = database;

            // Секретный ключ шифрования
            key = string.IsNullOrEmpty(secretKey) || secretKey.Length < 16
                ? null 
                : new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        }

        #endregion Основные функции
    }
}

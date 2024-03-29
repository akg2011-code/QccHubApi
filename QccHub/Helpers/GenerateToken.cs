﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using QccHub.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace QccHub.Helpers
{
    public static class GenerateToken
    {
        public static string GenerateJSONWebToken(List<Claim> claims)
        {
            string jwtKey = ConfigValueProvider.Get("Jwt:Key");
            string jwtIssuer = ConfigValueProvider.Get("Jwt:Issuer");
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var JWToken = new JwtSecurityToken(jwtIssuer, jwtIssuer, claims,
                          expires: DateTime.Now.AddDays(30), signingCredentials: credentials);

            string userToken = new JwtSecurityTokenHandler().WriteToken(JWToken);

            return userToken;
        }
    }
}

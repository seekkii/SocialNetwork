﻿using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SocialNetwork.API.Entities.User;
using SocialNetwork.API.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SocialNetwork.API.Authorization;

/// <summary>
/// Interface provides methods to JwtUtils class
/// </summary>
public interface IJwtUtils
{
    public string GenerateToken(User user);
    public Guid? ValidateToken(string token);
}

/// <summary>
/// Provide jwt auth helper functions
/// </summary>
public class JwtUtils : IJwtUtils
{
    #region Properties
    /// <summary>
    /// Provide app secret saved in appsettings.json
    /// </summary>
    private readonly AppSettings _appSettings;
    #endregion Properties

    #region Constructor
    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="appSettings">Provide app secret saved in appsettings.json</param>
    public JwtUtils(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }
    #endregion Constructor

    #region Methods
    /// <summary>
    /// Generate token that is valid for 7 days
    /// </summary>
    /// <param name="user">Auth user</param>
    /// <returns>Token</returns>
    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    /// <summary>
    /// Check if token is valid 
    /// </summary>
    /// <param name="token">Jwt token</param>
    /// <returns>
    /// <para>userId if token is valid</para>
    /// <para>null if token is invalid</para>
    /// </returns>
    public Guid? ValidateToken(string token)
    {
        if (token == null)
            return null;

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = Guid.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

            // return user id from JWT token if validation successful
            return userId;
        }
        catch
        {
            // return null if validation fails
            return null;
        }
    }
    #endregion Methods
}

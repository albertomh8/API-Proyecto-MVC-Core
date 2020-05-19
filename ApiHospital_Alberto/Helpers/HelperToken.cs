using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace ApiHospital_Alberto.Helpers
{
    public class HelperToken
    {
        public string issuer { get; set; }
        public string audience { get; set; }
        public string secretKey { get; set; }
        public HelperToken(IConfiguration configuration)
        {
            this.issuer = configuration["ApiAuth:Issuer"];
            this.audience = configuration["ApiAuth:Audience"];
            this.secretKey = configuration["ApiAuth:SecretKey"];
        }

        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = Encoding.UTF8.GetBytes(secretKey);
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtOptions()
        {
            Action<JwtBearerOptions> jwtoptions = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateActor = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = GetKeyToken()
                };
            });
            return jwtoptions;
        }

        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> authOptions = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return authOptions;
        }
    }
}

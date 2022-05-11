using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace PCIBusiness
{
	public static class JWT
	{
		public static Object CreateToken(string providerCode)
		{
			if ( providerCode == Tools.BureauCode(Constants.PaymentProvider.WorldPay) )
			{
				var secretKey   = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("46701669-0eda-4655-85ac-8be80f87bf1f"));
				var credentials = new SigningCredentials(secretKey,SecurityAlgorithms.HmacSha256);
				var data        = new List<Claim>();
				data.Add(new Claim("jti",Guid.NewGuid().ToString()));
				data.Add(new Claim("iat",DateTimeOffset.Now.ToUnixTimeSeconds().ToString()));
				data.Add(new Claim("iss","62300ab3aea0da4831173a58"));
				data.Add(new Claim("exp",DateTimeOffset.Now.AddHours(2).ToUnixTimeSeconds().ToString()));
				data.Add(new Claim("OrgUnitId","62300ab3a0c1f03a146a0c80"));

//				JwtHeader header = new JwtHeader();
//				header.Add("typ","JWT");
//				header.Add("alg","HS256");
//				JwtPayload payload = new JwtPayload();
//				payload.AddClaims(data);
//				var token    = new JwtSecurityToken(header,payload);

				var token    = new JwtSecurityToken(null,null,data,null,null,credentials);
				var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
				return jwtToken;
			}

			return null;
		}
	}
}

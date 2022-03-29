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

/* 3DS Flex iFrame

<iframe height="1" width="1" style="display: none;">
  <!-- This is a Cardinal Commerce URL in LIVE -->
  <form id="frm3D" method="POST" action="https://centinelapistag.cardinalcommerce.com/V1/Cruise/Collect">
  <!-- This is a Cardinal Commerce URL in TEST -->
  <form id="collectionForm" method="POST" action="https://secure-test.worldpay.com/shopper/3ds/ddc.html">
    <input type="hidden" name="Bin" value="4000000000001000" />
    <input type="hidden" name="JWT" value="eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI2OWFkYzE4NS0xNzQ4LTQ1MjUtOWVmOS00M2YyNTlhMWMyZDYiLCJpYXQiOjE1NDg4Mzg4NTUsImlzcyI6IjViZDllMGU0NDQ0ZGNlMTUzNDI4Yzk0MCIsIk9yZ1VuaXRJZCI6IjViZDliNTVlNDQ0NDc2MWFjMGFmMWM4MCJ9.qTyYn4rItMMNdnh6ouqW6ZmcCNzaG9JI_GdWGIaq6rY" />
  </form>
  <script>
    window.onload = function() {
      document.getElementById('frm3D').submit();
    }
  </script>
</iframe>

*/
			}

			return null;
		}
	}
}

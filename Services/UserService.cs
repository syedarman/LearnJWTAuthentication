using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LearnJWTAuthentication.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LearnJWTAuthentication.Services{
    public class UserService : IUserServices
    {
        private readonly AppSettings _appsettings;
        private List<UserEntity> userEntityList = new List<UserEntity>(){
            new UserEntity(){
                ID = 1,
                Firstname = "Syed",
                Lastname = "Arman",
                Username = "syed.arman",
                Password = "12345678"
            },
            new UserEntity(){
                ID = 1,
                Firstname = "Syed",
                Lastname = "Shihab",
                Username = "syed.shihab",
                Password = "123456780"
            }
        };
        
        public UserService(IOptions<AppSettings> appsettings)
        {
            _appsettings = appsettings.Value;
        }
        

        public AuthenticationResponse AuthenticateUser(AuthenticationRequest request)
        {
            UserEntity user = userEntityList.FirstOrDefault(user => user.Username == request.Username && user.Password == request.Password);

            if(user == null)
            {
                return null;
            }

            //generate token
            string token = GenerateToken(user);

            return new AuthenticationResponse(){
                Token = token
            };
        }

        public UserEntity GetUser(int id)
        {
            return userEntityList.FirstOrDefault(user => user.ID == id);
        }

        private string GenerateToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.ASCII.GetBytes(_appsettings.JWTSecret);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]{new Claim("ID",user.ID.ToString())}),
                Expires = DateTime.Now.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
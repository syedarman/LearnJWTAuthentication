using LearnJWTAuthentication.Models;

namespace LearnJWTAuthentication.Services
{
    public interface IUserServices
    {
        UserEntity GetUser(int id);
        AuthenticationResponse AuthenticateUser(AuthenticationRequest request);
    }
}
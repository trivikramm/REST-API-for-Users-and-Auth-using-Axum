using LoginDashboardAPI.Models;
using System.Threading.Tasks;

namespace LoginDashboardAPI.Services
{
    public interface IAuthService
    {
        Task<LoginResponse> AuthenticateAsync(LoginRequest request);
        Task<string> GenerateToken(User user);
    }
}
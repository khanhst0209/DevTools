using DevTools.Domain.Entities;
using DevTools.Dto.user;

namespace DevTools.Application.Services.Interfaces
{
    public interface IPremiumUpgradeRequestService
    {
        Task<List<UserDTO>> GetAllRequest();
        Task PremiumUpgradeSubmit(string userId);
        Task AcceptPremiumUpgradeRequest(string userId);

        Task DeninePremiumUpgradeRequest(string userId);
    }
}
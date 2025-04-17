using DevTools.Application.Dto.user;
using DevTools.Application.Services.Interfaces;
using DevTools.Domain.Entities;
using DevTools.Dto.user;
using DevTools.Exceptions.AccountManager.UserException;
using DevTools.Infrastructure.Repositories.Interfaces;
using MyWebAPI.Services.Interfaces;

namespace DevTools.Application.Services
{
    public class PremiumUpgradeRequestService : IPremiumUpgradeRequestService
    {
        private readonly IPremiumUpgradeRequestRepository _premiumUpgradeRepository;
        private readonly IAccountManagerService _accountManagerService;

        public PremiumUpgradeRequestService(IPremiumUpgradeRequestRepository premiumUpgradeRepository,
                                        IAccountManagerService accountManagerService)
        {
            _premiumUpgradeRepository = premiumUpgradeRepository;
            _accountManagerService = accountManagerService;
        }
        public async Task AcceptPremiumUpgradeRequest(string userId)
        {
            var temp = await _premiumUpgradeRepository.GetByIdAsync(userId);
            if (temp == null)
                throw new UserNotFound(userId);

            await _accountManagerService.RoleChange(new ChangeRoleDTO { UserId = userId, Role = "Premium" });
            await _premiumUpgradeRepository.RemoveAsync(userId);
        }

        public async Task DeninePremiumUpgradeRequest(string userId)
        {
            var temp = await _premiumUpgradeRepository.GetByIdAsync(userId);
            if (temp == null)
                throw new UserNotFound(userId);

            await _premiumUpgradeRepository.RemoveAsync(userId);
        }

        public async Task<List<UserDTO>> GetAllRequest()
        {
            var requests = await _premiumUpgradeRepository.GetAllAsync();
            var users = new List<UserDTO>();
            foreach(var request in requests)
            {
                users.Add(await _accountManagerService.GetUserById(request.UserId));
            }

            return users;
        }


        public async Task PremiumUpgradeSubmit(string userId)
        {
            var temp = await _premiumUpgradeRepository.GetByIdAsync(userId);
            if (temp != null)
                throw new Exception("User already submited Premium Upgrade before");
            await _premiumUpgradeRepository.AddAsync(new PremiumUpgradeRequest { UserId = userId });
        }
    }
}
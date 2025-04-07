using Microsoft.AspNetCore.Identity;
using MyWebAPI.data;

namespace DevTools.data.Seed
{
    public static class UserSeedData
    {
        public static async Task SeedUsersAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            var users = new List<(string fullName, string Email, string UserName, string Password, string Role)>
            {
                ("Phạm Nguyên Khánh", "pnkhanh22@clc.fitus.edu.vn", "khanhst0209", "Adminhcmus@123", "Admin"),
                ("Trịnh Anh Tài", "tatai22@clc.fitus.edu.vn", "trinhanhtai", "Adminhcmus@123", "Admin"),
                ("Trần Nhật Huy", "tnhuy22@clc.fitus.edu.vn", "trannhathuy", "Userhcmus@123", "User"),
                ("Nguyễn Hồng Quân", "nhquan@clc.fitus.edu.vn", "nguyenhongquan", "Premiumhcmus@123", "Premium")
            };

            foreach (var (fullName, email, username, password, role) in users)
            {
                if (await userManager.FindByEmailAsync(email) == null)
                {
                    var user = new User
                    {
                        FullName = fullName,
                        UserName = username,
                        Email = email,
                        IsPremium = true,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }
}

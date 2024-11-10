using Klochko_Site.Models;
using Microsoft.EntityFrameworkCore;

public class UserCredentialService
{
    private readonly GeneralPostDbContext _context;

    public UserCredentialService(GeneralPostDbContext context)
    {
        _context = context;
    }

    public async Task<UserCredential?> ValidateCredentials(string username, string password)
    {
        // Находим пользователя по логину и паролю без хэширования
        return await _context.UserCredentials
            .FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
    }
}

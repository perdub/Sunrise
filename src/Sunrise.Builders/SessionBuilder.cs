using Microsoft.Extensions.Logging;
using Sunrise.Types;
using Sunrise.Database;

namespace Sunrise.Builders;

public class SessionBuilder{
    private SunriseContext _context;
    private ILogger<SessionBuilder> _logger;

    public SessionBuilder(SunriseContext context, ILogger<SessionBuilder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Session BuildSession(Account a){
        var accountInDb = _context.Accounts.Find(a.AccountId);
        if(accountInDb is null){
            _logger.LogWarning("Fall to find account in database.");
            throw new NullReferenceException("Account not found.");
        }

        Session newSession = new Session{
            SessionId = SessionId(),
            Account = accountInDb
        };
        _context.Sessions.Add(newSession);

        _context.SaveChanges();

        return newSession;
    }

    private string SessionId(){
        var iHash = SharpHash.Base.HashFactory.Crypto.CreateSHA3_512();
        var hashResult = iHash.ComputeString(DateTime.UtcNow.Ticks.ToString(), System.Text.Encoding.UTF8);
        return hashResult.ToString();
    }
}
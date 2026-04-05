using System.Collections.Concurrent;

namespace EtudeReussie.Api.Services;

public class AdminSessionStore
{
    private readonly ConcurrentDictionary<string, DateTime> _tokens = new();

    public string CreateToken()
    {
        var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
            .Replace("/", string.Empty)
            .Replace("+", string.Empty)
            .Replace("=", string.Empty);

        _tokens[token] = DateTime.UtcNow;
        return token;
    }

    public bool IsValid(string? token)
    {
        return !string.IsNullOrWhiteSpace(token) && _tokens.ContainsKey(token);
    }

    public void Revoke(string? token)
    {
        if (!string.IsNullOrWhiteSpace(token))
        {
            _tokens.TryRemove(token, out _);
        }
    }
}

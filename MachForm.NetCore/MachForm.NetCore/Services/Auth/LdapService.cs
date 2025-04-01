using System.Data;
using System.DirectoryServices.Protocols;
using System.Net;
using MachForm.NetCore.Models.Account;
using Microsoft.Extensions.Options;

namespace MachForm.NetCore.Services.Auth;

public class LdapService : ILdapService
{
    private readonly LdapSettings _settings;
    private readonly ILogger<LdapService> _logger;

    public LdapService(IOptions<LdapSettings> settings, ILogger<LdapService> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public async Task<(bool success, string? error, LdapUserInfo? userInfo)> Authenticate(string username, string password)
    {
        try
        {
            using var adConnection = new LdapConnection(new LdapDirectoryIdentifier(Array.Empty<string>(), false, false))
            {
                AuthType = AuthType.Basic
            };

            var servers = _settings.Host.Split(',');
            var server = servers[new Random().Next(servers.Length)];

            adConnection.SessionOptions.SecureSocketLayer = _settings.Encryption == "ssl";
            adConnection.SessionOptions.ProtocolVersion = 3;
            adConnection.SessionOptions.VerifyServerCertificate = new VerifyServerCertificateCallback((con, cer) => true);

            adConnection.SessionOptions.HostName = server;
            adConnection.Timeout = new TimeSpan(0, 0, 30); // Set a network timeout instead of PortNumber

            if (_settings.Encryption == "tls")
            {
                adConnection.SessionOptions.StartTransportLayerSecurity(null);
            }

            var userDn = $"{username}{_settings.AccountSuffix}";
            var networkCredential = new NetworkCredential(userDn, password);
            adConnection.Bind(networkCredential);

            // Get user info
            var searchFilter = $"(sAMAccountName={username})";
            var attributes = new[] { "givenName", "sn", "mail", "memberOf" };

            var searchRequest = new SearchRequest(
                _settings.BaseDn,
                searchFilter,
                SearchScope.Subtree,
                attributes);

            var response = (SearchResponse)adConnection.SendRequest(searchRequest);

            if (response.Entries.Count == 0)
                return (false, "User not found in LDAP", null);

            var entry = response.Entries[0];
            var userInfo = new LdapUserInfo
            {
                Username = username,
                Email = entry.Attributes["mail"]?[0]?.ToString(),
                FullName = $"{entry.Attributes["givenName"]?[0]} {entry.Attributes["sn"]?[0]}"
            };

            // Check required groups
            if (!string.IsNullOrEmpty(_settings.RequiredGroup))
            {
                var requiredGroups = _settings.RequiredGroup.Split(',').Select(g => g.Trim()).ToList();
                var userGroups = entry.Attributes["memberOf"]?.GetValues(typeof(string)).Cast<string>().ToList() ?? new List<string>();

                if (!requiredGroups.Any(rg => userGroups.Any(ug => ug.Contains($"CN={rg},"))))
                    return (false, "You're not in an authorized group", null);
            }

            return (true, null, userInfo);
        }
        catch (LdapException ex)
        {
            _logger.LogError(ex, "LDAP authentication failed");
            return (false, "Incorrect login credentials (LDAP)", null);
        }
    }
}

using Renci.SshNet;
using Renci.SshNet.Common;


namespace MachForm.NetCore.Helpers;

public class SftpFileChecker : ISftpFileChecker
{
    private readonly ISftpClient _sftpClient;

    public SftpFileChecker(ISftpClient sftpClient)
    {
        _sftpClient = sftpClient;
    }

    public SftpFileChecker(string host, string username, string password)
    {
        _sftpClient = new SftpClient(host, username, password);
    }

    public SftpFileChecker(string host, int port, string username, string password)
    {
        _sftpClient = new SftpClient(host, port, username, password);
    }

    public bool IsWritable(string path)
    {
        if (!_sftpClient.IsConnected)
        {
            try
            {
                _sftpClient.Connect();
            }
            catch
            {
                return false;
            }
        }

        try
        {
            // Try to open the file in write mode
            using (var stream = _sftpClient.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                // If we got here, the file is writable
                // If the file didn't exist, it was created, so delete it
                if (!_sftpClient.Exists(path))
                {
                    _sftpClient.Delete(path);
                }
                return true;
            }
        }
        catch (SftpPathNotFoundException)
        {
            // Path doesn't exist - check if parent directory is writable
            var directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory))
            {
                directory = ".";
            }

            try
            {
                var testFile = Path.Combine(directory, Guid.NewGuid().ToString());
                using (var stream = _sftpClient.Create(testFile))
                {
                    _sftpClient.Delete(testFile);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        catch (SftpPermissionDeniedException)
        {
            return false;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Unexpected error while checking write permissions: {ex.Message}");
        }
    }

    public void Dispose()
    {
        if (_sftpClient.IsConnected)
        {
            _sftpClient.Disconnect();
        }
        _sftpClient.Dispose();
    }
}

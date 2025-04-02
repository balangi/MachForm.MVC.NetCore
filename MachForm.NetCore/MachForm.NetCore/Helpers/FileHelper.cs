using Renci.SshNet;
using Renci.SshNet.Common;

namespace MachForm.NetCore.Helpers;

public class FileHelper: IFileHelper
{
    private static ISftpClient? _sftpClient;

    public FileHelper(ISftpClient sftpClient)
    {
        _sftpClient = sftpClient;
    }

    public bool IsDirectoryWritable(string path)
    {
        if (!Precheck())
        {
            return false;
        }

        try
        {
            // Try to open the file in write mode to check if it's writable
            using (var stream = _sftpClient?.Open(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                return true;
            }
        }
        catch (SftpPathNotFoundException)
        {
            // File doesn't exist - check if we can create it
            try
            {
                using (var stream = _sftpClient?.Create(path))
                {
                    // If we got here, we could create the file, so it's writable
                    // Clean up by deleting the test file
                    _sftpClient.Delete(path);
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
            throw new InvalidOperationException($"Unexpected error while checking write permissions. Error: {ex.Message}");
        }
    }

    private bool Precheck()
    {
        // Implement any necessary prechecks here
        // For example, check if the SFTP client is connected
        return _sftpClient != null && _sftpClient.IsConnected;
    }
}
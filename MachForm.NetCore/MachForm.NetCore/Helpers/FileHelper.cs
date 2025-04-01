using System.IO;

namespace MachForm.NetCore.Helpers;

public static class FileHelper
{
    public static bool MfFullRmdir(string dirPath)
    {
        try
        {
            if (Directory.Exists(dirPath))
            {
                Directory.Delete(dirPath, true);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }
}
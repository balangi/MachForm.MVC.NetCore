namespace MachForm.NetCore.Helpers;

public interface ISftpFileChecker
{
    bool IsWritable(string path);
    void Dispose();
}
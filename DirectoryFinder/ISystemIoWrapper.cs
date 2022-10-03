namespace UnitTestProject1
{
    public interface ISystemIoWrapper
    {
        List<string> GetDirectories(string path, string searchPattern, SearchOption option = SearchOption.TopDirectoryOnly);
    }
}

namespace UnitTestProject1
{
    public class DefaultSystemIoWrapper : ISystemIoWrapper
    {
        public List<string> GetDirectories(string path, string searchPattern, SearchOption option)
        {
            return Directory.GetDirectories(path, searchPattern, option).ToList();
        }
    }
}

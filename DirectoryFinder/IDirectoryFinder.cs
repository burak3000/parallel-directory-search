namespace UnitTestProject1
{
    public interface IDirectoryFinder
    {
        List<string> GetDirectories(IDirectoryFindParameters parameters);
    }

    public interface IDirectoryFindParameters { };
}

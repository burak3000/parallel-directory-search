namespace UnitTestProject1
{
    public class RecursiveDirectoryFinderParameters : IDirectoryFindParameters
    {
        public string RootDirectory { get; set; }
        public string SearchPattern { get; set; } = "*";
        public int RecursionLevel { get; set; }
    }
}

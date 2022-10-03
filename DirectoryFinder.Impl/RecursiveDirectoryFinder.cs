using System.Collections.Concurrent;

namespace UnitTestProject1
{
    public class RecursiveDirectoryFinder : IDirectoryFinder
    {
        private ISystemIoWrapper m_SystemIoWrapper;

        public RecursiveDirectoryFinder(ISystemIoWrapper wrapper)
        {
            m_SystemIoWrapper = wrapper;
        }
        public List<string> GetDirectories(IDirectoryFindParameters parameters)
        {
            RecursiveDirectoryFinderParameters recursiveFinderParams = parameters as RecursiveDirectoryFinderParameters;
            if (recursiveFinderParams != null && recursiveFinderParams.RecursionLevel <= 0)
            {
                var directories = m_SystemIoWrapper.GetDirectories(recursiveFinderParams.RootDirectory,
                                                                   recursiveFinderParams.SearchPattern,
                                                                   System.IO.SearchOption.AllDirectories);
                return directories.ToList();
            }
            else
            {
                var directories = m_SystemIoWrapper.GetDirectories(recursiveFinderParams.RootDirectory,
                                                                   recursiveFinderParams.SearchPattern,
                                                                   System.IO.SearchOption.TopDirectoryOnly);

                ConcurrentBag<string> subDirectories = new ConcurrentBag<string>();
                Parallel.ForEach(directories, dir =>
                {
                    IDirectoryFindParameters subDirectoryFindParameters = new RecursiveDirectoryFinderParameters
                    {
                        RootDirectory = dir,
                        RecursionLevel = --recursiveFinderParams.RecursionLevel,
                        SearchPattern = recursiveFinderParams.SearchPattern
                    };
                    var subDirs = GetDirectories(subDirectoryFindParameters);
                    foreach (var subdir in subDirs)
                    {
                        subDirectories.Add(subdir);
                    }
                });
                directories.AddRange(subDirectories.ToList());
                return directories.ToList();
            }
        }
    }
}

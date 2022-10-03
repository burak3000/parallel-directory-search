using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace UnitTestProject1
{
    internal class RecursiveDirectoryFinderTest
    {
        int m_DummyDirCount = 0;
        int m_FirstLevelFolderCount = 0;
        int m_SecondLevelFoldercount = 0;
        string m_TestDir = Path.Combine(Path.GetTempPath(), "RecursiveDirectoryFinderTests");
        [SetUp]
        public void CreateTempStructure()
        {
            if (Directory.Exists(m_TestDir))
            {
                Directory.Delete(m_TestDir, true);
            }
            Directory.CreateDirectory(m_TestDir);
            CreateDummyFolderStructure(2);
        }

        [Test]
        public void GetDirectories_RecursionLevelIs0_ReturnsOnlyTopLevelDirectories()
        {
            Stopwatch sw = Stopwatch.StartNew();
            IDirectoryFinder finder = new RecursiveDirectoryFinder(new DefaultSystemIoWrapper());

            var directories = finder.GetDirectories(new RecursiveDirectoryFinderParameters
            {
                RecursionLevel = 0,
                RootDirectory = m_TestDir
            });

            Assert.AreEqual(m_DummyDirCount, directories.Count);
            sw.Stop();
            Console.WriteLine($"Recursion level 0 took: {sw.ElapsedMilliseconds}(ms)");
        }

        [Test]
        public void GetDirectories_RecursionLevelIs2_ReturnsOnlyTopLevelDirectories()
        {
            Stopwatch sw = Stopwatch.StartNew();

            IDirectoryFinder finder = new RecursiveDirectoryFinder(new DefaultSystemIoWrapper());

            var directories = finder.GetDirectories(new RecursiveDirectoryFinderParameters
            {
                RecursionLevel = 2,
                RootDirectory = m_TestDir
            });

            Assert.AreEqual(m_DummyDirCount, directories.Count);
            sw.Stop();
            Console.WriteLine($"Recursion level 2 took: {sw.ElapsedMilliseconds}(ms)");
            //mockIoWrapper.Verify(mock => mock.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<SearchOption>()), Times.Exactly(2));
        }

        private List<string> CreateDummyFolderStructure(int callCount)
        {
            List<string> allCreatedFolders = new List<string>();
            List<string> firstLevel = new List<string>();
            List<string> secondLevel = new List<string>();

            int firstLevelFolderCount = 10;

            for (int i = 0; i < firstLevelFolderCount; i++)
            {
                string directoryPath = Path.Combine(m_TestDir, "firstLevelDir" + i.ToString());
                Directory.CreateDirectory(directoryPath);
                m_FirstLevelFolderCount++;
                firstLevel.Add(directoryPath);
            }
            allCreatedFolders.AddRange(firstLevel);
            if (callCount == 1)
            {
                m_DummyDirCount = allCreatedFolders.Count;
                return allCreatedFolders;
            }
            foreach (var firstLevelFolder in firstLevel)
            {
                int secondLevelFolderCount = 25;
                for (int i = 0; i < secondLevelFolderCount; i++)
                {
                    string directoryPath = Path.Combine(firstLevelFolder, "secondLevelDir" + i.ToString());
                    Directory.CreateDirectory(directoryPath);
                    m_SecondLevelFoldercount++;
                    allCreatedFolders.Add(directoryPath);
                    secondLevel.Add(directoryPath);

                }
            }

            foreach (var secondLevelFolder in secondLevel)
            {
                int thirdLevelFolderCount = 25;
                for (int i = 0; i < thirdLevelFolderCount; i++)
                {
                    string directoryPath = Path.Combine(secondLevelFolder, "thirdLevelDir" + i.ToString());
                    Directory.CreateDirectory(directoryPath);
                    allCreatedFolders.Add(directoryPath);
                }
            }
            m_DummyDirCount = allCreatedFolders.Count;
            return allCreatedFolders;
        }
    }
}

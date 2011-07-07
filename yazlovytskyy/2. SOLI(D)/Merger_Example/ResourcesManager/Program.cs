using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using ResourceMerger.Merger;

namespace ResourceMerger
{
    public class Program
    {
        private static readonly List<string> ResourcesFileNames = new List<string>
                                                              {
                                                                  "Res_Eng.resx",
                                                                  "Res_Pol.resx",
                                                                  "Res_Rom.resx",
                                                                  "Res_Rus.resx",
                                                                  "Res_Ukr.resx"
                                                              };

        static void Main(string[] args)
        {
            ValidateArgs(args);

            var sourceFiles = GetResourcesFilesFromDir(GetNormalizedDirPath(args[0]));
            var destinationFiles = GetResourcesFilesFromDir(GetNormalizedDirPath(args[1]));

            Console.WriteLine("Resources merging started");
            MergeResources(sourceFiles.ToArray(), destinationFiles.ToArray());
            ExitWithSuccess("Resources merging finished successfully");
        }

        private static IEnumerable<string> GetResourcesFilesFromDir(string pathToDir)
        {

            var dir = new DirectoryInfo(pathToDir);
            var resources = new List<FileInfo>();
            foreach (var fileName in ResourcesFileNames)
            {
                resources.AddRange(dir.GetFiles(fileName, SearchOption.AllDirectories));
            }
            return from fileInfo in resources
                   select fileInfo.FullName;
        }

        private static string GetNormalizedDirPath(string pathToDir)
        {
            if (pathToDir.Contains(":"))
            {
                return pathToDir;
            }
            return AppDomain.CurrentDomain.BaseDirectory + pathToDir;
        }

        private static void ValidateArgs(string[] args)
        {
            if (args.Length < 2)
            {
                ExitWithError("Please input two arguments wich are paths to directories");
            }

            var existenceValidationMessages = (from arg in args
                                               where !Directory.Exists(arg)
                                               select "Directory '" + arg + "' doesn't exist"
                                               ).ToList();

            if (existenceValidationMessages.Count > 0)
            {
                ExitWithError(String.Join(Environment.NewLine, existenceValidationMessages));
            }
        }

        private static void MergeResources(IList<string> sourceFiles, IList<string> destinationFiles)
        {
            for (var i = 0; i < sourceFiles.Count(); i++)
            {
                MergeFiles(sourceFiles[i], destinationFiles[i]);
            }
        }

        public static void MergeFiles(string source, string destination)
        {
            try
            {
                var sourceXml = new XmlDocument();
                sourceXml.Load(source);
                var destinationXml = new XmlDocument();
                destinationXml.Load(destination);
                var sourceFileName = new FileInfo(source).Name;
                Console.WriteLine("Merging " + sourceFileName + "...");
                Merger.Merger.MergeNodes(sourceXml, destinationXml);
                destinationXml.Save(destination);
            }
            catch (Exception e)
            {
                ExitWithError("Merging failed. " +
                                e.Message + " " +
                                Environment.NewLine +
                                "Stack Trace: " + e.StackTrace);
            }
        }

        private static void ExitWithError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            Environment.Exit(2);
        }

        private static void ExitWithSuccess(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}

using System.Collections.Generic;

namespace Visyn.Build.VisualStudio.CsProj
{
    public class CsProjComparison
    {
        public static IEnumerable<string> Compare(VisualStudioCsProject project1, VisualStudioCsProject project2, bool verbose)
        {
            var project1Missing = new List<ProjectFile>();
            var project2Missing = new List<ProjectFile>();

            //if(project1.SourceFiles.Count == project2.SourceFiles.Count)
            {
                foreach(var file1 in project1.SourceFiles)
                {
                    if(!project2.SourceFiles.Contains(file1))
                    {
                        project2Missing.Add(file1);
                    }
                }
                foreach(var file2 in project2.SourceFiles)
                {
                    if(project1.SourceFiles.Contains(file2))
                    {
                        project1Missing.Add(file2);
                    }
                }
            }
            yield return $"{project1.ProjectFilename} Missing {project1Missing.Count} files";
            if(verbose)
            {
                foreach (var file in project1Missing) yield return $"{file.FileName}";
            }
            yield return $"{project2.ProjectFilename} Missing {project2Missing.Count} files.";
            if (verbose)
            {
                foreach (var file in project2Missing) yield return $"{file.FileName}";
            }
        }
    }
}

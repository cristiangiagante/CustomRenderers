using System;
using System.Collections.Generic;
using System.IO;

namespace AddCustomFonts
{
    class UWP : Project
    {
        private DirectoryInfo _solutionName;
        private List<string> _files;
        private string _resourceFolder = "Assets\\Fonts";
        public UWP(DirectoryInfo solutionName, List<string> files)
        {
            _solutionName = solutionName;
            _files = files;
        }
        public void CreateResources()
        {
            var projectName = new DirectoryInfo(Path.Combine(_solutionName.FullName, _solutionName.Name, _solutionName.Name + ".UWP"));
            projectName.CreateSubdirectory(_resourceFolder);
            var resourceFolder = new DirectoryInfo(Path.Combine(projectName.FullName, _resourceFolder));
            var rootPath = _solutionName.FullName.Split(new string[] { _solutionName.Name }, StringSplitOptions.RemoveEmptyEntries);
            var tempPath = "";
            var csProjFile = Path.Combine(rootPath[0], _solutionName.Name, _solutionName.Name, projectName.Name, _solutionName.Name + ".UWP.csproj");
            var csProjFileRead = File.OpenText(csProjFile);
            var content = csProjFileRead.ReadToEnd();
            csProjFileRead.Close();
            var nuevocsProjFile = File.Create(csProjFile);
            var csProjFileWrite = new StreamWriter(nuevocsProjFile);
            var keyAndroidAsset = "<None Include=\"project.json\" />";
            var fontContent = "";
            foreach (string file in _files)
            {
                var fileInfo = new FileInfo(file);
                tempPath = Path.Combine(rootPath[0], _solutionName.Name, _solutionName.Name, projectName.Name, _resourceFolder, fileInfo.Name);
                if (!File.Exists(tempPath))
                {
                    File.Copy(file, tempPath);
                    var nameContentFile = $"\"Assets\\Fonts\\{fileInfo.Name}\"";
                    fontContent += $@"
    <Content Include={nameContentFile} />
    ";
                }

            }

            if (!string.IsNullOrEmpty(content))
            {
                int indexUWPAsset = content.LastIndexOf(keyAndroidAsset);
                var newContent = content.Insert(indexUWPAsset, Environment.NewLine + fontContent + Environment.NewLine);
                csProjFileWrite.WriteLine(newContent.Replace(Environment.NewLine + Environment.NewLine, ""));
                csProjFileWrite.Close();
            }

        }
    }
}

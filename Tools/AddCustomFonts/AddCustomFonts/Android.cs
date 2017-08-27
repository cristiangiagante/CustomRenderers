using System;
using System.Collections.Generic;
using System.IO;

namespace AddCustomFonts
{
    class Android : Project
    {
        private DirectoryInfo _solutionName;
        private List<string> _files;
        private string _resourceFolder = "Assets\\Fonts";
        public Android(DirectoryInfo solutionName, List<string> files)
        {
            _solutionName = solutionName;
            _files = files;
        }
        public void CreateResources()
        {
            var projectName = new DirectoryInfo(Path.Combine(_solutionName.FullName, _solutionName.Name, _solutionName.Name + ".Android"));
            projectName.CreateSubdirectory(_resourceFolder);
            var resourceFolder = new DirectoryInfo(Path.Combine(projectName.FullName, _resourceFolder));
            var rootPath = _solutionName.FullName.Split(new string[] { _solutionName.Name }, StringSplitOptions.RemoveEmptyEntries);
            var tempPath = "";
            var csProjFile = Path.Combine(rootPath[0], _solutionName.Name, _solutionName.Name, projectName.Name, _solutionName.Name + ".Android.csproj");
            var csProjFileRead = File.OpenText(csProjFile);
            var content = csProjFileRead.ReadToEnd();
            csProjFileRead.Close();
            var nuevocsProjFile = File.Create(csProjFile);
            var csProjFileWrite = new StreamWriter(nuevocsProjFile);
            var keyAndroidAsset = "<None Include=\"packages.config\" />";
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
    <AndroidAsset Include={nameContentFile} />
    ";
                }

            }

            if (!string.IsNullOrEmpty(content))
            {
                int indexAndroidAsset = content.LastIndexOf(keyAndroidAsset);
                var newContent = content.Insert(indexAndroidAsset, Environment.NewLine + fontContent + Environment.NewLine);
                csProjFileWrite.WriteLine(newContent.Replace(Environment.NewLine + Environment.NewLine, ""));
                csProjFileWrite.Close();
            }
        }
    }
}

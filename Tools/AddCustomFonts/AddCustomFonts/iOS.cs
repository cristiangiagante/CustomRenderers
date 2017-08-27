using System;
using System.Collections.Generic;
using System.IO;

namespace AddCustomFonts
{
    class iOS : Project
    {
        private DirectoryInfo _solutionName;
        private List<string> _files;
        private string _resourceFolder = "Resources\\Fonts";
        private string _plistFile = "info.plist";
        public iOS(DirectoryInfo solutionName, List<string> files)
        {
            _solutionName = solutionName;
            _files = files;
        }
        public void CreateResources()
        {
            var projectName = new DirectoryInfo(Path.Combine(_solutionName.FullName, _solutionName.Name, _solutionName.Name + ".iOS"));
            projectName.CreateSubdirectory(_resourceFolder);
            var resourceFolder = new DirectoryInfo(Path.Combine(projectName.FullName, _resourceFolder));
            var rootPath = _solutionName.FullName.Split(new string[] { _solutionName.Name }, StringSplitOptions.RemoveEmptyEntries);
            var plistPath = Path.Combine(rootPath[0], _solutionName.Name, _solutionName.Name, projectName.Name, _plistFile);
            var plistFileRead = File.OpenText(plistPath);
            var content = plistFileRead.ReadToEnd();
            plistFileRead.Close();
            var nuevoPlistFile = File.Create(plistPath);
            var plistFileWrite = new StreamWriter(nuevoPlistFile);
            var keyPlistInfo = @"<key>UIAppFonts</key>
	<array>";
            var tempPath = "";
            string fontContent = "";
            foreach (string file in _files)
            {
                var fileInfo = new FileInfo(file);
                tempPath = Path.Combine(rootPath[0], _solutionName.Name, _solutionName.Name, projectName.Name, _resourceFolder, fileInfo.Name);
                if (!File.Exists(tempPath))
                {
                    File.Copy(file, tempPath);
                    fontContent += $@"<string>Fonts/{fileInfo.Name}</string>{Environment.NewLine}";
                }
            }
            if (!string.IsNullOrEmpty(fontContent))
            {
                if (content.Contains("UIAppFonts"))
                {
                    int indexUIAppFonts = content.LastIndexOf(keyPlistInfo);
                    int indexOffsetUIAppFonts = keyPlistInfo.Length;
                    var newContent = content.Insert(indexUIAppFonts + indexOffsetUIAppFonts, Environment.NewLine + fontContent + Environment.NewLine);
                    plistFileWrite.WriteLine(newContent);
                    plistFileWrite.Close();
                }
                else
                {
                    int indexUIAppFonts = content.LastIndexOf("</dict>");
                    var newFontContent = keyPlistInfo + fontContent + $"</array>";
                    var newContent = content.Insert(indexUIAppFonts, Environment.NewLine + newFontContent + Environment.NewLine);
                    plistFileWrite.WriteLine(newContent);
                    plistFileWrite.Close();
                }
            }
            else
            {
                plistFileWrite.WriteLine(content);
                plistFileWrite.Close();
            }
        }
    }
}

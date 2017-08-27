using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddCustomFonts
{
    public partial class Form1 : Form
    {
        List<string> Files = new List<string>();
        public Form1()
        {
            InitializeComponent();
            MessageBox.Show("This tool is beta version, please make a backup of your proyect before use it.");
        }

        private void Button_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            SolutionPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var selections = checkedListBox1.CheckedItems;
                if (selections.Count != 0 && !string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath) && openFileDialog1.FileNames.Length > 0 && !openFileDialog1.FileNames[0].Contains("openFileDialog1"))
                {
                    DirectoryInfo solutionDir = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
                    Project project;

                    foreach (string selection in selections)
                    {
                        switch (selection)
                        {
                            case "Android":
                                project = new Android(solutionDir, Files);
                                project.CreateResources();
                                break;
                            case "iOS":
                                project = new iOS(solutionDir, Files);
                                project.CreateResources();
                                break;
                            case "UWP":
                                project = new UWP(solutionDir, Files);
                                project.CreateResources();
                                break;
                        }
                    }
                    MessageBox.Show("Finish! Fonts has been added.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error!: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            foreach (string file in openFileDialog1.FileNames)
            {
                comboBox1.Items.Add(file);
                Files.Add(file);
            };

        }
    }
    public enum ProjectTypes
    {
        Android = 1,
        iOS = 2,
        UWP = 3

    }
}
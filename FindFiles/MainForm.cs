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

namespace FindFiles
{
    public partial class MainForm : Form
    {
        List<string> receivedFileNames = new List<string>();
        Timer UITimer = new Timer();
        Model model;
        TreeView treeView;
        Label pathLabel;
        Label regexLabel;
        TextBox pathTextBox;
        TextBox regexTextBox;
        Button startButton;
        Button pauseButton;
        Button unpauseButton;
        Button stopButton;
        Label currentDirLabel;
        Label currentDir;
        Label foundFilesAmountLabel;
        Label foundFilesAmount;
        Label allFilesAmountLabel;
        Label allFilesAmount;
        Label timeFromStartLabel;
        Label timeFromStart;

        public MainForm(Model model, Controller controller)
        {
            this.model = model;
            this.Size = new Size(800, 600);

            this.FormClosed+= (sender, args) => OnFormClosed();

            CreateControls();
            Controls.Add(CreateLayouts());
            UploadParameters();

            startButton.Click += (sender, args) => controller.StartSearch(pathTextBox.Text,regexTextBox.Text);
            pauseButton.Click += (sender, args) => controller.PauseSearch();
            unpauseButton.Click += (sender, args) => controller.UnpauseSearch();
            stopButton.Click += (sender, args) => controller.StopSearch();

            model.TreeElementAdded += OnTreeAdded;
            model.TreeCleared += OnTreeCleared;
            InitializeTimer();
        }

        private void OnFormClosed()
        {
            try
            {
                string allParams = pathTextBox.Text + "\n" + regexTextBox.Text;
                File.WriteAllText(@"..\parameters.txt", allParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void OnTreeCleared()
        {
            treeView.Nodes.Clear();
            currentDir.Text = "";
        }

        private void OnTreeAdded(string name)
        {
            lock (receivedFileNames)
            {
                receivedFileNames.Add(name);
            }
        }

        private void UploadParameters()
        {
            try
            {
                var parameters = File.ReadLines(@"..\parameters.txt").ToList();
                pathTextBox.Text = parameters[0];
                regexTextBox.Text = parameters[1];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private void CreateControls()
        {
            treeView = new TreeView()
            {
                Dock = DockStyle.Fill
            };
            pathLabel = new Label
            {
                Text = "Path:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            regexLabel = new Label
            {
                Text = "RegEx:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            pathTextBox = new TextBox
            {
                Dock = DockStyle.Fill
            };

            regexTextBox = new TextBox
            {
                Dock = DockStyle.Fill
            };
            startButton = new Button
            {
                Text = "Start",
                Dock = DockStyle.Fill
            };
            pauseButton = new Button
            {
                Text = "Pause",
                Dock = DockStyle.Fill
            };
            unpauseButton = new Button
            {
                Text = "Unpause",
                Dock = DockStyle.Fill
            };
            stopButton = new Button
            {
                Text = "Stop",
                Dock = DockStyle.Fill
            };
            currentDir = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            foundFilesAmount = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            allFilesAmount = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            timeFromStart = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            currentDirLabel = new Label
            {
                Text = "Current directory:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            foundFilesAmountLabel = new Label
            {
                Text = "Number of found files:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
            allFilesAmountLabel = new Label
            {
                Text = "Number of all files:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };

            timeFromStartLabel = new Label
            {
                Text = "Time from start:",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft
            };
        }
        private TableLayoutPanel CreateLayouts()
        {
            var pathTextLayout = new TableLayoutPanel();
            pathTextLayout.ColumnStyles.Clear();
            pathTextLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            pathTextLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            pathTextLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            pathTextLayout.Controls.Add(pathLabel, 0, 0);
            pathTextLayout.Controls.Add(pathTextBox, 1, 0);
            pathTextLayout.Dock = DockStyle.Fill;

            var regexTextLayout = new TableLayoutPanel();
            regexTextLayout.ColumnStyles.Clear();
            regexTextLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));
            regexTextLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            regexTextLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            regexTextLayout.Controls.Add(regexLabel, 0, 0);
            regexTextLayout.Controls.Add(regexTextBox, 1, 0);
            regexTextLayout.Dock = DockStyle.Fill;

            var currentDirLayout = new TableLayoutPanel();
            currentDirLayout.ColumnStyles.Clear();
            currentDirLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            currentDirLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            currentDirLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            currentDirLayout.Controls.Add(currentDirLabel, 0, 0);
            currentDirLayout.Controls.Add(currentDir, 1, 0);
            currentDirLayout.Dock = DockStyle.Fill;

            var foundFilesAmountLayout = new TableLayoutPanel();
            foundFilesAmountLayout.ColumnStyles.Clear();
            foundFilesAmountLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            foundFilesAmountLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            foundFilesAmountLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            foundFilesAmountLayout.Controls.Add(foundFilesAmountLabel, 0, 0);
            foundFilesAmountLayout.Controls.Add(foundFilesAmount, 1, 0);
            foundFilesAmountLayout.Dock = DockStyle.Fill;

            var allFilesAmountLayout = new TableLayoutPanel();
            allFilesAmountLayout.ColumnStyles.Clear();
            allFilesAmountLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            allFilesAmountLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            allFilesAmountLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            allFilesAmountLayout.Controls.Add(allFilesAmountLabel, 0, 0);
            allFilesAmountLayout.Controls.Add(allFilesAmount, 1, 0);
            allFilesAmountLayout.Dock = DockStyle.Fill;

            var timeFromStartLayout = new TableLayoutPanel();
            timeFromStartLayout.ColumnStyles.Clear();
            timeFromStartLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
            timeFromStartLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            timeFromStartLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            timeFromStartLayout.Controls.Add(timeFromStartLabel, 0, 0);
            timeFromStartLayout.Controls.Add(timeFromStart, 1, 0);
            timeFromStartLayout.Dock = DockStyle.Fill;


            var leftLayout = new TableLayoutPanel();
            leftLayout.RowStyles.Clear();
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            leftLayout.Controls.Add(treeView, 0, 0);
            leftLayout.Dock = DockStyle.Fill;

            var rightLayout = new TableLayoutPanel();
            rightLayout.RowStyles.Clear();
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20));
            for (int i=0;i<8;i++)
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
            rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            rightLayout.Controls.Add(new Panel(), 0, 0);
            rightLayout.Controls.Add(pathTextLayout, 0, 1);
            rightLayout.Controls.Add(regexTextLayout, 0, 2);
            rightLayout.Controls.Add(new Panel(), 0, 3);
            rightLayout.Controls.Add(startButton, 0, 4);
            rightLayout.Controls.Add(pauseButton, 0, 5);
            rightLayout.Controls.Add(unpauseButton, 0, 6);
            rightLayout.Controls.Add(stopButton, 0, 7);
            rightLayout.Controls.Add(currentDirLayout, 0, 8);
            rightLayout.Controls.Add(foundFilesAmountLayout, 0, 9);
            rightLayout.Controls.Add(allFilesAmountLayout, 0, 10);
            rightLayout.Controls.Add(timeFromStartLayout, 0, 11);

            rightLayout.Controls.Add(new Panel(), 0, 12);
            rightLayout.Dock = DockStyle.Fill;

            var mainLayout = new TableLayoutPanel();
            mainLayout.ColumnStyles.Clear();
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));
            mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainLayout.Controls.Add(leftLayout, 0, 0);
            mainLayout.Controls.Add(rightLayout, 1, 0);
            mainLayout.Dock = DockStyle.Fill;

            return mainLayout;
        }
        private void InitializeTimer()
        {
            UITimer.Interval = 100;
            UITimer.Tick += new EventHandler(UITimer_Tick);

            UITimer.Enabled = true;          
        }
        private void UITimer_Tick(object Sender, EventArgs e)
        {
            lock (receivedFileNames)
            {
                foreach (var record in receivedFileNames)
                    AddItemToTree(record);
                receivedFileNames.Clear();              
            }
            foundFilesAmount.Text = model.NumberOfMatchingElements.ToString();
            allFilesAmount.Text = model.NumberOfAllElements.ToString();
            timeFromStart.Text = model.MeasuredTime.ToString(@"dd\.hh\:mm\:ss\:ff");
        }

        private void AddItemToTree(string fullName)
        {
            List<string> path = fullName.Split('\\').ToList();
            string fileName = path.Last();
            path.RemoveAt(path.Count - 1);

            UpdateTree(path, fileName);
            currentDir.Text= fullName;
        }

        private void UpdateTree(List<string> path, string fileName)
        {
            treeView.BeginUpdate();

            TreeNodeCollection treeNodeCollection = FindTreeNodeCollection(path);
            treeNodeCollection.Add(fileName, fileName);

            treeView.EndUpdate();
        }

        private TreeNodeCollection FindTreeNodeCollection(List<string> path)
        {
            TreeNodeCollection treeNodeCollection = treeView.Nodes;

            foreach (var dirName in path)
            {
                TreeNode newTreeNode = treeNodeCollection.Find(dirName, false).FirstOrDefault();
                if (newTreeNode == null)
                    newTreeNode = treeNodeCollection.Add(dirName, dirName);
                treeNodeCollection = newTreeNode.Nodes;
            }

            return treeNodeCollection;
        }
    }
}

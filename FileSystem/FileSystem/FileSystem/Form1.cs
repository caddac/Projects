using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FileSystem;
using System.Windows.Controls;
using System.IO;
namespace FileSystem
{
    public partial class frmFileSystem : Form
    {
        private readonly int diskSizeMultiplier = 1048576;
        BindingList<string> list = new BindingList<string>();
        BindingList<string> fileNames = new BindingList<string>();


        public frmFileSystem()
        {
            InitializeComponent();
            cbMounted.DataSource = list;
            lstbxFilesList.DataSource = fileNames;
        }


        private void btnPut_Click(object sender, EventArgs e)
        {

            if (lstbxFilesList.SelectedItem == null)
                return;
            if (cbMounted.SelectedItem == null)
                return;

            string fileName = lstbxFilesList.SelectedItem.ToString();
            string path = string.Empty;
            string diskName = cbMounted.SelectedItem.ToString();
            if (string.IsNullOrEmpty(diskName))
            {
                MessageBox.Show("Please pick a disk!");
                return;
            }
            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show("Please pick a file!");
                return;
            }
            using (FolderBrowserDialog fbd2 = new FolderBrowserDialog())
            {
                if (fbd2.ShowDialog() != DialogResult.OK)
                    return;
                path = fbd2.SelectedPath;

                DiskController.PutFileToOSFromFS(cbMounted.SelectedItem.ToString(), fileName, path);
            }
        }


        private void btnGet_Click(object sender, EventArgs e)
        {
            if (cbMounted.SelectedItem == null)
                return;
                
            string   diskName = cbMounted.SelectedItem.ToString();
            string fileName = string.Empty;
            byte[] file = null;
            bool success = false;
            if (string.IsNullOrEmpty(diskName))
            {
                MessageBox.Show("Please pick a disk!");
                return;
            }

            using (OpenFileDialog openFileDialog2 = new OpenFileDialog())
            {
                if (openFileDialog2.ShowDialog() != DialogResult.OK)
                    return;
                fileName = openFileDialog2.FileName;
                file = File.ReadAllBytes(fileName);

                string[] parts = fileName.Split('\\');
                if (parts[parts.Length - 1].Length > 16)
                {
                    MessageBox.Show("The selected file name is too long.  Please rename the file and try again");
                    return;
                }
                
                try
                {
                    DiskController.WriteFileToDisk(diskName, parts[parts.Length-1] , file);
                    List<string> files = DiskController.GetListFilesOnDiskAsStrings(cbMounted.SelectedItem.ToString());
                    fileNames.Clear();
                    foreach (string s in files)
                        fileNames.Add(s);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was a problem writing the file.  Please choose another file" + ex.ToString());
                }
            }
            //if (!success)
            //    MessageBox.Show("Problem writing file. Oops!");
        }





        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtbxFSName.Text))
                return;
            string diskName = txtbxFSName.Text;
            int diskSize = (int)numericUpDown.Value;//MB conversion!!

            if (string.IsNullOrEmpty(diskName))
            {
                MessageBox.Show("Please enter a disk name.");
                return;
            }
            DiskController.CreateDisk(diskSize, diskName);
            
            txtbxFSName.Text = "";

            List<string> names = DiskController.GetMountedDiskNames();
            list.Clear();
            foreach (string s in names)
                list.Add(s);
        }



        private void btnSaveFS_Click(object sender, EventArgs e)
        {
            if (cbMounted.SelectedItem == null)
                return;
            string diskName = cbMounted.SelectedItem.ToString();
            string path = string.Empty;
            using (FolderBrowserDialog fbd1 = new FolderBrowserDialog())
            {
                if (fbd1.ShowDialog() != DialogResult.OK)
                    return;
                path = fbd1.SelectedPath;

                DiskController.SaveFsToOS(path, diskName);
            }
        }



        private void btnMountBrowse_Click(object sender, EventArgs e)
        {
//            byte[] file;
            string filename;
            bool success = false;
            using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
            {
                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;
                filename = openFileDialog1.FileName;
                //file = File.ReadAllBytes(filename);

                try
                {
                   success =  DiskController.MountExistingDiskToFS(filename);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("There was a problem mounting the File System.  Please choose another file" +ex.ToString());
                }
            }
            if (!success)
                MessageBox.Show("Problem mounting File System.  It may be already mounted.");

            List<string> names = DiskController.GetMountedDiskNames();
            list.Clear();

            foreach (string s in names)
                list.Add(s);
            cbMounted_SelectedIndexChanged(null, null);
        }



        private void btnUnMount_Click(object sender, EventArgs e)
        {
            if (cbMounted.SelectedItem == null)
                return;
            string diskName = cbMounted.SelectedItem.ToString();
            DiskController.UnMountDiskFromFSByName(diskName);

            List<string> names = DiskController.GetMountedDiskNames();
            list.Clear();
            foreach (string s in names)
                list.Add(s);

            cbMounted_SelectedIndexChanged(null, null);
        }

        private void cbMounted_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbMounted.SelectedItem == null)
            {
                fileNames.Clear();
                return;
            }
            List<string> files = DiskController.GetListFilesOnDiskAsStrings(cbMounted.SelectedItem.ToString());
            fileNames.Clear();
            foreach (string s in files)
                fileNames.Add(s);
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lstbxFilesList.SelectedItem == null)
                return;

            string fileName = lstbxFilesList.SelectedItem.ToString();
            string diskName = cbMounted.SelectedItem.ToString();

            DiskController.DeleteFileFromDisk(diskName, fileName);
            List<string> files = DiskController.GetListFilesOnDiskAsStrings(cbMounted.SelectedItem.ToString());
            fileNames.Clear();
            foreach (string s in files)
                fileNames.Add(s);

        }
    }
}



using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HeadFirst__ExcusesManagement
{
    public partial class Form1 : Form
    {
        Excuse currentExcuse;
        bool notSavedChanges;
        string folderPath = "";
        public Form1()
        {
            InitializeComponent();
            notSavedChanges = false;
            currentExcuse = new Excuse();
            currentExcuse.LastUsed = lastUsed.Value;
            saveButton.Enabled = false;
            openButton.Enabled = false;
            randomButton.Enabled = false;
        }

        private void UpdateForm(bool changed)
        {
            if (!changed)
            {
                description.Text = currentExcuse.Description;
                results.Text = currentExcuse.Results;
                lastUsed.Value = currentExcuse.LastUsed;
                if (!String.IsNullOrEmpty(currentExcuse.ExcusePath))
                    fileDate.Text = File.GetLastWriteTime(currentExcuse.ExcusePath).ToString();
                Text = "Program do zarządzania wymówkami";
            }
            else
                Text = "Program do zarządzania wymówkami*";
            notSavedChanges = changed;
        }

        private void Description_TextChanged(object sender, EventArgs e)
        {
            notSavedChanges = true;
            UpdateForm(notSavedChanges);
        }

        private void Results_TextChanged(object sender, EventArgs e)
        {
            notSavedChanges = true;
            UpdateForm(notSavedChanges);
        }

        private void LastUsed_ValueChanged(object sender, EventArgs e)
        {
            notSavedChanges = true;
            UpdateForm(notSavedChanges);
        }

        private void FolderButton_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                folderPath = folderBrowserDialog1.SelectedPath;
                saveButton.Enabled = true;
                openButton.Enabled = true;
                randomButton.Enabled = true;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(description.Text) || String.IsNullOrEmpty(results.Text))
            {
                MessageBox.Show("Określ wymówkę i rezultat", "Nie można zapisać pliku", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                saveFileDialog1.InitialDirectory = folderPath;
                saveFileDialog1.FileName = description.Text + ".txt";
                saveFileDialog1.Filter = "Pliki tekstowe (*.txt)|*.txt|" +
                    "Wszystkie pliki (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentExcuse.Description = description.Text;
                    currentExcuse.Results = results.Text;
                    currentExcuse.LastUsed = lastUsed.Value;
                    currentExcuse.ExcusePath = saveFileDialog1.FileName;

                    string text = currentExcuse.Description + Environment.NewLine + currentExcuse.Results +
                        Environment.NewLine + currentExcuse.LastUsed;
                    currentExcuse.Save(currentExcuse.ExcusePath, text);

                    notSavedChanges = false;
                    UpdateForm(notSavedChanges);

                    MessageBox.Show("Wymówka zapisana", "Zapisano plik", MessageBoxButtons.OK);
                }
            }
        }

        private void OpenButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = folderPath;
            openFileDialog1.Filter = "Pliki tekstowe (*.txt)|*.txt|" +
                    "Wszystkie pliki (*.*)|*.*";
            if (notSavedChanges)
            {
                if (MessageBox.Show("Bieżąca wymówka nie została zapisana. Czy kontynuować?",
                    "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.No)
                {
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        currentExcuse = new Excuse(openFileDialog1.FileName);
                        currentExcuse.ExcusePath = openFileDialog1.FileName;
                        notSavedChanges = false;
                        UpdateForm(notSavedChanges);
                    }
                }
            }
            else
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentExcuse = new Excuse(openFileDialog1.FileName);
                    currentExcuse.ExcusePath = openFileDialog1.FileName;
                    notSavedChanges = false;
                    UpdateForm(notSavedChanges);
                }
            }

        }

        private void RandomButton_Click(object sender, EventArgs e)
        {
            if (notSavedChanges)
            {
                if (MessageBox.Show("Bieżąca wymówka nie została zapisana. Czy kontynuować?",
                    "Ostrzeżenie", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.No)
                {
                    Random random = new Random();
                    string[] filePaths = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                    currentExcuse = new Excuse(filePaths, random);
                    notSavedChanges = false;
                    UpdateForm(notSavedChanges);
                }
            }
            else
            {
                Random random = new Random();
                string[] filePaths = Directory.GetFiles(folderPath, "*.txt", SearchOption.AllDirectories);
                currentExcuse = new Excuse(filePaths, random);
                notSavedChanges = false;
                UpdateForm(notSavedChanges);
            }
        }
    }
}

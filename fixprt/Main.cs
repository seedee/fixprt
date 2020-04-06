/*
 * fixprt
 * Copyright 06/4/2020
 * by seedee
 * 
 * fixprt is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * fixprt is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with fixprt. If not, see <https://www.gnu.org/licenses/>.
 */
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace fixprt
{
    public partial class Main : Form
    {
        string filename = String.Empty; // Current file name
        string filenamesafe = String.Empty; // Current file name without path
        string prt = String.Empty; // Imported portal file contents
        string prtfix = String.Empty; // Fixed portal file contents
        bool fixd = false; // The state of an opened portal file
        bool unsaved = false; // The state of a fixed portal file
        bool oldautofix; // Gets settings before options form opened
        bool oldautosave; // Gets settings before options form opened
        Regex open = new Regex(@"[^\.\-\(\)\s\d]"); // Match valid portal file characters
        Regex fix = new Regex(@"(?<=^(?:.*[\n]+){2})(?:(?!.*[()]).*[\r\n]+)+"); // Match lines after first 2 and before those with parentheses

        public Keys ShortcutKeys { get; set; }

        public Main()
        {
            InitializeComponent();
            console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Nothing loaded" + Environment.NewLine);
        }

        private void fixToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixPortalFile();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPortalFile(false);
        }
        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenPortalFile(true);
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePortalFile(false);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SavePortalFile(true);
        }
        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (unsaved == true)
            {
                switch (MessageBox.Show("Do you want to save changes to " + filenamesafe + "?",
                         "fixprt",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SavePortalFile(false);
                        ClosePortalFile();
                        break;
                    case DialogResult.No:
                        ClosePortalFile();
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else if (unsaved == false)
            {
                ClosePortalFile();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Options optionsForm = new Options())
            {
                oldautofix = Properties.Settings.Default.autofix;
                oldautosave = Properties.Settings.Default.autosave;
                switch (optionsForm.ShowDialog(this))
                {
                    case DialogResult.OK:
                        if (Properties.Settings.Default.autofix != oldautofix)
                        {
                            if (Properties.Settings.Default.autofix == true)
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Automatic fixing enabled" + Environment.NewLine);
                            }
                            else if (Properties.Settings.Default.autofix == false)
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Automatic fixing disabled" + Environment.NewLine);
                            }
                        }
                        if (Properties.Settings.Default.autosave != oldautosave)
                        {
                            if (Properties.Settings.Default.autosave == true)
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Automatic saving enabled" + Environment.NewLine);
                            }
                            else if (Properties.Settings.Default.autosave == false)
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Automatic saving disabled" + Environment.NewLine);
                            }
                        }
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (About aboutForm = new About())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (unsaved == true)
            {
                switch (MessageBox.Show("Do you want to save changes to " + filenamesafe + "?",
                         "fixprt",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SavePortalFile(false);
                        ClosePortalFile();
                        break;
                    case DialogResult.No:
                        ClosePortalFile();
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        // Methods

        public void OpenPortalFile(bool reload)
        {
            if (unsaved == true)
            {
                switch (MessageBox.Show("Do you want to save changes to " + filenamesafe + "?",
                         "fixprt",
                         MessageBoxButtons.YesNoCancel,
                         MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SavePortalFile(false);
                        if (reload == false)
                        {
                            {
                                using (openFileDialog1)
                                {
                                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                                    {
                                        if (!(String.IsNullOrEmpty(prt)))
                                        {
                                            ClosePortalFile();
                                        }
                                        filename = openFileDialog1.FileName;
                                        filenamesafe = openFileDialog1.SafeFileName;
                                        if (File.Exists(filename))
                                        {
                                            ReadPortalFile("Loaded");
                                        }
                                        else if (!(File.Exists(filename)))
                                        {
                                            console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                                            MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                        else if (reload == true)
                        {
                            if (File.Exists(filename))
                            {
                                ReadPortalFile("Reloaded");
                            }
                            else if (!(File.Exists(filename)))
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                                MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    case DialogResult.No:
                        if (reload == false)
                        {
                            {
                                using (openFileDialog1)
                                {
                                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                                    {
                                        filename = openFileDialog1.FileName;
                                        filenamesafe = openFileDialog1.SafeFileName;
                                        if (File.Exists(filename))
                                        {
                                            ReadPortalFile("Loaded");
                                        }
                                        else if (!(File.Exists(filename)))
                                        {
                                            console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                                            MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                            }
                        }
                        else if (reload == true)
                        {
                            if (File.Exists(filename))
                            {
                                ReadPortalFile("Reloaded");
                            }
                            else if (!(File.Exists(filename)))
                            {
                                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                                MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        break;
                    case DialogResult.Cancel:
                        break;
                }
            }
            else if (unsaved == false)
            {
                if (reload == false)
                {
                    {
                        using (openFileDialog1)
                        {
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                if (!(String.IsNullOrEmpty(prt)))
                                {
                                    ClosePortalFile();
                                }
                                filename = openFileDialog1.FileName;
                                filenamesafe = openFileDialog1.SafeFileName;
                                if (File.Exists(filename))
                                {
                                    ReadPortalFile("Loaded");
                                }
                                else if (!(File.Exists(filename)))
                                {
                                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                                    MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
                else if (reload == true)
                {
                    if (File.Exists(filename))
                    {
                        ReadPortalFile("Reloaded");
                    }
                    else if (!(File.Exists(filename)))
                    {
                        console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " does not exist" + Environment.NewLine);
                        MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void ReadPortalFile(string action)
        {
            fixd = false;
            using (StreamReader sr = new StreamReader(openFileDialog1.FileName))
            {
                var sb = new StringBuilder();
                while (!sr.EndOfStream)
                {
                    sb.AppendLine(sr.ReadLine());
                }
                prt = sb.ToString();

                if (String.IsNullOrWhiteSpace(prt))
                {
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " is not a valid portal file (null or whitespace)" + Environment.NewLine);
                    MessageBox.Show("File is empty.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (prt.Contains("PRT1")) // Source engine portal files contain the PRT1 signature
                {
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " is not a valid portal file (PRT signature detected)" + Environment.NewLine);
                    MessageBox.Show("Only Half-Life portal files are supported.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (open.IsMatch(prt)) // Only digits, whitespace, dashes, parentheses and full stops are allowed

                {
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " is not a valid portal file (invalid characters)" + Environment.NewLine);
                    MessageBox.Show("File contains invalid characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (!(open.IsMatch(prt)))
                {
                    fixToolStripMenuItem.Enabled = true;
                    reloadToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = true;
                    prtcontents.Text = prt;
                    Text = filename + " - " + "fixprt";
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] " + action + " " + filenamesafe + Environment.NewLine);
                }
            }
            if (Properties.Settings.Default.autofix == true)
            {
                FixPortalFile();
            }
        }
        public void FixPortalFile()
        {
            if (fixd == false)
            {
                if (fix.IsMatch(prt))
                {
                    unsaved = true;
                    prtfix = fix.Replace(prt, "");
                    prtcontents.Text = prtfix;
                    reloadToolStripMenuItem.Enabled = true;
                    saveToolStripMenuItem.Enabled = true;
                    saveAsToolStripMenuItem.Enabled = true;
                    closeToolStripMenuItem.Enabled = true;
                    Text = "*" + filename + " - " + "fixprt";
                    int prtcount = 0;
                    int prtfixcount = 0;
                    int prtposition = -1;
                    int prtfixposition = -1;
                    while ((prtposition = prt.IndexOf(Environment.NewLine, prtposition + 1)) != -1)
                    {
                        prtcount++;
                    }
                    while ((prtfixposition = prtfix.IndexOf(Environment.NewLine, prtfixposition + 1)) != -1)
                    {
                        prtfixcount++;
                    }
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Fixed " + filenamesafe + " (reduced " + prtcount + " lines to " + prtfixcount + " lines)" + Environment.NewLine);
                    if (Properties.Settings.Default.autosave == true)
                    {
                        SavePortalFile(false);
                    }
                    fixd = true;
                }
                else
                {
                    console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " is already fixed" + Environment.NewLine);
                    MessageBox.Show("File is already fixed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (fixd == true)
            {
                console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] ERROR: " + filenamesafe + " is already fixed" + Environment.NewLine);
                MessageBox.Show("File is already fixed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SavePortalFile(bool saveas)
        {
            if (saveas == true)
            {
                using (saveFileDialog1)
                {
                    saveFileDialog1.FileName = filenamesafe;
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        saveToolStripMenuItem.Enabled = false;
                        filenamesafe = Path.GetFileName(saveFileDialog1.FileName);
                        filename = saveFileDialog1.FileName;
                        WritePortalFile();
                    }
                }
            }
            else if (saveas == false)
            {
                saveToolStripMenuItem.Enabled = false;
                WritePortalFile();
            }
        }

        public void WritePortalFile()
        {
            if (!(String.IsNullOrWhiteSpace(prtfix)))
            {
                File.WriteAllText(filename, prtfix);
            }
            else if (!(String.IsNullOrWhiteSpace(prt)))
            {
                File.WriteAllText(filename, prt);
            }
            else
            {
                File.WriteAllText(filename, prtcontents.Text);
            }
            Text = filename + " - " + "fixprt";
            console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Saved " + filenamesafe + Environment.NewLine);
            unsaved = false;
        }

        public void ClosePortalFile()
        {
            fixToolStripMenuItem.Enabled = false;
            reloadToolStripMenuItem.Enabled = false;
            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;
            closeToolStripMenuItem.Enabled = false;
            prtcontents.Text = String.Empty;
            Text = "fixprt";
            console.AppendText("[" + DateTime.Now.ToString("HH:mm:ss") + "] Closed " + filenamesafe + Environment.NewLine);
            prt = String.Empty;
            prtfix = String.Empty;
            filename = String.Empty;
            filenamesafe = String.Empty;
            unsaved = false;
        }
    }
}
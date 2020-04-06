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
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace fixprt
{
    public partial class About : Form
    {
        public About()
        {
            InitializeComponent();
            fixprtLogo.Image = Properties.Resources.fixprt128;
            nameLabel.Text = ". : fixprt : .";
            versionLabel.Text = "1.0.0"; // Major.Minor.Revision
            dateLabel.Text = "06/4/2020"; // DD/MM/YYYY
            authorLabel.Text = // Add your name here if you modify fixprt
                "seedee" + Environment.NewLine + "(cdaniel9000@gmail.com)"/* + Environment.NewLine +
                "name (email/site)" + Environment.NewLine +
                "name (email/site)" + Environment.NewLine +
                "name (email/site)" */;
            licenceLogo.Image = Properties.Resources.GPL_16x;
            manualLogo.Image = Properties.Resources.TextFile_16x;
            gitHubLogo.Image = Properties.Resources.GitHub_16x;
            gameBananaLogo.Image = Properties.Resources.GameBanana_16x;
        }

        private void manualLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists("manual.html"))
            {
                System.Diagnostics.Process.Start("manual.html");
            }
            else if (!(File.Exists("manual.html")))
            {
                switch (MessageBox.Show("Could not find the manual.html file." + Environment.NewLine + "Do you want to go to the GitHub repository instead?", "View Manual", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        Process.Start("http://github.com/seedee/fixprt/");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void licenceLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (File.Exists("gpl-3.0.txt"))
            {
                System.Diagnostics.Process.Start("gpl-3.0.txt");
            }
            else if (!(File.Exists("gpl-3.0.txt")))
            {
                switch (MessageBox.Show("Could not find the gpl-3.0.txt file." + Environment.NewLine + "Do you want to go to the GNU website instead?", "View Licence", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        Process.Start("https://www.gnu.org/licenses/gpl-3.0.txt");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void gameBananaLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://gamebanana.com/");
        }

        private void gitHubLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/seedee/fixprt/");
        }

        private void manualLogo_Click(object sender, EventArgs e)
        {
            if (File.Exists("readme.txt"))
            {
                Process.Start("notepad.exe", "readme.txt");
            }
            else if (!(File.Exists("readme.txt")))
            {
                switch (MessageBox.Show("Could not find the readme.txt file." + Environment.NewLine + "Do you want to go to the GitHub repository instead?", "View Manual", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        Process.Start("http://github.com/seedee/fixprt/");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void licenceLogo_Click(object sender, EventArgs e)
        {
            if (File.Exists("gpl-3.0.txt"))
            {
                Process.Start("notepad.exe", "gpl-3.0.txt");
            }
            else if (!(File.Exists("gpl-3.0.txt")))
            {
                switch (MessageBox.Show("Could not find the gpl-3.0.txt file." + Environment.NewLine + "Do you want to go to the GNU website instead?", "View Licence", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    case DialogResult.Yes:
                        Process.Start("https://www.gnu.org/licenses/gpl-3.0.txt");
                        break;
                    case DialogResult.No:
                        break;
                }
            }
        }

        private void gameBananaLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://gamebanana.com/");
        }

        private void gitHubLogo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://github.com/seedee/fixprt/");
        }
    }
}

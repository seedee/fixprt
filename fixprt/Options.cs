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
using System.Windows.Forms;

namespace fixprt
{
    public partial class Options : Form
    {
        public Options()
        {
            InitializeComponent();
            getSettings();
        }

        private void getSettings()
        {
            if (Properties.Settings.Default.autofix == true)
            {
                autoFixCheckBox.CheckState = CheckState.Checked;
            }
            else if (Properties.Settings.Default.autofix == false)
            {
                autoFixCheckBox.CheckState = CheckState.Unchecked;
            }
            if (Properties.Settings.Default.autosave == true)
            {
                autoSaveCheckBox.CheckState = CheckState.Checked;
            }
            else if (Properties.Settings.Default.autosave == false)
            {
                autoSaveCheckBox.CheckState = CheckState.Unchecked;
            }
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (autoFixCheckBox.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.autofix = true;
            }
            else if (autoFixCheckBox.CheckState == CheckState.Unchecked)
            {
                Properties.Settings.Default.autofix = false;
            }
            if (autoSaveCheckBox.CheckState == CheckState.Checked)
            {
                Properties.Settings.Default.autosave = true;
            }
            else if (autoSaveCheckBox.CheckState == CheckState.Unchecked)
            {
                Properties.Settings.Default.autosave = false;
            }
            Properties.Settings.Default.Save();
        }
    }
}
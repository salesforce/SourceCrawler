/*
 * Copyright (c) 2020, salesforce.com, inc.
 * All rights reserved.
 * SPDX-License-Identifier: BSD-3-Clause
 * For full license text, see the LICENSE file in the repo root or https://opensource.org/licenses/BSD-3-Clause
*/

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace SourceCrawler
{
    public partial class ManageRoots : Form
    {
        BindingList<RootObject> _allRoots;
        private RepositoryFile _currentRepo; 

        public ManageRoots(RepositoryFile currentRepo)
        {
            InitializeComponent();
            
            _allRoots = new BindingList<RootObject>(RepositoryUtils.GetAllRoots());
            lbxRoots.DataSource = _allRoots;
            lbxRoots.DisplayMember = "RootPath";

            if (_allRoots.Count > 0)
            {
                lbxRoots.SelectedItem = _allRoots.FirstOrDefault(r => r.IsDefault);
            }
            else
            {
                btnRemove.Enabled = false;
            }
            _currentRepo = currentRepo;
            lbxRoots.Focus();
        }
        
        private void addNewRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            if (folder.ShowDialog() == DialogResult.OK)
            {
                var newRoot = new RootObject
                {
                    RootId = Guid.NewGuid().ToString(),
                    RootPath = folder.SelectedPath,
                    IsDefault = _allRoots.Count == 0 ? true : false,
                    LastUpdate = DateTime.Now
                };

                RepositoryUtils.Upsert(newRoot);
                _allRoots.Add(newRoot);
                lbxRoots.SelectedItem = newRoot;
            }

            AdjustAddRemoveButtons();
        }

        private void deleteRootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbxRoots.Items.Count == 0 || lbxRoots.SelectedItem == null)
            {
                MessageBox.Show("Nothing to delete.");
                return;
            }

            var IdToDelete = (lbxRoots.SelectedItem as RootObject).RootId;
            var objToDelete = _allRoots.First(r => r.RootId == IdToDelete);

            if (_currentRepo != null && IdToDelete == _currentRepo.Root.RootId)
            {
                _currentRepo.ClearCache();
            }

            RepositoryUtils.DeleteRoot(IdToDelete);
            _allRoots.Remove(objToDelete);

            AdjustAddRemoveButtons();
        }

        private void AdjustAddRemoveButtons()
        {
            if (lbxRoots.SelectedItem != null)
            {
                btnRemove.Enabled = true;
            }
            else
            {
                btnRemove.Enabled = false;
            }
        }

        private void lbxRoots_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                deleteRootToolStripMenuItem.Enabled = _allRoots.Count > 0;
                
                lbxRoots.SelectedItems.Clear();
                lbxRoots.SelectedIndex = lbxRoots.IndexFromPoint(e.X, e.Y);
                if (lbxRoots.SelectedItems.Count > 0)
                {
                    deleteRootToolStripMenuItem.Enabled = true;
                }
                else
                {
                    deleteRootToolStripMenuItem.Enabled = false;
                }

                mnuContext.Show(MousePosition);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //Check if a default exists
            if (lbxRoots.SelectedItem != null)
            {
                foreach (var item in _allRoots)
                {
                    item.IsDefault = lbxRoots.SelectedItem.Equals(item);

                    RepositoryUtils.Upsert(item);
                }
            }

            DialogResult = DialogResult.OK;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addNewRootToolStripMenuItem_Click(null, null);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            deleteRootToolStripMenuItem_Click(null, null);
        }

        private void lbxRoots_DoubleClick(object sender, EventArgs e)
        {
            btnOk_Click(null, null);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DLMS.BO;

namespace DLMS
{
    public partial class fDesignations : Form
    {

        DLMSEntities db = null;
        string SearchItem = string.Empty;
        List<Designaiton> _designationList = new List<Designaiton>();
        public fDesignations()
        {
            InitializeComponent();
        }
       
        private void RefreshList()
        {

            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    var odesignation = db.Designaitons;
                    ListViewItem item = null;
                    lsvDesignation.Items.Clear();
                    
                    _designationList = odesignation.OrderBy(m => m.Name).ToList();

                    List<ListViewItem> lItems = new List<ListViewItem>();
                    if (_designationList != null)
                    {
                        foreach (Designaiton grd in _designationList)
                        {
                            item = new ListViewItem();
                            item.Text = grd.Code;
                            item.SubItems.Add(grd.Name);
                            item.Tag = grd;
                           lsvDesignation.Items.Add(item);
                        }

                        lsvDesignation.Items.AddRange(lItems.ToArray());
                        lblTotal.Text = "Total :" + _designationList.Count().ToString();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fDesignations_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            fDesignation frm = new fDesignation();
            frm.ItemChanged = RefreshList;
           
            frm.ShowDlg(new Designaiton(), true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvDesignation.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Designaiton oDesignation = null;
            fDesignation frm = new fDesignation();

            if (lsvDesignation.SelectedItems != null && lsvDesignation.SelectedItems.Count > 0)
            {
                oDesignation = (Designaiton)lsvDesignation.SelectedItems[0].Tag;
            }
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(oDesignation, false);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
             
                Designaiton oDesignation= new Designaiton();

                if (lsvDesignation.SelectedItems != null && lsvDesignation.SelectedItems.Count > 0)
                {
                    oDesignation = (Designaiton)lsvDesignation.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.Designaitons.Attach(oDesignation);
                            db.Designaitons.Remove(oDesignation);
                            //Save to database
                            db.SaveChanges();
                        }
                        RefreshList();
                    }
                }
                else
                    MessageBox.Show("select an item to Delete", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Cannot delete item due to " + Ex.Message);
            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

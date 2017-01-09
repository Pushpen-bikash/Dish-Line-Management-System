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
    public partial class fOutlets : Form
    {
        public fOutlets()
        {
            InitializeComponent();
        }

        private void RefreshList()
        {
            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    List<Outlet> _Outlet = db.Outlets.OrderBy(d => d.OutletCode).ToList();

                    ListViewItem item = null;
                    lsvOutlet.Items.Clear();

                    if (_Outlet != null)
                    {
                        foreach (Outlet oDItem in _Outlet)
                        {
                            item = new ListViewItem();
                            item.Text = oDItem.OutletCode;
                            item.SubItems.Add(oDItem.OutletName);
                            item.Tag = oDItem;
                            lsvOutlet.Items.Add(item);
                        }

                        lblTotal.Text = "Total :" + _Outlet.Count().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            fOutlet frm = new fOutlet();
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(new Outlet(),true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvOutlet.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Outlet oOutlet = null;
            fOutlet frm = new fOutlet();

            if (lsvOutlet.SelectedItems != null && lsvOutlet.SelectedItems.Count > 0)
            {
                oOutlet = (Outlet)lsvOutlet.SelectedItems[0].Tag;
            }
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(oOutlet, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Outlet oOutlet = new Outlet();
                if (lsvOutlet.SelectedItems != null && lsvOutlet.SelectedItems.Count > 0)
                {
                    oOutlet = (Outlet)lsvOutlet.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.Outlets.Attach(oOutlet);
                            db.Outlets.Remove(oOutlet);
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

        private void fOutlets_Load(object sender, EventArgs e)
        {
            RefreshList();
        }
    }
}

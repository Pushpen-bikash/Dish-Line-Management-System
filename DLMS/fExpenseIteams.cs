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
    public partial class fExpenseIteams : Form
    {
        DLMSEntities db = null;
        string SearchItem = string.Empty;
        List<ExpenseItem> _ExpenseItemsList = new List<ExpenseItem>();

        public fExpenseIteams()
        {
            db = new DLMSEntities();
            InitializeComponent();
        }

        

        private void fExpenseIteams_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    var _ExpenseItems = db.ExpenseItems;
                    ListViewItem item = null;
                    lsvExpenseIteams.Items.Clear();
                    _ExpenseItemsList = _ExpenseItems.OrderBy(m => m.Code).ToList();

                    List<ListViewItem> lItems = new List<ListViewItem>();
                    if (_ExpenseItemsList != null)
                    {
                        foreach (ExpenseItem grd in _ExpenseItemsList)
                        {
                            item = new ListViewItem();
                            item.Text = grd.Code;
                            item.SubItems.Add(grd.Description);
                            item.Tag = grd;
                            lsvExpenseIteams.Items.Add(item);
                        }

                        lsvExpenseIteams.Items.AddRange(lItems.ToArray());
                        lblTotal.Text = "Total :" + _ExpenseItemsList.Count().ToString();
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
            fExpenseItem frm = new fExpenseItem();
            frm.ItemChanged1 = RefreshList;
            frm.ShowDlg(new ExpenseItem(), true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvExpenseIteams.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ExpenseItem oExpenseItem = null;
            fExpenseItem frm = new fExpenseItem();

            if (lsvExpenseIteams.SelectedItems != null && lsvExpenseIteams.SelectedItems.Count > 0)
            {
                oExpenseItem = (ExpenseItem)lsvExpenseIteams.SelectedItems[0].Tag;
            }
            frm.ItemChanged1 = RefreshList;
            frm.ShowDlg(oExpenseItem, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                ExpenseItem oExpenseItem = new ExpenseItem();
                if (lsvExpenseIteams.SelectedItems != null && lsvExpenseIteams.SelectedItems.Count > 0)
                {
                    oExpenseItem = (ExpenseItem)lsvExpenseIteams.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.ExpenseItems.Attach(oExpenseItem);
                            db.ExpenseItems.Remove(oExpenseItem);
                            //Save to database
                            db.SaveChanges();
                        }
                        RefreshList();
                    }
                }

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

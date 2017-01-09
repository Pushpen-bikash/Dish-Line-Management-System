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
    public partial class fExpenditures : Form
    {
        DLMSEntities db = null;
        string SearchItem = string.Empty;
        List<Expenditure> _ExpenditureList = new List<Expenditure>();
        public fExpenditures()
        {
            db = new DLMSEntities();
            InitializeComponent();
        }

        private void fExpenditures_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    var _Expenditures = db.Expenditures;
                    ListViewItem item = null;
                    lsvExpenditure.Items.Clear();
                    //_ExpenditureList = _Expenditures.OrderBy(m => m.EntryDate).ToList();
                    _ExpenditureList = _Expenditures.ToList();

                    List<ListViewItem> lItems = new List<ListViewItem>();
                    if (_ExpenditureList != null)
                    {
                        foreach (Expenditure grd in _ExpenditureList)
                        {
                            item = new ListViewItem();
                            item.Text = grd.EntryDate.ToString();
                            //item.SubItems.Add(grd.ExpenseItem.ToString());
                            //string expense = new fExpenditure().expense();
                            //item.SubItems.Add(expense);
                            int i = grd.ExpenseItemID;
                            //IEnumerable<string> ex = new DLMSEntities().ExpenseItems.Where(m => m.ExpenseItemID == i).Select(n=>n.Description);
                            //item.SubItems.Add(i.ToString());
                            var ex = new DLMSEntities().ExpenseItems.Where(m => m.ExpenseItemID == i).Select(n=>n.Description).FirstOrDefault();
                            item.SubItems.Add(ex.ToString());
                            item.SubItems.Add(grd.Purpose);
                            item.SubItems.Add(grd.Amount.ToString());
                            item.Tag = grd;
                            lsvExpenditure.Items.Add(item);
                        }

                        lsvExpenditure.Items.AddRange(lItems.ToArray());
                        lblTotal.Text = "Total :" + _ExpenditureList.Count().ToString();
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
            fExpenditure frm = new fExpenditure();
            frm.ItemChanged2 = RefreshList;
            frm.ShowDlg(new Expenditure(), true);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvExpenditure.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Expenditure oExpenditure = null;
            fExpenditure frm = new fExpenditure();

            if (lsvExpenditure.SelectedItems != null && lsvExpenditure.SelectedItems.Count > 0)
            {
                oExpenditure = (Expenditure)lsvExpenditure.SelectedItems[0].Tag;
            }
            frm.ItemChanged2 = RefreshList;
            frm.ShowDlg(oExpenditure, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Expenditure oExpenditure = new Expenditure();
                if (lsvExpenditure.SelectedItems != null && lsvExpenditure.SelectedItems.Count > 0)
                {
                    oExpenditure = (Expenditure)lsvExpenditure.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.Expenditures.Attach(oExpenditure);
                            db.Expenditures.Remove(oExpenditure);
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

        }

        
    }
}

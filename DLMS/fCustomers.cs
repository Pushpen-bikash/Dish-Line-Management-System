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
    public partial class fCustomers : Form
    {
        List<Customer> _Customers = null;
        public fCustomers()
        {
            InitializeComponent();
        }

        private void fCustomers_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    if (txtSearch.Text.Length == 0)
                    {
                        _Customers = db.Customers.OrderBy(d => d.Code).ToList();
                    }
                    else
                       _Customers = db.Customers.OrderBy(d => d.Code).Where(n=>n.Name.StartsWith(txtSearch.Text)).ToList();

                        ListViewItem item = null;
                        lsvCustomer.Items.Clear();
                        string status = "";

                        if (_Customers != null)
                        {
                            foreach (Customer oDItem in _Customers)
                            {
                                item = new ListViewItem();
                                item.Text = oDItem.Code;
                                item.SubItems.Add(oDItem.Name);
                                item.SubItems.Add(oDItem.ContactNo);
                                EnumCustomerStatus stat = (EnumCustomerStatus)oDItem.CusStatus;
                                status = stat.ToString();
                                if (oDItem.CusStatus == 2)
                                {
                                    item.BackColor = Color.Red;
                                }
                                item.SubItems.Add(status);
                                item.Tag = oDItem;
                                lsvCustomer.Items.Add(item);
                            }

                            lblTotal.Text = "Total :" + _Customers.Count().ToString();
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
            fCustomer frm = new fCustomer();
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(new Customer(), true);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvCustomer.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Customer oCustomer = null;
            fCustomer frm = new fCustomer();

            if (lsvCustomer.SelectedItems != null && lsvCustomer.SelectedItems.Count > 0)
            {
                oCustomer = (Customer)lsvCustomer.SelectedItems[0].Tag;
            }
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(oCustomer, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Customer oCustomer = new Customer();
                if (lsvCustomer.SelectedItems != null && lsvCustomer.SelectedItems.Count > 0)
                {
                    oCustomer = (Customer)lsvCustomer.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.Customers.Attach(oCustomer);
                            db.Customers.Remove(oCustomer);
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
                    RefreshList();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

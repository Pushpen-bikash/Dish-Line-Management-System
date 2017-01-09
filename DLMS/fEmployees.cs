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
    public partial class fEmployees : Form
    {
        public fEmployees()
        {
            InitializeComponent();
        }

        private void RefreshList()
        {
            try
            {
                using (DLMSEntities db = new DLMSEntities())
                {
                    //var _Employees = db.Employees;

                    List<Employee> _Employees = db.Employees.OrderBy(e => e.EmpCode).ToList();

                    ListViewItem item = null;
                    lsvEmployee.Items.Clear();

                    if (_Employees != null)
                    {
                        foreach (Employee oEmpItem in _Employees)
                        {
                            item = new ListViewItem();
                            item.Text = oEmpItem.EmpCode;
                            item.SubItems.Add(oEmpItem.EmpName);
                            item.SubItems.Add(oEmpItem.Designaiton.Name);
                            //item.SubItems.Add(oEmpItem.Designation.Description);
                            item.SubItems.Add(oEmpItem.ContactNo);
                            item.Tag = oEmpItem;
                            lsvEmployee.Items.Add(item);
                        }

                        lblTotal.Text = "Total :" + _Employees.Count().ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void fEmployees_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            fEmployee frm = new fEmployee();
            frm.ItemChanged = RefreshList;

            frm.ShowDlg(new Employee(), true);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtSearch.Text != "")
                {
                    foreach (ListViewItem item in lsvEmployee.Items)
                    {
                        if (item.SubItems[1].Text.ToLower().Contains(txtSearch.Text.ToLower()))
                        {
                            item.Selected = true;
                        }
                        else
                        {
                            lsvEmployee.Items.Remove(item);
                        }
                        item.Selected = false;
                    }
                    if (lsvEmployee.SelectedItems.Count == 1)
                    {
                        lsvEmployee.Focus();
                    }
                }
                else
                {
                    RefreshList();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }



        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvEmployee.SelectedItems.Count <= 0)
            {
                MessageBox.Show("select an item to edit", "Item not yet selected", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Employee oEmployee = null;
            fEmployee frm = new fEmployee();

            if (lsvEmployee.SelectedItems != null && lsvEmployee.SelectedItems.Count > 0)
            {
                oEmployee = (Employee)lsvEmployee.SelectedItems[0].Tag;
            }
            frm.ItemChanged = RefreshList;
            frm.ShowDlg(oEmployee, false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Employee oEmployee = new Employee();
                if (lsvEmployee.SelectedItems != null && lsvEmployee.SelectedItems.Count > 0)
                {
                    oEmployee = (Employee)lsvEmployee.SelectedItems[0].Tag;
                    if (MessageBox.Show("Do you want to delete the selected item?", "Delete Setup", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        using (DLMSEntities db = new DLMSEntities())
                        {
                            db.Employees.Attach(oEmployee);
                            db.Employees.Remove(oEmployee);
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
    }
}

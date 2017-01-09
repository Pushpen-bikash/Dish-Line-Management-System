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
    public partial class fExpenseItem : Form
    {

        private ExpenseItem _ExpenseItem = null;
        public Action ItemChanged1;
        bool _IsNew = false;

        public fExpenseItem()
        {
            InitializeComponent();
        }

        private void fExpenseItem_Load(object sender, EventArgs e)
        {
            if (_IsNew)
                txtCode.Text = GenerateCategoryCode();
        }

        public void ShowDlg(ExpenseItem oExpenseItem, bool IsNew)
        {
            _IsNew = IsNew;
            _ExpenseItem = oExpenseItem;
            RefreshValue();
            this.ShowDialog();
        }

        private void RefreshValue()
        {
            txtCode.Text = _ExpenseItem.Code;
            txtName.Text = _ExpenseItem.Description;

        }

        private string GenerateCategoryCode()
        {
            int i = 0;
            string sCode = "";

            using (DLMSEntities db = new DLMSEntities())
            {
                i = db.ExpenseItems.Count();

                if (i.ToString().Length == 1)
                {
                    sCode = "0000" + Convert.ToString(db.ExpenseItems.Count() + 1);
                }
                else if (i.ToString().Length == 2)
                {
                    sCode = "000" + Convert.ToString(db.ExpenseItems.Count() + 1);
                }
                else if (i.ToString().Length == 3)
                {
                    sCode = "00" + Convert.ToString(db.ExpenseItems.Count() + 1);
                }
                else if (i.ToString().Length == 4)
                {
                    sCode = "0" + Convert.ToString(db.ExpenseItems.Count() + 1);
                }
                else
                {
                    sCode = "0" + Convert.ToString(db.ExpenseItems.Count() + 1);
                }
            }
            return sCode;
        }

        private bool IsValid()
        {
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show("Please Enter Student Name.", "Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtName.Focus();
                return false;
            }
            return true;
        }

        private void RefreshObject()
        {
            _ExpenseItem.Code = txtCode.Text;
            _ExpenseItem.Description = txtName.Text;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValid()) return;
                bool isNew = false;
                using (DLMSEntities db = new DLMSEntities())
                {
                    if (_ExpenseItem.ExpenseItemID <= 0)
                    {
                        RefreshObject();
                        _ExpenseItem.ExpenseItemID = db.ExpenseItems.Count() > 0 ? db.ExpenseItems.Max(obj => obj.ExpenseItemID) + 1 : 1;
                        db.ExpenseItems.Add(_ExpenseItem);
                        isNew = true;
                    }
                    else
                    {
                        _ExpenseItem = db.ExpenseItems.FirstOrDefault(obj => obj.ExpenseItemID == _ExpenseItem.ExpenseItemID);
                        RefreshObject();
                    }

                    db.SaveChanges();

                    MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!isNew)
                    {
                        if (ItemChanged1 != null)
                        {
                            ItemChanged1();
                        }
                        this.Close();
                    }
                    else
                    {
                        if (ItemChanged1 != null)
                        {
                            ItemChanged1();
                        }
                        _ExpenseItem = new ExpenseItem();
                        RefreshValue();
                        txtCode.Text = GenerateCategoryCode();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    MessageBox.Show(ex.Message, "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    MessageBox.Show(ex.InnerException.Message, "Failed to save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

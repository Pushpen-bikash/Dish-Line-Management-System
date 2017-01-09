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
    public partial class fExpenditure : Form
    {
        private Expenditure _Expenditure = null;
        public Action ItemChanged2;
        bool _IsNew = false;

        public fExpenditure()
        {
            InitializeComponent();
        }

        private void fExpenditure_Load(object sender, EventArgs e)
        {
            PopulateTranTypeCbo();
            if(!_IsNew)
            {
                cboExpenseItem.SelectedValue = _Expenditure.ExpenseItemID;
            }
        }

        private void PopulateTranTypeCbo()
        {
            using (DLMSEntities db = new DLMSEntities())
            {
                cboExpenseItem.DisplayMember = "Description";
                cboExpenseItem.ValueMember = "ExpenseItemID";
                cboExpenseItem.DataSource = db.ExpenseItems.OrderBy(x => x.Description).ToList();
            }
        }

        public void ShowDlg(Expenditure oExpenditure, bool IsNew)
        {
            _IsNew = IsNew;
            _Expenditure = oExpenditure;
            PopulateTranTypeCbo();
            if (!_IsNew)
            {
                cboExpenseItem.SelectedValue = _Expenditure.ExpenseItemID;
            }
            RefreshValue();
            this.ShowDialog();
        }

        private void RefreshValue()
        {
            //dtpJoiningDate.Value = _Employee.JoiningDate != DateTime.MinValue ? (DateTime)_Employee.JoiningDate : DateTime.Now;
            dtpEntryDate.Value = _Expenditure.EntryDate != DateTime.MinValue ? (DateTime)_Expenditure.EntryDate : DateTime.Now;
            numAmount.Value = _Expenditure.Amount;
            txtPurpose.Text = _Expenditure.Purpose;

        }

        private bool IsValid()
        {
            if (numAmount.Value == 0)
            {
                MessageBox.Show("Please Enter the Amount.", "Amount", MessageBoxButtons.OK, MessageBoxIcon.Information);
                numAmount.Focus();
                return false;
            }
            return true;
        }

        private void RefreshObject()
        {
            _Expenditure.EntryDate = dtpEntryDate.Value;
            _Expenditure.Amount = numAmount.Value;
            _Expenditure.Purpose = txtPurpose.Text;
            _Expenditure.ExpenseItemID = (int)cboExpenseItem.SelectedValue;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValid()) return;
                bool isNew = false;
                using (DLMSEntities db = new DLMSEntities())
                {
                    if (_Expenditure.ExpenditureID <= 0)
                    {
                        RefreshObject();
                        _Expenditure.ExpenditureID = db.Expenditures.Count() > 0 ? db.Expenditures.Max(obj => obj.ExpenditureID) + 1 : 1;
                        db.Expenditures.Add(_Expenditure);
                        isNew = true;
                    }
                    else
                    {
                        _Expenditure = db.Expenditures.FirstOrDefault(obj => obj.ExpenditureID == _Expenditure.ExpenditureID);
                        RefreshObject();
                    }

                    db.SaveChanges();

                    MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!isNew)
                    {
                        if (ItemChanged2 != null)
                        {
                            ItemChanged2();
                        }
                        this.Close();
                    }
                    else
                    {
                        if (ItemChanged2 != null)
                        {
                            ItemChanged2();
                        }
                        _Expenditure = new Expenditure();
                        RefreshValue();
                        //txtCode.Text = GenerateCategoryCode();
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

        public string str;
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cboExpenseItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            //str = cboExpenseItem.Text;
        }

       //public string expense()
        //{
          //  return str;
        //}

    }
}

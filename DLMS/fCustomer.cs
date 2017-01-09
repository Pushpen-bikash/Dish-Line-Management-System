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
using System.IO;
namespace DLMS
{
    public partial class fCustomer : Form
    {
        private DLMS.BO.SystemInformation _SystemInformation = null;
        private Customer _Customer = null;
        public Action ItemChanged;
        bool _IsNew = false;
        public fCustomer()
        {
            InitializeComponent();
        }

        private void fCustomer_Load(object sender, EventArgs e)
        {
            if (_IsNew)
               txtCusCode.Text=GenerateCusCode();
        }

        public void ShowDlg(Customer oCustomer, bool IsNew)
        {
            _Customer = oCustomer;
            _IsNew = IsNew;
            PopulateOutletName();
            PopulateCustomerStatus();
            DLMSEntities db = new DLMSEntities();
            _SystemInformation = (from c in db.SystemInformations
                                  where c.SystemInfoID == 1
                                  select c).FirstOrDefault();
            RefreshValue();
            this.ShowDialog();

        }

        private void PopulateCustomerStatus()
        {
            cboCusStatus.DisplayMember = "Department";
            cboCusStatus.ValueMember = "ID";
            cboCusStatus.DataSource = Enum.GetValues(typeof(EnumCustomerStatus)).Cast<EnumCustomerStatus>().Select(x => new { ID = (int)x, Department = x.ToString() }).ToList();

        }

        private void PopulateOutletName()
        {
            using (DLMSEntities db = new DLMSEntities())
            {
                cboOutletName.ValueMember = "OutletID";
                cboOutletName.DisplayMember = "OutletName";
                cboOutletName.DataSource = db.Outlets.OrderBy(x => x.OutletName).ToList();
            }
        }
        private string GenerateCusCode()
        {
            int i = 0;
            string sCode = "";

            using (DLMSEntities db = new DLMSEntities())
            {
                i = db.Customers.Max(id=>id.CustomerID);

                if (i.ToString().Length == 1)
                {
                    sCode = "0000" + Convert.ToString(i + 1);
                }
                else if (i.ToString().Length == 2)
                {
                    sCode = "000" + Convert.ToString(i + 1);
                }
                else if (i.ToString().Length == 3)
                {
                    sCode = "00" + Convert.ToString(i+ 1);
                }
                else if (i.ToString().Length == 4)
                {
                    sCode = "0" + Convert.ToString(i+ 1);
                }
                else
                {
                    sCode = "0" + Convert.ToString(i + 1);
                }
            }
            return sCode;
        }
        private bool IsValid()
        {
            if (txtCusCode.Text.Length == 0||txtCusName.Text.Length==0||txtContactNo.Text.Length==0||numEntryFee.Value<=0||numMonCharge.Value<=0)
            {
                MessageBox.Show("Please Enter Student Name.", "Student", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtCusName.Focus();
                return false;
            }
            return true;
        }
  
        private void RefreshObject()
        {
            _Customer.Code = txtCusCode.Text;
            _Customer.Name = txtCusName.Text;
            _Customer.FName = txtFName.Text;
            _Customer.MName = txtMName.Text;
            _Customer.ContactNo = txtContactNo.Text;
            _Customer.EntryDate = dtpEntryDate.Value;
            _Customer.EntryFee = numEntryFee.Value;
            _Customer.MonthlyCharge = numMonCharge.Value;
            _Customer.OutletID = ((Outlet)cboOutletName.SelectedItem).OutletID;
            _Customer.CusStatus = (int)cboCusStatus.SelectedValue;
         
            #region Employee Picture

            if ((pbxCusPic.ImageLocation != string.Empty) && (pbxCusPic.ImageLocation != null))
            {
                if (_Customer.Photopath == null)
                {
                    _Customer.Photopath = _SystemInformation.CusPhotoPath;
                }

                if ((_Customer.Photopath.Trim()).ToUpper() != new FileInfo(pbxCusPic.ImageLocation).Name.Trim().ToUpper())
                {
                    FileInfo fInfo = new FileInfo(pbxCusPic.ImageLocation.Trim());
                    string photoName = txtCusCode.Text + fInfo.Extension;
                    _Customer.Photopath = _SystemInformation.CusPhotoPath;

                    if (fInfo.Exists == false)
                    {
                        MessageBox.Show("Picture does not exist", "Customer Picture", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    if (_Customer.Photopath == "")
                    {
                        MessageBox.Show("Employee's Picture will not be saved. because it's path is not defined.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        if (!Directory.Exists(_Customer.Photopath))
                            Directory.CreateDirectory(_Customer.Photopath);

                        if (new FileInfo(_SystemInformation.CusPhotoPath + photoName).Exists)
                        {
                            if (MessageBox.Show("A picture named " + photoName + " already exists. Do you want to replace it?", "Employee Picture", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                            {
                                FileInfo info = new FileInfo(_SystemInformation.CusPhotoPath + photoName);
                                if (info.IsReadOnly)
                                    info.IsReadOnly = false;
                                File.Copy(pbxCusPic.ImageLocation.Trim(), _SystemInformation.CusPhotoPath + "\\" + photoName, true);
                            }
                            else
                            {
                                return;
                            }
                        }
                        else
                        {
                            File.Copy(pbxCusPic.ImageLocation.Trim(), _SystemInformation.CusPhotoPath + photoName, true);
                        }

                        new FileInfo(_SystemInformation.CusPhotoPath + photoName).IsReadOnly = true;
                       _Customer.Photopath = photoName;
                    }
                }
            }
            else
            {
                _Customer.Photopath = string.Empty;
            }
            #endregion            
        }
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                odPicture.FileName = "";
                odPicture.ShowDialog();
                if (odPicture.FileName != "")
                {
                    pbxCusPic.ImageLocation = odPicture.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!IsValid()) return;
                bool isNew = false;
                using (DLMSEntities db = new DLMSEntities())
                {
                    if (_Customer.CustomerID <= 0)
                    {
                        RefreshObject();
                        _Customer.CustomerID = db.Customers.Count() > 0 ? db.Customers.Max(obj => obj.CustomerID) + 1 : 1;
                        db.Customers.Add(_Customer);
                        isNew = true;
                    }
                    else
                    {
                        _Customer = db.Customers.FirstOrDefault(obj => obj.CustomerID == _Customer.CustomerID);
                        RefreshObject();
                    }

                    db.SaveChanges();

                    MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    if (!isNew)//edit
                    {
                        if (ItemChanged != null)
                        {
                            ItemChanged();
                        }
                        this.Close();
                    }
                    else
                    {
                        if (ItemChanged != null)
                        {
                            ItemChanged();
                        }
                        _Customer = new Customer();
                        RefreshValue();
                        txtCusCode.Text = GenerateCusCode();
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

        private void RefreshValue()
        {
            txtCusCode.Text = _Customer.Code;
            txtCusName.Text = _Customer.Name;
            txtFName.Text = _Customer.FName;
            txtMName.Text = _Customer.MName;
            txtContactNo.Text = _Customer.ContactNo;
            
            numEntryFee.Value = _Customer.EntryFee;
            numMonCharge.Value = _Customer.MonthlyCharge;

           dtpEntryDate.Value = _Customer.EntryDate != DateTime.MinValue ? (DateTime)_Customer.EntryDate : DateTime.Now;

            if ((Outlet)cboOutletName.SelectedItem != null)
                cboOutletName.SelectedValue = ((Outlet)cboOutletName.SelectedItem).OutletID;



            if (_Customer.Photopath != null)
            {
                string path = _SystemInformation.CusPhotoPath + "\\" + _Customer.Photopath;
                if (File.Exists(path))
                {
                    pbxCusPic.ImageLocation = _SystemInformation.CusPhotoPath + "//" + _Customer.Photopath;
                }
                else
                {
                    MessageBox.Show("Error Loading Employee Picture" + Environment.NewLine + "Picture Not Found");
                }
            }
            txtCusName.Focus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

      
    }
}

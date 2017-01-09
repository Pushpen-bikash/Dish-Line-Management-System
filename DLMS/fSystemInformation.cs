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
    public partial class fSystemInformation : Form
    {
        private DLMS.BO.SystemInformation _SystemInformation = new BO.SystemInformation();
        public Action ItemChanged;
        ToolTip _toolTip;

        public fSystemInformation()
        {
            InitializeComponent();
            _toolTip = new ToolTip();
        }

        private void fSystemInformation_Load(object sender, EventArgs e)
        {
            try
            {
                btnPhotoClearCus.Enabled = true;
                btnEmpPPClear.Enabled = true;
                btnSuppClear.Enabled = true;

                using (DLMSEntities db = new DLMSEntities())
                {


                    _SystemInformation = (from c in db.SystemInformations
                                          where c.SystemInfoID == 1
                                          select c).FirstOrDefault();

                    if (_SystemInformation != null)
                    {
                        RefreshValue();
                    }
                    else
                    {
                        _SystemInformation = new BO.SystemInformation();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void RefreshValue()
        {
            txtName.Text = _SystemInformation.CompanyName;
            rtbCompAdd.Text = _SystemInformation.Address;
            txtTelephone.Text = _SystemInformation.TelephoneNo;
            txtEmail.Text = _SystemInformation.EamilAddress;
            txtWebAdd.Text = _SystemInformation.WebAdd;
            txtPhotoPathCus.Text = _SystemInformation.CusPhotoPath;
            //txtNIDPath.Text=_SystemInformation.CustomerNIDPatht;
            txtSuppPhotoPath.Text = _SystemInformation.SuppPhotoPath;
            //txtSuppDocument.Text=_SystemInformation.SupplierDocPath;
            dtpSysStartDate.Value = _SystemInformation.SystemStartDate;
            txtEmpPhoto.Text = _SystemInformation.EmpPhotoPath;
        }

        private void btnPhotoPathCus_Click(object sender, EventArgs e)
        {
            if (_SystemInformation != null)
            {
                if (_SystemInformation.CusPhotoPath != "")
                {
                    dlgOpenFolderCPP.SelectedPath = _SystemInformation.CusPhotoPath;
                }
            }

            dlgOpenFolderProduct.ShowNewFolderButton = true;
            if (dlgOpenFolderCPP.ShowDialog() == DialogResult.OK)
            {
                if (dlgOpenFolderCPP.SelectedPath != "")
                {
                    btnPhotoClearCus.Enabled = true;
                    txtPhotoPathCus.Text = dlgOpenFolderCPP.SelectedPath + "\\";
                    _toolTip.SetToolTip(txtPhotoPathCus, txtPhotoPathCus.Text);
                }
            }
        }

        private void btnPhotoClearCus_Click(object sender, EventArgs e)
        {
            txtPhotoPathCus.Text = "";
        }

        private void btnSuppPhoto_Click(object sender, EventArgs e)
        {
            if (_SystemInformation != null)
            {
                if (_SystemInformation.SuppPhotoPath != "")
                {
                    dlgOpenFolderSPP.SelectedPath = _SystemInformation.SuppPhotoPath;
                }
            }

            dlgOpenFolderSPP.ShowNewFolderButton = true;
            if (dlgOpenFolderSPP.ShowDialog() == DialogResult.OK)
            {
                if (dlgOpenFolderSPP.SelectedPath != "")
                {
                    btnSuppClear.Enabled = true;
                    txtSuppPhotoPath.Text = dlgOpenFolderSPP.SelectedPath + "\\";
                    _toolTip.SetToolTip(txtSuppPhotoPath, txtSuppPhotoPath.Text);
                }
            }
        }

        private void btnSuppClear_Click(object sender, EventArgs e)
        {
            txtSuppPhotoPath.Text = "";
        }

        private void btnEmpPhotoPath_Click(object sender, EventArgs e)
        {
            if (_SystemInformation != null)
            {
                if (_SystemInformation.EmpPhotoPath != "")
                {
                    dlgOpenFolderEmp.SelectedPath = _SystemInformation.EmpPhotoPath;
                }
            }

            dlgOpenFolderProduct.ShowNewFolderButton = true;
            if (dlgOpenFolderEmp.ShowDialog() == DialogResult.OK)
            {
                if (dlgOpenFolderEmp.SelectedPath != "")
                {
                    btnEmpPPClear.Enabled = true;
                    txtEmpPhoto.Text = dlgOpenFolderEmp.SelectedPath + "\\";
                    _toolTip.SetToolTip(txtEmpPhoto, txtEmpPhoto.Text);
                }
            }
        }

        private void btnEmpPPClear_Click(object sender, EventArgs e)
        {
            txtEmpPhoto.Text = "";
        }

        private void RefreshObject()
        {
            _SystemInformation.CompanyName = txtName.Text;
            _SystemInformation.Address = rtbCompAdd.Text;
            _SystemInformation.TelephoneNo = txtTelephone.Text;
            _SystemInformation.EamilAddress = txtEmail.Text;
            _SystemInformation.WebAdd = txtWebAdd.Text;
            _SystemInformation.CusPhotoPath = txtPhotoPathCus.Text;
            //_SystemInformation.CustomerNIDPatht = txtNIDPath.Text;
            _SystemInformation.SuppPhotoPath = txtSuppPhotoPath.Text;
            //_SystemInformation.SupplierDocPath = txtSuppDocument.Text;
            _SystemInformation.SystemStartDate = dtpSysStartDate.Value;
            _SystemInformation.EmpPhotoPath = txtEmpPhoto.Text;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to save the information?", "Save Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {

                    using (DLMSEntities db = new DLMSEntities())
                    {
                        if (_SystemInformation.SystemInfoID <= 0)
                        {
                            RefreshObject();
                            _SystemInformation.SystemInfoID = db.SystemInformations.Count() > 0 ? db.SystemInformations.Max(obj => obj.SystemInfoID) + 1 : 1;
                            db.SystemInformations.Add(_SystemInformation);
                        }
                        else
                        {
                            _SystemInformation = db.SystemInformations.FirstOrDefault(obj => obj.SystemInfoID == _SystemInformation.SystemInfoID);
                            RefreshObject();
                        }

                        db.SaveChanges();
                        MessageBox.Show("Data saved successfully.", "Save Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}

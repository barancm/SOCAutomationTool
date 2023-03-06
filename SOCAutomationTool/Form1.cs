using DomainReputationServices.VirusTotal;
using SOCAutomationTool.Models;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace SOCAutomationTool
{
    public partial class Form1 : Form
    {
        public DataTable domainList { get; set; }
        public string filePath { get; set; }
        public bool isFileProcess { get; set; }
        public Form1()
        {
            isFileProcess = false;
            InitializeComponent();
        }
        private void dosyaYukle_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            filePath = openFileDialog1.FileName;
            if (openFileDialog1.FileName == "openFileDialog1")
            {
                return;
            }
            isFileProcess = true;
            label1.Text = openFileDialog1.FileName + " dosyasý yüklenmiþtir.";
        }
        private void GetDataFromFile(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    lblInfo.Text = filePath + " adlý dosyada veri bulunamamýþtýr.";
                    return;
                }

                DataTable dt = new DataTable();
                string[] satirlar = File.ReadAllLines(filePath);
                if (satirlar.Length > 0)
                {
                    string baslikArr = satirlar[0];
                    string[] basliklar = baslikArr.Split(',');
                    foreach (string baslik in basliklar)
                    {
                        dt.Columns.Add(new DataColumn(baslik));
                    }

                    for (int i = 1; i < satirlar.Length; i++)
                    {
                        FileModel fileModel = new FileModel();
                        string[] veriler = satirlar[i].Split(',');
                        DataRow dr = dt.NewRow();
                        int columnIndex = 0;
                        foreach (string veri in basliklar)
                        {
                            dr[veri] = veriler[columnIndex++];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                else
                {
                    lblInfo.Text = filePath + " adlý dosyada veri bulunamamýþtýr.";
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dt;
                }

                isFileProcess = false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnSorgula_Click(object sender, EventArgs e)
        {
            if (isFileProcess && !string.IsNullOrEmpty(filePath) && openFileDialog1.FileName != "openFileDialog1")
            {
                GetDataFromFile(filePath);
            }
            else if (!string.IsNullOrEmpty(txtURL.Text))
            {
                VirusTotal virusTotal = new VirusTotal();
                var result = virusTotal.POSTVirusTotalNetAsync(txtURL.Text);
            }
            else
            {
                lblInfo.Text = "Sorgulanacak veri bulunamadý.";
            }
        }
    }
}
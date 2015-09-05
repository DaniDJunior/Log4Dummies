using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AnalizadorLog
{
    public partial class Principal : Form
    {

        private List<Log4Dummies.Log> Logs = new List<Log4Dummies.Log>();

        public Principal()
        {
            InitializeComponent();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string i in openFileDialog.FileNames)
                {
                    List<Log4Dummies.Log> aux = Log4Dummies.Log.RecuperaLogs(openFileDialog.FileName);
                    foreach (Log4Dummies.Log ii in aux)
                    {
                        Logs.Add(ii);
                    }
                }
                PopulaLogs();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Logs = new List<Log4Dummies.Log>();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string i in openFileDialog.FileNames)
                {
                    List<Log4Dummies.Log> aux = Log4Dummies.Log.RecuperaLogs(openFileDialog.FileName);
                    foreach (Log4Dummies.Log ii in aux)
                    {
                        Logs.Add(ii);
                    }
                }
                PopulaLogs();
            }
        }

        private void PopulaLogs()
        {
            listLogs.Items.Clear();
            List<Log4Dummies.Log> AuxList = (from Dados in Logs orderby Dados.Data descending select Dados).ToList();
            foreach (Log4Dummies.Log i in AuxList)
            {
                ListViewItem Item = new ListViewItem();
                Item.Tag = i;
                Item.ImageIndex = 0;
                Item.Text = i.Data.ToString("dd/MM/yyyy HH:mm:ss");
                Item.SubItems.Add(i.Texto);
                listLogs.Items.Add(Item);
            }
        }

        private void listLogs_DoubleClick(object sender, EventArgs e)
        {
            foreach (ListViewItem i in listLogs.SelectedItems)
            {
                View aux = new View();
                aux.Texto = ((Log4Dummies.Log)i.Tag).Texto;
                aux.Show();
            }
        }

        private void Principal_Load(object sender, EventArgs e)
        {

        }

    }
}

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
    public partial class View : Form
    {

        public string Texto { get; set; }

        public View()
        {
            InitializeComponent();
        }

        private void View_Load(object sender, EventArgs e)
        {
            textBox1.Text = Texto;
        }
    }
}

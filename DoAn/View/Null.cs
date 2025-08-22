using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn.View
{
    public partial class Null: Form
    {
        public Null()
        {
            InitializeComponent();
            Form1 f1 = new Form1();
            f1.Show();
            this.Hide();
        }
    }
}

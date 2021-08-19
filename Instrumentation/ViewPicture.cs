using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Instrumentation
{
    public partial class ViewPicture : Form
    {
        public ViewPicture()
        {
            InitializeComponent();
        }

        public ViewPicture(Image img)
        {
            InitializeComponent();
            pictureBox1.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}

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
    public partial class InfoForm : Form
    {
        public InfoForm()
        {
            InitializeComponent();
        }      

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Size size = new Size(800, 600);
           //Bitmap bitt = new(bit, size);
           // bit = new Bitmap(bitt);
            Form imgform = new infoForm((Bitmap)pictureBox2.Image);
            imgform.ShowDialog();
           // ImgForm imgForm = new ImgForm();
           // imgForm.Show();
        }
    }

    internal class infoForm : Form
    {
        private Bitmap image;

        public infoForm(Bitmap image)
        {
            this.image = image;
        }
    }
}

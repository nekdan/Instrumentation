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
    public partial class InfoForm : System.Windows.Forms.Form
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
            //Form imgform = new infoForm((Bitmap)pictureBox2.Image);
            //imgform.ShowDialog();

            ImgForm form = new ImgForm();
            if ((Application.OpenForms["ImgForm"] as ImgForm) != null)
            {
                //Form is already open                
            }
            else
            {
                // Form is not open
                form.Show();
            }

            // ImgForm imgForm = new ImgForm();
            // imgForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }

    internal class infoForm : System.Windows.Forms.Form
    {
        private Bitmap image;

        public infoForm(Bitmap image)
        {
            this.image = image;
        }
    }
}

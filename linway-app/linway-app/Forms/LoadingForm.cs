using System;
//using System.Drawing;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class LoadingForm : Form
    {
        public LoadingForm()
        {
            InitializeComponent();
        }
        private void LoadingForm_Load(object sender, EventArgs e)
        {
            // pictureBox1.Image = Image.FromFile(@"Forms\src\cargando.jpg");
        }
        public void OpenIt()
        {
            Show();
            TopMost = true;
        }
        public void CloseIt()
        {
            Hide();
        }
    }
}

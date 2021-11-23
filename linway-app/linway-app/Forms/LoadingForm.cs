using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace linway_app.Forms
{
    public partial class LoadingForm : Form
    {

        public LoadingForm()
        {
            InitializeComponent();

        }

        private void Button_Click(object sender, EventArgs e)
        {

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

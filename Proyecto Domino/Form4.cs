using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto_Domino
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void SiguienteJugada_Click(object sender, EventArgs e)
        {

        }

        private void Atras_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.Show();
        }

        private void JuegoAtomatico_Click(object sender, EventArgs e)
        {

        }

        private void MostrarFinal_Click(object sender, EventArgs e)
        {

        }
    }
}

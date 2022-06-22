namespace Proyecto_Domino
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Iniciar_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Close();
            form2.Show();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
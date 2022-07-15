using System.Media;

namespace Proyecto_Domino
{
    public partial class Presentacion : Form
    {      
        
        
        public Presentacion()
        {
            InitializeComponent();
         try{
            string direccion = Directory.GetCurrentDirectory();
   
            string direccionMenu=direccion.Substring(0,direccion.Length - 14)+ "mus_menu0_3.wav";//string de la direccion de la cancion de fondo  

            SoundPlayer sonidoMenu = new SoundPlayer();
            sonidoMenu.SoundLocation = direccionMenu;
            sonidoMenu.PlayLooping();
            }
         catch{}
        }

        private void Iniciar_Click(object sender, EventArgs e)
        {
            Opciones form3 = new Opciones();
            this.Hide();
            form3.Show();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Presentacion_Load(object sender, EventArgs e)
        {

        }
    }
}
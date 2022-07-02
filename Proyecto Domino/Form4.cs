using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Logica;

namespace Proyecto_Domino
{
    public partial class Form4 : Form
    {
        Juego juego;
        List<string> guardadas = new List<string>();

        List<Jugador> jugadores;

        public Form4(List<Jugador> jugadores,ICondicionDeFinalizacion condicionDeFinalizacion,IOrdenDeLasJugadas ordenDeLasJugadas,IFormadeRepartir formadeRepartir,IFicha modoDeJuego,IFormaDeCalcularPuntuacion formaDeCalcular)
        {
            InitializeComponent();
            juego = new Juego(jugadores,condicionDeFinalizacion,ordenDeLasJugadas,formadeRepartir,modoDeJuego,formaDeCalcular);///////
            this.jugadores = jugadores;


            
        }
        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void SiguienteJugada_Click(object sender, EventArgs e)
        {
            if (juego.MoveNext()) { listBox1.Text = null;guardadas.Add(juego.Current.ToString());
                //listBox1.Text = guardadas; 
            }/////
        }

        private void Atras_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(jugadores);
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

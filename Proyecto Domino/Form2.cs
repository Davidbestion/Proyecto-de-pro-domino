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
    public partial class Form2 : Form
    {


        public List<Jugador> jugadores = new List<Jugador>();
        public TiposDeJugador tipos = new TiposDeJugador();
        public List<string> nombresDeJugadores = new List<string>();

        public Form2()
        {
            InitializeComponent();
            ////////////////////////////////////
            jugadores = new List<Jugador>();
            tipos = new TiposDeJugador();
            nombresDeJugadores = new List<string>();
            comboBox1.DataSource = tipos.NombreDeLosTipos;
            ////////////////////////////////////
        }
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)//Agregar Jugador
        {
            Jugador jugador = tipos.Comparer(comboBox1.SelectedItem.ToString());
            bool estaElNombre = false;
            foreach (var item in jugadores)
            {
                if (item.Nombre == textBox1.Text) { MessageBox.Show("No puede haber mas de un jugador con el mismo nombre"); estaElNombre = true; break; }
            }
            if (!estaElNombre)
            {
                jugador.Nombre = textBox1.Text;
                nombresDeJugadores.Add(textBox1.Text + " (" + comboBox1.SelectedItem.ToString() + ")");
                jugadores.Add(jugador);
                listBox1.DataSource = null;
                listBox1.DataSource = nombresDeJugadores;
            }
        }

        private void button3_Click(object sender, EventArgs e)//Siguiente
        {
            Form3 form3 = new Form3();
            this.Hide();
            form3.Show();
        }

        private void button2_Click(object sender, EventArgs e)//Eliminar Jugador
        {
            for (int i = 0; i < nombresDeJugadores.Count; i++)
            {
                if (listBox1.SelectedItem.ToString() == nombresDeJugadores[i]) { nombresDeJugadores.RemoveAt(i); jugadores.RemoveAt(i); }
                listBox1.DataSource = null;
                listBox1.DataSource = nombresDeJugadores;
            }
        }
    }
}

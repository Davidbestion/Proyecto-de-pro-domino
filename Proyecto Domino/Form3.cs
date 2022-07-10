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
using System.Reflection;

namespace Proyecto_Domino
{
    public partial class Form3 : Form
    {
        Assembly assembly;
        public List<Type> tiposDeModoDeJuego = new List<Type>();
        public List<string> NombreDeLosTiposDeJuego = new List<string>();
        public List<Type> tiposDeCondicionDeFinalizacion= new List<Type>();
        public List<string> NombreDeCondicionDeFinalizacion = new List<string>();
        public List<Type> tiposDeOrdenDeJugadas = new List<Type>();
        public List<string> NombreDeTiposDeOrdenDeJugada = new List<string>();
        public List<Type> tiposDeFormasDeRepartir = new List<Type>();
        public List<string> NombreDeFormasDeRepartir = new List<string>();
        public List<Type> tiposDeFormasDeCalcularPuntuacion = new List<Type>();
        public List<string> NombreDeFormasDeCalcularPuntuacion = new List<string>();



        public List<Jugador> jugadores;
        public Form3(List<Jugador>jugadores)
        {
            this.jugadores = jugadores;
            InitializeComponent();

           


            try
            {
                assembly = Assembly.Load("Logica");
                foreach (var x in assembly.GetTypes())
                {
                    if (x.GetInterface("IFicha") != null) { tiposDeModoDeJuego.Add(x); }
                    else if(x.GetInterface("ICondicionDeFinalizacion") != null) { tiposDeCondicionDeFinalizacion.Add(x); }
                    else if (x.GetInterface("IOrdenDeLasJugadas") != null) { tiposDeOrdenDeJugadas.Add(x); }
                    else if (x.GetInterface("IFormadeRepartir") != null) { tiposDeFormasDeRepartir.Add(x); }
                    else if (x.GetInterface("IFormaDeCalcularPuntuacion") != null) { tiposDeFormasDeCalcularPuntuacion.Add(x); }

                    //if (x.BaseType != null)
                    //{
                    //    x.GetInterface("Ificha")///
                    //    //switch (x.BaseType.Name)
                    //    //{
                    //    //    case "IFicha":
                    //    //        tiposDeModoDeJuego.Add(x);
                    //    //        break;
                    //    //    case "ICondicionDeFinalizacion":
                    //    //        tiposDeCondicionDeFinalizacion.Add(x);
                    //    //        break;
                    //    //    case "IOrdenDeLasJugadas":
                    //    //        tiposDeOrdenDeJugadas.Add(x);
                    //    //        break;
                    //    //    case "IFormadeRepartir":
                    //    //        tiposDeFormasDeRepartir.Add(x);
                    //    //        break;
                    //    //    case "IFormaDeCalcularPuntuacion":
                    //    //        tiposDeFormasDeCalcularPuntuacion.Add(x);
                    //    //        break;
                    //    //}
                    //}
                }
                foreach (var item in tiposDeModoDeJuego)
                {
                    NombreDeLosTiposDeJuego.Add(item.Name);
                }
                foreach (var item in tiposDeCondicionDeFinalizacion)
                {
                    NombreDeCondicionDeFinalizacion.Add(item.Name);
                }
                foreach (var item in tiposDeOrdenDeJugadas)
                {
                    NombreDeTiposDeOrdenDeJugada.Add(item.Name);
                }
                foreach (var item in tiposDeFormasDeRepartir)
                {
                    NombreDeFormasDeRepartir.Add(item.Name);
                }
                foreach (var item in tiposDeFormasDeCalcularPuntuacion)
                {
                    NombreDeFormasDeCalcularPuntuacion.Add(item.Name);
                }

            }
            catch (Exception) { }
            comboBox1.DataSource = null;
            comboBox1.DataSource = NombreDeLosTiposDeJuego;

            comboBox2.DataSource = null;
            comboBox2.DataSource = NombreDeCondicionDeFinalizacion;

            comboBox3.DataSource = null;
            comboBox3.DataSource = NombreDeTiposDeOrdenDeJugada;

            comboBox4.DataSource = null;
            comboBox4.DataSource = NombreDeFormasDeRepartir;

            comboBox5.DataSource = null;
            comboBox5.DataSource = NombreDeFormasDeCalcularPuntuacion;

        }
        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Siguiente_Click(object sender, EventArgs e)
        {
            //IFicha modoDeJuego =new FichasDe6();
            //ICondicionDeFinalizacion condicionDeFinalizacion = new FinalizacionPorPuntos();
            //IOrdenDeLasJugadas ordenDeLasJugadas = new OrdenNormal();
            //IFormadeRepartir formadeRepartir = new RepartoAleatorio();
            //IFormaDeCalcularPuntuacion formaDeCalcular = new ContarFichasDoblesPor2();

            IFichas modoDeJuego = null;
            ICondicionDeFinalizacion condicionDeFinalizacion = null;
            IOrdenDeLasJugadas ordenDeLasJugadas = null;
            IFormadeRepartir formadeRepartir = null;
            IFormaDeCalcularPuntuacion formaDeCalcular = null;


            foreach (var item in tiposDeModoDeJuego)
            {
                if (comboBox1.Text != null && comboBox1.Text == item.Name) { modoDeJuego= (IFichas)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeCondicionDeFinalizacion)
            {
                if (comboBox2.Text != null && comboBox2.Text == item.Name) {  condicionDeFinalizacion = (ICondicionDeFinalizacion)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeOrdenDeJugadas)
            {
                if (comboBox3.Text != null && comboBox3.Text == item.Name) {  ordenDeLasJugadas = (IOrdenDeLasJugadas)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeFormasDeRepartir)
            {
                if (comboBox4.Text != null && comboBox4.Text == item.Name) {  formadeRepartir = (IFormadeRepartir)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeFormasDeCalcularPuntuacion)
            {
                if (comboBox5.Text != null && comboBox5.Text == item.Name) {  formaDeCalcular = (IFormaDeCalcularPuntuacion)assembly.CreateInstance(item.FullName); }
            }


            if (modoDeJuego != null && condicionDeFinalizacion != null && ordenDeLasJugadas != null && formadeRepartir != null && formaDeCalcular != null)
            {

                if (modoDeJuego.GeneradorDeFichas().Count / modoDeJuego.FichasPorJugador >= jugadores.Count)//Condicion para saber si se puede jugar el modo seleccionado con esta cantidad de jugadores
                {
                    Form4 form4 = new Form4(jugadores, condicionDeFinalizacion, ordenDeLasJugadas, formadeRepartir, modoDeJuego, formaDeCalcular);
                    this.Hide();
                    form4.Show();
                }
                else { MessageBox.Show("No se puede jugar este modo de juego con esta cantidad de jugadores"); }

            }
            else { MessageBox.Show("Debe completar bien los campos para poder iniciar una partida"); }
        }

        private void Atras_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            this.Hide();
            form2.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}

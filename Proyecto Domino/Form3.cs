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
            IFicha modoDeJuego =new FichasDe6();
            ICondicionDeFinalizacion condicionDeFinalizacion = new FinalizacionPorPuntos();
            IOrdenDeLasJugadas ordenDeLasJugadas = new OrdenNormal();
            IFormadeRepartir formadeRepartir = new RepartoAleatorio();
            IFormaDeCalcularPuntuacion formaDeCalcular = new ContarFichasDoblesPor2();

            foreach (var item in tiposDeModoDeJuego)
            {
                if (comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == item.Name) { modoDeJuego= (IFicha)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeCondicionDeFinalizacion)
            {
                if (comboBox2.SelectedItem != null && comboBox2.SelectedItem.ToString() == item.Name) {  condicionDeFinalizacion = (ICondicionDeFinalizacion)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeOrdenDeJugadas)
            {
                if (comboBox3.SelectedItem != null && comboBox3.SelectedItem.ToString() == item.Name) {  ordenDeLasJugadas = (IOrdenDeLasJugadas)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeFormasDeRepartir)
            {
                if (comboBox4.SelectedItem != null && comboBox4.SelectedItem.ToString() == item.Name) {  formadeRepartir = (IFormadeRepartir)assembly.CreateInstance(item.FullName); }
            }
            foreach (var item in tiposDeFormasDeCalcularPuntuacion)
            {
                if (comboBox5.SelectedItem != null && comboBox5.SelectedItem.ToString() == item.Name) {  formaDeCalcular = (IFormaDeCalcularPuntuacion)assembly.CreateInstance(item.FullName); }
            }


            ///////////
            Form4 form4 = new Form4(jugadores,condicionDeFinalizacion,ordenDeLasJugadas,formadeRepartir,modoDeJuego,formaDeCalcular);
            this.Hide();
            form4.Show();
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

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
    public partial class Crear_Jugadores : Form
    {


        public List<IJugador> jugadores = new List<IJugador>();
        public List<string> nombresDeJugadores = new List<string>();
        public List<Type> tipos = new List<Type>();
        public List<string>nombreDeLosTiposDeJugadores=new List<string>();
        public Assembly assembly;
        ICondicionDeFinalizacion condicionDeFinalizacion;
        IOrdenDeLasJugadas ordenDeLasJugadas;
        IFormadeRepartir formadeRepartir;
        IFichas modoDeJuego;
        IFormaDeCalcularPuntuacion formaDeCalcular;


        public Crear_Jugadores(ICondicionDeFinalizacion condicionDeFinalizacion, IOrdenDeLasJugadas ordenDeLasJugadas, IFormadeRepartir formadeRepartir, IFichas modoDeJuego, IFormaDeCalcularPuntuacion formaDeCalcular)
        {

            InitializeComponent();

            this.condicionDeFinalizacion = condicionDeFinalizacion;
            this.ordenDeLasJugadas = ordenDeLasJugadas;
            this.formadeRepartir = formadeRepartir;
            this.modoDeJuego = modoDeJuego;
            this.formaDeCalcular = formaDeCalcular;
           

            try
            {
                assembly = Assembly.Load("Logica");//cargando los componentes del archivo Logica
                foreach (var x in assembly.GetTypes())
                {
                    if (x.GetInterface("IJugador") != null) { tipos.Add(x); }//añadiendo las clases que implementen de la interfaz IJuego
                }
                foreach (var item in tipos)
                {
                    nombreDeLosTiposDeJugadores.Add(item.Name);//añadiendo los nombres de esas clases
                }
            }
            catch (Exception) { }
            comboBox1.DataSource = nombreDeLosTiposDeJugadores;//añadiendo esos elementos para mostrarlos en el combobox

        }

        private void Agregar_Click(object sender, EventArgs e)//Agregar Jugador
        {
            
            if (comboBox1.Text == "") MessageBox.Show("Elija un tipo de jugador por favor.");//Da error si no selecciono nada del combobox
            else if (textBox1.Text == "") MessageBox.Show("Debe ingresar un nombre para su jugador.");//Da error si no le dio nombre al jugador
            else
            {
                IJugador jugador = null;
                foreach (var item in tipos)//Creando Jugadores segun lo seleccionado
                {
                    if (comboBox1.Text != null && comboBox1.Text== item.Name) { jugador = (IJugador)assembly.CreateInstance(item.FullName); }
                }
                bool estaElNombre = false;
                bool tipoDeJugadorExiste= false;
                foreach (var item in jugadores)//Comprobando que no haya mas de un jugador con el mismo nombre
                {
                    if (item.Nombre == textBox1.Text) { MessageBox.Show("No puede haber mas de un jugador con el mismo nombre"); estaElNombre = true; break; }
                }
                foreach (var item in nombreDeLosTiposDeJugadores)//Comprobando que selecciono algo valido en Tipo de Jugador
                {
                    if(item == comboBox1.Text) { tipoDeJugadorExiste = true;break; }
                }
                if (!tipoDeJugadorExiste) { MessageBox.Show("Este tipo de jugador no existe"); }

                if (!estaElNombre && tipoDeJugadorExiste)
                {
                    jugador.Nombre = textBox1.Text!;
                    nombresDeJugadores.Add(textBox1.Text + " (" + comboBox1.Text + ")");
                    jugadores.Add(jugador);
                    listBox1.DataSource = null;
                    listBox1.DataSource = nombresDeJugadores;
                }
            }
        }

        private void Siguiente_Click(object sender, EventArgs e)//Siguiente
        {
            if (jugadores.Count <= 1) { MessageBox.Show("Debe entrar mas jugadores"); }
            else
            {
                if (modoDeJuego.GeneradorDeFichas().Count / modoDeJuego.FichasPorJugador >= jugadores.Count)//Condicion para saber si se puede jugar el modo seleccionado con esta cantidad de jugadores
                {
                    Jugar form4 = new Jugar(jugadores, condicionDeFinalizacion, ordenDeLasJugadas, formadeRepartir, modoDeJuego, formaDeCalcular);
                    this.Hide();
                    form4.Show();
                }
                else { MessageBox.Show("No se puede jugar este modo de juego con esta cantidad de jugadores"); }
            }
        }

        private void Eliminar_Click(object sender, EventArgs e)//Eliminar Jugador
        {
            for (int i = 0; i < nombresDeJugadores.Count; i++)
            {
                if (listBox1.SelectedItem.ToString() == nombresDeJugadores[i]) { nombresDeJugadores.RemoveAt(i); jugadores.RemoveAt(i); }
                listBox1.DataSource = null;
                listBox1.DataSource = nombresDeJugadores;
            }
        }

        private void Atras_Click(object sender, EventArgs e)
        {
            Opciones opciones = new Opciones();
            this.Hide();
            opciones.Show();
        }

        private void Crear_Jugadores_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

﻿using System;
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
    public partial class Jugar : Form
    {
        Juego juego;
        List<string> guardadas = new List<string>();
        bool juegoAutomatico = false; 
        List<IJugador> jugadores;

        ICondicionDeFinalizacion condicionDeFinalizacion;
        IOrdenDeLasJugadas ordenDeLasJugadas;
        IFormadeRepartir formadeRepartir;
        IFicha modoDeJuego;
        IFormaDeCalcularPuntuacion formaDeCalcular;

        public Jugar(List<IJugador> jugadores,ICondicionDeFinalizacion condicionDeFinalizacion,IOrdenDeLasJugadas ordenDeLasJugadas,IFormadeRepartir formadeRepartir,IFicha modoDeJuego,IFormaDeCalcularPuntuacion formaDeCalcular)
        {
            this.condicionDeFinalizacion = condicionDeFinalizacion;
            this.ordenDeLasJugadas = ordenDeLasJugadas;
            this.formadeRepartir = formadeRepartir;
            this.modoDeJuego = modoDeJuego;
            this.formaDeCalcular = formaDeCalcular;

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
            if (juego.MoveNext()) { listBox1.DataSource = null;guardadas.Add(juego.Current.ToString());
                listBox1.DataSource  = guardadas; 
            }
        }

        private void Atras_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Crear_Jugadores form2 = new Crear_Jugadores(condicionDeFinalizacion, ordenDeLasJugadas, formadeRepartir, modoDeJuego, formaDeCalcular);
            this.Hide();
            form2.Show();
        }

        private void JuegoAtomatico_Click(object sender, EventArgs e)
        {
            juegoAutomatico = !juegoAutomatico;
            timer1.Enabled = juegoAutomatico;
            timer1.Interval = 1000;
        }

        private void MostrarFinal_Click(object sender, EventArgs e)
        {
            while (juego.MoveNext())
            {
                guardadas.Add(juego.Current.ToString());
            }
            listBox1.DataSource = null;
            listBox1.DataSource = guardadas;
            //Foreach in juego{guardadas.add(item.current)}

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (juegoAutomatico) { SiguienteJugada_Click(sender, e); }
        }

        private void Resetear_Click(object sender, EventArgs e)
        {
            guardadas = new List<string>();
            listBox1.DataSource = null;
            listBox1.DataSource=guardadas;
            juego.Reset();
        }
    }
}

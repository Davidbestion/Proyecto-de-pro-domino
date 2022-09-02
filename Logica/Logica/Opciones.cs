using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public interface IFicha
    {
        Tuple<object, object> contenido { get; }
        string ToString();
    }
    public class Ficha : IFicha
    {
        public Ficha(object a, object b)
        {
            contenido = new Tuple<object, object>(a, b);
        }
        public Tuple<object, object> contenido { get; set; }
        public override string ToString()
        {
            return this.contenido.Item1.ToString() + " | " + this.contenido.Item2.ToString();
        }
    }

    public interface IFichas
    {
        int FichasPorJugador { get; }
        List<IFicha> GeneradorDeFichas();//Para generar las fichas segun el tipo de juego que hayan escogido
    }
    public class FichasDe6 : IFichas
    {
        public int FichasPorJugador { get { return 7; } }
        public List<IFicha> GeneradorDeFichas()
        {
            List<IFicha> fichas = new List<IFicha>();
            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    fichas.Add(new Ficha(i, j));
                }
            }
            return fichas;
        }
    }
    public class FichasDe9 : IFichas
    {
        public int FichasPorJugador { get { return 10; } }
        public List<IFicha> GeneradorDeFichas()
        {
            List<IFicha> fichas = new List<IFicha>();
            for (int i = 0; i <= 9; i++)
            {
                for (int j = i; j <= 9; j++)
                {
                    fichas.Add(new Ficha(i, j));
                }
            }
            return fichas;
        }
    }
    public interface ICondicionDeFinalizacion
    {
        bool Finalizo(IJugador jugador, IFicha fichaJugada, List<IJugador> jugadores, object extremo1, object extremo2, out bool tabla);
        void Reset();
    }
    public class PorPuntos_50puntos : ICondicionDeFinalizacion
    {
        public void Reset()
        {

        }
        public bool Finalizo(IJugador jugador, IFicha fichaJugada, List<IJugador> jugadores, object extremo1, object extremo2, out bool tabla)
        {

            if (jugador.Fichas.Count == 0)//Comprobando si algun jugador se pego
            {
                jugador.Ganador = true;
                tabla = false;
                return true;
            }

            if (jugador.Puntuacion >= 50) { jugador.Ganador = true; tabla = false; return true; }

            foreach (var item in jugadores)//Comprobando si se tranco el juego
            {
                foreach (var ficha in item.Fichas)
                {
                    if (ficha.contenido.Item1.Equals(extremo1) || ficha.contenido.Item2.Equals(extremo1) || ficha.contenido.Item1.Equals(extremo2) || ficha.contenido.Item2.Equals(extremo2))
                    { tabla = false; return false; }
                }
            }

            tabla = true;

            double mejorPuntuacion = 0;
            int jugadorConMasPuntos = 0;
            foreach (var item in jugadores)//como se tranco el juego se saca el jugador con mayor puntaje
            {
                if (mejorPuntuacion < item.Puntuacion)
                {
                    jugadorConMasPuntos = jugadores.IndexOf(item);
                    mejorPuntuacion = item.Puntuacion;
                }
            }
            jugadores[jugadorConMasPuntos].Ganador = true;


            return true;
        }

    }
    public class PorPase : ICondicionDeFinalizacion
    {
        int[] pasesSeguidos;
        bool primeraVez = true;

        public void Reset()
        {
            primeraVez = true;
        }

        public bool Finalizo(IJugador jugador, IFicha fichaJugada, List<IJugador> jugadores, object extremo1, object extremo2, out bool tabla)
        {
            if (primeraVez) { pasesSeguidos = new int[jugadores.Count]; primeraVez = false; }
            if (jugador.Fichas.Count == 0)
            {
                jugador.Ganador = true;
                tabla = false;
                primeraVez = true;
                return true;
            }
            if (fichaJugada == null)
            {
                int indice = jugadores.IndexOf(jugador);
                pasesSeguidos[indice]++;
                if (pasesSeguidos[indice] == 2)
                {
                    double MejorPuntuacion = 0;
                    int JugadorConMasPuntos = 0;
                    foreach (var item in jugadores)
                    {
                        if (MejorPuntuacion < item.Puntuacion) { JugadorConMasPuntos = jugadores.IndexOf(item); MejorPuntuacion = item.Puntuacion; }
                    }
                    jugadores[JugadorConMasPuntos].Ganador = true;
                    tabla = false;
                    primeraVez = true;
                    return true;
                }
            }
            else if (fichaJugada != null) { pasesSeguidos[jugadores.IndexOf(jugador)] = 0; }


            foreach (var item in jugadores)//Comprobando si se tranco el juego
            {
                foreach (var item2 in item.Fichas)
                {
                    if (item2.contenido.Item1.Equals(extremo1) || item2.contenido.Item2.Equals(extremo1) || item2.contenido.Item1.Equals(extremo2) || item2.contenido.Item2.Equals(extremo2))
                    { tabla = false; return false; }
                }
            }

            tabla = true;

            double mejorPuntuacion = 0;
            int jugadorConMasPuntos = 0;
            foreach (var item in jugadores)
            {
                if (mejorPuntuacion < item.Puntuacion) { jugadorConMasPuntos = jugadores.IndexOf(item); mejorPuntuacion = item.Puntuacion; }
            }
            jugadores[jugadorConMasPuntos].Ganador = true;

            primeraVez = true;
            return true;
        }
    }
    public interface IOrdenDeLasJugadas
    {
        IJugador Siguiente(List<IJugador> jugadores, bool SePaso);
        public void Reset();//Para cuando se haga un juego nuevo se vuelva a empezar desde el jugador 1
    }
    public class Normal : IOrdenDeLasJugadas
    {
        public IJugador Siguiente(List<IJugador> jugadores, bool SePaso)
        {
            for (int i = 0; i < jugadores.Count; i++)
            {
                if (jugadores[i].EsTurno)
                {
                    if (i == jugadores.Count - 1)
                    {
                        jugadores[jugadores.Count - 1].EsTurno = false;
                        jugadores[0].EsTurno = true;
                        return jugadores[0];
                    }
                    else
                    {
                        jugadores[i].EsTurno = false;
                        jugadores[i + 1].EsTurno = true;
                        return jugadores[i + 1];
                    }
                }
            }

            jugadores[0].EsTurno = true;//Para cuando sea el primer turno
            return jugadores[0];
        }
        public void Reset()
        {

        }
    }
    public class CambioSiSePasa : IOrdenDeLasJugadas
    {
        List<IJugador> pasados = new List<IJugador>();//para en el caso que se entre en un bucle entre dos jugadores q no pueden jugar,poner a jugar a otro Jugador
        bool ordenCambiado = false;
        int VecesPasadasSeguidas = 0;
        bool primeraVez = true;
        public void Reset()
        {
            pasados = new List<IJugador>();
            ordenCambiado = false;
            VecesPasadasSeguidas = 0;
            primeraVez = true;
        }

        public IJugador Siguiente(List<IJugador> jugadores, bool SePaso)
        {
            if (primeraVez)
            {
                foreach (IJugador jugador in jugadores)
                {
                    pasados.Add(jugador);
                }
                primeraVez = false;
            }



            for (int i = 0; i < jugadores.Count; i++)
            {
                if (jugadores[i].EsTurno)
                {
                    if (SePaso)
                    {
                        pasados.Remove(jugadores[i]);
                        VecesPasadasSeguidas++;
                        ordenCambiado = !ordenCambiado;
                    }
                    else { VecesPasadasSeguidas = 0; primeraVez = true; pasados = new List<IJugador>(); }//Se pone primeraVez en true para q vuelva a crear la lista de jugadores pasados

                    if (VecesPasadasSeguidas >= 2)
                    {
                        jugadores[i].EsTurno = false;
                        pasados[0].EsTurno = true;
                        return pasados[0];
                    }


                    if (ordenCambiado)
                    {
                        if (i == 0)
                        {
                            jugadores[0].EsTurno = false;
                            jugadores[jugadores.Count - 1].EsTurno = true;
                            return jugadores[jugadores.Count - 1];
                        }
                        else
                        {
                            jugadores[i].EsTurno = false;
                            jugadores[i - 1].EsTurno = true;
                            return jugadores[i - 1];
                        }
                    }




                    if (i == jugadores.Count - 1)
                    {
                        jugadores[jugadores.Count - 1].EsTurno = false;
                        jugadores[0].EsTurno = true;
                        return jugadores[0];
                    }
                    else
                    {
                        jugadores[i].EsTurno = false;
                        jugadores[i + 1].EsTurno = true;
                        return jugadores[i + 1];
                    }
                }
            }

            jugadores[0].EsTurno = true;//Para cuando sea el primer turno
            return jugadores[0];
        }
    }
    public interface IFormadeRepartir
    {
        void Repartir(List<IFicha> fichas, List<IJugador> jugadores, int cantidadDeFichas);
    }
    public class Aleatorio : IFormadeRepartir
    {
        public void Repartir(List<IFicha> fichas, List<IJugador> jugadores, int cantidadDeFichas)
        {

            foreach (var item in jugadores)
            {
                item.Seleccionar(fichas, false, cantidadDeFichas);
            }
        }
    }
    public class BocaArriba : IFormadeRepartir
    {
        public void Repartir(List<IFicha> fichas, List<IJugador> jugadores, int cantidadDeFichas)
        {
            foreach (var item in jugadores)
            {
                item.Seleccionar(fichas, true, cantidadDeFichas);
            }
        }
    }
    public interface IFormaDeCalcularPuntuacion
    {
        int CalcularPuntuacion(IFicha ficha);
        int Conversion(object elemento);
    }
    public class FichasDoblesPor2 : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(IFicha ficha)
        {
            if (ficha == null || ficha.contenido.Item1 == null || ficha.contenido.Item2 == null) return 0;//si se paso que no le sume puntuacion
            int valor1 = Conversion(ficha.contenido.Item1);
            int valor2 = Conversion(ficha.contenido.Item2);
            if (valor1 == valor2) { return (valor1 + valor2) * 2; }
            else { return valor1 + valor2; }
        }
        public int Conversion(object elemento)
        {
            //Agregar aqui las conversiones que necesite.
            if (elemento.GetType().Equals(typeof(int)))//verifico si el object es de tipo int
            {
                return (int)elemento;
            }

            else return 0;
        }
    }
    public class PenalizarFichasDobles : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(IFicha ficha)
        {
            if (ficha == null || ficha.contenido.Item1 == null || ficha.contenido.Item2 == null) return 0;//si se paso que no le sume puntuacion
            int valor1 = Conversion(ficha.contenido.Item1);
            int valor2 = Conversion(ficha.contenido.Item2);
            if (valor1 == valor2) { return -(valor1 + valor2); }
            else { return valor1 + valor2; }
        }
        public int Conversion(object elemento)
        {
            //Agregar aqui las conversiones que necesite.
            if (elemento.GetType().Equals(typeof(int)))//verifico si el object es de tipo int
            {
                return (int)elemento;
            }

            else return 0;
        }
    }
    public class Usual : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(IFicha ficha)
        {
            if (ficha == null || ficha.contenido.Item1 == null || ficha.contenido.Item2 == null) return 0;//si se paso que no le sume puntuacion
            int valor1 = Conversion(ficha.contenido.Item1);
            int valor2 = Conversion(ficha.contenido.Item2);
            return valor1 + valor2;
        }
        public int Conversion(object elemento)
        {
            //Agregar aqui las conversiones que necesite.
            if (elemento.GetType().Equals(typeof(int)))//verifico si el object es de tipo int
            {
                return (int)elemento;
            }

            else return 0;
        }
    }
}

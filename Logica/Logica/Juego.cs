using System.Collections;
using System.Reflection;

namespace Logica
{
    public class Jugada//Para imprimir cada jugada 
    {
        IJugador jugador;//jugador que le tocaba el turno
        IFicha ficha;
        bool terminó;//Si es true, es porq terminó el juego, osea que un jugador jugo todas sus fichas
        bool trancado;//Si es true, es porq terminó el juego, osea que ningun jugador tiene ficha jugable disponible
        IJugador ganador;

        public Jugada(IJugador jugador, IFicha ficha,bool terminó,IJugador ganador,bool trancado)
        {
            this.jugador = jugador;
            this.ficha = ficha;
            this.terminó= terminó;
            this.ganador = ganador;
            this.trancado= trancado;
        }

        public override string ToString()
        {
            string oracion;
            if (ficha == null)
            {
                oracion = jugador.Nombre + " se ha pasado." + " Se mantiene con: " + jugador.Puntuacion + " puntos."; 
            }
            else { oracion = jugador.Nombre + " ha jugado [" + ficha.ToString() + "]. Tiene: " + jugador.Puntuacion + " puntos."; }
            if (terminó)
            {
                if (trancado) { oracion += " Juego trancado, el ganador es " + ganador.Nombre + " con: " + ganador.Puntuacion + " puntos."; }
                else { oracion += " El ganador es " + ganador.Nombre + " con: " + ganador.Puntuacion + " puntos."; }
            }
            return oracion ;
        }
    }
    
    public class Juego : IEnumerator<Jugada>
    {
        List<IJugador> Jugadores;
        ICondicionDeFinalizacion CondicionDeFin;
        IOrdenDeLasJugadas OrdenDeJugadas;
        IFormadeRepartir FormadeRepartir;
        IModoDeJuego Fichas;
        IFormaDeCalcularPuntuacion CalcularPuntuacion;
        IJugador ganador;

        bool PrimerTurno;
        bool SePaso;
        bool FinDelJuego;
        bool JuegoTrancado;
        object Extremo1;
        object Extremo2;
        List<IFicha> fichas;//Fichas que repartir
        Jugada jugadaActual;
        
        public Juego(List<IJugador> Jugadores, ICondicionDeFinalizacion CondicionDeFin, IOrdenDeLasJugadas OrdenDeJugadas, IFormadeRepartir FormadeRepartir, IModoDeJuego Fichas, IFormaDeCalcularPuntuacion CalcularPuntuacion)
        {

            this.Jugadores= Jugadores;
            this.CondicionDeFin= CondicionDeFin;
            this.OrdenDeJugadas= OrdenDeJugadas;
            this.FormadeRepartir= FormadeRepartir;
            this.Fichas= Fichas;
            this.CalcularPuntuacion = CalcularPuntuacion;
            
            PrimerTurno = true;
            SePaso = false;
            Extremo1 = null;
            Extremo2 = null;
            fichas = Fichas.GeneradorDeFichas();
            FinDelJuego = false;
            JuegoTrancado = false;
        }

        public Jugada Current { get { return jugadaActual; } }

        object IEnumerator.Current { get { return Current; } }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (!FinDelJuego)
            {
                if (PrimerTurno)
                {
                    foreach (var item in Jugadores)//intruyendo a los jugadores de la manera de calcular las puntuaciones
                    {
                        item.CalcularPuntuacion = CalcularPuntuacion.CalcularPuntuacion;
                    }
                    FormadeRepartir.Repartir(fichas, Jugadores,Fichas.FichasPorJugador);//Repartiendo y haciendo que los jugadores escojan sus fichas
                   
                }
                IJugador jugadorActual = OrdenDeJugadas.Siguiente(Jugadores, SePaso);
                SePaso = false;//reseteando el SePaso
                IFicha fichaJugada = jugadorActual.Juega(fichas, Extremo1, Extremo2);
                jugadorActual.Puntuacion+= CalcularPuntuacion.CalcularPuntuacion(fichaJugada);
                ReconocerExtremos(fichaJugada);
                if (fichaJugada != null)
                {
                    fichas.Add(fichaJugada);//Recogiendo la ficha q se jugo
                }
                FinDelJuego = CondicionDeFin.Finalizo(jugadorActual,fichaJugada,Jugadores,Extremo1,Extremo2,out JuegoTrancado);
                if (FinDelJuego)//Si termino el juego,buscar al ganador
                {    
                    foreach (var item in Jugadores)
                    {
                        if (item.Ganador) { ganador = item; }
                    }
                }

                jugadaActual = new Jugada(jugadorActual, fichaJugada, FinDelJuego,ganador, JuegoTrancado);

                PrimerTurno = false;

                return true;
            }
            return false;

        }

        public void Reset()
        {
            PrimerTurno = true;
            SePaso = false;
            Extremo1 = null;
            Extremo2 = null;
            fichas = Fichas.GeneradorDeFichas();
            FinDelJuego = false;
            JuegoTrancado = false;
            OrdenDeJugadas.Reset();
            CondicionDeFin.Reset();


            foreach (var jugador in Jugadores)
            {
                jugador.Fichas = new List<IFicha>();
                jugador.Ganador = false;
                jugador.EsTurno = false;
                jugador.Puntuacion = 0;
            }
        }

        public void ReconocerExtremos(IFicha fichaJugada)
        {
            if (fichaJugada == null)//Reconociendo si se paso
            {
                SePaso = true;
                return;
            }

            object objeto1 = fichaJugada.contenido.Item1;
            object objeto2 = fichaJugada.contenido.Item2;

            if (PrimerTurno)
            {
                Extremo1 = objeto1;
                Extremo2 = objeto2;
                return;
            }

            if (objeto1.Equals(Extremo1))
            {
                Extremo1 = objeto2;
            }
            else if(objeto1.Equals(Extremo2))
            {
                Extremo2 = objeto2;
            }
            else if (objeto2.Equals(Extremo1))
            {
                Extremo1 = objeto1;
            }
            else { Extremo2 = objeto1; }
        }
    }
}
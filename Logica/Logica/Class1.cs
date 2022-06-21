namespace Logica
{
    public abstract class Jugador
    {
        public string Nombre;

        public double Puntuacion;

        public bool EsTurno;

        public List<Tuple<int, int>> Fichas;

        public Jugador(string Nombre)
        {
            this.Nombre = Nombre;
        }

        protected abstract int Juega(List<Tuple<int, int>> fichas);
        protected abstract void Seleccionar(List<Tuple<int, int>> fichas,bool[]fichas2);  

    }
    public class JugadorAleatorio : Jugador
    {
        public JugadorAleatorio(string Nombre) : base(Nombre) { }

        protected override int Juega(List<Tuple<int, int>> fichas)
        {
            Random random = new Random();
            int jugada = random.Next(fichas.Count);//Si la ficha no puede jugarse?
            return jugada;
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool[] fichas2)
        {
            Random selecciona = new Random();
            for (int i = 0; i <= 7; i++)//Cantidad de fichas a robar
            {
                while (true)
                {
                    int fichaSeleccionada = selecciona.Next(0, fichas2.Length - 1);
                    if (!fichas2[fichaSeleccionada]) { fichas2[fichaSeleccionada] = true;Fichas.Add(fichas[fichaSeleccionada]);break; }
                }
            }

        }
    }

    public class JugadorGoloso : Jugador
    {
        public JugadorGoloso(string Nombre) : base(Nombre) { }

        protected override int Juega(List<Tuple<int, int>> fichas)
        {
            int mayorValor = 0;
            int fichaDeMayorValor = 0;
            int[] jugadas = new int[fichas.Count];
            for (int i = 0; i < jugadas.Length; i++)
            {
                jugadas[i] = fichas[i].Item1 + fichas[i].Item2;
                if (jugadas[i] > mayorValor) { mayorValor = jugadas[i]; fichaDeMayorValor = i; }
            }
            return fichaDeMayorValor;
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool[] fichas2)
        {

            //Random selecciona = new Random();
            //for (int i = 0; i <= 7; i++)//Cantidad de fichas a robar
            //{
            //    while (true)
            //    {
            //        int fichaSeleccionada = selecciona.Next(0, fichas2.Length - 1);
            //        if (!fichas2[fichaSeleccionada]) { fichas2[fichaSeleccionada] = true; Fichas.Add(fichas[fichaSeleccionada]); break; }
            //    }
            //}
        }
    }

    // public class JugadorInteligente : Jugador { }
    public class TiposDeJugador///////////////////Assembly
    {

        public List<string> NombreDeLosTipos { get { return nombreDeLosTipos; } }
        public List<string> nombreDeLosTipos = new List<string> { "Jugador Aleatorio", "Jugador Goloso" };//
        //List<Jugador> tiposdejugadores = new List<Jugador> { new JugadorAleatorio(""), new JugadorGoloso("") };

        public void Add(string tipo)
        {
            nombreDeLosTipos.Add(tipo);
        }
        public Jugador Comparer(string nombre)
        {
            switch (nombre)
            {
                case "Jugador Aleatorio":
                    return new JugadorAleatorio("");
                case "Jugador Goloso":
                    return new JugadorGoloso("");
                default:
                    throw new Exception("paso algo en el Comparer");
            }
        }

    }

    public class Jugada
    {
        Jugador jugador;
        Tuple<int, int> ficha;

        public Jugada(Jugador jugador, Tuple<int, int> ficha)
        {
            this.jugador = jugador;
            this.ficha = ficha;
        }

        public override string ToString()
        {
            return jugador.Nombre + " ha jugado [" + ficha.Item1 + "|" + ficha.Item2 + "]";
        }
    }
        /////////////////////////////////////////////////
        ///Cosas DEl FOrm 3

        interface IFicha
        {
            List<Tuple<int, int>> GeneradorDeFichas(int valor);
        }

        public class Fichas : IFicha
        {
            public List<Tuple<int, int>> GeneradorDeFichas(int valor)
            {
                List<Tuple<int, int>> listaDeFichas = new List<Tuple<int, int>>();
                for (int i = 0; i <= valor; i++)
                {
                    for (int j = 0; j <= valor; j++)
                    {
                        listaDeFichas.Add(new Tuple<int, int>(i, j));

                    }
                }
                return listaDeFichas;
            }
        }



        interface ICondicionDeFinalizacion
        {
            //  bool Finalizo(List<Jugador> jugadores);

        }
        public class FinalizacionPorPuntos : ICondicionDeFinalizacion
        {
            public bool Finalizo(List<Jugador> jugadores, double puntuacion)
            {
                foreach (var item in jugadores)
                {
                    if (item.Puntuacion >= puntuacion) { return true; }
                }
                return false;
            }
        }

        // public class FinalizacionPorPase : ICondicionDeFinalizacion { }


        interface IOrdenDeLasJugadas
        {
            Jugador Orden(List<Jugador> jugadores, bool SePaso);
        }


        public class OrdenNormal : IOrdenDeLasJugadas
        {
            public Jugador Orden(List<Jugador> jugadores, bool SePaso)
            {
                for (int i = 0; i < jugadores.Count; i++)
                {
                    if (jugadores[i].EsTurno)
                    {
                        if (i == jugadores.Count - 1)
                        {
                            jugadores[0].EsTurno = true;
                            return jugadores[0];
                        }
                        else
                        {
                            jugadores[i + 1].EsTurno = true;
                            return jugadores[i + 1];
                        }
                    }
                }

                jugadores[0].EsTurno = true;//Para cuando sea el primer turno
                return jugadores[0];
            }
        }

        public class OrdenCambiadoSiSePasa : IOrdenDeLasJugadas
        {
            public Jugador Orden(List<Jugador> jugadores, bool SePaso)
            {
                for (int i = 0; i < jugadores.Count; i++)
                {
                    if (jugadores[i].EsTurno)
                    {
                        if (SePaso)
                        {
                            if (i == 0)
                            {
                                jugadores[jugadores.Count - 1].EsTurno = true;
                                return jugadores[jugadores.Count - 1];
                            }
                            else
                            {
                                jugadores[i - 1].EsTurno = true;
                                return jugadores[i - 1];
                            }


                        }


                        if (i == jugadores.Count - 1)
                        {
                            jugadores[0].EsTurno = true;
                            return jugadores[0];
                        }
                        else
                        {
                            jugadores[i + 1].EsTurno = true;
                            return jugadores[i + 1];
                        }
                    }
                }

                jugadores[0].EsTurno = true;//Para cuando sea el primer turno
                return jugadores[0];
            }
        }

    interface IFormadeRepartir
    {
        void Repartir(IList<Tuple<int, int>> fichas, List<Jugador> jugadores);
    }
    public class RepartoAleatorio : IFormadeRepartir
    {
        public void Repartir(IList<Tuple<int, int>> fichas,List<Jugador>jugadores)
        {
            bool[]FichasBocaAbajo=new bool[fichas.Count];
            for (int i = 0; i < jugadores.Count; i++)
            {

            }

            
           
        }
    }

}
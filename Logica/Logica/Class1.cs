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

        protected abstract int Juega(List<Tuple<int, int>> fichas, int num1, int num2);//Los numero disponibles en el tablero
        protected abstract void Seleccionar(List<Tuple<int, int>> fichas, bool[] fichas2);

        protected abstract bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2);//Pa cuando la hagas interface en vez de abstracta.
        //{
        //    return fichas[i].Item1 == num1 || fichas[i].Item2 == num1 || fichas[i].Item1 == num2 || fichas[i].Item2 == num2;
        //}

    }
    public class JugadorAleatorio : Jugador
    {
        public JugadorAleatorio(string Nombre) : base(Nombre) { }

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            throw new NotImplementedException();
        }

        protected override int Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = fichas.Count;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(fichas[i], num1, num2)) break;
                else if (i == length - 1) return 0;//Cambia en el return lo q quieras devolver si no se encuentra ficha jugable
            }
            int jugada = 0;
            do
            {
                jugada = random.Next(length);//Si la ficha no puede jugarse?
            } while (fichas[jugada].Item1 == num1 || fichas[jugada].Item2 == num1 || fichas[jugada].Item1 == num2 || fichas[jugada].Item2 == num2);
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

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            throw new NotImplementedException();
        }

        protected override int Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = fichas.Count;
            int mayorValor = 0;
            int fichaDeMayorValor = 0;
            int valor = 0;
            //int[] jugadas = new int[fichas.Count];Pa no crear array, q ocupa espacio en memoria...
            for (int i = 0; i < length; i++)
            {
                //Condicional q determina si la ficha actual se puede poner en el tablero
                if (fichas[i].Item1 == num1 || fichas[i].Item2 == num1 || fichas[i].Item1 == num2 || fichas[i].Item2 == num2)
                {
                    valor = fichas[i].Item1 + fichas[i].Item2;
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = i; }
                }
                //jugadas[i] = fichas[i].Item1 + fichas[i].Item2;
                //if (jugadas[i] > mayorValor) { mayorValor = jugadas[i]; fichaDeMayorValor = i; }
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

    public class JugadorInteligente : Jugador
    {
        public JugadorInteligente(string Nombre) : base(Nombre) { }

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            throw new NotImplementedException();
        }

        protected override int Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            throw new NotImplementedException();
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool[] fichas2)
        {
            throw new NotImplementedException();
        }
    }
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
                    for (int j = i; j <= valor; j++)//tenias j = 0, puse j = i pa q no se creen duplicados de las fichas q se han creado hasta ahora
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
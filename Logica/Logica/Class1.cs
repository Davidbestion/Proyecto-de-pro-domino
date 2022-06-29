using System.Collections;

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

        public abstract Tuple<int,int> Juega(List<Tuple<int, int>> fichas, int num1, int num2);//Los numeros disponibles en el tablero
        protected abstract void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas);

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

        public override Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;
            Random random = new Random();

            if (num1 == -1 && num2 == -1)//Para si es el primer turno jugar una ficha random
            {
                return Fichas[random.Next(length)];
            }
            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(Fichas[i], num1, num2)) break;
                else if (i == length - 1) return new Tuple<int, int>(-1, -1);//Si no puede jugar ninguna ficha, retorna (-1,-1)
            }
            Tuple<int, int> ficha = new Tuple<int, int>(-1, -1);
            do
            {
                ficha = Fichas[random.Next(length)];
            } while (!EsFichaJugable(ficha, num1, num2));
            return ficha;
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {
            Random selecciona = new Random();
            while (Fichas.Count < cantFichas)
            {
                int a = selecciona.Next(fichas.Count);
                Fichas.Add(fichas[a]);
                fichas.Remove(fichas[a]);
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

        public  override Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;// -_- really nigga?
            int mayorValor = 0;
            Tuple<int,int> fichaDeMayorValor = new Tuple<int, int>(-1,-1);
            //int[] jugadas = new int[fichas.Count];Pa no crear array, q ocupa espacio en memoria...

            if (num1 == -1 && num2 == -1)//Por si es el primer turno,que juegue la ficha mas grande
            {
                for (int i = 0; i < Fichas.Count; i++)
                {
                    int valor = Fichas[i].Item1 + Fichas[i].Item2;
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
                return fichaDeMayorValor;
            }


            for (int i = 0; i < length; i++)
            {
                
                //Condicional q determina si la ficha actual se puede poner en el tablero
                if (Fichas[i].Item1 == num1 || Fichas[i].Item2 == num1 || Fichas[i].Item1 == num2 || Fichas[i].Item2 == num2)
                {
                    int valor = Fichas[i].Item1 + Fichas[i].Item2;
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
                //jugadas[i] = fichas[i].Item1 + fichas[i].Item2;
                //if (jugadas[i] > mayorValor) { mayorValor = jugadas[i]; fichaDeMayorValor = i; }
            }
            return fichaDeMayorValor;
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {/////////////////////////////////////////ATENCION//////////////////////////////////////////////////////////////
            //cantFichas TIENE Q SER MENOR Q fichas.Count!!!!!!!!!!!!
            Random random = new Random();
            if (!bocaArriba)
            {
                while (Fichas.Count < cantFichas)
                {
                    int a = random.Next(fichas.Count);
                    Fichas.Add(fichas[a]);
                    fichas.Remove(fichas[a]);
                }
            }
            else
            {
                int length = fichas.Count;
                int valor = 0;
                Tuple<int, int> fichaDeMenorValor = fichas[0];
                int menorValor = fichaDeMenorValor.Item1 + fichaDeMenorValor.Item2;
                while (Fichas.Count < cantFichas)
                {
                    length = fichas.Count;
                    for(int i = 0; i < length; i++)
                    {
                        if (fichaDeMenorValor == fichas[i]) continue;
                        valor = fichas[i].Item1 + fichas[i].Item2;
                        if(valor < menorValor)
                        {
                            fichaDeMenorValor = fichas[i];
                            menorValor = valor;
                        }
                        if(i == length - 1)
                        {
                            Fichas.Add(fichaDeMenorValor);
                            fichas.Remove(fichaDeMenorValor);
                        }
                    }
                }
            }
        }
    }

    public class JugadorInteligente : Jugador
    {
        public JugadorInteligente(string Nombre) : base(Nombre) 
        { 
            Fichas = new List<Tuple<int, int>>();
        }

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            throw new NotImplementedException();
        }

        public override Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {//La idea de esto es escojer la ficha q pueda jugar por un numero y q su otro numero
         //sea lo mas comun posible entre el resto de las fichas q tengo. 
            int length = Fichas.Count;
            int potencialMaximo = 0;
            Tuple<int, int> fichaElegida = new Tuple<int, int>(-1, -1);
            foreach (Tuple<int, int> ficha in Fichas)//Reviso por las fichas q tengo
            {
                int potencial = 0;
                if (EsFichaJugable(ficha, num1, num2))//Escojo las q puedo jugar
                {
                    bool puedeJugar = true;
                    int numero = -1;
                    //Guardo el numero por el cual la puedo jugar
                    if (ficha.Item1 == num1 || ficha.Item1 == num2)
                    {
                        numero = ficha.Item2;
                        foreach (Tuple<int, int> ficha2 in Fichas)
                        {//Reviso cuales fichas de las q tengo tienen el otro numero
                            //eso me dira que tan buena es la ficha jugable
                            if (ficha2 == ficha) continue;
                            if (ficha2.Item1 == numero || ficha2.Item2 == numero)
                            {
                                potencial++;//potencial de la ficha, NO DE ficha2.!!!!!!!!!
                            }
                            if (potencial > potencialMaximo)
                            {
                                fichaElegida = ficha;
                                potencialMaximo = potencial;
                            }

                        }
                    }//Repito el proceso con el otro numero de la ficha jugable por si la puedo pegar
                    //con ambos numero en el "tablero": num1 y num2.
                    if (ficha.Item2 == num1 || ficha.Item2 == num2)
                    {
                        numero = ficha.Item1;
                        foreach (Tuple<int, int> ficha2 in Fichas)
                        {
                            if (ficha2 == ficha) continue;
                            if (ficha2.Item1 == numero || ficha2.Item2 == numero)
                            {
                                potencial++;
                            }
                            if (potencial > potencialMaximo)
                            {
                                fichaElegida = ficha;
                                potencialMaximo = potencial;
                            }

                        }
                    }
                }
            }
            //Si nunca encontro ficha jugable entonces retorna (-1,-1)
            return fichaElegida;
        }

        protected override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {
            Random random = new Random();
            if(!bocaArriba)
            {
                while()
                Fichas.Add(fichas[random.Next(fichas.Count)]);
            }
            else
            {
                //TENER LA MAYOR CANTIDAD POSIBLE DE CADA NUMERO EN Fichas

            }
        }
        /////METODO DE SELECCIONAR DEL JUGADOR INTELIGENTE INCOMPLETO
        private List<Tuple<int, int>> EscogerFichas(int cantFichas, List<Tuple<int, int>> fichas, List<Tuple<int, int>> escogidas, int length, int indice)
        {
            if (indice > cantFichas) return escogidas;
            for (int i = indice; i < length; i++)//Escojo una ficha
            {
                Tuple<int, int> ficha = fichas[i];
                escogidas.Add(ficha);//La agrego
                for (int j = 0; j < escogidas.Count; j++) 
                {//Verifico si 
                    if(ficha.Item1 == escogidas[j].Item1 || ficha.Item2 == escogidas[j].Item1 || ficha.Item1 == escogidas[j].Item2 || ficha.Item2 == escogidas[j].Item2)
                    {
                        if (ficha.Equals(escogidas[j])) continue;

                        break;
                    }
                    if (j == escogidas.Count - 1) return EscogerFichas(cantFichas, fichas, escogidas, length, i + 1);
                }
                escogidas.Remove(ficha);
            } 
        }
        private bool EsDoble(Tuple<int,int> ficha)
        {
            return ficha.Item1 == ficha.Item2;
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
            if(ficha.Item1==-1&& ficha.Item2 == -1)
            {
                return jugador.Nombre + " se ha pasado";
            }
            return jugador.Nombre + " ha jugado [" + ficha.Item1 + "|" + ficha.Item2 + "]";
        }
        public string Ganador(Jugador ganador)
        {
            return "El ganador es " + ganador.Nombre;
        }
    }
        /////////////////////////////////////////////////
        ///Cosas DEl FOrm 3

        public interface IFicha
        {
            List<Tuple<int, int>> GeneradorDeFichas();
        }

        public class FichasDe6 : IFicha
        {
            public List<Tuple<int, int>> GeneradorDeFichas()
            {
                List<Tuple<int, int>> listaDeFichas = new List<Tuple<int, int>>();
                for (int i = 0; i <= 6; i++)
                {
                    for (int j = i; j <= 6; j++)//tenias j = 0, puse j = i pa q no se creen duplicados de las fichas q se han creado hasta ahora
                    {
                        listaDeFichas.Add(new Tuple<int, int>(i, j));
                    }
                }
                return listaDeFichas;
            }
        }



        public interface ICondicionDeFinalizacion
        {
        //bool Finalizo(List<Jugador> jugadores);
        bool Finalizo(Jugador jugador);

    }
        public class FinalizacionPorPuntos : ICondicionDeFinalizacion
        {
        //Aqui quite puntuacion para asignarselo directamente,ya q no tengo modo en q me entren una puntuacion
        //public bool Finalizo(List<Jugador> jugadores)
        //{
        //    foreach (var item in jugadores)
        //    {
        //        if (item.Puntuacion >= 50) { return true; }
        //    }
        //    return false;
        //}
        public bool Finalizo(Jugador jugador)
        {
           
           if (jugador.Puntuacion >= 50) { return true; }
       
            return false;
        }

        }

    // public class FinalizacionPorPase : ICondicionDeFinalizacion { }


    public interface IOrdenDeLasJugadas
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

    public interface IFormadeRepartir
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
    ///Form 4

    public class Juego : IEnumerator<Jugada>
    {
        List<Jugador> ListadeJugadores;
        ICondicionDeFinalizacion CondicionDeFinalizacion;
        IOrdenDeLasJugadas OrdenDeLasJugadas;
        IFormadeRepartir FormadeRepartir;
        IFicha ModoDeJuego;
        //IFormaDeCalcularPuntuacionFinal FormaDeCalcularPuntuacionFinal;

        bool PrimerTurno;
        bool HuboMoveNext;
        bool SePaso;
        int Extremo1;
        int Extremo2;
        List<Tuple<int, int>> fichas;



        public Juego(List<Jugador> ListadeJugadores, ICondicionDeFinalizacion CondicionDeFinalizacion, IOrdenDeLasJugadas OrdenDeLasJugadas, IFormadeRepartir FormadeRepartir, IFicha ModoDeJuego)
        {
            this.ListadeJugadores= ListadeJugadores;
            this.CondicionDeFinalizacion= CondicionDeFinalizacion;
            this.OrdenDeLasJugadas= OrdenDeLasJugadas;
            this.FormadeRepartir= FormadeRepartir;
            this.ModoDeJuego= ModoDeJuego;
            //
            PrimerTurno= true;
            HuboMoveNext = false;
            SePaso = false;
            Extremo1 = -1;
            Extremo2 = -1;
            fichas = ModoDeJuego.GeneradorDeFichas();

        }

        public Jugada Current => throw new NotImplementedException();

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            if (PrimerTurno)
            {
              // List<Tuple<int,int>>fichas= ModoDeJuego.GeneradorDeFichas();
               FormadeRepartir.Repartir(fichas,ListadeJugadores);
               PrimerTurno = false;
            }
            Jugador actual = OrdenDeLasJugadas.Orden(ListadeJugadores, SePaso);
            SePaso = false;//reseteando el SePaso
            Tuple<int,int>fichaJugada= actual.Juega(fichas, Extremo1, Extremo2);
            Jugada jugadaActual = new Jugada(actual, fichaJugada);
            ReconocerExtremos(fichaJugada);
            fichas.Add(fichaJugada);
            if (CondicionDeFinalizacion.Finalizo(actual))
            {

            }




        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public void ReconocerExtremos(Tuple<int, int> fichaJugada)
        {
            if (fichaJugada.Item1 == -1 && fichaJugada.Item2 == -1)//Reconociendo si se paso
            {
                SePaso = true;
                return;
            }


            if (fichaJugada.Item1 == Extremo1)
            {
                Extremo1 = fichaJugada.Item2;
            }
            else if(fichaJugada.Item1 == Extremo2)
            {
                Extremo2 = fichaJugada.Item2;
            }
            else if (fichaJugada.Item2 == Extremo1)
            {
                Extremo1= fichaJugada.Item1;
            }
            else { Extremo2= fichaJugada.Item1; }
        }



    }




}
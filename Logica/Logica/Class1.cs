﻿using System.Collections;
using System.Reflection;

namespace Logica
{
    public abstract class Jugador
    {
        public string Nombre;

        public double Puntuacion;

        public bool EsTurno;

        public bool Ganador;

        public List<Tuple<int, int>> Fichas;

        public Func<Tuple<int, int>,int> FormaDeCalcularPuntuacionDeLasFichas;

        public abstract Tuple<int,int> Juega(List<Tuple<int, int>> fichas, int num1, int num2);//Los numeros disponibles en el tablero
        public abstract void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas);

        protected abstract bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2);//Pa cuando la hagas interface en vez de abstracta.
        //{
        //    return fichas[i].Item1 == num1 || fichas[i].Item2 == num1 || fichas[i].Item1 == num2 || fichas[i].Item2 == num2;
        //}

    }
    public class JugadorAleatorio : Jugador
    {

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public override Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;
            Random random = new Random();
            int a = random.Next(length);
            Tuple<int, int> ficha = new Tuple<int, int>(-1, -1);
            if (num1 == -1 && num2 == -1)//Para si es el primer turno jugar una ficha random
            {
                ficha = Fichas[a];    
                Fichas.Remove(ficha);
                return ficha;
            }

            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(Fichas[i], num1, num2)) break;
                else if (i == length - 1) return new Tuple<int, int>(-1, -1);//Si no puede jugar ninguna ficha, retorna (-1,-1)
            }
            do
            {
                ficha = Fichas[random.Next(length)];
            } while (!EsFichaJugable(ficha, num1, num2));
            Fichas.Remove(ficha);
            return ficha;
        }

        public override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<Tuple<int, int>>();
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

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public  override Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;// Yes nigga :) , esto es una pequeñisima optimizacion
            int mayorValor = 0;
            Tuple<int,int> fichaDeMayorValor = new Tuple<int, int>(-1,-1);

            if (num1 == -1 && num2 == -1)//Por si es el primer turno,que juegue la ficha mas grande
            {
                for (int i = 0; i < length; i++)
                {
                    int valor = Fichas[i].Item1 + Fichas[i].Item2;
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
                Fichas.Remove(fichaDeMayorValor);
                return fichaDeMayorValor;
            }


            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(Fichas[i], num1, num2))
                {
                    int valor = Fichas[i].Item1 + Fichas[i].Item2;
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
            }
            Fichas.Remove(fichaDeMayorValor);
            return fichaDeMayorValor;
        }

        public override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<Tuple<int, int>>();
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
        //public JugadorInteligente(string Nombre) : base(Nombre) 
        //{ 
        //    Fichas = new List<Tuple<int, int>>();
        //}

        protected override bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
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
            Fichas.Remove(fichaElegida);
            //Si nunca encontro ficha jugable entonces retorna (-1,-1)
            return fichaElegida;
        }

        public override void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<Tuple<int, int>>();
            Random random = new Random();
            if(!bocaArriba)
            {
                while (this.Fichas.Count < cantFichas)
                {
                    int a = random.Next(fichas.Count);
                    Fichas.Add(fichas[a]);
                    fichas.RemoveAt(a);
                }
            }
            else
            {
                //TENER LA MAYOR CANTIDAD POSIBLE DE CADA NUMERO EN Fichas
                List<Tuple<int, int>> escogidas = new List<Tuple<int, int>>();

                escogidas = new List<Tuple<int, int>>();
                escogidas = EscogerFichas(cantFichas, fichas, escogidas, 0);
                this.Fichas.AddRange(escogidas);

                if (this.Fichas.Count > cantFichas)
                {
                    int resto = Fichas.Count - cantFichas;
                    Fichas.RemoveRange(Fichas.Count - resto, resto);
                }
                //Por si no se eliminaron las fichas escogidas del monto inicial al ejecutar EscogerFichas()
                foreach (Tuple<int, int> ficha in fichas)
                {
                    if (Fichas.Contains(ficha)) fichas.Remove(ficha);
                }
                if(this.Fichas.Count < cantFichas)
                {
                    while(this.Fichas.Count < cantFichas)
                    {
                        int a = random.Next(fichas.Count);
                        Fichas.Add(fichas[a]);
                        fichas.RemoveAt(a);
                    }
                }
            }
        }
        /////METODO DE SELECCIONAR DEL JUGADOR INTELIGENTE INCOMPLETO
        private List<Tuple<int, int>> EscogerFichas(int cantFichas, List<Tuple<int, int>> fichas, List<Tuple<int, int>> escogidas, int indice)
        {
            if (indice > cantFichas) return escogidas;
            List<Tuple<int, int>> opcion1 = new List<Tuple<int, int>>();
            List<Tuple<int, int>> opcion2 = new List<Tuple<int, int>>();

            Tuple<int, int> ficha = fichas[indice];
            escogidas.Add(ficha);//La agrego
            fichas.Remove(ficha);
            for (int j = 0; j < escogidas.Count; j++)
            {//Verifico si sus numeros los tengo ya en otras fichas q escogi antes
             //si es asi, la desestimo
                if (ficha.Item1 == escogidas[j].Item1 || ficha.Item2 == escogidas[j].Item1 || ficha.Item1 == escogidas[j].Item2 || ficha.Item2 == escogidas[j].Item2)
                {
                    if (ficha.Equals(escogidas[j])) continue;//Si es la misma no la tengo en cuenta

                    break;
                }
                if (j == escogidas.Count - 1)
                {
                    opcion1 = EscogerFichas(cantFichas, fichas, escogidas, indice + 1);
                }
            }
            escogidas.Remove(ficha);
            fichas.Add(ficha);
            opcion2 = EscogerFichas(cantFichas, fichas, escogidas, indice + 1);

            return escogidas.Count > opcion1.Count ? escogidas : opcion1;
        }
        private bool EsDoble(Tuple<int,int> ficha)
        {
            return ficha.Item1 == ficha.Item2;
        }
    }
    //public class TiposDeJugador///////////////////Assembly
    //{

    //    public List<string> NombreDeLosTipos { get { return nombreDeLosTipos; } }
    //    public List<string> nombreDeLosTipos = new List<string> { "Jugador Aleatorio", "Jugador Goloso" };//
    //    //List<Jugador> tiposdejugadores = new List<Jugador> { new JugadorAleatorio(""), new JugadorGoloso("") };

    //    public void Add(string tipo)
    //    {
    //        nombreDeLosTipos.Add(tipo);
    //    }
    //    public Jugador Comparer(string nombre)
    //    {
    //        switch (nombre)
    //        {
    //            case "Jugador Aleatorio":
    //                return new JugadorAleatorio("");
    //            case "Jugador Goloso":
    //                return new JugadorGoloso("");
    //            default:
    //                throw new Exception("paso algo en el Comparer");
    //        }
    //    }

    //}

    public class Jugada
    {
        Jugador jugador;
        Tuple<int, int> ficha;
        bool termino;
        bool trancado;
        Jugador ganador;

        public Jugada(Jugador jugador, Tuple<int, int> ficha,bool termino,Jugador ganador,bool trancado)
        {
            this.jugador = jugador;
            this.ficha = ficha;
            this.termino= termino;
            this.ganador = ganador;
            this.trancado= trancado;
        }

        public override string ToString()
        {
            string oracion;
            if (ficha.Item1 == -1 && ficha.Item2 == -1)
            {
                oracion = jugador.Nombre + " se ha pasado";
            }
            else { oracion = jugador.Nombre + " ha jugado [" + ficha.Item1 + "|" + ficha.Item2 + "]"; }
            if (termino)
            {
                if (trancado) { oracion += " Juego trancado,el ganador es " + ganador.Nombre; }
                else { oracion += "\n El ganador es " + ganador.Nombre; }
            }
            return oracion;
        }
    }
        /////////////////////////////////////////////////
        ///Cosas DEl FOrm 3

        public interface IFicha
        {
            int CantDeFichasParaCadaJugador { get; }
            List<Tuple<int, int>> GeneradorDeFichas();
        }

        public class FichasDe6 : IFicha
        {
            public int CantDeFichasParaCadaJugador { get { return 7; } }
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

        public class FichasDe9 : IFicha
        {
        public int CantDeFichasParaCadaJugador { get { return 10; } }
        public List<Tuple<int, int>> GeneradorDeFichas()
        {
            List<Tuple<int, int>> listaDeFichas = new List<Tuple<int, int>>();
            for (int i = 0; i <= 9; i++)
            {
                for (int j = i; j <= 9; j++)//tenias j = 0, puse j = i pa q no se creen duplicados de las fichas q se han creado hasta ahora
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
        bool Finalizo(Jugador jugador, Tuple<int, int> fichaJugada, List<Jugador> jugadores, int extremo1, int extremo2, out bool tabla);

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
        public bool Finalizo(Jugador jugador,Tuple<int, int>fichaJugada,List<Jugador>jugadores,int extremo1,int extremo2,out bool tabla)
        {

            if (jugador.Fichas.Count == 0)
            {
                jugador.Ganador = true;
                tabla = false;
                return true;
            }

           if (jugador.Puntuacion >= 50) { jugador.Ganador = true; tabla = false;  return true; }

            foreach (var item in jugadores)//Comprobando si se tranco el juego
            {
                foreach (var item2 in item.Fichas )
                {
                    if(item2.Item1==extremo1||item2.Item2==extremo1|| item2.Item1 == extremo2 || item2.Item2 == extremo2) { tabla = false; return false; }
                }
            }

            tabla = true;
            
                double mejorPuntuacion = 0;
                int indiceJugadorConMejorPuntaje = 0;
                foreach (var item in jugadores)
                {
                    if (mejorPuntuacion < item.Puntuacion) { indiceJugadorConMejorPuntaje = jugadores.IndexOf(item); mejorPuntuacion = item.Puntuacion; }
                }
                jugadores[indiceJugadorConMejorPuntaje].Ganador = true;
            

            return true;
        }

        }

    public class FinalizacionPorPase : ICondicionDeFinalizacion
    {
        int[] vecesPasadasSeguidas;
        bool primeraVez = true;
        public bool Finalizo(Jugador jugador,Tuple<int,int>fichaJugada, List<Jugador> jugadores, int extremo1, int extremo2, out bool tabla)
        {
            if (primeraVez) { vecesPasadasSeguidas = new int[jugadores.Count];primeraVez = false; }
            if (jugador.Fichas.Count == 0)
            {
                jugador.Ganador = true;
                tabla = false;
                return true;
            }
            if (fichaJugada.Item1 == -1 && fichaJugada.Item2 == -1)
            {
                int indice = jugadores.IndexOf(jugador);
                vecesPasadasSeguidas[indice]++;
                if (vecesPasadasSeguidas[indice] == 2) {
                    double MejorPuntuacion = 0;
                    int indiceDeJugadorConMejorPuntaje = 0;
                    foreach (var item in jugadores)
                    {
                        if (MejorPuntuacion < item.Puntuacion) { indiceDeJugadorConMejorPuntaje = jugadores.IndexOf(item); MejorPuntuacion = item.Puntuacion; }
                    }
                    jugadores[indiceDeJugadorConMejorPuntaje].Ganador = true;
                    tabla = false;
                    return true;
                }
            }
            else if (fichaJugada.Item1 != -1 || fichaJugada.Item2 != -1) { vecesPasadasSeguidas[jugadores.IndexOf(jugador)] = 0; }


            foreach (var item in jugadores)//Comprobando si se tranco el juego
            {
                foreach (var item2 in item.Fichas)
                {
                    if (item2.Item1 == extremo1 || item2.Item2 == extremo1 || item2.Item1 == extremo2 || item2.Item2 == extremo2) { tabla = false; return false; }
                }
            }

            tabla = true;

            double mejorPuntuacion = 0;
            int indiceJugadorConMejorPuntaje = 0;
            foreach (var item in jugadores)
            {
                if (mejorPuntuacion < item.Puntuacion) { indiceJugadorConMejorPuntaje = jugadores.IndexOf(item); mejorPuntuacion = item.Puntuacion; }
            }
            jugadores[indiceJugadorConMejorPuntaje].Ganador = true;

            return true;
        }
    }


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
                        jugadores[jugadores.Count - 1].EsTurno = false;
                            jugadores[0].EsTurno = true;
                            return jugadores[0];
                        }
                        else
                        {
                            jugadores[i].EsTurno=false;
                            jugadores[i+1].EsTurno = true;
                            return jugadores[i+1];
                        }
                    }
                }

                jugadores[0].EsTurno = true;//Para cuando sea el primer turno
                return jugadores[0];
            }
        }

    public class OrdenCambiadoSiSePasa : IOrdenDeLasJugadas
        {
        bool ordenCambiado = false;
            public Jugador Orden(List<Jugador> jugadores, bool SePaso)
            {
                for (int i = 0; i < jugadores.Count; i++)
                {
                    if (jugadores[i].EsTurno)
                    {
                    if (SePaso)
                    {
                        ordenCambiado = !ordenCambiado;
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
                            jugadores[jugadores.Count - 1].EsTurno= false;
                            jugadores[0].EsTurno = true;
                            return jugadores[0];
                        }
                        else
                        {
                            jugadores[i].EsTurno=false;
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
        void Repartir(List<Tuple<int, int>> fichas, List<Jugador> jugadores, int cantidadDeFichas);
    }
    public class RepartoAleatorio : IFormadeRepartir
    {
        public void Repartir(List<Tuple<int, int>> fichas,List<Jugador>jugadores,int cantidadDeFichas)
        {

            foreach (var item in jugadores)
            {
                item.Seleccionar(fichas, false, cantidadDeFichas);
            }
        }
    }
    public class RepartoBocaArriba : IFormadeRepartir
    {
        public void Repartir(List<Tuple<int, int>> fichas, List<Jugador> jugadores, int cantidadDeFichas)
        {
           foreach(var item in jugadores)
            {
                item.Seleccionar(fichas, true, cantidadDeFichas);
            }
        }
    }



    public interface IFormaDeCalcularPuntuacion
    {
        int CalcularPuntuacion( Tuple<int, int> ficha);
    }
    public class ContarFichasDoblesPor2 : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(Tuple<int,int>ficha)
        {
            if (ficha.Item1 == ficha.Item2) { return (ficha.Item1 + ficha.Item2) * 2; }
            else { return  ficha.Item1 + ficha.Item2; }
        }
    }
    public class PenalizarFichasDobles : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(Tuple<int, int> ficha)
        {
            if (ficha.Item1 == ficha.Item2) { return -(ficha.Item1 + ficha.Item2); }
            else { return -(ficha.Item1 + ficha.Item2); }
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
        IFormaDeCalcularPuntuacion FormaDeCalcularPuntuacion;

        bool PrimerTurno;
        bool HuboMoveNext;/////////////
        bool SePaso;
        int Extremo1;
        int Extremo2;
        List<Tuple<int, int>> fichas;
        Jugada jugadaActual;
        Jugador ganador;

        bool FinalizoElJuego;
        bool JuegoTrancado;



        public Juego(List<Jugador> ListadeJugadores, ICondicionDeFinalizacion CondicionDeFinalizacion, IOrdenDeLasJugadas OrdenDeLasJugadas, IFormadeRepartir FormadeRepartir, IFicha ModoDeJuego, IFormaDeCalcularPuntuacion FormaDeCalcularPuntuacion)
        {

            this.ListadeJugadores= ListadeJugadores;
            this.CondicionDeFinalizacion= CondicionDeFinalizacion;
            this.OrdenDeLasJugadas= OrdenDeLasJugadas;
            this.FormadeRepartir= FormadeRepartir;
            this.ModoDeJuego= ModoDeJuego;
            this.FormaDeCalcularPuntuacion = FormaDeCalcularPuntuacion;
            //
            PrimerTurno = true;
            HuboMoveNext = false;////////////
            SePaso = false;
            Extremo1 = -1;
            Extremo2 = -1;
            fichas = ModoDeJuego.GeneradorDeFichas();
            FinalizoElJuego = false;
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
            if (!FinalizoElJuego)
            {
                if (PrimerTurno)
                {
                    // List<Tuple<int,int>>fichas= ModoDeJuego.GeneradorDeFichas();
                    foreach (var item in ListadeJugadores)//////
                    {
                        item.FormaDeCalcularPuntuacionDeLasFichas = FormaDeCalcularPuntuacion.CalcularPuntuacion;
                    }
                    FormadeRepartir.Repartir(fichas, ListadeJugadores,ModoDeJuego.CantDeFichasParaCadaJugador);
                   
                }
                Jugador actual = OrdenDeLasJugadas.Orden(ListadeJugadores, SePaso);
                SePaso = false;//reseteando el SePaso
                Tuple<int, int> fichaJugada = actual.Juega(fichas, Extremo1, Extremo2);
                //aqui iba lo de jugada actual

                actual.Puntuacion+= FormaDeCalcularPuntuacion.CalcularPuntuacion(fichaJugada);

                ReconocerExtremos(fichaJugada);
                fichas.Add(fichaJugada);
                FinalizoElJuego = CondicionDeFinalizacion.Finalizo(actual,fichaJugada,ListadeJugadores,Extremo1,Extremo2,out JuegoTrancado);
                if (FinalizoElJuego)//Si termino el juego,buscar al ganador
                {
                    
                    foreach (var item in ListadeJugadores)
                    {
                        if (item.Ganador) { ganador = item; }
                    }
                }


                jugadaActual = new Jugada(actual, fichaJugada, FinalizoElJuego,ganador, JuegoTrancado);//

                PrimerTurno = false;

                return true;
            }
            return false;

        }

        public void Reset()
        {
            PrimerTurno = true;
            SePaso = false;
            Extremo1 = -1;
            Extremo2 = -1;
            fichas = ModoDeJuego.GeneradorDeFichas();
            FinalizoElJuego = false;
            foreach (var jugador in ListadeJugadores)
            {
                jugador.EsTurno = false;
                jugador.Puntuacion = 0;
            }
        }

        public void ReconocerExtremos(Tuple<int, int> fichaJugada)
        {
            if (fichaJugada.Item1 == -1 && fichaJugada.Item2 == -1)//Reconociendo si se paso
            {
                SePaso = true;
                return;
            }
            if (PrimerTurno)
            {
                Extremo1 = fichaJugada.Item1;
                Extremo2=fichaJugada.Item2;
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
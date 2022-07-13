using System.Collections;
using System.Reflection;

namespace Logica
{
    public interface IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<Tuple<int, int>> Fichas { get; set; }//Las fichas que tiene cada Jugador

        public Func<Tuple<int, int>,int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }//Para saber como jugar en dependencia de como se esten calculando los puntajes

        public  Tuple<int,int> Juega(List<Tuple<int, int>> fichas, int num1, int num2);//num1 y num2 son los numeros disponibles por donde jugar en el tablero
        public  void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas);//Para poder seleccionar las fichas segun el comportamiento que uno le de al Jugador

        protected  bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2);//Para saber si la ficha se puede jugar

}
    public class JugadorAleatorio : IJugador
    {

        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<Tuple<int, int>> Fichas { get; set; }

        public Func<Tuple<int, int>, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }

        public  bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
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

        public void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
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
    public class JugadorGoloso : IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<Tuple<int, int>> Fichas { get; set; }

        public Func<Tuple<int, int>, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }
        public bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;
            int mayorValor = int.MinValue;
            Tuple<int,int> fichaDeMayorValor = new Tuple<int, int>(-1,-1);

            if (num1 == -1 && num2 == -1)//Por si es el primer turno, que juegue la ficha mas grande
            {
                for (int i = 0; i < length; i++)
                {
                    int valor = FormaDeCalcularPuntuacionDeLasFichas(Fichas[i]);
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
                Fichas.Remove(fichaDeMayorValor);
                return fichaDeMayorValor;
            }


            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(Fichas[i], num1, num2))
                {
                    int valor = FormaDeCalcularPuntuacionDeLasFichas(Fichas[i]);
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
            }
            Fichas.Remove(fichaDeMayorValor);
            return fichaDeMayorValor;
        }

        public void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
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
                Tuple<int, int> fichaDeMayorValor = fichas[0];
                int mayorValor = FormaDeCalcularPuntuacionDeLasFichas(fichaDeMayorValor);
                while (Fichas.Count < cantFichas)
                {
                    length = fichas.Count;
                    for(int i = 0; i < length; i++)
                    {
                        if (fichaDeMayorValor == fichas[i]) continue;
                        valor = FormaDeCalcularPuntuacionDeLasFichas(fichas[i]);
                        if(valor > mayorValor)
                        {
                            fichaDeMayorValor = fichas[i];
                            mayorValor = valor;
                        }
                        if(i == length - 1)
                        {
                            Fichas.Add(fichaDeMayorValor);
                            fichas.Remove(fichaDeMayorValor);
                            fichaDeMayorValor = fichas[0];
                            mayorValor = FormaDeCalcularPuntuacionDeLasFichas(fichaDeMayorValor);
                        }
                    }
                }
            }
        }
    }
    public class JugadorInteligente : IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<Tuple<int, int>> Fichas { get; set; }

        public Func<Tuple<int, int>, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }

        public bool EsFichaJugable(Tuple<int, int> ficha, int num1, int num2)
        {
            return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public  Tuple<int, int> Juega(List<Tuple<int, int>> fichas, int num1, int num2)
        {
            int length = Fichas.Count;
            int potencialMaximo = 0;
            Tuple<int, int> fichaElegida = new Tuple<int, int>(-1, -1);
            if (num1 == -1 && num2 == -1)
            {
                foreach (Tuple<int, int> ficha in Fichas)//Reviso por las fichas q tengo
                {
                    int potencial = 1;
                    int numero = ficha.Item2;

                    foreach (Tuple<int, int> ficha2 in Fichas)
                    {//Reviso cuales fichas de las q tengo tienen el otro numero
                     //eso me dira que tan buena es la ficha
                        if (ficha2.Equals(ficha)) continue;
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
                    //Repito el proceso con el otro numero de la ficha 

                    numero = ficha.Item1;
                    foreach (Tuple<int, int> ficha2 in Fichas)
                    {
                        if (ficha2.Equals(ficha)) continue;
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
                Fichas.Remove(fichaElegida);
                return fichaElegida;
            }
            //La idea de esto es escojer la ficha q pueda jugar por un numero y q su otro numero
            //sea lo mas comun posible entre el resto de las fichas q tengo. 
           
            foreach (Tuple<int, int> ficha in Fichas)//Reviso por las fichas q tengo
            {
                int potencial = 1;
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
                            if (ficha2.Equals(ficha) && Fichas.Count != 1) continue;
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
                            if (ficha2.Equals(ficha) && Fichas.Count != 1) continue;
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

        public void Seleccionar(List<Tuple<int, int>> fichas, bool bocaArriba, int cantFichas)
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

                do
                {
                    escogidas = EscogerFichas(cantFichas, fichas, NumMaximoEnFichas(fichas));
                    if (escogidas.Count != 0)
                    {
                        this.Fichas.AddRange(escogidas);
                        for (int i = 0; i < escogidas.Count; i++)
                        {
                            fichas.Remove(escogidas[i]);
                        }
                    }
                } while (escogidas.Count != 0 && this.Fichas.Count < cantFichas);

                if (this.Fichas.Count > cantFichas)
                {
                    int resto = Fichas.Count - cantFichas;
                    for (int i = this.Fichas.Count - 1, j = resto; i >= 0 && j > 0; i--, j--) 
                    {
                        fichas.Add(Fichas[i]);
                        Fichas.RemoveAt(i);
                    }
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
        private static int NumMaximoEnFichas(List<Tuple<int, int>> fichas)
        {
            int numero = int.MinValue;
            foreach (var ficha in fichas)
            {
                if(ficha.Item1 > numero)numero = ficha.Item1;
                if(ficha.Item2 > numero)numero = ficha.Item2;
            }
            return numero + 1;
        }
        /////LA IDEA ES QUE ESCOJA LAS FICHAS DE MANERA Q POSEA LA MAYOR CANTIDAD DE NUMEROS POSIBLES
        private List<Tuple<int,int>> EscogerFichas(int cantFichas, List<Tuple<int, int>> fichas, int numMax)
        {
            List<Tuple<int,int>> escogidas = new List<Tuple<int,int>>();
            List<Tuple<int, int>> fichasSinDobles = new List<Tuple<int, int>>();
            List<Tuple<int, int>> fichasDobles = new List<Tuple<int, int>>();

            foreach (var ficha in fichas)
            {
                if(ficha.Item1 != ficha.Item2)
                {
                    fichasSinDobles.Add(ficha);
                }
                if(ficha.Item2 == ficha.Item1)
                {
                    fichasDobles.Add(ficha);
                }
            }
            escogidas.Add(fichasSinDobles[0]);//////////////AQUI DA EL ERROR
            escogidas = FichasElegidas(fichasSinDobles, escogidas);
            return FichasElegidas(fichasDobles, escogidas);
        }
        private List<Tuple<int,int>> FichasElegidas(List<Tuple<int,int>> fichas, List<Tuple<int, int>> escogidas)
        {
            foreach(var ficha in fichas)
            {
                for (int j = 0; j < escogidas.Count; j++)
                {
                    if (EsFichaJugable(ficha, escogidas[j].Item1, escogidas[j].Item2))//Compruebo si alguno de los numeros de ficha ya lo tengo dentro de las q escogi
                    {
                        break;
                    }
                    if (j == escogidas.Count - 1 && !ficha.Equals(escogidas[j]))
                    {
                        escogidas.Add(ficha);
                    }
                }
            }
            return escogidas;
        }
    }

    public class Jugada//Para imprimir cada jugada 
    {
        IJugador jugador;//jugador que le tocaba el turno
        Tuple<int, int> ficha;
        bool terminó;//Si es true, es porq terminó el juego, osea que un jugador jugo todas sus fichas
        bool trancado;//Si es true, es porq terminó el juego, osea que ningun jugador tiene ficha jugable disponible
        IJugador ganador;

        public Jugada(IJugador jugador, Tuple<int, int> ficha,bool terminó,IJugador ganador,bool trancado)
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
            if (ficha.Item1 == -1 && ficha.Item2 == -1)
            {
                oracion = jugador.Nombre + " se ha pasado." + " Se mantiene con: " + jugador.Puntuacion + " puntos."; 
            }
            else { oracion = jugador.Nombre + " ha jugado [" + ficha.Item1 + "|" + ficha.Item2 + "]. Tiene: " + jugador.Puntuacion + " puntos."; }
            if (terminó)
            {
                if (trancado) { oracion += " Juego trancado,el ganador es " + ganador.Nombre + " con: " + ganador.Puntuacion + " puntos."; }
                else { oracion += " El ganador es " + ganador.Nombre + " con: " + ganador.Puntuacion + " puntos."; }
            }
            return oracion ;
        }
    }
    


    public interface IFicha
    {
        int FichasPorJugador { get; }
        List<Tuple<int, int>> GeneradorDeFichas();//Para generar las fichas segun el tipo de juego que hayan escogido
    }
    public class FichasDe6 : IFicha
    {
        public int FichasPorJugador { get { return 7; } }
        public List<Tuple<int, int>> GeneradorDeFichas()
        {
            List<Tuple<int, int>> listaDeFichas = new List<Tuple<int, int>>();
            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    listaDeFichas.Add(new Tuple<int, int>(i, j));
                }
            }
            return listaDeFichas;
        }
    }
    public class FichasDe9 : IFicha
    {
        public int FichasPorJugador { get { return 10; } }
        public List<Tuple<int, int>> GeneradorDeFichas()
        {
            List<Tuple<int, int>> listaDeFichas = new List<Tuple<int, int>>();
            for (int i = 0; i <= 9; i++)
            {
                for (int j = i; j <= 9; j++)
                {
                    listaDeFichas.Add(new Tuple<int, int>(i, j));
                }
            }
            return listaDeFichas;
        }
    }
    public interface ICondicionDeFinalizacion
    {
        bool Finalizo(IJugador jugador, Tuple<int, int> fichaJugada, List<IJugador> jugadores, int extremo1, int extremo2, out bool tabla);

    }
    public class FinalizacionPorPuntos_50puntos : ICondicionDeFinalizacion
    {
        public bool Finalizo(IJugador jugador, Tuple<int, int> fichaJugada, List<IJugador> jugadores, int extremo1, int extremo2, out bool tabla)
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
                    if (ficha.Item1 == extremo1 || ficha.Item2 == extremo1 || ficha.Item1 == extremo2 || ficha.Item2 == extremo2) { tabla = false; return false; }
                }
            }

            tabla = true;

            double mejorPuntuacion = 0;
            int indiceJugadorConMejorPuntaje = 0;
            foreach (var item in jugadores)//como se tranco el juego se saca el jugador con mayor puntaje
            {
                if (mejorPuntuacion < item.Puntuacion) 
                { 
                    indiceJugadorConMejorPuntaje = jugadores.IndexOf(item); 
                    mejorPuntuacion = item.Puntuacion;
                }
            }
            jugadores[indiceJugadorConMejorPuntaje].Ganador = true;


            return true;
        }

    }
    public class FinalizacionPorPase : ICondicionDeFinalizacion
    {
        int[] vecesPasadasSeguidas;
        bool primeraVez = true;
        public bool Finalizo(IJugador jugador,Tuple<int,int>fichaJugada, List<IJugador> jugadores, int extremo1, int extremo2, out bool tabla)
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
        IJugador Siguiente(List<IJugador> jugadores, bool SePaso);
        public void Reset();//Para cuando se haga un juego nuevo se vuelva a empezar desde el jugador 1
    }
    public class OrdenNormal : IOrdenDeLasJugadas
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
    public class OrdenCambiadoSiSePasa : IOrdenDeLasJugadas
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
        void Repartir(List<Tuple<int, int>> fichas, List<IJugador> jugadores, int cantidadDeFichas);
    }
    public class RepartoAleatorio : IFormadeRepartir
    {
        public void Repartir(List<Tuple<int, int>> fichas,List<IJugador> jugadores,int cantidadDeFichas)
        {

            foreach (var item in jugadores)
            {
                item.Seleccionar(fichas, false, cantidadDeFichas);
            }
        }
    }
    public class RepartoBocaArriba : IFormadeRepartir
    {
        public void Repartir(List<Tuple<int, int>> fichas, List<IJugador> jugadores, int cantidadDeFichas)
        {
           foreach(var item in jugadores)
            {
                item.Seleccionar(fichas, true, cantidadDeFichas);
            }
        }
    }
    public interface IFormaDeCalcularPuntuacion
    {
        int CalcularPuntuacion(Tuple<int, int> ficha);
    }
    public class ContarFichasDoblesPor2 : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(Tuple<int,int>ficha)
        {
            if(ficha.Item1 == -1 || ficha.Item2 == -1)return 0;//si se paso que no le sume puntuacion
            if (ficha.Item1 == ficha.Item2) { return (ficha.Item1 + ficha.Item2) * 2; }
            else { return  ficha.Item1 + ficha.Item2; }
        }
    }
    public class PenalizarFichasDobles : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(Tuple<int, int> ficha)
        {
            if (ficha.Item1 == -1 || ficha.Item2 == -1) return 0;//si se paso que no le sume puntuacion
            if (ficha.Item1 == ficha.Item2) { return -(ficha.Item1 + ficha.Item2); }
            else { return ficha.Item1 + ficha.Item2; }
        }
    }
    public class PuntuacionNormal : IFormaDeCalcularPuntuacion
    {
        public int CalcularPuntuacion(Tuple<int, int> ficha)
        {
            if (ficha.Item1 == -1 || ficha.Item2 == -1) return 0;//si se paso que no le sume puntuacion
            return ficha.Item1 + ficha.Item2; 
        }
    }


    public class Juego : IEnumerator<Jugada>
    {
        List<IJugador> ListadeJugadores;
        ICondicionDeFinalizacion CondicionDeFinalizacion;
        IOrdenDeLasJugadas OrdenDeLasJugadas;
        IFormadeRepartir FormadeRepartir;
        IFicha ModoDeJuego;
        IFormaDeCalcularPuntuacion FormaDeCalcularPuntuacion;
        IJugador ganador;

        bool PrimerTurno;
        bool SePaso;
        bool FinalizoElJuego;
        bool JuegoTrancado;
        int Extremo1;
        int Extremo2;
        List<Tuple<int, int>> fichas;//Fichas que repartir
        Jugada jugadaActual;
        
        public Juego(List<IJugador> ListadeJugadores, ICondicionDeFinalizacion CondicionDeFinalizacion, IOrdenDeLasJugadas OrdenDeLasJugadas, IFormadeRepartir FormadeRepartir, IFicha ModoDeJuego, IFormaDeCalcularPuntuacion FormaDeCalcularPuntuacion)
        {

            this.ListadeJugadores= ListadeJugadores;
            this.CondicionDeFinalizacion= CondicionDeFinalizacion;
            this.OrdenDeLasJugadas= OrdenDeLasJugadas;
            this.FormadeRepartir= FormadeRepartir;
            this.ModoDeJuego= ModoDeJuego;
            this.FormaDeCalcularPuntuacion = FormaDeCalcularPuntuacion;
            
            PrimerTurno = true;
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
                    foreach (var item in ListadeJugadores)//intruyendo a los jugadores de la manera de calcular las puntuaciones
                    {
                        item.FormaDeCalcularPuntuacionDeLasFichas = FormaDeCalcularPuntuacion.CalcularPuntuacion;
                    }
                    FormadeRepartir.Repartir(fichas, ListadeJugadores,ModoDeJuego.FichasPorJugador);//Repartiendo y haciendo que los jugadores escojan sus fichas
                   
                }
                IJugador jugadorActual = OrdenDeLasJugadas.Siguiente(ListadeJugadores, SePaso);
                SePaso = false;//reseteando el SePaso
                Tuple<int, int> fichaJugada = jugadorActual.Juega(fichas, Extremo1, Extremo2);
                jugadorActual.Puntuacion+= FormaDeCalcularPuntuacion.CalcularPuntuacion(fichaJugada);
                ReconocerExtremos(fichaJugada);
                if (fichaJugada.Item1 != -1 && fichaJugada.Item2 != -1)
                {
                    fichas.Add(fichaJugada);//Recogiendo la ficha q se jugo
                }
                FinalizoElJuego = CondicionDeFinalizacion.Finalizo(jugadorActual,fichaJugada,ListadeJugadores,Extremo1,Extremo2,out JuegoTrancado);
                if (FinalizoElJuego)//Si termino el juego,buscar al ganador
                {    
                    foreach (var item in ListadeJugadores)
                    {
                        if (item.Ganador) { ganador = item; }
                    }
                }

                jugadaActual = new Jugada(jugadorActual, fichaJugada, FinalizoElJuego,ganador, JuegoTrancado);//

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
            JuegoTrancado = false;
            OrdenDeLasJugadas.Reset();

            foreach (var jugador in ListadeJugadores)
            {
                jugador.Fichas = new List<Tuple<int, int>>();
                jugador.Ganador = false;
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
                Extremo2 = fichaJugada.Item2;
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
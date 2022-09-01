using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logica
{
    public interface IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<IFicha> Fichas { get; set; }//Las fichas que tiene cada Jugador

        public Func<IFicha, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }//Para saber como jugar en dependencia de como se esten calculando los puntajes

        public IFicha Juega(List<IFicha> fichas, object extremo1, object extremo2);//extremo1 y extremo2 son los "numeros" disponibles por donde jugar en el tablero
        public void Seleccionar(List<IFicha> fichas, bool bocaArriba, int cantFichas);//Para poder seleccionar las fichas segun el comportamiento que uno le de al Jugador

        protected bool EsFichaJugable(IFicha ficha, object extremo1, object extremo2);//Para saber si la ficha se puede jugar

    }
    public class JugadorAleatorio : IJugador
    {

        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<IFicha> Fichas { get; set; }

        public Func<IFicha, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }

        public bool EsFichaJugable(IFicha ficha, object extremo1, object extremo2)
        {
            if (ficha.contenido.Item1.Equals(extremo1) || ficha.contenido.Item1.Equals(extremo2) || ficha.contenido.Item2.Equals(extremo1) || ficha.contenido.Item2.Equals(extremo2))
            {
                return true;
            }
            return false;
            //return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public IFicha Juega(List<IFicha> fichas, object extremo1, object extremo2)
        {
            int length = Fichas.Count;
            Random random = new Random();
            int a = random.Next(length);
            IFicha ficha = null;
            if (extremo1 == null && extremo2 == null)//Para si es el primer turno jugar una ficha random
            {
                ficha = Fichas[a];
                Fichas.Remove(ficha);
                return ficha;
            }

            for (int i = 0; i < length; i++)
            {
                if (EsFichaJugable(Fichas[i], extremo1, extremo2)) break;
                else if (i == length - 1) return null;   //Si no puede jugar ninguna ficha, retorna (-1,-1)
            }
            do
            {
                ficha = Fichas[random.Next(length)];
            } while (!EsFichaJugable(ficha, extremo1, extremo2));
            Fichas.Remove(ficha);
            return ficha;
        }

        public void Seleccionar(List<IFicha> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<IFicha>();
            Random selecciona = new Random();
            while (Fichas.Count < cantFichas)
            {
                int a = selecciona.Next(fichas.Count);
                Fichas.Add(fichas[a]);
                fichas.Remove(fichas[a]);
            }
        }
    }
    public class Goloso : IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<IFicha> Fichas { get; set; }

        public Func<IFicha, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }
        public bool EsFichaJugable(IFicha ficha, object extremo1, object extremo2)
        {
            if (ficha.contenido.Item1.Equals(extremo1) || ficha.contenido.Item1.Equals(extremo2) || ficha.contenido.Item2.Equals(extremo1) || ficha.contenido.Item2.Equals(extremo2))
            {
                return true;
            }
            return false;
            //return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public IFicha Juega(List<IFicha> fichas, object extremo1, object extremo2)
        {
            int length = Fichas.Count;
            int mayorValor = int.MinValue;
            IFicha fichaDeMayorValor = null;

            if (extremo1 == null && extremo2 == null)//Por si es el primer turno, que juegue la ficha mas grande
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
            {//La idea es jugar la ficha de mayor valor
                if (EsFichaJugable(Fichas[i], extremo1, extremo2))
                {
                    int valor = FormaDeCalcularPuntuacionDeLasFichas(Fichas[i]);
                    if (valor > mayorValor) { mayorValor = valor; fichaDeMayorValor = Fichas[i]; }
                }
            }
            Fichas.Remove(fichaDeMayorValor);
            return fichaDeMayorValor;
        }

        public void Seleccionar(List<IFicha> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<IFicha>();
            Random random = new Random();
            if (!bocaArriba)//Si estan boca abajo las escoge al azar, se supone que no puede ver los numeros de las fichas
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
                IFicha fichaDeMayorValor = fichas[0];
                int mayorValor = FormaDeCalcularPuntuacionDeLasFichas(fichaDeMayorValor);
                while (Fichas.Count < cantFichas)
                {
                    if (fichas.Count == 1)//Si solo queda una ficha, la agrego
                    {
                        Fichas.Add(fichas[0]);
                        fichas.RemoveAt(0);
                    }
                    length = fichas.Count;
                    for (int i = 0; i < length; i++)//Busco la ficha de mayor valor pues al jugarla obtendra mas puntos
                    {
                        if (fichaDeMayorValor == fichas[i]) continue;//Si es la misma con la que estoy comparando no hago nada
                        valor = FormaDeCalcularPuntuacionDeLasFichas(fichas[i]);//Calculo su valor
                        if (valor > mayorValor)
                        {
                            fichaDeMayorValor = fichas[i];
                            mayorValor = valor;
                        }
                        if (i == length - 1)
                        {
                            Fichas.Add(fichaDeMayorValor);//Agrego la de mayor valor a Fichas
                            fichas.Remove(fichaDeMayorValor);//La quito de las fichas del juego pues la cogio el jugador, pa q no se repitan fichas
                            fichaDeMayorValor = fichas[0];
                            mayorValor = FormaDeCalcularPuntuacionDeLasFichas(fichaDeMayorValor);
                        }

                    }
                }
            }
        }
    }
    public class Inteligente : IJugador
    {
        public string Nombre { get; set; }

        public double Puntuacion { get; set; }

        public bool EsTurno { get; set; }

        public bool Ganador { get; set; }

        public List<IFicha> Fichas { get; set; }

        public Func<IFicha, int> FormaDeCalcularPuntuacionDeLasFichas { get; set; }

        public bool EsFichaJugable(IFicha ficha, object extremo1, object extremo2)
        {
            if (ficha.contenido.Item1.Equals(extremo1) || ficha.contenido.Item1.Equals(extremo2) || ficha.contenido.Item2.Equals(extremo1) || ficha.contenido.Item2.Equals(extremo2))
            {
                return true;
            }
            return false;
            //return ficha.Item1 == num1 || ficha.Item2 == num1 || ficha.Item1 == num2 || ficha.Item2 == num2;
        }

        public IFicha Juega(List<IFicha> fichas, object extremo1, object extremo2)
        {
            int length = Fichas.Count;
            int potencialMaximo = 0;
            IFicha fichaElegida = null;
            if (extremo1 == null && extremo2 == null)
            {
                foreach (IFicha ficha in Fichas)//Reviso por las fichas q tengo
                {
                    int potencial = 1;
                    object elemento = ficha.contenido.Item2;

                    foreach (IFicha ficha2 in Fichas)
                    {//Reviso cuales fichas de las q tengo tienen el otro elemento
                     //eso me dira que tan buena es la ficha
                        if (ficha2.Equals(ficha)) continue;
                        if (ficha2.contenido.Item1.Equals(elemento) || ficha2.contenido.Item2.Equals(elemento))
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
                    //Por si la puedo poner en ambos lados

                    elemento = ficha.contenido.Item1;
                    foreach (IFicha ficha2 in Fichas)
                    {
                        if (ficha2.Equals(ficha)) continue;
                        if (ficha2.contenido.Item1.Equals(elemento) || ficha2.contenido.Item2.Equals(elemento))
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
                if (fichaElegida.Equals(null)) return null;
                Fichas.Remove(fichaElegida);
                return fichaElegida;
            }
            //La idea de esto es escojer la ficha q pueda jugar por un numero y q su otro numero
            //sea lo mas comun posible entre el resto de las fichas q tengo. 

            foreach (IFicha ficha in Fichas)//Reviso por las fichas q tengo
            {
                int potencial = 1;
                if (EsFichaJugable(ficha, extremo1, extremo2))//Escojo las q puedo jugar
                {

                    object objeto = null;
                    //Guardo el numero por el cual la puedo jugar
                    if (ficha.contenido.Item1.Equals(extremo1) || ficha.contenido.Item1.Equals(extremo2))
                    {
                        objeto = ficha.contenido.Item2;
                        foreach (IFicha ficha2 in Fichas)
                        {//Reviso cuales fichas de las q tengo tienen el otro numero
                            //eso me dira que tan buena es la ficha jugable
                            if (ficha2.Equals(ficha) && Fichas.Count != 1) continue;
                            if (ficha2.contenido.Item1.Equals(objeto) || ficha2.contenido.Item2.Equals(objeto))
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
                    //con ambos numeros en el "tablero": num1 y num2.
                    if (ficha.contenido.Item2.Equals(extremo1) || ficha.contenido.Item2.Equals(extremo2))
                    {
                        objeto = ficha.contenido.Item1;
                        foreach (IFicha ficha2 in Fichas)
                        {
                            if (ficha2.Equals(ficha) && Fichas.Count != 1) continue;
                            if (ficha2.contenido.Item1.Equals(objeto) || ficha2.contenido.Item2.Equals(objeto))
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
            if (fichaElegida == null) return null;//Si nunca encontro ficha jugable entonces retorna null
            Fichas.Remove(fichaElegida);

            return fichaElegida;
        }

        public void Seleccionar(List<IFicha> fichas, bool bocaArriba, int cantFichas)
        {
            Fichas = new List<IFicha>();
            Random random = new Random();
            if (!bocaArriba)
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
                //TENER LA MAYOR CANTIDAD POSIBLE DE CADA NUMERO EN "Fichas"

                List<IFicha> escogidas = new List<IFicha>();

                do
                {
                    escogidas = EscogerFichas(cantFichas, fichas);//Escojo un grupo de fichas cuyos numeros no esten repetidos en las otras
                    if (escogidas.Count != 0)//Si encontro fichas con esas caracteristicas, las quito de las fichas del juego pa que no haya repetidas
                    {
                        this.Fichas.AddRange(escogidas);
                        for (int i = 0; i < escogidas.Count; i++)
                        {
                            fichas.Remove(escogidas[i]);
                        }
                    }
                } while (escogidas.Count != 0 && this.Fichas.Count < cantFichas);//Si aun no tengo suficientes fichas, repito el proceso
                                                                                 //hasta que tenga suficientes o hasta que escogidas salga vacia 

                if (this.Fichas.Count > cantFichas)//Si la cantidad de fichas que escogi excede el total de fichas por jugador, elimino las ultimas agregadas
                {
                    int resto = Fichas.Count - cantFichas;
                    for (int i = this.Fichas.Count - 1, j = resto; i >= 0 && j > 0; i--, j--)
                    {
                        fichas.Add(Fichas[i]);
                        Fichas.RemoveAt(i);
                    }
                }
                if (this.Fichas.Count < cantFichas)//Si me faltaron fichas por escoger, las escojo al azar
                {
                    while (this.Fichas.Count < cantFichas)
                    {
                        int a = random.Next(fichas.Count);
                        Fichas.Add(fichas[a]);
                        fichas.RemoveAt(a);
                    }
                }
            }
        }
        /////LA IDEA ES QUE ESCOJA LAS FICHAS DE MANERA Q POSEA LA MAYOR CANTIDAD DE NUMEROS POSIBLES
        private List<IFicha> EscogerFichas(int cantFichas, List<IFicha> fichas)
        {
            List<IFicha> escogidas = new List<IFicha>();
            List<IFicha> fichasSinDobles = new List<IFicha>();
            List<IFicha> fichasDobles = new List<IFicha>();

            foreach (IFicha ficha in fichas)
            {
                if (!ficha.contenido.Item1.Equals(ficha.contenido.Item2))
                {
                    fichasSinDobles.Add(ficha);
                }
                if (ficha.contenido.Item1.Equals(ficha.contenido.Item2))
                {
                    fichasDobles.Add(ficha);
                }
            }

            escogidas = FichasElegidas(fichasSinDobles, escogidas);
            return FichasElegidas(fichasDobles, escogidas);
        }
        private List<IFicha> FichasElegidas(List<IFicha> fichas, List<IFicha> escogidas)
        {
            if (fichas.Count == 0) return escogidas;
            if (escogidas.Count == 0) escogidas.Add(fichas[0]);
            foreach (IFicha ficha in fichas)
            {
                for (int j = 0; j < escogidas.Count; j++)
                {
                    if (EsFichaJugable(ficha, escogidas[j].contenido.Item1, escogidas[j].contenido.Item2))//Compruebo si alguno de los numeros de ficha ya lo tengo dentro de las q escogi
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
}

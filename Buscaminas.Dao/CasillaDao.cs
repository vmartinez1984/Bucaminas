using Buscaminas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buscaminas.Dao
{
    public class CasillaDao
    {
        /// <summary>
        /// Obtiene la lista de las casillas, con minas y numero de minas cerca
        /// </summary>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <param name="numeroDeMinas"></param>
        /// <returns></returns>
        public static List<Casilla> GetLst(int X = 8, int Y = 8, int numeroDeMinas = 10)
        {
            var i = 1;
            var lst = new List<Casilla>();
            var lstMinas = GetLstMinas(numeroDeMinas, (X * Y));
            for (int y = 0; y < Y; y++)
            {
                for (int x = 0; x < X; x++)
                {
                    var item = new Casilla();
                    item.Id = i;
                    item.X = x;
                    item.Y = y;
                    item.IsMina = lstMinas.Where(a => a == i).Count() == 1 ? true : false;
                    item.Estado = Estado.SinAbrir;
                    lst.Add(item);
                    i++;
                }
            }

            lst = AddNumeroDeMinasCerca(lst);

            return lst;
        }

        /// <summary>
        /// Obtiene una lista de número aleatorios
        /// </summary>
        /// <param name="numerodeMinas"></param>
        /// <param name="totalDeCasillas"></param>
        /// <returns></returns>
        private static List<int> GetLstMinas(int numerodeMinas, int totalDeCasillas)
        {
            Random random = new Random();
            int numeroAleatorio = 0;
            var lst = new List<int>();
            do
            {
                numeroAleatorio = random.Next(1, totalDeCasillas);
                if (lst.Where(x => x == numeroAleatorio).Count() == 1)
                {

                }
                else
                {
                    lst.Add(numeroAleatorio);
                }

            } while (lst.Count < numerodeMinas);
            return lst;
        }

        /// <summary>
        /// Agrega si hay mina cerca
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        private static List<Casilla> AddNumeroDeMinasCerca(List<Casilla> lst)
        {
            var lstConMinas = lst.Where(x => x.IsMina == true).ToList();
            foreach (var item in lstConMinas)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        var casilla = lst.Where(_casilla => _casilla.X == (item.X + i) && _casilla.Y == (item.Y + j)).FirstOrDefault();
                        if (casilla != null)
                            casilla.NumeroDeMinasCerca++;
                    }
                }
            }
            return lst;
        }
    }
}
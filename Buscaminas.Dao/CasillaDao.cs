using Buscaminas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Buscaminas.Dao
{
    public class CasillaDao
    {
        public static List<Casilla> GetLst(int x = 8, int y = 8, int numeroDeMinas = 10)
        {
            var lst = new List<Casilla>();
            var lstMinas = GetLstMinas(numeroDeMinas, (x * y));
            for (int i = 1; i <= (x * y); i++)
            {
                var item = new Casilla();
                item.Id = i;
                item.IsMina = lstMinas.Where(a => a == i).Count() == 1 ? true : false;
                item.Estado = Estado.SinAbrir;
                lst.Add(item);
            }
            return lst;
        }

        private static List<int> GetLstMinas(int numerodeMinas, int totalDeCasillas)
        {
            Random random = new Random();
            var lst = new List<int>();
            for (int i = 0; i < numerodeMinas; i++)
            {
                lst.Add(random.Next(1, totalDeCasillas));
            }
            return lst;
        }
    }
}

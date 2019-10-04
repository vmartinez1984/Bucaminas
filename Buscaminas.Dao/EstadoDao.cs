using Buscaminas.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buscaminas.Dao
{
    public class EstadoDao
    {
        public static List<Estado> GetLst()
        {
            var lst = new List<Estado>();
            lst.Add(new Estado { Id = 1, Nombre = "Duda" });
            lst.Add(new Estado { Id = 2, Nombre = "Sin Abrir" });
            lst.Add(new Estado { Id = 3, Nombre = "Abierta" });
            lst.Add(new Estado { Id = 4, Nombre = "Bandera" });
            return lst;
        }
    }
}

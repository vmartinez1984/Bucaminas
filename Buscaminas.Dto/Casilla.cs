using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buscaminas.Dto
{
    public class Casilla
    {
        public int Id { get; set; }
        public bool IsMina { get; set; }
        public int NumeroDeMinasCerca { get; set; }

    }
}

﻿namespace Buscaminas.Dto
{
    public class Casilla
    {
        public int Id { get; set; }
        public bool IsMina { get; set; }
        public int NumeroDeMinasCerca { get; set; }
        public Estado Estado { get; set; }
        public bool IsBanderaCorrecta { get; set; }
        public bool IsBandera { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]
    public class VideoCantidades
    {
        public static int CantidadDias=2;
        public static double PorcentajeMinimo = 0.8;

        public Estacion Estacion { get; set; }
        public List<DateTime> Fechas { get; set; }
        public VideoFechaL Videos { get; set; }

        public string NombreEstacion
        {
            get
            {
                return Estacion.Nombre;
            }
        }
    }

    [Serializable]
    public class VideoCantidadesL : List<VideoCantidades>
    {
    }

    [Serializable]
    public class VideoFecha
    {
        public int CantidadArchivos { get; set; }
        public int CantidadEventos { get; set; }
        public string Path { get; set; }
    }

    [Serializable]
    public class VideoFechaL : List<VideoFecha>
    {
    }
}

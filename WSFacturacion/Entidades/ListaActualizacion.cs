using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    [Serializable]

    public class ListaActualizacion
    {

        public ListaActualizacion(ListaActualizacionTipoLista TipoLista,ListaActualizacionModo Modo, string Nivel, int FrecuenciaHoraria,
                                  string HorariodeConsulta, Int16 DiferenciaEntreVias, Int16 Reintento) 
        {
            this.TipoLista = TipoLista;
            this.Modo = Modo;
            this.Nivel = Nivel;
            this.FrecuenciaHoraria = FrecuenciaHoraria;
            this.HorariodeConsulta = HorariodeConsulta;
            this.DiferenciaEntreVias = DiferenciaEntreVias;
            this.Reintento = Reintento;

        }

        public ListaActualizacion()
        {

        }

        // Tipo de Lista
        public ListaActualizacionTipoLista TipoLista { get; set; }

        // Modo 
        public ListaActualizacionModo Modo { get; set; }

        // Frecuencia Horaria
        public int? FrecuenciaHoraria { get; set; }

        // Horario de Consulta
        public string HorariodeConsulta { get; set; }

        // Nivel 
        public string Nivel { get; set; }

        // Diferencia entre Vias
        public Int16 DiferenciaEntreVias { get; set; }

        // Reintento 
        public Int16 Reintento { get; set; }


    }


    [Serializable]

    public class ListaActualizacionL : List<ListaActualizacion>
    {

    }

}

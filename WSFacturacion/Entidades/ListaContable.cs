using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{

    /// <summary>
    /// Estructura de una identidad de Tipo Lista Contable
    /// </summary>
    [Serializable]
    public class ListaContable
    {


        //campo Tipo
        public string ShortType { get; set; }

        //numero de estacion
        public int CodigoEstacion { get; set; }

        //campo Jornada
        public DateTime Jornada { get; set; }

        //campo Description
        public string DescriptionType { get; set; }

        //campo Nombre de estacion
        public string NombreEstacion { get; set; }

        //campo Via
        public string CodeVia { get; set; }

        //campo Turno
        public string CodeTurno { get; set; }

        //campo SubTurno
        public string CodeSubTurno { get; set; }

        //Descripcion Contable 
        public string Descripcion {

            get {

                string retval = "";
                if (CodeVia != null)
                {
                    retval += "C" + CodeVia.Trim() + " T" + CodeTurno.Trim() + "S" + CodeSubTurno.Trim();
                    retval += " F " + PrimerTicket.ToString("D07") + "-" + UltimoTicket.ToString("D07");
                }
                else
                {
                    retval += DescriptionType.Trim();
                }

                return retval;
            }
        
        }

        //campo PrimerTicket 
        public int PrimerTicket { get; set; }

        //campo UltimoTicket
        public int UltimoTicket { get; set; }

        //campo Monto
        public double Monto { get; set; }

        //public DateTime FechaJornada { get; set; }
        public override string ToString()
        {
            string retval = string.Empty;
            retval += ShortType.Trim() + ",";
            retval += Jornada.Day.ToString("D02")+Jornada.Month.ToString("D02")+Jornada.Year.ToString() + ",";
            retval += Descripcion;
            retval += ",," + Monto.ToString("F2").Replace(",",".");
            return retval;
        }

        //Constructor vacio
        public ListaContable() { }

    }

    /// <summary>
    /// Lista de objetos ListaContable.
    /// </summary>
    [Serializable]
    public class ListaContableL : List<ListaContable>
    {
    }

}

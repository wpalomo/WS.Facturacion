using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class CodAutorizacionElectronica
    {
        /// <summary>
        /// Codigo CAE
        /// </summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Tipo (F - T - D - C)
        /// </summary>
        public char Tipo { get; set; }

        public char? TipoReferencia { get; set; }

        /// <summary>
        /// Identity
        /// </summary>
        public int Identity { get; set; }

        /// <summary>
        /// Fecha
        /// </summary>
        public DateTime Fecha { get; set; }


        /// <summary>
        /// Tipo Descripcion (Factura - F: Ticket - T)
        /// </summary>
        public string sTipo
        {
            get
            {
                string Descripcion = "";

                switch (Tipo)
                {
                    case 'F':
                        Descripcion = "Factura";
                        break;

                    case 'T':
                        Descripcion = "Ticket";
                        break;

                    case 'C':
                        Descripcion = "Nota de Credito";
                        break;

                    case 'D':
                        Descripcion = "Nota de Debito";
                        break;

                    default:
                        Descripcion = "";
                        break;
                }

                return Descripcion;
            }
        }


        public string sTipoReferencia
        {
            get
            {
                string Descripcion = "";

                switch (TipoReferencia)
                {
                    case 'F':
                        Descripcion = "Factura";
                        break;

                    case 'T':
                        Descripcion = "Ticket";
                        break;
                }

                return Descripcion;
            }
        }

        /// <summary>
        /// Fecha Vencimiento CAE
        /// </summary>
        public DateTime Vencimiento { get; set; }

        /// <summary>
        /// Serie CAE
        /// </summary>
        public string Serie { get; set; }

        /// <summary>
        /// Numero Inicial CAE
        /// </summary>
        public int NumeroInicial { get; set; }

        /// <summary>
        /// Numero Fianl CAE
        /// </summary>
        public int NumeroFinal { get; set; }

        /// <summary>
        /// Tipo Comprobante (CFE - CFC)
        /// </summary>
        public string Comprobante { get; set; }

        /// <summary>
        /// Concatenacion de CAE - Serie - tipo - numero
        /// </summary>
        public string Descripcion { get; set; }

    }

    /// <summary>
    /// Lista de la misma entidad
    /// </summary>
    [Serializable]
    public class CodAutorizacionElectronicaL : List<CodAutorizacionElectronica>
    {
    }

}

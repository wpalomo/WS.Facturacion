using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region CHIP: Clase para entidad de una Tarjeta Chip

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Chip
    /// </summary>*********************************************************************************************

    [Serializable]
    public class Chip
    {
        public Chip()
        {
        }

        public Chip(Estacion estacioEmision,                string patente,                 string dispositivo, 
                    int numeroExterno,                      string habilitado,              DateTime fechaEntrega,
                    ChipListaNegra listaNegra,  int? numeroInterno)
        {
            this.EstacionEmision = estacioEmision;
            this.Patente = patente;
            if( numeroInterno != null )
                this.NumeroInterno = (int)numeroInterno;
            this.NumeroExterno = numeroExterno;
            this.Habilitado = habilitado;
            this.FechaEntrega = fechaEntrega;
            this.ListaNegra = listaNegra;
            this.Dispositivo = dispositivo;
        }


        // Estacion en la que se dio de alta la tarjeta chip
        public Estacion EstacionEmision { get; set; }

        // Patente a la que pertenece 
        public string Patente { get; set; }

        // Numero interno
        public int NumeroInterno { get; set; }
        // dispositivo
        public string Dispositivo { get; set; }
        // Numero externo
        public int NumeroExterno { get; set; }

        // Si esta o no habilitado
        public string Habilitado { get; set; }

        // Si esta o no entregado
        public bool Entregado { get; set; }

        // Fecha de entrega
        public DateTime FechaEntrega { get; set; }

        // Booleano que determina si esta habilitado o no
        public bool esHabilitado
        {
            get
            {
                return (Habilitado == "S");
            }
        }

        // Si esta en lista negra o no
        public ChipListaNegra ListaNegra { get; set; }

        // Booleano que determina si esta o no en lista negra
        public bool esListaNegra
        {
            get
            {
                return (ListaNegra != null);
            }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Chip
    /// </summary>*********************************************************************************************
    public class ChipL : List<Chip>
    {
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region CHIPLISTANEGRA: Clase para entidad de Lista Negra de una Tarjeta Chip

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ChipListaNegra
    /// </summary>*********************************************************************************************

    [Serializable]
    public class ChipListaNegra
    {
        public ChipListaNegra()
        {
        }

        public ChipListaNegra(string numeroChip,           int numeroExterno,    DateTime fechaInhabilitacion,           
                             Usuario usuario,               string comentario)
        {
            this.NumeroChip = numeroChip;
            this.NumeroExterno = numeroExterno;
            this.FechaInhabilitacion = fechaInhabilitacion;
            this.Usuario = usuario;
            this.Comentario = comentario;
        }


        // Numero de tarjeta
        public string NumeroChip { get; set; }

        // Numero externo de tarjeta
        public int NumeroExterno { get; set; }

        // Fecha de puesta en LN
        public DateTime FechaInhabilitacion { get; set; }

        // Usuario que lo puso en LN
        public Usuario Usuario { get; set; }

        // Comentario colocado en el momento de ponerlo en LN
        public string Comentario { get; set; }

        // Patente
        public string Patente { get; set; }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos ChipListaNegra
    /// </summary>*********************************************************************************************
    public class ChipListaNegraL : List<ChipListaNegra>
    {
    }

    #endregion
}

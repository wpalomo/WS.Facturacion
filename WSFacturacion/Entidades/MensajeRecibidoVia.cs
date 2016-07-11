using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region MENSAJE RECIBIDO DE LA VIA: Clase para entidad de MensajeRecibidoVia.
    /// <summary>
    /// Estructura de una entidad MensajeRecibidoVia
    /// </summary>

    [Serializable]

        public class MensajeRecibidoVia
        {
            /// ***********************************************************************************************
            /// <summary>
            /// En el constructor de la clase asigna los valores de un mensaje en particular
            /// </summary>
            /// <typeparam name="int">int</typeparam>
            /// <param name="codigo">Codigo de mensaje</param>
            /// <typeparam name="int">string</typeparam>
            /// <param name="codigo">Descripcion del mensaje</param>
            /// <returns></returns>
            /// ***********************************************************************************************

            public MensajeRecibidoVia(int estacion, byte via, DateTime fecha, MensajePredefinido mensaje, string visto, string nombreVia)
            {
                this.Estacion = estacion;
                this.Fecha = fecha;
                this.Mensaje = mensaje;
                this.Via = new Via(estacion, via, nombreVia);
                this.Visto = visto;
            }


            // Estacion
            public int Estacion { get; set; }

            // Via
            public Via Via { get; set; }

            // Fecha
            public DateTime Fecha { get; set; }

            // Mensaje
            public MensajePredefinido Mensaje { get; set; }

            // Visto
            public string Visto { get; set; }

            //Para mostrar en las grillas, etc
            public override string ToString()
            {
                return Mensaje.Descripcion;
            }
        }


        [Serializable]

        /// <summary>
        /// Lista de objetos MensajePredefinido.
        /// </summary>
        public class MensajeRecibidoViaL : List<MensajeRecibidoVia>
        {
        }
    
    #endregion

}

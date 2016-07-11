using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region ALARMA DE RETIRO: Clase para entidad de AlarmaRetiro.
    /// <summary>
    /// Estructura de una entidad AlarmaRetiro
    /// </summary>

    [Serializable]

        public class AlarmaRetiro
        {
            /// ***********************************************************************************************
            /// <summary>
            /// En el constructor de la clase asigna los valores de una alarma en particular
            /// </summary>
            /// <typeparam name="int">int</typeparam>
            /// <param name="codigo">Estación</param>
            /// <typeparam name="int">int</typeparam>
            /// <param name="codigo">Vía</param>
            /// <typeparam name="int">int</typeparam>
            /// <param name="codigo">Parte</param>
            /// <typeparam name="int">DateTime</typeparam>
            /// <param name="codigo">Jornada</param>
            /// <typeparam name="int">int</typeparam>
            /// <param name="codigo">Turno</param>
            /// <typeparam name="int">string</typeparam>
            /// <param name="codigo">Peajista</param>
            /// <typeparam name="int">decimal</typeparam>
            /// <param name="codigo">RecaudoNeto</param>
            /// <typeparam name="int">decimal</typeparam>
            /// <param name="codigo">Minimo</param>
            /// <returns></returns>
            /// ***********************************************************************************************

            public AlarmaRetiro(int estacion, int via, int parte, DateTime jornada, int turno, string id, string peajista, decimal recaudoNeto, decimal minimo)
            {
                this.Estacion = estacion;
                this.Via = via;
                this.Parte = parte;
                this.Jornada = jornada;
                this.Turno = turno;
                this.Peajista = peajista;
                this.Id = id;
                this.RecaudoNeto = recaudoNeto;
                this.Minimo = minimo;
            }


            // Estacion
            public int Estacion { get; set; }

            // Via
            public int Via { get; set; }

            // Parte
            public int Parte { get; set; }

            // Jornada
            public DateTime Jornada { get; set; }

            // Turno
            public int Turno { get; set; }

            // Id Peajista
            public string Id { get; set; }

            // Peajista
            public string Peajista { get; set; }

            // RecaudoNeto
            public decimal RecaudoNeto { get; set; }

            // Minimo
            public decimal Minimo { get; set; }
    
    }


        [Serializable]

        /// <summary>
        /// Lista de objetos AlarmaRetiro.
        /// </summary>
        public class AlarmaRetiroL : List<AlarmaRetiro>
        {
        }
    
    #endregion

}

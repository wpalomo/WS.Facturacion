using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region MENSAJE DE LA VIA: Clase para entidad de MensajePredefinido.
    /// <summary>
    /// Estructura de una entidad MensajePredefinido
    /// </summary>

    [Serializable]

    public class MensajePredefinido
   
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
            
            public MensajePredefinido(int codigo,
                                      string descripcion)
            {
                this.Codigo = codigo;
                this.Descripcion = descripcion;
            }

            public MensajePredefinido()
            {

            }


            // Codigo de mensaje de la via
            public int Codigo { get; set; }

            // Descripcion de mensaje de la via
            public string Descripcion { get; set; }

            //Para mostrar en las grillas, etc
            public override string ToString()
            {
                return Descripcion;
            }
        }


    [Serializable]

    /// <summary>
    /// Lista de objetos MensajePredefinido.
    /// </summary>
    public class MensajePredefinidoL : List<MensajePredefinido>
    {
    }

    #endregion

    #region MENSAJE DE LA SUPERVISION: Clase para entidad de MensajePredefinidoSup.
    /// <summary>
    /// Estructura de una entidad MensajePredefinidoSup
    /// </summary>

    [Serializable]

    public class MensajePredefinidoSup
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

        public MensajePredefinidoSup(int codigo,
                                  string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }

        public MensajePredefinidoSup()
        {

        }


        // Codigo de mensaje de la via
        public int Codigo { get; set; }

        // Descripcion de mensaje de la via
        public string Descripcion { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos MensajePredefinidoSup.
    /// </summary>
    public class MensajePredefinidoSupL : List<MensajePredefinidoSup>
    {
    }

    #endregion
 }
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TipoAutorizacionExento: Clase para entidad de los posibles sentidos de circulacion de una vía

    #region Atributos
    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoAutorizacionExento
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class TipoAutorizacionExento
    {
        #region Constructor

        // Constructor que asigna la descripcion
        public TipoAutorizacionExento(string sCodigo)
        {
            Codigo = sCodigo;

            switch (sCodigo)
            {
                case "A":
                    Descripcion = "Autorização";
                    break;

                case "M":
                    Descripcion = "Matrícula";
                    break;

                case "R":
                    Descripcion = "RENAVAM";
                    break;

                default:
                    Descripcion = "<Ninguno>";
                    break;
            }
        }

        #endregion

        #region Atributos

        // Codigo del sentido de circulacion
        public string Codigo { get; set; }

        // Descripcion del Sentido de Circulacion
        public string Descripcion { get; set; }

        // Por defecto la estructura retorna la descripcion (para las grillas)
        public override string ToString()
        {
            return Descripcion;
        }

        #endregion
    }
    
    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoAutorizacionExento
    /// </summary>*********************************************************************************************
    [Serializable]
    public class TipoAutorizacionExentoL: List<TipoAutorizacionExento>
    {
    }

    #endregion
}

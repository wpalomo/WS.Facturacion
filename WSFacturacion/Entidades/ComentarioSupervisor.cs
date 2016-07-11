using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Telectronica.Peaje
{
    #region COMENTARIOS DEL SUPERVISOR: Clase para entidad de ComentarioSupervisor.

    /// <summary>
    /// Estructura de una entidad ComentarioSupervisor
    /// </summary>
    [Serializable]
    public class ComentarioSupervisor
    {
        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un mensaje en particular
        /// </summary>
        /// <typeparam name="">int</typeparam>
        /// <param name="codigo">Estación</param>
        /// <typeparam name="">int</typeparam>
        /// <param name="codigo">Vía</param>
        /// <typeparam name="">long</typeparam>
        /// <param name="codigo">Número de Tránsito</param>
        /// <typeparam name="">datetime</typeparam>
        /// <param name="codigo">Fecha</param>
        /// <typeparam name="">string</typeparam>
        /// <param name="codigo">Comentario</param>
        /// ***********************************************************************************************
        public ComentarioSupervisor(int iEstacion, int iVia, long lTransito, string sId, DateTime dFecha, string sComentario)
        {
            comentario = sComentario;
            estacion = iEstacion;
            fecha = dFecha;
            via = iVia;
            id = sId;
            transito = lTransito;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase por defecto
        /// </summary>
        /// ***********************************************************************************************
        public ComentarioSupervisor()
        {
        }

        public int estacion { get; set; }
        public int via { get; set; }
        public DateTime fecha{ get; set; }
        public string comentario { get; set; }
        public string id { get; set; }
        public long transito { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return comentario;
        }
    }
    
    /// <summary>
    /// Lista de objetos MensajePredefinidoSup.
    /// </summary>
    [Serializable]
    public class ComentarioSupervisorL : List<ComentarioSupervisor>
    {
    }

    /// <summary>
    /// Representa una opción de filtro para mostrar los comentarios de supervisor
    /// </summary>
    [Serializable]
    [DataContract]
    public class FiltroComentarioSup
    {
        /// <summary>
        /// Representa la clave
        /// </summary>
        [DataMember]
        public string Key { get; set; }

        /// <summary>
        /// Representa la descripción o el texto a mostrar
        /// </summary>        
        [DataMember]
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si este filtro está seleccionado por defecto
        /// </summary>
        [DataMember]
        public bool esPorDefecto { get; set; }
    }

    /// <summary>
    /// Representa una lista de opciones de filtro para mostrar los comentarios de supervisor
    /// </summary>
    [Serializable]
    public class FiltroComentarioSupL : List<FiltroComentarioSup>
    {    
    }

    #endregion
}

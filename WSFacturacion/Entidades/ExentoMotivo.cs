using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase entidad que representa un Motivo de Exento
    /// </summary>
    [DataContract]
    public class ExentoMotivo
    {

        // Constructor Vacio
        public ExentoMotivo()
        {
        }


        // Constructor que carga las propiedades con los parametros recibidos
        public ExentoMotivo(string codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }
        

        /// <summary>
        /// Obtiene o Establece el código único e identificatorio
        /// </summary>
        [DataMember]
        public string Codigo { get; set; }

        /// <summary>
        /// Obtiene o establece una descripción para este motivo de exento.
        /// </summary>
        [DataMember]
        public string Descripcion { get; set; }

        /// <summary>
        /// Devuelve la descripción al hacer un ToString()
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Descripcion;
        }
    }
    
    [Serializable]
    public class ExentoMotivoL : List<ExentoMotivo>
    {
    }
}
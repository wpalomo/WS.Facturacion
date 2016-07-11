using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region AUDITORIACODIGO: Clase para entidad de Codigos de Auditoria

    [Serializable]
    [XmlRootAttribute(ElementName = "Registro", IsNullable = false)]
    public class AuditoriaCodigo
    {
        /// ***********************************************************************************************        /// <summary>
        /// En el constructor de la clase asigna los valores de una zona en particular
        /// </summary>
        /// <param name="codigo">string - Codigo de Auditoria</param>
        /// <param name="Descripcion">Descripcion del Codigo de auditoria</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public AuditoriaCodigo(string codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        ///****************************************************************************************************        /// <summary>
        /// Sobreescribimos el constructor de la clase de codigo de auditoria con uno que no requiera parametros
        /// </summary>****************************************************************************************************
        public AuditoriaCodigo()
        {
        
        }


        // Codigo de auditoria
        public string Codigo { get; set; }

        // Descripcion del codigo de auditoria
        public string Descripcion { get; set; }

        //Código y Descripción juntos.
        public string CodigoDescripcion
        { 
            get 
            { 
                return Codigo + " - " + Descripcion; 
            } 
        }
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos AuditoriaCodigo
    /// </summary>*********************************************************************************************
    public class AuditoriaCodigoL : List<AuditoriaCodigo>
    {
    }


    #endregion

}

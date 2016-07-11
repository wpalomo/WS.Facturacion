using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Facturacion
{
    #region TIPODOCUMENTO: Clase para entidad de Tipo de Documento

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoDocumento
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class TipoDocumento
    {
        public TipoDocumento(int codigo, string descripcion)
        {
            this.Codigo = codigo;
            this.Descripcion = descripcion;
        }


        // Codigo de Tipo deDocumento
        public int Codigo { get; set; }

        // Descripcion del Tipo de Documento
        public string Descripcion { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TipoDocumento
    /// </summary>*********************************************************************************************
    public class TipoDocumentoL : List<TipoDocumento>
    {
    }

    #endregion

}

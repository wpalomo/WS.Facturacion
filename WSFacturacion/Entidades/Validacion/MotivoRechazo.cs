using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    #region MotivoRechazoTag
    /// <summary>
    /// Estructura de una entidad 
    /// </summary>

    [Serializable]
    public class MotivoRechazo
    {
        private string descripcionCompleta;
        private string descripcion;

        // Codigo del Motivo de Rechazo
        public String Codigo { get; set; }

        // Descripción del Motivo de Rechazo
        public string Descripcion
        {
            get
            {
                return this.descripcion;
            }
            set
            {
                this.descripcion = value;
                this.descripcionCompleta = this.Codigo + " - " + this.Descripcion;
            }
        }


        public MotivoRechazo(String lcodigo, String ldescripcion)
        {
            this.Codigo = lcodigo;
            this.Descripcion = ldescripcion;
            descripcionCompleta = lcodigo + " - " + ldescripcion;
        }
        
        public MotivoRechazo()
        {
        }
    }

    [Serializable]

    /// <summary>
    /// Lista de objetos MotivoRechazo.
    /// </summary>
    public class MotivoRechazoL : List<MotivoRechazo>
    {
    }
    #endregion
}

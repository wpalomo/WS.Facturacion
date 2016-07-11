using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region CAUSA_SUPERVISION: Clase para entidad de Causas de Supervisón.
    /// <summary>

    /// </summary>
    /// 

    [Serializable]
    [XmlRootAttribute(ElementName = "CausaSupervision", IsNullable = false)]

    public class CausaSupervision
    {
        public CausaSupervision()
        {
        }
        //public CausaSupervision(Int16 nCodigo, string sDescr, string tipop, string tipbo, string subfp, string defecto)
        //{
        //    this.Codigo = nCodigo;
        //    this.Descripcion = sDescr;
        //    this.MedioPago = tipop;
        //    this.FormaPago = tipbo;
        //    this.SubformaPago = subfp;
        //    this.EsDefecto = defecto;
        //}
        // Codigo Motivo Quiebre
        public Int16 Codigo { get; set; }

        // Descripción Motivo Quiebre
        public String Descripcion { get; set; }

        // Medio de Pago
        public String MedioPago { get; set; }

        // Forma de Pago
        public String FormaPago { get; set; }        

        // Subforma de Pago
        public string SubformaPago { get; set; }

        // Codigo por Defecto S/N
        public string EsDefecto { get; set; }

        // Codigo Motivo Quiebre
        public Int16 Anomal { get; set; }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Feriado.
    /// </summary>
    public class CausaSupervisionL : List<CausaSupervision>
    {
    }

    #endregion


}

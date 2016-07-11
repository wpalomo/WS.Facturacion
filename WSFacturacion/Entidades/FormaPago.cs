using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region FORMAPAGO: Clase para entidad de las Formas de Pago definidas


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Forma de Pago
    /// </summary>*********************************************************************************************

    [Serializable]

    public class FormaPago
    {

        public FormaPago()
        {
        }


        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de Tipo y Subtipo de forma de pago en particular
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public FormaPago(string tipo, string subTipo, string descripcion)
        {
            this.Tipo = tipo;
            this.SubTipo = subTipo;
            this.Descripcion = descripcion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de Tipo y Subtipo de forma de pago concatenados 
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public FormaPago(string codigo, string descripcion)
        {
            this.CodigoFormaPago = codigo;
            this.Descripcion = descripcion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de una forma de pago en particular
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public FormaPago(string tipo, string subTipo, string descripcion, string nombreCorto, string iniciales, string automatica, string cobraVia )
        {
            this.Tipo = tipo;
            this.SubTipo = subTipo;
            this.Descripcion = descripcion;
            this.NombreCorto = nombreCorto;
            this.Iniciales = iniciales;
            this.Automatica = automatica;
            this.CobraVia = cobraVia;
        }


        // Codigo de Tipo de Operacion (Tipop)
        public string Tipo { get; set; }

        // Codigo de Sub Tipo de OPeracion (Tipbo)
        public string SubTipo { get; set; }

        // Codigo de tipo y subtipo de forma de pago concatenados
        public string CodigoFormaPago
        {
            get { return Tipo + SubTipo; }
            set
            {
                this.Tipo = value.Substring(0, 1);
                this.SubTipo = value.Substring(1, 1);
            }
        }

        // Descripcion de la forma de pago
        public string Descripcion { get; set; }

        // Nombre corto utilizado en Online
        public string NombreCorto { get; set; }

        // Iniciales, utilizadas en Online nivel 0
        public string Iniciales { get; set; }

        // Codigo que identifica si es una forma de pago automatica
        public string Automatica { get; set; }

        // Codigo que identifica si la forma de pago se utiliza en la via
        public string CobraVia { get; set; }

        // Determina si la forma de pago es una forma de pago automatica
        public bool esFormaPagoAutomatica
        {
            get { return Automatica == "S"; }
            set { Automatica = value ? "S" : "N"; }
        }

        // Determina si la forma de pago se utiliza en la via 
        public bool esCobraEnVia
        {
            get { return CobraVia == "S"; }
            set { CobraVia = value ? "S" : "N"; }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos FormaPago
    /// </summary>*********************************************************************************************
    public class FormaPagoL : List<FormaPago>  
    {
    }


    #endregion



}

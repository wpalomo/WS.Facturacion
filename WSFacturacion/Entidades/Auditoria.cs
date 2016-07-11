using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region AUDITORIA: Clase para entidad de Auditoria del sistema


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Auditoria 
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class Auditoria
    {

        /// *********************************************************************************************<summary>
        /// Sobrecargamos el constructor de la clase, pasando los datos basicos que conocemos al momento de auditar
        /// </summary>*********************************************************************************************
        public Auditoria(string codigoauditoria,
                         string movimiento, 
                         string codigoregistro, 
                         string descripcion)
        {

            CodigoAuditoria = new AuditoriaCodigo(codigoauditoria, "");
            Movimiento = movimiento;
            CodigoRegistro = codigoregistro;
            Descripcion = descripcion;
        }


        // Estacion en la que se realiza la auditoria
        public int Estacion { get; set; }

        // Fecha del movimiento
        public DateTime Fecha { get; set; }

        // Usuario que realiza el movimiento
        public string Usuario { get; set; }

        // Codigo de Auditoria
        public AuditoriaCodigo CodigoAuditoria { get; set; }

        // Movimiento realizado (alta, baja, etc)
        public string Movimiento { get; set; }

        // Codigo que identifica al registro auditado (Nº cliente, tag, etc)
        public string CodigoRegistro { get; set; }

        // Texto descriptivo de la operacion realizada
        public string Descripcion { get; set; }

        // Terminal desde la que se realizo el movimiento
        public string Terminal { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Auditoria
    /// </summary>*********************************************************************************************
    public class AuditoriaL: List<Auditoria>
    {
    }


    #endregion

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Facturacion
{
    #region CUENTA: Clase para entidad de las Cuentas de Clientes

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Cuenta
    /// </summary>*********************************************************************************************

    [Serializable]
    [XmlRootAttribute(ElementName = "Cuenta", IsNullable = false)]
    public class Cuenta
    {
        // Constructor vacio
        public Cuenta()
        {
        }

        public Cuenta(int numero,                 TipoCuenta tipoCuenta,          DateTime? fechaEgreso,
                      AgrupacionCuenta agrupacion, string descripcion,            string eliminado,
                      Cliente cliente)
        {
            this.Numero = numero;
            this.TipoCuenta = tipoCuenta;
            this.FechaEgreso = fechaEgreso;
            this.Agrupacion = agrupacion;
            this.Descripcion = descripcion;
            this.Eliminado = eliminado;
            this.Cliente = cliente;
        }

        // Numero de cuenta
        public int Numero { get; set; }

        // Tipo de cuenta
        public TipoCuenta TipoCuenta { get; set; }

        // Fecha de egreso
        public DateTime? FechaEgreso { get; set; }

        // Subtipo de cuenta
        public AgrupacionCuenta Agrupacion { get; set; }

        // Descripcion de la cuenta
        public string Descripcion { get; set; }

        // Eliminado
        public string Eliminado { get; set; }

        // Determina si la cuenta esta eliminada
        public bool esEliminada
        {
            get
            {
                return (Eliminado == "S");
            }
        }


        // Cliente al que le pertenece la cuenta
        public Cliente Cliente { get; set; }

        //Minimo monto de recarga posible para cuentas PREPAGAS
        public decimal MontoMinimoRecarga { get; set; }

        //Máximo monto de recarga posible para cuentas PREPAGAS
        public decimal MontoMaximoRecarga { get; set; }

        //Estaciones Habilitadas
        public CuentaEstacionL EstacionesHabilitadas { get; set; }

        //Vehiculos Habilitados en la cuenta
        public VehiculoHabilitadoL VehiculosHabilitados { get; set; }
    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Cuenta
    /// </summary>*********************************************************************************************
    public class CuentaL : List<Cuenta>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una cuenta con un determinado numero
        /// </summary>
        /// <param name="numero">int - Numero de Cuenta a buscar</param>
        /// <returns>objeto Cuenta que corresponda al numero buscado</returns>
        /// ***********************************************************************************************
        public Cuenta FindNumeroCuenta(int numero)
        {
            Cuenta oCuenta = null;

            foreach (Cuenta oCue in this)
            {
                if (numero == oCue.Numero)
                {

                    oCuenta = oCue;
                    break;
                }
            }

            return oCuenta;
        }
    }

    #endregion
}

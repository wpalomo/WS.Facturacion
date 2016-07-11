using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region VEHICULO: Clase para entidad de los vehiculos definidos

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Vehiculo
    /// </summary>*********************************************************************************************

    [Serializable]

    public class Vehiculo
    {

        // Constructor vacio
        public Vehiculo()
        {
        }


        public Vehiculo(string patente,                 Cliente cliente,                VehiculoMarca marca,
                        VehiculoModelo modelo,          VehiculoColor color,            CategoriaManual categoria,
                        DateTime? fechaVencimiento,     Tag tag,                        Chip chip, 
                        Cuenta cuenta,                  string causaInhabilitacion)
        {
            this.Patente = patente;
            this.Cliente = cliente;
            this.Marca = marca;
            this.Modelo = modelo;
            this.Color = color;
            this.Categoria = categoria;
            this.FechaVencimiento = fechaVencimiento;
            this.Tag = tag;
            this.Chip = chip;
            this.Cuenta = cuenta;
            this.CausaInhabilitacion = causaInhabilitacion;
        }


        // Patente por la cual se localiza el vehiculo (unico)
        public string Patente { get; set; }

        // Cliente al que le pertenece el vehiculo
        public Cliente Cliente { get; set; }

        // Marca del vehiculo
        public VehiculoMarca Marca { get; set; }

        // Modelo del vehiculo
        public VehiculoModelo Modelo { get; set; }

        // Color del vehiculo
        public VehiculoColor Color { get; set; }

        // Categoria del vehiculo
        public CategoriaManual Categoria { get; set; }

        // Fecha de vencimiento del vehiculo
        public DateTime? FechaVencimiento { get; set; }

        // Tag que posee el vehiculo (solo 1)
        public Tag Tag { get; set; }

       // public OSAsTag TagPex { get; set; }

        public OSAsTag TagOSA { get; set; }

        // Tag o Chip en lista negra
        public string ListaNegra { get; set; }

        // Tarjeta chip que posee el vehiculo (solo 1)
        public Chip Chip { get; set; }

        // Cuenta habilitada y prioritara para el vehiculo localizado a traves de la patente del vehiculo
        public Cuenta Cuenta { get; set; }

        //Lo sacamos porque las referencias circulares dan error al serializar para Silverlight
        // Todas las cuentas a las que pertenece el vehiculo
        //public CuentaL Cuentas { get; set; }

        // Causa de inhabilitacion clasificada por el SP
        public string CausaInhabilitacion { get; set; }

        //VehiculoExistente
        public bool VehiculoExistente { get; set; }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Vehiculo
    /// </summary>*********************************************************************************************
    public class VehiculoL : List<Vehiculo>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un vehiculo con una determinada patente
        /// </summary>
        /// <param name="patente">string - Patente a buscar</param>
        /// <returns>objeto Vehiculo que corresponda a la patente buscada</returns>
        /// ***********************************************************************************************
        public Vehiculo FindPatente(string patente)
        {
            Vehiculo oVehiculo = null;

            foreach (Vehiculo oVeh in this)
            {
                if (patente.Trim().ToUpper() == oVeh.Patente.Trim().ToUpper())
                {

                    oVehiculo = oVeh;
                    break;
                }
            }

            return oVehiculo;
        }

        public VehiculoSLL ConvertEnSL()
        {
            VehiculoSLL lista = new VehiculoSLL();
            foreach (Vehiculo item in this)
            {
                lista.Add(new VehiculoSL(item));
            }
            return lista;

        }
    }


   

    [Serializable]

    public class VehiculoSL
    {

        // Constructor Vacio
        public VehiculoSL()
        {
        }

        // Constructor Vacio
        public VehiculoSL(Vehiculo oVehiculo)
        {
            this.ClienteSL = new ClienteSL(oVehiculo.Cliente);
            this.Patente = oVehiculo.Patente;
            this.VehiculoExistente = oVehiculo.VehiculoExistente;
            this.Agrupacion = oVehiculo.Cuenta.Agrupacion.DescrAgrupacion;
        }


        // public VehiculoSL(int numeroCliente, string razonSocial, string domicilio)
        // {
        //     this.NumeroCliente = numeroCliente;
        //     this.RazonSocial = razonSocial;

        //  }
        // Numero de cliente
        public ClienteSL ClienteSL { get; set; }
        // Razon Social 
        public String Patente { get; set; }

        // agrupacion 
        public String Agrupacion { get; set; }

        //VehiculoExistente
        public bool VehiculoExistente { get; set; }
    }

    /// *********************************************************************************************<summary>
    /// Lista de objetos Cliente
    /// </summary>*********************************************************************************************
    [Serializable]
    public class VehiculoSLL : List<VehiculoSL>
    {

        public VehiculoSL FindPatente(string patente)
        {
            VehiculoSL oVehiculo = null;

            foreach (VehiculoSL oVeh in this)
            {
                if (patente.Trim().ToUpper() == oVeh.Patente.Trim().ToUpper())
                {

                    oVehiculo = oVeh;
                    break;
                }
            }

            return oVehiculo;
        }

    }





    #endregion
}

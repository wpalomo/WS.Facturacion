using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Facturacion;

namespace Telectronica.Peaje
{
    #region PAGODIFERIDO: Clase para entidad de los pagos diferidos generados en las vias

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Pago Diferido
    /// </summary>*********************************************************************************************

    [Serializable]

    public class PagoDiferido
    {
        public Estacion Estacion { get; set; }
        public int NumeroPagoDiferido { get; set; }
        public int Evento { get; set; }
        public string Patente { get; set; }
        public string Documento { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public int NumeroVia { get; set; }
        public CategoriaManual Categoria { get; set; }
        public decimal Importe { get; set; }
        public Usuario Operador { get; set; }
        public int NumeroParte { get; set; }
        public Usuario Supervisor { get; set; }
        public String Estado { get; set; }
        public String DescEstado { get; set; }
        public DateTime? FechaVencimiento { get; set; }

        public PagoDiferido()
        {
        }
    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos PagoDiferido
    /// </summary>*********************************************************************************************
    public class PagoDiferidoL : List<PagoDiferido>
    {
    }

    #endregion

    #region PAGODIFERIDOSUPERVISOR: Clase para entidad de los pagos diferidos autorizados o rechazados por el supervisor

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Pago Diferido Supervisor
    /// </summary>*********************************************************************************************

    [Serializable]

    public class PagoDiferidoSupervisor: PagoDiferido
    {        
        public string Nombre { get; set; }        
        public string Direccion { get; set; }
        public string Localidad { get; set; }
        public int? Provincia { get; set; }
        public string Telefono { get; set; }
        public Vehiculo Vehiculo { get; set; } 
        //public PagoDiferido PagoDiferido { get; set; }
        public string Comentario { get; set; }
        public CausaPagoDiferido Causa { get; set; }
        public string TitularVehiculo { get; set; }
        public Estado EstadoUF { get; set; }
        public PagoDiferidoSupervisor()
        {
        }
    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos PagoDiferidoSupervisor
    /// </summary>*********************************************************************************************
    public class PagoDiferidoSupervisorL : List<PagoDiferidoSupervisor>
    {
    }

    #endregion

    #region CAUSAPAGODIFERIDO: Clase para entidad de causa de los pagos diferidos 

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Pago Diferido Supervisor
    /// </summary>*********************************************************************************************

    [Serializable]

    public class CausaPagoDiferido
    {
        public int Codigo { get; set; }
        public string Descripcion { get; set; }

        public CausaPagoDiferido()
        {
        }

        public CausaPagoDiferido(int cod, string descr)
        {
            Codigo = cod;
            Descripcion = descr;
        }
    }

    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos CausaPagoDiferido
    /// </summary>*********************************************************************************************
    public class CausaPagoDiferidoL : List<CausaPagoDiferido>
    {
    }

    #endregion


}

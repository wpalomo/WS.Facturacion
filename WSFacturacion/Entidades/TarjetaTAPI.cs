using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{

    /// <summary>
    /// Estructura de una identidad de Tipo TarjetaTAPI
    /// </summary>
    [Serializable]
    public class TarjetaTAPI
    {
        #region Propiedades

        //Numero de Lista
        public int NumeroLista { get; set; }

        //Numero de Tarjeta
        public string NumeroTarjeta { get; set; }

        //Vencimiento
        public DateTime Vencimiento { get; set; }

        //Razon Social
        public string RazonSocial { get; set; }

        //Patente
        public string Patente { get; set; }

        //Estacion de Origen
        public int EstacionOrigen { get; set; }

        //Estacion de Origen
        public Estacion oEstacionOrigen { get; set; }

        //Nombre de la estacion de origen
        public string EstacionOrigenDescripcion
        {
            get
            {
                return oEstacionOrigen.Nombre;
            }
        }

        #endregion

        #region Metodos

        //Constructor vacio
        public TarjetaTAPI() { }

        //Constructor basico
        public TarjetaTAPI(int iNumeroLista, string sNumeroTarjeta, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            NumeroTarjeta = sNumeroTarjeta;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }

        //Constructor completo
        public TarjetaTAPI(int iNumeroLista, string sNumeroTarjeta, DateTime dtFechaVencimiento, string sRazonSocial, string sPatente, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            NumeroTarjeta = sNumeroTarjeta;
            Vencimiento = dtFechaVencimiento;
            RazonSocial = sRazonSocial;
            Patente = sPatente;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }


        #endregion
    }

    /// <summary>
    /// Lista de objetos TarjetaTAPI.
    /// </summary>
    [Serializable]
    public class TarjetaTAPIL : List<TarjetaTAPI>
    {
    }

}

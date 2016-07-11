using System;
using System.Collections.Generic;

namespace Telectronica.Facturacion
{

    /// <summary>
    /// Estructura de una identidad de Tipo TarjetaCHIP
    /// </summary>
    [Serializable]
    public class TarjetaCHIP
    {
        #region Propiedades

        //Código de Estación
        public short Estacion { get; set; }

        //Numero de Tarjeta
        public int NumeroInterno { get; set; }

        //Número Externo
        public int NumeroExterno { get; set; }

        //Categoria
        public short Categoria { get; set; }

        //Marca
        public string Marca { get; set; }

        //Patente
        public string Patente { get; set; }

        //NombreCliente
        public string NombreCliente { get; set; }

        //Dispositivo
        public string Dispositivo { get; set; }
 
        //Saldo
        public decimal Saldo { get; set; }

        //Propia
        public bool Propia { get; set; }

        #endregion

        #region Metodos

        //Constructor vacio
        public TarjetaCHIP() { }

        //Constructor completo
        public TarjetaCHIP(short estacion, int numeroInterno, int numeroExterno, short categoria, string marca, string patente, string nombreCliente)
        {
            Estacion = estacion;
            NumeroExterno = numeroExterno;
            NumeroInterno = numeroInterno;
            Categoria = categoria;
            Marca = marca;
            Patente = patente;
            NombreCliente = nombreCliente;
        }


        #endregion
    }

    /// <summary>
    /// Lista de objetos TarjetaChip
    /// </summary>
    [Serializable]
    public class TarjetaCHIPL : List<TarjetaCHIP>
    {
    }

}

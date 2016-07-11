using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace Telectronica.Peaje
{
    #region Nivel2_TrcXFp: Nivel 2 - Transitos por forma de pago

    [Serializable]
    [XmlRootAttribute(ElementName = "Nivel2_TrcXFp", IsNullable = false)]
    public class Nivel2_TrcXFp
    {

        public Nivel2_TrcXFp(string xsTipoTrc, int xiCantidad)
        {
            this.TipoTrc = xsTipoTrc;
            this.Cantidad = xiCantidad;
        }

        public Nivel2_TrcXFp()
        {

        }

        // DEFINICION DE PROPIEDADES

        public string TipoTrc { get; set; }

        public int Cantidad { get; set; }
    }

    [Serializable]
    public class Nivel2_TrcXFpL : List<Nivel2_TrcXFp>
    {

    }
    #endregion

    #region Nivel2_Exento: Nivel 2 - Exentos
    [Serializable]
    [XmlRootAttribute(ElementName = "Nivel2_Exento", IsNullable = false)]
    public class Nivel2_Exento
    {

        public Nivel2_Exento(string xsTipoExento, int xiCantidad)
        {
            this.TipoExento = xsTipoExento;
            this.Cantidad = xiCantidad;
        }

        public Nivel2_Exento()
        {

        }

        // DEFINICION DE PROPIEDADES

        public string TipoExento { get; set; }

        public int Cantidad { get; set; }
    }

    [Serializable]
    public class Nivel2_ExentoL : List<Nivel2_Exento>
    {

    }
    #endregion


    #region Nivel2_Ventas: Nivel 2 - Exentos
    [Serializable]
    [XmlRootAttribute(ElementName = "Nivel2_Ventas", IsNullable = false)]
    public class Nivel2_Venta
    {

        public Nivel2_Venta(string xsTipoVenta, int xiCantidad)
        {
            this.TipoVenta= xsTipoVenta;
            this.Cantidad = xiCantidad;
        }

        public Nivel2_Venta()
        {

        }

        // DEFINICION DE PROPIEDADES

        public string TipoVenta { get; set; }

        public int Cantidad { get; set; }
    }

    [Serializable]
    public class Nivel2_VentasL : List<Nivel2_Venta>
    {

    }
    #endregion

    #region Nivel2_OpeDac: Clase para manejar la información de diferencias entre lo categorizado por el Operador y el Dac

    [Serializable]
    [XmlRootAttribute(ElementName = "Nivel2_OpeDac", IsNullable = false)]
    public class Nivel2_OpeDac
    {

        public Nivel2_OpeDac(int xiCodEstacion, int xiNumeroVia, int xiCatMan, int xiCatDac, int xiCantidad)
        {
            this.CodEstacion = xiCodEstacion;
            this.NumeroVia = xiNumeroVia;
            this.CatManual = xiCatMan;
            this.CatDac = xiCatDac;
            this.Cantidad = xiCantidad;
        }

        public Nivel2_OpeDac()
        {

        }

        // DEFINICION DE PROPIEDADES

        public int CodEstacion { get; set; }                // CODIGO DE ESTACION

        public int NumeroVia { get; set; }                  // NRO DE VIA

        public int CatManual { get; set; }                  // CATEGORIA MANUAL

        public int CatDac { get; set; }                     // CATEGORIA DAC

        public int Cantidad { get; set; }                   // CANTIDAD DE DIFERENCIA

        public string Manual { get; set; }                  // MANUAL

        public string Dac { get; set; }                     // DAC
    }

    [Serializable]
    public class Nivel2_OpeDacL : List<Nivel2_OpeDac>
    {

    }

    #endregion


    #region Nivel2_Diferencias: Clase para manejar la información que se mostrara en la grilla de diferencias del Nivel 2

    [Serializable]
    [XmlRootAttribute(ElementName = "Nivel2_Diferencias", IsNullable = false)]
    public class Nivel2_Diferencias
    {

        public Nivel2_Diferencias(int xiCatOpe, int xiCatDac0, int xiCatDac0Cant, int xiCatDac1, int xiCatDac1Cant, int xiCatDac2, int xiCatDac2Cant, int xiCatDac3, int xiCatDac3Cant, int xiCatDac4, int xiCatDac4Cant, 
                                    int xiCatDac5, int xiCatDac5Cant, int xiCatDac6, int xiCatDac6Cant, int xiCatDac7, int xiCatDac7Cant, int xiCatDac8, int xiCatDac8Cant, int xiCatDac9, int xiCatDac9Cant,
                                    int xiCatDac10, int xiCatDac10Cant, int xiCatDac11, int xiCatDac11Cant, int xiCatDac12, int xiCatDac12Cant, int xiCatDac13, int xiCatDac13Cant, int xiCatDac14, int xiCatDac14Cant,
                                    int xiCatDac15, int xiCatDac15Cant, int xiCatDac16, int xiCatDac16Cant, int xiCatDac17, int xiCatDac17Cant, int xiCatDac18, int xiCatDac18Cant, int xiCatDac19, int xiCatDac19Cant,
                                    int xiCatDac20, int xiCatDac20Cant)
        {
            this.CatOpe = xiCatOpe;
            this.CatDac0 = xiCatDac0;
            this.CatDac0Cant = xiCatDac0Cant;
            this.CatDac1 = xiCatDac1;
            this.CatDac1Cant = xiCatDac1Cant;
            this.CatDac2 = xiCatDac2;
            this.CatDac2Cant = xiCatDac2Cant;
            this.CatDac3 = xiCatDac3;
            this.CatDac3Cant = xiCatDac3Cant;
            this.CatDac4 = xiCatDac4;
            this.CatDac4Cant = xiCatDac4Cant;
            this.CatDac5 = xiCatDac5;
            this.CatDac5Cant = xiCatDac5Cant;
            this.CatDac6 = xiCatDac6;
            this.CatDac6Cant = xiCatDac6Cant;
            this.CatDac7 = xiCatDac7;
            this.CatDac7Cant = xiCatDac7Cant;
            this.CatDac8 = xiCatDac8;
            this.CatDac8Cant = xiCatDac8Cant;
            this.CatDac9 = xiCatDac9;
            this.CatDac9Cant = xiCatDac9Cant;
            this.CatDac10 = xiCatDac10;
            this.CatDac10Cant = xiCatDac10Cant;
            this.CatDac11 = xiCatDac11;
            this.CatDac11Cant = xiCatDac11Cant;
            this.CatDac12 = xiCatDac12;
            this.CatDac12Cant = xiCatDac12Cant;
            this.CatDac13 = xiCatDac13;
            this.CatDac13Cant = xiCatDac13Cant;
            this.CatDac14 = xiCatDac14;
            this.CatDac14Cant = xiCatDac14Cant;
            this.CatDac15 = xiCatDac15;
            this.CatDac15Cant = xiCatDac15Cant;
            this.CatDac16 = xiCatDac16;
            this.CatDac16Cant = xiCatDac16Cant;
            this.CatDac17 = xiCatDac17;
            this.CatDac17Cant = xiCatDac17Cant;
            this.CatDac18 = xiCatDac18;
            this.CatDac18Cant = xiCatDac18Cant;
            this.CatDac19 = xiCatDac19;
            this.CatDac19Cant = xiCatDac19Cant;
            this.CatDac20 = xiCatDac20;
            this.CatDac20Cant = xiCatDac20Cant;
        }

        public Nivel2_Diferencias()
        {

        }

        // DEFINICION DE PROPIEDADES

        public int CatOpe { get; set; }                    // CATEGORIA OPERADOR  //

        public int CatDac0 { get; set; }                   // CATEGORIA DAC
        public int CatDac0Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac1 { get; set; }                   // CATEGORIA DAC
        public int CatDac1Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac2 { get; set; }                   // CATEGORIA DAC
        public int CatDac2Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac3 { get; set; }                   // CATEGORIA DAC
        public int CatDac3Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac4 { get; set; }                   // CATEGORIA DAC
        public int CatDac4Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac5 { get; set; }                   // CATEGORIA DAC
        public int CatDac5Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac6 { get; set; }                   // CATEGORIA DAC
        public int CatDac6Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac7 { get; set; }                   // CATEGORIA DAC
        public int CatDac7Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac8 { get; set; }                   // CATEGORIA DAC
        public int CatDac8Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac9 { get; set; }                   // CATEGORIA DAC
        public int CatDac9Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac10 { get; set; }                   // CATEGORIA DAC
        public int CatDac10Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac11 { get; set; }                   // CATEGORIA DAC
        public int CatDac11Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac12 { get; set; }                   // CATEGORIA DAC
        public int CatDac12Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac13 { get; set; }                   // CATEGORIA DAC
        public int CatDac13Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac14 { get; set; }                   // CATEGORIA DAC
        public int CatDac14Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac15 { get; set; }                   // CATEGORIA DAC
        public int CatDac15Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac16 { get; set; }                   // CATEGORIA DAC
        public int CatDac16Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac17 { get; set; }                   // CATEGORIA DAC
        public int CatDac17Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac18 { get; set; }                   // CATEGORIA DAC
        public int CatDac18Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac19 { get; set; }                   // CATEGORIA DAC
        public int CatDac19Cant { get; set; }               // CANTIDAD DE DIFERENCIA

        public int CatDac20 { get; set; }                   // CATEGORIA DAC
        public int CatDac20Cant { get; set; }               // CANTIDAD DE DIFERENCIA
         
        public string Ope { get; set; }                    // CATEGORIA OPERADOR  //
        public string OpeDac { get; set; }                 // STRING CATEGORIA OPERADOR  //
    }

    [Serializable]
    public class Nivel2_DiferenciasL : List<Nivel2_Diferencias>
    {

    }

    #endregion


}


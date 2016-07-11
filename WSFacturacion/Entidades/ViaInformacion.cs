using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region VIAINFORMACION: Clase para manejar la información de la vía en el Nivel 0

    [Serializable]
    [XmlRootAttribute(ElementName = "ViaInformacion", IsNullable = false)]
    public class ViaInformacion
    {

        public ViaInformacion(int iCodEstacion, int iNumeroVia, string sNombreVia, string sOperadorId, string sOperadorNombre, string sEstado, DateTime dFechaApertura, DateTime dFechaCierre, int iParte, int iTurno, string sSenti,
                                decimal dPordi, ViaInformacionTotalL totalPorForPago)
        {
            CodEstacion = iCodEstacion;
            NumeroVia = iNumeroVia;
            NombreVia = sNombreVia;
            OperadorId = sOperadorId;
            OperadorNombre = sOperadorNombre;
            Estado = sEstado;
            FechaApertura = dFechaApertura;
            FechaCierre = dFechaCierre;
            Parte = iParte;
            Turno = iTurno;
            Senti = sSenti;
            Pordi = dPordi;
            TotalXForPago = totalPorForPago;
        }

        public ViaInformacion()
        {
        }

        // DEFINICION DE PROPIEDADES

        public int CodEstacion { get; set; }                            // CODIGO DE ESTACION
        
        public int NumeroVia { get; set; }                              // NRO DE VIA

        public string NombreVia { get; set; }                           // NOMBRE DE LA VIA

        public string OperadorId { get; set; }                          // ID DEL OPERADOR

        public string OperadorNombre { get; set; }                      // NOMBRE DEL OPERADOR

        public string Estado { get; set; }                              // ESTADO DE LA VIA

        public DateTime? FechaApertura { get; set; }                    // FECHA DE APERTURA DEL TURNO

        public DateTime? FechaCierre { get; set; }                      // FECHA DE CIERRE DEL TURNO

        public int Parte { get; set; }                                  // NRO DE PARTE

        public int Turno { get; set; }                                  // NRO DE TURNO

        public string Senti { get; set; }                               // SENTIDO DE LA VIA EN EL TURNO

        public decimal Pordi { get; set; }                              // PORCENTAJE DE DISCREPANCIA DEL BLOQUE

        public ViaInformacionTotalL TotalXForPago { get; set; }         // LISTA CON TOTALES POR FORMA DE PAGO
    }

    [Serializable]
    public class ViaInformacionL : List<ViaInformacion>
    {
    }

    #endregion

    #region VIAINFORMACIONTOTALES: Clase para manejar la información de totales de la vía en el Nivel 0

    [Serializable]
    [XmlRootAttribute(ElementName = "ViaInformacionTotal", IsNullable = false)]
    public class ViaInformacionTotal
    {
        public ViaInformacionTotal(int iCodEstacion, int iNumeroVia, string sFormaDePago, int iCantidad)
        {
            CodEstacion = iCodEstacion;
            NumeroVia = iNumeroVia;
            FormaDePago = sFormaDePago;
            Cantidad = iCantidad;
        }

        public ViaInformacionTotal()
        {
        }

        // DEFINICION DE PROPIEDADES

        public int CodEstacion { get; set; }                    // CODIGO DE ESTACION

        public int NumeroVia { get; set; }                      // NRO DE VIA

        public string FormaDePago { get; set; }                 // FORMA DE PAGO

        public int Cantidad { get; set; }                       // CANTIDAD DE TRANSITOS PARA LA FORMA DE PAGO
    }

    [Serializable]
    public class ViaInformacionTotalL : List<ViaInformacionTotal>
    {
    }

    #endregion

    #region VIAINFORMACIONULTIMOBLOQUE: Clase para manejar la información del último bloque de la vía, con información de totales incluída

    [Serializable]
    [XmlRootAttribute(ElementName = "ViaInformacionUltimoBloque", IsNullable = false)]
    public class ViaInformacionUltimoBloque
    {

        public ViaInformacionUltimoBloque(int xiCodEstacion, int xiNumeroVia, string xsNombreVia, string xsOperadorId, string xsOperadorNombre, string xsEstado, DateTime xdFechaApertura, DateTime xdFechaCierre, int xiParte, int xiTurno,
                                           string xsSenti, decimal xdPordi, string xsForPag1, string xiCant1, string xsForPag2, string xiCant2, string xsForPag3, string xiCant3, string xsForPag4, string xiCant4,
                                            string xsForPag5, string xiCant5, string xsForPag6, string xiCant6, string xsForPag7, string xiCant7, string xsForPag8, string xiCant8, string xsForPag9, string xiCant9,
                                            string xsForPag10, string xiCant10,
                                            string FP1, string FP2, string FP3, string FP4, string FP5, string FP6, string FP7, string FP8, string FP9, string FP10, int xiTotalTransitos,
                                            string FP11, string FP12, string FP13, string FP14, string FP15, string FP16, string FP17, string FP18, string FP19, string FP20)
        {
            CodEstacion = xiCodEstacion;
            NumeroVia = xiNumeroVia;
            NombreVia = xsNombreVia;
            OperadorId = xsOperadorId;
            OperadorNombre = xsOperadorNombre;
            Estado = xsEstado;
            FechaApertura = xdFechaApertura;
            FechaCierre = xdFechaCierre;
            Parte = xiParte;
            Turno = xiTurno;
            Senti = xsSenti;
            Pordi = xdPordi;
            FormaPago1 = xsForPag1;
            Cant1 = xiCant1;
            FormaPago1 = xsForPag2;
            Cant1 = xiCant2;
            FormaPago1 = xsForPag3;
            Cant1 = xiCant3;
            FormaPago1 = xsForPag4;
            Cant1 = xiCant4;
            FormaPago1 = xsForPag5;
            Cant1 = xiCant5;
            FormaPago1 = xsForPag6;
            Cant1 = xiCant6;
            FormaPago1 = xsForPag7;
            Cant1 = xiCant7;
            FormaPago1 = xsForPag8;
            Cant1 = xiCant8;
            FormaPago1 = xsForPag9;
            Cant1 = xiCant9;
            FormaPago1 = xsForPag10;
            Cant1 = xiCant10;

            this.FP1 = FP1;
            this.FP2 = FP2;
            this.FP3 = FP3;
            this.FP4 = FP4;
            this.FP5 = FP5;
            this.FP6 = FP6;
            this.FP7 = FP7;
            this.FP8 = FP8;
            this.FP9 = FP9;
            this.FP10 = FP10;
            this.FP11 = FP11;
            this.FP12 = FP12;
            this.FP13 = FP13;
            this.FP14 = FP14;
            this.FP15 = FP15;
            this.FP16 = FP16;
            this.FP17 = FP17;
            this.FP18 = FP18;
            this.FP19 = FP19;
            this.FP20 = FP20;

            this.TotalTransitos = xiTotalTransitos;
        }

        public ViaInformacionUltimoBloque()
        {

        }

        // DEFINICION DE PROPIEDADES

        public int CodEstacion { get; set; }                    // CODIGO DE ESTACION

        public int NumeroVia { get; set; }                      // NRO DE VIA

        public string NombreVia { get; set; }                  // NOMBRE DE LA VIA

        public string OperadorId { get; set; }                  // ID DEL OPERADOR

        public string OperadorNombre { get; set; }              // NOMBRE DEL OPERADOR

        public string Estado { get; set; }                      // ESTADO DE LA VIA

        public DateTime? FechaApertura { get; set; }            // FECHA DE APERTURA DEL TURNO

        public DateTime? FechaCierre { get; set; }              // FECHA DE CIERRE DEL TURNO

        public int Parte { get; set; }                          // NRO DE PARTE

        public int Turno { get; set; }                          // NRO DE TURNO

        public string Senti { get; set; }                       // SENTIDO DE LA VIA EN EL TURNO

        public decimal Pordi { get; set; }                      // PORCENTAJE DE DISCREPANCIA DEL BLOQUE

        public string FormaPago1 { get; set; }
        public string Cant1 { get; set; }

        public string FormaPago2 { get; set; }
        public string Cant2 { get; set; }

        public string FormaPago3 { get; set; }
        public string Cant3 { get; set; }

        public string FormaPago4 { get; set; }
        public string Cant4 { get; set; }

        public string FormaPago5 { get; set; }
        public string Cant5 { get; set; }

        public string FormaPago6 { get; set; }
        public string Cant6 { get; set; }

        public string FormaPago7 { get; set; }
        public string Cant7 { get; set; }

        public string FormaPago8 { get; set; }
        public string Cant8 { get; set; }

        public string FormaPago9 { get; set; }
        public string Cant9 { get; set; }

        public string FormaPago10 { get; set; }
        public string Cant10 { get; set; }

        public string FP1 { get; set; }
        public string FP2 { get; set; }
        public string FP3 { get; set; }
        public string FP4 { get; set; }
        public string FP5 { get; set; }
        public string FP6 { get; set; }
        public string FP7 { get; set; }
        public string FP8 { get; set; }
        public string FP9 { get; set; }
        public string FP10 { get; set; }
        public string FP11 { get; set; }
        public string FP12 { get; set; }
        public string FP13 { get; set; }
        public string FP14 { get; set; }
        public string FP15 { get; set; }
        public string FP16 { get; set; }
        public string FP17 { get; set; }
        public string FP18 { get; set; }
        public string FP19 { get; set; }
        public string FP20 { get; set; }

        public int TotalTransitos { get; set; }
    }

    [Serializable]
    public class ViaInformacionUltimoBloqueL : List<ViaInformacionUltimoBloque>
    {
    }

    #endregion
}


using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Runtime.CompilerServices;
using Telectronica.Utilitarios;


namespace Telectronica.Peaje
{
    public class VISABs
    {

        public enum enmTipoArcivoVisa
        {
            enmBad,
            enmCielo,
            enmVisaNet
        }

        /// <summary>
        /// Procesa el archivo TXT con el detalle de transitos visa.
        /// </summary>
        /// <param name="archivo"></param>
        public static bool ProcesarArchivoVisaTXT(string archivo)
        {
            
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                string line ;
                
                
                try
                {

                    using (StreamReader reader = new StreamReader(archivo))
                    {

                        //Leo la primera linea
                        line = reader.ReadLine();
                        string NumeroROencabezado = "";
                        DateTime? FechaEncabezado = null;
                        DateTime FechaDesde = DateTime.Parse("31/12/9999"), FechaHasta = DateTime.Parse("01/01/1900");
                        //Proceso primera linea
                        if (line!=null)
                        {
                            //string FechaCompleta= line.Substring(2, 8) + line.Substring(10, 2) + line.Substring(12, 2) +line.Substring( 14, 2)
                            FechaEncabezado = DateTime.ParseExact(line.Substring(2, 14), "yyyyMMddHHmmss",CultureInfo.InvariantCulture);
                            NumeroROencabezado = line.Substring(30, 6);

                        }

                        while ((line = reader.ReadLine()) != null)
                        {
                            if (line.Length > 0)
                            {
                                string tipo = line.Substring(1, 1);

                                VISA visa = new VISA();
                                if (tipo == "2")
                                {
                                
                                    visa.NumeroRegistro = Convert.ToInt32(line.Substring(1, 7));
                                    visa.NumeroConcentrador = Convert.ToInt32(line.Substring(8, 3));
                                    visa.NumeroPinPad = Convert.ToInt32(line.Substring(11, 3));
                                    //Fecha que se lee en el archivo
                                    visa.FechaVisaPinPad = DateTime.ParseExact(line.Substring(14, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                                    visa.Monto = Convert.ToDecimal(line.Substring(28, 8))/100;
                                    visa.NumeroCartao = line.Substring(36, 16);
                                    visa.NumeroTran = Convert.ToInt32(line.Substring(58, 6));
                                    visa.FechaCompra = null;
                                    visa.EstadoOperacion = "";
                                    visa.MotivoRechazo = null;
                                    visa.NumeroRO = NumeroROencabezado;

                                    if (visa.FechaVisaPinPad < FechaDesde)
                                        FechaDesde = visa.FechaVisaPinPad;
                                    if (visa.FechaVisaPinPad > FechaHasta)
                                        FechaHasta = visa.FechaVisaPinPad;

                                    VISADt.GrabarComprobanteVenta(conn, visa, null);
    
                                }
                                if (tipo == "9")
                                {
                                    visa.NumeroRO = NumeroROencabezado;
                                    
                                    string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
                                    VISADt.GrabarSecuenciaRO(conn, visa, FechaEncabezado,nombreArchivo, FechaDesde, FechaHasta);

                                }
                            


                            }
                        }


                    }

                    conn.Finalizar(true);

                }
                catch (Exception ex)
                {
                    return false;
                    
                }

            }


            //Si el archivo fue procesado completamente y correcto se llega hasta este punto.
            return true;
        }

        ///*******************************************************************************************************************
        /// <summary>
        /// Procesa el archivo de VISA
        /// </summary>
        /// <param name="archivo">Nombre del archivo</param>
        /// <param name="secuencia">Secuencia que viene en el archivo</param>
        /// <param name="administradora">No se usa</param>
        /// <param name="cantReg"></param>
        /// <returns>int  - Resultado.</returns>
        ///*******************************************************************************************************************
        public static bool ProcesarArchivoVisa(string archivo, ref int cantReg, enmTipoArcivoVisa tipoArchivo,ref bool completo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                string line;
                int i = 0, SecuenciaInsertada = -1, Simbolo = 1, CantidadReg = 0;
                string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
                bool ventas= false;
                

                using (StreamReader reader = new StreamReader(archivo))
                {
                    bool procesarLote = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            
                            VISA VS = new VISA();
                            string TipoRegistro = Convert.ToString(line.Substring(0, 1));

                            if (TipoRegistro == "0") // HEADER
                            {
                                if( tipoArchivo == enmTipoArcivoVisa.enmCielo )
                                {
                                    VS.NombreArchivo = nombreArchivo;
                                    VS.FechaProcesamiento = DateTime.Now;
                                    VS.PeriodoInicial = DateTime.ParseExact(line.Substring(19, 8).ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                                    VS.PeriodoFinal = DateTime.ParseExact(line.Substring(27, 8).ToString()+ " 23:59:59", "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
                                    VS.Secuencia = line.Substring(35, 7).ToString();    //38,7
                                    VS.Usuario = "Consumidor-OSAs";
                                }
                                else
                                {
                                    VS.NombreArchivo = nombreArchivo;
                                    VS.FechaProcesamiento = DateTime.Now;
                                    VS.PeriodoInicial = DateTime.ParseExact(line.Substring(19, 8).ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                                    VS.PeriodoFinal = DateTime.ParseExact(line.Substring(27, 8).ToString()+ " 23:59:59", "yyyyMMdd HH:mm:ss", CultureInfo.InvariantCulture);
                                    VS.Secuencia = line.Substring(38, 7).ToString();    //38,7
                                    VS.Usuario = "Consumidor-OSAs";
                                }

                                VISADt.GrabarCabeceraVisa(conn, VS, out SecuenciaInsertada);
                            }

                            else if (TipoRegistro == "1") // Detalle del RO
                            {
                               procesarLote = true;

                                if (tipoArchivo == enmTipoArcivoVisa.enmCielo)
                                {

                                    ventas = true;

                                    VS.NumeroRO = line.Substring(12, 6).ToString();
                                    VS.EstabVisa = line.Substring(1, 10).ToString();
                                    VS.FechaDeposito = DateTime.ParseExact(line.Substring(25, 6).ToString(),
                                        "yyMMdd", CultureInfo.InvariantCulture); //38,6
                                    VS.FechaPago = DateTime.ParseExact(line.Substring(31, 6).ToString(), "yyMMdd",
                                        CultureInfo.InvariantCulture); //44,6

                                    if (line.Substring(37, 6).ToString() != "000000") //50,6
                                        VS.FechaEnvioBanco = DateTime.ParseExact(line.Substring(37, 6).ToString(),
                                            "yyMMdd", CultureInfo.InvariantCulture);
                                    else
                                        VS.FechaEnvioBanco = null;

                                    Simbolo = (line.Substring(43, 1) == "-" ? -1 : 1);
                                    VS.ValorBruto = (Convert.ToDecimal(line.Substring(44, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(57, 1) == "-" ? -1 : 1);
                                    VS.ValorComision = (Convert.ToDecimal(line.Substring(58, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(71, 1) == "-" ? -1 : 1);
                                    VS.ValorRechazado = (Convert.ToDecimal(line.Substring(72, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(85, 1) == "-" ? -1 : 1);
                                    VS.ValorNeto = (Convert.ToDecimal(line.Substring(86, 13))/100)*Simbolo;

                                    VS.CantCVAcetpadas = Convert.ToInt32(line.Substring(124, 6));
                                    VS.CantCVRechazadas = Convert.ToInt32(line.Substring(132, 6));
                                    VS.StatusVenta = Convert.ToInt32(line.Substring(36, 2)); //????
                                }
                                else
                                {
                                    //tipoDetalle "C":PAGOS, "D" :Ventas  -->SOLO RECIBIMOS VENTAS 
                                    string tipoDetalle = line.Substring(149, 1);
                                    ventas = (tipoDetalle == "D");

                                    VS.NumeroRO = line.Substring(12, 6).ToString();
                                    VS.EstabVisa = line.Substring(1, 10).ToString();
                                    VS.FechaDeposito = DateTime.ParseExact(line.Substring(38, 6).ToString(),
                                        "yyMMdd", CultureInfo.InvariantCulture); //38,6
                                    VS.FechaPago = DateTime.ParseExact(line.Substring(44, 6).ToString(), "yyMMdd",
                                        CultureInfo.InvariantCulture); //44,6

                                    if (line.Substring(50, 6).ToString() != "000000") //50,6
                                        VS.FechaEnvioBanco = DateTime.ParseExact(line.Substring(50, 6).ToString(),
                                            "yyMMdd", CultureInfo.InvariantCulture);
                                    else
                                        VS.FechaEnvioBanco = null;

                                    Simbolo = (line.Substring(56, 1) == "-" ? -1 : 1);
                                    VS.ValorBruto = (Convert.ToDecimal(line.Substring(57, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(70, 1) == "-" ? -1 : 1);
                                    VS.ValorComision = (Convert.ToDecimal(line.Substring(71, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(84, 1) == "-" ? -1 : 1);
                                    VS.ValorRechazado = (Convert.ToDecimal(line.Substring(85, 13))/100)*Simbolo;

                                    Simbolo = (line.Substring(98, 1) == "-" ? -1 : 1);
                                    VS.ValorNeto = (Convert.ToDecimal(line.Substring(99, 13))/100)*Simbolo;

                                    VS.CantCVAcetpadas = Convert.ToInt32(line.Substring(135, 6));
                                    VS.CantCVRechazadas = Convert.ToInt32(line.Substring(141, 6));
                                    VS.StatusVenta = Convert.ToInt32(line.Substring(36, 2)); //????
                                }

                                if (ventas)
                                {
                                    try
                                    {
                                        //Si el lote es uno ya procesado anteriormente se ignora el detalle que sigue en el RET (tipo 2)
                                        VISADt.GrabarPagosVC(conn, VS, SecuenciaInsertada,ref procesarLote);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Loguear(" Error: Archivo RET " + nombreArchivo + " RO:"+VS.NumeroRO+ ". Falta recepcion archivo TXT", "Consumidor");
                                        procesarLote = false;
                                        completo = false;
                                        
                                        
                                    }
                                }
                                    
                            }
                            else if (TipoRegistro == "2" && ventas && procesarLote) // Detalle del Comprobante de Venta
                            {
                                
                                    if (tipoArchivo == enmTipoArcivoVisa.enmCielo)
                                    {
                                        VS.FechaCompra = DateTime.ParseExact(line.Substring(37, 8).ToString(),
                                            "yyyyMMdd", CultureInfo.InvariantCulture);
                                        VS.EstabVisa = line.Substring(1, 10).ToString();
                                        VS.NumeroRO = line.Substring(12, 6);
                                        VS.NumeroCartao = line.Substring(18, 16).ToString();
                                        VS.EstadoOperacion = line.Substring(34, 3).ToString();
                                        Simbolo = (line.Substring(45, 1) == "-" ? -1 : 1);
                                        VS.Monto = (Convert.ToDecimal(line.Substring(46, 13))/100)*Simbolo;
                                        VS.MotivoRechazo = line.Substring(72, 20).ToString();
                                        VS.NumeroTran = Convert.ToInt32(line.Substring(92, 6));
                                    }
                                    else
                                    {
                                        VS.FechaCompra = DateTime.ParseExact(line.Substring(37, 8).ToString(),
                                            "yyyyMMdd", CultureInfo.InvariantCulture);
                                        VS.EstabVisa = line.Substring(1, 10).ToString();
                                        VS.NumeroRO = line.Substring(12, 6);
                                        VS.NumeroCartao = line.Substring(18, 16).ToString();
                                        VS.EstadoOperacion = line.Substring(34, 3).ToString();
                                        Simbolo = (line.Substring(45, 1) == "-" ? -1 : 1);
                                        VS.Monto = (Convert.ToDecimal(line.Substring(46, 13))/100)*Simbolo;
                                        VS.MotivoRechazo = line.Substring(63, 30).ToString();
                                        VS.NumeroTran = Convert.ToInt32(line.Substring(139, 6));

                                    }
                                    try
                                    {
                                        VISADt.updComprobanteVenta(conn, VS, SecuenciaInsertada);
                                    }
                                    catch (Exception)
                                    {
                                        Log.Loguear(" Error: Archivo RET " + nombreArchivo + " RO:" + VS.NumeroRO + 
                                            
                                                    " Transaccion: "+VS.NumeroTran+
                                            ".  Transaccion no encontrada", "Consumidor");

                                        return false;
                                        
                                    }
                                 
                                
                            }

                            else if (TipoRegistro == "9") // Detalle Trhiller
                            {
                                CantidadReg = Convert.ToInt32(line.Substring(1, 11));

                                VISADt.UpdCabeceraVisa(conn, CantidadReg, SecuenciaInsertada);
                            }

                            i++;

                        }
                    }
                }

                cantReg = i - 2;

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                return true;

            }
        }


        ///***********************************************************************************************
        /// <summary>
        /// Verifico si el archivo RET de VISA ya se encontraba procesado
        /// </summary>
        /// <param name="Archivo">Nombre del archivo</param>
        /// <returns>bool</returns>
        ///***********************************************************************************************
        public static bool EsArchivoYaProcesado(string Archivo)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                return VISADt.EsArchivoYaProcesado(conn, Archivo);
            }
        }


        ///**********************************************************************************
        /// <summary>
        /// Verifica el encabezado del archivo (Por el momento no hay nada)
        /// </summary>
        /// <param name="archivo">Archivo</param>
        /// <returns>Bool</returns>
        /// *********************************************************************************
        public static enmTipoArcivoVisa VerificarArchivoRET(string archivo)
        {

            string extension = Path.GetExtension(archivo).Replace(".", "");
            string nombreArchivo = Path.GetFileNameWithoutExtension(archivo);
            enmTipoArcivoVisa Valid = enmTipoArcivoVisa.enmBad;

                using (StreamReader reader = new StreamReader(archivo))
                {
                    string line;
                    int i = 0;
                    int CantRegArch = 0;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.Length > 0)
                        {
                            VISA VS = new VISA();
                            string TipoRegistro = Convert.ToString(line.Substring(0, 1));

                            if (TipoRegistro == "0")
                            {
                                string Tipo = line.Substring(35, 3).ToString();
                                if (Tipo == "LIQ" && line.Substring(45,7) == "VISANET" )
                                {
                                    Valid = enmTipoArcivoVisa.enmVisaNet;
                                }
                                else if( line.Substring(42,5) == "CIELO" )
                                {
                                    Valid = enmTipoArcivoVisa.enmCielo;
                                }
                                else
                                {
                                    Valid = enmTipoArcivoVisa.enmBad;
                                    break;
                                }
                            }

                            else if (TipoRegistro == "9") // Detalle Trhiller
                            {
                                CantRegArch = Convert.ToInt32(line.Substring(1, 11));
                            }

                            if (TipoRegistro != "0" && TipoRegistro != "9")
                                i++;

                        }
                    }
                    if (CantRegArch != i)
                        Valid = enmTipoArcivoVisa.enmBad;
                }
            
            return Valid;
        }




       
    }
}


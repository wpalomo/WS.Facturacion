using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Telectronica.Peaje
{
    public static class PexBs
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="admin"></param>
        public static void delTags(int admin)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.delTags(conn,admin);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public static void delTag(PexTag tag)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.delTag(tag, conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public static void addTag(PexTag tag,char completo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                PexDt.addTag(tag, conn,completo);
                
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public static void prepararProcesoTags()
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                PexDt.prepararProcesoTags(conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        public static void finalizarProcesoTags(int secuencia)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                PexDt.finalizarProcesoTags(conn, secuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="admin"></param>
        public static void delListasNegras(int admin)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.delListasNegras(conn, admin);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }//iniciamos una transaccion

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ln"></param>
        public static void delListaNegra(PexListaNegra ln)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.delListaNegra(ln, conn);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ln"></param>
        public static void addListaNegra(PexListaNegra ln, char completo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.addListaNegra(ln, conn,completo);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Administradora"></param>
        /// <param name="Secuencia"></param>
        /// <param name="Tipo"></param>
        /// <param name="fechaSecuencia"></param>
        public static void addSeclis(int Administradora, int Secuencia, string Tipo, DateTime fechaSecuencia, string ListCompl)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                PexDt.addSeclis(conn, Administradora, Secuencia, Tipo, fechaSecuencia, ListCompl);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }

        }

        /// <summary>
        /// Guarda la auditoria de las importaciones realizadas
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="secuencia"></param>
        /// <param name="archivo"></param>
        /// <param name="cantReg"></param>
        /// <param name="estado"></param>
        /// <param name="motivoError"></param>
        public static void addAuditoriaImportacion(DateTime fecha, string tipo, int secuencia, string archivo, int cantReg, string estado, string motivoError)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);
                
                PexDt.addAuditoriaImportacion(conn, fecha, tipo, secuencia, archivo, cantReg, estado, motivoError);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        public static void anularTransitoPex(PexTransito transito)
        {            
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                PexDt.anularTransitoPex(conn, transito);
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                //Grabamos auditoria                    
                //Auditoria
                using (Conexion connAud = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoPex(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(transito),
                                                            getAuditoriaDescripcion(transito, "Anular")),
                                                            connAud);
                }
            }
        }

        public static void perderTransitoPex(PexTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                PexDt.perderTransitoPex(conn, transito);
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                //Grabamos auditoria                    
                //Auditoria
                using (Conexion connAud = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoPex(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(transito),
                                                            getAuditoriaDescripcion(transito, "Perdida")),
                                                            connAud);
                }
            }
        }

        public static void deshacerTransitoPex(PexTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                PexDt.deshacerTransitoPex(conn, transito);                
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                //Grabamos auditoria                    
                //Auditoria
                using (Conexion connAud = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoPex(),
                                                            "R",
                                                            getAuditoriaCodigoRegistro(transito),
                                                            getAuditoriaDescripcion(transito, "Perdida")),
                                                            connAud);
                }
            }
        }

        public static void reenviarTransitoPex(PexTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);                
                PexDt.reenviarTransitoPex(conn, transito);
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                //Grabamos auditoria                    
                //Auditoria
                using (Conexion connAud = new Conexion())
                {
                    //conn.ConectarGSTThenPlaza();
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoPex(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(transito),
                                                            getAuditoriaDescripcion(transito, "Reenviar")),
                                                            connAud);
                }
            }
        }

        public static void modificarTransitoPex(PexTransito transito)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidado(true);
                    PexDt.modificarTransitoPex(conn, transito);
                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);


                    //Grabamos auditoria                    
                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        //conn.ConectarGSTThenPlaza();
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                        //Grabamos auditoria
                        
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoPex(),
                                                                "M",
                                                                getAuditoriaCodigoRegistro(transito),
                                                                getAuditoriaDescripcion(transito, "Modificación")),
                                                                connAud);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Administradora"></param>
        /// <returns></returns>
        public static PexTransitoL getTransitos(int Administradora, int estacion)
        {            
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {                
                conn.ConectarConsolidado(false);

                //Eliminamos el tag
                 return PexDt.getTransitos(conn, Administradora, estacion);                
            }

        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeroSecuencia"></param>
        /// <param name="status"></param>
        /// <param name="estacion"></param>
        /// <param name="numeroVia"></param>
        /// <param name="fecha"></param>
        /// <param name="evento"></param>
        public static void addTransitoEnviado(int numeroSecuencia, string status, int estacion, int numeroVia, DateTime fecha, int evento,int admin)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.addTransitoEnviado(conn, numeroSecuencia, status, estacion, numeroVia, fecha, evento, admin);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }

        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        public static void setTransitosProcesados(int estacion, DateTime? fecha)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.setTransitosProcesados(conn, estacion, fecha);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        public static void addSecuenciaEnviada(PexSecuencia secuencia)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.addSecuenciaEnviada(conn, secuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oProtocolo"></param>
        public static void updProtocoloTecnico(PexProtocoloTecnico oProtocolo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.updProtocoloTecnico(conn, oProtocolo);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }



        ///***************************************************************************************
        /// <summary>
        /// devuelve uan lista de transitos que pex a rechazado
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="rechazo"></param>
        /// <param name="iCantRows"></param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        ///***************************************************************************************
        public static PexTransitoL getTransitosRechazados(DateTime fechaDesde, DateTime fechaHasta, int? estacion, int? administradoraTag, char estado, string codigoRechazo, int xiCantRows, ref bool llegoAlTope)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                return PexDt.getTransitosRechazados(conn, fechaDesde, fechaHasta, estacion, administradoraTag, estado, codigoRechazo, xiCantRows, ref llegoAlTope);
            }
        }
        
        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oProtocolo"></param>
        public static void updProtocoloFinanciero(int secu, int admtag, DateTime fechaRecibido, decimal totalAceptado, decimal totalRechazado)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.updProtocoloFinanciero(conn, secu,admtag, fechaRecibido, totalAceptado,totalRechazado);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }*/

        /// <summary>
        /// Obtiene el proximo numero de secuencia a usar de los archivos TRN y TAF
        /// </summary>
        /// <param name="tipo">TAF o TRN</param>
        /// <returns>Numero Secuencia a utilizar</returns>
        public static int getSecuencia(string tipo, int administradora)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return PexDt.getSecuencia(conn, tipo, administradora);
            }
        }

        /// <summary>
        /// Obtiene el ultimo numero de secuencia de los archivos recibidos
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        public static int getSeclis(string tipo, int administradora)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                return PexDt.getSeclis(conn, tipo, administradora);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="estacion"></param>
        /// <returns></returns>
        public static PexCambioTarifaL getCambioTarifas(int estacion, int admtag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return PexDt.getCambioTarifas(conn, estacion, admtag);
            }
        }

        /// <summary>
        /// Obtiene la lista de tarifas
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static PexTarifaL getTarifas(int estacion, DateTime fecha)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return PexDt.getTarifas(conn, estacion, fecha);
            }
        }

        public static void addAlerta(int secuencia, int proxSecuencia, string extArchivo, string descr)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                PexDt.addAlerta(conn, secuencia, proxSecuencia, extArchivo,descr);
            }
        }

        public static void clrAlerta(int secuenciaRec, string extArchivo)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                PexDt.clrAlerta(conn, secuenciaRec, extArchivo);
            }
        }

        public static void prepararProcesoListaNegra()
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                PexDt.prepararProcesoListaNegra(conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        public static void finalizarProcesoListaNegra(int secuencia)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                PexDt.finalizarProcesoListaNegra(conn, secuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Obtiene el numero de secuencia completo
        /// </summary>
        /// <param name="tipo">TAF o TRN</param>
        /// <returns>Numero Secuencia Completa</returns>
        public static int getSecuenciaCompleta(int secuencia, DateTime fecha)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return PexDt.getSecuenciaCompleta(conn,secuencia, fecha);
            }
        }

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="administradora"></param>
        public static void delTRFCCO(int secuencia, int administradora)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.delTRFCCO(conn, secuencia, administradora);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }*/

        /// <summary>
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="secuencia"></param>
        /// <param name="administradora"></param>
        /// <param name="cantReg"></param>
        public static void ProcesarArchivoFinanciero(string archivo, int secuencia, int administradora, ref int cantReg)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                PexDt.delPropag(conn, secuencia, administradora);
                PexDt.delTRFCCO(conn, secuencia, administradora);

                string line;
                int i = 0;
                int secEnviada = 0;
                DateTime fechaRecibido = DateTime.Now;
                decimal totalAceptado = 0;
                decimal totalRechazado = 0;


                using (StreamReader reader = new StreamReader(archivo))
                {

                    while ((line = reader.ReadLine()) != null)
                    {
                        PexProtocoloFinanciero pf = new PexProtocoloFinanciero();

                        pf.Administradora = administradora;
                        pf.Secuencia = secuencia;

                        if (i == 0 && line.Length > 1)
                        {
                            secEnviada = Convert.ToInt32(line.Substring(11, 5));
                            //totalAceptado = Convert.ToDecimal(line.Substring(36, 10) + "." + line.Substring(46, 2));
                            //totalRechazado = Convert.ToDecimal(line.Substring(48, 10) + "." + line.Substring(58, 2));
                            totalAceptado = Convert.ToDecimal(line.Substring(36, 12)) / 100;
                            totalRechazado = Convert.ToDecimal(line.Substring(48, 12)) / 100;
                            string sFecha = line.Substring(16,14);
                            fechaRecibido  = Convert.ToDateTime(sFecha.Substring(0, 4) + "/" + sFecha.Substring(4, 2) + "/" + sFecha.Substring(6, 2)
                                                        + " " + sFecha.Substring(8, 2) + ":" + sFecha.Substring(10, 2) + ":" + sFecha.Substring(12, 2));

                        }

                        pf.SecuenciaEnviada = secuencia;

                        if (i != 0 && line.Length > 1)
                        {
                            pf.Tipo = line.Substring(0, 2);

                            if (pf.Tipo == "DR")
                            {
                                pf.NumeroRegistro = Convert.ToInt32(line.Substring(2, 6));
                                pf.PaisEmisor = line.Substring(8, 4);
                                pf.EmisorTag = line.Substring(12, 5);
                                pf.NumeroTag = line.Substring(17, 10);
                                pf.FechaRegEnviado = Convert.ToDateTime(line.Substring(27, 4) + "/" + line.Substring(31, 2) + "/" + line.Substring(33, 2)
                                                    + " " + line.Substring(35, 2) + ":" + line.Substring(37, 2) + ":" + line.Substring(39, 2));
                                pf.CodigoPlaza = line.Substring(41, 4);
                                pf.CodigoPista = line.Substring(45, 3);
                                //pf.ValorPasada = Convert.ToDecimal(line.Substring(48, 6) + "." + line.Substring(54, 2)); //ver que esta bien esto
                                pf.ValorPasada = Convert.ToDecimal(line.Substring(48, 8)) / 100;
                                pf.CodRetorno = line.Substring(56, 2);
                                pf.Placa = line.Substring(58, 7);
                                pf.Administradora = administradora;

                                PexDt.addRechazosProtocoloFinanciero(conn, pf);
                            }

                            if (pf.Tipo == "DF")
                            {
                                pf.FechaPago = Convert.ToDateTime(line.Substring(2, 4) + "/" + line.Substring(6, 2) + "/" + line.Substring(8, 2));
                                //pf.ValorPago = Convert.ToDecimal(line.Substring(10, 10) + "." + line.Substring(20, 2));
                                pf.ValorPago = Convert.ToDecimal(line.Substring(10, 12)) / 100;

                                PexDt.addPropag(conn, pf);
                            }

                        }

                        i++;
                    }

                    PexDt.updProtocoloFinanciero(conn, secuencia, administradora, fechaRecibido, totalAceptado, totalRechazado);

                    cantReg = i - 1;

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
        }


        ///***************************************************************************************
        /// <summary>
        ///  Generacion del archivo de TRN
        /// </summary>
        /// <param name="Administradora"></param>
        /// <param name="PathTemporal"></param>
        /// <param name="PathDestino"></param>
        /// <param name="valorTotal"></param>
        /// <param name="cantRegistros"></param>
        /// <param name="nroSecuencia"></param>
        /// <param name="cantFotos"></param>
        /// <param name="transitos"></param>
        /// <param name="numerSecuencia"></param>
        /// <param name="valorSecuencia"></param>
        /// <param name="cantRegSecuencia"></param>       
        /// <param name="cantImgUltSecuenciaTRN"></param>
        ///***************************************************************************************
        public static void GenerarArchivoTRN(int Administradora, string PathTemporal, string PathDestino, ref decimal valorTotal, ref int cantRegistros, ref int nroSecuencia, ref int cantFotos, List<PexTransito> transitos, ref int numerSecuencia, ref decimal valorSecuencia, ref int cantRegSecuencia, ref int cantImgUltSecuenciaTRN)
        {
            DateTime fechaGeneracion = DateTime.Now;
            string nombreArchivo = "";
     
            //int cantFotos = 0;


            foreach (PexTransito tran in transitos)
            {
                if (fechaGeneracion > tran.Fecha)
                {
                    valorTotal += tran.Valor;
                    cantRegistros++;
                    tran.FechaMal = false;
                }
                else
                {
                    tran.FechaMal = true; 
                }
            }           
                        
            
            PexTransito transito = transitos[0];
            nroSecuencia = PexBs.getSecuencia("TRN", Administradora);

            
            if (nroSecuencia % 100000 == 0)
            {

                // Secuencia 00000, la salteamos generando un archivo vacio con ese numero
                // La generacion de este archivo la hacemos directamente en la carpeta TRN (no en la temporal) porque casi no demora su creacion

                string sSecuencia = nroSecuencia.ToString("D05");
                if (sSecuencia.Length > 5)
                    sSecuencia = sSecuencia.Substring(sSecuencia.Length - 5, 5);
                nombreArchivo = fechaGeneracion.ToString("yyyyMMddHHmmss") + sSecuencia + ".TRN";
                using (StreamWriter sw = new StreamWriter(PathDestino + "\\TRN\\" + nombreArchivo))
                {
                    //escribo el encabezado
                    sw.WriteLine("TR" + transito.CodigoPais + transito.CodigoConcesionaria + nroSecuencia.ToString("D05").Substring(nroSecuencia.ToString().Length - 5, 5) + fechaGeneracion.ToString("yyyyMMddHHmmss") + 0.ToString("D06") + FormatValor2(0));
                }

                //Salteamos esta secuencia
                nroSecuencia++;
            }

            
            // Generamos el nombre del archivo
            nombreArchivo = fechaGeneracion.ToString("yyyyMMddHHmmss") + nroSecuencia.ToString("D05").Substring(nroSecuencia.ToString().Length - 5, 5) + ".TRN";

                
            // Generamos el archivo en una carpeta temporal para despues volcarlo a la carpeta definitiva (y que no lo usen mientras se genera)
            using (StreamWriter sw = new StreamWriter(PathTemporal + "\\" + nombreArchivo))            
            {
                //escribo el encabezado
                sw.WriteLine("TR" + transito.CodigoPais + transito.CodigoConcesionaria + nroSecuencia.ToString("D05").Substring(nroSecuencia.ToString().Length - 5, 5) + fechaGeneracion.ToString("yyyyMMddHHmmss") + cantRegistros.ToString("D06") + FormatValor2(valorTotal));

                int i = 1;
                foreach (PexTransito tran in transitos)
                {

                    if (!tran.FechaMal)
                    {

                        //Si no existe ninguna imagen hay que forzar el motivo a 00
                        string imagen1 = CopiarImagen(PathDestino, tran.FotoLateral1, "L1", fechaGeneracion, nroSecuencia, tran);
                        string imagen2 = CopiarImagen(PathDestino, tran.FotoLateral2, "L2", fechaGeneracion, nroSecuencia, tran);
                        string imagen3 = CopiarImagen(PathDestino, tran.FotoFrontal, "FR", fechaGeneracion, nroSecuencia, tran);
                        if (imagen1.Trim() == "" && imagen2.Trim() == "" && imagen3.Trim() == "")
                            tran.MotivoImagen = "00";
                        else if (tran.MotivoImagen == "00")
                            tran.MotivoImagen = "06";   //categoria diferente


                        sw.WriteLine("D" + i.ToString("D06") + tran.CodigoPais + tran.CodigoEmisorTag + tran.NumeroTag + tran.Fecha.ToString("yyyyMMddHHmmss")
                            + tran.CodigoPlaza + tran.CodigoPista + tran.CategoriaManual.Categoria.ToString("D02") + tran.CategoriaDetectada.Categoria.ToString("D02")
                            + tran.CategoriaConsolidada.Categoria.ToString("D02") + FormatValor(tran.Valor) + tran.StatusCobrada + tran.StatusPasada
                            + "0" + "0" + "0000000" + tran.Placa.Trim().PadRight(7) + "0000" + "00000" + "0000" + "000" + "00000000" + "000000" + tran.MotivoImagen
                            + imagen1 + imagen2 + imagen3);

                        if (!String.IsNullOrEmpty(tran.FotoLateral1))
                            cantFotos++;

                        if (!String.IsNullOrEmpty(tran.FotoLateral2))
                            cantFotos++;

                        if (!String.IsNullOrEmpty(tran.FotoFrontal))
                            cantFotos++;


                        i++;

                    }
                }

            }

            PexSecuencia oSecuencia = new PexSecuencia();
            oSecuencia.NroSecuencia = nroSecuencia;
            oSecuencia.AdministradoraTag = Administradora;
            oSecuencia.Directorio = PathDestino + "\\TRN\\" + nombreArchivo;
            oSecuencia.Estacion = transito.Estacion;
            oSecuencia.FechaEnvio = fechaGeneracion;
            oSecuencia.Jornada = transito.FechaJornada;
            oSecuencia.MontoTotal = valorTotal;
            oSecuencia.Registros = cantRegistros;
            oSecuencia.CantFotos = cantFotos;
            oSecuencia.TipoArchivo = "TRN";

            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //usamos transaccion 
                conn.ConectarConsolidado(true);


                foreach (PexTransito tran in transitos)
                {
                    if (!tran.FechaMal)
                    {
                        //PexBs.addTransitoEnviado(nroSecuencia, "P", tran.Estacion.Numero, tran.NumeroVia, tran.Fecha, tran.NumeroEvento, Administradora);
                        PexDt.addTransitoEnviado(conn, nroSecuencia, "P", tran.Estacion.Numero, tran.NumeroVia, tran.Fecha, tran.NumeroEvento, Administradora, tran.IdDov);
                    }
                }
                PexBs.addSecuenciaEnviada(oSecuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }


            // Movemos el arvchivo de la carpeta temporal a la definitiva
            System.IO.File.Move(PathTemporal + "\\" + nombreArchivo, PathDestino + "\\TRN\\" + nombreArchivo);


        }


        public static string CopiarImagen(string PathDestino, string archivoImagen, string tipoImg, DateTime fechaGenTransaccion, int nroSecuencia, PexTransito transito)
        {
            string nuevoNombre = "                                   ";

            if (!String.IsNullOrEmpty(archivoImagen))
            {
                //si no existe hay que mandarlo vacio
                if (File.Exists(transito.PathVideo + "\\" + archivoImagen))
                {
                    string extImagen = Path.GetExtension(transito.PathVideo + "\\" + archivoImagen);

                    nuevoNombre = transito.CodigoConcesionaria + transito.CodigoPlaza + transito.Fecha.ToString("yyyyMMddHHmmssff") + tipoImg + "_" + transito.CodigoPista + extImagen;

                    string path = PathDestino + "\\TRN\\" + fechaGenTransaccion.ToString("yyyyMMdd") + nroSecuencia.ToString("D05").Substring(nroSecuencia.ToString().Length - 5, 5);

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    File.Copy(transito.PathVideo + "\\" + archivoImagen, path + "\\" + nuevoNombre, true);
                }
            }

            return nuevoNombre;
        }

        public static string FormatValor(decimal valor)
        {
            long valsincent = (long)(valor * 100);
            return valsincent.ToString("D08");
        }

        public static string FormatValor2(decimal valor)
        {
            long valsincent = (long)(valor * 100);
            return valsincent.ToString("D12");
        }



        /// *******************************************************************************************************
        /// <summary>
        /// Actualiza el status de los tags eliminados (que no vienen en el nuevo archivo) cuando es un archivo TOTAL
        /// </summary>
        /// <param name="administradora">numero de Administradora</param>
        /// <param name="nuevaSecuencia">numero de la nueva secuencia que se importo</param>
        /// *******************************************************************************************************
        public static void finalizarProcesoTagsListaCompleta(int administradora, int nuevaSecuencia)
        {

            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                PexDt.finalizarProcesoTagsListaCompleta(conn, administradora, nuevaSecuencia);

            }
        }


        /// *******************************************************************************************************
        /// <summary>
        /// Actualiza el status de los eliminados de la lista negra (que no vienen en el nuevo archivo) cuando es un archivo TOTAL
        /// </summary>
        /// <param name="administradora">numero de Administradora</param>
        /// <param name="nuevaSecuencia">numero de la nueva secuencia que se importo</param>
        /// *******************************************************************************************************
        public static void finalizarProcesoListaNegraListaCompleta(int administradora, int nuevaSecuencia)
        {

            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                PexDt.finalizarProcesoListaNegraListaCompleta(conn, administradora, nuevaSecuencia);

            }
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTransitoPex()
        {
            return "TTP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado 
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(PexTransito transito)
        {
            return transito.Placa;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(PexTransito transito, string operacion)
        {
            var sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Transito Tratamiento Pex", operacion);
            AuditoriaBs.AppendCampo(sb, "Estación", transito.Estacion.Nombre);
            AuditoriaBs.AppendCampo(sb, "N° Via", transito.NumeroVia.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha", transito.Fecha.ToString());
            if (transito.NumeroEvento != null)
            {
                AuditoriaBs.AppendCampo(sb, "N° de Evento", transito.NumeroEvento.ToString());
            }
            else if (transito.IdDov != null)
            {
                AuditoriaBs.AppendCampo(sb, "N° de Evento", transito.NumeroEvento.ToString());
            }
            AuditoriaBs.AppendCampo(sb, "Dovid", transito.IdDov.ToString());            
            if(operacion=="Modificación")
            {
                AuditoriaBs.AppendCampo(sb, "Datos Modificados", "");
                AuditoriaBs.AppendCampo(sb, "Placa", transito.Placa);
                AuditoriaBs.AppendCampo(sb, "N° Tag", transito.NumeroTag);
                AuditoriaBs.AppendCampo(sb, "Emisor", transito.CodigoEmisorTag);
                AuditoriaBs.AppendCampo(sb, "Categoria Cobrada", transito.CategoriaConsolidada.Descripcion);

                if (transito.FotoFrontal != null)
                {
                    AuditoriaBs.AppendCampo(sb, "Foto Frontal", transito.FotoFrontal);
                }
                if (transito.FotoFrontal != null)
                {
                    AuditoriaBs.AppendCampo(sb, "Foto Lateral1", transito.FotoLateral1);
                }
                if (transito.FotoFrontal != null)
                {
                    AuditoriaBs.AppendCampo(sb, "Foto Lateral2", transito.FotoLateral2);
                }
                                
            }
            if (transito.ObservacionTratamiento.Length > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Observación", transito.ObservacionTratamiento); 
            }
            
            //TODO datos de cada anomalia
            return sb.ToString();
        }
    }
}

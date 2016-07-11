using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Telectronica.Validacion;
using Telectronica.Utilitarios;
using System.Globalization;
namespace Telectronica.Peaje
{
    public class OSAsBs
    {
        #region Constantes
        public const int MAXEJES = 9;

        public const int CATMAX = 63;
        public const int CATMAX2 = 62;

        public const int MAXEJESCGMP = 6;
        public const int SUMARCGMP = 54;

        #endregion

        #region OSASs: Metodos de la Clase OSAs.


        /// <summary> OK
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
                OSAsDt.delTags(conn, admin);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public static void delTag(OSAsTag tag)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                OSAsDt.delTag(tag, conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="tag"></param>
        public static void addTag(OSAsTag tag, char completo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                OSAsDt.addTag(tag, conn, completo);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
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

                OSAsDt.prepararProcesoTags(conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        public static void finalizarProcesoTags(int secuencia, int Administradora)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                OSAsDt.finalizarProcesoTags(conn, secuencia, Administradora);

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
        public static OSAsTransitoL getTransitosRechazados(DateTime fechaDesde, DateTime fechaHasta, int? estacion, int? administradoraTag, char estado, string codigoRechazo, int xiCantRows, ref bool llegoAlTope)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                return OSAsDt.getTransitosRechazados(conn, fechaDesde, fechaHasta, estacion, administradoraTag, estado, codigoRechazo, xiCantRows, ref llegoAlTope);
            }
        }

       

        /// <summary> OK
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
                OSAsDt.delListasNegras(conn, admin);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }//iniciamos una transaccion

        }

        /// <summary> OK
        ///  
        /// </summary>
        /// <param name="ln"></param>
        public static void delListaNegra(OSAsListaNegra ln)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                OSAsDt.delListaNegra(ln, conn);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="ln"></param>
        public static void addListaNegra(OSAsListaNegra ln, char completo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Eliminamos el tag
                OSAsDt.addListaNegra(ln, conn, completo);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
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
                OSAsDt.addSeclis(conn, Administradora, Secuencia, Tipo, fechaSecuencia, ListCompl);


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }

        }

        /// <summary> OK
        /// Guarda la auditoria de las importaciones realizadas
        /// </summary>
        /// <param name="fecha"></param>
        /// <param name="tipo"></param>
        /// <param name="secuencia"></param>
        /// <param name="archivo"></param>
        /// <param name="cantReg"></param>
        /// <param name="estado"></param>
        /// <param name="motivoError"></param>
        public static void addAuditoriaImportacion(DateTime fecha, string tipo, int secuencia, string archivo, int cantReg, string estado, string motivoError,int Adminis)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                OSAsDt.addAuditoriaImportacion(conn, fecha, tipo, secuencia, archivo, cantReg, estado, motivoError, Adminis);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        public static void anularTransitoPex(OSAsTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                OSAsDt.anularTransitoPex(conn, transito);
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

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="transito"></param>
        public static void perderTransitoPex(OSAsTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                OSAsDt.perderTransitoPex(conn, transito);

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
                                                            "D",
                                                            getAuditoriaCodigoRegistro(transito),
                                                            getAuditoriaDescripcion(transito, "Perdida")),
                                                            connAud);
                }
            }
        }

        public static void modificarTransitoPex(OSAsTransito transito)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidado(true);
                    OSAsDt.modificarTransitoPex(conn, transito);
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

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="Administradora"></param>
        /// <returns></returns>
        public static OSAsTransitoL getTransitos(int Administradora, int estacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                //Eliminamos el tag
                return OSAsDt.getTransitos(conn, Administradora, estacion);
            }

        }

        public static string reenviarTransitoPex(OSAsTransitoL transitos)
        {
            string erroresTratamiento = "";
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                using (Conexion connAud = new Conexion())
                {
                    //Para auditoria
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    foreach (var pexTransito in transitos)
                    {
                        string error = reenviarTransitoPex(conn, connAud, pexTransito);
                        if( error != "" )
                            erroresTratamiento = erroresTratamiento + error + "\r\n\r\n";
                        
                    }

                    //Grabo OK hacemos COMMIT
                    connAud.Finalizar(true);
                }
                conn.Finalizar(true);

            }

            return erroresTratamiento;
        }

        public static string reenviarTransitoPex(Conexion conn, Conexion connAud, OSAsTransito transito)
        {
            string errores = "";
            try
            {
                OSAsDt.reenviarTransitoPex(conn, transito);
                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTransitoOSA(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(transito),
                                                        getAuditoriaDescripcion(transito, "Reenviar")),
                                                        connAud);
            }
            catch (Exception ex)
            {
                errores = "Error en transito " + transito.Placa + " " + transito.Fecha.ToShortDateString() + " " + transito.Fecha.ToShortTimeString()
                    + " " + ex.Message + "\r\n\r\n";
            }

            return errores;
        }



        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTransitoOSA()
        {
            return "TTP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado 
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(OSAsTransito transito)
        {
            return transito.Placa;
        }

         ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(OSAsTransito transito, string operacion)
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
                if (transito.FotoLateral1 != null)
                {
                    AuditoriaBs.AppendCampo(sb, "Foto Lateral1", transito.FotoLateral1);
                }
                if (transito.FotoLateral2 != null)
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

        ///****************************************************************************************************
        /// <summary> OK
        /// Setea los transitos procesados
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        ///****************************************************************************************************
        public static void setTransitosProcesados(int estacion, DateTime? fecha)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                OSAsDt.setTransitosProcesados(conn, estacion, fecha);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        public static void addSecuenciaEnviada(OSAsSecuencia secuencia)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                OSAsDt.addSecuenciaEnviada(conn, secuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="oProtocolo"></param>
        public static void updProtocoloTecnico(OSAsProtocoloTecnico oProtocolo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                OSAsDt.updProtocoloTecnico(conn, oProtocolo);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
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


        ///***************************************************************************************
        /// <summary> OK
        /// Obtiene el proximo numero de secuencia a usar de los archivos TRN y TAF 
        /// </summary>
        /// <param name="tipo">TAF o TRN</param>
        /// <returns>Numero Secuencia a utilizar</returns>
        ///***************************************************************************************
        public static int getSecuencia(string tipo, int administradora)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return OSAsDt.getSecuencia(conn, tipo, administradora);
            }
        }

        ///***************************************************************************************
        /// <summary> OK
        /// Obtiene la ultima Secuencia de Todas las Administradoras de Todos los Tipos
        /// </summary>
        /// <param name="tipo">TAF o TRN o TAG o NEL</param>
        /// <returns>Numero Secuencia a utilizar</returns>
        ///***************************************************************************************
        public static OSAsSecuenciaL getAllSecuencias()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidado(false);

                    return OSAsDt.getAllSecuencias(conn);
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        ///***************************************************************************************
        /// <summary> OK
        /// Obtiene el ultimo numero de secuencia de los archivos recibidos
        /// </summary>
        /// <param name="tipo"></param>
        /// <returns></returns>
        ///***************************************************************************************
        public static int getSeclis(string tipo, int administradora)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                return OSAsDt.getSeclis(conn, tipo, administradora);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="estacion"></param>
        /// <returns></returns>
        public static OSAsCambioTarifaL getCambioTarifas(int estacion, int admtag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return OSAsDt.getCambioTarifas(conn, estacion, admtag);
            }
        }

        /// <summary> OK
        /// Obtiene la lista de tarifas
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static OSAsTarifaL getTarifas(int estacion, DateTime fecha)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);

                return OSAsDt.getTarifas(conn, estacion, fecha);
            }
        }

        ///*****************************************************************************************************
        /// <summary> OK
        /// Agrega una Alerta
        /// </summary>
        /// <param name="secuencia"></param>
        /// <param name="proxSecuencia"></param>
        /// <param name="extArchivo"></param>
        /// <param name="descr"></param>
        ///*****************************************************************************************************
        public static void addAlerta(int secuencia, int proxSecuencia, string extArchivo, string descr)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                OSAsDt.addAlerta(conn, secuencia, proxSecuencia, extArchivo, descr);
            }
        }

        /// <summary>OK
        /// 
        /// </summary>
        /// <param name="secuenciaRec"></param>
        /// <param name="extArchivo"></param>
        public static void clrAlerta(int secuenciaRec, string extArchivo)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                OSAsDt.clrAlerta(conn, secuenciaRec, extArchivo);
            }
        }

        /// <summary> OK
        /// 
        /// </summary>
        public static void prepararProcesoListaNegra()
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                OSAsDt.prepararProcesoListaNegra(conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        /// <summary> OK
        /// 
        /// </summary>
        /// <param name="secuencia"></param>
        public static void finalizarProcesoListaNegra(int secuencia, int Administradora)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                OSAsDt.finalizarProcesoListaNegra(conn, secuencia, Administradora);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        public static void deshacerTransitoPex(OSAsTransito transito)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                OSAsDt.deshacerTransitoPex(conn, transito);                
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

        /// <summary> OK
        /// Obtiene el numero de secuencia completo
        /// </summary>
        /// <param name="tipo">TAF o TRN</param>
        /// <returns>Numero Secuencia Completa</returns>
        public static OSAsSecuencia getSecuenciaCompleta(int secuencia, DateTime fecha, int codAdministradora)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);


                return OSAsDt.getSecuenciaCompleta(conn, secuencia, fecha, codAdministradora);
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
         
        /// <summary> VER
        /// 
        /// </summary>
        /// <param name="archivo"></param>
        /// <param name="secuencia"></param>
        /// <param name="administradora"></param>
        /// <param name="cantReg"></param>
        public static void ProcesarArchivoFinanciero(string archivo, int secuencia, int administradora, ref int cantReg,DateTime FechaEnvio)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                OSAsDt.delPropag(conn, secuencia, administradora);
                OSAsDt.delTRFCCO(conn, secuencia, administradora);

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
                        OSAsProtocoloFinanciero pf = new OSAsProtocoloFinanciero();

                        pf.Administradora = administradora;
                        pf.Secuencia = secuencia;

                        if (i == 0 && line.Length > 1)
                        {
                            secEnviada = Convert.ToInt32(line.Substring(11, 5));
                            //totalAceptado = Convert.ToDecimal(line.Substring(36, 10) + "." + line.Substring(46, 2));
                            //totalRechazado = Convert.ToDecimal(line.Substring(48, 10) + "." + line.Substring(58, 2));
                            totalAceptado = Convert.ToDecimal(line.Substring(36, 12)) / 100;
                            totalRechazado = Convert.ToDecimal(line.Substring(48, 12)) / 100;
                            string sFecha = line.Substring(16, 14);
                            fechaRecibido = Convert.ToDateTime(sFecha.Substring(0, 4) + "/" + sFecha.Substring(4, 2) + "/" + sFecha.Substring(6, 2)
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

                                OSAsDt.addRechazosProtocoloFinanciero(conn, pf);
                            }

                            if (pf.Tipo == "DF")
                            {
                                pf.FechaPago = Convert.ToDateTime(line.Substring(2, 4) + "/" + line.Substring(6, 2) + "/" + line.Substring(8, 2));
                                //pf.ValorPago = Convert.ToDecimal(line.Substring(10, 10) + "." + line.Substring(20, 2));
                                pf.ValorPago = Convert.ToDecimal(line.Substring(10, 12)) / 100;

                                if (line.Length > 22)
                                {
                                    //se agrego un campo con tipo
                                    if (line.Substring(22, 2) == "VP" || line.Substring(22, 2) == "AE")
                                    {
                                        pf.TipoArchivo = "VF";
                                    }
                                    else
                                    {
                                        pf.TipoArchivo = "TRN";
                                    }
                                }
                                else
                                {
                                    // SI LA FECHA DE PAGO DEL REGISTRO DF ES ANTERIOR A LA FECHE DE ENVIO + 10 DIAS ES VIA FACIL SI NO ES UN TRANSITO NORMAL 
                                    if (pf.FechaPago < FechaEnvio.AddDays(10))
                                        pf.TipoArchivo = "VF";
                                    else
                                        pf.TipoArchivo = "TRN";
                                }
                                OSAsDt.addPropag(conn, pf);
                            }

                        }

                        i++;
                    }

                    OSAsDt.updProtocoloFinanciero(conn, secuencia, administradora, fechaRecibido, totalAceptado, totalRechazado);

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
        public static void GenerarArchivoTRN(int Administradora, string PathTemporal, string pathDestino, string PathImagenes, ref decimal valorTotal, ref int cantRegistros, ref int nroSecuencia, ref int cantFotos, List<OSAsTransito> transitos, ref int numerSecuencia, ref decimal valorSecuencia, ref int cantRegSecuencia, ref int cantImgUltSecuenciaTRN)
        {
            DateTime fechaGeneracion = DateTime.Now;
            string nombreArchivo = "";
            string sSecuencia = "";
            //int cantFotos = 0;


            foreach (OSAsTransito tran in transitos)
            {
                if (fechaGeneracion < tran.Fecha)
                {
                    tran.datosMal = true;
                    tran.impedimento = false;
                }
                else if(!ValidarPlaca(tran.Placa))
                {
                    tran.datosMal = true;
                    tran.impedimento = true;
                    tran.CausaRechazo = new MotivoRechazo("i1", Traduccion.Traducir("Placa errónea"));
                }
                else if (!ValidarTag(tran.NumeroTag))
                {
                    tran.datosMal = true;
                    tran.impedimento = true;
                    tran.CausaRechazo = new MotivoRechazo("i3", Traduccion.Traducir("Tag erróneo"));
                }
                else if (!ValidarEmisor(tran.CodigoEmisorTag))
                {
                    tran.datosMal = true;
                    tran.impedimento = true;
                    tran.CausaRechazo = new MotivoRechazo("i2", Traduccion.Traducir("Emisor erróneo"));
                }
                else
                {                    
                    valorTotal += tran.Valor;
                    cantRegistros++;
                    tran.datosMal = false;
                    //Categoria especial hay que mandar dos registros
                    if (tran.esEspecial && tran.EjeAdicionalConsolidado > MAXEJES)
                        cantRegistros++;
            
                }
            }


            OSAsTransito transito = transitos[0];
            nroSecuencia = OSAsBs.getSecuencia("TRN", Administradora);




            if (nroSecuencia % 100000 == 0)
            {
                // Secuencia 00000, la salteamos generando un archivo vacio con ese numero
                // La generacion de este archivo la hacemos directamente en la carpeta TRN (no en la temporal) porque casi no demora su creacion
                sSecuencia = nroSecuencia.ToString("D05");

                if (sSecuencia.Length > 5)
                    sSecuencia = sSecuencia.Substring(sSecuencia.Length - 5, 5);


                nombreArchivo = fechaGeneracion.ToString("yyyyMMddHHmmss") + sSecuencia + ".TRN";
                using (StreamWriter sw = new StreamWriter(pathDestino  + nombreArchivo))
                {
                    //escribo el encabezado
                    sw.WriteLine("TR" + transito.CodigoPais + transito.CodigoConcesionaria + sSecuencia + fechaGeneracion.ToString("yyyyMMddHHmmss") + 0.ToString("D06") + FormatValor12(0));
                }

                //Salteamos esta secuencia
                nroSecuencia++;
            }


            // Generamos el nombre del archivo
            sSecuencia = nroSecuencia.ToString("D05");
            if (sSecuencia.Length > 5)
                sSecuencia = sSecuencia.Substring(sSecuencia.Length - 5, 5);


            nombreArchivo = fechaGeneracion.ToString("yyyyMMddHHmmss") + sSecuencia + ".TRN";


            // Generamos el archivo en una carpeta temporal para despues volcarlo a la carpeta definitiva (y que no lo usen mientras se genera)
            using (StreamWriter sw = new StreamWriter(PathTemporal + "\\TRN\\" + nombreArchivo))
            {
                //escribo el encabezado
                sw.WriteLine("TR" + transito.CodigoPais + transito.CodigoConcesionaria + sSecuencia + fechaGeneracion.ToString("yyyyMMddHHmmss") + cantRegistros.ToString("D06") + FormatValor12(valorTotal));

                int i = 1;
                foreach (OSAsTransito tran in transitos)
                {

                    OSAsTransito NewTran = null;
                    string imagen1 = "                                   ";
                    string imagen2 = "                                   ";
                    string imagen3 = "                                   ";

                    if (!tran.datosMal)
                    {
                        //Solo enviamos imagenes si hay Motivo
                        if (tran.MotivoImagen != "00")
                        {
                            //Si no existe ninguna imagen hay que forzar el motivo a 00
                            imagen1 = CopiarImagen(PathImagenes, tran.FotoLateral1, "L1", fechaGeneracion, nroSecuencia, tran);
                            imagen2 = CopiarImagen(PathImagenes, tran.FotoLateral2, "L2", fechaGeneracion, nroSecuencia, tran);
                            imagen3 = CopiarImagen(PathImagenes, tran.FotoFrontal, "FR", fechaGeneracion, nroSecuencia, tran);
                            if (imagen1.Trim() == "" && imagen2.Trim() == "" && imagen3.Trim() == "")
                                tran.MotivoImagen = "00";
                            else if (tran.MotivoImagen == "00")
                                tran.MotivoImagen = "06";   //categoria diferente
                        }
                        //SI ES UN TRANSITO DE CATEGORIA ESPECIAL
                        if (tran.esEspecial && tran.EjeAdicionalConsolidado > MAXEJES)
                        {
                            //Copio el Transito 
                            NewTran = new OSAsTransito();       //OJO que solo tiene algunos campos
                            //La fecha del Transito + 1 Segundo
                            NewTran.Fecha = tran.Fecha.AddSeconds(1);

                            //Calcular la Categoria Segun el numero de EJES
                            if (tran.EjeAdicionalConsolidado > MAXEJES + MAXEJES)
                            {
                                //TODO habria que mandar 3 transitos
                                NewTran.CategoriaConsolidada = new CategoriaManual(CATMAX, "");
                                tran.CategoriaConsolidada.Categoria = CATMAX;

                            }
                            else if (tran.EjeAdicionalConsolidado > MAXEJES + 1)
                            {
                                NewTran.CategoriaConsolidada = new CategoriaManual((byte)(tran.EjeAdicionalConsolidado - MAXEJES), "");
                                tran.CategoriaConsolidada.Categoria = CATMAX;
                            }
                            else
                            {
                                NewTran.CategoriaConsolidada = new CategoriaManual(2, "");
                                tran.CategoriaConsolidada.Categoria = CATMAX2;
                            }
                            NewTran.Valor = TarifaBs.getTarifa(tran.Estacion.Numero, NewTran.Fecha, NewTran.CategoriaConsolidada.Categoria, 0);

                            //traducir NewTran.Categoria al equivalente CGMP
                            if (NewTran.CategoriaConsolidada.Categoria > MAXEJESCGMP)
                                NewTran.CategoriaConsolidada.Categoria += SUMARCGMP;

                            tran.Valor -= NewTran.Valor;
                        }


                        sw.WriteLine("D" +
                                      i.ToString("D06") +
                                      tran.CodigoPais +
                                      tran.CodigoEmisorTag +
                                      tran.NumeroTag +
                                      tran.Fecha.ToString("yyyyMMddHHmmss") +
                                      tran.CodigoPlaza + tran.CodigoPista +
                                      tran.CategoriaManual.Categoria.ToString("D02") +
                                      tran.CategoriaDetectada.Categoria.ToString("D02") +
                                      tran.CategoriaConsolidada.Categoria.ToString("D02") +
                                      FormatValor8(tran.Valor) +
                                      tran.StatusCobrada +
                                      tran.StatusPasada +
                                      "0" +
                                      "0" +
                                      tran.SecuenciaTagAnterior.Trim().PadLeft(7, '0') +
                                      tran.Placa.Trim().PadRight(7) +
                                      tran.PaisAnterior.Trim().PadLeft(4, '0') +
                                      tran.ConcesionAnterior.Trim().PadLeft(5, '0') +
                                      tran.PlazaAnterior.Trim().PadLeft(4, '0') +
                                      tran.PistaAnterior.Trim().PadLeft(3, '0') +
                                      (tran.FechaAnterior == null ? "00000000000000" :
                                      Convert.ToDateTime(tran.FechaAnterior).ToString("yyyyMMddHHmmss")) +
                            //"000000" + 
                                      tran.MotivoImagen +
                                      imagen1 +
                                      imagen2 +
                                      imagen3);

                        if (NewTran != null)
                        {
                            i++;
                            sw.WriteLine("D" +
                                          i.ToString("D06") +
                                          tran.CodigoPais +
                                          tran.CodigoEmisorTag +
                                          tran.NumeroTag +
                                          NewTran.Fecha.ToString("yyyyMMddHHmmss") +
                                          tran.CodigoPlaza +
                                          tran.CodigoPista +
                                          tran.CategoriaManual.Categoria.ToString("D02") +
                                          tran.CategoriaDetectada.Categoria.ToString("D02") +
                                          NewTran.CategoriaConsolidada.Categoria.ToString("D02") +
                                          FormatValor8(NewTran.Valor) +
                                          tran.StatusCobrada +
                                          tran.StatusPasada +
                                          "0" +
                                          "0" +
                                          tran.SecuenciaTagAnterior.Trim().PadLeft(7, '0') +
                                          tran.Placa.Trim().PadRight(7) +
                                          tran.PaisAnterior.Trim().PadLeft(4, '0') +
                                          tran.ConcesionAnterior.Trim().PadLeft(5, '0') +
                                          tran.PlazaAnterior.Trim().PadLeft(4, '0') +
                                          tran.PistaAnterior.Trim().PadLeft(3, '0') +
                                          (tran.FechaAnterior == null ? "00000000000000" :
                                           Convert.ToDateTime(tran.FechaAnterior).ToString("yyyyMMddHHmmss")) +
                                //"000000" + Hora
                                          tran.MotivoImagen +
                                          imagen1 +
                                          imagen2 +
                                          imagen3);
                        }

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

            OSAsSecuencia oSecuencia = new OSAsSecuencia();
            oSecuencia.NroSecuencia = nroSecuencia;
            oSecuencia.AdministradoraTag = Administradora;
            oSecuencia.Directorio = pathDestino +  nombreArchivo;
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


                foreach (OSAsTransito tran in transitos)
                {
                    if (!tran.datosMal)
                    {
                        //OSAsBs.addTransitoEnviado(nroSecuencia, "P", tran.Estacion.Numero, tran.NumeroVia, tran.Fecha, tran.NumeroEvento, Administradora);
                        OSAsDt.addTransitoEnviado(conn, nroSecuencia, "P", tran.Estacion.Numero, tran.NumeroVia, tran.Fecha, tran.NumeroEvento, Administradora, tran.IdDov, null);
                    }
                    else //datosMal
                    {
                        if (tran.impedimento)
                        {
                            OSAsDt.addTransitoEnviado(conn, nroSecuencia, "I", tran.Estacion.Numero, tran.NumeroVia, tran.Fecha, tran.NumeroEvento, Administradora, tran.IdDov, tran.CausaRechazo);
                        }
                    }
                }
                OSAsBs.addSecuenciaEnviada(oSecuencia);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }

            //JAS Copiamos en vez de mover para quedarnos con una copia
            // Movemos el arvchivo de la carpeta temporal a la definitiva
            System.IO.File.Copy(PathTemporal + "\\TRN\\" + nombreArchivo, pathDestino  + "\\" + nombreArchivo);
            //// Movemos el arvchivo de la carpeta temporal a la definitiva
            //System.IO.File.Move(PathTemporal + "\\" + nombreArchivo, pathDestino +  nombreArchivo);


        }


        public static string CopiarImagen(string PathImagenes, string archivoImagen, string tipoImg, DateTime fechaGenTransaccion, int nroSecuencia, OSAsTransito transito)
        {
            string nuevoNombre = "                                   ";
            string sSecuencia = "";

            if (!String.IsNullOrEmpty(archivoImagen))
            {
                //si no existe hay que mandarlo vacio
                if (File.Exists(transito.PathVideo + "\\" + archivoImagen))
                {
                    string extImagen = Path.GetExtension(transito.PathVideo + "\\" + archivoImagen);

                    sSecuencia = nroSecuencia.ToString("D05");

                    if (sSecuencia.Length > 5)
                        sSecuencia = sSecuencia.Substring(sSecuencia.Length - 5, 5);

                    nuevoNombre = transito.CodigoConcesionaria + transito.CodigoPlaza + transito.Fecha.ToString("yyyyMMddHHmmssff") + tipoImg + "_" + transito.CodigoPista + extImagen;

                    string path = PathImagenes + /*"\\TRN" +*/  "\\" + fechaGenTransaccion.ToString("yyyyMMdd") + sSecuencia;

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    File.Copy(transito.PathVideo + "\\" + archivoImagen, path + "\\" + nuevoNombre, true);
                }
            }

            return nuevoNombre;
        }

        public static string FormatValor8(decimal valor)
        {
            long valsincent = (long)(valor * 100);
            return valsincent.ToString("D08");
        }

        public static string FormatValor12(decimal valor)
        {
            long valsincent = (long)(valor * 100);
            return valsincent.ToString("D12");
        }

        public static bool ValidarPlaca(string placa)
        {
            bool ok = true;
            if (placa.Trim().Length != 7)
                ok = false;
            else if (!SoloAlfaNumerico(placa.Trim()))
                ok = false;

            return ok;
        }

        public static bool ValidarTag(string tag)
        {
            bool ok = true;
            if (tag.Trim().Length != 10)
                ok = false;
            else if (!SoloNumeros(tag.Trim()))
                ok = false;
            else if (tag == "0000000000")       //Tag Violado
                ok = false;


            return ok;
        }

        public static bool ValidarEmisor(string emisor)
        {
            bool ok = true;
            if (emisor.Trim().Length != 5)
                ok = false;
            else if (!SoloNumeros(emisor.Trim()))
                ok = false;

            return ok;
        }

        public static bool SoloNumeros(string dato)
        {
            bool ret = true;
            for (int i = 0; i < dato.Length; i++)
            {
                char aux = dato[i];
                if( !(aux >= '0' && aux <= '9'))
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }

        public static bool SoloAlfaNumerico(string dato)
        {
            bool ret = true;
            for (int i = 0; i < dato.Length; i++)
            {
                char aux = dato[i];
                if (!(aux >= '0' && aux <= '9'
                     || aux >= 'A' && aux <= 'Z'
                     || aux >= 'a' && aux <= 'z'))
                {
                    ret = false;
                    break;
                }
            }

            return ret;
        }



        /// *******************************************************************************************************
        /// <summary> OK
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

                OSAsDt.finalizarProcesoTagsListaCompleta(conn, administradora, nuevaSecuencia);

            }
        }


        /// *******************************************************************************************************
        /// <summary> OK
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

                OSAsDt.finalizarProcesoListaNegraListaCompleta(conn, administradora, nuevaSecuencia);

            }
        }

        #endregion


        /// <summary>
        /// Actualiza la secuencia de novedades para la recepcion de listas de TAGS
        /// </summary>
        public static void ActualizarSecuenciaNovedades()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                OSAsDt.ActualizarSecuenciaNovedades(conn);

            }
        }


        /// <summary>
        /// Obtener la ultima fecha de borrado que tengo en la tabla
        /// </summary>
        /// <returns></returns>
        public static DateTime? ObtenerFechaBorrado()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                return OSAsDt.ObtenerFechaBorrado(conn);

            }
        }


        /// <summary>
        /// Borrar Novedades de Listas de Tags
        /// </summary>
        public static void BorrarNovedadesTags()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGST(false);

                OSAsDt.BorrarNovedadesTags(conn);

            }
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTransitoPex()
        {
            return "TTP";
        }



        /// <summary>
        /// Procesa los archivos Passagem Realizados >D
        /// </summary>
        /// <param name="arch"></param>
        /// <returns></returns>
        public static bool ProcesarArchivoPR(string archivo)
        {
            //Creamos la entidad para cargar los datos a importar.
            ValePedagio.PasadaRealizada pasadaRealizada = new ValePedagio.PasadaRealizada();    
                        
            string line;
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    line = reader.ReadLine();
                    //Primero procesamos el encabezado.
                    if (line != null)
                    {
                        procesarLineaEncabezadoVP(line,pasadaRealizada.encabezado);
                    }

                    //Luego procesamos todas las lineas
                    int cLine = 1;
                    while ((line = reader.ReadLine())!= null)
                    {
                        cLine++;
                        procesarLineaDetallePR(line,pasadaRealizada.detalle,cLine);
                    }

                    //Cuando termina de procesar podemos verificar si el encabezado era consistente con su detalle
                    bool integro = pasadaRealizada.checkIntegridad();
                        if (!integro)
                        {
                            throw new Exception("Fallo la comprobacion de integridad del encabezado: Valor total o Cantidad de registros incorrecto");
                        }  
  
                    //Si el archivo fue procesado correctamente ( Todas las lineas se han podido convertir y se comprobo el encabezado)

                        guardarPasadaRealizada(pasadaRealizada);
                    
                }

                return true;
            }
            catch (Exception ex )
            {
             //TRATAR CASOS EN QUE NO TENGA EL FORMATO CORRECTO   
                throw ex;
                return false;
            }
            
        }


        /// <summary>
        /// Envia los datos a la capa de @datos@ para que lo guarde en la base de @datos@
        /// </summary>
        /// <param name="pasadaRealizada"></param>
        private static void guardarPasadaRealizada(ValePedagio.PasadaRealizada pasadaRealizada)
        {
            //Para cada linea de detalle la paso a la capa de datos.
            using (Conexion oConn = new Conexion())
            {
                try
                {
                    oConn.ConectarConsolidado(true);

                    foreach (var pasada in pasadaRealizada.detalle)
                    {
                        OSAsDt.addPasadaRealizada(oConn, pasada, pasadaRealizada.encabezado.numeroSecuencia);
                    }    

                    oConn.Commit();
                }
                catch (Exception ex)
                {
                    
                    throw ex;
                }
              
            }
            
        }


        /// <summary>
        /// Envia los datos a la capa de @datos@ para que lo guarde en la base de @datos@
        /// </summary>
        /// <param name="pasadaRealizada"></param>
        private static void guardarPasadaCobrada(ValePedagio.PasadaCobrada pasadaCobrada)
        {
            //Para cada linea de detalle la paso a la capa de datos.
            using (Conexion oConn = new Conexion())
            {
                try
                {
                    oConn.ConectarConsolidado(true);

                    foreach (var pasada in pasadaCobrada.detalle)
                    {
                        OSAsDt.addPasadaCobrada(oConn, pasada);
                    }

                    oConn.Commit();
                }
                catch (Exception ex)
                {

                    throw ex;
                }

            }

        }

        ///// <summary>
        ///// Comprueba que la cantidad de lineas y el monto del encabezado coincida con su detalle
        ///// </summary>
        ///// <param name="pasadaRealizada"></param>
        //private static bool checkIntegridadPR(ValePedagio.PasadaRealizada pasadaRealizada)
        //{
        //    decimal TotalEncabezado = pasadaRealizada.encabezado.valorTotal;
        //    decimal TotalRegistros = pasadaRealizada.detalle.Sum(p => p.valorViaje);

        //    if (TotalEncabezado != TotalRegistros)
        //    {
        //        return false;
        //    }

        //    if (pasadaRealizada.detalle.Count != pasadaRealizada.encabezado.totalRegistros)
        //    {
        //        return false;
        //    }
        //    return true;

        //}

        private static void procesarLineaDetallePR(string line, List<ValePedagio.PasadaRealizada.Detalle> detalles,int numeroLinea)
        {
            if (line!="")
            {
                if (line.Length ==97)
                {
                    ValePedagio.PasadaRealizada.Detalle detalle     = new ValePedagio.PasadaRealizada.Detalle();
                    detalle.tipo                    = line.Substring(0,2);
                    detalle.numeroSecuencia         = Convert.ToInt32(line.Substring(2,6));
                    detalle.idPaisEmisor            = line.Substring(8,4);
                    detalle.idEmisorTag             = line.Substring(12,5);
                    detalle.numeroTag               = line.Substring(17,10);
                    detalle.idTrn                   = Convert.ToInt64(line.Substring(27,19));
                    detalle.estacion                = Convert.ToInt32(line.Substring(46,4));
                    detalle.via                     = Convert.ToInt32(line.Substring(50,3));
                    detalle.numeroViaje             = Convert.ToInt32(line.Substring(53,12));
                    detalle.valorViaje              = Convert.ToDecimal(line.Substring(65,10))/100;
                    detalle.categoria               = Convert.ToInt32(line.Substring(75,2));
                    detalle.fechaPasada             = DateTime.ParseExact(line.Substring(77,14),"yyyyMMddHHmmss",CultureInfo.InvariantCulture);
                    detalle.mesProtocoloFinanciero  = DateTime.ParseExact(line.Substring(91,6),"yyyyMM",CultureInfo.InvariantCulture);

                    detalles.Add(detalle);
                }
                else
                {
                    throw new Exception("La linea numero: " + numeroLinea.ToString() + " .Tiene un formato erroneo");
                }
            }
        }

        private static void procesarLineaEncabezadoVP(string line,ValePedagio.Encabezado encabezado)
        {

            if (line != "")
            {
                if (line.Length == 48)
	            {
		            encabezado.tipo                     =line.Substring(0,2);
                    encabezado.iDPaisConcesionaria      =line.Substring(2,4);
                    encabezado.iDConcesionaria          =line.Substring(6,5);
                    encabezado.numeroSecuencia          =Convert.ToInt32(line.Substring(11,5));
                    encabezado.fechaGeneracion          =DateTime.ParseExact(line.Substring(16,14),"yyyyMMddHHmmss",CultureInfo.InvariantCulture);
                    encabezado.totalRegistros           =Convert.ToInt32(line.Substring(30,6));
                    encabezado.valorTotal               =Convert.ToDecimal(line.Substring(36,12))/100;
	            }else
	            {
                    throw new Exception("El encabezado tiene un formato erroneo");
            	}
                
                
            }
        }

        public static bool ProcesarArchivoPC(string archivo)
        {
            //Creamos la entidad para cargar los datos a importar.
            ValePedagio.PasadaCobrada pasadaCobrada = new ValePedagio.PasadaCobrada();

            string line;
            try
            {
                using (StreamReader reader = new StreamReader(archivo))
                {
                    line = reader.ReadLine();
                    //Primero procesamos el encabezado.
                    if (line != null)
                    {
                        procesarLineaEncabezadoVP(line, pasadaCobrada.encabezado);
                    }

                    //Luego procesamos todas las lineas
                    int cLine = 1;
                    while ((line = reader.ReadLine()) != null)
                    {
                        cLine++;
                        procesarLineaDetallePC(line, pasadaCobrada.detalle, cLine);
                    }

                    //Cuando termina de procesar podemos verificar si el encabezado era consistente con su detalle
                    bool integro = pasadaCobrada.checkIntegridad();

                    if (!integro)
                    {
                        throw new Exception("Fallo la comprobacion de integridad del encabezado: Valor total o Cantidad de registros incorrecto");
                    }

                    //Si el archivo fue procesado correctamente ( Todas las lineas se han podido convertir y se comprobo el encabezado)

                    guardarPasadaCobrada(pasadaCobrada);

                }

                return true;
            }
            catch (Exception ex)
            {
                //TRATAR CASOS EN QUE NO TENGA EL FORMATO CORRECTO   
                throw ex;
                return false;
            }
        
        }

        private static void procesarLineaDetallePC(string line, List<ValePedagio.PasadaCobrada.Detalle> listDetalles, int numeroLinea)
        {
            if (line != "")
            {
                if (line.Length == 79)
                {
                    ValePedagio.PasadaCobrada.Detalle pasadaCobrada = new ValePedagio.PasadaCobrada.Detalle();
                    pasadaCobrada.tipo = line.Substring(0, 2);
                    pasadaCobrada.numeroSecuencia = Convert.ToInt32(line.Substring(2, 6));
                    pasadaCobrada.idPaisEmisor = line.Substring(8, 4);
                    pasadaCobrada.idEmisorTag = line.Substring(12, 5);
                    pasadaCobrada.numeroTag = line.Substring(17, 10);
                    pasadaCobrada.numeroViaje = Convert.ToInt64(line.Substring(27, 12));
                    pasadaCobrada.estacion = Convert.ToInt32(line.Substring(39, 5));
                    pasadaCobrada.categoria = Convert.ToInt32(line.Substring(44, 2));
                    pasadaCobrada.valorViaje = Convert.ToDecimal(line.Substring(46,10)) / 100;
                    pasadaCobrada.statusPasada = Convert.ToChar(line.Substring(56, 1));
                    pasadaCobrada.fechaPasada = DateTime.ParseExact(line.Substring(57, 8), "yyyyMMdd", CultureInfo.InvariantCulture);

                    //Solo si el status Pasada viene en 1 venia una fecha en formato valido
                    if (pasadaCobrada.statusPasada == '1'  )
                    {
                        pasadaCobrada.fechaCancelacion = DateTime.ParseExact(line.Substring(65, 14), "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        pasadaCobrada.fechaCancelacion = null;
                    }
                    

                    listDetalles.Add(pasadaCobrada);
                }
                else
                {
                    throw new Exception("La linea numero: " + numeroLinea.ToString() + " .Tiene un formato erroneo");
                }
            }   
        }
    }
}


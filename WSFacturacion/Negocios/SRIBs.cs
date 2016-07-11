using System;
using System.Data;
using System.IO;
using System.Configuration;
using Telectronica.Facturacion;
using Telectronica.Tesoreria;
using Telectronica.Validacion;
using Telectronica.Peaje.FacturacionElectronicaWS; //Web Service para Facturacion Electronica
using Log = Telectronica.Utilitarios.Log;
using System.Web.Configuration;

namespace Telectronica.Peaje
{
    public class SRIBs
    {
        /*
        //OJO no usar para variables diferentes en cada thread
        public static string g_PathDeSalidaXMLFacturas { get; set; }                  // Ruta en la cual se van a depositar los XML generados
        public static int g_DiasBorradoXML { get; set; }                              // Dias de retencion de los XML, anterior a eso los borra
        //ATENCION!!!! ESTA VARIABLE NO PUEDE USARSE MULTITHREAD
        public static string g_ZonasProcesar { get; set; }                            // Lista de zonas que se van a procesar con esta instancia del motor
        public static int g_RegistrosAProcesarPorCiclo { get; set; }                  // Cantidad de registros a procesar por cada ejecucion
        */

        //esta bien que haya un solo bool estatico porque quiero detener todos los threads
        public static bool g_DetenerProceso { get; set; }                             // Flag para detencion del proceso


        /// ***********************************************************************************************
        /// <summary>
        /// Procesa las facturas de via. Es invocado directamente por el thread del motor de envio SRI
        /// </summary>
        /// <param name="formaProcesamiento">A: Automatica, M:Manual</param>
        /// ***********************************************************************************************
        public static void ProcesarFacturaThread(string formaProcesamiento, ThreadProcesoSRI threadProcesoSRI, out string mensaje)
        {

            string sMensa = "";
            mensaje = "";


            // En el inicio del procesamiento ponemos la variable global de detencion en FALSE
            g_DetenerProceso = false;


            // Como el proceso se sincroniza entre varios motores, le ponemos un nombre que contenga la concesion que se procesa
            string sNomreProceso = "SRI" + threadProcesoSRI.Codigo;

            try
            {
                if (!LogActividadBS.InicioBloqueoProceso(sNomreProceso, out sMensa))
                {
                    Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                    mensaje += sMensa + "\n";
                }
                else
                {

                    sMensa = "PROCESA CONCESION: " + threadProcesoSRI.Zona.Codigo.ToString() + " - THREAD: " + threadProcesoSRI.NumeroThread.ToString();
                    Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);



                    //------------------------------------------------------
                    // Procesa los comprobantes generados en la via
                    //------------------------------------------------------
                    try
                    {

                        // Si alguien desde fuera de la interface indico que se detenga el thread, sale del bucle
                        if (g_DetenerProceso)
                        {
                            sMensa += ".    ProcesarVia - DETENIDO MANUALMENTE";
                        }
                        else
                        {
                            sMensa = ".    Inicio ProcesarVia" + "\n";
                            Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);


                            // EJECUCION Y LOGUEO DEL PROCESO
                            sMensa = ProcesarVia(formaProcesamiento, threadProcesoSRI);                                


                            sMensa = ".    Fin ProcesarVia" + "\n";
                            Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        sMensa = "Concesion: " + threadProcesoSRI.Zona.Codigo.ToString() + "-" + threadProcesoSRI.Zona.Descripcion + "\n";
                        sMensa += "Error procesando comprobantes de vía: " + Log.getDescripcionError(ex);
                        Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                        mensaje += sMensa + "\n";
                    }                

                    // Logueamos cuando se detiene el proceso (la corrida actual)
                    LogActividadBS.FinBloqueoProceso(sNomreProceso, true, mensaje);              

                }
            }
            catch (Exception ex)
            {

                sMensa = "Error procesando comprobantes de vía: " + Log.getDescripcionError(ex);
                Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                mensaje += sMensa + "\n";

                
                // Logueamos cuando se detiene el proceso (liberamos el proceso) pero con error
                LogActividadBS.FinBloqueoProceso(sNomreProceso, false, mensaje);

                throw;
            }
        }        


        /// ***********************************************************************************************
        /// <summary>
        /// Procesa los registros de facturas de la via.
        /// </summary>
        /// <param name="formaProcesamiento">A: Automatica, M:Manual</param>
        /// ***********************************************************************************************
        public static string ProcesarVia(string formaProcesamiento, ThreadProcesoSRI oThread)
        {
            try
            {
                string sMensa;
                string sMensaRet;
                bool bLogDetallado;
                string sArchivoLog;

                Zona oZona;
                int registrosAProcesar;


                // Leemos del app.config si se indico que logueemos en forma detallada cada comunicacion con el WS
                bLogDetallado = GenerarLogDetallado();

                if(bLogDetallado)
                    sArchivoLog = oThread.CodigoArchivo;
                else
                    sArchivoLog = "";


                oZona = oThread.Zona;
                registrosAProcesar = oThread.RegistrosAProcesarPorCiclo;

                
                //----------------------------------------------------
                // Procesamiento de los comprobantes de via
                //----------------------------------------------------

                sMensa = ".    Inicio GetFacturas()" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);

                Telectronica.Facturacion.FacturaL facturasSRI = GetFacturas(oThread.NumeroThread, oZona, registrosAProcesar);

                sMensa = ".    Fin GetFacturas()" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);



                // Retornamos ademas de un posible error, cuantos registros proceso 
                sMensaRet = ".       Solicitados: " + registrosAProcesar.ToString() + ", A Procesar: " + facturasSRI.Count.ToString();



                // Realiza el registro electronico de la factura, loguea el mismo y guarda el XML en disco
                // Le envio un parametro FacturaVia en true para que no procese de inmediato la respuesta.
                if (InterfaceBS.FacElect_Factura(facturasSRI, oZona, true, out sMensa, sArchivoLog))
                {
                    sMensaRet += sMensa;
                    Log.Loguear(sMensaRet, oThread.CodigoArchivo);
                }



                return sMensaRet;       // Ya no interesa el retorno, lo logueo en el momento para no perder la traza de la hora

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Marca las facturas como procesadas. Se hace en un thread diferente.
        /// </summary>
        /// ***********************************************************************************************
        public static void MarcarEnviosProcesadosThread(ThreadProcesoSRI oThread)
        {

            string sMensa;

            try
            {

                //-------------------------------------------------
                //   Marca como procesados
                //-------------------------------------------------

                sMensa = " .       Inicio MarcarEnviosProcesados" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);

                // MARCO LOS ENVIOS DE FACTURA DE VIA QUE FUERON ENVIADAS.
                MarcarEnviosProcesados(null);

                sMensa = " .       Fin MarcarEnviosProcesados" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);



                //-------------------------------------------------
                //   Marca repetidas
                //-------------------------------------------------

                sMensa = " .       Inicio MarcarRepetidas " + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);

                // MARCO LOS ENVIOS DE FACTURA DE VIA QUE FUERON ENVIADAS.
                MarcarEnviosProcesados(oThread.Zona);

                sMensa = " .       Fin MarcarRepetidas" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Procesa los acuse de recibo. Es invocado directamente por el thread del motor de envio SRI
        /// </summary>
        /// <param name="formaProcesamiento">A: Automatica, M:Manual</param>
        /// ***********************************************************************************************
        public static void ProcesarAcuseReciboThread(string formaProcesamiento, ThreadProcesoSRI threadProcesoSRI, out string mensaje)
        {

            string sMensa = "";
            mensaje = "";


            // En el inicio del procesamiento ponemos la variable global de detencion en FALSE
            g_DetenerProceso = false;


            // Como el proceso se sincroniza entre varios motores, le ponemos un nombre que contenga la concesion que se procesa
            string sNomreProceso = "SRI" + threadProcesoSRI.Codigo;

            try
            {
                if (!LogActividadBS.InicioBloqueoProceso(sNomreProceso, out sMensa))
                {
                    Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                    mensaje += sMensa + "\n";
                }
                else
                {

                    sMensa = "PROCESA CONCESION: " + threadProcesoSRI.Zona.Codigo.ToString() + " - THREAD: " + threadProcesoSRI.NumeroThread.ToString();
                    Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);



                    //------------------------------------------------------
                    // Procesa Las respuestas del SRI Facturas de VIA
                    //------------------------------------------------------

                    try
                    {
                        // Si alguien desde fuera de la interface indico que se detenga el thread, sale del bucle
                        if (g_DetenerProceso)
                        {
                            sMensa += ".    ProcesarAcusesDeRecibo - DETENIDO MANUALMENTE";
                        }
                        else
                        {
                            sMensa = ".    Inicio ProcesarAcusesDeRecibo" + "\n";
                            Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);


                            // EJECUCION Y LOGUEO DEL PROCESO
                            sMensa = ProcesarAcusesDeRecibo(threadProcesoSRI);
                            Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);


                            sMensa = ".    Fin ProcesarAcusesDeRecibo" + "\n";
                            Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                        }
                    }
                    catch (Exception ex)
                    {
                        sMensa = "Zona: " + threadProcesoSRI.Zona.Codigo.ToString() + "-" + threadProcesoSRI.Zona.Descripcion + "\n";
                        sMensa += "Error procesando acuse de recibo: " + Log.getDescripcionError(ex);
                        Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                        mensaje += sMensa + "\n";
                    }


                }

                // Logueamos cuando se detiene el proceso (la corrida actual)
                LogActividadBS.FinBloqueoProceso(sNomreProceso, true, mensaje);

            }
            catch (Exception ex)
            {

                sMensa = "Error procesando comprobantes de vía: " + Log.getDescripcionError(ex);
                Log.Loguear(sMensa, threadProcesoSRI.CodigoArchivo);
                mensaje += sMensa + "\n";

                
                // Logueamos cuando se detiene el proceso (liberamos el proceso) pero con error
                LogActividadBS.FinBloqueoProceso(sNomreProceso, false, mensaje);

                throw;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Procesa los acuse de recibo generados por el robot que se comunica con el SRI. 
        /// </summary>
        /// ***********************************************************************************************
        public static string ProcesarAcusesDeRecibo(ThreadProcesoSRI oThread)
        {
            try            
            {

                //------------------------------------------------------------------
                //  1. Consultamos los registros de envios que aun no tienen respuesta
                //  2. por cada registro consultamos al WS por su estado
                //  3. Si el resultado es distinto de desconocido, actualizamos el mismo en la DB
                //------------------------------------------------------------------


                string sMensa;
                string sMensaRet;
                
                Zona oZona;
                int registrosAProcesar;
                int count;

                bool bLogDetallado;
                string sArchivoLog;


                // Leemos del app.config si se indico que logueemos en forma detallada cada comunicacion con el WS
                bLogDetallado = GenerarLogDetallado();

                if (bLogDetallado)
                    sArchivoLog = oThread.CodigoArchivo;
                else
                    sArchivoLog = "";



                count = 0;

                oZona = oThread.Zona;
                registrosAProcesar = oThread.RegistrosAProcesarPorCiclo;



                sMensa = ".    Inicio GetEnviosPendientesVia()" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);

                //TRAIGO LA LISTA DE ENVIOS AL SRI QUE ESTAN PENDIENTES DE RESPUESTA
                EnvioSRIL oEnvioSRI = GetEnviosPendientesVia(oThread.NumeroThread, oZona, registrosAProcesar);

                sMensa = ".    Fin GetEnviosPendientesVia()" + "\n";
                Log.Loguear(sMensa, oThread.CodigoArchivo);




                // Retornamos ademas de un posible error, cuantos registros proceso 
                sMensaRet = ".       Solicitados: " + registrosAProcesar.ToString() + ", A Procesar: " + oEnvioSRI.Count.ToString();


                //RECORRER LA LISTA DE ENVIOS, PREGUNTANDO AL SRI POR EL ESTADO DE CADA UNO DE ELLOS
                if (oEnvioSRI != null) //verifico que haya traid algun envio
                {
                    // INSTANCIA EL WS
                    using (OperationsSoapClient WS = new OperationsSoapClient())
                    {
                        WS.Open();

                        foreach (EnvioSRI Envio in oEnvioSRI)
                        {
                            try
                            {
                                EstadosEnvioSRI oEstado = InterfaceBS.SetActualizarStatus_SRI(Envio, true, WS, sArchivoLog);

                                count++;

                                // Cada cierta cantidad de registros logueamos, para mostrar actividad
                                if (count % 100 == 0)
                                {
                                    Log.Loguear("Procesados " + count.ToString() + " registros..", oThread.CodigoArchivo);

                                    // Leemos del app.config si se indico que logueemos en forma detallada cada comunicacion con el WS
                                    bLogDetallado = GenerarLogDetallado();

                                    if (bLogDetallado)
                                        sArchivoLog = oThread.CodigoArchivo;
                                    else
                                        sArchivoLog = "";

                                }


                                // Si alguien desde fuera de la interface indico que se detenga el thread, sale del bucle
                                if (g_DetenerProceso)
                                {
                                    sMensaRet += DateTime.Now.ToString("HH:mm:ss.fff") + " .       .....  DETENIDO MANUALMENTE  .....";
                                    break;
                                }


                            }
                            catch (Exception ex)
                            {
                                sMensa = "Error procesando Consulta de Estado SRI " + ": " + Log.getDescripcionError(ex);
                                Log.Loguear(sMensa, oThread.CodigoArchivo);
                            }
                        }//foreach

                        
                        // Cerramos el WS
                        WS.Close();


                    }//using
               
                }

                sMensaRet += ", Procesados: " + count.ToString();

                return sMensaRet;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///// ***********************************************************************************************
        ///// <summary>
        ///// Funcion que retorna la lista de zonas a procesar. Levanta del app.config la lista, sino todas
        ///// </summary>
        ///// <returns>Lista de zonas a procesar con esta isntancia del motor</returns>
        ///// ***********************************************************************************************
        //private static ZonaL GetZonasAProcesar(string sZonasAProcesar)
        //{
        //    string sMensa;
        //    ZonaL oZonRet = null;                

        //    try
        //    {
        //        // Puede venir nulo
        //        if (sZonasAProcesar == null)
        //        {
        //            sZonasAProcesar = "";
        //        }


        //        // Si recibo el string vacio es que proceso todas las zonas juntas en el mismo motor, sino cargo una lista con cada una de las zonas
        //        if (sZonasAProcesar.Trim() == "")
        //        {
        //            oZonRet = EstacionBs.getZonas();
        //        }
        //        else
        //        {
        //            // Separo las zonas
        //            string[] zonasStr = sZonasAProcesar.Split(',');

        //            if (zonasStr[0] != "")
        //            {
        //                oZonRet = new ZonaL();

        //                foreach (var strZ in zonasStr)
        //                {
        //                    oZonRet.Add(EstacionBs.getZonas(Convert.ToInt16(strZ))[0]);
        //                }
        //            }
        //        }


        //        return oZonRet;

        //    }
        //    catch (Exception ex)
        //    {
        //        sMensa = "Error procesando GetZonasAProcesar(): " + Log.getDescripcionError(ex) + "\n";
        //        Log.Loguear(sMensa, Codigo);

        //        return null;
        //    }
        //}


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la carpeta de destino donde se deben colocar los archivos finales
        /// </summary>
        /// ***********************************************************************************************
        protected static string PathDeHistorico(string pathDelArchivo, string PathDestinoBackup )
        {

            var carpeta = PathDestinoBackup + "\\" + pathDelArchivo.Substring(pathDelArchivo.Length - 8, 8);

            //var pathDeHistorico = Path.Combine(path, carpeta);
            if (!Directory.Exists(carpeta))
                Directory.CreateDirectory(carpeta);
            
            
            return carpeta;         

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene una lista de XML de boletas emitidas por las vías, no enviadas al SRI.
        /// </summary>
        /// <returns>Lista de XML de boletas</returns>
        /// ***********************************************************************************************
        private static Facturacion.FacturaL GetFacturas(int numeroThread, Zona oZona, int registrosAProcesar)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.GetFacturas(conn, numeroThread, oZona, registrosAProcesar);
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Genera un update de trcruc 
        /// </summary>
        /// <param name="oZona"></param>
        /// ***********************************************************************************************
        internal static bool MarcarEnviosProcesados(Zona oZona)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.MarcarEnviosProcesados(conn, oZona);
            }
        }


        ///***********************************************************************************************
        /// <summary>
        /// Trae una lista con los envios pendiente de Respuesta del SRI de la VIA
        /// </summary>
        /// <param name="oZona"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static EnvioSRIL GetEnviosPendientesVia(int numeroThread, Zona oZona, int registrosAProcesar)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.GetEnviosPendientesVia(conn, numeroThread, oZona, registrosAProcesar);
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Cambia el estado a las facturas repetidas
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void MarcarFacturasRepetidas()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);
                EnviosSRIDt.MarcarFacturasRepetidas(conn);
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Marca como procesada una boleta de la vía.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion para manejar transaccion desde arriba</param>
        /// <param name="coest">Código de estación</param>
        /// <param name="nuvia">Número de vía</param>
        /// <param name="fecha">Fecha de la factura</param>
        /// <param name="numev">Número de evento</param>
        /// <param name="estado">Estado a grabar</param>
        /// ***********************************************************************************************
        private static void MarcarProcesada(Conexion oConn, int coest, int nuvia, DateTime fecha, int? numev, string estado)
        {
                EnviosSRIDt.MarcarProcesada(oConn, coest, nuvia, fecha, numev, estado);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Registra el envío de un documento al SRI
        /// </summary>
        /// <param name="oConn">Conexion - Objeto pasado por referencia para hacer todo el proceso en la misma transaccion</param>
        /// <param name="nombreArchivo">Nombre del archivo</param>
        /// <param name="rucConcesionario">RUC del concesionario</param>
        /// <param name="tipoComprobante">Tipo de comprobante</param>
        /// <param name="comprobante">Número de comprobante</param>
        /// <param name="fecha">Fecha de generacion del XML</param>
        /// ***********************************************************************************************
        private static void RegistrarEnvio(Conexion oConn, EnvioSRI oEnvioSRI)
        {

            EnviosSRIDt.RegistrarEnvio(oConn, oEnvioSRI);
        }
  

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los threads configurados a ser ejecutado por cada zona
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ThreadProcesoSRIL getThreadsPorZona()
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.getThreadsPorZona(conn);
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Lee del app.config la configuracion de logueo detallado en archivo
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool GenerarLogDetallado()
        {
            bool bRet = false;


            bRet = Convert.ToBoolean(WebConfigurationManager.AppSettings["LOG_DETALLADO"]);

            return bRet;
        }



#region REGENERACION DE COMPROBANTES


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los comprobantes y su estado de envio al SRI
        /// </summary>
        /// <param name="desde">Fecha Inicial</param>
        /// <param name="hasta">Fecha Final</param>
        /// <param name="concesion">Numero de concesion</param>
        /// <param name="establecimiento">Numero de establecimiento del comprobante</param>
        /// <param name="puntoVenta">Numero de punto de venta del comprobante</param>
        /// <param name="numeroComprobante">Numero de comprobante</param>
        /// <param name="statusComprobante">Status del comprobante</param>
        /// <returns>Lista de comprobantes y su estado de envio</returns>
        /// ***********************************************************************************************
        public static EnvioSRIL getRegistroEnviosSRI(DateTime fechaDesde, DateTime fechaHasta, int? zona, int? establecimiento,
                                                     string puntoVenta, int? numeroComprobante, string statusComprobante, string nombreCliente, string nroRuc, char incluirPendientes, out bool llegoTop, int top)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.getRegistroEnviosSRI(conn, fechaDesde, fechaHasta, zona, establecimiento, puntoVenta, numeroComprobante, statusComprobante, nombreCliente, nroRuc, incluirPendientes, out llegoTop, top);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de cantidad de comprobantes separados por estacion y estado
        /// </summary>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRegistroSRI_EnviosPorEstacion(DateTime fechaDesde, DateTime fechaHasta)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.getRegistroSRI_EnviosPorEstacion(conn, fechaDesde, fechaHasta);
            }
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Regenera un comprobante, marcandolo para que sea reenviado al SRI
        /// </summary>
        /// <param name="fecha">Fecha del comprobante</param>
        /// <param name="estacion">Estacion de generacion del comprobante</param>
        /// <param name="establecimiento">Establecimiento de la estacion del comprobante</param>
        /// <param name="puntoVenta">Punto de venta</param>
        /// <param name="numeroComprobante">Numero de comprobante</param>
        /// ***********************************************************************************************
        public static int RegenerarComprobante(Facturacion.FacturaL oFacturaL)
        {

            int countFacturasProcesadas = 0;

            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(true);

                                
                foreach (Facturacion.Factura item in oFacturaL)
                {
                    if (item.EstadoEnvio.StatusEnvio == "E" || item.EstadoEnvio.StatusEnvio == "N")
                        EnviosSRIDt.RegenerarComprobante(conn, item);

                    countFacturasProcesadas++;
                }

                // Confirmamos la transaccion
                conn.Finalizar(true);
             }

            return countFacturasProcesadas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna la lista de los registros de archivos enviados a la Sunat
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ResumenSRIL getResumenEnviosSRI()
        {
            //TODO BON: Para el cuadro de RESUMEN

            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidado(false);
                return EnviosSRIDt.getResumenEnviosSunat(conn);
            }
        }

#endregion

    }

}

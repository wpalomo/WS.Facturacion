using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Threading;
using System.Xml;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Validacion;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telectronica.Peaje;
using System.Text;
using Telectronica.Facturacion;

 

namespace Telectronica.Peaje
{

   
    public class InterfaceDT
    {
        
      
        #region INTERFACE 

        /// ***********************************************************************************************
        /// <summary>
        /// Carga la lista de tarjetas TAPI
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto datareader con los datos</param>
        /// <returns>Objeto de tarjeta TAPI</returns>
        /// ***********************************************************************************************
        private static ListaTarjetasTAPI CargarListaTarjetasTAPI(System.Data.IDataReader oDR)
        {
            ListaTarjetasTAPI oListaTarjetasTAPI = new ListaTarjetasTAPI();

            oListaTarjetasTAPI.EstacionOrigen = Convert.ToInt16(oDR["lts_coest"]);
            oListaTarjetasTAPI.FechaArchivo = Convert.ToDateTime(oDR["lts_fearc"]);
            oListaTarjetasTAPI.FechaOperacion = Convert.ToDateTime(oDR["lts_fecha"]);
            oListaTarjetasTAPI.NumeroLista = Convert.ToInt32(oDR["lts_lista"]);
            oListaTarjetasTAPI.Status = Convert.ToChar(oDR["lts_status"]);
            oListaTarjetasTAPI.Usuario = Convert.ToString(oDR["lts_usuar"]);

            return oListaTarjetasTAPI;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Carga la lista de tarjetas RUTA
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto datareader con los datos</param>
        /// <returns>Objeto de tarjeta RUTA</returns>
        /// ***********************************************************************************************
        private static ListaTarjetasRUTA CargarListaTarjetasRUTA(System.Data.IDataReader oDR)
        {
            ListaTarjetasRUTA oListaTarjetasRUTA = new ListaTarjetasRUTA();
            // TODO: Hay que agregar la fecha de modificacion del archivo de tarjetas. 
            // Ahora solamente esta la fecha del archivo de certificados.

            oListaTarjetasRUTA.EstacionOrigen = Convert.ToInt16(oDR["ltr_coest"]);
            oListaTarjetasRUTA.FechaArchivoCertificados = Convert.ToDateTime(oDR["ltr_fearc"]);
            oListaTarjetasRUTA.FechaArchivoTarjetas = Convert.ToDateTime(oDR["ltr_fearc"]);
            oListaTarjetasRUTA.FechaArchivoMensajes = Convert.ToDateTime(oDR["ltr_fearc"]);
            oListaTarjetasRUTA.FechaOperacion = Convert.ToDateTime(oDR["ltr_fecha"]);
            oListaTarjetasRUTA.NumeroLista = Convert.ToInt32(oDR["ltr_lista"]);
            oListaTarjetasRUTA.Status = Convert.ToChar(oDR["ltr_status"]);
            oListaTarjetasRUTA.Usuario = Convert.ToString(oDR["ltr_usuar"]);

            return oListaTarjetasRUTA;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga el archivo de subsidio
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto datareader con los datos</param>
        /// <returns>Objeto de archivo de subsidio</returns>
        /// ***********************************************************************************************
        private static ArchivoSubsidio CargarArchivoSubsidio(System.Data.IDataReader oDR)
        {
            ArchivoSubsidio oArchivoSubsidio = new ArchivoSubsidio();
            oArchivoSubsidio.EstacionOrigen = Convert.ToInt16(oDR["coest"]);
            oArchivoSubsidio.FechaArchivo = Convert.ToDateTime(oDR["fearc"]);
            oArchivoSubsidio.FechaOperacion = Convert.ToDateTime(oDR["fecha"]);
            oArchivoSubsidio.NumeroLista = Convert.ToInt32(oDR["lista"]);
            oArchivoSubsidio.Status = Convert.ToChar(oDR["statu"]);
            oArchivoSubsidio.Usuario = Convert.ToString(oDR["usuar"]);
            oArchivoSubsidio.oUsuario = new Usuario(Convert.ToString(oDR["usuar"]), Convert.ToString(oDR["usunombre"]));
            oArchivoSubsidio.oEstacionOrigen = new Estacion(Convert.ToInt16(oDR["coest"]), Convert.ToString(oDR["estnombr"]));
            oArchivoSubsidio.ListaNombre = Convert.ToString(oDR["listanombre"]);
            
            return oArchivoSubsidio;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Devuelva la ultima lista de tarjeta TAPI
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de coneccion con la base de datos</param>
        /// <returns>Objeto de Lata de TAPI</returns>
        /// ***********************************************************************************************
        public static ListaTarjetasTAPI GetUltimaListaTAPI(Conexion oConn)
        {
            ListaTarjetasTAPI oListaTarjetasTAPI = new ListaTarjetasTAPI();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ListasTAPI_getUltimaListaTAPI";

                oDR = oCmd.ExecuteReader();

                if (oDR.Read())
                {
                    oListaTarjetasTAPI = CargarListaTarjetasTAPI(oDR);
                }
                else
                {
                    //No habia datos
                    oListaTarjetasTAPI = null;
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oListaTarjetasTAPI;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelva la ultima lista de tarjeta RUTA
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de coneccion con la base de datos</param>
        /// <returns>Objeto de Lista de RUTA</returns>
        /// ***********************************************************************************************
        public static ListaTarjetasRUTA GetUltimaListaRUTA(Conexion oConn)
        {
            ListaTarjetasRUTA oListaTarjetasRUTA = new ListaTarjetasRUTA();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ListasRUTA_getUltimaListaRUTA";

                oDR = oCmd.ExecuteReader();

                if (oDR.Read())
                {
                    oListaTarjetasRUTA = CargarListaTarjetasRUTA(oDR);
                }
                else
                {
                    //No habia datos
                    oListaTarjetasRUTA = null;
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oListaTarjetasRUTA;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la recepcion de la lista TAPI
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de coneccion con la base de datos</param>
        /// <param name="oListaTarjetasTAPI">ListaTarjetasTAPI - Objeto con la lista TAPI</param>
        /// <param name="Arbol">XmlDocument - Documento leido del archivo</param>
        /// <returns>Nada</returns>
        /// ***********************************************************************************************
        public static void addListaTarjetasTAPI(Conexion oConn, ListaTarjetasTAPI oListaTarjetasTAPI, XmlDocument Arbol)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ListasTAPI_addListaTAPI";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oListaTarjetasTAPI.FechaOperacion;
                oCmd.Parameters.Add("@fearc", SqlDbType.DateTime).Value = oListaTarjetasTAPI.FechaArchivo;
                oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oListaTarjetasTAPI.Usuario;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Convert.ToInt16(oListaTarjetasTAPI.EstacionOrigen);
                oCmd.Parameters.Add("@XMLData", SqlDbType.Xml).Value = Arbol.InnerXml;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;


                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("Este número de categoría ya existe");
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                        msg = Traduccion.Traducir("Este número de categoría fue eliminado");

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la recepcion de la lista RUTA
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de coneccion con la base de datos</param>
        /// <param name="oListaTarjetasRUTA">ListaTarjetasRUTA - Objeto con la lista RUTA</param>
        /// <param name="RutaCertificados"></param>
        /// <param name="RutaTarjetas"></param>
        /// <param name="RutaMensajes"></param>
        /// <param name="Arbol">XmlDocument - Documento leido del archivo</param>
        /// <returns>Nada</returns>
        /// ***********************************************************************************************
        public static void addListaTarjetasRUTA(Conexion oConn, ListaTarjetasRUTA oListaTarjetasRUTA, 
                                                string RutaCertificados, string RutaTarjetas, 
                                                string RutaMensajes)
        {
            try
            {
                // Cargamos los dos archivos en memoria:

                // Abrimos:
                System.IO.FileStream fsCertificados = new FileStream(RutaCertificados, System.IO.FileMode.Open);
                System.IO.FileStream fsTarjetas = new FileStream(RutaTarjetas, System.IO.FileMode.Open);
                System.IO.FileStream fsMensajes = new FileStream(RutaMensajes, System.IO.FileMode.Open);

                // Creamos los arrays de bytes para almacenar los datos leídos
                Byte[] dataCertificados = new byte[fsCertificados.Length];
                Byte[] dataTarjetas = new byte[fsTarjetas.Length];
                Byte[] dataMensajes = new byte[fsMensajes.Length];

                // Guardamos los datos en los arrays
                fsCertificados.Read(dataCertificados, 0, Convert.ToInt32(fsCertificados.Length));
                fsTarjetas.Read(dataTarjetas, 0, Convert.ToInt32(fsTarjetas.Length));
                fsMensajes.Read(dataMensajes, 0, Convert.ToInt32(fsMensajes.Length));

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ListasRUTA_addListaRUTA";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oListaTarjetasRUTA.FechaOperacion;
                oCmd.Parameters.Add("@fearcCertificados", SqlDbType.DateTime).Value = oListaTarjetasRUTA.FechaArchivoCertificados;
                oCmd.Parameters.Add("@fearcTarjetas", SqlDbType.DateTime).Value = oListaTarjetasRUTA.FechaArchivoTarjetas;
                oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oListaTarjetasRUTA.Usuario;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Convert.ToInt16(oListaTarjetasRUTA.EstacionOrigen);
                oCmd.Parameters.Add("@CertificadosDAT", SqlDbType.VarBinary).Value = dataCertificados;
                oCmd.Parameters.Add("@TarjetasDAT", SqlDbType.VarBinary).Value = dataTarjetas;
                oCmd.Parameters.Add("@MensajesDAT", SqlDbType.VarBinary).Value = dataMensajes;
                oCmd.Parameters.Add("@MensajesName", SqlDbType.VarChar, 30).Value = RutaMensajes.Substring(RutaMensajes.LastIndexOf('\\') + 1);
                oCmd.Parameters.Add("@CertificadosName", SqlDbType.VarChar, 30).Value = RutaCertificados.Substring(RutaCertificados.LastIndexOf('\\') + 1);
                oCmd.Parameters.Add("@TarjetasName", SqlDbType.VarChar, 30).Value = RutaTarjetas.Substring(RutaTarjetas.LastIndexOf('\\') + 1);


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("Este número de categoría ya existe");
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                        msg = Traduccion.Traducir("Este número de categoría fue eliminado");

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el archivo de detalle 
        /// </summary>
        /// ***********************************************************************************************
        public static DataSet ExportarArhivoDetalle(Conexion oConn, DateTime JornadaDesde, DateTime JornadaHasta, char SoloRUTA)
        {
            DataSet DsResultado = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getDetalleTXT";
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = JornadaDesde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = JornadaHasta;
            oCmd.Parameters.Add("@SoloRUTA", SqlDbType.Char, 1).Value = SoloRUTA;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultado, "Resultado");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
            //oConn.Close();

            return DsResultado;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Crea el archivo de detalle TXT
        /// </summary>
        /// ***********************************************************************************************
        public static DataSet CreateTotalDetalleTXT(Conexion oConn, DateTime JornadaDesde, DateTime JornadaHasta)
        {
            DataSet DsResultado = new DataSet();
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_createTotalDetalleTXT";
            oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = JornadaDesde;
            oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = JornadaHasta;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultado, "TotalDetalleTXT");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return DsResultado;
        }

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Recuados para un mes
        /// </summary>
        /// ***********************************************************************************************
        public static void getFechaInformeConsolidado(DateTime FechaDesde,DateTime FechaHasta,int CantDias, ref DataSet DsResultados)
        {
            DataTable dt = new DataTable();
            DataRow dr;
            dt.Columns.Add(new System.Data.DataColumn("ID", typeof(int)));
            dt.Columns.Add(new System.Data.DataColumn("Fecha_Desde", typeof(String)));
            dt.Columns.Add(new System.Data.DataColumn("Fecha_Hasta", typeof(String)));
            dt.Columns.Add(new System.Data.DataColumn("Cant_Dias", typeof(String)));
            dr = dt.NewRow();
            dr[0] = 1;
            dr[1] = FechaDesde.ToString("dd-MMM-yyyy");
            dr[2] = FechaHasta.ToString("dd-MMM-yyyy");
            dr[3] = CantDias.ToString();
            dt.Rows.Add(dr);
            dt.TableName = "Datos_Fecha";
            
            DsResultados.Tables.Add(dt);
        }
        */

        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 1 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro1(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro1";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro1");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 2 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro2(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro2";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro2");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 3 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro3(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro3";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro3");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 4 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro4(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro4";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro4");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 5 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro5(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro5";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro5");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Exporta el cuadro numero 6 del excel de declaracion jurada
        /// </summary>
        /// ***********************************************************************************************
        public static void ExportarCuadro6(Conexion oConn, DataSet DsDataSet)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getCuadro6";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsDataSet, "Cuadro6");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista con los ultimos archivos de subsidio (RUTA y TAPI)
        /// </summary>
        /// ***********************************************************************************************
        public static ArchivoSubsidioL GetUltimosArchivosSubsidio(Conexion oConn)
        {
            ArchivoSubsidioL oListaArchivos = new ArchivoSubsidioL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ArchivosSubsidios_getArchivosSubsidios";

                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                    oListaArchivos.Add(CargarArchivoSubsidio(oDR));

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oListaArchivos;
        }

        #endregion

        #region INTERFACE CONTABLE



        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaContableL
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static ListaContableL getInterfaces(Conexion oConn, DateTime dtJornada, int? estacion)
        {
            ListaContableL oListaContables = new ListaContableL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getInterfaseContable";

                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = dtJornada;
                oCmd.Parameters.Add("@SalvarTabla", SqlDbType.Char,1).Value = "N";
                oCmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = null;
                oCmd.CommandTimeout = 300;
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oListaContables.Add(CargarContable(oDR));
                }

                // Cerramos el objeto
                oCmd = null;

                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oListaContables;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba los registros en la Interfase Contable
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool setInterfaceContable(Conexion oConn, DateTime dtJornada, string usuario , int? estacion)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getInterfaseContable";

                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = dtJornada;
                oCmd.Parameters.Add("@SalvarTabla", SqlDbType.Char, 1).Value = "S";
                oCmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al generar la interfase contable") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("No existe el registro");
                    if (retval == -102)
                        msg = Traduccion.Traducir("Los Débitos y Créditos no cuadran");
                    throw new ErrorSPException(msg);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la ListaContable
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader Jornada.ToString("yyyyMMdd") </param>
        /// <returns>ListaContable</returns>
        /// ***********************************************************************************************
        private static ListaContable CargarContable(System.Data.IDataReader oDR)
        {
            ListaContable oListaContable = new ListaContable();
            if (oDR["Tipo"] != DBNull.Value)
                oListaContable.ShortType = oDR["Tipo"].ToString();
            oListaContable.CodigoEstacion = (byte)oDR["est_codig"];
            oListaContable.Jornada = (DateTime)oDR["Jornada"];
            oListaContable.NombreEstacion = oDR["est_nombr"].ToString();
            oListaContable.Monto = Convert.ToDouble(oDR["Monto"]);
            if (oDR["Descripcion"] != DBNull.Value)
                oListaContable.DescriptionType = oDR["Descripcion"].ToString();
            if (oDR["Via"] != DBNull.Value) 
                oListaContable.CodeVia = oDR["Via"].ToString();
            if (oDR["Turno"] != DBNull.Value) 
                oListaContable.CodeTurno = oDR["Turno"].ToString();
            if (oDR["SubTurno"] != DBNull.Value) 
                oListaContable.CodeSubTurno = oDR["SubTurno"].ToString();
            if (oDR["PrimerTicket"] != DBNull.Value)
                oListaContable.PrimerTicket = (int)oDR["PrimerTicket"];
            else
                oListaContable.PrimerTicket = 0;
            if (oDR["UltimoTicket"] != DBNull.Value)
                oListaContable.UltimoTicket = (int)oDR["UltimoTicket"];
            else
                oListaContable.UltimoTicket = 0;
            return oListaContable;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba un registro en la Interfase Contable
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oRegistro">ListaContable - datos del registro</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool setInterfaceContable(Conexion oConn, ListaContable oRegistro, string usuario , int? estacion  )
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_setInterfaseContable";
                oCmd.Parameters.Add("@Concepto", SqlDbType.Char).Value = oRegistro.ShortType.Trim() + oRegistro.CodigoEstacion.ToString();
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oRegistro.Jornada;
                oCmd.Parameters.Add("@Peaje", SqlDbType.Decimal).Value = oRegistro.CodigoEstacion;
                oCmd.Parameters.Add("@Descripcion", SqlDbType.Char).Value = oRegistro.Descripcion;
                oCmd.Parameters.Add("@Observacion", SqlDbType.Char).Value = null;
                oCmd.Parameters.Add("@Valor", SqlDbType.Money).Value = oRegistro.Monto;
                oCmd.Parameters.Add("@Estado", SqlDbType.Char).Value = "N";
                oCmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro de jornada") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("No existe el registro");
                    throw new ErrorSPException(msg);
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return true;
        }


        #endregion

        #region INTERFACE SRI

       


         /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la ListaContable
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader Jornada.ToString("yyyyMMdd") </param>
        /// <returns>ListaContable</returns>
        /// ***********************************************************************************************
        
        
        public static CambioAutorizacionSRI CargarAutorizacionSRI(System.Data.IDataReader oDR)
        {
            CambioAutorizacionSRI oCambio = new  CambioAutorizacionSRI();
            if( oDR["ant_numau"] != DBNull.Value) 
                oCambio.autOld = oDR["ant_numau"].ToString();
            oCambio.autNew = oDR["asr_numau"].ToString();
            oCambio.ruc = oDR["cfg_ruc"].ToString();
            oCambio.fecha = ((DateTime)oDR["asr_fecha"]).ToShortDateString();
            oCambio.Inicio = (DateTime)oDR["asr_fecha"];
            oCambio.detalles = new ListaAutorizacionDetalleL();
            oCambio.Vencimiento = (DateTime) oDR["asr_venci"];
            if (oDR["ant_fecha"] != DBNull.Value)
                oCambio.InicioAnterior = (DateTime)oDR["ant_fecha"];
            if (oDR["ant_venci"] != DBNull.Value)
                oCambio.VencimientoAnterior = (DateTime)oDR["ant_venci"];

            return oCambio;
        }


       

        public static ListaAutorizacionDetalle CargarDetalleAutorizacionSRI(System.Data.IDataReader oDR)
        {
            ListaAutorizacionDetalle oCambio = new ListaAutorizacionDetalle();
            int estab, ptovta;
            oCambio.codDoc = 1;
            estab = Convert.ToInt32(oDR["Establecimiento"]);
            oCambio.estab = estab.ToString("D03");
            if (oDR["UltimoTicketAnterior"] != DBNull.Value)
                oCambio.finOld = oDR["UltimoTicketAnterior"].ToString();
            else
                oCambio.finOld = "0";
            //Si no hubo tickets informar 0
            if (oDR["PrimerTicket"] != DBNull.Value)
                oCambio.iniNew = oDR["PrimerTicket"].ToString();
            else
                oCambio.iniNew = "0";
            if (oDR["UltimoTicket"] != DBNull.Value)
                oCambio.finNew = oDR["UltimoTicket"].ToString();
            else
                oCambio.finNew = "0";
            ptovta = Convert.ToInt32(oDR["PuntoVenta"]);
            oCambio.ptoEmi = ptovta.ToString("D03");
          
            return oCambio;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaContableL
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>ListaContableL</returns>
        /// ***********************************************************************************************
        public static CambioAutorizacionSRI getInterfacesSRI(Conexion oConn, DateTime dtFecha, int? estacion, string PuntoVenta, bool baja)
        {
            CambioAutorizacionSRI oCambioAutorizacion = null;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getTransitosSRI";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = dtFecha;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@puntos_venta", SqlDbType.VarChar).Value = PuntoVenta;
                oCmd.Parameters.Add("@baja", SqlDbType.Char).Value = baja?"S":"N";

                oCmd.CommandTimeout = 300;
                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oCambioAutorizacion = CargarAutorizacionSRI(oDR);
                    oCambioAutorizacion.detalles.Add(CargarDetalleAutorizacionSRI(oDR));
                    while (oDR.Read())
                    {
                        oCambioAutorizacion.detalles.Add(CargarDetalleAutorizacionSRI(oDR));
                    }
                }


                // Cerramos el objeto
                oCmd = null;

                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oCambioAutorizacion;
        }

        public static ListaPuntosVentaL getInterfacesPuntosVenta(Conexion oConn, int estacion)
        {
            ListaPuntosVentaL oListaPuntosVenta = new ListaPuntosVentaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getPuntosVenta";

                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion;
              
                oDR = oCmd.ExecuteReader();
     
                 while (oDR.Read())
                 {
                     oListaPuntosVenta.Add(CargarPuntosVenta(oDR));
                   
                 }
                
                // Cerramos el objeto
                oCmd = null;

                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oListaPuntosVenta;
        }

        public static ListaPuntosVenta CargarPuntosVenta(System.Data.IDataReader oDR)
        {
            ListaPuntosVenta oPv = new ListaPuntosVenta();
            
            oPv.Establecimiento = oDR["Establecimiento"].ToString();
            if (oDR["PuntoVenta"] != DBNull.Value)
                oPv.PuntoVenta = oDR["PuntoVenta"].ToString();
            if (oDR["Nombre"] != DBNull.Value)
                oPv.Nombre = oDR["Nombre"].ToString();
            if (oDR["via_nuvia"] != DBNull.Value)
                oPv.Numvia = (byte)oDR["via_nuvia"];
            if (oDR["Origen"] != DBNull.Value)
                oPv.Origen = oDR["Origen"].ToString();
            if (oDR["Deleted"].ToString() == "S")
                oPv.Deleted = true;

            return oPv;
        }

        #endregion

        #region INTERFACE FACTURAS




        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la ListaFacturas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader  </param>
        /// <returns>ListaFacturas</returns>
        /// ***********************************************************************************************


        public static ListaFacturas CargarListaFacturas(System.Data.IDataReader oDR)
        {
            ListaFacturas oFactura = new ListaFacturas();

            oFactura.Identificado = oDR["Identificado"].ToString();
            oFactura.RazonSocialConcesionario = oDR["RazonSocialConcesionario"].ToString();
            oFactura.RucConcesionario = oDR["RucConcesionario"].ToString();
            oFactura.DireccionMatriz = oDR["DireccionMatriz"].ToString();
            oFactura.DireccionEstablecimiento = oDR["DireccionEstablecimiento"].ToString();

            oFactura.AutorizacionSRI = oDR["AutorizacionSRI"].ToString();
            if( oDR["InicioSRI"] != DBNull.Value)
                oFactura.InicioSRI = (DateTime)oDR["InicioSRI"];
            if (oDR["VencimientoSRI"] != DBNull.Value)
                oFactura.VencimientoSRI = (DateTime)oDR["VencimientoSRI"];
            oFactura.ContribuyenteEspecial = oDR["ContribuyenteEspecial"].ToString();
            oFactura.TipoComprobante = oDR["TipoComprobante"].ToString();
            if (oDR["Establecimiento"] != DBNull.Value)
                oFactura.Establecimiento = (int)oDR["Establecimiento"];
            oFactura.PuntoVenta = Convert.ToInt32(oDR["PuntoVenta"]);
            oFactura.SecuencialFactura = (int)oDR["SecuencialFactura"];
            oFactura.FechaEmision = (DateTime)oDR["FechaEmision"];

            oFactura.NombreCliente = oDR["NombreCliente"].ToString();
            oFactura.RucCliente = oDR["RucCliente"].ToString();

            if (oDR["Anulado"].ToString() == "S")
                oFactura.Anulado = true;

            oFactura.MontoNeto = (decimal)oDR["MontoNeto"];
            oFactura.MontoIVA = (decimal)oDR["MontoIVA"];
            oFactura.MontoTotal = (decimal)oDR["MontoTotal"];


            oFactura.detalles = new ListaFacturasDetalleL();
            return oFactura;
        }


        public static ListaFacturasDetalle CargarListaFacturasDetalle(System.Data.IDataReader oDR)
        {
            ListaFacturasDetalle oItem = new ListaFacturasDetalle();

            oItem.Cantidad = (int)oDR["Cantidad"];
            oItem.DescripcionItem = oDR["DescripcionItem"].ToString();
            oItem.PrecioUnitario = (decimal)oDR["PrecioUnitario"];

            return oItem;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la ListaFacturasL
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>ListaFacturasL</returns>
        /// ***********************************************************************************************
        public static ListaFacturasL getFacturas(Conexion oConn, int? establecimiento, int? puntoventa, int? facturaDesde, int? facturaHasta)
        {
            ListaFacturasL oFacturas = new ListaFacturasL();
            try
            {
                SqlDataReader oDR;
                ListaFacturas oFactura = null;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getInterfaseFacturas";

                oCmd.Parameters.Add("@estab", SqlDbType.Int).Value = establecimiento;
                oCmd.Parameters.Add("@puvta", SqlDbType.Int).Value = puntoventa;
                oCmd.Parameters.Add("@factuD", SqlDbType.Int).Value = facturaDesde;
                oCmd.Parameters.Add("@factuH", SqlDbType.Int).Value = facturaHasta;

                oCmd.CommandTimeout = 300;
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    ListaFacturas oFacturaAux = CargarListaFacturas(oDR);
                    //en el primer registro cargamos la factura
                    if (oFactura == null || oFactura.Identificado != oFacturaAux.Identificado
                        || oFactura.Establecimiento != oFacturaAux.Establecimiento
                        || oFactura.PuntoVenta != oFacturaAux.PuntoVenta
                        || oFactura.SecuencialFactura != oFacturaAux.SecuencialFactura)
                    {
                        oFactura = oFacturaAux;
                        oFacturas.Add(oFacturaAux);
                    }
                    //en todos cargamos el detalle
                    oFactura.detalles.Add(CargarListaFacturasDetalle(oDR));
                }


                // Cerramos el objeto
                oCmd = null;

                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oFacturas;
        }


        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Genera el anexo transaccional
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de coneccion con la base de datos</param>
        /// <param name="dtDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="dtHasta">DateTime - Fecha y Hora Hasta</param>
        /// <returns>Nada</returns>
        /// ***********************************************************************************************
        public static void GenerarAnexoTransaccional(Conexion oConn, DateTime dtDesde, DateTime dtHasta, string usuario)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Interfases_getAnexoTransaccional";

                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = dtDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = dtHasta;
                oCmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = usuario;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 3000;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar los registros ") + retval.ToString();

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #region EXCEL CONSOLIDADO
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Transitos para un mes
        /// </summary>
        /// ***********************************************************************************************
        public static DataSet getTransitoInformeConsolidado(Conexion oConn, DateTime MesAno, int? Coest, DataSet DsResultados)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getTransitosConsolidados";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Coest;
            oCmd.Parameters.Add("@Mes", SqlDbType.DateTime).Value = MesAno;
            oCmd.CommandTimeout = 300;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultados, "Datos_Trafico");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return DsResultados;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Recuados para un mes
        /// </summary>
        /// ***********************************************************************************************
        public static void getRecaudoInformeConsolidado(Conexion oConn, DateTime MesAno, int? Coest, ref DataSet DsResultados)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getRecaudosConsolidados";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Coest;
            oCmd.Parameters.Add("@Mes", SqlDbType.DateTime).Value = MesAno;
            oCmd.CommandTimeout = 300;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultados, "Datos_Recaudo");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Recsultados para un mes
        /// </summary>
        /// ***********************************************************************************************
        public static void getResultadoInformeConsolidado(Conexion oConn, DateTime MesAno, int? Coest, ref DataSet DsResultados)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getResultadoConsolidado";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Coest;
            oCmd.Parameters.Add("@Mes", SqlDbType.DateTime).Value = MesAno;
            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultados, "Datos_Resultado");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Recargas discriminadas en funcion de la agrupacion
        /// </summary>
        /// ***********************************************************************************************
        public static void getRecargasConsolidado(Conexion oConn, DateTime MesAno, int? Coest, ref DataSet DsResultados)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Interfases_getRecargasConsolidados";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Coest;
            oCmd.Parameters.Add("@Mes", SqlDbType.DateTime).Value = MesAno;
            oCmd.CommandTimeout = 3000;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(DsResultados, "Datos_Recarga");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }



        public static void ActualizarTransitoConsolidado(OleDbConnection oConn, DataTable dtTransitos)
        {
            try
            {
                string campo;
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = oConn;

                foreach (DataRow item in dtTransitos.Rows)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Datos_Trafico SET ");
                    for (int i = 1; i <= 31; i++)
                    {
                        campo = "DIA_" + i.ToString("D02");
                        if(item[campo] != DBNull.Value )
                        {
                            sb.Append(campo+" = "+((int)item[campo]).ToString()+", ");
                        }
                    }
                    if (item["TOTAL"] != DBNull.Value)
                    {
                        //Si el total es null no hay nada                        
                        sb.Append("TOTAL = " + ((int)item["TOTAL"]).ToString() + " ");
                        sb.Append("WHERE TIPO = '" + item["TIPO"]+"' AND FP = '"+item["FP"]+"' AND Cancelado = '" + item["Cancelado"]+ "' AND Preliminar = '" + item["Preliminar"]+"' ");
                        objCmdUpdate.CommandText = sb.ToString();
                        objCmdUpdate.ExecuteNonQuery();
                    }
                    
                }

                objCmdUpdate = null;
                


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void ActualizarRecaudoConsolidado(OleDbConnection oConn, DataTable dtRecaudo)
        {
            try
            {
                string campo;
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = oConn;

                foreach (DataRow item in dtRecaudo.Rows)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Datos_Recaudo SET ");
                    for (int i = 1; i <= 31; i++)
                    {
                        campo = "DIA_" + i.ToString("D02");
                        if (item[campo] != DBNull.Value)
                        {
                            sb.Append(campo + " = " + Conversiones.MoneyToString((decimal)item[campo]) + ", ");
                        }
                    }
                    if (item["TOTAL"] != DBNull.Value)
                    {
                        //Si el total es null no hay nada                        
                        sb.Append("TOTAL = " + Conversiones.MoneyToString((decimal)item["TOTAL"]) + " ");
                        sb.Append("WHERE TIPO = '" + item["TIPO"] + "' AND FP = '" + item["FP"] + "' ");
                        objCmdUpdate.CommandText = sb.ToString();
                        objCmdUpdate.ExecuteNonQuery();
                    }

                }

                objCmdUpdate = null;



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void ActualizarResultadoConsolidado(OleDbConnection oConn, DataTable dtResultado)
        {
            try
            {
                string campo;
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = oConn;

                foreach (DataRow item in dtResultado.Rows)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Datos_Resultado SET ");
                    for (int i = 1; i <= 31; i++)
                    {
                        campo = "DIA_" + i.ToString("D02");
                        if (item[campo] != DBNull.Value)
                        {
                            if (item["TIPO"].ToString() != "CRE")
                                sb.Append(campo + " = " + Conversiones.MoneyToString((decimal)item[campo]) + ", ");
                            else
                                sb.Append(campo + " = " + (Convert.ToInt32(item[campo])).ToString() + ", ");
                        }
                    }
                    if (item["TOTAL"] != DBNull.Value)
                    {
                        //Si el total es null no hay nada  
                        if (item["TIPO"].ToString() != "CRE")
                            sb.Append("TOTAL = " + Conversiones.MoneyToString((decimal)item["TOTAL"]) + " ");
                        else
                            sb.Append("TOTAL = " + (Convert.ToInt32(item["TOTAL"])).ToString() + " ");
                    }
                    sb.Append("WHERE TIPO = '" + item["TIPO"] + "' ");
                    objCmdUpdate.CommandText = sb.ToString();
                    objCmdUpdate.ExecuteNonQuery();
                }

                objCmdUpdate = null;



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void ActualizarRecargasConsolidado(OleDbConnection oConn, DataTable dtResultado)
        {
            try
            {
                string campo;
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = oConn;

                foreach (DataRow item in dtResultado.Rows)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.Append("UPDATE Datos_Recarga SET ");
                    for (int i = 1; i <= 31; i++)
                    {
                        campo = "DIA_" + i.ToString("D02");
                        if (item[campo] != DBNull.Value)
                        {
                            sb.Append(campo + " = " + Conversiones.MoneyToString((decimal)item[campo]) + ", ");
                        }
                    }
                    if (item["TOTAL"] != DBNull.Value)
                    {
                        //Si el total es null no hay nada                          
                        sb.Append("TOTAL = " + Conversiones.MoneyToString((decimal)item["TOTAL"]) + " ");
                    }
                    sb.Append("WHERE TIPO = '" + item["TIPO"] + "' ");
                    objCmdUpdate.CommandText = sb.ToString();
                    objCmdUpdate.ExecuteNonQuery();
                }

                objCmdUpdate = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void ActualizarFechasConsolidado(OleDbConnection oConn, DateTime desde, DateTime hasta, int dias, int estacion, string nombreEstacion, string direccion)
        {
            try
            {
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = oConn;

                StringBuilder sb = new StringBuilder();
                sb.Append("UPDATE Datos_Fecha SET ");
                sb.Append("Fecha_Desde = '" + desde.ToShortDateString() + "', ");
                sb.Append("Fecha_Hasta = '" + hasta.ToShortDateString() + "', ");
                sb.Append("Cant_Dias = " + dias.ToString() + ", ");
                sb.Append("Plaza = '" + estacion.ToString() + " - " + nombreEstacion + "', ");
                sb.Append("Direccion = '" + direccion + "' ");
                objCmdUpdate.CommandText = sb.ToString();
                objCmdUpdate.ExecuteNonQuery();


                objCmdUpdate = null;



            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion
    }

}

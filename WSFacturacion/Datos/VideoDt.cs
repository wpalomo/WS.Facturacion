using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class VideoDt
    {
        #region EVENTOCODIGOS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de eventos de la captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de codigos de eventos de la captura de video</returns>
        /// ***********************************************************************************************
        public static VideoEventoCodigoL getCodigosEventos(Conexion oConn)
        {
            VideoEventoCodigoL oVideoEventoCodigoL = new VideoEventoCodigoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getCodigosEventos";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoEventoCodigoL.Add(CargarCodigoEvento(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoEventoCodigoL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Codigos de Eventos de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de codigos de eventos de captura de video</param>
        /// <returns>Lista con el elemento VideoEventoCodigo de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoEventoCodigo CargarCodigoEvento(System.Data.IDataReader oDR)
        {
            try
            {
                VideoEventoCodigo oEventoCodigo = new VideoEventoCodigo(oDR["cod_codig"].ToString(),
                                                                        oDR["cod_descr"].ToString());

                return oEventoCodigo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region ACCIONCODIGOS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de acciones de la captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de codigos de acciones de la captura de video</returns>
        /// ***********************************************************************************************
        public static VideoAccionCodigoL getCodigosAcciones(Conexion oConn)
        {
            VideoAccionCodigoL oVideoAccionCodigoL = new VideoAccionCodigoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getCodigosAcciones";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoAccionCodigoL.Add(CargarCodigoAccion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoAccionCodigoL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Codigos de Acciones de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de codigos de acciones de captura de video</param>
        /// <returns>Lista con el elemento VideoAccionCodigo de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoAccionCodigo CargarCodigoAccion(System.Data.IDataReader oDR)
        {
            try
            {
                VideoAccionCodigo oAccionCodigo = new VideoAccionCodigo(oDR["cod_codig"].ToString(),
                                                                        oDR["cod_descr"].ToString());

                return oAccionCodigo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region ACCIONES

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de acciones de la captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via de la que se filtra la configuracion de Acciones de captura de video</param>
        /// <returns>Lista de acciones de la captura de video</returns>
        /// ***********************************************************************************************
        public static VideoAccionL getConfiguracionAcciones(Conexion oConn, string ModeloVia)
        {
            VideoAccionL oVideoAccionL = new VideoAccionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getConfiguracionAcciones";
                oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = ModeloVia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoAccionL.Add(CargarAccion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoAccionL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Acciones de captura de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de acciones de captura de video</param>
        /// <returns>Lista con el elemento VideoAccion de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoAccion CargarAccion(System.Data.IDataReader oDR)
        {
            try
            {
                VideoAccion oAccion = new VideoAccion(new ViaModelo(oDR["ModeloVia"].ToString(),
                                                                    oDR["DescrModeloVia"].ToString()),
                                                      new VideoAccionCodigo(oDR["CodigoAccion"].ToString(),
                                                                            oDR["DescrCodigoAccion"].ToString()),
                                                      oDR["ComienzoGrabacionC1"].ToString(),
                                                      oDR["FinalizaGrabacionC1"].ToString(),
                                                      oDR["ComienzoGrabacionC2"].ToString(),
                                                      oDR["FinalizaGrabacionC2"].ToString());

                return oAccion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de una accion
        /// </summary>
        /// <param name="oVideoAccion">VideoAccion - Objeto con la informacion de una accion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionAccion(VideoAccion oVideoAccion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updConfiguracionAccion";
                oCmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 3).Value = oVideoAccion.ModeloVia.Modelo;
                oCmd.Parameters.Add("@Accion", SqlDbType.Char, 1).Value = oVideoAccion.Accion.Codigo;
                oCmd.Parameters.Add("@Comienzo1", SqlDbType.Char, 1).Value = oVideoAccion.ComienzaGrabacionC1;
                oCmd.Parameters.Add("@Fin1", SqlDbType.Char, 1).Value = oVideoAccion.FinalizaGrabacionC1;
                oCmd.Parameters.Add("@Comienzo2", SqlDbType.Char, 1).Value = oVideoAccion.ComienzaGrabacionC2;
                oCmd.Parameters.Add("@Fin2", SqlDbType.Char, 1).Value = oVideoAccion.FinalizaGrabacionC2;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("Se afectaron más de 1 registro de Configuración de Acción");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region CATEGORIAS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de configuracion por categoria de la captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via de la que se filtra la configuracion por categoria de captura de video</param>
        /// <returns>Lista de Configuracion por categoria de la captura de video</returns>
        /// ***********************************************************************************************
        public static VideoCategoriaL getConfiguracionCateogorias(Conexion oConn, string ModeloVia)
        {
            VideoCategoriaL oVideoCategoriaL = new VideoCategoriaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getConfiguracionCategorias";
                oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = ModeloVia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoCategoriaL.Add(CargarCategoria(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoCategoriaL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Configuracion por categoria de captura de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de configuracion por categoria de captura de video</param>
        /// <returns>Lista con el elemento VideoCategoria de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoCategoria CargarCategoria(System.Data.IDataReader oDR)
        {
            try
            {
                VideoCategoria oCategoria = new VideoCategoria(new ViaModelo(oDR["ModeloVia"].ToString(),
                                                                             oDR["DescrModeloVia"].ToString()),
                                                               new CategoriaManual((byte)oDR["Categoria"], 
                                                                                   oDR["DescrCategoria"].ToString()),
                                                               ((short) oDR["TiempoMaximoC1"]) / 10.0,
                                                               new VideoConfiguracionAlmacenamiento(oDR["AlmacenamientoC1"].ToString(), ""),
                                                               (byte) oDR["PorcentajeC1"],
                                                               ((short)oDR["TiempoMaximoC2"]) / 10.0,
                                                               new VideoConfiguracionAlmacenamiento(oDR["AlmacenamientoC2"].ToString(), ""),
                                                               (byte) oDR["PorcentajeC2"]);

                return oCategoria;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de una categoria
        /// </summary>
        /// <param name="oVideoCategoria">VideoCategoria - Objeto con la informacion de una categoria a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionCategoria(VideoCategoria oVideoCategoria, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updConfiguracionCategoria";
                oCmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 3).Value = oVideoCategoria.ModeloVia.Modelo;
                oCmd.Parameters.Add("@Categoria", SqlDbType.TinyInt).Value = oVideoCategoria.Categoria.Categoria;
                oCmd.Parameters.Add("@TiempoMaximo1", SqlDbType.SmallInt).Value = oVideoCategoria.TiempoMaximoGrabacionC1 * 10;
                oCmd.Parameters.Add("@Almacenamiento", SqlDbType.Char, 1).Value = oVideoCategoria.AlmacenamientoC1.Codigo;
                oCmd.Parameters.Add("@Porcentaje", SqlDbType.TinyInt).Value = oVideoCategoria.PorcentajeMuestreoC1;
                oCmd.Parameters.Add("@TiempoMaximo2", SqlDbType.SmallInt).Value = oVideoCategoria.TiempoMaximoGrabacionC2 * 10;
                oCmd.Parameters.Add("@Almacenamiento2", SqlDbType.Char, 1).Value = oVideoCategoria.AlmacenamientoC2.Codigo;
                oCmd.Parameters.Add("@Porcentaje2", SqlDbType.TinyInt).Value = oVideoCategoria.PorcentajeMuestreoC2;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de Configuración de la Categoría");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region CONFIGURACIONMODELO

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de configuracion general de un modelo determinado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via de la que se filtra la configuracion general de captura de video</param>
        /// <returns>Lista de Configuracion general de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoConfiguracionModeloL getConfiguracionModelo(Conexion oConn, string ModeloVia)
        {
            VideoConfiguracionModeloL oVideoConfiguracionModeloL = new VideoConfiguracionModeloL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getConfiguracionModelo";
                oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = ModeloVia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoConfiguracionModeloL.Add(CargarConfiguracionModelo(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoConfiguracionModeloL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Configuracion por modelo de captura de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de configuracion de captura de video</param>
        /// <returns>Lista con el elemento VideoConfiguracionModelo de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoConfiguracionModelo CargarConfiguracionModelo(System.Data.IDataReader oDR)
        {
            try
            {
                VideoConfiguracionModelo oConfiguracionModelo = new VideoConfiguracionModelo(new ViaModelo(oDR["ModeloVia"].ToString(),
                                                                                                           oDR["DescrModeloVia"].ToString()),
                                                                                             (byte) oDR["Cuadros"]);
                                                                                        
                return oConfiguracionModelo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion general propia del modelo de via
        /// </summary>
        /// <param name="oVideoConfigModelo">VideoConfiguracionModelo - Objeto con la informacion de configuracion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionModelo(VideoConfiguracionModelo oVideoConfigModelo, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updConfiguracionModelo";
                oCmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 3).Value = oVideoConfigModelo.Modelo.Modelo;
                oCmd.Parameters.Add("@Cuadros", SqlDbType.TinyInt).Value = oVideoConfigModelo.CuadrosPorSegundo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("Se afectaron más de 1 registro de Configuración del Modelo de Via");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region EVENTOS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de eventos de la captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via de la que se filtra la configuracion de Eventos de captura de video</param>
        /// <returns>Lista de eventos de la captura de video</returns>
        /// ***********************************************************************************************
        public static VideoEventoL getConfiguracionEventos(Conexion oConn, string ModeloVia)
        {
            VideoEventoL oVideoEventoL = new VideoEventoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getConfiguracionEventos";
                oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = ModeloVia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoEventoL.Add(CargarEvento(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoEventoL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Eventos de captura de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de eventos de captura de video</param>
        /// <returns>Lista con el elemento VideoEvento de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoEvento CargarEvento(System.Data.IDataReader oDR)
        {
            try
            {
                VideoEvento oEvento = new VideoEvento(new ViaModelo(oDR["ModeloVia"].ToString(),
                                                                    oDR["DescrModeloVia"].ToString()),
                                                      new VideoEventoCodigo(oDR["CodigoEvento"].ToString(),
                                                                            oDR["DescrCodigoEvento"].ToString()),
                                                      new VideoConfiguracionAlmacenamiento(oDR["AlmacenamientoC1"].ToString(), ""),
                                                      (byte) oDR["PorcentajeMuestreoC1"],
                                                      new VideoConfiguracionAlmacenamiento(oDR["AlmacenamientoC2"].ToString(), ""),
                                                      (byte) oDR["PorcentajeMuestreoC2"]);

                return oEvento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de un Evento
        /// </summary>
        /// <param name="oVideoEvento">VideoEvento - Objeto con la informacion de un Evento a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionEvento(VideoEvento oVideoEvento, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updConfiguracionEvento";
                oCmd.Parameters.Add("@Modelo", SqlDbType.VarChar, 3).Value = oVideoEvento.ModeloVia.Modelo;
                oCmd.Parameters.Add("@Evento", SqlDbType.Char, 2).Value = oVideoEvento.Evento.Codigo;
                oCmd.Parameters.Add("@Almacenamiento1", SqlDbType.Char, 1).Value = oVideoEvento.AlmacenamientoC1.Codigo;
                oCmd.Parameters.Add("@Porcentaje1", SqlDbType.TinyInt).Value = oVideoEvento.PorcentajeMuestreoC1;
                oCmd.Parameters.Add("@Almacenamiento2", SqlDbType.Char, 1).Value = oVideoEvento.AlmacenamientoC2.Codigo;
                oCmd.Parameters.Add("@Porcentaje2", SqlDbType.TinyInt).Value = oVideoEvento.PorcentajeMuestreoC2;
                

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de Configuración del Evento");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region CONFIGURACION

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de configuracion general de captura de video
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via de la que se filtra la configuracion general de captura de video</param>
        /// <returns>Lista de Configuracion general de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoConfiguracionL getConfiguracion(Conexion oConn)
        {
            VideoConfiguracionL oVideoConfiguracionL = new VideoConfiguracionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getConfiguracion";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoConfiguracionL.Add(CargarConfiguracion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoConfiguracionL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Configuracion general de captura de Video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de configuracion de captura de video</param>
        /// <returns>Lista con el elemento VideoConfiguracion de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoConfiguracion CargarConfiguracion(System.Data.IDataReader oDR)
        {
            try
            {
                VideoConfiguracion oConfiguracion = new VideoConfiguracion((short) oDR["DiasRotacion"],
                                                                           (short) oDR["DiasBorrado"]);

                return oConfiguracion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de dias de rotacion y borrado
        /// </summary>
        /// <param name="oVideoConfig">VideoConfiguracion - Objeto con la informacion de dias de rotacion y borrado a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracion(VideoConfiguracion oVideoConfig, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updConfiguracion";

                oCmd.Parameters.Add("@DiasRotacion", SqlDbType.SmallInt).Value = oVideoConfig.DiasRotacionDirectorios;
                oCmd.Parameters.Add("@DiasBorrado", SqlDbType.SmallInt).Value = oVideoConfig.DiasBorradoVideos;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de Configuración");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region PATHS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de carpetas de rotacion de videos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de carpetas de rotacion de videos</returns>
        /// ***********************************************************************************************
        public static VideoPathL getCarpetasRotacion(Conexion oConn, int? IDRegistro)
        {
            VideoPathL oVideoPathL = new VideoPathL();
            try
            {
                int? IDReg;

                if (IDRegistro == null)
                    IDReg = null;
                else
                    IDReg = IDRegistro;


                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_getCarpetasRotacion";                
                oCmd.Parameters.Add("@ID", SqlDbType.Int).Value = IDReg;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVideoPathL.Add(CargarPath(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVideoPathL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Carpetas de rotacion de video
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Rotaciones de video</param>
        /// <returns>Lista con el elemento VideoPath de la base de datos</returns>
        /// ***********************************************************************************************
        private static VideoPath CargarPath(System.Data.IDataReader oDR)
        {
            try
            {
                VideoPath oVideoPath = new VideoPath((byte) oDR["ID"],
                                                     oDR["PathRotacion"].ToString(),
                                                     oDR["PathRotacionFTP"].ToString());

                return oVideoPath;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una rotacion de carpeta de video en la base de datos
        /// </summary>
        /// <param name="oVideoPath">VideoPath - Objeto con la informacion de la rotacion a grabar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCarpetaRotacion(VideoPath oVideoPath, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_addCarpetaRotacion";
                oCmd.Parameters.Add("@Path", SqlDbType.VarChar, 255).Value = oVideoPath.PathCarpeta;
                oCmd.Parameters.Add("@PathFTP", SqlDbType.VarChar, 100).Value = oVideoPath.PathCarpetaFTP;

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
        /// Modifica un directorio de rotacion de video de la base de datos
        /// </summary>
        /// <param name="oVideoPath">VideoPath - Objeto con la informacion de la rotacion de video a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updCarpetaRotacion(VideoPath oVideoPath, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_updCarpetaRotacion";

                oCmd.Parameters.Add("@IDRegistro", SqlDbType.Int).Value = oVideoPath.IDRegistro;
                oCmd.Parameters.Add("@Path", SqlDbType.VarChar, 255).Value = oVideoPath.PathCarpeta;
                oCmd.Parameters.Add("@PathFTP", SqlDbType.VarChar, 100).Value = oVideoPath.PathCarpetaFTP;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de la Rotación");
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
        /// Elimina un directorio de rotacion de video de la base de datos
        /// </summary >
        /// <param name="oVideoPath">VideoPath - Objeto que contiene la rotacion de video a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCarpetaRotacion(VideoPath oVideoPath, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Video_delCarpetaRotacion";
                oCmd.Parameters.Add("@IDRegistro", SqlDbType.TinyInt).Value = oVideoPath.IDRegistro;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("No existe el registro de la Rotación");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion


        #region TIPOALMACENAMIENTO

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tipos de almacenamiento de los videos
        /// </summary>
        /// ***********************************************************************************************
        public static VideoConfiguracionAlmacenamientoL getTiposAlmacenamiento() 
        {

            VideoConfiguracionAlmacenamientoL oVideoConfiguracionAlmacenamientoL = new VideoConfiguracionAlmacenamientoL();

            oVideoConfiguracionAlmacenamientoL.Add(new VideoConfiguracionAlmacenamiento("N", getDescripcionAlmacenamiento("N")));
            oVideoConfiguracionAlmacenamientoL.Add(new VideoConfiguracionAlmacenamiento("S", getDescripcionAlmacenamiento("S")));
            oVideoConfiguracionAlmacenamientoL.Add(new VideoConfiguracionAlmacenamiento("M", getDescripcionAlmacenamiento("M")));

        
            return oVideoConfiguracionAlmacenamientoL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido del tipo de almacenamiento
        /// </summary>
        /// <param name="codigo">string - Codigo del tipo de almacenamiento que deseo devolver como texto</param>
        /// <returns>El texto traducido del tipo de almacenamiento</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionAlmacenamiento(string codigo)
        {
            string retorno = string.Empty;
            string caseSwitch = codigo;

            switch (caseSwitch)
            {
                case "N":
                    retorno = "Nunca";
                    break;

                case "S":
                    retorno = "Siempre";
                    break;

                case "M":
                    retorno = "Muestreo";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }

        #endregion
    }
}

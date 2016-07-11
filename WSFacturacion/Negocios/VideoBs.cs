using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class VideoBs
    {
        #region METODOS_INTEGRADORES

        /// ****************************************************************************************** <summary>
        /// Graba la configuracion de las categorias, eventos, acciones y configuracion propia del modelo de via
        /// <param name="oVideoConfiguracionModeloL">VideoConfiguracionModeloL - ListA (con 1 solo elemento) con la configuracion general del modelo de via</param>
        /// <param name="oVideoCategoriaL">VideoCategoriaL - Lista con la configuracion de tiempos de grabacion por categoria</param>
        /// <param name="oVideoAccionL">VideoAccionL - Lista con la configuracion de acciones de inicio y fin de grabacion</param>
        /// <param name="oVideoEventoL">VideoEventoL - Lista con la configuracion de los eventos</param>
        /// </summary>********************************************************************************
        public static void updConfiguracionCaptura(VideoConfiguracionModeloL oVideoConfiguracionModeloL,
                                                   VideoCategoriaL oVideoCategoriaL,
                                                   VideoAccionL oVideoAccionL,
                                                   VideoEventoL oVideoEventoL)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);


                    //------------------------------------------------------------
                    //Modificamos la configuracion general del modelo de via
                    //------------------------------------------------------------
                    VideoDt.updConfiguracionModelo(oVideoConfiguracionModeloL[0], conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oVideoConfiguracionModeloL[0]),
                                                           getAuditoriaDescripcion(oVideoConfiguracionModeloL[0])),
                                                           conn);


                    //------------------------------------------------------------
                    //Modificamos la configuracion de tiempos por categoria
                    //------------------------------------------------------------
                    foreach (VideoCategoria item in oVideoCategoriaL)
                    {

                        VideoDt.updConfiguracionCategoria(item, conn);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                               "M",
                                                               getAuditoriaCodigoRegistro(item),
                                                               getAuditoriaDescripcion(item)),
                                                               conn);
                    }


                    //------------------------------------------------------------
                    //Modificamos la configuracion de Acciones 
                    //------------------------------------------------------------
                    foreach (VideoAccion item in oVideoAccionL)
                    {

                        VideoDt.updConfiguracionAccion(item, conn);

                        if( item.esComienzaGrabacionC1 || item.esComienzaGrabacionC2
                               || item.esFinalizaGrabacionC1 || item.esFinalizaGrabacionC2 )
                        {
                            //Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                                   "M",
                                                                   getAuditoriaCodigoRegistro(item),
                                                                   getAuditoriaDescripcion(item)),
                                                                   conn);
                        }
                    }


                    //------------------------------------------------------------
                    //Modificamos la configuracion de Eventos
                    //------------------------------------------------------------
                    foreach (VideoEvento item in oVideoEventoL)
                    {

                        VideoDt.updConfiguracionEvento(item, conn);

                        if( item.AlmacenamientoC2.Codigo == "S" || item.AlmacenamientoC2.Codigo == "M"
                            || item.AlmacenamientoC2.Codigo == "S" || item.AlmacenamientoC2.Codigo == "M")
                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                               "M",
                                                               getAuditoriaCodigoRegistro(item),
                                                               getAuditoriaDescripcion(item)),
                                                               conn);
                    }

                   
                    
                    
                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region EVENTOCODIGOS

        /// ****************************************************************************************** <summary>
        /// Devuelve la lista codigos de eventos de captura de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoEventoCodigoL getCodigosEventos()
        {
            return getCodigosEventos(false);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista codigos de eventos de captura de video
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <returns>Lista de Codigos de eventos de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoEventoCodigoL getCodigosEventos(bool bConsolidado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getCodigosEventos(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        #endregion


        #region ACCIONCODIGOS

        /// ****************************************************************************************** <summary>
        /// Devuelve la lista codigos de acciones de captura de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoAccionCodigoL getCodigosAcciones()
        {
            return getCodigosAcciones(false);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista codigos de acciones de captura de video
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <returns>Lista de Codigos de acciones de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoAccionCodigoL getCodigosAcciones(bool bConsolidado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getCodigosAcciones(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region ACCIONES


        /// ****************************************************************************************** <summary>
        /// Devuelve la lista de configuracion de acciones de captura de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoAccionL getConfiguracionAcciones(string ModeloVia)
        {
            return getConfiguracionAcciones(false, ModeloVia);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de configuracion de acciones de captura de video
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar</param>
        /// <returns>Lista de Acciones de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoAccionL getConfiguracionAcciones(bool bConsolidado, string ModeloVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getConfiguracionAcciones(conn, ModeloVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoAccion oVideoAccion)
            {
                return oVideoAccion.ModeloVia.Modelo;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoAccion oVideoAccion)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Acción", oVideoAccion.Accion.Descripcion);
                if( oVideoAccion.esComienzaGrabacionC1 )
                    AuditoriaBs.AppendCampo(sb, "Comienzo Camara (1)", Traduccion.getSi());
                if (oVideoAccion.esFinalizaGrabacionC1)
                    AuditoriaBs.AppendCampo(sb, "Fin Camara (1)", Traduccion.getSi());
                if (oVideoAccion.esComienzaGrabacionC2)
                    AuditoriaBs.AppendCampo(sb, "Comienzo Camara (2)", Traduccion.getSi());
                if (oVideoAccion.esFinalizaGrabacionC2)
                    AuditoriaBs.AppendCampo(sb, "Fin Camara (2)", Traduccion.getSi());

                return sb.ToString();
            }

            #endregion

        #endregion


        #region CATEGORIAS

        /// ****************************************************************************************** <summary>
        /// Devuelve la lista de configuracion de captura de video por categoria (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoCategoriaL getConfiguracionCategorias(string ModeloVia)
        {
            return getConfiguracionCategorias(false, ModeloVia);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de configuracion de captura de video por categoria
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar</param>
        /// <returns>Lista de Configuracion de captura de video para las categorias</returns>
        /// ***********************************************************************************************
        public static VideoCategoriaL getConfiguracionCategorias(bool bConsolidado, string ModeloVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getConfiguracionCateogorias(conn, ModeloVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoCategoria oVideoCategoria)
            {
                return oVideoCategoria.ModeloVia.Modelo ;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoCategoria oVideoCategoria)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Categoría", oVideoCategoria.Categoria.Categoria.ToString());
                AuditoriaBs.AppendCampo(sb, "Tiempo (1)", oVideoCategoria.TiempoMaximoGrabacionC1.ToString());
                AuditoriaBs.AppendCampo(sb, "Almacena (1)", oVideoCategoria.AlmacenamientoC1.Descripcion);                 
                AuditoriaBs.AppendCampo(sb, "Porcentaje (1)", oVideoCategoria.PorcentajeMuestreoC1.ToString());
                AuditoriaBs.AppendCampo(sb, "Tiempo (2)", oVideoCategoria.TiempoMaximoGrabacionC2.ToString());
                AuditoriaBs.AppendCampo(sb, "Almacena (2)", oVideoCategoria.AlmacenamientoC2.Descripcion);                 
                AuditoriaBs.AppendCampo(sb, "Porcentaje (2)", oVideoCategoria.PorcentajeMuestreoC2.ToString());

                return sb.ToString();
            }

            #endregion

        #endregion


        #region CONFIGURACIONMODELO

        /// ****************************************************************************************** <summary>
        /// Devuelve la configuracion general de captura de video para el modelo de via indicado (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoConfiguracionModeloL getConfiguracionModelo(string ModeloVia)
        {
            return getConfiguracionModelo(false, ModeloVia);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la configuracion general de captura de video para el modelo de via indicado
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar</param>
        /// <returns>Configuracion general de captura de video para el modelo indicado</returns>
        /// ***********************************************************************************************
        public static VideoConfiguracionModeloL getConfiguracionModelo(bool bConsolidado, string ModeloVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getConfiguracionModelo(conn, ModeloVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoConfiguracionModelo oVideoConfiguracionModelo)
            {
                return oVideoConfiguracionModelo.Modelo.Modelo;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoConfiguracionModelo oVideoConfiguracionModelo)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Cuadros por Segundo", oVideoConfiguracionModelo.CuadrosPorSegundo.ToString());

                return sb.ToString();
            }

            #endregion

        #endregion


        #region EVENTOS

        /// ****************************************************************************************** <summary>
        /// Devuelve la lista de Eventos de captura de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoEventoL getConfiguracionEventos(string ModeloVia)
        {
            return getConfiguracionEventos(false, ModeloVia);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Eventos de captura de video
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar</param>
        /// <returns>Lista de Eventos de captura de video</returns>
        /// ***********************************************************************************************
        public static VideoEventoL getConfiguracionEventos(bool bConsolidado, string ModeloVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return VideoDt.getConfiguracionEventos(conn, ModeloVia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoEvento oVideoEvento)
            {
                return oVideoEvento.ModeloVia.Modelo;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoEvento oVideoEvento)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Evento", oVideoEvento.Evento.Descripcion);
                if( oVideoEvento.AlmacenamientoC1.Codigo == "S" || oVideoEvento.AlmacenamientoC1.Codigo == "M")
                {
                    AuditoriaBs.AppendCampo(sb, "Almacena Camara (1)", oVideoEvento.AlmacenamientoC1.Descripcion);
                    AuditoriaBs.AppendCampo(sb, "Porcentaje Camara (1)", oVideoEvento.PorcentajeMuestreoC1.ToString());
                }
                if( oVideoEvento.AlmacenamientoC2.Codigo == "S" || oVideoEvento.AlmacenamientoC2.Codigo == "M")
                {
                    AuditoriaBs.AppendCampo(sb, "Almacena Camara (2)", oVideoEvento.AlmacenamientoC2.Descripcion);
                    AuditoriaBs.AppendCampo(sb, "Porcentaje Camara (2)", oVideoEvento.PorcentajeMuestreoC2.ToString());
                }
                return sb.ToString();
            }

            #endregion


        #endregion


        #region CONFIGURACION

        /// ****************************************************************************************** <summary>
        /// Devuelve la configuracion general de captura de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoConfiguracionL getConfiguracion()
        {
            return getConfiguracion(ConexionBs.getGSToEstacion());
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la configuracion general de captura de video 
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <returns>Configuracion general de captura de video </returns>
        /// ***********************************************************************************************
        public static VideoConfiguracionL getConfiguracion(bool bGST)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return VideoDt.getConfiguracion(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de la configuracion de dias de rotacion y borrado
        /// </summary>
        /// <param name="oVideoConfig">VideoConfiguracion - Objeto que contiene la informacion a modificar
        /// ***********************************************************************************************
        public static void updConfiguracion(VideoConfiguracion oVideoConfig)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    //Modificamos el directorio de rotacion
                    VideoDt.updConfiguracion(oVideoConfig, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oVideoConfig),
                                                           getAuditoriaDescripcion(oVideoConfig)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoConfiguracion oVideoConfig)
            {
                return " ";
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoConfiguracion oVideoConfig)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Cantidad de días de Rotación",oVideoConfig.DiasRotacionDirectorios.ToString());
                AuditoriaBs.AppendCampo(sb, "Cantidad de días para Eliminarlos", oVideoConfig.DiasBorradoVideos.ToString());

                return sb.ToString();
            }

            #endregion

        #endregion


        #region PATHS

        /// ****************************************************************************************** <summary>
        /// Devuelve la lista de directorios de rotacion de video (por defecto para bConsolidado = false; todas las rotaciones)
        /// </summary>********************************************************************************
        public static VideoPathL getCarpetasRotacion()
        {
            return getCarpetasRotacion(ConexionBs.getGSToEstacion(), null);
        }


        /// ****************************************************************************************** <summary>
        /// Devuelve la lista de directorios de rotacion de video (por defecto para bConsolidado = false)
        /// </summary>********************************************************************************
        public static VideoPathL getCarpetasRotacion(int? IDRegistro)
        {
            return getCarpetasRotacion(ConexionBs.getGSToEstacion(), IDRegistro);
        }

        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de directorios de rotacion de video 
        /// </summary>
        /// <param name="bConsolidado">bool - Si se conecta a consolidado o a la estacion</param>
        /// <param name="IDRegistro">int - ID del registro de rotacion. Si pasamos null traemos todas las rotaciones</param>
        /// <returns>Lista de directorios de rotacion de video</returns>
        /// ***********************************************************************************************
        public static VideoPathL getCarpetasRotacion(bool bGST, int? IDRegistro)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return VideoDt.getCarpetasRotacion(conn, IDRegistro);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un directorio de Rotacion de Video
        /// </summary>
        /// <param name="oVideopath">VideoPath - Objeto que contiene el directorio de rotacion de video a insertar
        /// ***********************************************************************************************
        public static void addCarpetaRotacion(VideoPath oVideoPath)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    //Agregamos el directorio de rotacion
                    VideoDt.addCarpetaRotacion(oVideoPath, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                          "A",
                                                          getAuditoriaCodigoRegistro(oVideoPath),
                                                          getAuditoriaDescripcion(oVideoPath)),
                                                          conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de un directorio de rotacion de video
        /// </summary>
        /// <param name="oVideopath">VideoPath - Objeto que contiene el directorio de rotacion de video a modificar
        /// ***********************************************************************************************
        public static void updCarpetaRotacion(VideoPath oVideoPath)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Gestion o Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(),true);

                    //Modificamos el directorio de rotacion
                    VideoDt.updCarpetaRotacion(oVideoPath, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oVideoPath),
                                                           getAuditoriaDescripcion(oVideoPath)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Baja de un directorio de rotacion de video
        /// </summary>
        /// <param name="oVideopath">VideoPath - Objeto que contiene el directorio de rotacion de video a modificar
        /// ***********************************************************************************************
        public static void delCarpetaRotacion(VideoPath oVideoPath)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Gestion o Estacion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);

                    //eliminamos un directorio de rotacion 
                    VideoDt.delCarpetaRotacion(oVideoPath, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oVideoPath),
                                                           getAuditoriaDescripcion(oVideoPath)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(VideoPath oVideoPath)
            {
                return oVideoPath.IDRegistro.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(VideoPath oVideoPath)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Código", oVideoPath.IDRegistro.ToString());
                AuditoriaBs.AppendCampo(sb, "Directorio de Acceso por Terminal", oVideoPath.PathCarpeta);
                AuditoriaBs.AppendCampo(sb, "Directorio de Acceso por FTP", oVideoPath.PathCarpetaFTP);

                return sb.ToString();
            }

            #endregion


        #endregion


        #region TIPOSALMACENAMIENTO
            
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los tipos de almacenamiento de los videos
        /// </summary>
        /// <returns>Lista de Tipos de Almacenamiento</returns>
        /// ***********************************************************************************************
        public static VideoConfiguracionAlmacenamientoL getTiposAlmacenamiento()
        {
            VideoConfiguracionAlmacenamientoL oVideoConfiguracionAlmacenamientoL = new VideoConfiguracionAlmacenamientoL();
            oVideoConfiguracionAlmacenamientoL = VideoDt.getTiposAlmacenamiento();


            return oVideoConfiguracionAlmacenamientoL;
        }

        #endregion


        #region AUDITORIA_GENERAL

            ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// Es el mismo codigo para toda la configuracion de video. La descripcion y codigo es particular de cada entidad
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "VID";
        }

        #endregion



    }
}

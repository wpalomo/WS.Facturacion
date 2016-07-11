using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class MensajeBS
    {
        #region MENSAJE DE LA VIA: Metodos de la Clase Mensaje de la Via.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los mensajes definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoL getMensajes()
        {
            return getMensajes(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los mensajes definidos. 
        /// </summary>
        /// <param name="codigoMensaje">Int - Permite filtrar por un mensaje determinado
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoL getMensajes(int? codigoMensaje)
        {
            return getMensajes(ConexionBs.getGSToEstacion(), codigoMensaje);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de mensajes definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMensaje">Int - Permite filtrar por un mensaje determinada
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoL getMensajes(bool bGST, int? codigoMensaje)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return MensajeDT.getMensajes(conn, codigoMensaje);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Mensaje
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Estructura del Mensaje a insertar
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static void addMensaje(MensajePredefinido oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que no exista el Mensaje

                    //Agregamos Mensaje
                    MensajeDT.addMensajes(oMensaje, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensaje(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oMensaje),
                                                           getAuditoriaDescripcion(oMensaje)),
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
        /// Modificacion de un Mensaje
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Estructura del mensaje a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMensaje(MensajePredefinido oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que ya exista el Mensaje

                    //Modificamos Mensaje
                    MensajeDT.updMensajes(oMensaje, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensaje(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oMensaje),
                                                           getAuditoriaDescripcion(oMensaje)),
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
        /// Eliminacion de un Mensaje
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Estructura del Mensaje a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMensaje(MensajePredefinido oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verficamos que haya registros para el Mensaje

                    //eliminamos el Mensaje
                    MensajeDT.delMensaje(oMensaje, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensaje(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oMensaje),
                                       getAuditoriaDescripcion(oMensaje)),
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


        #region MENSAJE DE SUPERVISION: Metodos de la Clase Mensaje de Supervisión.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los mensajes definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoSupL getMensajesSup()
        {
            return getMensajesSup(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los mensajes definidos. 
        /// </summary>
        /// <param name="codigoMensaje">Int - Permite filtrar por un mensaje determinado
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoSupL getMensajesSup(int? codigoMensaje)
        {
            return getMensajesSup(ConexionBs.getGSToEstacion(), codigoMensaje);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de mensajes definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMensaje">Int - Permite filtrar por un mensaje determinada
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoSupL getMensajesSup(bool bGST, int? codigoMensaje)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return MensajeDT.getMensajesSup(conn, codigoMensaje);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Mensaje
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Estructura del Mensaje a insertar
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static void addMensajeSup(MensajePredefinidoSup oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Plaza, con transaccion
                    conn.ConectarGSTPlaza(false, true);

                    //Verificamos que no exista el Mensaje

                    //Agregamos Mensaje
                    MensajeDT.addMensajeSup(oMensaje, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensajeSup(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroSup(oMensaje),
                                                           getAuditoriaDescripcionSup(oMensaje)),
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
        /// Eliminacion de un Mensaje
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Estructura del Mensaje a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMensajeSup(MensajePredefinidoSup oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Plaza, con transaccion
                    conn.ConectarGSTPlaza(false, true);

                    //Verficamos que haya registros para el Mensaje

                    //eliminamos el Mensaje
                    MensajeDT.delMensajeSup(oMensaje, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensajeSup(),
                                       "B",
                                       getAuditoriaCodigoRegistroSup(oMensaje),
                                       getAuditoriaDescripcionSup(oMensaje)),
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

        #endregion

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaMensaje()
        {
            return "MSG";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(MensajePredefinido oMensaje)
        {
            return oMensaje.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(MensajePredefinido oMensaje)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Texto", oMensaje.Descripcion);

            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaMensajeSup()
        {
            return "EMV";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroSup(MensajePredefinidoSup oMensaje)
        {
            return oMensaje.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionSup(MensajePredefinidoSup oMensaje)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Código", oMensaje.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Texto", oMensaje.Descripcion);

            return sb.ToString();
        }

        #endregion


        #endregion





        #region MENSAJES RECIBIDOS DE LA VIA: Metodos de la Clase Mensaje de la Via.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los mensajes recibidos pendientes de ver
        /// </summary>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajeRecibidoViaL getMensajesRecibidosVia()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return MensajeDT.getMensajesRecibidosVia(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad mensajes recibidos pendientes de ver
        /// </summary>
        /// <returns>Int</returns>
        /// ***********************************************************************************************
        public static int getHayMensajesRecibidosVia()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return MensajeDT.getHayMensajesRecibidosVia(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Mensaje Recibido
        /// </summary>
        /// <param name="oMensaje">MensajeRecibidoVia - Estructura del Mensaje a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMensajeRecibidoVia(MensajeRecibidoVia oMensaje)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(false,true);

                    //eliminamos el Mensaje
                    MensajeDT.delMensajeRecibidoVia(oMensaje, conn);


                    ////Grabamos auditoria
                    //AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensaje(),
                    //                   "B",
                    //                   getAuditoriaCodigoRegistro(oMensaje),
                    //                   getAuditoriaDescripcion(oMensaje)),
                    //                   conn);

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

    }
}

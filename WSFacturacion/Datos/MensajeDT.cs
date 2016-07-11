using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class MensajeDT
    {

        #region MensajeDT: Clase de Datos de Mensaje de la Via.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo de mensaje a filtrar</param>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoL getMensajes(Conexion oConn, int? codigoMensaje)
        {
            MensajePredefinidoL oMensajes = new MensajePredefinidoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesVias_GetMensajesVia";
                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = codigoMensaje;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMensajes.Add(CargarMensajes(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMensajes;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Mensajes</param>
        /// <returns>Lista con el elemento Mensaje de la base de datos</returns>
        /// ***********************************************************************************************
        private static MensajePredefinido CargarMensajes(System.Data.IDataReader oDR)
        {
            MensajePredefinido oMensajePredefinido = new MensajePredefinido((byte)oDR["men_codig"],
                                  oDR["men_texto"].ToString());

            return oMensajePredefinido;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Mensaje de la Via en la base de datos
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addMensajes(MensajePredefinido oMensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesVias_addMensajesVia";

                oCmd.Parameters.Add("men_codig", SqlDbType.TinyInt).Value = oMensaje.Codigo;
                oCmd.Parameters.Add("men_texto", SqlDbType.VarChar, 80).Value = oMensaje.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este Código de Mensaje ya existe");
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
        /// Modifica un mensaje de la Via en la base de datos
        /// </summary>
        /// <param name="oMensaje">MensajePredefinido - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMensajes(MensajePredefinido oMensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesVias_updMensajesVia";

                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = oMensaje.Codigo;
                oCmd.Parameters.Add("@men_texto", SqlDbType.VarChar, 80).Value = oMensaje.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del número de la Via");
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
        /// Elimina un mensaje de la Via de la base de datos
        /// </summary>
        /// <param name="Mensaje">Int - Número de Via a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMensaje(int Mensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesVias_delMensajesVia";

                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = Mensaje;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Número de Via");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Mensaje no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
            return;
        }

        public static void delMensaje(MensajePredefinido oMensaje, Conexion oConn)
        {
            delMensaje(oMensaje.Codigo, oConn);
        }

        #endregion

        #region MensajeDT SUPERVISION: Clase de Datos de Mensaje de Supervisión.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo de mensaje a filtrar</param>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajePredefinidoSupL getMensajesSup(Conexion oConn, int? codigoMensaje)
        {
            MensajePredefinidoSupL oMensajes = new MensajePredefinidoSupL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesSup_GetMensajesSup";
                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = codigoMensaje;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMensajes.Add(CargarMensajesSup(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMensajes;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Mensajes</param>
        /// <returns>Lista con el elemento Mensaje de la base de datos</returns>
        /// ***********************************************************************************************
        private static MensajePredefinidoSup CargarMensajesSup(System.Data.IDataReader oDR)
        {
            MensajePredefinidoSup oMensajePredefinido = new MensajePredefinidoSup((byte)oDR["men_codig"],
                                  oDR["men_texto"].ToString());

            return oMensajePredefinido;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Mensaje de Supervisión en la base de datos
        /// </summary>
        /// <param name="oMensaje">MensajePredefinidoSup - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addMensajeSup(MensajePredefinidoSup oMensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesSup_addMensajesSup";

                oCmd.Parameters.Add("men_texto", SqlDbType.VarChar, 80).Value = oMensaje.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este Código de Mensaje ya existe");
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
        /// Elimina un mensaje de Supervisión de la base de datos
        /// </summary>
        /// <param name="Mensaje">Int - Código de Mensaje a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMensajeSup(int Mensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesSup_delMensajesSup";

                oCmd.Parameters.Add("@men_codig", SqlDbType.TinyInt).Value = Mensaje;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Número de Via");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Mensaje no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
            return;
        }

        public static void delMensajeSup(MensajePredefinidoSup oMensaje, Conexion oConn)
        {
            delMensajeSup(oMensaje.Codigo, oConn);
        }

        #endregion


        #region MensajeDT MENSAJESRECIBIDOSVIA: Clase de Datos de Mensaje Recibidos de la Via.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes Recibidos de la Vía Pendientes de ver
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static MensajeRecibidoViaL getMensajesRecibidosVia(Conexion oConn)
        {
            MensajeRecibidoViaL oMensajes = new MensajeRecibidoViaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesRecibidosVia_GetMensajes";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMensajes.Add(CargarMensajeRecibidoVia(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMensajes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de mensajes pendientes de ver 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Cantidad de mensajes pendientes </returns>
        /// ***********************************************************************************************
        public static int getHayMensajesRecibidosVia(Conexion oConn)
        {
            int retorno = 0;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesRecibidosVia_GetHayMensajes";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    retorno = (int)oDR["Cantidad"];
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retorno;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Mensajes</param>
        /// <returns>Lista con el elemento Mensaje de la base de datos</returns>
        /// ***********************************************************************************************
        private static MensajeRecibidoVia CargarMensajeRecibidoVia(System.Data.IDataReader oDR)
        {
            MensajeRecibidoVia oMensajeRecibido = new MensajeRecibidoVia(Convert.ToInt16(oDR["men_coest"]),
                Convert.ToByte(oDR["men_nuvia"]), Convert.ToDateTime(oDR["men_fecha"]), new MensajePredefinido(Convert.ToInt32(oDR["men_codig"]),
                    Convert.ToString(oDR["men_texto"])), Convert.ToString(oDR["men_visto"]), Convert.ToString(oDR["via_nombr"]));

            return oMensajeRecibido;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un mensaje recibido de la Via de la base de datos
        /// </summary>
        /// <param name="Mensaje">MensajeRecibidoVia - Objeto Mensaje a borrar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMensajeRecibidoVia(MensajeRecibidoVia Mensaje, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajesRecibidosVia_delMensaje";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Mensaje.Estacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = Mensaje.Via.NumeroVia;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = Mensaje.Fecha;
                oCmd.Parameters.Add("@mensaje", SqlDbType.TinyInt).Value = Mensaje.Mensaje.Codigo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Número de Via");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Mensaje no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
            return;
        }


        #endregion




    }
}

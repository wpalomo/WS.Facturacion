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
    public class AccionDT
    {


        #region AccionDT AccionDT: Clase de Datos de Acciones.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Acciones Pendientes de Supervisión.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Acciones</returns>
        /// ***********************************************************************************************
        public static AccionL getAccionesPendientes(Conexion oConn)
        {
            AccionL oAcciones = new AccionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_getAccionesPendientes";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAcciones.Add(CargarAccionPendiente(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAcciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Acciones Pendientes de Supervisión.
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Acciones</returns>
        /// ***********************************************************************************************
        public static AccionL getAccionesHistorico(Conexion oConn)
        {
            AccionL oAcciones = new AccionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_getAccionesHistorico";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAcciones.Add(CargarAccionHistorico(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAcciones;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Acciones de Supervisión
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Acciones de Supervisión</param>
        /// <returns>Lista con el elemento Acciones de Supervisión de la base de datos</returns>
        /// ***********************************************************************************************
        private static Accion CargarAccionPendiente(System.Data.IDataReader oDR)
        {
            Accion oAccion = new Accion();
            oAccion.ID = Convert.ToInt32(oDR["ast_ident"]);
            oAccion.Estacion = Convert.ToInt32(oDR["ast_coest"]);
            oAccion.Via = Convert.ToInt32(oDR["ast_nuvia"]);
            oAccion.FechaGen = Convert.ToDateTime(oDR["ast_fegen"]);
            oAccion.Modo = Convert.ToString(oDR["ast_modo"]);
            oAccion.TipoAlarma = Convert.ToInt32(oDR["ast_tipal"]);
            oAccion.TipoAlarmaDescr = Convert.ToString(oDR["cal_descr"]);
            oAccion.Estado = Convert.ToString(oDR["ast_estad"]);
            oAccion.UsuarioAutorizo = Convert.ToString(oDR["ast_usera"]);
            oAccion.TerminalAutorizo = Convert.ToString(oDR["ast_terma"]);
            if(oDR["ast_cadac"] != DBNull.Value)
                oAccion.CategoriaDAC = Convert.ToInt32(oDR["ast_cadac"]);
            if (oDR["ast_fotov"] != DBNull.Value)
                oAccion.NombreFoto = oDR["ast_fotov"].ToString();
            if (oDR["ast_numtr"] != DBNull.Value)
                oAccion.Numtran = Convert.ToInt32(oDR["ast_numtr"]);
            if (oDR["ast_alarm"] != DBNull.Value)
                oAccion.IDAlarma = Convert.ToInt32(oDR["ast_alarm"]);

            return oAccion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Acciones de Supervisión
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Acciones de Supervisión</param>
        /// <returns>Lista con el elemento Acciones de Supervisión de la base de datos</returns>
        /// ***********************************************************************************************
        private static Accion CargarAccionHistorico(System.Data.IDataReader oDR)
        {
            Accion oAccion = new Accion();
            oAccion.ID = Convert.ToInt32(oDR["ast_ident"]);
            oAccion.Estacion = Convert.ToInt32(oDR["ast_coest"]);
            oAccion.Via = Convert.ToInt32(oDR["ast_nuvia"]);
            oAccion.FechaGen = Convert.ToDateTime(oDR["ast_fegen"]);
            oAccion.Modo = Convert.ToString(oDR["ast_modo"]);
            oAccion.TipoAlarma = Convert.ToInt32(oDR["ast_tipal"]);
            oAccion.Estado = Convert.ToString(oDR["ast_estad"]);
            oAccion.UsuarioAutorizo = Convert.ToString(oDR["ast_usera"]);
            oAccion.TerminalAutorizo = Convert.ToString(oDR["ast_terma"]);
            oAccion.FechaAutorizo = Convert.ToDateTime(oDR["ast_fecha"]);
            oAccion.SubFp = Convert.ToInt32(oDR["ast_subfp"]);
            oAccion.Patente = Convert.ToString(oDR["ast_paten"]);
            oAccion.NumeroTag = Convert.ToString(oDR["ast_numer"]);
            oAccion.CategoriaDescr = Convert.ToString(oDR["cat_descr"]);
            oAccion.CodigoCausa = Convert.ToInt32(oDR["ast_causa"]);
            oAccion.Causa = Convert.ToString(oDR["cod_descr"]);
            oAccion.Comentario = Convert.ToString(oDR["ast_comen"]);
            if(oDR["ast_fefin"] != DBNull.Value)
                oAccion.FechaFin = Convert.ToDateTime(oDR["ast_fefin"]);
            if (oDR["ast_cafin"] != DBNull.Value)
                oAccion.CausaFin = Convert.ToString(oDR["ast_cafin"]);
            if (oDR["ast_cadac"] != DBNull.Value)
                oAccion.CategoriaDAC = Convert.ToInt32(oDR["ast_cadac"]);
            if (oDR["ast_fotov"] != DBNull.Value)
                oAccion.NombreFoto = oDR["ast_fotov"].ToString();
            if (oDR["ast_numtr"] != DBNull.Value)
                oAccion.Numtran = Convert.ToInt32(oDR["ast_numtr"]);
            if (oDR["for_descr"] != DBNull.Value)
                oAccion.DescrForpag = oDR["for_descr"].ToString();
            

            return oAccion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la accion como en curso en la base de datos
        /// </summary>
        /// <param name="oAccion">Accion - Objeto con la informacion de la accion en curso a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setAccionEnCurso(Accion oAccion, Conexion oConn)
        {
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_setAccionEnCurso";
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oAccion.ID;                
                oCmd.Parameters.Add("@Usera", SqlDbType.VarChar, 10).Value = oAccion.UsuarioAutorizo;
                oCmd.Parameters.Add("@Terma", SqlDbType.VarChar, 100).Value = oAccion.TerminalAutorizo;

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
        /// Agrega la accion como autorizada en la base de datos
        /// </summary>
        /// <param name="oAccion">Accion - Objeto con la informacion de la accion autorizada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setAccionAutorizada(Accion oAccion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_setAccionAutorizada";
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oAccion.ID;
                oCmd.Parameters.Add("@Usera", SqlDbType.VarChar, 10).Value = oAccion.UsuarioAutorizo;
                oCmd.Parameters.Add("@Terma", SqlDbType.VarChar, 100).Value = oAccion.TerminalAutorizo;
                oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = oAccion.FechaAutorizo;
                oCmd.Parameters.Add("@SubFp", SqlDbType.TinyInt).Value = oAccion.SubFp;
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oAccion.Patente;
                oCmd.Parameters.Add("@Numer", SqlDbType.VarChar, 15).Value = oAccion.NumeroTag;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oAccion.Categoria;
                oCmd.Parameters.Add("@Causa", SqlDbType.VarChar, 255).Value = oAccion.CodigoCausa;
                oCmd.Parameters.Add("@Comen", SqlDbType.VarChar, 2000).Value = oAccion.Comentario;
                oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = oAccion.tipop;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = oAccion.tipbo;

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
        /// Inserta la accion como finalizada en la base de datos
        /// </summary>
        /// <param name="oAccion">Accion - Objeto con la informacion de la accion autorizada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static int setAccionFinalizada(Accion oAccion, Conexion oConn)
        {
            int retval = -1;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_setAccionFinalizada";
                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oAccion.Estacion;
                oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = oAccion.Via;
                oCmd.Parameters.Add("@Fegen", SqlDbType.DateTime).Value = oAccion.FechaGen;
                oCmd.Parameters.Add("@Modo", SqlDbType.VarChar, 2).Value = oAccion.Modo;
                oCmd.Parameters.Add("@Tipal", SqlDbType.TinyInt).Value = oAccion.TipoAlarma;
                oCmd.Parameters.Add("@Estad", SqlDbType.VarChar, 1).Value = oAccion.Estado;
                oCmd.Parameters.Add("@Usera", SqlDbType.VarChar, 10).Value = oAccion.UsuarioAutorizo;
                oCmd.Parameters.Add("@Terma", SqlDbType.VarChar, 100).Value = oAccion.TerminalAutorizo;
                oCmd.Parameters.Add("@SubFp", SqlDbType.TinyInt).Value = oAccion.SubFp;
                oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = oAccion.Patente;
                oCmd.Parameters.Add("@Numer", SqlDbType.VarChar, 15).Value = oAccion.NumeroTag;
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oAccion.Categoria;
                oCmd.Parameters.Add("@Causa", SqlDbType.VarChar, 255).Value = oAccion.CodigoCausa;
                oCmd.Parameters.Add("@Comen", SqlDbType.VarChar, 2000).Value = oAccion.Comentario;
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oAccion.ID;
                oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = oAccion.tipop;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = oAccion.tipbo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval < (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Pone la accion En Curso como Pendiente en la base de datos
        /// </summary>
        /// <param name="oAccion">Accion - Objeto con la informacion de la accion autorizada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setAccionPendiente(int Ident, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_setAccionPendiente";
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = Ident;

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

        #endregion



        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oAccion"></param>
        public static void setAccionSiguioConTag(Conexion oConn,Accion oAccion)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_setAccionSiguioConTag";
                oCmd.Parameters.Add("@NumTran", SqlDbType.Int).Value = oAccion.Numtran;
                oCmd.Parameters.Add("@Patente", SqlDbType.VarChar, 8).Value = oAccion.Patente;
                oCmd.Parameters.Add("@Tag", SqlDbType.VarChar,24).Value = oAccion.NumeroTag;
                oCmd.Parameters.Add("@Emisor", SqlDbType.VarChar, 5).Value = oAccion.Emisor;
                oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = oAccion.tipop;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = oAccion.tipbo;
                oCmd.Parameters.Add("@categ", SqlDbType.TinyInt).Value = oAccion.Categoria;

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
    }
}

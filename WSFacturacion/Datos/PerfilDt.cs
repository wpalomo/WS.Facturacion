using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class PerfilDt
    {
        #region PERFIL

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Perfiles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="perfil">string - Codigo del perfil para la busqueda</param>
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static DataSet rptPerfiles(Conexion oConn, string perfil)
        {
            DataSet dsPerfiles = new DataSet();
            dsPerfiles.DataSetName = "RptPerfiles_PerfilesDS";   

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Perfiles_getPerfiles";
            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;

            oCmd.CommandTimeout = 3600;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPerfiles, "Perfiles");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsPerfiles;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Perfiles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="perfil">string - Codigo del perfil para la busqueda</param>
        /// <returns>Lista de Perfiles</returns>
        /// ***********************************************************************************************
        public static PerfilL getPerfiles(Conexion oConn, string perfil)
        {
            PerfilL oPefiles = new PerfilL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Perfiles_getPerfiles";
            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar,30).Value = perfil;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oPefiles.Add(CargarPerfil(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oPefiles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Pefil
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Perfiles</param>
        /// <returns>Objeto Perfil con los datos</returns>
        /// ***********************************************************************************************
        private static Perfil CargarPerfil(System.Data.IDataReader oDR)
        {
            Perfil oPerfil = new Perfil();
            oPerfil.Codigo = oDR["gru_grupo"].ToString();
            oPerfil.Descripcion = oDR["gru_visua"].ToString();
            oPerfil.NivelVia = new PerfilNivel((byte)oDR["gru_nivel"], oDR["niv_descr"].ToString());
            oPerfil.OpcionesModificacion = oDR["gru_modif"].ToString();

            oPerfil.Permisos = null;
            oPerfil.PermisosEventos = null;
            oPerfil.Jerarquia = null;

            return oPerfil;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un perfil en la base de datos
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto con la informacion del perfil a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addPerfil(Perfil oPerfil, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Perfiles_addPerfil";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = oPerfil.Codigo;
            oCmd.Parameters.Add("@visua", SqlDbType.VarChar, 30).Value = oPerfil.Descripcion;
            oCmd.Parameters.Add("@nivel", SqlDbType.TinyInt).Value = oPerfil.NivelVia.Codigo;

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
                {
                    msg = Traduccion.Traducir("Este codigo de perfil ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este Código de perfil fue eliminado");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un perfil en la base de datos
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto con la informacion del perfil a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updPerfil(Perfil oPerfil, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Perfiles_updPerfil";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = oPerfil.Codigo;
            oCmd.Parameters.Add("@visua", SqlDbType.VarChar, 30).Value = oPerfil.Descripcion;
            oCmd.Parameters.Add("@nivel", SqlDbType.TinyInt).Value = oPerfil.NivelVia.Codigo;

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
                {
                    msg = Traduccion.Traducir("No existe el registro del perfil");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un perfil en la base de datos
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delPerfil(string perfil, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Perfiles_delPerfil";

                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;

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
                    {
                        msg = Traduccion.Traducir("No existe el registro del perfil");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex,Traduccion.Traducir("El Perfil no se puede dar de baja porque está siendo utilizado"));
                throw;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un perfil en la base de datos, invocando a otro metodo que lo hace efectivo
        /// </summary>
        /// <param name="oPerfil">Perfil - Objeto de perfil a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delPerfil(Perfil oPerfil, Conexion oConn)
        {
            delPerfil(oPerfil.Codigo, oConn);
        }

        #endregion

        #region PERFILJERARQUIA

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Jerarquias de un perfil
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Perfiles controlados</returns>
        /// ***********************************************************************************************
        public static void rptJerarquias(Conexion oConn, DataSet dsPerfil)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Jerarquias_rptJerarquias";

            oCmd.CommandTimeout = 3600;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPerfil, "Jerarquias");


            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Jerarquias de un perfil
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="perfil">string - Codigo del Perfil para la busqueda</param>
        /// <returns>Lista de Perfiles controlados</returns>
        /// ***********************************************************************************************
        public static PerfilJerarquiaL getJerarquias(Conexion oConn, string perfil)
        {
            PerfilJerarquiaL oJerarquias = new PerfilJerarquiaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Jerarquias_GetJerarquias";
            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;

            oDR = oCmd.ExecuteReader();


            while (oDR.Read())
            {
                oJerarquias.Add(CargarJerarquia(oDR, perfil));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oJerarquias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Jerarquia
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Perfiles</param>
        /// <param name="perfil">string - Perfil que controla</param>
        /// <returns>Objeto PerfilJerarquia con los datos</returns>
        /// ***********************************************************************************************
        private static PerfilJerarquia CargarJerarquia(System.Data.IDataReader oDR, string perfil)
        {
            PerfilJerarquia oJerarquia = new PerfilJerarquia();
            oJerarquia.Perfil = perfil;
            oJerarquia.PerfilMenor = new Perfil( oDR["gru_grupo"].ToString(), oDR["gru_visua"].ToString());
            oJerarquia.Controlado = !(oDR["gje_menor"] is DBNull);

            return oJerarquia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Jerarquia en la base de datos
        /// Si no existe la inserta, si existe la modifica
        /// </summary>
        /// <param name="perfil">string - codigo del perfil</param>
        /// <param name="oJerarquia">PerfilJerarquia - Datos de la Jeraraquia</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updJerarquia(string perfil, PerfilJerarquia oJerarquia, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Jerarquias_updJerarquia";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
            oCmd.Parameters.Add("@menor", SqlDbType.VarChar, 30).Value = oJerarquia.PerfilMenor.Codigo;

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
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todas las Jerarquias de un perfil en la base de datos
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delJerarquias(string perfil, Conexion oConn)
        {
            delJerarquia(perfil, null, oConn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina las Jerarquias de un perfil en la base de datos
        /// si oJerarquia es null elimina todas las jerarquias del perfil
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oJerarquia">PerfilJerarquia - datos de la Jerarquia a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delJerarquia(string perfil, PerfilJerarquia oJerarquia, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Jerarquias_delJerarquia";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
            string perfilmenor = null;
            if (oJerarquia != null)
                perfilmenor = oJerarquia.PerfilMenor.Codigo;
            oCmd.Parameters.Add("@menor", SqlDbType.VarChar, 30).Value = perfilmenor;

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
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Inserta los registros de Jerarquia de Grupos para el administrador
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addJerarquiasAdmin(Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Jerarquias_addJerarquiasAdmin";

            oCmd.ExecuteNonQuery();

            // Cerramos el objeto
            oCmd = null;
        }
        
        #endregion

        #region PERFILNIVEL
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Niveles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="nivel">int? - Codigo del nivel para la busqueda</param>
        /// <returns>Lista de Niveles</returns>
        /// ***********************************************************************************************
        public static PerfilNivelL getNiveles(Conexion oConn, int? nivel)
        {
            PerfilNivelL oNiveles = new PerfilNivelL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Niveles_getNiveles";
                oCmd.Parameters.Add("@nivel", SqlDbType.TinyInt).Value = nivel;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oNiveles.Add(CargarNivel(oDR));
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
            return oNiveles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto PefilNivel
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Niveles</param>
        /// <returns>Objeto PerfilNivel con los datos</returns>
        /// ***********************************************************************************************
        private static PerfilNivel CargarNivel(System.Data.IDataReader oDR)
        {
            try
            {
                PerfilNivel oNivel = new PerfilNivel();
                oNivel.Codigo = (byte) oDR["niv_nivel"];
                oNivel.Descripcion = oDR["niv_descr"].ToString();


                return oNivel;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region EVENTOPERFIL

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Eventos que puede ver un perfil
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Eventos Habilitados</returns>
        /// ***********************************************************************************************
        public static void rptEventos(Conexion oConn, DataSet dsPerfil)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_EventoPerfil_rptEventos";

            oCmd.CommandTimeout = 3600;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsPerfil, "Eventos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Eventos que puede ver un perfil
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="perfil">string - Codigo del Perfil para la busqueda</param>
        /// <returns>Lista de Eventos Habilitados</returns>
        /// ***********************************************************************************************
        public static EventoPerfilL getEventos(Conexion oConn, string perfil)
        {
            EventoPerfilL oEventos = new EventoPerfilL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_EventoPerfil_GetEventos";
            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;

            oDR = oCmd.ExecuteReader();
            
            while (oDR.Read())
            {
                oEventos.Add(CargarEvento(oDR, perfil));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oEventos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto EventoPerfil
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Perfiles</param>
        /// <param name="perfil">string - Perfil a buscar</param>
        /// <returns>Objeto EventoPerfil con los datos</returns>
        /// ***********************************************************************************************
        private static EventoPerfil CargarEvento(System.Data.IDataReader oDR, string perfil)
        {
            EventoPerfil oEvento = new EventoPerfil();
            oEvento.Evento = new ClaveEvento((short)oDR["cla_codev"], oDR["cla_descr"].ToString(), oDR["cla_tipom"].ToString());
            oEvento.Evento.TipoEvento.Descripcion = oDR["tcl_descr"].ToString();
            oEvento.Perfil = perfil;
            oEvento.Habilitado = !(oDR["cla_grupo"] is DBNull); 

            return oEvento;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un EventoPerfil en la base de datos
        /// Si no existe la inserta, si existe la modifica
        /// </summary>
        /// <param name="perfil">string - codigo del perfil</param>
        /// <param name="oEvento">EventoPerfil - Datos del Evento</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updEvento(string perfil, EventoPerfil oEvento, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_EventoPerfil_updEvento";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
            oCmd.Parameters.Add("@codev", SqlDbType.SmallInt).Value = oEvento.Evento.Codigo;

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
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todos los Permisos de eventos de un perfil en la base de datos
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEventos(string perfil, Conexion oConn)
        {
            delEvento(perfil, null, oConn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un permiso de evento en la base de datos
        /// si oEvento es null elimina todos los permisos de eventos del perfil
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oEvento">EventoPerfil - datos del evento a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEvento(string perfil, EventoPerfil oEvento, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_EventoPerfil_delEvento";

            oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
            short? codigoevento = null;
            if (oEvento != null)
            {
                codigoevento = oEvento.Evento.Codigo;
            }
            oCmd.Parameters.Add("@codev", SqlDbType.SmallInt).Value = codigoevento;

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
                throw new ErrorSPException(msg);
            }
        }
        
        #endregion
    }
}

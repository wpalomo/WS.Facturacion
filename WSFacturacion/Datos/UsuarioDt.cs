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
    public class UsuarioDt
    {
        #region USUARIO
        
        public static DataSet getRptUsuariosEstaciones(Conexion oConn, string nombre, int? estacion, bool incluirEliminados)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptPeaje_UsuariosEstacionesDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_getRptUsuariosEstaciones";
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 100).Value = nombre;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@delet", SqlDbType.Char, 1).Value = incluirEliminados;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsExentos, "UsuariosEstaciones");
                
                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsExentos;
        }

        public static DataSet getRptUsuarios(Conexion oConn, string nombre, int? estacion, bool incluirEliminados)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptPeaje_UsuariosDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_getUsuarios";
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = null;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 100).Value = nombre;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@delet", SqlDbType.Char, 1).Value = incluirEliminados;
                oCmd.Parameters.Add("@ntarj", SqlDbType.BigInt).Value = null;
                oCmd.Parameters.Add("@estlocal", SqlDbType.TinyInt).Value = null;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsExentos, "Usuarios");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsExentos;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene una lista de usuarios
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="id"></param>
        /// <param name="nombre"></param>
        /// <param name="estacion"></param>
        /// <param name="incluirEliminados"></param>
        /// <param name="tarjeta"></param>
        /// <param name="estacionLocal"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioL getUsuarios(Conexion oConn, string id, string nombre, int? estacion, bool incluirEliminados, long? tarjeta, int? estacionLocal)
        {
            UsuarioL oUsuarios = new UsuarioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_getUsuarios";
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = id;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 100).Value = nombre;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@delet", SqlDbType.Char, 1).Value = incluirEliminados ? "S" : "N";
                oCmd.Parameters.Add("@ntarj", SqlDbType.BigInt).Value = tarjeta;
                oCmd.Parameters.Add("@estlocal", SqlDbType.TinyInt).Value = estacionLocal;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oUsuarios.Add(CargarUsuario(oDR));
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
            return oUsuarios;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Métetodo encargado de cargar una entidad desde los resultados de la BD
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        private static Usuario CargarUsuario(System.Data.IDataReader oDR)
        {
            try
            {
                Usuario oUsuario = new Usuario();
                oUsuario.ID = oDR["use_id"].ToString();
                oUsuario.Nombre = oDR["use_nombr"].ToString();
                if (oDR["use_ntarj"] != DBNull.Value)
                {
                    oUsuario.Tarjeta = (long)oDR["use_ntarj"];
                }
                if (oDR["use_zpal"] != DBNull.Value)
                {
                    oUsuario.ZonaPrincipal = new Zona((byte)oDR["use_zpal"], oDR["ZonaPrincipal"].ToString());
                }
                if (oDR["use_zona"] != DBNull.Value)
                {
                    oUsuario.ZonaHabitual = new Zona((byte)oDR["use_zona"], oDR["ZonaHabitual"].ToString());
                }
                oUsuario.Eliminado = (oDR["use_delet"].ToString() == "S");
                oUsuario.FechaEgreso = Util.DbValueToNullable<DateTime>(oDR["use_feegr"]);
                oUsuario.Password = oDR["use_passw"].ToString();
                if (oDR["use_grupo"] != null)
                    oUsuario.PerfilGestion = new Perfil(oDR["use_grupo"].ToString(), oDR["gru_visua"].ToString());
                if (oDR["use_tipo"] != null && oDR["use_tipo"].ToString() != "")
                    oUsuario.TipoPersonal = getTipoPersonal(oDR["use_tipo"].ToString());

                oUsuario.UltimoAcceso = Util.DbValueToNullable<DateTime>(oDR["use_ultac"]);
                if (oDR["use_fevto"] != DBNull.Value)
                    oUsuario.UltimoCambioPassword = (DateTime)oDR["use_fevto"];
                else
                    oUsuario.UltimoCambioPassword = new DateTime(1900, 1, 1);

                oUsuario.EstacionesHabilitadas = null;
                oUsuario.EsLocal = (oDR["use_local"].ToString() == "S");
                oUsuario.DiasVencimiento = (short)oDR["con_venci"];
                oUsuario.ID_Nombre = oUsuario.ID + " - " + oUsuario.Nombre;
                oUsuario.NombreCorto = oDR["use_recib"].ToString();


                return oUsuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene usuarios que pueden ser validadores
        /// TODO: Es necesario revisar este metodo cuando se definan los perfiles de acceso para la validacion
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioL getUsuariosValidador(Conexion oConn)
        {
            UsuarioL oUsuarios = new UsuarioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_getUsuariosValidador";
               

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oUsuarios.Add(CargarUsuarioValidador(oDR));
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
            return oUsuarios;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Métetodo encargado de cargar una entidad desde los resultados de la BD
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        private static Usuario CargarUsuarioValidador(System.Data.IDataReader oDR)
        {
            Usuario oUsuario = new Usuario();
            try
            {                
                oUsuario.ID = oDR["use_id"].ToString();
                oUsuario.Nombre = oDR["use_nombr"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oUsuario;
        }
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene usuarios activos de una estación y grupo determinado
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="xiEstacion"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioL getUsuariosEstPeajista(Conexion oConn, int xiEstacion)
        {
            UsuarioL oUsuarios = new UsuarioL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Usuarios_GetUsuarioEstPeajista";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oUsuarios.Add(CargarUsuarioEstGru(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oUsuarios;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Métetodo encargado de cargar una entidad desde los resultados de la BD
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        private static Usuario CargarUsuarioEstGru(IDataReader oDR)
        {
            Usuario usuario = new Usuario();

            usuario.ID = oDR["use_id"].ToString();
            usuario.Nombre = oDR["use_nombr"].ToString();
            usuario.EsSupervisorACargo = oDR["SupervisorACargo"].ToString();

            return usuario;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Agrega un usuario en la base de datos
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto con la informacion del usuario a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void addUsuario(Usuario oUsuario, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_addUsuario";

                int? zpal = null;
                if (oUsuario.ZonaPrincipal != null)
                    zpal = oUsuario.ZonaPrincipal.Codigo;
                int? zona = null;
                if (oUsuario.ZonaHabitual != null)
                    zona = oUsuario.ZonaHabitual.Codigo;
                string grupo = null;
                if (oUsuario.PerfilGestion != null)
                    grupo = oUsuario.PerfilGestion.Codigo;
                string tipo = null;
                if (oUsuario.TipoPersonal != null)
                    tipo = oUsuario.TipoPersonal.Codigo;

                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oUsuario.ID;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 100).Value = oUsuario.Nombre;
                oCmd.Parameters.Add("@ntarj", SqlDbType.BigInt).Value = oUsuario.Tarjeta;
                oCmd.Parameters.Add("@zpal", SqlDbType.TinyInt).Value = zpal;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@feegr", SqlDbType.DateTime).Value = oUsuario.FechaEgreso;
                oCmd.Parameters.Add("@passw", SqlDbType.VarChar, 40).Value = oUsuario.Password;
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = grupo;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = tipo;
                oCmd.Parameters.Add("@fevto", SqlDbType.DateTime).Value = oUsuario.UltimoCambioPassword;
                oCmd.Parameters.Add("@recib", SqlDbType.VarChar, 30).Value = oUsuario.NombreCorto;

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
                        msg = Traduccion.Traducir("Este codigo de usuario ya existe");
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                        msg = Traduccion.Traducir("Este codigo de usuario fue eliminado");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Modifica un usaurio en la base de datos
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto con la informacion del usuario a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void updUsuario(Usuario oUsuario, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_updUsuario";

                int? zpal = null;
                if (oUsuario.ZonaPrincipal != null)
                    zpal = oUsuario.ZonaPrincipal.Codigo;
                int? zona = null;
                if (oUsuario.ZonaHabitual != null)
                    zona = oUsuario.ZonaHabitual.Codigo;
                string grupo = null;
                if (oUsuario.PerfilGestion != null)
                    grupo = oUsuario.PerfilGestion.Codigo;
                string tipo = null;
                if (oUsuario.TipoPersonal != null)
                    tipo = oUsuario.TipoPersonal.Codigo;


                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oUsuario.ID;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 100).Value = oUsuario.Nombre;
                oCmd.Parameters.Add("@ntarj", SqlDbType.BigInt).Value = oUsuario.Tarjeta;
                oCmd.Parameters.Add("@zpal", SqlDbType.TinyInt).Value = zpal;
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@feegr", SqlDbType.DateTime).Value = oUsuario.FechaEgreso;
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = grupo;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 1).Value = tipo;
                oCmd.Parameters.Add("@recib", SqlDbType.VarChar, 30).Value = oUsuario.NombreCorto;


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
                        msg = Traduccion.Traducir("No existe el registro del usuario");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Elimina un usuario en la base de datos
        /// </summary>
        /// <param name="usaurio">string - codigo de usuario a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void delUsuario(string usuario, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_delUsuario";

                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = usuario;

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
                        msg = Traduccion.Traducir("No existe el registro del usuario");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Usuario no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
            return;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Eliminacion de un usuario en la base de datos, invocando a otro metodo que lo hace efectivo
        /// </summary>
        /// <param name="oUsuario">Usuario - Objeto de usuario a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void delUsuario(Usuario oUsuario, Conexion oConn)
        {
            delUsuario(oUsuario.ID, oConn);
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Actualiza el ultimo acceso de un usuario
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oUsuario">Usuario - Objeto con los datos del usuario</param>
        /// <param name="fechahora">DateTime - Fecha y hora del ultimo acceso</param>
        /// ----------------------------------------------------------------------------------------------
        public static void updUltimoAcceso(Conexion oConn, Usuario oUsuario, DateTime fechahora)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_updUltimoAcceso";

                oCmd.Parameters.Add("@ID", SqlDbType.VarChar, 10).Value = oUsuario.ID;
                //lo pone el servidor
                //oCmd.Parameters.Add("@ultac", SqlDbType.DateTime).Value = fechahora;

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
                        msg = Traduccion.Traducir("Este Código de Usuario no existe");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene todos los supervisores de una estación
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="xiEstacion"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioL GetUsuarioSupervisorPorEstacion(Conexion oConn, int iEstacion, string sId)
        {
            UsuarioL oUsuarios = new UsuarioL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Usuarios_GetSupervisorPorEstacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;
            oCmd.Parameters.Add("@use_id", SqlDbType.VarChar, 10).Value = sId;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oUsuarios.Add(CargarUsuarioEstGru(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oUsuarios;
        }

        #endregion

        #region ESTACIONES HABILITADAS

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene una lista de los usuarios de la estación
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oUsuario"></param>
        /// <param name="estacion"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioEstacionL getEstacionesHabilitadas(Conexion oConn, Usuario oUsuario, int? estacion)
        {
            return getEstacionesHabilitadas(oConn, oUsuario, estacion, false);
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Obtiene una lista de los usuarios de una estación
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oUsuario"></param>
        /// <param name="estacion"></param>
        /// <param name="soloHabilitadas"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static UsuarioEstacionL getEstacionesHabilitadas(Conexion oConn, Usuario oUsuario, int? estacion, bool soloHabilitadas)
        {
            UsuarioEstacionL oEstacionesHabilitadas = new UsuarioEstacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_getEstacionesHabilitadas";
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oUsuario.ID;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                
                while (oDR.Read())
                {
                    UsuarioEstacion oEstacion = CargarEstacionHabilitada(oDR, oUsuario);
                    if (!soloHabilitadas || oEstacion.Perfil != null)
                        oEstacionesHabilitadas.Add(oEstacion);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEstacionesHabilitadas;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Carga una entidad desde los resultados de la BD
        /// </summary>
        /// <param name="oDR"></param>
        /// <param name="oUsuario"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        private static UsuarioEstacion CargarEstacionHabilitada(System.Data.IDataReader oDR, Usuario oUsuario)
        {


            UsuarioEstacion oEstacionHabilitada = new UsuarioEstacion();
            oEstacionHabilitada.Estacion = new Estacion((int)(oDR["est_codig"]), oDR["est_nombr"].ToString());
            oEstacionHabilitada.Usuario = oUsuario;
            if (oDR["use_grupo"] != DBNull.Value)
                oEstacionHabilitada.Perfil = new Perfil(oDR["use_grupo"].ToString(), oDR["gru_visua"].ToString());
            oEstacionHabilitada.Local = (oDR["use_local"].ToString() == "S");


            return oEstacionHabilitada;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Modifica una Estacion Habilitada para un Usuario en la base de datos
        /// Si no existe la inserta, si existe la modifica
        /// </summary>
        /// <param name="usuario">string - codigo del usuario</param>
        /// <param name="oEstacion">UsuarioEsatcion - Datos de la Estacion</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void updEstacionHabilitada(string usuario, UsuarioEstacion oEstacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_updEstacionHabilitada";

                string grupo = null;
                if (oEstacion.Perfil != null)
                    grupo = oEstacion.Perfil.Codigo;

                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oEstacion.Estacion.Numero;
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = grupo;
                oCmd.Parameters.Add("@local", SqlDbType.Char, 1).Value = oEstacion.Local ? "S" : "N";

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
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Elimina todas las Estaciones Habilitadas para un usuario en la base de datos
        /// </summary>
        /// <param name="usuario">string - codigo de usuario a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void delEstacionesHabilitadas(string usuario, Conexion oConn)
        {
            delEstacionesHabilitadas(usuario, null, oConn);
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Elimina las Estaciones Habilitadas de un perfil en la base de datos
        /// si oEstacion es null elimina todas las estaciones del usuario
        /// </summary>
        /// <param name="usuario">string - codigo de usuario a eliminar</param>
        /// <param name="oUsuario">UsuarioEstacion - datos de la Estacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ----------------------------------------------------------------------------------------------
        public static void delEstacionesHabilitadas(string usuario, UsuarioEstacion oEstacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_delEstacionesHabilitadas";

                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = usuario;
                int? estacion = null;
                if (oEstacion != null)
                    estacion = oEstacion.Estacion.Numero;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

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
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }
        
        #endregion

        #region PASSWORD

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Actualiza la password (o la borra)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="usuario">string - Nombre del usuario</param>
        /// <param name="pwdvieja">string - Password anterior</param>
        /// <param name="pwdnueva">string - Password nueva</param>
        /// <param name="vencimiento">DateTime - Fecha y hora del proximo cambio obligatorio</param>
        /// ----------------------------------------------------------------------------------------------
        public static void updPassword(Conexion oConn, string usuario, string pwdvieja, string pwdnueva, DateTime vencimiento)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Usuarios_updPassword";

                oCmd.Parameters.Add("@ID", SqlDbType.VarChar, 10).Value = usuario;
                oCmd.Parameters.Add("@pwdvieja", SqlDbType.VarChar, 40).Value = pwdvieja;
                oCmd.Parameters.Add("@pwdnueva", SqlDbType.VarChar, 40).Value = pwdnueva;
                oCmd.Parameters.Add("@fevto", SqlDbType.DateTime).Value = vencimiento;

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
                        msg = Traduccion.Traducir("Las password anterior no es correcta");
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

        #region TIPOPERSONAL: Clase de Tipos de Personal.
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve un objeto Tipo de Personal. 
        /// </summary>
        /// <param name="codigo">string - Codigo del tipo de personal que deseo devolver como texto</param>
        /// <returns>Objeto de Tipo de Personal</returns>
        /// ----------------------------------------------------------------------------------------------
        public static TipoPersonal getTipoPersonal(string codigo)
        {
            TipoPersonal oTipo = new TipoPersonal();
            oTipo.Codigo = codigo;
            oTipo.Descripcion = RetornaTipoPersonal(codigo);

            return oTipo;
        }
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve el texto traducido de la descripción del Tipo de Personal
        /// </summary>
        /// <param name="codigo">string - Codigo del tipo de personal que deseo devolver como texto</param>
        /// <returns>El texto traducido del tipo de personal</returns>
        /// ----------------------------------------------------------------------------------------------
        protected static string RetornaTipoPersonal(string codigo)
        {
            string retorno = string.Empty;

            switch (codigo)
            {
                case "E":
                    retorno = "Estable";
                    break;
                case "V":
                    retorno = "Eventual";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }

        #endregion
    }
}

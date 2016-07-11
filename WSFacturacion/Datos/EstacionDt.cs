using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using System.Data;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Estaciones de Peaje de la concesion
    /// </summary>
    ///****************************************************************************************************
    public static class EstacionDt
    {
        #region ESTACION: Clase de Datos de Estaciones de Peaje

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de peaje por la cual filtrar la busqueda</param>
        /// <returns>Lista de Estaciones de Peaje</returns>
        /// ***********************************************************************************************
        public static EstacionL getEstaciones(Conexion oConn, int? numeroEstacion, int? zona)
        {
            EstacionL oEstaciones = new EstacionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Estaciones_getEstaciones";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = numeroEstacion;
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oEstaciones.Add(CargarEstacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oEstaciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Estaciones
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Estaciones</param>
        /// <returns>Lista con el elemento Estacion de la base de datos</returns>
        /// ***********************************************************************************************
        private static Estacion CargarEstacion(IDataReader oDR)
        {
            Estacion oEstacion = new Estacion();
            oEstacion.Numero = (byte)oDR["est_codig"];
            oEstacion.Nombre = oDR["est_nombr"].ToString();

            if (oDR["est_estab"] == DBNull.Value)
            {
                oEstacion.Establecimiento = 0;
            }
            else
            {
                oEstacion.Establecimiento = (short)oDR["est_estab"];
            }

            oEstacion.BaseDatos = oDR["est_peaje"].ToString();
            oEstacion.ServidorDatos = oDR["est_server"].ToString();
            oEstacion.Zona = new Zona((byte)oDR["est_zona"], oDR["zon_descr"].ToString());
            oEstacion.URL = oDR["est_url"].ToString();
            oEstacion.Direccion = oDR["est_direc"].ToString();

            //TENER EN CUENTA QUE SI DE LA BD ME TRAE NULL O VACÍO, QUIERE DECIR QUE LA ESTACION ES ASCENDENTE Y DESCENTENTE
            oEstacion.Sentido = new ViaSentidoCirculacion(Convert.ToString(oDR["est_senti"]), oDR["sub_sencar"].ToString());

            if (oDR["est_site"] != DBNull.Value)
            {
                oEstacion.Site.Codigo = Convert.ToInt32(oDR["est_site"]);
                oEstacion.Site.Descripcion = Convert.ToString(oDR["sit_descr"]);
            }

            if (oDR["est_retir"] != DBNull.Value)
            {
                oEstacion.PermiteRetirosAnticipados = Convert.ToString(oDR["est_retir"]);
            }

            if (oDR["VisaEstab"] != DBNull.Value)
                oEstacion.EstablecimientoVisa = Convert.ToString(oDR["VisaEstab"]);


            return oEstacion;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una estacion de peaje en la base de datos
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto con la informacion de la estacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addEstacion(Estacion oEstacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Estaciones_addEstacion";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oEstacion.Numero;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar,50).Value = oEstacion.Nombre;
            oCmd.Parameters.Add("@Estab", SqlDbType.SmallInt, 50).Value = oEstacion.Establecimiento;
            oCmd.Parameters.Add("@Server", SqlDbType.VarChar,50).Value = oEstacion.ServidorDatos;
            oCmd.Parameters.Add("@Peaje", SqlDbType.VarChar,50).Value = oEstacion.BaseDatos;
            oCmd.Parameters.Add("@Zona", SqlDbType.TinyInt).Value = oEstacion.Zona.Codigo;
            oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oEstacion.URL;
            oCmd.Parameters.Add("@Direc", SqlDbType.VarChar, 100).Value = oEstacion.Direccion;
            oCmd.Parameters.Add("@senti", SqlDbType.Char, 1).Value = oEstacion.Sentido.Codigo;
            oCmd.Parameters.Add("@site", SqlDbType.Int).Value = oEstacion.Site.Codigo;
            oCmd.Parameters.Add("@retir", SqlDbType.Char, 1).Value = oEstacion.PermiteRetirosAnticipados;
            oCmd.Parameters.Add("@EstabVisa", SqlDbType.VarChar, 10).Value = oEstacion.EstablecimientoVisa;
            

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int) EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("Este numero de estación ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este número de Estación fue eliminado");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una estacion de peaje en la base de datos
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto con la informacion de la estacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updEstacion(Estacion oEstacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Estaciones_updEstacion";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oEstacion.Numero;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = oEstacion.Nombre;
            oCmd.Parameters.Add("@Estab", SqlDbType.SmallInt, 50).Value = oEstacion.Establecimiento;
            oCmd.Parameters.Add("@Server", SqlDbType.VarChar, 50).Value = oEstacion.ServidorDatos;
            oCmd.Parameters.Add("@Peaje", SqlDbType.VarChar, 50).Value = oEstacion.BaseDatos;
            oCmd.Parameters.Add("@Zona", SqlDbType.TinyInt).Value = oEstacion.Zona.Codigo;
            oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oEstacion.URL;
            oCmd.Parameters.Add("@Direc", SqlDbType.VarChar, 100).Value = oEstacion.Direccion;
            oCmd.Parameters.Add("@senti", SqlDbType.Char, 1).Value = oEstacion.Sentido.Codigo;
            oCmd.Parameters.Add("@site", SqlDbType.Int).Value = oEstacion.Site.Codigo;
            oCmd.Parameters.Add("@retir", SqlDbType.Char, 1).Value = oEstacion.PermiteRetirosAnticipados;
            oCmd.Parameters.Add("@EstabVisa", SqlDbType.VarChar, 10).Value = oEstacion.EstablecimientoVisa;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int) parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una estacion de peaje en la base de datos
        /// </summary>
        /// <param name="estacion">int - Numero de estacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEstacion(int estacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Estaciones_delEstacion";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg =   Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la estación");
                    }

                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Estación no se puede dar de baja porque está siendo utilizado"));
                throw;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una estacion de peaje en la base de datos, invocando a otro metodo que lo hace efectivo
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto de estacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEstacion(Estacion oEstacion, Conexion oConn)
        {
            delEstacion(oEstacion.Numero, oConn);
        }
        
        #endregion
        
        #region ZONA: Clase de Datos de Zonas.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Zonas definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoZona">int - Codigo de zona a filtrar</param>
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static ZonaL getZonas(Conexion oConn, int? codigoZona)
        {
            ZonaL oZonas = new ZonaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Zonas_getZonas";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = codigoZona;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oZonas.Add(CargarZona(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oZonas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Zonas 
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de zonas</param>
        /// <returns>Lista con el elmento Zona de la base de datos</returns>
        /// ***********************************************************************************************
        private static Zona CargarZona(IDataReader oDR)
        {
            return new Zona((byte)oDR["zon_zona"], oDR["zon_descr"].ToString());
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una zona en la base de datos
        /// </summary>
        /// <param name="oZona">Zona - Objeto con la informacion de la zona a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addZona(Zona oZona, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Zonas_addZona";

            oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = oZona.Codigo;
            oCmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 30).Value = oZona.Descripcion;

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
                {
                    msg = Traduccion.Traducir("Este Código de Zona ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este Código de Zona fue eliminado");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una zona en la base de datos
        /// </summary>
        /// <param name="oZona">Zona - Objeto con la informacion de la zona a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updZona(Zona oZona, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Zonas_updZona";

            oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = oZona.Codigo;
            oCmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 30).Value = oZona.Descripcion;

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
                {
                    msg = Traduccion.Traducir("No existe el registro de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una zona de la base de datos
        /// </summary>
        /// <param name="Zona">Int - Numero de zona a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delZona(int Zona, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Zonas_DelZona";

                oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = Zona;

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
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Zona");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Zona no se puede dar de baja porque está siendo utilizado"));
                throw;
            }
        }

        public static void delZona(Zona oZona, Conexion oConn)
        {
            delZona(oZona.Codigo, oConn);
        }

        #endregion

        #region Subestaciones

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la lista de subestaciones
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="sSentido"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static SubestacionL GetSubestaciones(Conexion conn, int? iNumeroEstacion, string sSentido)
        {
            var subestaciones = new SubestacionL();
            var oCmd = new SqlCommand();

            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Estacion_getSubEstacion";
            oCmd.Parameters.Add("@est_codig", SqlDbType.TinyInt).Value = iNumeroEstacion;
            oCmd.Parameters.Add("@sen_senti", SqlDbType.Char, 1).Value = sSentido;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                subestaciones.Add(CargarSubestacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return subestaciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un objeto del tipo estacion y su atributo Subestacion de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static Subestacion CargarSubestacion(IDataReader oDR)
        {
            var subestacion = new Subestacion();
            //Estacion
            subestacion.Estacion.Numero = Convert.ToInt32(oDR["est_codig"]);
            subestacion.Estacion.Nombre = Convert.ToString(oDR["est_nombr"]);

            //Sentido
            subestacion.Sentido.Codigo = Convert.ToString(oDR["sen_senti"]);
            subestacion.Sentido.Descripcion = Convert.ToString(oDR["sen_descr"]);

            //Codigo Subestación y Descripción
            if (oDR["sub_subes"] != DBNull.Value)
            {
                subestacion.CodigoSubEstacion = Convert.ToInt32(oDR["sub_subes"]);
                subestacion.Descripcion = Convert.ToString(oDR["sub_descr"]);
            }

            if (oDR["sub_cdsia"] != DBNull.Value)
            {
                subestacion.NumeroSia = Convert.ToString(oDR["sub_cdsia"]);
            }

            subestacion.SentidoCardinal = Convert.ToString(oDR["sub_sencar"]);
            subestacion.PlazaAntena = Convert.ToString(oDR["sub_pztag"]);

            return subestacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un objeto del tipo Subestacion de la base de datos
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void UpdSubestacion(Conexion conn, Subestacion subestacion)
        {
            var oCmd = new SqlCommand();

            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Estacion_updSubEstacion";
            oCmd.Parameters.Add("@sub_coest", SqlDbType.TinyInt).Value = subestacion.Estacion.Numero;
            oCmd.Parameters.Add("@sub_senti", SqlDbType.Char, 1).Value = subestacion.Sentido.Codigo;
            oCmd.Parameters.Add("@sub_subes", SqlDbType.Int).Value = subestacion.CodigoSubEstacion;
            oCmd.Parameters.Add("@sub_descr", SqlDbType.VarChar, 255).Value = subestacion.Descripcion;
            oCmd.Parameters.Add("@sub_cdsia", SqlDbType.Char, 4).Value = subestacion.NumeroSia;
            oCmd.Parameters.Add("@sub_sencar", SqlDbType.VarChar, 255).Value = subestacion.SentidoCardinal;
            oCmd.Parameters.Add("@sub_pztag", SqlDbType.Char, 4).Value = subestacion.PlazaAntena;
                        
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
                {
                    msg = Traduccion.Traducir("No existe el registro de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion

        #region  CabeceraTicket

        public static void UpdCabeceraTicket(Conexion conn, CabeceraTicket cabeceraTicket)
        {

            var oCmd = new SqlCommand();

            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CabeceraTicket_updCabeceraTicket";
            oCmd.Parameters.Add("@cab_coest", SqlDbType.TinyInt).Value = cabeceraTicket.Estacion.Numero;
            oCmd.Parameters.Add("@cab_senti", SqlDbType.Char, 1).Value = cabeceraTicket.Sentido.Codigo;
            oCmd.Parameters.Add("@cab_descr", SqlDbType.VarChar, 255).Value = cabeceraTicket.Descripcion;
          

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
                {
                    msg = Traduccion.Traducir("No existe el registro de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene la lista de subestaciones
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="sSentido"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static CabeceraTicketL GetCabeceraTicket(Conexion conn, int? iNumeroEstacion, string sSentido)
        {
            var cabeceraTickets = new CabeceraTicketL();
            var oCmd = new SqlCommand();

            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CabeceraTicket_getCabeceraTickets";
            oCmd.Parameters.Add("@est_codig", SqlDbType.TinyInt).Value = iNumeroEstacion;
            oCmd.Parameters.Add("@sen_senti", SqlDbType.Char, 1).Value = sSentido;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                cabeceraTickets.Add(CargarCabeceraTicket(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return cabeceraTickets;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un objeto del tipo estacion y su atributo Subestacion de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static CabeceraTicket CargarCabeceraTicket(IDataReader oDR)
        {
          

            Estacion oEsta = new Estacion(Convert.ToInt32(oDR["est_codig"]),Convert.ToString(oDR["est_nombr"]));
            ViaSentidoCirculacion oSentiVia = new ViaSentidoCirculacion();
            oSentiVia.Codigo = oDR["sub_senti"].ToString();
            oSentiVia.Descripcion = oDR["sen_descr"].ToString();


            var cabeceraTicket = new CabeceraTicket(oEsta,
                                                    oSentiVia,
                                                    Convert.ToInt32(oDR["sub_subes"]),
                                                    oDR["sub_descr"].ToString());

            //Sentido
           
            //cabeceraTicket.Sentido.Descripcion = oDR["sub_sencar"].ToString();
            //cabeceraTicket.Descripcion = oDR["cab_texto"].ToString();
            //cabeceraTicket.subest.Descripcion = oDR["sub_descr"].ToString();
           
            return cabeceraTicket;
        }

        #endregion

        #region Sites

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos del tipo Site
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="iNumeroSite"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static SiteL getSites(Conexion conn, int? iNumeroSite)
        {
            var sites = new SiteL();
            var oCmd = new SqlCommand();

            oCmd.Connection = conn.conection;
            oCmd.Transaction = conn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Site_getSites";
            oCmd.Parameters.Add("@sit_site", SqlDbType.TinyInt).Value = iNumeroSite;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                sites.Add(CargarSite(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return sites;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un objeto Site con datos traidos de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static Site CargarSite(IDataReader oDR)
        {
            return new Site
            {
                Codigo = Convert.ToInt32(oDR["sit_site"]),
                Descripcion = Convert.ToString(oDR["sit_descr"])
            };
        }

        #endregion

        #region ESTADO DE LA LISTA NEGRA DE LAS ESTACIONES

        /// <summary>
        /// Devuelve una lista con las estaciones y los estados de sus listas negras
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        public static DataSet GetEstacionEstadosListaNegra(Conexion oConn)
        {
            DataSet estados = new DataSet();
            estados.DataSetName = "EstacionEstadosListaNegra";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_EstacionEstadoListaNegra_getEstadoListaNegraEstaciones";
            
            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(estados, "Estados");

            oCmd = null;
            oDA.Dispose();

            return estados;
        }

        #endregion
    }
}
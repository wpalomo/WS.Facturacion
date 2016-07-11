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
    public class ComandoDt
    {
        #region CodigoComando

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Codigos de Comandos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xTipo">string? - Tipo de Comando para la busqueda</param>
        /// <returns>Lista de Codigos de Comandos</returns>
        /// ***********************************************************************************************
        public static CodigoComandoL getCodigosComando(Conexion oConn, string xsTipoComando)
        {
            CodigoComandoL oCodigosComando = new CodigoComandoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Comando_GetCodigosComando";
            oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = xsTipoComando;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oCodigosComando.Add(CargarCodigoComando(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oCodigosComando;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto CodigoComando
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Codigo de comando</param>
        /// <returns>Objeto CodigoComando con los datos</returns>
        /// ***********************************************************************************************
        private static CodigoComando CargarCodigoComando(System.Data.IDataReader oDR)
        {
            CodigoComando oCodigoComando = new CodigoComando();
            oCodigoComando.Tipo = Conversiones.edt_Str(oDR["cod_tipo"]);
            oCodigoComando.TipoDesc = Conversiones.edt_Str(oDR["tip_descr"]);
            oCodigoComando.Codigo = Conversiones.edt_Str(oDR["cod_codig"]);
            oCodigoComando.Descripcion = Conversiones.edt_Str(oDR["cod_descr"]);

            return oCodigoComando;
        }

        #endregion

        #region Comando: Metodos de la Clase Comando.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Comandos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <param name="xsStatus">string? - Status para la busqueda</param>
        /// <returns>Lista de Comandos</returns>
        /// ***********************************************************************************************
        public static ComandoL getComandos(Conexion oConn, int? xiEstacion, string xsStatus, ViaL xVias, DateTime dtFechaHoraDesde, DateTime dtFechaHoraHasta)
        {
            ComandoL oComandos = new ComandoL();

            // SERIALIZA VIAS 
            string xmlVias = "";
            xmlVias = Utilitarios.xmlUtil.SerializeObject(xVias);
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Comandos_GetComandos";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;
            oCmd.Parameters.Add("@status", SqlDbType.Char, 1).Value = xsStatus;
            oCmd.Parameters.Add("@xmlVias", SqlDbType.NText).Value = xmlVias;
            oCmd.Parameters.Add("@FechaDesde", SqlDbType.DateTime).Value = dtFechaHoraDesde;
            oCmd.Parameters.Add("@FechaHasta", SqlDbType.DateTime).Value = dtFechaHoraHasta;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oComandos.Add(CargarComando(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oComandos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Comando
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Comando</param>
        /// <returns>Objeto Comando con los datos</returns>
        /// ***********************************************************************************************
        private static Comando CargarComando(System.Data.IDataReader oDR)
        {
            Comando oComando = new Comando();

            oComando.FechaPedido = Util.DbValueToNullable<DateTime>(oDR["com_fecha"]);
            oComando.TipoDesc = Conversiones.edt_Str(oDR["tip_descr"]);
            oComando.CodigoDesc = Conversiones.edt_Str(oDR["cod_descr"]);
            oComando.UsuarioDesc = Conversiones.edt_Str(oDR["use_nombr"]);
            oComando.NumeroVia = Conversiones.edt_Int32(oDR["com_nuvia"]);
            oComando.FechaEjecucion = Util.DbValueToNullable<DateTime>(oDR["com_feeje"]);
            oComando.FechaVencimiento = Util.DbValueToNullable<DateTime>(oDR["com_venci"]);
            oComando.Status = Conversiones.edt_Str(oDR["com_status"]);
            oComando.Parametros = Conversiones.edt_Str(oDR["com_param"]);
            oComando.ParametrosDesc = Conversiones.edt_Str(oDR["com_pardesc"]);
            oComando.NombreVia = Conversiones.edt_Str(oDR["via_nombr"]);

            return oComando;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Comando en la base de datos
        /// </summary>
        /// <param name="oComando">Comando - Objeto con la informacion del Comando a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addComando(Comando oComando, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Comandos_addComando";

            oCmd.Parameters.Add("@FechaPedido", SqlDbType.DateTime).Value = oComando.FechaPedido;
            oCmd.Parameters.Add("@TipoComando", SqlDbType.Char,1).Value = oComando.Tipo;
            oCmd.Parameters.Add("@CodigoComando", SqlDbType.Char,4).Value = oComando.Codigo;
            oCmd.Parameters.Add("@CodigoEstacion", SqlDbType.TinyInt).Value = oComando.Estacion;
            oCmd.Parameters.Add("@NumeroVia", SqlDbType.TinyInt).Value = oComando.NumeroVia;
            oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar,10).Value = oComando.Usuario;
            oCmd.Parameters.Add("@FechaEjecucion", SqlDbType.DateTime).Value = oComando.FechaEjecucion;
            oCmd.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime).Value = oComando.FechaVencimiento;
            oCmd.Parameters.Add("@Status", SqlDbType.Char,1).Value = oComando.Status;
            oCmd.Parameters.Add("@Parametros", SqlDbType.VarChar,2000).Value = oComando.Parametros;
            oCmd.Parameters.Add("@ParametrosDesc", SqlDbType.VarChar, 6000).Value = oComando.ParametrosDesc;
            oCmd.Parameters.Add("@SegundosTolerancia", SqlDbType.Int).Value = oComando.SegundosTolerancia;
            oCmd.Parameters.Add("@Comentario", SqlDbType.VarChar, 255).Value = oComando.Comentario;

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

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Comando en la base de datos para todas las vias de un cierto tipo
        /// </summary>
        /// <param name="oComando">Comando - Objeto con la informacion del Comando a insertar</param>
        /// <param name="modeloVia">String - Modelo de vias para el que se agrega el comando</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addComandoVias(Comando oComando, string modeloVia, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Comandos_addComandoVias";


            oCmd.Parameters.Add("@TipoComando", SqlDbType.Char, 1).Value = oComando.Tipo;
            oCmd.Parameters.Add("@CodigoComando", SqlDbType.Char, 4).Value = oComando.Codigo;
            oCmd.Parameters.Add("@CodigoEstacion", SqlDbType.TinyInt).Value = oComando.Estacion;
            oCmd.Parameters.Add("@ModeloVia", SqlDbType.VarChar, 3).Value = modeloVia;
            oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 10).Value = oComando.Usuario;
            oCmd.Parameters.Add("@FechaVencimiento", SqlDbType.DateTime).Value = oComando.FechaVencimiento;
            oCmd.Parameters.Add("@Parametros", SqlDbType.VarChar, 2000).Value = oComando.Parametros;
            oCmd.Parameters.Add("@ParametrosDesc", SqlDbType.VarChar, 6000).Value = oComando.ParametrosDesc;
            oCmd.Parameters.Add("@SegundosTolerancia", SqlDbType.Int).Value = oComando.SegundosTolerancia;

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
        
        #endregion
    }
}

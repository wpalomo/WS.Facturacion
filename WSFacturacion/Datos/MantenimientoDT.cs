using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class MantenimientoDT
    {
        public static ListaActualizacionTipoListaL p_oConfiguracionListado;

        #region ConfiguracionContraseña: Clase de Datos de Configuracion Contraseña.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Contraseñas definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configuracion de Contraseñas</returns>
        /// ***********************************************************************************************
        public static PasswordConfiguracion getConfiguracionContraseña(Conexion oConn)
        {
            PasswordConfiguracion oConfiguracionContraseña = new PasswordConfiguracion();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionPassword_getPassword";

            oDR = oCmd.ExecuteReader();

            if (oDR.Read())
            {
                oConfiguracionContraseña = CargarConfiguracionContraseña(oDR);
            }
            else
            {
                //No habia datos
                oConfiguracionContraseña = new PasswordConfiguracion();
                oConfiguracionContraseña.CaracteresRepetidos = 0;
                oConfiguracionContraseña.ControlRepeticion = "S";
                oConfiguracionContraseña.LargoMinimo = 0;
                oConfiguracionContraseña.Vencimiento = 0;
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oConfiguracionContraseña;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Configuracion de Contraseñas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de CONPAS</param>
        /// <returns>Lista con el elmento Configuracion de Contraseñas de la base de datos</returns>
        /// ***********************************************************************************************
        private static PasswordConfiguracion CargarConfiguracionContraseña(System.Data.IDataReader oDR)
        {
            PasswordConfiguracion oConfiguracionContraseña = new PasswordConfiguracion();

            oConfiguracionContraseña.CaracteresRepetidos = (byte)oDR["con_forma"];
            oConfiguracionContraseña.ControlRepeticion = (oDR["con_repet"]).ToString();
            oConfiguracionContraseña.LargoMinimo = (byte)(oDR["con_largo"]);
            oConfiguracionContraseña.Vencimiento = (Int16)(oDR["con_venci"]);
            oConfiguracionContraseña.IdContraseña = (int)(oDR["con_key"]);
            
            return oConfiguracionContraseña;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion de Contraseñas en la base de datos
        /// </summary>
        /// <param name="oConfig">Configuracion de Contraseñas - Objeto con la informacion de la configuracion de Contraseñas a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionContraseña(PasswordConfiguracion oConfiguracionContraseña, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionPassword_updConfiguracionPassword";

            oCmd.Parameters.Add("@con_repet", SqlDbType.Char, 1).Value = oConfiguracionContraseña.ControlRepeticion;
            oCmd.Parameters.Add("@con_forma", SqlDbType.SmallInt).Value = oConfiguracionContraseña.CaracteresRepetidos;
            oCmd.Parameters.Add("@con_largo", SqlDbType.SmallInt).Value = oConfiguracionContraseña.LargoMinimo;
            oCmd.Parameters.Add("@con_venci", SqlDbType.SmallInt).Value = oConfiguracionContraseña.Vencimiento;


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
                    msg = Traduccion.Traducir("No existe el registro de la Configuración de Contraseñas");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion

        #region ConfiguracionListado: Clase de Datos de Configuracion Listado.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Listas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configuracion de Listas</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionL getConfiguracionListado(Conexion oConn, string TipoListado, string Nivel)
        {
            ListaActualizacionL oConfiguracionListas = new ListaActualizacionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionListas_getConfiguracion";
            oCmd.Parameters.Add("@lis_codig", SqlDbType.Char, 1).Value = TipoListado;
            oCmd.Parameters.Add("@lis_Nivel", SqlDbType.Char, 1).Value = Nivel;

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oConfiguracionListas.Add(CargarConfiguracionListas(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oConfiguracionListas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Configuracion de Listas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de lisact</param>
        /// <returns>Lista con el elmento Configuracion de Listas de la base de datos</returns>
        /// ***********************************************************************************************
        private static ListaActualizacion CargarConfiguracionListas(System.Data.IDataReader oDR)
        {
            ListaActualizacion oConfiguracionListas = new ListaActualizacion();

            oConfiguracionListas.TipoLista = new ListaActualizacionTipoLista(oDR["lis_codig"].ToString(), oDR["tip_descr"].ToString());
            oConfiguracionListas.Modo = new ListaActualizacionModo(oDR["lis_modo"].ToString(), getDescripcionModo(oDR["lis_modo"].ToString()));
            oConfiguracionListas.FrecuenciaHoraria = Util.DbValueToNullable<Int32>(oDR["lis_cohor"]);
            oConfiguracionListas.HorariodeConsulta = oDR["lis_codia"].ToString();
            oConfiguracionListas.Nivel = oDR["lis_nivel"].ToString();
            oConfiguracionListas.DiferenciaEntreVias = (byte)(oDR["lis_midif"]);
            oConfiguracionListas.Reintento = (byte)(oDR["lis_reint"]);

            return oConfiguracionListas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion de Listas en la base de datos
        /// </summary>
        /// <param name="oConfig">Configuracion de Listas - Objeto con la informacion de la configuracion de Listas</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfiguracionListas(ListaActualizacion oConfiguracionListas, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionListas_updConfiguracion";

            oCmd.Parameters.Add("@lis_codig", SqlDbType.Char, 1).Value = oConfiguracionListas.TipoLista.Codigo;
            oCmd.Parameters.Add("@lis_modo", SqlDbType.Char, 1).Value = oConfiguracionListas.Modo.Codigo;
            oCmd.Parameters.Add("@lis_cohor", SqlDbType.Int).Value = oConfiguracionListas.FrecuenciaHoraria;
            oCmd.Parameters.Add("@lis_codia", SqlDbType.Char, 5).Value = oConfiguracionListas.HorariodeConsulta;
            oCmd.Parameters.Add("@lis_midif", SqlDbType.TinyInt).Value = oConfiguracionListas.DiferenciaEntreVias;
            oCmd.Parameters.Add("@lis_reint", SqlDbType.TinyInt).Value = oConfiguracionListas.Reintento;
            oCmd.Parameters.Add("@lis_Nivel", SqlDbType.Char, 1).Value = oConfiguracionListas.Nivel;

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
                    msg = Traduccion.Traducir("No existe el registro de la Configuración de Listas");
                throw new ErrorSPException(msg);
            }
        }

        #endregion

        #region MODO: Clase de Datos de Configuracion Listado
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto Modo. 
        /// </summary>
        /// <param name="codigo">int - Codigo de modo que deseo devolver como texto</param>
        /// <returns>Objeto de Efecto</returns>
        /// ***********************************************************************************************
        public static ListaActualizacionModoL getModos()
        {
            ListaActualizacionModoL oModosL = new ListaActualizacionModoL();

            oModosL.Add(new ListaActualizacionModo("H", getDescripcionModo("H")));
            oModosL.Add(new ListaActualizacionModo("D", getDescripcionModo("D")));

            return oModosL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido del Modo
        /// </summary>
        /// <param name="codigo">int - Codigo de Modo que deseo devolver como texto</param>
        /// <returns>El texto traducido del Modo</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionModo(string codigo)
        {
            string retorno = string.Empty;
            string caseSwitch = codigo;

            switch (caseSwitch)
            {
                case "H":
                    retorno = "Horario";
                    break;
                case "D":
                    retorno = "Diario";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }

        #endregion

        #region ReferenciaFK

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de Tablas que usan un registro a dar de Baja
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="tabla">string - Tabla de la que se quiere eliminar el registro (incluir nombre de Schema)</param>
        /// <param name="claves">string[] - valoress de la PK del registro que se quiere eliminar</param>
        /// <param name="tablasNoExcluyentes">string[] - Tablas con FK que permiten dar de baja igual</param>
        /// <returns>Lista de Referencias de FK</returns>
        /// ***********************************************************************************************
        public static ReferenciaFKL getReferenciasFK(Conexion oConn, string tabla, string[] claves, string[] tablasNoExcluyentes)
        {
            ReferenciaFKL oRFKs = new ReferenciaFKL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.RevisarFKBajas";

            // Parametro de la tabla a chequear
            oCmd.Parameters.Add("@Tabla", SqlDbType.VarChar, 100).Value = tabla;

            // Parametros con las claves de PK de la tabla
            for (int i = 1; i <= claves.Length; i++)
            {
                string clave = claves[i - 1];
                string campo = "@Clave" + i.ToString();
                oCmd.Parameters.Add(campo, SqlDbType.VarChar, 2000).Value = clave;
            }

            // Parametro de lista de tablas que son NO excluyentes (no me importan chequear)
            string sTablasNoExcluyentes = "";
            for (int i = 0; i < tablasNoExcluyentes.Length; i++)
            {
                sTablasNoExcluyentes += tablasNoExcluyentes[i] + ",";
            }
            oCmd.Parameters.Add("@TablasNoExcluyentes", SqlDbType.VarChar, 2000).Value = sTablasNoExcluyentes;

            oCmd.CommandTimeout = 3600;

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oRFKs.Add(CargarReferenciaFK(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oRFKs;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en el objeto ReferenciaFK
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con las FK</param>
        /// <returns>Objeto de foreign keys de la tabla</returns>
        /// ***********************************************************************************************
        private static ReferenciaFK CargarReferenciaFK(System.Data.IDataReader oDR)
        {
            ReferenciaFK oRFK = new ReferenciaFK();

            oRFK.Tabla = oDR["Tabla"].ToString();
            string[] campos = null;

            if (oDR["Campo3"] != DBNull.Value)
            {
                campos = new string[] { oDR["Campo1"].ToString(), oDR["Campo2"].ToString(), oDR["Campo3"].ToString() };
            }
            else if (oDR["Campo2"] != DBNull.Value)
            {
                campos = new string[] { oDR["Campo1"].ToString(), oDR["Campo2"].ToString() };
            }
            else if (oDR["Campo1"] != DBNull.Value)
            {
                campos = new string[] { oDR["Campo1"].ToString() };
            }

            oRFK.Campos = campos;
            oRFK.CantidadRegistros = (int)oDR["Registros"];
            oRFK.Excluyente = (oDR["Excluyente"].ToString() == "S");

            return oRFK;
        }

        #endregion

        #region Otros

        /// <summary>
        /// Obtiene la fecha y hora del servidor
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        public static DateTime GetDateTimeServer(Conexion oConn)
        {
            DateTime dtFechaServidor;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_GetDate";

            dtFechaServidor = Convert.ToDateTime(oCmd.ExecuteScalar());

            oCmd = null;
            return dtFechaServidor;
        }
        
        #endregion
    }
}

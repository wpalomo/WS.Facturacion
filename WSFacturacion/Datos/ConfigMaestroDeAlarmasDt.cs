using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public static class ConfigMaestroDeAlarmasDt
    {
        #region METODOS ENCARGADOS DE CARGAR LOS PARAMETERS

        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="alarma"></param>
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, ConfigMaestroDeAlarmas alarma)
        {
            cmd.Parameters.Add("@cal_codig", SqlDbType.TinyInt).Value = alarma.Codigo;
            cmd.Parameters.Add("@cal_veces", SqlDbType.TinyInt).Value = alarma.Veces;
            cmd.Parameters.Add("@cal_tpson", SqlDbType.TinyInt).Value = alarma.Sonido.Tipo;
        }

        /// <summary>
        /// Sobrecarga, Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, ConfigMaestroDeAlarmas alarma)
        {
            AddParametersToGetOrDel(cmd, alarma.Codigo);        
        }

        /// <summary>
        /// Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="codigo"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, byte? codigo)
        {
            cmd.Parameters.Add("@cal_codig", SqlDbType.TinyInt).Value = codigo;
        }

        #endregion

        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static ConfigMaestroDeAlarmas CargarConfigMaestroDeAlarmas(IDataReader oDR)
        {
            var alarma = new ConfigMaestroDeAlarmas();

            alarma.Codigo = (byte)oDR["cal_codig"];
            alarma.Descripcion = (string)oDR["cal_descr"];
            alarma.Veces = (byte)oDR["cal_veces"];
            //Pueden ser NULL
            alarma.Sonido = new TipoDeSonido((oDR["cal_tpson"] as byte?));   

            return alarma;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        public static ConfigMaestroDeAlarmasL getConfigMaestroDeAlarmas(Conexion oConn, byte? codigo)
        {
            var alarmaL = new ConfigMaestroDeAlarmasL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_MaestroDeAlarmas_getMaestroDeAlarmas";

            AddParametersToGetOrDel(oCmd, codigo);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                alarmaL.Add(CargarConfigMaestroDeAlarmas(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return alarmaL;
        }

        /// <summary>
        /// Método encargado de actualizar en la base de datos
        /// </summary>
        /// <param name="alarma"></param>
        /// <param name="oConn"></param>
        public static void updConfigMaestroDeAlarmas(ConfigMaestroDeAlarmas alarma, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_MaestroDeAlarmas_updMaestroDeAlarmas";

            AddParametersToUpdOrAdd(oCmd, alarma);

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
                    msg = Traduccion.Traducir("No existe el registro del Maestro de Alarmas");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}

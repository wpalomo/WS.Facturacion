using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public static class CodigoDeSimulacionDt
    {
        #region METODOS ENCARGADOS DE CARGAR LOS PARAMETERS

        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, CodigoDeSimulacion codigoDeSimulacion)
        {
            cmd.Parameters.Add("@csm_codig", SqlDbType.TinyInt).Value = codigoDeSimulacion.Codigo;
            cmd.Parameters.Add("@csm_descr", SqlDbType.VarChar, 30).Value = codigoDeSimulacion.Descripcion;
        }

        /// <summary>
        /// Sobrecarga, Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, CodigoDeSimulacion codigoDeSimulacion)
        {
            AddParametersToGetOrDel(cmd, codigoDeSimulacion.Codigo);
        }

        /// <summary>
        /// Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="codigo"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, byte? codigo)
        {
            cmd.Parameters.Add("@csm_codig", SqlDbType.TinyInt).Value = codigo;
        }

        #endregion

        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static CodigoDeSimulacion CargarCodigoDeSimulacion(IDataReader oDR)
        {
            var tipoCupon = new CodigoDeSimulacion();

            tipoCupon.Codigo = (byte)oDR["csm_codig"];
            tipoCupon.Descripcion = (string)oDR["csm_descr"];

            return tipoCupon;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        public static CodigoDeSimulacionL getCodigosDeSimulaciones(Conexion oConn, byte? codigo)
        {
            CodigoDeSimulacionL codigoDeSimulacionL = new CodigoDeSimulacionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeSimulacion_getCodigosDeSimulacion";

            AddParametersToGetOrDel(oCmd, codigo);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                codigoDeSimulacionL.Add(CargarCodigoDeSimulacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return codigoDeSimulacionL;
        }

        /// <summary>
        /// Método encargado de agregar en la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void addCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeSimulacion_addCodigosDeSimulacion";

            AddParametersToUpdOrAdd(oCmd, codigoDeSimulacion);

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
                
                    msg = Traduccion.Traducir("Este Código de Simulación ya existe");
              
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
              
                    msg = Traduccion.Traducir("Este Código de Causa de cierre fue eliminado");
                
                throw new ErrorSPException(msg);
            }
        }

        /// <summary>
        /// Método encargado de actualizar en la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void updCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeSimulacion_updCodigosDeSimulacion";

            AddParametersToUpdOrAdd(oCmd, codigoDeSimulacion);

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
                    msg = Traduccion.Traducir("No existe el registro del código de simulación");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// <summary>
        /// Método encargado de eliminar de la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void delCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeSimulacion_delCodigosDeSimulacion";

            AddParametersToGetOrDel(oCmd, codigoDeSimulacion);

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
                    msg = Traduccion.Traducir("No existe el registro del código de simulación");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}
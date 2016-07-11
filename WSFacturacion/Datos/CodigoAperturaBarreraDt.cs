using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public static class CodigoAperturaBarreraDt
    {
        #region METODOS ENCARGADOS DE CARGAR LOS PARAMETERS

        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, CodigoAperturaBarrera codigoAperturaBarrera)
        {
            cmd.Parameters.Add("@cab_codig", SqlDbType.TinyInt).Value = codigoAperturaBarrera.Codigo;
            cmd.Parameters.Add("@cab_descr", SqlDbType.VarChar, 30).Value = codigoAperturaBarrera.Descripcion;
        }

        /// <summary>
        /// Sobrecarga, Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, CodigoAperturaBarrera codigoAperturaBarrera)
        {
            AddParametersToGetOrDel(cmd, codigoAperturaBarrera.Codigo);
        }

        /// <summary>
        /// Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="codigo"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, byte? codigo)
        {
            cmd.Parameters.Add("@cab_codig", SqlDbType.TinyInt).Value = codigo;
        }

        #endregion

        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static CodigoAperturaBarrera CargarCodigoAperturaBarrera(IDataReader oDR)
        {
            var tipoCupon = new CodigoAperturaBarrera();

            tipoCupon.Codigo = (byte)oDR["cab_codig"];
            tipoCupon.Descripcion = (string)oDR["cab_descr"];

            return tipoCupon;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        public static CodigoAperturaBarreraL getCodigosAperturaBarrera(Conexion oConn, byte? codigo)
        {
            CodigoAperturaBarreraL codigoAperturaBarreraL = new CodigoAperturaBarreraL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosAperturaBarrera_getCodigosAperturaBarrera";

            AddParametersToGetOrDel(oCmd, codigo);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                codigoAperturaBarreraL.Add(CargarCodigoAperturaBarrera(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return codigoAperturaBarreraL;
        }

        /// <summary>
        /// Método encargado de agregar en la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void addCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosAperturaBarrera_addCodigosAperturaBarrera";

            AddParametersToUpdOrAdd(oCmd, codigoAperturaBarrera);

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
        public static void updCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosAperturaBarrera_updCodigosAperturaBarrera";

            AddParametersToUpdOrAdd(oCmd, codigoAperturaBarrera);

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
        public static void delCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosAperturaBarrera_delCodigosAperturaBarrera";

            AddParametersToGetOrDel(oCmd, codigoAperturaBarrera);

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

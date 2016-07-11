using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public static class CodigoDeCancelacionDt
    {
        #region METODOS ENCARGADOS DE CARGAR LOS PARAMETERS

        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, CodigoDeCancelacion codigoDeCancelacion)
        {
            cmd.Parameters.Add("@ccn_codig", SqlDbType.TinyInt).Value = codigoDeCancelacion.Codigo;
            cmd.Parameters.Add("@ccn_descr", SqlDbType.VarChar, 30).Value = codigoDeCancelacion.Descripcion;
        }

        /// <summary>
        /// Sobrecarga, Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, CodigoDeCancelacion codigoDeCancelacion)
        {
            AddParametersToGetOrDel(cmd, codigoDeCancelacion.Codigo);
        }

        /// <summary>
        /// Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="codigo"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, byte? codigo)
        {
            cmd.Parameters.Add("@ccn_codig", SqlDbType.TinyInt).Value = codigo;
        }

        #endregion

        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static CodigoDeCancelacion CargarCodigoDeCancelacion(IDataReader oDR)
        {
            var tipoCupon = new CodigoDeCancelacion();

            tipoCupon.Codigo = (byte)oDR["ccn_codig"];
            tipoCupon.Descripcion = (string)oDR["ccn_descr"];

            return tipoCupon;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        public static CodigoDeCancelacionL getCodigosDeCancelacion(Conexion oConn, byte? codigo)
        {
            CodigoDeCancelacionL codigoDeCancelacionL = new CodigoDeCancelacionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeCancelacion_getCodigosDeCancelacion";

            AddParametersToGetOrDel(oCmd, codigo);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                codigoDeCancelacionL.Add(CargarCodigoDeCancelacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return codigoDeCancelacionL;
        }

        /// <summary>
        /// Método encargado de agregar en la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void addCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeCancelacion_addCodigosDeCancelacion";

            AddParametersToUpdOrAdd(oCmd, codigoDeCancelacion);

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
        public static void updCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeCancelacion_updCodigosDeCancelacion";

            AddParametersToUpdOrAdd(oCmd, codigoDeCancelacion);

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
        public static void delCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_CodigosDeCancelacion_delCodigosDeCancelacion";

            AddParametersToGetOrDel(oCmd, codigoDeCancelacion);

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

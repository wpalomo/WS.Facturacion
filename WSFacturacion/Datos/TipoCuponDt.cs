using System;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public static class TipoCuponDt
    {
        #region METODOS ENCARGADOS DE CARGAR LOS PARAMETERS

        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, TipoCupon tipoCupon)
        {
            cmd.Parameters.Add("@tcu_tipva", SqlDbType.SmallInt).Value = tipoCupon.Codigo;
            cmd.Parameters.Add("@tcu_descr", SqlDbType.VarChar, 30).Value = tipoCupon.Descripcion;
            cmd.Parameters.Add("@tcu_avia", SqlDbType.Char, 1).Value = tipoCupon.UsoEnVia;
            cmd.Parameters.Add("@tcu_concb", SqlDbType.Char, 1).Value = tipoCupon.ConCodigoDeBarra;
            cmd.Parameters.Add("@tcu_titar", SqlDbType.TinyInt).Value = tipoCupon.TipoTarifa.CodigoTarifa;
            cmd.Parameters.Add("@tcu_recib", SqlDbType.Char, 1).Value = tipoCupon.RegistraCupones;
        }

        /// <summary>
        /// Sobrecarga, Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, TipoCupon tipoCupon)
        {
            AddParametersToGetOrDel(cmd, tipoCupon.Codigo);        
        }

        /// <summary>
        /// Método encargado de agregar los parámetros de la clave principal para un Get o un Del
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="codigo"></param>
        private static void AddParametersToGetOrDel(SqlCommand cmd, int? codigo)
        {
            cmd.Parameters.Add("@tcu_tipva", SqlDbType.SmallInt).Value = codigo;
        }

        #endregion

        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static TipoCupon CargarTipoCupon(IDataReader oDR)
        {
            var tipoCupon = new TipoCupon();

            tipoCupon.Codigo = Convert.ToInt32(oDR["tcu_tipva"]);
            tipoCupon.Descripcion = (string)oDR["tcu_descr"];
            //Pueden ser NULL
            if (oDR["tcu_avia"] != DBNull.Value)
            {
                tipoCupon.UsoEnVia = (string)oDR["tcu_avia"];
            }
            if (oDR["tcu_concb"] != DBNull.Value)
            {
                tipoCupon.ConCodigoDeBarra = (string)oDR["tcu_concb"];
            }
            if (oDR["tcu_recib"] != DBNull.Value)
            {
                tipoCupon.RegistraCupones = (string)oDR["tcu_recib"];
            }
            tipoCupon.TipoTarifa = new TarifaDiferenciada((byte)oDR["tcu_titar"], oDR["tit_descr"].ToString());

            return tipoCupon;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        public static TipoCuponL getTiposCupones(Conexion oConn, int? codigo)
        {
            TipoCuponL tipoCuponL = new TipoCuponL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDeCupones_getTiposDeCupones";

            AddParametersToGetOrDel(oCmd, codigo);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                tipoCuponL.Add(CargarTipoCupon(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return tipoCuponL;
        }

        /// <summary>
        /// Método encargado de agregar a la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void addTipoCupon(TipoCupon tipoCupon, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDeCupones_addTiposDeCupones";

            AddParametersToUpdOrAdd(oCmd, tipoCupon);

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
                    msg = Traduccion.Traducir("Este Tipo de cupón ya existe");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// <summary>
        /// Método encargado de actualizar en la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void updTipoCupon(TipoCupon tipoCupon, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDeCupones_updTiposDeCupones";

            AddParametersToUpdOrAdd(oCmd, tipoCupon);

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
                    msg = Traduccion.Traducir("No existe el registro del tipo de cupón");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// <summary>
        /// Método encargado de eliminar de la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void delTipoCupon(TipoCupon tipoCupon, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDeCupones_delTiposDeCupones";

            AddParametersToGetOrDel(oCmd, tipoCupon);

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
                    msg = Traduccion.Traducir("No existe el registro del tipo de cupón");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}

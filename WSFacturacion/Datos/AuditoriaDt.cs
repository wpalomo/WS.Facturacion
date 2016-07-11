using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion y registro de la auditoria realizada en el sistema
    /// </summary>
    ///****************************************************************************************************

    public static class AuditoriaDt
    {
        #region AUDITORIA: Clase de Datos de Auditoria



        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la auditoria de una operacion en la base de datos
        /// </summary>
        /// <param name="oAuditoria">Auditoria - Objeto con la informacion de la auditoria a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addAuditoria(Auditoria oAuditoria, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Auditoria_addAuditoria";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oAuditoria.Estacion;
                //La fecha y hora la pone el servidor
                //oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oAuditoria.Fecha;
                oCmd.Parameters.Add("@usuar", SqlDbType.VarChar, 10).Value = oAuditoria.Usuario;
                oCmd.Parameters.Add("@opera", SqlDbType.Char, 3).Value = oAuditoria.CodigoAuditoria.Codigo;
                oCmd.Parameters.Add("@movim", SqlDbType.Char, 1).Value = oAuditoria.Movimiento;
                oCmd.Parameters.Add("@codig", SqlDbType.VarChar, 50).Value = oAuditoria.CodigoRegistro;
                oCmd.Parameters.Add("@valor", SqlDbType.VarChar, 3000).Value = oAuditoria.Descripcion;
                oCmd.Parameters.Add("@wrkst", SqlDbType.VarChar, 100).Value = oAuditoria.Terminal;

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
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        
        #endregion



        #region CODIGO_AUDITORIA: Clase de Datos de los Codigos de Auditoria


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Codigos de Auditoria
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoAuditoria">string - Codigo de auditoria por el que filtramos la consulta</param>
        /// <returns>Lista de Zonas</returns>
        /// ***********************************************************************************************
        public static AuditoriaCodigoL getCodigosAuditoria(Conexion oConn, string codigoAuditoria)
        {
            AuditoriaCodigoL oAuditoriaCodigoL = new AuditoriaCodigoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Auditoria_getCodigosAuditoria";
                oCmd.Parameters.Add("@Codigo", SqlDbType.Char, 3).Value = codigoAuditoria;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAuditoriaCodigoL.Add(CargarCodigoAuditoria(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAuditoriaCodigoL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Codigos de Auditoria 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de codigos de auditoria</param>
        /// <returns>Lista con el elemento AuditoriaCodigo de la base de datos</returns>
        /// ***********************************************************************************************
        private static AuditoriaCodigo CargarCodigoAuditoria(System.Data.IDataReader oDR)
        {
            AuditoriaCodigo oAuditoriaCodigo = new AuditoriaCodigo((string)oDR["tab_codig"],
                                                                   (string)oDR["tab_descr"]);

            return oAuditoriaCodigo;
        }


        #endregion

    }
}

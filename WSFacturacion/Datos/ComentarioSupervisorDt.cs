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
    public class ComentarioSupervisorDt
    {
        #region ComentarioSupervisorDT: Clase de Datos de Comentarios del Supervisor.

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Comentario de Supervisor a un tránsito
        /// </summary>
        /// <param name="oComentario">ComentarioSupervisor - Objeto con el Comentario del Supervisor</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addComentarioSupervisor(ComentarioSupervisor oComentario, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ComentarioSup_addComentarioSup";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oComentario.estacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = oComentario.via;
                oCmd.Parameters.Add("@numtr", SqlDbType.Int).Value = oComentario.transito;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oComentario.fecha;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar).Value = oComentario.id;
                oCmd.Parameters.Add("@comen", SqlDbType.VarChar).Value = oComentario.comentario;


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
    }
}

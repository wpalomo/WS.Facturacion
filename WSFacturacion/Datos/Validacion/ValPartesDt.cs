using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class ValPartesDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Jornadas en un periodo
        /// </summary>
        /// <returns>Lista de Jornadas</returns>
        /// ***********************************************************************************************

        public static bool setFacturasValidacion(Conexion oConn, int estacion, DateTime jornada, string validador )
        {
            bool facturo = false;
            int retval = 0;

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_setFacturasValidacion"; 
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@causa", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;                

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. -102
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("Error de Parametrización");
                    if (retval == -102)
                        msg = Traduccion.Traducir("");
                    if (retval == -103)
                        msg = Traduccion.Traducir("Parte Pendiente de validacion");
                    if (retval == -104)
                        msg = Traduccion.Traducir("La Jornada se encuentra cerrada");
                    if (retval == -105)
                        msg = Traduccion.Traducir("");
                    if (retval == -106)
                        msg = Traduccion.Traducir("No hay operaciones a Facturar");
                    

                    throw new Telectronica.Errores.ErrorSPException(msg);
                }
                else
                {
                    facturo = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return facturo;
        }

    }
}

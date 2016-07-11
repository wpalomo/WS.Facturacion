using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class MotivoRechazoDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Motivos de Rechazos        
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>        
        /// <returns>Lista de MotivosRechazos</returns>
        /// ***********************************************************************************************
        public static MotivoRechazoL getMotivosRechazos(Conexion oConn)
        {
            MotivoRechazoL oMotivoRechazos = new MotivoRechazoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_MotivoRechazo_GetMotivoRechazos";

                oCmd.CommandTimeout = 5000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMotivoRechazos.Add(CargarMotivoRechazo(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMotivoRechazos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de MotivoRechazo
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de MotivoRechazo</param>
        /// <returns>elemento MotivoRechazo</returns>
        /// ***********************************************************************************************
        internal static MotivoRechazo CargarMotivoRechazo(System.Data.IDataReader oDR)
        {
            MotivoRechazo oMotivoRechazo = new MotivoRechazo(oDR["mot_codig"].ToString(), oDR["mot_descr"].ToString());

            return oMotivoRechazo;
        }
    }
}

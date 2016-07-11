using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Tesoreria;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de los Cambios de personas a cargo de una estación, terminal, etc
    /// </summary>
    ///****************************************************************************************************
    public class CambiosDt
    {
        #region CAMBIOSUPERVISOR: Metodos relacionados con el supervisor a cargo.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el supervisor a cargo de una estación
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <returns>Objeto de Supervisor a Cargo</returns>
        /// ***********************************************************************************************
        public static CambioSupervisor getSupervisorACargo(Conexion oConn,
                                                     int estacion)
        {
            CambioSupervisor oCambioSupervisor = null;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CambiosSupervisor_GetSupervisorACargo";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                if(oDR.Read())
                {
                    oCambioSupervisor = CargarCambioSupervisor(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCambioSupervisor;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un CambioSupervisor
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Categorias manuales</param>
        /// <returns>Objeto Cambio Supervisor</returns>
        /// ***********************************************************************************************
        private static CambioSupervisor CargarCambioSupervisor(System.Data.IDataReader oDR)
        {
            CambioSupervisor oCambioSupervisor = new CambioSupervisor();
            oCambioSupervisor.Estacion = new Estacion((byte)oDR["cam_coest"], oDR["est_nombr"].ToString());
            oCambioSupervisor.FechaInicio = (DateTime)oDR["cam_fecin"];
            oCambioSupervisor.FechaFinal = Util.DbValueToNullable<DateTime>(oDR["cam_fecfi"]);
            oCambioSupervisor.Identity = (int) oDR["cam_ident"];
            oCambioSupervisor.Supervisor = new Usuario(oDR["cam_supid"].ToString(), oDR["use_nombr"].ToString());
            oCambioSupervisor.Supervisor.PerfilActivo = new Perfil(oDR["use_grupo"].ToString(), oDR["gru_visua"].ToString());
            oCambioSupervisor.Parte = new Parte((int)oDR["cam_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);

            return oCambioSupervisor;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una asignacion de supervisor en la base de datos
        /// </summary>
        /// <param name="oCambio">CambioSupervisor - Objeto con la informacion de la asignacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setSupervisorACargo(CambioSupervisor oCambio, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CambiosSupervisor_setSupervisorACargo";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oCambio.Estacion.Numero;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oCambio.Supervisor.ID;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oCambio.Parte.Numero;

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

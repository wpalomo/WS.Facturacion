using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;


namespace Telectronica.Peaje
{
    public class AdministradoraTagsDt
    {

        #region OSAsDt: Clase de Datos de OSAs.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de OSAs definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo de OSA a filtrar</param>
        /// <returns>Lista de OSAs</returns>
        /// ***********************************************************************************************
        public static AdministradoraTagsL getOSAs(Conexion oConn, int? codigoOSA)
        {
            AdministradoraTagsL oOSAs = new AdministradoraTagsL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_GetOSAs";
                oCmd.Parameters.Add("@adt_codig", SqlDbType.TinyInt).Value = codigoOSA;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oOSAs.Add(CargarOSA(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oOSAs;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de OSAs 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de OSAs</param>
        /// <returns>Lista con el elemento OSAs de la base de datos</returns>
        /// ***********************************************************************************************
        private static AdministradoraTags CargarOSA(System.Data.IDataReader oDR)
        {
            AdministradoraTags oOSA = new AdministradoraTags(int.Parse(oDR["adt_codig"].ToString()), oDR["adt_descr"].ToString());

            return oOSA;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una OSA en la base de datos
        /// </summary>
        /// <param name="oOSA">OSA - Objeto con la informacion de la OSA a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addOSA(AdministradoraTags oOSA, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addOSA";

                oCmd.Parameters.Add("@adt_codig", SqlDbType.TinyInt).Value = oOSA.Codigo;
                oCmd.Parameters.Add("@adt_descr", SqlDbType.VarChar, 50).Value = oOSA.Descripcion;

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
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este Código de Administradora ya existe");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una OSA en la base de datos
        /// </summary>
        /// <param name="oOSA">OSA - Objeto con la informacion de la OSA a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updOSA(AdministradoraTags oOSA, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_updOSA";

                oCmd.Parameters.Add("@adt_codig", SqlDbType.TinyInt).Value = oOSA.Codigo;
                oCmd.Parameters.Add("@adt_descr", SqlDbType.VarChar, 50).Value = oOSA.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro con este código de Administradora");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una OSA de la base de datos
        /// </summary>
        /// <param name="OSA">Int - Código de OSA</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delOSA(int codOSA, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delOSA";

                oCmd.Parameters.Add("@adt_codig", SqlDbType.TinyInt).Value = codOSA;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro con este código de Administradora");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void delOSA(AdministradoraTags oOSA, Conexion oConn)
        {
            delOSA(oOSA.Codigo, oConn);
        }

        //Lo que trae esta son EMISORES y NO ADMINISTRADORAS (usar getOSas)

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Administradoras de Tag
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>        
        /// <returns>Lista de Administradoras de Tag</returns>
        /// ***********************************************************************************************
        public static AdministradoraTagsL getAdministradorasTags(Conexion oConn)
        {
            AdministradoraTagsL oAdministradoraTag = new AdministradoraTagsL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_AdministradoraTag_GetAdministradoraTag";

                oCmd.CommandTimeout = 5000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oAdministradoraTag.Add(CargarAdministradoraTags(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAdministradoraTag;
        }
        */
        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de AdministradoraTags
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Emitag</param>
        /// <returns>elemento AdministradoraTags</returns>
        /// ***********************************************************************************************
        internal static AdministradoraTags CargarAdministradoraTags(System.Data.IDataReader oDR)
        {
            AdministradoraTags oAdministradoraTags = new AdministradoraTags(Convert.ToInt32(oDR["emi_codig"].ToString()), oDR["emi_descr"].ToString());

            return oAdministradoraTags;
        }
         */
        #endregion

    }
}

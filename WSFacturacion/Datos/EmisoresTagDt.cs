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
    public class EmisoresTagDt
    {

        #region EmisoresTagDs: Clase de Datos de Emisores de Tag.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Emisores de Tag definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo del Emisor a filtrar</param>
        /// <returns>Lista de Emisores</returns>
        /// ***********************************************************************************************
        public static EmisoresTagL getEmisoresTag(Conexion oConn, string codEmisor, int? codAdmin)
        {
            EmisoresTagL oEmisoresTag = new EmisoresTagL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_EmisoresTag_GetEmisoresTag";
                oCmd.Parameters.Add("@emi_codig", SqlDbType.VarChar, 5).Value = codEmisor;
                oCmd.Parameters.Add("@emi_admtg", SqlDbType.TinyInt).Value = codAdmin;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEmisoresTag.Add(CargarEmisores(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEmisoresTag;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Emisores 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Emisores</param>
        /// <returns>Lista con el elemento Emisores de Tag de la base de datos</returns>
        /// ***********************************************************************************************
        private static EmisoresTag CargarEmisores(System.Data.IDataReader oDR)
        {
            AdministradoraTags oOSA = new AdministradoraTags(int.Parse(oDR["adt_codig"].ToString()), oDR["adt_descr"].ToString());

            EmisoresTag oEmisoresTag = new EmisoresTag(oDR["emi_codig"].ToString(), oDR["emi_descr"].ToString(), oOSA);

            return oEmisoresTag;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Emisor en la base de datos
        /// </summary>
        /// <param name="oEmisoresTag">Emisor de Tag - Objeto con la informacion del Emisor a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addEmisorTag(EmisoresTag oEmisorTag, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_EmisoresTag_addEmisorTag";

                oCmd.Parameters.Add("@emi_codig", SqlDbType.VarChar, 5).Value = oEmisorTag.Codigo;
                oCmd.Parameters.Add("@emi_descr", SqlDbType.VarChar, 80).Value = oEmisorTag.Descripcion;
                oCmd.Parameters.Add("@emi_admtg", SqlDbType.TinyInt).Value = oEmisorTag.Administradora.Codigo;

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
                        msg = Traduccion.Traducir("Este Código de Emisor ya existe");
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
        /// Modifica un Emisor de Tag en la base de datos
        /// </summary>
        /// <param name="oEmisoresTag">Emisores de Tag - Objeto con la informacion del Emisor a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updEmisorTag(EmisoresTag oEmisorTag, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_EmisoresTag_updEmisorTag";

                oCmd.Parameters.Add("@emi_codig", SqlDbType.VarChar, 5).Value = oEmisorTag.Codigo;
                oCmd.Parameters.Add("@emi_descr", SqlDbType.VarChar, 80).Value = oEmisorTag.Descripcion;
                oCmd.Parameters.Add("@emi_admtg", SqlDbType.TinyInt).Value = oEmisorTag.Administradora.Codigo;

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
                        msg = Traduccion.Traducir("No existe el registro con ese código de Emisor");
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
        /// Elimina un Emisor de la base de datos
        /// </summary>
        /// <param name="codEmisor">Int - Código de Emisor</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEmisorTag(string codEmisor, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_EmisoresTag_delEmisorTag";

                oCmd.Parameters.Add("@emi_codig", SqlDbType.VarChar, 5).Value = codEmisor;

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
                        msg = Traduccion.Traducir("No existe el registro con ese código de Emisor");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void delEmisorTag(EmisoresTag oEmisorTag, Conexion oConn)
        {
            delEmisorTag(oEmisorTag.Codigo, oConn);
        }

        #endregion

    }
}


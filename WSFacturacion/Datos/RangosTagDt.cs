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
    public class RangosTagDt
    {

        #region RangosTagDs: Clase de Datos de Rangos de Tag.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Rangos de Tag definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo del Rango a filtrar</param>
        /// <returns>Lista de Rangos</returns>
        /// ***********************************************************************************************
        public static RangosTagL getRangosTag(Conexion oConn, int? codigoRango)
        {
            RangosTagL oRangosTag = new RangosTagL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RangosTag_GetRangosTag";
                oCmd.Parameters.Add("@ran_ident", SqlDbType.Int).Value = codigoRango;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRangosTag.Add(CargarRangos(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRangosTag;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Rangos 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Rangos</param>
        /// <returns>Lista con el elemento Rangos de Tag de la base de datos</returns>
        /// ***********************************************************************************************
        private static RangosTag CargarRangos(System.Data.IDataReader oDR)
        {
            AdministradoraTags oOSA = new AdministradoraTags(int.Parse(oDR["adt_codig"].ToString()), oDR["adt_descr"].ToString());

            EmisoresTag oEmisor = null;

            if (oDR["emi_codig"] != DBNull.Value)
            {
                oEmisor = new EmisoresTag(oDR["emi_codig"].ToString(), oDR["emi_descr"].ToString());
            }

            TecnoTag oTecnoTag = new TecnoTag(int.Parse(oDR["tec_codig"].ToString()), oDR["tec_descr"].ToString());

            RangosTag oRangosTag = new RangosTag((int)oDR["ran_ident"], oDR["ran_desde"].ToString(), oDR["ran_hasta"].ToString(), oOSA, oEmisor, oTecnoTag);

            return oRangosTag;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Rango en la base de datos
        /// </summary>
        /// <param name="oRangosTag">Rango de Tag - Objeto con la informacion del Rango a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addRangoTag(RangosTag oRangoTag, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RangosTag_addRangoTag";

                oCmd.Parameters.Add("@ran_admtag", SqlDbType.TinyInt).Value = oRangoTag.Administradora.Codigo;
                if (oRangoTag.Emisor != null)
                    oCmd.Parameters.Add("@ran_emitag", SqlDbType.VarChar, 5).Value = oRangoTag.Emisor.Codigo;
                else
                    oCmd.Parameters.Add("@ran_emitag", SqlDbType.VarChar, 5).Value = null;
                oCmd.Parameters.Add("@ran_desde", SqlDbType.VarChar, 10).Value = oRangoTag.RangoDesde;
                oCmd.Parameters.Add("@ran_hasta", SqlDbType.VarChar, 10).Value = oRangoTag.RangoHasta;
                oCmd.Parameters.Add("@ran_tectag", SqlDbType.TinyInt).Value = oRangoTag.Tecnologia.Codigo;

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
                        msg = Traduccion.Traducir("Este Código de Rango ya existe");
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
        /// Modifica un Rango de Tag en la base de datos
        /// </summary>
        /// <param name="oRangosTag">Rangos de Tag - Objeto con la informacion del Rango a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updRangoTag(RangosTag oRangoTag, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RangosTag_updRangoTag";

                oCmd.Parameters.Add("@ran_ident", SqlDbType.Int).Value = oRangoTag.Codigo;
                oCmd.Parameters.Add("@ran_admtag", SqlDbType.TinyInt).Value = oRangoTag.Administradora.Codigo;
                if (oRangoTag.Emisor != null)
                    oCmd.Parameters.Add("@ran_emitag", SqlDbType.VarChar, 5).Value = oRangoTag.Emisor.Codigo;
                else
                    oCmd.Parameters.Add("@ran_emitag", SqlDbType.VarChar, 5).Value = null;
                oCmd.Parameters.Add("@ran_desde", SqlDbType.VarChar, 10).Value = oRangoTag.RangoDesde;
                oCmd.Parameters.Add("@ran_hasta", SqlDbType.VarChar, 10).Value = oRangoTag.RangoHasta;
                oCmd.Parameters.Add("@ran_tectag", SqlDbType.TinyInt).Value = oRangoTag.Tecnologia.Codigo;

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
                        msg = Traduccion.Traducir("No existe el registro con ese código de Rango");
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
        /// Elimina un Rango de la base de datos
        /// </summary>
        /// <param name="codRango">Int - Código de Rango</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delRangoTag(int codRango, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_RangosTag_delRangoTag";

                oCmd.Parameters.Add("@ran_ident", SqlDbType.TinyInt).Value = codRango;

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
                        msg = Traduccion.Traducir("No existe el registro con ese código de Rango");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void delRangoTag(RangosTag oRangoTag, Conexion oConn)
        {
            delRangoTag(oRangoTag.Codigo, oConn);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tecnología que utiliza la administradora de Tag definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo de la Tecnología a filtrar</param>
        /// <returns>Lista de Rangos</returns>
        /// ***********************************************************************************************
        public static TecnoTagL getTecnoTag(Conexion oConn, int? codigoTecno)
        {
            TecnoTagL oTecnoTag = new TecnoTagL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_TecnoTag_GetTecnoTag";
                oCmd.Parameters.Add("@tec_codig", SqlDbType.Int).Value = codigoTecno;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTecnoTag.Add(CargarTecno(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTecnoTag;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Tecnologías
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tecnologías</param>
        /// <returns>Lista con el elemento Tegnología de Tag de la base de datos</returns>
        /// ***********************************************************************************************
        private static TecnoTag CargarTecno(System.Data.IDataReader oDR)
        {
            TecnoTag oTecnoTag = new TecnoTag(int.Parse(oDR["tec_codig"].ToString()), oDR["tec_descr"].ToString());

            return oTecnoTag;
        }


        #endregion

    }
}


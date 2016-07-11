using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Validacion;

namespace Telectronica.Validacion
{
    public class ConfiguracionValidacionDt
    {
        #region ESATCIONVALIDACION: Clase de Datos de Configuracion de Estacion de Valdiacion.


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega que la estacion ya es valida y debe incluirse en los listados
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto con la informacion de la estacion</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setEstacionValida(Estacion oEstacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Configuracion_setEstacionValida";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oEstacion.Numero;

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

        #region Anomalia: Clase de Datos de Anomalia.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Anomalias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Anomalias</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getAnomalias(Conexion oConn, int? codAnomalia)
        {
            AnomaliaL anomalias = new AnomaliaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetAnomalias";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.TinyInt).Value = codAnomalia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    anomalias.Add(CargarAnomalias(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return anomalias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Anomalias
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de anomalias</param>
        /// <returns>Lista con el elemento Anomalia de la base de datos</returns>
        /// ***********************************************************************************************
        private static Anomalia CargarAnomalias(SqlDataReader oDR)
        {
            Anomalia oAnomalia = new Anomalia(Convert.ToInt16(oDR["anom_codig"]), oDR["anom_descr"].ToString());

            return oAnomalia;
        }

        #endregion

        #region CodigoValidacion: Clase de Datos de CodigoValidacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Codigos de Validacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>        
        /// <returns>Lista de Codigos de Validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacionGeneral(Conexion oConn)
        {
            CodigoValidacionL codigosValidacion = new CodigoValidacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetCodigosValidacionGeneral";                

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosValidacion.Add(CargarCodigosValidacionGeneral(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Codigos de Validacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Monedas</param>
        /// <returns>Lista con el elemento CodValid de la base de datos</returns>
        /// ***********************************************************************************************
        private static CodigoValidacion CargarCodigosValidacionGeneral(SqlDataReader oDR)
        {
            Anomalia oAnomalia = CargarAnomalias(oDR);

            CodigoValidacion oCodigoValidacion = new CodigoValidacion();

            oCodigoValidacion.Anomalia = oAnomalia;
                        
            oCodigoValidacion.Tipo = Convert.ToString(oDR["TipoAceptacion"]); 
            oCodigoValidacion.Codigo = Convert.ToInt16(oDR["CodigoAceptacion"]); 
            oCodigoValidacion.Descripcion = Convert.ToString(oDR["DescripcionCodigoAceptacion"]);
            if (oDR["FormaPago"] != DBNull.Value)
                oCodigoValidacion.FormaPago = Convert.ToString(oDR["FormaPago"]);
            if (oDR["MedioPago"] != DBNull.Value) 
                oCodigoValidacion.MedioPago = Convert.ToString(oDR["MedioPago"]);
            if (oDR["SubformaPago"] != DBNull.Value)
                oCodigoValidacion.SubformaPago = Convert.ToInt16(oDR["SubformaPago"]);
            if (oDR["DescripcionFormaPago"] != DBNull.Value) 
                oCodigoValidacion.DescripcionFormaPago = Convert.ToString(oDR["DescripcionFormaPago"]);
            if (oDR["TipoTarifa"] != DBNull.Value)
                oCodigoValidacion.TipoTarifa = Convert.ToInt32(oDR["TipoTarifa"]); 
            if (oDR["PorDefecto"] != DBNull.Value)
                oCodigoValidacion.PorDefecto = Convert.ToString(oDR["PorDefecto"]);
            if (oDR["Invisible"] != DBNull.Value)
                oCodigoValidacion.CodigoInvisible = Convert.ToString(oDR["Invisible"]);
            return oCodigoValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Codigos de Validacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una Anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo de validacion
        /// <param name="medioPago">string - Tipo de medio de pago</param>
        /// <param name="formaPago">string - Subtipo de medio de pago</param>
        /// <param name="subformaPago">tinyint - Tipo de forma de pago</param>
        /// <returns>Lista de Codigos de Validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion(Conexion oConn, int? codAnomalia, string tipo, String medioPago, String formaPago, int? subformaPago)
        {
            CodigoValidacionL codigosValidacion = new CodigoValidacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetCodigosValidacion";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char).Value = tipo;
                oCmd.Parameters.Add("@tipop", SqlDbType.Char).Value = medioPago;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char).Value = formaPago;
                oCmd.Parameters.Add("@subfp", SqlDbType.TinyInt).Value = subformaPago;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosValidacion.Add(CargarCodigosValidacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un Codigo de Validacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Codigo de Validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacion getCodigoValidacion(Conexion oConn, int? codAnomalia, string tipo, int? codigo)
        {
            CodigoValidacion oCodigosValidacion = new CodigoValidacion();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetCodigosValidacion";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char).Value = tipo;
                oCmd.Parameters.Add("@cod", SqlDbType.TinyInt).Value = codigo;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oCodigosValidacion = CargarCodigosValidacion(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCodigosValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Codigos de Validacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Monedas</param>
        /// <returns>Lista con el elemento CodValid de la base de datos</returns>
        /// ***********************************************************************************************
        private static CodigoValidacion CargarCodigosValidacion(SqlDataReader oDR)
        {
            Anomalia oAnomalia = CargarAnomalias(oDR);

            CodigoValidacion oCodigoValidacion = new CodigoValidacion(oDR["cod_tipo"].ToString(), Convert.ToInt16(oDR["cod_codig"]), oDR["cod_descr"].ToString(), oAnomalia,
                                                               " "       /*oDR["cod_otrofal"].ToString() */, oDR["cod_invisible"].ToString(), oDR["cod_defecto"].ToString(), oDR["habParaDefecto"].ToString());

            return oCodigoValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un codigo de validacion en la base de datos
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacion - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCodigoValidacion(CodigoValidacion oCodigoValidacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_addCodigoValidacion";

                oCmd.Parameters.Add("@cod_anomal", SqlDbType.TinyInt).Value = oCodigoValidacion.Anomalia.Codigo;
                oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = oCodigoValidacion.Codigo;
                oCmd.Parameters.Add("@cod_defecto", SqlDbType.Char,1).Value = oCodigoValidacion.PorDefecto;
                oCmd.Parameters.Add("@cod_descr", SqlDbType.VarChar,50).Value = oCodigoValidacion.Descripcion;
                oCmd.Parameters.Add("@cod_invisible", SqlDbType.Char,1).Value = oCodigoValidacion.CodigoInvisible;
                oCmd.Parameters.Add("@cod_otrofal", SqlDbType.Char,1).Value = oCodigoValidacion.OtrosFaltantes;
                oCmd.Parameters.Add("@cod_tipo", SqlDbType.Char,1).Value = oCodigoValidacion.Tipo;       

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("El Código de Validación ya existe");
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
        /// Modifica un codigo de validacion en la base de datos
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacion - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updCodigoValidacion(CodigoValidacion oCodigoValidacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_updCodigoValidacion";

                oCmd.Parameters.Add("@cod_anomal", SqlDbType.TinyInt).Value = oCodigoValidacion.Anomalia.Codigo;
                oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = oCodigoValidacion.Codigo;
                oCmd.Parameters.Add("@cod_defecto", SqlDbType.Char, 1).Value = oCodigoValidacion.PorDefecto;
                oCmd.Parameters.Add("@cod_descr", SqlDbType.VarChar, 50).Value = oCodigoValidacion.Descripcion;
                oCmd.Parameters.Add("@cod_invisible", SqlDbType.Char, 1).Value = oCodigoValidacion.CodigoInvisible;
                oCmd.Parameters.Add("@cod_otrofal", SqlDbType.Char, 1).Value = oCodigoValidacion.OtrosFaltantes;
                oCmd.Parameters.Add("@cod_tipo", SqlDbType.Char, 1).Value = oCodigoValidacion.Tipo;   

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
                        msg = Traduccion.Traducir("No existe el registro del Código de Validación");
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
        /// Elimina una codigo de validacion de la base de datos
        /// </summary>
        /// <param name="Codigo">Int - codigo a eliminar</param>
        /// <param name="Tipo">String - tipo a eliminar</param>
        /// <param name="Anomalia">Int - anomalia a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCodigoValidacion(Conexion oConn, int? codigo, string tipo, int codAnomalia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_delCodigoValidacion";

                oCmd.Parameters.Add("@cod_anomal", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@cod_tipo", SqlDbType.Char).Value = tipo;
                oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = codigo;

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
                        msg = Traduccion.Traducir("No existe el registro del Código de Validación");
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

        #region CodigoValidacionFormaPago: Clase de Datos de CodigoValidacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias, formas de pago y tipos de exentos para la validacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de de anomalias, formas de pago y tipos de exentos para la validacion</returns>
        /// ***********************************************************************************************
        public static AnomaliaFormaPagoL getAnomaliasFormaPago(Conexion oConn, int? codAnomalia)
        {
            AnomaliaFormaPagoL anomaliaFormaPago = new AnomaliaFormaPagoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetAnomaliasFormaPago";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.Int).Value = codAnomalia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    anomaliaFormaPago.Add(CargarAnomaliasFormaPago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return anomaliaFormaPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de codigos validacion forma de pago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static AnomaliaFormaPago CargarAnomaliasFormaPago(SqlDataReader oDR)
        {
            Anomalia anomalia = new Anomalia(Convert.ToInt16(oDR["anom_codig"]), oDR["anom_descr"].ToString());

            FormaPagoValidacion formaPago = new FormaPagoValidacion(oDR["fvr_tipop"].ToString(), oDR["fvr_tipbo"].ToString(), Convert.ToInt16(oDR["fvr_subfp"]), oDR["for_descr"].ToString(), oDR["cod_descr"].ToString(), Convert.ToInt16(oDR["fvr_subfp"]));

            AnomaliaFormaPago oAnomaliaFormaPago = new AnomaliaFormaPago(anomalia, formaPago);

            return oAnomaliaFormaPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion por forma de pago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Codigo de validacion por forma de pago</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionFormaPagoL getCodigosValidacionFormaPago(Conexion oConn, int? codAnomalia, String medioPago, String formaPago, int? subformaPago)
        {
            CodigoValidacionFormaPagoL codigosValidacionFormaPago = new CodigoValidacionFormaPagoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_GetCodigosValidacionFormaPago";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@tipop", SqlDbType.Char, 1).Value = medioPago;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char, 1).Value = formaPago;
                oCmd.Parameters.Add("@subfp", SqlDbType.TinyInt).Value = subformaPago;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    codigosValidacionFormaPago.Add(CargarCodigosValidacionFormaPago(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosValidacionFormaPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de codigos validacion forma de pago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static CodigoValidacionFormaPago CargarCodigosValidacionFormaPago(SqlDataReader oDR)
        {
            Anomalia oAnomalia = new Anomalia(Convert.ToInt16(oDR["anom_codig"]), oDR["anom_descr"].ToString());
            CodigoValidacion oCodigoValidacion = new CodigoValidacion(oDR["cod_tipo"].ToString(), Convert.ToInt16(oDR["cod_codig"]), oDR["cod_descr"].ToString(), oAnomalia);
            CodigoValidacionFormaPago oCodigoValidacionFormaPago = new CodigoValidacionFormaPago(oCodigoValidacion, oDR["cod_checked"].ToString(), oDR["cod_defecto"].ToString());
            return oCodigoValidacionFormaPago;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion por forma de pago
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Codigo de validacion por forma de pago</returns>
        /// ***********************************************************************************************
        public static DataSet rptCodigosValidacionFormaPago(Conexion oConn, int? codAnomalia)
        {
            DataSet codigosValidacionFormaPago = new DataSet();
            codigosValidacionFormaPago.DataSetName = "RptValidacion_CodigosValidacionFormaPagoDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_rptCodigosValidacionFormaPago";
                oCmd.Parameters.Add("@codAnoma", SqlDbType.TinyInt).Value = codAnomalia;

                oCmd.CommandTimeout = 500;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(codigosValidacionFormaPago, "CodigosValidacionFormaPago");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return codigosValidacionFormaPago;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un codigo de validacion por forma de pago en la base de datos
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacionFormaPago - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updCodigoValidacionFormaPago(CodigoValidacionFormaPago oCodigoValidacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_updCodigoValidacionFormaPago";

                oCmd.Parameters.Add("@cod_anomal", SqlDbType.TinyInt).Value = oCodigoValidacion.CodigoValidacion.Anomalia.Codigo;
                oCmd.Parameters.Add("@cod_tipo", SqlDbType.Char, 1).Value = "A";
                oCmd.Parameters.Add("@cod_tipop", SqlDbType.Char, 1).Value = oCodigoValidacion.FormaPago.MedioPago;
                oCmd.Parameters.Add("@cod_tipbo", SqlDbType.Char, 1).Value = oCodigoValidacion.FormaPago.FormaPago;
                if (oCodigoValidacion.FormaPago.SubformaPago > 0)
                    oCmd.Parameters.Add("@cod_subfp", SqlDbType.TinyInt).Value = oCodigoValidacion.FormaPago.SubformaPago;
                oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = oCodigoValidacion.CodigoValidacion.Codigo;
                oCmd.Parameters.Add("@cod_defecto", SqlDbType.Char, 1).Value = oCodigoValidacion.PorDefecto;
                oCmd.Parameters.Add("@checked", SqlDbType.Char, 1).Value = oCodigoValidacion.Checked; 

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Código de Validación");
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

        #region Controles: Clase para entidad de Controles_Gral.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de controles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de controles</returns>
        /// ***********************************************************************************************
        public static ControlesL getControles(Conexion oConn, int? id)
        {
            ControlesL controles = new ControlesL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_getControles";
                oCmd.Parameters.Add("@cod", SqlDbType.TinyInt).Value = id;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    controles.Add(CargarControles(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return controles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de controles
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla Controles</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static Controles CargarControles(SqlDataReader oDR)
        {
            Anomalia oAnomalia = new Anomalia(Convert.ToInt16(oDR["anom_codig"]), oDR["anom_descr"].ToString());
            Int16? codigoValidacion = null;
            if (!String.IsNullOrEmpty(oDR["cod_codig"].ToString())) codigoValidacion = Convert.ToInt16(oDR["cod_codig"]);
            CodigoValidacion oCodigoValidacion = new CodigoValidacion(oDR["cod_tipo"].ToString(), codigoValidacion, oDR["cod_descr"].ToString(), oAnomalia);
            FormaPagoValidacion oFormaPago = new FormaPagoValidacion(oDR["for_tipop"].ToString(), oDR["for_tipbo"].ToString(), null, oDR["for_descr"].ToString());
            Estacion oEstacion = !String.IsNullOrEmpty(oDR["est_codig"].ToString()) ? new Estacion(Convert.ToInt16(oDR["est_codig"]), oDR["est_nombr"].ToString()) : new Estacion(0, "");
            Controles oControles = new Controles();
            oControles.Id = Convert.ToInt16(oDR["cng_ident"]);
            oControles.Prioridad = Convert.ToInt16(oDR["cng_prior"]);
            oControles.Estacion = oEstacion;
            oControles.FormaPago = oFormaPago;
            oControles.Porcentaje = Convert.ToInt16(oDR["cng_repet"]);
            oControles.CodigoValidacion = oCodigoValidacion;
            oControles.TipoValidacion = new TipoValidacion {Codigo = oDR["tip_codig"].ToString(), Descripcion = oDR["tip_descr"].ToString()};

            if( oDR["cng_catde"] != DBNull.Value)
                oControles.CategoriaDac = new CategoriaManual(Convert.ToByte(oDR["cng_catde"]), oDR["CategoriaDetectada"].ToString());
            if( oDR["cng_catma"] != DBNull.Value)
                oControles.Categoria = new CategoriaManual(Convert.ToByte(oDR["cng_catma"]), oDR["CategoriaTabulada"].ToString());
            //if (oDR["cng_categ"] != DBNull.Value)
            //{
            //oControles.Categoria = new CategoriaManual();
            //    oControles.Categoria.Categoria = Convert.ToByte(oDR["cng_categ"]);
            //    oControles.Categoria.Descripcion = oDR["cat_descr"].ToString();
            //}

            //if (oDR["cng_porva"] != DBNull.Value)
            //    oControles.PorcentajeValidacion = Convert.ToInt16(oDR["cng_porva"]);

            return oControles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un control en la base de datos
        /// </summary>
        /// <param name="oControles">Controles - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addControles(Controles oControles, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_addControles";

                oCmd.Parameters.Add("@cng_prior", SqlDbType.TinyInt).Value = oControles.Prioridad;
                if (oControles.Estacion.Numero != 0)
                    oCmd.Parameters.Add("@cng_coest", SqlDbType.TinyInt).Value = oControles.Estacion.Numero;
                oCmd.Parameters.Add("@cng_anomal", SqlDbType.TinyInt).Value = oControles.CodigoValidacion.Anomalia.Codigo;
                if (oControles.FormaPago.MedioPago != null)
                    oCmd.Parameters.Add("@cng_tipop", SqlDbType.Char, 1).Value = oControles.FormaPago.MedioPago;
                if (oControles.FormaPago.FormaPago != null)
                    oCmd.Parameters.Add("@cng_tipbo", SqlDbType.Char, 1).Value = oControles.FormaPago.FormaPago;
                oCmd.Parameters.Add("@cng_tipval", SqlDbType.Char, 1).Value = oControles.TipoValidacion.Codigo;
                oCmd.Parameters.Add("@cng_repet", SqlDbType.TinyInt).Value = oControles.Porcentaje;
                if (!String.IsNullOrEmpty(oControles.CodigoValidacion.Tipo))
                    oCmd.Parameters.Add("@cng_estad", SqlDbType.Char, 1).Value = oControles.CodigoValidacion.Tipo;
                if (oControles.CodigoValidacion.Codigo != 0)
                    oCmd.Parameters.Add("@cng_valid", SqlDbType.TinyInt).Value = oControles.CodigoValidacion.Codigo;
                if (oControles.Categoria != null && oControles.Categoria.Categoria >= 0)
                    oCmd.Parameters.Add("@cng_catma", SqlDbType.TinyInt).Value = oControles.Categoria.Categoria;
                if (oControles.CategoriaDac != null && oControles.CategoriaDac.Categoria >= 0)
                    oCmd.Parameters.Add("@cng_catde", SqlDbType.TinyInt).Value = oControles.CategoriaDac.Categoria;
                //oCmd.Parameters.Add("@cng_porva", SqlDbType.TinyInt).Value = oControles.PorcentajeValidacion;
                //if (oControles.Categoria != null)
                //    oCmd.Parameters.Add("@cng_categ", SqlDbType.TinyInt).Value = oControles.Categoria.Categoria;

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
                        msg = Traduccion.Traducir("El registro ya existe");
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
        /// Modifica un control en la base de datos
        /// </summary>
        /// <param name="oControles">Controles - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updControles(Controles oControles, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_updControles";

                oCmd.Parameters.Add("@cng_prior", SqlDbType.TinyInt).Value = oControles.Prioridad;
                oCmd.Parameters.Add("@cng_tipval", SqlDbType.Char, 1).Value = oControles.TipoValidacion.Codigo;
                oCmd.Parameters.Add("@cng_estad", SqlDbType.Char, 1).Value = oControles.CodigoValidacion.Tipo;
                oCmd.Parameters.Add("@cng_valid", SqlDbType.TinyInt).Value = oControles.CodigoValidacion.Codigo;
                oCmd.Parameters.Add("@cng_ident", SqlDbType.TinyInt).Value = oControles.Id;
                //oCmd.Parameters.Add("@cng_repet", SqlDbType.TinyInt).Value = oControles.Porcentaje; // Agregado permitir modificar el porcentaje de discrepancia
                //oCmd.Parameters.Add("@cng_porva", SqlDbType.TinyInt).Value = oControles.PorcentajeValidacion;
                //if (oControles.Categoria != null)
                //    oCmd.Parameters.Add("@cng_categ", SqlDbType.TinyInt).Value = oControles.Categoria.Categoria;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro");
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
        /// Elimina un Control Gral de la base de datos
        /// </summary>
        /// <param name="id">Int - Identity a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delControles(Conexion oConn, int id)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_delControles";

                oCmd.Parameters.Add("@cng_ident", SqlDbType.TinyInt).Value = id;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro");
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
        /// Devuelve la lista de tipos de validacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de tipos de validacion</returns>
        /// ***********************************************************************************************
        public static TipoValidacionL getTiposValidacion(Conexion oConn)
        {
            TipoValidacionL tiposValidacion = new TipoValidacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_getTiposValidacion";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    tiposValidacion.Add(CargarTiposValidacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tiposValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de tipos de validacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla TIPVAL</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoValidacion CargarTiposValidacion(SqlDataReader oDR)
        {
            TipoValidacion oTipoValidacion = new TipoValidacion(oDR["tip_codig"].ToString(), oDR["tip_descr"].ToString());
            return oTipoValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tipos de validacion Invisible
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de tipos de validacion Invisible</returns>
        /// ***********************************************************************************************
        public static TipoValidacionInvisibleL getTiposValidacionInvisible(Conexion oConn, int? codAnomalia, String tipo, String invisible)
        {
            TipoValidacionInvisibleL tiposValidacion = new TipoValidacionInvisibleL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_ConfiguracionValidacion_getTiposValidacionInvisible";
                oCmd.Parameters.Add("@codAnom", SqlDbType.TinyInt).Value = codAnomalia;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = tipo;
                oCmd.Parameters.Add("@invisible", SqlDbType.Char, 1).Value = invisible;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    tiposValidacion.Add(CargarTiposValidacionInvisible(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tiposValidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de tipos de validacion Invisible
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla TIPOVALID</param>
        /// <returns>el elemento de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoValidacionInvisible CargarTiposValidacionInvisible(SqlDataReader oDR)
        {
            Anomalia oAnomalia = new Anomalia(Convert.ToInt16(oDR["anom_codig"]), oDR["anom_descr"].ToString());
            TipoValidacionInvisible oTipoValidacion = new TipoValidacionInvisible(oAnomalia, oDR["tip_estad"].ToString(), oDR["tip_invisible"].ToString());
            return oTipoValidacion;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    public class MonedaDt
    {
        #region MonedaDT: Clase de Datos de Moneda.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Monedas definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMoneda">int - Codigo de Moneda a filtrar</param>
        /// <returns>Lista de Monedas</returns>
        /// ***********************************************************************************************
        public static MonedaL getMonedas(Conexion oConn, int? codigoMoneda, bool? referencia)
        {
            MonedaL oMonedas = new MonedaL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Monedas_GetMonedas";
                oCmd.Parameters.Add("@mon_moned", SqlDbType.TinyInt).Value = codigoMoneda;
                string sreferencia = null;
                if (referencia == true)
                    sreferencia = "S";
                else if (referencia == false)
                    sreferencia = "N";
                oCmd.Parameters.Add("@referencia", SqlDbType.Char,1).Value = sreferencia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oMonedas.Add(CargarMonedas(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oMonedas;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Monedas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Monedas</param>
        /// <returns>Lista con el elemento Moneda de la base de datos</returns>
        /// ***********************************************************************************************
        private static Moneda CargarMonedas(System.Data.IDataReader oDR)
        {
            Moneda oMoneda = new Moneda((Int16)oDR["mon_moned"],
                                        oDR["mon_descr"].ToString(),
                                        oDR["mon_simbo"].ToString(),
                                        oDR["esMonedaReferencia"].ToString());

            return oMoneda;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Moneda en la base de datos
        /// </summary>
        /// <param name="oMoneda">Moneda - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addMonedas(Moneda oMoneda, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Monedas_addMoneda";

                oCmd.Parameters.Add("@mon_descr", SqlDbType.VarChar, 30).Value = oMoneda.Desc_Moneda;
                oCmd.Parameters.Add("@mon_simbo", SqlDbType.Char, 3).Value = oMoneda.Simbolo;
                
                SqlParameter parIdentity = oCmd.Parameters.Add("@Ident", SqlDbType.Int);
                parIdentity.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                oMoneda.Codigo = Convert.ToInt16(parIdentity.Value);

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este Código de Moneda ya existe");
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
        /// Modifica una Moneda en la base de datos
        /// </summary>
        /// <param name="oMoneda">Moneda - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMonedas(Moneda oMoneda, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Monedas_updMoneda";

                oCmd.Parameters.Add("@mon_moned", SqlDbType.SmallInt).Value = oMoneda.Codigo;
                oCmd.Parameters.Add("@mon_descr", SqlDbType.VarChar, 30).Value = oMoneda.Desc_Moneda;
                oCmd.Parameters.Add("@mon_simbo", SqlDbType.Char,3).Value = oMoneda.Simbolo;

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
                        msg = Traduccion.Traducir("No existe el registro de la moneda");
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
        /// Elimina una Moneda de la base de datos
        /// </summary>
        /// <param name="Monedas">Int - Moneda a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMoneda(int Moneda, Conexion oConn)
        {   
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Monedas_delMoneda";

                oCmd.Parameters.Add("@mon_moned", SqlDbType.TinyInt).Value = Moneda;

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
                        msg = Traduccion.Traducir("No existe el registro de la Moneda");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        public static void delMoneda(Moneda oMoneda, Conexion oConn)
        {
            delMoneda(oMoneda.Codigo, oConn);
        }

        #endregion


        #region DenominacionDT: Clase de Datos de denominacion.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Denominaciones definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oDenominacion">Denominacion - Objeto con la informacion del mensaje a insertar</param>
        /// <returns>Lista de Denominaciones</returns>
        /// ***********************************************************************************************
        public static DenominacionL getDenominacion(Conexion oConn, int? CodMoneda, int? CodDenominacion)
        {
            DenominacionL oDenominacion = new DenominacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Denominaciones_getDenominaciones";
                oCmd.Parameters.Add("@den_moned", SqlDbType.SmallInt).Value = CodMoneda;
                oCmd.Parameters.Add("@den_denom", SqlDbType.SmallInt).Value = CodDenominacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oDenominacion.Add(CargarDenominaciones(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDenominacion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Denominaciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Denominaciones</param>
        /// <returns>Lista con el elemento Denominacion de la base de datos</returns>
        /// ***********************************************************************************************
        private static Denominacion CargarDenominaciones(System.Data.IDataReader oDR)
        {
            Denominacion oDenominacion = new Denominacion(new Moneda((Int16)oDR["den_moned"],
                                                                     oDR["mon_descr"].ToString(),
                                                                     oDR["mon_simbo"].ToString(), 
                                                                     ""),
                                                          (short) oDR["den_denom"],
                                                          oDR["den_descr"].ToString(),
                                                          (decimal)oDR["den_valor"], oDR["den_bil"].ToString());
            
            return oDenominacion;
        }
        

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Denominacion en la base de datos
        /// </summary>
        /// <param name="oDenominacion">Denominacion - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addDenominacion(Denominacion oDenominacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Denominaciones_addDenominacion";

                oCmd.Parameters.Add("@den_moned", SqlDbType.TinyInt).Value = oDenominacion.Moneda.Codigo;
                oCmd.Parameters.Add("@den_descr", SqlDbType.VarChar, 30).Value = oDenominacion.DescDenominacion;
                oCmd.Parameters.Add("@den_valor", SqlDbType.SmallMoney).Value = oDenominacion.ValorDenominacion;
                oCmd.Parameters.Add("@den_bil", SqlDbType.VarChar, 1).Value = oDenominacion.BilleteMoneda;

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
                        msg = Traduccion.Traducir("No existe el registro de la Denominación");
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
        /// Modifica una Denominacion en la base de datos
        /// </summary>
        /// <param name="oDenominacion">Denominacion - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updDenominacion(Denominacion oDenominacion, Conexion oConn)

        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Denominaciones_updDenominacion";

                oCmd.Parameters.Add("@den_moned", SqlDbType.TinyInt).Value = oDenominacion.Moneda.Codigo;
                oCmd.Parameters.Add("@den_denom", SqlDbType.TinyInt).Value = oDenominacion.CodDenominacion;
                oCmd.Parameters.Add("@den_descr", SqlDbType.VarChar,30).Value = oDenominacion.DescDenominacion;
                oCmd.Parameters.Add("@den_valor", SqlDbType.SmallMoney).Value = oDenominacion.ValorDenominacion;
                oCmd.Parameters.Add("@den_bil", SqlDbType.VarChar, 1).Value = oDenominacion.BilleteMoneda;

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
                        msg = Traduccion.Traducir("No existe el registro de la Denominación");
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
        /// Elimina una Denominacion de la base de datos
        /// </summary>
        /// <param name="Denominacion">Int - Denominacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delDenominacion(Denominacion oDenominacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Denominaciones_delDenominacion";

                oCmd.Parameters.Add("@den_moned", SqlDbType.TinyInt).Value = oDenominacion.Moneda.Codigo;
                oCmd.Parameters.Add("@den_denom", SqlDbType.TinyInt).Value = oDenominacion.CodDenominacion;

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
                        msg = Traduccion.Traducir("No existe el registro de la Denominación");
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


        #region COTIZACION: Clase de Datos de Cotizaciones


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de cotizaciones realizadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Estacion">Estacion - Estacion en la que se realizo la cotizacion</param>
        /// <param name="FechaInicialVigencia">Datetime - Fecha de inicio de busqueda de la cotizacion</param>
        /// <returns>Lista de Cotizaciones realizadas</returns>
        /// ***********************************************************************************************
        public static CotizacionL getCotizacionesCabecera(Conexion oConn, int? codigoEstacion, DateTime? FechaInicialVigencia, int? identity)
        {
            CotizacionL oCotizaciones = new CotizacionL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_getCotizacionesCabecera";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = codigoEstacion;
                oCmd.Parameters.Add("FecIn", SqlDbType.DateTime).Value = FechaInicialVigencia;
                oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = identity;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCotizaciones.Add(CargarCotizacionesCabecera(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oCotizaciones;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de cotizaciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cotizaciones</param>
        /// <returns>Lista con los elementos Cotizacion de la base de datos</returns>
        /// ***********************************************************************************************
        private static Cotizacion CargarCotizacionesCabecera(System.Data.IDataReader oDR)
        {
            Cotizacion oCotizacion = new Cotizacion();

            oCotizacion.Identity = (int)oDR["cot_ident"];
            oCotizacion.FechaInicialVigencia = (DateTime)oDR["cot_fecha"];
            oCotizacion.FechaFinalVigencia = Util.DbValueToNullable<DateTime>(oDR["cot_fecfi"]);
            oCotizacion.FechaCarga = (DateTime)oDR["cot_fecre"];

            // El numero de estacion puede ser 0. En ese caso el nombre lo pongo a mano
            Estacion oEstacion = new Estacion();
            oEstacion.Numero = (byte)oDR["cot_orige"];
            if (oEstacion.Numero == 0)
            {
                oEstacion.Nombre = Traduccion.getTextoGestion();
            }
            else
            {
                oEstacion.Nombre = oDR["est_nombr"].ToString();
            }

            oCotizacion.EstacionOrigen = oEstacion;


            return oCotizacion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todas las cotizaciones de monedas de una definicion de cotizacion determinada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="identity">int - Identity de una determinada definicion de cotizacion</param>
        /// <param name="estacion">int - Numero de estacion (La PK esta compuesta por identity y estacion)</param>
        /// <returns>Lista de valor de las cotizaciones de una determinada definicion de cotizaciones</returns>
        /// ***********************************************************************************************
        public static CotizacionDetalleL getCotizacionesDetalle(Conexion oConn, int estacion, int identity)
        {
            CotizacionDetalleL oCotizacionDetalleL = new CotizacionDetalleL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_getCotizacionesDetalle";
                oCmd.Parameters.Add("Coest", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("IdentCotiza", SqlDbType.Int).Value = identity;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCotizacionDetalleL.Add(CargarCotizacionesDetalle(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oCotizacionDetalleL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de cotizaciones realizadas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de detalle de Cotizaciones</param>
        /// <returns>Lista con los elementos CotizacionDetalle de la base de datos</returns>
        /// ***********************************************************************************************
        private static CotizacionDetalle CargarCotizacionesDetalle(System.Data.IDataReader oDR)
        {
            CotizacionDetalle oCotizacionDetalle = new CotizacionDetalle();
         
            oCotizacionDetalle.Moneda = new Moneda((short)oDR["ctd_moned"],
                                                   oDR["mon_descr"].ToString(), 
                                                   oDR["mon_simbo"].ToString(), "");
            oCotizacionDetalle.ValorCotizacion = (decimal)oDR["ctd_cotiz"];

            return oCotizacionDetalle;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la cabecera de una definicion de cotizacion en la base de datos
        /// </summary>
        /// <param name="oCotizacion">Cotizacion - Objeto con la informacion de la cotizacion definida</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCotizacionCabecera(Cotizacion oCotizacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_addCotizacionCabecera";

                oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = oCotizacion.EstacionOrigen.Numero;
                SqlParameter parFecIn = oCmd.Parameters.Add("@FecIn", SqlDbType.DateTime);
                parFecIn.Direction = ParameterDirection.InputOutput;
                parFecIn.Value = oCotizacion.FechaInicialVigencia;

                oCmd.Parameters.Add("@MonRef", SqlDbType.Int).Value = oCotizacion.MonedaReferencia.Codigo;

                // Valor identity insertado en la cabecera
                SqlParameter paramIdent = oCmd.Parameters.Add("@Ident", SqlDbType.Int);
                paramIdent.Direction = ParameterDirection.Output;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();


                int retval = (int)parRetVal.Value;
                oCotizacion.FechaInicialVigencia=(DateTime)parFecIn.Value;


                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                        msg = Traduccion.Traducir("La definición de la cotizacion ya existe");

                    throw new ErrorSPException(msg);
                }
                else
                {
                    oCotizacion.Identity = (int)paramIdent.Value;
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
        /// Agrega el detalle de las cotizaciones definidas en la base de datos
        /// </summary>
        /// <param name="IdentCotiza">int - Identity de la cabecera</param>
        /// <param name="oCotizacionDetalle">CotizacionDetalle - Objeto con la informacion de la cotizacion detallada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCotizacionDetalle(int estacion, int IdentCotiza, CotizacionDetalle oCotizacionDetalle, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_addCotizacionDetalle";
                oCmd.Parameters.Add("@IdentCotiza", SqlDbType.Int).Value = IdentCotiza;
                oCmd.Parameters.Add("@Estac", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@Moneda", SqlDbType.SmallInt).Value = oCotizacionDetalle.Moneda.Codigo;
                oCmd.Parameters.Add("@Valor", SqlDbType.SmallMoney).Value = oCotizacionDetalle.ValorCotizacion;

                // Valor de retorno del SP
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


        /// ***********************************************************************************************
        /// <summary>
        /// Elimina el detalle de las Cotizaciones de la base de datos
        /// </summary>
        /// <param name="oCotizacion">Cotizacion - Objeto con la informacion de las Cotizaciones detalle a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCotizacionDetalle(Cotizacion oCotizacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_delCotizacionDetalle";
                oCmd.Parameters.Add("@IdentCotiza", SqlDbType.Int).Value = oCotizacion.Identity;
                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oCotizacion.EstacionOrigen.Numero;
                oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oCotizacion.FechaInicialVigencia;

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
                        msg = Traduccion.Traducir("No existe el registro de la Cotización");
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
        /// Elimina la cabecera de la definicion de la cotizacion de la base de datos
        /// </summary>
        /// <param name="oCotizacion">Cotizacion - Objeto con la informacion de la Cotizacion a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCotizacionCabecera(Cotizacion oCotizacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Cotizaciones_delCotizacionCabecera";
                oCmd.Parameters.Add("@IdentCotiza", SqlDbType.Int).Value = oCotizacion.Identity;
                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oCotizacion.EstacionOrigen.Numero;
                oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oCotizacion.FechaInicialVigencia;

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
                        msg = Traduccion.Traducir("No existe el registro de la Cotizacion");
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


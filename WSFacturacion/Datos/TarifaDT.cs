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
    /// Metodos para administracion de las Tarifas definidas en el sistema 
    /// </summary>
    ///****************************************************************************************************
    public static class TarifaDt
    {
        #region TARIFA_DIFERENCIADA: Clase de Datos de Tarifas Diferenciadas definidas
        
       
        
     ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de las Tarifas definidas en el sistema 
    /// </summary>
    ///****************************************************************************************************

        public static decimal getTarifa(Conexion oConn, int estacion, DateTime fecha, int categoria, int titari, string tipdh)
        {
            decimal tarifa = 0;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_GetTarifa";            
            oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;
            oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = fecha;
            oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = titari;
            oCmd.Parameters.Add("@tipdh", SqlDbType.Char, 4).Value = tipdh;
            
            SqlParameter parNumero = oCmd.Parameters.Add("@tarifa", SqlDbType.SmallMoney);
            parNumero.Direction = ParameterDirection.Output;

            oCmd.ExecuteNonQuery();
            if (parNumero.Value != DBNull.Value)
            {
                tarifa = Convert.ToDecimal(parNumero.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            return tarifa;
        }
        
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tarifas diferenciadas definidas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoTarifa">int - Codigo de tarifa diferenciada por la cual filtrar la busqueda</param>
        /// <returns>Lista de Categorias manuales definidas</returns>
        /// ***********************************************************************************************
        public static TarifaDiferenciadaL getTarifasDiferenciadas(Conexion oConn, int? codigoTarifa)
        {
            TarifaDiferenciadaL oTarifasDiferenciadas = new TarifaDiferenciadaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TarifasDiferenciadas_getTarifasDiferenciadas";
            oCmd.Parameters.Add("Titar", SqlDbType.TinyInt).Value = codigoTarifa;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTarifasDiferenciadas.Add(CargarTarifaDiferenciada(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTarifasDiferenciadas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de tarifas diferenciadas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tarifas Diferenciadas</param>
        /// <returns>Lista con los elementos TarifaDiferenciada de la base de datos</returns>
        /// ***********************************************************************************************
        private static TarifaDiferenciada CargarTarifaDiferenciada(System.Data.IDataReader oDR)
        {
            TarifaDiferenciada oTarifaDiferenciada = new TarifaDiferenciada();
            oTarifaDiferenciada.CodigoTarifa = (byte) oDR["tit_titar"];
            oTarifaDiferenciada.Descripcion = oDR["tit_descr"].ToString();
            oTarifaDiferenciada.Porcentaje = (float) oDR["tit_porce"];

            return oTarifaDiferenciada;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una tarifa diferenciada en la base de datos
        /// </summary>
        /// <param name="oTarifaDiferenciada">TarifaDiferenciada - Objeto con la informacion de la Tarifa Diferenciada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TarifasDiferenciadas_addTarifaDiferenciada";

            oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = oTarifaDiferenciada.CodigoTarifa;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oTarifaDiferenciada.Descripcion;
            oCmd.Parameters.Add("@Porce", SqlDbType.Real).Value = oTarifaDiferenciada.Porcentaje;

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
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("La Tarifa Diferenciada ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este Código de Tarifa Diferenciada fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza una tarifa diferenciada en la base de datos
        /// </summary>
        /// <param name="oTarifa">TarifaDiferenciada - Objeto con la informacion de la Tarifa Diferenciada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updTarifaCabecera(Tarifa oTarifa, Conexion oConn)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_updTarifaCabecera";
            oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = oTarifa.Identity;
            oCmd.Parameters.Add("@piva", SqlDbType.SmallMoney).Value = oTarifa.PorcentajeIva;

            oCmd.Parameters.Add("@FecIn", SqlDbType.DateTime).Value = oTarifa.FechaInicialVigencia;
            oCmd.Parameters.Add("@Fec", SqlDbType.DateTime).Value = oTarifa.FechaInicialVigencia;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Modificar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Tarifa");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una tarifa diferenciada en la base de datos
        /// </summary>
        /// <param name="oTarifaDiferenciada">TarifaDiferenciada - Objeto con la informacion de la Tarifa Diferenciada</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TarifasDiferenciadas_updTarifaDiferenciada";

            oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = oTarifaDiferenciada.CodigoTarifa;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oTarifaDiferenciada.Descripcion;
            oCmd.Parameters.Add("@Porce", SqlDbType.Real).Value = oTarifaDiferenciada.Porcentaje;
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Tarifa Diferenciada");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Tarifa Diferenciada de la base de datos
        /// </summary>
        /// <param name="oTarifaDiferenciada">TarifaDiferenciada - Objeto de la Tarifa Diferenciada a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTarifaDiferenciada(TarifaDiferenciada oTarifaDiferenciada, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_TarifasDiferenciadas_delTarifaDiferenciada";

                oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = oTarifaDiferenciada.CodigoTarifa;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Tarifa Diferenciada");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Tipo de Tarifa no se puede dar de baja porque está siendo utilizado"));
                throw;
            }
        }
        
        #endregion

        #region TARIFA: Clase de Datos de Tarifas y cambios de tarifas definidas

        public static DataSet getRptTarifasDetalle(Conexion oConn)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptPeaje_CambiosTarifasDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_getRptTarifas";
            oCmd.Parameters.Add("@IdentTarifa", SqlDbType.Int).Value = null;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsExentos, "Tarifas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsExentos;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de cambios de tarifas realizadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Estacion">Estacion - Estacion a la que pertenece la tarifa</param>
        /// <param name="FechaInicialVigencia">Datetime - Fecha de inicio de busqueda de las tarifas</param>
        /// <returns>Lista de Cambios de Tarifas realizadas</returns>
        /// ***********************************************************************************************
        public static TarifaL getTarifasCabecera(Conexion oConn, int? codigoEstacion, DateTime? FechaInicialVigencia, int? identity)
        {
            TarifaL oCambiosTarifa = new TarifaL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_getTarifasCabecera";
            oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = codigoEstacion;
            oCmd.Parameters.Add("FecIn", SqlDbType.DateTime).Value = FechaInicialVigencia;
            oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = identity;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oCambiosTarifa.Add(CargarTarifasCabecera(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oCambiosTarifa;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de cambios de tarifas 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tarifas</param>
        /// <returns>Lista con los elementos Tarifa de la base de datos</returns>
        /// ***********************************************************************************************
        private static Tarifa CargarTarifasCabecera(System.Data.IDataReader oDR)
        {
            Tarifa oTarifa = new Tarifa();

            oTarifa.Identity = (int)oDR["tar_ident"];
            oTarifa.Estacion = new Estacion((byte)oDR["tar_estac"], oDR["est_nombr"].ToString());
            oTarifa.FechaInicialVigencia = (DateTime)oDR["tar_fecha"];
            oTarifa.FechaFinalVigencia = Util.DbValueToNullable<DateTime>(oDR["tar_fecti"]);

            if (oDR["tar_IVA"] != DBNull.Value)
            {
                oTarifa.PorcentajeIva = Convert.ToDouble(oDR["tar_IVA"]);
            }
            return oTarifa;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todas las tarifas de un cambio de tarifa determinado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="identity">int - Identity de un determinado cambio de tarifa</param>
        /// <returns>Lista de valor de las tarifas de un determinado cambio de tarifas</returns>
        /// ***********************************************************************************************
        public static TarifaDetalleL getTarifasDetalle(Conexion oConn, int identity)
        {
            TarifaDetalleL oTarifaDetalleL = new TarifaDetalleL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_getTarifasDetalle";
            oCmd.Parameters.Add("IdentTarifa", SqlDbType.Int).Value = identity;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTarifaDetalleL.Add(CargarTarifasDetalle(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTarifaDetalleL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de tarifas definidas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tarifas</param>
        /// <returns>Lista con los elementos TarifaDetalle de la base de datos</returns>
        /// ***********************************************************************************************
        private static TarifaDetalle CargarTarifasDetalle(System.Data.IDataReader oDR)
        {
            TarifaDetalle oTarifaDetalle = new TarifaDetalle();

            oTarifaDetalle.Categoria = new CategoriaManual((byte)oDR["tar_tarif"], "");
            oTarifaDetalle.TarifaDiferenciada = new TarifaDiferenciada((byte)oDR["tar_titar"], "");
            oTarifaDetalle.TipoDia = new TipoDiaHora(oDR["tar_tipdh"].ToString(), "", "");
            oTarifaDetalle.ValorTarifa = (decimal)oDR["tar_valor"];

            if (oDR["tar_valorRed"] is DBNull)
            {
                oTarifaDetalle.ValorTarifaRed = 0;
            }
            else
            {
                oTarifaDetalle.ValorTarifaRed = (decimal)oDR["tar_valorRed"];
            }
            return oTarifaDetalle;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todas las combinaciones de tarifas posibles (categoria, tipo de tarifa y tipo de dia) y sus habilitaciones 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de valor de las tarifas de un determinado cambio de tarifas</returns>
        /// ***********************************************************************************************
        public static TarifaDetalleL getHabilitacionesTarifasDetalle(Conexion oConn)
        {
            TarifaDetalleL oTarifaDetalleL = new TarifaDetalleL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_getHabilitacionTarifas";

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTarifaDetalleL.Add(CargarHabilitacionTarifasDetalle(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTarifaDetalleL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de habilitacion de tarifas 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tarifas cn su habilitacion</param>
        /// <returns>Lista con los elementos TarifaDetalle de la base de datos</returns>
        /// ***********************************************************************************************
        private static TarifaDetalle CargarHabilitacionTarifasDetalle(System.Data.IDataReader oDR)
        {
            TarifaDetalle oTarifaDetalle = new TarifaDetalle();

            oTarifaDetalle.Categoria = new CategoriaManual((byte)oDR["Categoria"], "");
            oTarifaDetalle.TarifaDiferenciada = new TarifaDiferenciada((byte)oDR["TipoTarifa"], "");
            oTarifaDetalle.TipoDia = new TipoDiaHora(oDR["TipoDia"].ToString(), "", "");
            oTarifaDetalle.Habilitado = (oDR["Habilitado"].ToString()=="S") ? true : false;
            oTarifaDetalle.ValorTarifa = 0;

            return oTarifaDetalle;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la cabecera de un cambio de tarifa en la base de datos
        /// </summary>
        /// <param name="oTarifa">Tarifa - Objeto con la informacion del cambio de tarifa realizado</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTarifaCabecera(Tarifa oTarifa, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_addTarifaCabecera";

            oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = oTarifa.Estacion.Numero;
            oCmd.Parameters.Add("@piva", SqlDbType.SmallMoney).Value = oTarifa.PorcentajeIva;

            SqlParameter parFecIn = oCmd.Parameters.Add("@FecIn", SqlDbType.DateTime);
            parFecIn.Direction = ParameterDirection.InputOutput;
            parFecIn.Value = oTarifa.FechaInicialVigencia;

            // Valor identity insertado en la cabecera
            SqlParameter paramIdent = oCmd.Parameters.Add("@Ident", SqlDbType.Int);
            paramIdent.Direction = ParameterDirection.Output;

                
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();

                
            int retval = (int)parRetVal.Value;
            oTarifa.FechaInicialVigencia = (DateTime)parFecIn.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("La Tarifa definida ya existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                oTarifa.Identity = (int)paramIdent.Value;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega el detalle de las tarifas definidas en la base de datos
        /// </summary>
        /// <param name="IdentTarifa">int - Identity de la cabecera</param>
        /// <param name="oTarifaDetalle">TarifaDetalle - Objeto con la informacion de la tarifa detallada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTarifaDetalle(int IdentTarifa, TarifaDetalle oTarifaDetalle, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.CommandText = "Peaje.usp_Tarifas_addTarifaDetalle";
            oCmd.Parameters.Add("@IdentTarifa", SqlDbType.Int).Value = IdentTarifa;
            oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oTarifaDetalle.Categoria.Categoria;
            oCmd.Parameters.Add("@Titar", SqlDbType.TinyInt).Value = oTarifaDetalle.TarifaDiferenciada.CodigoTarifa;
            oCmd.Parameters.Add("@TipoDia", SqlDbType.Char, 4).Value = oTarifaDetalle.TipoDia.Codigo;
            oCmd.Parameters.Add("@Valor", SqlDbType.SmallMoney).Value = oTarifaDetalle.ValorTarifa;
            oCmd.Parameters.Add("@ValorRedo", SqlDbType.SmallMoney).Value = oTarifaDetalle.ValorTarifaRed;
                
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

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina el detalle de las Tarifas de la base de datos
        /// </summary>
        /// <param name="oTarifa">Tarifa - Objeto con la informacion de las tarifas detalle a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTarifaDetalle(Tarifa oTarifa, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_delTarifaDetalle";
            oCmd.Parameters.Add("@IdentTarifa", SqlDbType.Int).Value = oTarifa.Identity;
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oTarifa.Estacion.Numero;
            oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oTarifa.FechaInicialVigencia;

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
                {
                    msg = Traduccion.Traducir("No existe el registro de la Tarifa");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina la cabecera del Cambio de Tarifa de la base de datos
        /// </summary>
        /// <param name="oTarifa">Tarifa - Objeto con la informacion de la tarifa a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTarifaCabecera(Tarifa oTarifa, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_delTarifaCabecera";
            oCmd.Parameters.Add("@IdentTarifa", SqlDbType.Int).Value = oTarifa.Identity;
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oTarifa.Estacion.Numero;
            oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oTarifa.FechaInicialVigencia;

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
                {
                    msg = Traduccion.Traducir("No existe el registro de la Tarifa");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        #endregion
                
        #region AUTORIZACION_PASO: Clase de Datos de Autorizaciones de Paso definidas
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Autorizaciones de Paso definidas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="categoria">byte? - Numero de categoria por la cual filtrar. Pueden ser todas (null)</param>
        /// <param name="estacionOrigen">int? - Estacion de origen a filtrar. Pueden ser todas (null)</param>
        /// <param name="sentidoOrigen">string - Sentido de origen a filtrar. Pueden ser todas (null)</param>
        /// <param name="estacionDestino">int? - Estacion de destino a filtrar. Pueden ser todas (null)</param>
        /// <param name="sentidoSentido">string - Sentido de destino a filtrar. Pueden ser todas (null)</param>
        /// <returns>Lista de Autorizaciones de paso definidas</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoL getAutorizacionesPaso(Conexion oConn, int? estacionOrigen, string sentidoOrigen, byte? categoria, int? estacionDestino, string sentidoDestino)
        {
            TarifaAutorizacionPasoL oTarifaAutorizacionPasoL = new TarifaAutorizacionPasoL();
            SqlDataReader oDR;
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AutorizacionesPaso_getAutorizacionesPaso";
            oCmd.Parameters.Add("EstOrigen", SqlDbType.TinyInt).Value = estacionOrigen;
            oCmd.Parameters.Add("SentOrigen", SqlDbType.Char,1).Value = sentidoOrigen;
            oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = categoria;
            oCmd.Parameters.Add("EstDestino", SqlDbType.TinyInt).Value = estacionDestino;
            oCmd.Parameters.Add("SentDestino", SqlDbType.Char, 1).Value = sentidoDestino;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTarifaAutorizacionPasoL.Add(CargarAutorizacionPaso(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTarifaAutorizacionPasoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de autorizaciones de paso
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Autorizaciones de Paso</param>
        /// <returns>Lista con los elementos TarifaAutorizacionPaso de la base de datos</returns>
        /// ***********************************************************************************************
        private static TarifaAutorizacionPaso CargarAutorizacionPaso(System.Data.IDataReader oDR)
        {
            TarifaAutorizacionPaso oTarifaAutorizacionPaso = new TarifaAutorizacionPaso();

            oTarifaAutorizacionPaso.EstacionOrigen = new Estacion((byte)oDR["tau_estor"], oDR["EstacionOrigen"].ToString());
            oTarifaAutorizacionPaso.SentidoOrigen = new ViaSentidoCirculacion { Codigo = Convert.ToString(oDR["tau_senor"]) };
            oTarifaAutorizacionPaso.Categoria = new CategoriaManual((byte)oDR["tau_tarif"], "");
            oTarifaAutorizacionPaso.EstacionDestino = new Estacion((byte)oDR["tau_estde"], oDR["EstacionDestino"].ToString());
            oTarifaAutorizacionPaso.SentidoDestino = new ViaSentidoCirculacion { Codigo = Convert.ToString(oDR["tau_sende"].ToString()) };
            oTarifaAutorizacionPaso.FormaDescuento =  CargaAutorizacionPasoFormaDescuento(oDR["tau_forde"].ToString());
            
            oTarifaAutorizacionPaso.MinutosVigencia = (short)oDR["tau_lapso"];
            // Numero de pasada NO se usa en nadie mas que AUSOL. Lo dejamos sin implementar.

            // El tipo de tarifa puede ser NULL si se elige la forma "Descuento"
            if (oDR["tau_titar"] == DBNull.Value)
            {
                oTarifaAutorizacionPaso.TipoTarifa = null;
            }
            else
            {
                oTarifaAutorizacionPaso.TipoTarifa = new TarifaDiferenciada((byte)oDR["tau_titar"], oDR["TipoTarifa"].ToString());
            }
            return oTarifaAutorizacionPaso;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Autorizacion de paso
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Un elemento de autorizacion de paso a grabar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AutorizacionesPaso_addAutorizacionPaso";

            oCmd.Parameters.Add("EstOrigen", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionOrigen.Numero;
            oCmd.Parameters.Add("SentOrigen", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoOrigen.Codigo;
            oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.Categoria.Categoria;
            oCmd.Parameters.Add("EstDestino", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionDestino.Numero;
            oCmd.Parameters.Add("SentDestino", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoDestino.Codigo;
            oCmd.Parameters.Add("FormaDesc", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.FormaDescuento.Codigo;
            oCmd.Parameters.Add("Lapso", SqlDbType.SmallInt).Value = oTarifaAutorizacionPaso.MinutosVigencia;
            if (oTarifaAutorizacionPaso.TipoTarifa != null)
            {
                oCmd.Parameters.Add("TipoTarifa", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.TipoTarifa.CodigoTarifa;
            }

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
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("La combinación de la Autorización de Paso ya existe");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una autorizacion de paso en la base de datos
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Un elemento de autorizacion de paso a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AutorizacionesPaso_updAutorizacionPaso";

            oCmd.Parameters.Add("EstOrigen", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionOrigen.Numero;
            oCmd.Parameters.Add("SentOrigen", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoOrigen.Codigo;
            oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.Categoria.Categoria;
            oCmd.Parameters.Add("EstDestino", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionDestino.Numero;
            oCmd.Parameters.Add("SentDestino", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoDestino.Codigo;
            oCmd.Parameters.Add("FormaDesc", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.FormaDescuento.Codigo;
            oCmd.Parameters.Add("Lapso", SqlDbType.SmallInt).Value = oTarifaAutorizacionPaso.MinutosVigencia;
            if (oTarifaAutorizacionPaso.TipoTarifa != null)
                oCmd.Parameters.Add("TipoTarifa", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.TipoTarifa.CodigoTarifa;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Autorización de Paso");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Autorizacion de Paso de la base de datos
        /// </summary>
        /// <param name="oTarifaAutorizacionPaso">TarifaAutorizacionPaso - Un elemento de autorizacion de paso a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delAutorizacionPaso(TarifaAutorizacionPaso oTarifaAutorizacionPaso, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AutorizacionesPaso_delAutorizacionPaso";

            oCmd.Parameters.Add("EstOrigen", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionOrigen.Numero;
            oCmd.Parameters.Add("SentOrigen", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoOrigen.Codigo;
            oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.Categoria.Categoria;
            oCmd.Parameters.Add("EstDestino", SqlDbType.TinyInt).Value = oTarifaAutorizacionPaso.EstacionDestino.Numero;
            oCmd.Parameters.Add("SentDestino", SqlDbType.Char, 1).Value = oTarifaAutorizacionPaso.SentidoDestino.Codigo;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Autorización de Paso");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
        
        #region TARIFAAUTORIZACIONPASOFORMADESCUENTO: Las posibles formas de pago que se pueden configurar en la autorizacion de paso

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos TarifaAutorizacionPasoFormaDescuento (posibles formas de descuento de la autorizacion de paso)
        /// </summary>
        /// <returns>Lista de objetos TarifaAutorizacionPasoFormaDescuento</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoFormaDescuentoL getTarifasAutorizacionPasoFormaDescuento()
        {
            TarifaAutorizacionPasoFormaDescuentoL oTarifaAutorizacionPasoFormaDescuentoL = new TarifaAutorizacionPasoFormaDescuentoL();

            oTarifaAutorizacionPasoFormaDescuentoL.Add(CargaAutorizacionPasoFormaDescuento("T"));
            oTarifaAutorizacionPasoFormaDescuentoL.Add(CargaAutorizacionPasoFormaDescuento("D"));

            return oTarifaAutorizacionPasoFormaDescuentoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de sentido de circulacion
        /// </summary>
        /// <returns>Objeto ViaSentidoCirculacion con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        public static TarifaAutorizacionPasoFormaDescuento CargaAutorizacionPasoFormaDescuento(string codigo)
        {
            string descripcion = "";

            switch (codigo)
            {
                case "T":
                    descripcion = Traduccion.Traducir("Tipo de Tarifa");
                    break;

                case "D":
                    descripcion = Traduccion.Traducir("Descuento");
                    break;

                default:
                    descripcion = "";
                    break;
            }

            return new TarifaAutorizacionPasoFormaDescuento(codigo, descripcion);
        }

        #endregion
    }
}

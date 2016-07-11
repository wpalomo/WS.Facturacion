using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Tesoreria
{
    public class ApropiacionDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Inserta la cabecera de una apropiacion
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oApropiacion">Apropiacion - Objeto de la cabecera de la apropiacion</param>
        /// <returns>El resultado de la accion, si funciono o dio error</returns>
        /// ***********************************************************************************************
        public static bool addApropiacion(Conexion oConn, Apropiacion oApropiacion)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_addApropiacion";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oApropiacion.Estacion.Numero;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oApropiacion.Usuario.ID;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oApropiacion.Fecha;
                oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = oApropiacion.Bolsa;
                oCmd.Parameters.Add("@total", SqlDbType.Money).Value = oApropiacion.Total;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = oApropiacion.Jornada;
                oCmd.Parameters.Add("@testu", SqlDbType.TinyInt).Value = oApropiacion.Turno;
                oCmd.Parameters.Add("@cons", SqlDbType.Int).Value = oApropiacion.ConsignacionInterna;
                oCmd.Parameters.Add("@tipapr", SqlDbType.Char, 1).Value = oApropiacion.TipoApropiacion;

                SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parNumero.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (parNumero.Value != DBNull.Value)
                {
                    oApropiacion.NumeroApropiacion = Convert.ToInt32(parNumero.Value);
                }
                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Error en los parametros");
                    }

                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega el detalle de una apropiacion
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oApropiacionDetalle">ApropiacionDetalle - El registro de la apropiacion de bolsa (detalle)</param>
        /// <returns>El resultado de la accion, si funciono o dio error</returns>
        /// ***********************************************************************************************
        public static bool addApropiacionDetalle(Conexion oConn, Estacion oEstacion, int numeroApropiacion, BolsaApropiacion oApropiacionDetalle)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_addApropiacionDetalle";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oEstacion.Numero;
                oCmd.Parameters.Add("@idapr", SqlDbType.Int).Value = numeroApropiacion;
                oCmd.Parameters.Add("@idmoc", SqlDbType.Int).Value = oApropiacionDetalle.NumeroMovimientoCaja;

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
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Error en los parametros");
                    }

                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega el detalle de una apropiacion
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oApropiacionDetalle">ApropiacionDetalle - El registro de la apropiacion de bolsa (detalle)</param>
        /// <returns>El resultado de la accion, si funciono o dio error</returns>
        /// ***********************************************************************************************
        public static bool addApropiacionDetalle(Conexion oConn, MovimientoCajaDetalle oDetalle, Apropiacion oApropiacion)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_addApropiacionDetalles";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oApropiacion.Estacion.Numero;
                oCmd.Parameters.Add("@apr", SqlDbType.Int).Value = oApropiacion.NumeroApropiacion;
                oCmd.Parameters.Add("@denom", SqlDbType.SmallInt).Value = oDetalle.Denominacion.CodDenominacion;
                oCmd.Parameters.Add("@moned", SqlDbType.SmallInt).Value = oDetalle.Moneda.Codigo;
                oCmd.Parameters.Add("@canti", SqlDbType.Int).Value = oDetalle.Cantidad;

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
                    {
                        msg = Traduccion.Traducir("Apropriacion no es valida");
                    }
                    else if (retval == -104)
                    {
                        msg = Traduccion.Traducir("Apropriacion ya está");
                    }
                    else if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Datos no son validos");
                    }
                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue apropiadao
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos </param>
        /// <param name="oMovimiento">MovimientoCaja- Liquidacion, Retiro o Reposicion a Anular</param>
        /// <returns>El resultado de la accion, si funciono o dio error</returns>
        /// ***********************************************************************************************
        public static bool getBolsaApropiada(Conexion oConn, MovimientoCaja oMovimiento)
        {
            return getBolsaApropiada(oConn, oMovimiento.Parte, oMovimiento.NumeroMovimiento);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el movimiento ya fue Apropiado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oParte">Parte - Numero de parte en el que se genero el movimiento</param>
        /// <param name="numeroMovimiento">int? - Numero de movimiento de mocaja</param>
        /// ***********************************************************************************************
        public static bool getBolsaApropiada(Conexion oConn, Parte oParte, int? numeroMovimiento)
        {
            bool ret = false;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_getBolsaApropiada";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oParte.Numero;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroMovimiento;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = true;
                }
                
                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una apropiacion de bolsa completa, cabecera y detalle. Nunca se necesita solo el detalla, 
        /// porque cuando se elimina un movimiento es 1 cabecera - 1 detalle, y cuando es desde la pantalla de 
        /// apropiacion tambien se elimina completa
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int - Numero de estacion</param>
        /// <param name="numeroApropiacion">int - Numero de la apropiacion a eliminar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delApropiacionBolsa(Conexion oConn, int? estacion, int numeroApropiacion)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_delApropiacion";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numeroApropiacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de apropiaciones de la base de datos
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Desde"></param>
        /// <param name="Hasta"></param>
        /// <param name="NumeroBolsa"></param>
        /// <param name="Estacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ApropiacionL getApropiaciones(Conexion oConn, DateTime? Desde, DateTime? Hasta, int? NumeroBolsa, int? Estacion)
        {
            ApropiacionL oApropiaciones = new ApropiacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getApropiaciones";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;
                oCmd.Parameters.Add("@Desde", SqlDbType.DateTime).Value = Desde;
                oCmd.Parameters.Add("@Hasta", SqlDbType.DateTime).Value = Hasta;
                oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = NumeroBolsa;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oApropiaciones.Add(CargarApropiacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oApropiaciones;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader con los datos de un detalle de la Apropiacion
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Detalle de Apropiacion</param>
        /// <returns>elemento Detalle de Apropiacion</returns>
        /// ***********************************************************************************************
        private static MovimientoCajaDetalle CargarDetalleApropiacion(IDataReader oDR)
        {
            MovimientoCajaDetalle oDetalle = new MovimientoCajaDetalle();

            oDetalle.Moneda = new Moneda((short)oDR["mon_moned"], oDR["mon_descr"].ToString(), oDR["mon_simbo"].ToString(), "N");
            oDetalle.Denominacion = new Denominacion(oDetalle.Moneda, (short)oDR["den_denom"], oDR["den_descr"].ToString(), Convert.ToDecimal(oDR["den_valor"]));
            oDetalle.Cotizacion = (decimal)oDR["cot_cotiz"];
            if (oDR["apr_canti"] == DBNull.Value)
            {
                oDetalle.Cantidad = 0;
            }
            else
            {
                oDetalle.Cantidad = (int)oDR["apr_canti"];
            }
            return oDetalle;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos del Detalle de la Apropiacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>MovimientoCajaDetalleL</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaDetalleL getApropiacionDetalleDt(Conexion oConn, int EstNumero, int? NumeroApropiacion)
        {
            MovimientoCajaDetalleL oApropiacionDt = new MovimientoCajaDetalleL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getDetalle";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = EstNumero;
                oCmd.Parameters.Add("@apr", SqlDbType.Int).Value = NumeroApropiacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oApropiacionDt.Add(CargarDetalleApropiacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oApropiacionDt;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static Apropiacion CargarApropiacion(System.Data.IDataReader oDR)
        {
            // Preparamos los datos del turno
            TurnoTrabajo oTurnoTrabajo =  new TurnoTrabajo();
            oTurnoTrabajo.NumeroTurno = (Byte)oDR["apr_testu"];
            oTurnoTrabajo.Hora = Convert.ToDateTime(oDR["tes_horai"]);
            oTurnoTrabajo.HoraFinal = Convert.ToDateTime(oDR["tes_horaf"]);

            Apropiacion oApropiacion = new Apropiacion();
            oApropiacion.Bolsa = Util.DbValueToNullable<int>(oDR["apr_bolsa"]);
            oApropiacion.Estacion = new Estacion((Byte)oDR["apr_coest"], oDR["est_nombr"].ToString());
            oApropiacion.Fecha = (DateTime)oDR["apr_fecha"];
            oApropiacion.Jornada = (DateTime)oDR["apr_fejor"];
            oApropiacion.NumeroApropiacion = (int)oDR["apr_numer"];
            oApropiacion.Total = (decimal)oDR["apr_total"];
            oApropiacion.Turno = (Byte)oDR["apr_testu"];
            oApropiacion.Usuario = new Usuario(oDR["apr_id"].ToString(), oDR["use_nombr"].ToString());
            oApropiacion.Depositado = oDR["Depositado"].ToString();
            oApropiacion.Recontado = oDR["Recontado"].ToString();
            oApropiacion.TurnoTrabajo = oTurnoTrabajo;
            if (oDR["apr_cons"] != DBNull.Value)
            {
                oApropiacion.ConsignacionInterna = (int)oDR["apr_cons"];
            }

            return oApropiacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto del tipo BolsaApropiacion con los datos traidos de la base de dato
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static BolsaApropiacion CargarBolsaSinApropiacion(IDataReader oDR)
        {
            BolsaApropiacion oBolsaApropiacion = new BolsaApropiacion();

            oBolsaApropiacion.Bolsa = Util.DbValueToNullable<int>(oDR["Bolsa"]);
            oBolsaApropiacion.Estacion = new Estacion((Byte)oDR["est_codig"], oDR["est_nombr"].ToString());
            oBolsaApropiacion.Jornada = (DateTime)oDR["par_fejor"];
            oBolsaApropiacion.MontoEquivalente = (decimal)oDR["MontoEquivalente"];
            oBolsaApropiacion.MovimientoDescripcion = (string)oDR["MovimientoDescr"];
            oBolsaApropiacion.NumeroMovimientoCaja = (int)oDR["NumeroMovimiento"];
            oBolsaApropiacion.Parte = Util.DbValueToNullable<int>(oDR["Parte"]);
            oBolsaApropiacion.Turno = (Byte)oDR["par_testu"];
            
            return oBolsaApropiacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de bolsas sin apropiación
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Estacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static BolsaApropiacionL GetBolsasSinApropiar(Conexion oConn, int? Estacion)
        {
            BolsaApropiacionL oBolsasSinApropiacion = new BolsaApropiacionL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getBolsasNoApropiadas";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                    oBolsasSinApropiacion.Add(CargarBolsaSinApropiacion(oDR));

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oBolsasSinApropiacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza la Bolsa de una apropiacion
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oApropiacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool updApropiacion(Conexion oConn, Apropiacion oApropiacion)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_updApropiacion";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oApropiacion.Estacion.Numero;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oApropiacion.NumeroApropiacion ;
                oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = oApropiacion.Bolsa;

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
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Error en los parametros");
                    }

                    throw new ErrorSPException(msg);
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Propone como Bolsa el numero siguiente al anterior usado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int - Numero de estacion</param>
        /// <returns>ultimo numero de bolsa + 1</returns>
        /// ***********************************************************************************************
        public static int getProximoNumeroBolsa(Conexion oConn, int estacion)
        {
            int ret = 1;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getProximoNUmeroBolsa";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                
                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = (int)oDR["ProximaBolsa"];
                }
                
                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Propone como Consignacion el numero siguiente al anterior usado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intTurno">int - Numero de Turno</param>
        /// <param name="dtJornada">DateTime - Jornada</param>
        /// <returns>ultimo numero de consignacion + 1</returns>
        /// ***********************************************************************************************
        public static int getProximoNumeroConsignacion(Conexion oConn, int intTurno, DateTime dtJornada)
        {
            int ret = 1;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getProximoNumeroConsignacion";
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = dtJornada;
                oCmd.Parameters.Add("@testu", SqlDbType.TinyInt).Value = intTurno;
                
                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = (int)oDR["ProximoNumero"];
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el numero de deposito ya existe
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intDeposito">Deposito - Numero de deposito a utilizar</param>        
        /// ***********************************************************************************************
        public static bool getExisteNumeroDeposito(Conexion oConn, int intBolsa)
        {
            bool ret = false;
            try
            {
                SqlDataReader oDR;

                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getExisteNumeroBolsa";
                oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = intBolsa;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = true;
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Ver si el numero de consignacion ya fue utilizado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intConsignacion">Consignacion - Numero de consignacion a utilizar</param>        
        /// ***********************************************************************************************
        public static bool getExisteNumeroConsignacion(Conexion oConn,int intTurno,DateTime dtJornada, int intConsignacion)
        {
            bool ret = false;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Apropiacion_getExisteNumeroConsignacion";
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = dtJornada;
                oCmd.Parameters.Add("@testu", SqlDbType.TinyInt).Value = intTurno;
                oCmd.Parameters.Add("@cons", SqlDbType.TinyInt).Value = intConsignacion;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    ret = true;
                }
                
                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ret;
        }
    }
}

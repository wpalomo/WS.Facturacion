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
    public class VISADt
    {

        ///***********************************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="VS"></param>
        /// <param name="SecuenciaInsertada"></param>
        ///***********************************************************************************************
        public static void GrabarCabeceraVisa(Conexion oConn, VISA VS, out int SecuenciaInsertada)
        {
            try
            {
                SecuenciaInsertada = -1;
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_addSecenvvc";

                oCmd.Parameters.Add("@archivo", SqlDbType.VarChar, 255).Value = VS.NombreArchivo;
                oCmd.Parameters.Add("@Fecha", SqlDbType.Date).Value = VS.FechaProcesamiento.Date;
                oCmd.Parameters.Add("@FechaInicio", SqlDbType.Date).Value = VS.PeriodoInicial.Date;
                oCmd.Parameters.Add("@FechaFinalizacion", SqlDbType.Date).Value = VS.PeriodoFinal.Date;
                oCmd.Parameters.Add("@secuencia", SqlDbType.Int).Value = VS.Secuencia;
                oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 10).Value = VS.Usuario;

                SqlParameter NumeroInserted = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                NumeroInserted.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (NumeroInserted.Value != DBNull.Value)
                {
                    SecuenciaInsertada = Convert.ToInt32(NumeroInserted.Value);
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

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        ///***********************************************************************************************
        /// <summary>
        /// Verifico si el archivo RET de VISA ya se encontraba procesado
        /// </summary>
        /// <param name="conn">Conexion a consolidado</param>
        /// <param name="Archivo">Nombre del archivo</param>
        /// <returns></returns>
        ///***********************************************************************************************
        public static bool EsArchivoYaProcesado(Conexion oConn, string Archivo)
        {
            bool retorno = true;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_EsArchivoVisaProcesado";
                oCmd.Parameters.Add("@NombreArchivo", SqlDbType.VarChar, 255).Value = Archivo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    if (retval == -101)
                    {
                        string msg = Traduccion.Traducir("Error de Parametrización ") + retval.ToString();
                        throw new ErrorSPException(msg);
                    }
                    else if (retval == -102)
                    {
                        retorno = false;    // El archivo ya existe
                    }

                }

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        ///***********************************************************************************************
        /// <summary>
        /// Grabar pagos Visa Cash
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="VS"></param>
        /// <param name="SecuenciaInsertada"></param>
        ///***********************************************************************************************
        public static void GrabarPagosVC(Conexion oConn, VISA VS, int? SecuenciaInsertada,ref bool procesarLote)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_addPagosVC";

                oCmd.Parameters.Add("@NumeroSecuencia", SqlDbType.Int).Value = SecuenciaInsertada;
                oCmd.Parameters.Add("@EstablecimientoVISA", SqlDbType.VarChar,10).Value = VS.EstabVisa;
                oCmd.Parameters.Add("@NumeroRO", SqlDbType.Int).Value = VS.NumeroRO;
                oCmd.Parameters.Add("@FechaVenta", SqlDbType.Date).Value =  VS.FechaDeposito.Date;
                oCmd.Parameters.Add("@FechaPago", SqlDbType.Date).Value = VS.FechaPago.Date;
                oCmd.Parameters.Add("@FechaEnvioBanco", SqlDbType.Date).Value = (VS.FechaEnvioBanco != null ? Convert.ToDateTime(VS.FechaEnvioBanco).Date : VS.FechaEnvioBanco);
                oCmd.Parameters.Add("@ValorBruto", SqlDbType.Money).Value = VS.ValorBruto;
                oCmd.Parameters.Add("@ValorComision", SqlDbType.Money).Value = VS.ValorComision;
                oCmd.Parameters.Add("@ValorRechazado", SqlDbType.Money).Value = VS.ValorRechazado;
                oCmd.Parameters.Add("@ValorNeto", SqlDbType.Money).Value = VS.ValorNeto;
                oCmd.Parameters.Add("@TotalAceptado", SqlDbType.Int).Value = VS.CantCVAcetpadas;
                oCmd.Parameters.Add("@TotalRechazado", SqlDbType.Int).Value = VS.CantCVRechazadas;
                oCmd.Parameters.Add("@Status", SqlDbType.Int).Value = VS.StatusVenta;

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
                    if (retval == -103)
                    {
                        msg = Traduccion.Traducir("Error falta archivo TXT");
                    }
                    if (retval == -104)
                    {
                       // msg = Traduccion.Traducir("Lote ya procesado.");
                        procesarLote = false;
                        return;
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public static void GrabarSecuenciaRO(Conexion oConn, VISA VS,DateTime? fechaCabecera,string nombreArchivo, DateTime FechaDesde, DateTime FechaHasta)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_grabarSecRO";
                oCmd.Parameters.Add("@numeroRo", SqlDbType.Int).Value = VS.NumeroRO;
                oCmd.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaCabecera;
                oCmd.Parameters.Add("@nombreArchivo", SqlDbType.VarChar, 255).Value = nombreArchivo;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = FechaDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = FechaHasta;

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
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        ///***********************************************************************************************
        /// <summary>
        /// Graba comprobantes de Venta
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="VS"></param>
        /// <param name="SecuenciaInsertada"></param>
        ///***********************************************************************************************
        public static void GrabarComprobanteVenta(Conexion oConn, VISA VS, int? SecuenciaInsertada)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_addComprobanteVenta";

                //if (SecuenciaInsertada == null)
                //{
                //    oCmd.Parameters.Add("@NumeroSecuencia", SqlDbType.Int).Value = 0;
                //}
                //oCmd.Parameters.Add("@NumeroSecuencia", SqlDbType.Int).Value = SecuenciaInsertada;
                oCmd.Parameters.Add("@EstablecimientoVISA", SqlDbType.VarChar, 10).Value = VS.EstabVisa;

                if (VS.FechaCompra != null)
                {
                    oCmd.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = Convert.ToDateTime(VS.FechaCompra).Date;    
                }
                
                oCmd.Parameters.Add("@NumeroTarjeta", SqlDbType.VarChar,20).Value = VS.NumeroCartao;
                oCmd.Parameters.Add("@EstadoOperacion", SqlDbType.VarChar,3).Value = VS.EstadoOperacion;
                oCmd.Parameters.Add("@Monto", SqlDbType.Money).Value = VS.Monto;
                oCmd.Parameters.Add("@MotivoRechazo", SqlDbType.VarChar,30).Value = VS.MotivoRechazo;
                oCmd.Parameters.Add("@NumeroRO", SqlDbType.Int).Value = VS.NumeroRO;
                oCmd.Parameters.Add("@FechaPinPad", SqlDbType.DateTime).Value = VS.FechaVisaPinPad;
                oCmd.Parameters.Add("@FeVen", SqlDbType.DateTime).Value = null;
                oCmd.Parameters.Add("@NumeroTransaccion", SqlDbType.Int).Value = VS.NumeroTran;
                oCmd.Parameters.Add("@NumeroPinPad", SqlDbType.Int).Value = Convert.ToInt32(VS.NumeroPinPad);
                oCmd.Parameters.Add("@NumeroConcentrador", SqlDbType.Int).Value = Convert.ToInt32(VS.NumeroConcentrador);


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
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("Error, No existe transito asociado");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }



        public static void UpdCabeceraVisa(Conexion oConn, int CantidadReg, int SecuenciaInsertada)
        {
            try
            {
                //SecuenciaInsertada = 3;
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_updSecenvvc";

                oCmd.Parameters.Add("@NumeroSecuencia", SqlDbType.Int).Value = SecuenciaInsertada;
                oCmd.Parameters.Add("@NumeroRegistros", SqlDbType.Int).Value = CantidadReg;

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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void updComprobanteVenta(Conexion oConn, VISA VS, int SecuenciaInsertada)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_VISA_updComprobanteVenta";
                oCmd.Parameters.Add("@NumeroSecuencia", SqlDbType.Int).Value = SecuenciaInsertada;
                
                if (VS.FechaCompra != null)
                {
                    oCmd.Parameters.Add("@FechaCompra", SqlDbType.Date).Value = Convert.ToDateTime(VS.FechaCompra).Date;
                }
                oCmd.Parameters.Add("@EstadoOperacion", SqlDbType.VarChar, 3).Value = VS.EstadoOperacion;
                oCmd.Parameters.Add("@MotivoRechazo", SqlDbType.VarChar, 30).Value = VS.MotivoRechazo;
                oCmd.Parameters.Add("@NumeroRO", SqlDbType.Int).Value = VS.NumeroRO;
                oCmd.Parameters.Add("@NumeroTransaccion", SqlDbType.Int).Value = VS.NumeroTran;
                
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
                    if (retval == -103)
                    {
                        msg = Traduccion.Traducir("Error Transaccion no encontrada.");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}


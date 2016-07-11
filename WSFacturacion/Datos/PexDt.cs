using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using System.Data;


namespace Telectronica.Peaje
{
    public static class PexDt
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="oConn"></param>
        public static void delTag(PexTag tag, Conexion oConn)
        {
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delTag";

                oCmd.Parameters.Add("@EmisorTag", SqlDbType.VarChar, 5).Value = tag.EmisorTag;
                oCmd.Parameters.Add("@NumeroTag", SqlDbType.VarChar, 10).Value = tag.NumeroTag;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = tag.Secuencia;

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
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="oConn"></param>
        /// <param name="completo"></param>
        /// ***********************************************************************************************

        public static void addTag(PexTag tag, Conexion oConn, char completo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addTag";

                oCmd.Parameters.Add("@Tipo", SqlDbType.Char).Value = tag.Tipo;
                oCmd.Parameters.Add("@PaisEmisor", SqlDbType.VarChar,4).Value = tag.PaisEmisor;
                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = tag.Administradora;
                oCmd.Parameters.Add("@EmisorTag", SqlDbType.VarChar,5).Value = tag.EmisorTag;
                oCmd.Parameters.Add("@NumeroTag", SqlDbType.VarChar,10).Value = tag.NumeroTag;
                oCmd.Parameters.Add("@Placa", SqlDbType.VarChar,7).Value = tag.Placa;
                oCmd.Parameters.Add("@Categoria", SqlDbType.TinyInt).Value = tag.Categoria;
                oCmd.Parameters.Add("@DiaPago", SqlDbType.TinyInt).Value = tag.DiaPago;
                oCmd.Parameters.Add("@MedioPago", SqlDbType.VarChar,2).Value = tag.MedioPago;
                oCmd.Parameters.Add("@FormaPago", SqlDbType.VarChar,3).Value = tag.FormaPago;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = tag.Secuencia;
                oCmd.Parameters.Add("@Completo", SqlDbType.Char).Value = completo;
                
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ln"></param>
        /// <param name="oConn"></param>
        public static void addListaNegra(PexListaNegra ln, Conexion oConn,char completo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addListaNegra";

                oCmd.Parameters.Add("@Tipo", SqlDbType.Char).Value = ln.Tipo;
                oCmd.Parameters.Add("@PaisEmisor", SqlDbType.VarChar, 4).Value = ln.PaisEmisor;
                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = ln.Administradora;
                oCmd.Parameters.Add("@EmisorTag", SqlDbType.VarChar, 5).Value = ln.EmisorTag;
                oCmd.Parameters.Add("@NumeroTag", SqlDbType.VarChar, 10).Value = ln.NumeroTag;
                oCmd.Parameters.Add("@Placa", SqlDbType.VarChar, 7).Value = ln.Placa;
                oCmd.Parameters.Add("@Categoria", SqlDbType.TinyInt).Value = ln.Categoria;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = ln.Secuencia;
                oCmd.Parameters.Add("@Accion", SqlDbType.VarChar, 2).Value = ln.Accion;
                oCmd.Parameters.Add("@Estado", SqlDbType.VarChar, 2).Value = ln.Estado;
                oCmd.Parameters.Add("@Completo", SqlDbType.Char).Value = completo;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ln"></param>
        /// <param name="oConn"></param>
        public static void delListaNegra(PexListaNegra ln, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delListaNegra";

                oCmd.Parameters.Add("@EmisorTag", SqlDbType.VarChar,5).Value = ln.EmisorTag;
                oCmd.Parameters.Add("@NumeroTag", SqlDbType.VarChar, 10).Value = ln.NumeroTag;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = ln.Secuencia;

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
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="admin"></param>
        public static void delListasNegras(Conexion oConn, int admin)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delListasNegras";

                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = admin;

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
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="admin"></param>
        public static void delTags(Conexion oConn, int admin)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delTags";

                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = admin;

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
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Administradora"></param>
        /// <param name="Secuencia"></param>
        /// <param name="Tipo"></param>
        /// <param name="fechaSecuencia"></param>
        public static void addSeclis(Conexion oConn, int Administradora, int Secuencia, string Tipo, DateTime fechaSecuencia, string ListCompl)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addSeclis";

                oCmd.Parameters.Add("@Tipo", SqlDbType.Char,3).Value = Tipo;
                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = Administradora;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = Secuencia;
                oCmd.Parameters.Add("@fecsec", SqlDbType.DateTime).Value = fechaSecuencia;
                oCmd.Parameters.Add("@listComp", SqlDbType.Char, 1).Value = ListCompl;

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

       /// <summary>
       /// Guarda la auditoria de las importaciones realizadas
       /// </summary>
       /// <param name="oConn"></param>
       /// <param name="fecha"></param>
       /// <param name="tipo"></param>
       /// <param name="secuencia"></param>
       /// <param name="archivo"></param>
       /// <param name="cantReg"></param>
       /// <param name="estado"></param>
       /// <param name="motivoError"></param>
        public static void addAuditoriaImportacion(Conexion oConn, DateTime fecha, string tipo, int secuencia, string archivo, int cantReg, string estado, string motivoError)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addAuditoriaImportacion";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = tipo;
                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = secuencia;
                oCmd.Parameters.Add("@archi", SqlDbType.VarChar, 100).Value = archivo;
                oCmd.Parameters.Add("@registros", SqlDbType.Int).Value = cantReg;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = estado;
                oCmd.Parameters.Add("@error", SqlDbType.VarChar, 1000).Value = motivoError;

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
        }

        public static void actualizarEstadoTransitoPex(Conexion oConn, DateTime fecha, int estacion, int via, int identity, char estado, string observacion)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_actualizarEstadoTransito";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@estacion", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@via", SqlDbType.Int).Value = via;
                oCmd.Parameters.Add("@identity", SqlDbType.Int).Value = identity;                
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = estado;
                oCmd.Parameters.Add("@observacion", SqlDbType.VarChar, 250).Value = observacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al cambiar el estado del transito ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void anularTransitoPex(Conexion oConn, PexTransito transito)
        {
            try
            {
                actualizarEstadoTransitoPex(oConn, transito.Fecha, transito.Estacion.Numero, transito.NumeroVia, transito.Codigo, 'N', transito.ObservacionTratamiento);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void perderTransitoPex(Conexion oConn, PexTransito transito)
        {
            try
            {
                actualizarEstadoTransitoPex(oConn, transito.Fecha, transito.Estacion.Numero, transito.NumeroVia, transito.Codigo, 'D', transito.ObservacionTratamiento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void deshacerTransitoPex(Conexion oConn, PexTransito transito)
        {
            try
            {
                actualizarEstadoTransitoPex(oConn, transito.Fecha, transito.Estacion.Numero, transito.NumeroVia, transito.Codigo, 'R', transito.ObservacionTratamiento);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void reenviarTransitoPex(Conexion oConn, PexTransito transito)
        {
            try
            {
                modificarTransitoPex(oConn, transito);                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void modificarTransitoPex(Conexion oConn, PexTransito transito)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_modificarTransito";

                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = transito.Fecha;
                oCmd.Parameters.Add("@estacion", SqlDbType.Int).Value = transito.Estacion.Numero;
                oCmd.Parameters.Add("@via", SqlDbType.Int).Value = transito.NumeroVia;
                oCmd.Parameters.Add("@identity", SqlDbType.Int).Value = transito.Codigo;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = 'M';
                oCmd.Parameters.Add("@observacion", SqlDbType.VarChar, 250).Value = transito.ObservacionTratamiento;
                //atributos modificados con 
                oCmd.Parameters.Add("@categoriaCobrada", SqlDbType.Int).Value = transito.CategoriaConsolidada.Categoria;
                oCmd.Parameters.Add("@placa", SqlDbType.VarChar, 8).Value = transito.Placa;
                oCmd.Parameters.Add("@emisor", SqlDbType.VarChar, 5).Value = transito.CodigoEmisorTag;
                oCmd.Parameters.Add("@tag", SqlDbType.VarChar, 24).Value = transito.NumeroTag;
                oCmd.Parameters.Add("@fotoFrontal", SqlDbType.VarChar, 250).Value = transito.FotoFrontal;
                oCmd.Parameters.Add("@fotoLateral1", SqlDbType.VarChar, 250).Value = transito.FotoLateral1;
                oCmd.Parameters.Add("@fotoLateral2", SqlDbType.VarChar, 250).Value = transito.FotoLateral2;

                oCmd.Parameters.Add("@numev", SqlDbType.Int).Value = transito.NumeroEvento;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = transito.IdValidador;
                oCmd.Parameters.Add("@statusOriginal", SqlDbType.Char).Value = transito.EstadoPexOriginal;
                oCmd.Parameters.Add("@ejesAdic", SqlDbType.Int).Value = transito.NumeroEjes;
                oCmd.Parameters.Add("@tipbo", SqlDbType.Char).Value = transito.Tipbo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                
                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el transito ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }                

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oProtocolo"></param>
        public static void addPropag(Conexion oConn, PexProtocoloFinanciero oProtocolo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addPropag"; 

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = oProtocolo.SecuenciaEnviada;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oProtocolo.FechaPago;
                oCmd.Parameters.Add("@total", SqlDbType.Money).Value = oProtocolo.ValorPago;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = oProtocolo.Administradora;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="secuencia"></param>
        /// <param name="administradora"></param>
        public static void delPropag(Conexion oConn, int secuencia, int administradora)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delPropag";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = secuencia;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = administradora;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oProtocolo"></param>
        public static void addRechazosProtocoloFinanciero(Conexion oConn, PexProtocoloFinanciero oProtocolo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addRechazosProtocoloFinanciero";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = oProtocolo.SecuenciaEnviada;
                oCmd.Parameters.Add("@nureg", SqlDbType.Int).Value = oProtocolo.NumeroRegistro;
                oCmd.Parameters.Add("@codpais", SqlDbType.VarChar, 4).Value = oProtocolo.PaisEmisor;
                oCmd.Parameters.Add("@codEmisor", SqlDbType.VarChar, 5).Value = oProtocolo.EmisorTag;
                oCmd.Parameters.Add("@numTag", SqlDbType.VarChar, 10).Value = oProtocolo.NumeroTag;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oProtocolo.FechaRegEnviado;
                oCmd.Parameters.Add("@plaza", SqlDbType.VarChar, 4).Value = oProtocolo.CodigoPlaza;
                oCmd.Parameters.Add("@pista", SqlDbType.VarChar, 3).Value = oProtocolo.CodigoPista;
                oCmd.Parameters.Add("@total", SqlDbType.Money).Value = oProtocolo.ValorPasada;
                oCmd.Parameters.Add("@codRet", SqlDbType.Char, 2).Value = oProtocolo.CodRetorno;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = oProtocolo.Administradora;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Administradora"></param>
        /// <param name="estacion"></param>
        /// <returns></returns>
        public static PexTransitoL getTransitos(Conexion oConn, int Administradora, int estacion)
        {
            PexTransitoL oTransitos = new PexTransitoL();

            try
            {

                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getTransitos";
                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = Administradora;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                
                oCmd.CommandTimeout = 3000;
                oDR = oCmd.ExecuteReader();
                
                while (oDR.Read())
                {                                           
                    oTransitos.Add(CargarTransitoPex(oDR));
                }


                // Cerramos el objeto
                oDR.Close();
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oTransitos;
        }




        /// <summary>
        /// obtine 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="FECHA DESDE"></param>
        /// <param name="FECHA HASTA"></param>
        /// <param name="ESTACION"></param>
        /// <param name="PLACA"></param>
        /// <param name="CATEGORIA"></param>
        /// <param name="ESTADO PEX 'RECHAZADA, ACEPTADA, PENDIENTE, MODIFICADO, TECNICO'"></param>
        /// <param name="TIPO DE INGRESO, 'ES EL MODO QUE INGRESA EL TRANSITO AL SISTEMA, MANUAL, ANTENA'"></param>
        /// <param name="MODO DE MANTENIMIENTO"></param>
        /// <returns>DataSet</returns>
        public static DataSet getDetalleTransitosPex(Conexion oConn, DateTime desde, DateTime hasta, int? estacion, string categoria, string estadoPex, string tipoIngreso, string modoMantenimiento, string placa, string emisor, string tag)
        {
            DataSet dsDetalleTransitos = new DataSet();
            dsDetalleTransitos.DataSetName = "RptPeaje_DetalleTransitosPex";
            
            try
            {              
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getDetalleTransitos";
                oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@estacion", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@categoria", SqlDbType.Char).Value = Convert.ToChar(categoria);
                oCmd.Parameters.Add("@estadoPex", SqlDbType.Char).Value = Convert.ToChar(estadoPex);
                oCmd.Parameters.Add("@tipoIngreso", SqlDbType.Char).Value = Convert.ToChar(tipoIngreso);
                oCmd.Parameters.Add("@modoMantenimiento", SqlDbType.Char).Value = Convert.ToChar(modoMantenimiento);
                oCmd.Parameters.Add("@placa", SqlDbType.VarChar).Value = placa;
                oCmd.Parameters.Add("@emisor", SqlDbType.VarChar).Value = emisor;
                oCmd.Parameters.Add("@tag", SqlDbType.VarChar).Value = tag;

                oCmd.CommandTimeout = 3000; 
                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsDetalleTransitos, "ListaDetalleTransitos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsDetalleTransitos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static PexTransito CargarTransitoPex(SqlDataReader oDR)
        {
            PexTransito oTransito = new PexTransito();

            oTransito.Estacion = new Estacion { Numero = Convert.ToInt32(oDR["trc_coest"]) };
            oTransito.FechaJornada = Convert.ToDateTime(oDR["par_fejor"]);
            oTransito.NumeroVia = Convert.ToInt32(oDR["trc_nuvia"]);
            oTransito.Fecha = Convert.ToDateTime(oDR["trc_fecha"]);
            oTransito.NumeroEvento = Util.DbValueToNullable<Int32>(oDR["trc_numev"]);
            oTransito.IdDov = Util.DbValueToNullable<Int32>(oDR["dvs_dovid"]);
            oTransito.CodigoPais = Convert.ToString(oDR["trc_pais"]);
            oTransito.CodigoConcesionaria = Convert.ToString(oDR["trc_conce"]);
            oTransito.CodigoEmisorTag = Convert.ToString(oDR["trc_emitg"]);
            oTransito.NumeroTag = Convert.ToString(oDR["trc_numer"]);
            oTransito.CodigoPlaza = Convert.ToString(oDR["trc_plaza"]);
            oTransito.CodigoPista = Convert.ToString(oDR["trc_pista"]);
            oTransito.CategoriaManual = new CategoriaManual { Categoria = Convert.ToByte(oDR["tra_manua"]) };
            oTransito.CategoriaDetectada = new CategoriaManual { Categoria = Convert.ToByte(oDR["tra_dac"]) };
            if (oDR["tra_catco"] != DBNull.Value) oTransito.CategoriaConsolidada = new CategoriaManual { Categoria = Convert.ToByte(oDR["tra_catco"]) };
            oTransito.EjeAdicionalManual = Util.DbValueToNullable<Byte>(oDR["tra_ejadic"]);
            oTransito.EjeAdicionalDectectado = Util.DbValueToNullable<Byte>(oDR["tra_ejdacadic"]);
            oTransito.EjeAdicionalConsolidado = Util.DbValueToNullable<Byte>(oDR["tra_ejconadic"]);
            oTransito.Valor = Convert.ToDecimal(oDR["trc_valor"]);
            oTransito.StatusCobrada = Convert.ToString(oDR["StatusCob"]);
            oTransito.StatusPasada = Convert.ToString(oDR["StatusPass"]);
            oTransito.MotivoImagen = Convert.ToString(oDR["trc_motim"]);
            oTransito.PathVideo = Convert.ToString(oDR["PathVideo"]);
            oTransito.FotoFrontal = Convert.ToString(oDR["FotoFrontal"]);
            oTransito.FotoLateral1 = Convert.ToString(oDR["FotoLateral1"]);
            oTransito.FotoLateral2 = Convert.ToString(oDR["FotoLateral2"]);
            oTransito.Placa = Convert.ToString(oDR["trc_paten"]);
            
            return oTransito;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="numeroSecuencia"></param>
        /// <param name="status"></param>
        /// <param name="estacion"></param>
        /// <param name="numeroVia"></param>
        /// <param name="fecha"></param>
        /// <param name="evento"></param>
        public static void addTransitoEnviado(Conexion oConn, int numeroSecuencia, string status, int estacion, int numeroVia, DateTime fecha, int? evento, int admin, int? dovid)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addTransitoEnviado";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = numeroSecuencia;
                oCmd.Parameters.Add("@status", SqlDbType.Char).Value = status;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = numeroVia;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@evento", SqlDbType.Int).Value = evento;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = admin;
                oCmd.Parameters.Add("@dovid", SqlDbType.Int).Value = dovid;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        public static void setTransitosProcesados(Conexion oConn, int estacion, DateTime? fecha)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_setTransitosProcesados";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="secuencia"></param>
        public static void addSecuenciaEnviada(Conexion oConn, PexSecuencia oSecuencia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addSecuenciaEnviada";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = oSecuencia.NroSecuencia;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = oSecuencia.AdministradoraTag;
                oCmd.Parameters.Add("@dir", SqlDbType.VarChar, 100).Value = oSecuencia.Directorio;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oSecuencia.Estacion.Numero;
                oCmd.Parameters.Add("@feenv", SqlDbType.DateTime).Value = oSecuencia.FechaEnvio;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = oSecuencia.Jornada;
                oCmd.Parameters.Add("@total", SqlDbType.Money).Value = oSecuencia.MontoTotal;
                oCmd.Parameters.Add("@reg", SqlDbType.Int).Value = oSecuencia.Registros;
                oCmd.Parameters.Add("@cantFotos", SqlDbType.Int).Value = oSecuencia.CantFotos;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = oSecuencia.TipoArchivo;

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="protocolo"></param>
        public static void updProtocoloTecnico(Conexion oConn, PexProtocoloTecnico oProtocolo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_updProtocoloTecnico";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = oProtocolo.SecuenciaEnviada;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = oProtocolo.Administradora;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = "TRN";
                oCmd.Parameters.Add("@fechaRece", SqlDbType.DateTime).Value = oProtocolo.FechaTecnico;
                oCmd.Parameters.Add("@respuesta", SqlDbType.VarChar, 2).Value = oProtocolo.Resultado;
                oCmd.Parameters.Add("@regenc", SqlDbType.Int).Value = oProtocolo.RegEncontrados;
                oCmd.Parameters.Add("@totalenc", SqlDbType.Money).Value = oProtocolo.TotalEncontrado;
                
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
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("El protocolo Tecnico es de una secuencia inexistente");
                        throw new WarningException(msg);
                    }
                    else if (retval == -103)
                    {
                        //Probablemente copie 2 veces el mismo TRT
                        msg = Traduccion.Traducir("El protocolo Tecnico ya habia sido recibido");
                        throw new WarningException(msg);
                    }
                    else if (retval == -104)
                    {
                        //Probablemente envie 2 veces el mismo TRN
                        msg = Traduccion.Traducir("El protocolo Tecnico tuvo error 05 Repetido y ya esta secuencia ya habia sido recibida");
                        throw new WarningException(msg);
                    }
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="secu"></param>
        /// <param name="admtag"></param>
        /// <param name="totalAceptado"></param>
        /// <param name="totalRechazado"></param>
        public static void updProtocoloFinanciero(Conexion oConn, int secu, int admtag, DateTime fechaRecibido, decimal totalAceptado, decimal totalRechazado)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_updProtocoloFinanciero";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = secu;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = admtag;
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = "TRN";
                oCmd.Parameters.Add("@fechaRece", SqlDbType.DateTime).Value = fechaRecibido;
                oCmd.Parameters.Add("@totalAceptado", SqlDbType.Money).Value = totalAceptado;
                oCmd.Parameters.Add("@totalRechazado", SqlDbType.Money).Value = totalRechazado;

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
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("El protocolo Financiero es de una secuencia inexistente");
                        throw new WarningException(msg);
                    }
                    else if (retval == -103)
                    {//Probablemente copie 2 veces el mismo TRF
                        msg = Traduccion.Traducir("El protocolo Financiero ya habia sido recibido");
                        throw new WarningException(msg);
                    }
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="tipo"></param>
        /// <param name="administradora"></param>
        /// <returns></returns>
        public static int getSecuencia(Conexion oConn, string tipo, int administradora)
        {
            int secuencia = 1;

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getSecuencia";
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = tipo;
                oCmd.Parameters.Add("@admin", SqlDbType.TinyInt).Value = administradora;

                SqlParameter parSecuencia = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parSecuencia.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;
                if (parSecuencia.Value != DBNull.Value)
                {
                    secuencia = Convert.ToInt32(parSecuencia.Value);
                }

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al buscar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            return secuencia;
        }


        ///***************************************************************************************
        /// <summary>
        /// devuelve uan lista de transitos que pex a rechazado
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="fechaDesde"></param>
        /// <param name="fechaHasta"></param>
        /// <param name="rechazo"></param>
        /// <param name="xiCantRows"></param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        ///***************************************************************************************        
        public static PexTransitoL getTransitosRechazados(Conexion oConn, DateTime fechaDesde, DateTime fechaHasta, int? estacion, int? administradoraTag, char estado, string codigoRechazo, int xiCantRows, ref bool llegoAlTope)
        {
            PexTransitoL transitosRechazadosL = new PexTransitoL();
            try
            {
            SqlDataReader oDR;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_OSAs_getTransitosRechazados";
            oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechaDesde;
            oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechaHasta;          
            oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion;           
            oCmd.Parameters.Add("@admtag", SqlDbType.Int).Value = administradoraTag;            
            oCmd.Parameters.Add("@status", SqlDbType.Char).Value = estado;
            oCmd.Parameters.Add("@codrechazo", SqlDbType.VarChar).Value = codigoRechazo;
            oCmd.Parameters.Add("@CantRows", SqlDbType.Int).Value = xiCantRows;
            
            
            oDR = oCmd.ExecuteReader();
            int i = 0;
            while (oDR.Read())
            {
                i++;
                transitosRechazadosL.Add(CargarTransitoRechazado(oDR));
                //Verificamos de no pasarnos de xiCantRows para que no pinche el Silverlight
                if (i == xiCantRows)
                {
                    llegoAlTope = true;
                    break;
                }
            }            
            // Cerramos el objeto
            oDR.Close();
            oCmd = null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return transitosRechazadosL;
        }

        ///***************************************************************************************
        /// <summary>
        /// carga datos a un Transito rechzado por pex, devuelve el transito pex cargado
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        ///***************************************************************************************
        public static PexTransito CargarTransitoRechazado(SqlDataReader oDR)
        {
            PexTransito transitoRechazado = new PexTransito();

            if (oDR["Estacion"] != DBNull.Value)
            {
                transitoRechazado.Estacion = new Estacion();
                transitoRechazado.Estacion.Numero = Convert.ToInt32(oDR["Estacion"]);
            }
            if (oDR["EstacionDescr"] != DBNull.Value)
                transitoRechazado.Estacion.Nombre = Convert.ToString(oDR["EstacionDescr"]);

            if (oDR["Via"] != DBNull.Value)
            {
                transitoRechazado.NumeroVia = Convert.ToByte(oDR["Via"]);
            }
            if (oDR["NumeroSecuencia"] != DBNull.Value)
                transitoRechazado.NumeroSecuencia = Convert.ToInt32(oDR["NumeroSecuencia"]);
            
            if (oDR["Patente"] != DBNull.Value)
            {
                transitoRechazado.Placa= Convert.ToString(oDR["Patente"]);
            }
            if (oDR["Fecha"] != DBNull.Value)
                transitoRechazado.Fecha = Convert.ToDateTime(oDR["Fecha"]);

            if (oDR["NumeroEvento"] != DBNull.Value)
                transitoRechazado.NumeroEvento = Convert.ToInt32(oDR["NumeroEvento"]);

            if (oDR["Dovid"] != DBNull.Value)
                transitoRechazado.IdDov = Convert.ToInt32(oDR["Dovid"]);

            if (oDR["NumeroTag"] != DBNull.Value)
            {                
                transitoRechazado.NumeroTag = Convert.ToString(oDR["NumeroTag"]);
            }
            if (oDR["IDEmisorTag"] != DBNull.Value)
                transitoRechazado.CodigoEmisorTag = Convert.ToString(oDR["IDEmisorTag"]);

            if (oDR["ModoVia"] != DBNull.Value)
                transitoRechazado.ModoViaTurno = Convert.ToString(oDR["ModoViaDescr"]);

            if (oDR["ModoViaDescr"] != DBNull.Value)
            {
                transitoRechazado.ModoViaCodigo = Convert.ToString(oDR["ModoVia"]);
            }
            if (oDR["CategoriaDetectada"] != DBNull.Value)
            {
                transitoRechazado.CategoriaDetectada = new CategoriaManual();
                transitoRechazado.CategoriaDetectada.Categoria = Convert.ToByte(oDR["CategoriaDetectada"]);
            }
            if (oDR["CategoriaDetectadaDescr"] != DBNull.Value)
                transitoRechazado.CategoriaDetectada.Descripcion = Convert.ToString(oDR["CategoriaDetectadaDescr"]);
            
            if (oDR["CategoriaTabulada"] != DBNull.Value)
            {
                transitoRechazado.CategoriaManual = new CategoriaManual();
                transitoRechazado.CategoriaManual.Categoria = Convert.ToByte(oDR["CategoriaTabulada"]);
            }
            if (oDR["CategoriaTabuladaDescr"] != DBNull.Value)
                transitoRechazado.CategoriaManual.Descripcion = Convert.ToString(oDR["CategoriaTabuladaDescr"]);

            if (oDR["CategoriaCobrada"] != DBNull.Value)
            {
                transitoRechazado.CategoriaConsolidada = new CategoriaManual();
                transitoRechazado.CategoriaConsolidada.Categoria = Convert.ToByte(oDR["CategoriaCobrada"]);
            }
            if (oDR["CategoriaCobradaDescr"] != DBNull.Value)
                transitoRechazado.CategoriaConsolidada.Descripcion = Convert.ToString(oDR["CategoriaCobradaDescr"]);

            if (oDR["ModoLectura"] != DBNull.Value)
                transitoRechazado.ModoLectura = Convert.ToString(oDR["ModoLectura"]);

            if (oDR["ObservacionesInternas"] != DBNull.Value)
                transitoRechazado.ObservacionesInternasValidacion = Convert.ToString(oDR["ObservacionesInternas"]);

            if (oDR["ObservacionesInternas"] != DBNull.Value)
                transitoRechazado.ObservacionesExternasValidacion = Convert.ToString(oDR["ObservacionesExternas"]);

            if (oDR["ObservacionTratamiento"] != DBNull.Value)
                transitoRechazado.ObservacionTratamiento = Convert.ToString(oDR["ObservacionTratamiento"]);

            if (oDR["CausaRechazo"] != DBNull.Value)
            {
                transitoRechazado.CausaRechazo = new Telectronica.Validacion.MotivoRechazo();
                transitoRechazado.CausaRechazo.Descripcion = Convert.ToString(oDR["CausaRechazo"]);
            }
            if (oDR["ListaNegra"] != DBNull.Value)
                transitoRechazado.Diponibilidad = Convert.ToString(oDR["ListaNegra"]);

            if (oDR["TipoDispositivo"] != DBNull.Value)
                transitoRechazado.TipoDispositivo = Convert.ToString(oDR["TipoDispositivo"]);

            if (oDR["Codigo"] != DBNull.Value)
                transitoRechazado.Codigo = Convert.ToInt32(oDR["Codigo"]);

            if (oDR["FotoFrontal"] != DBNull.Value)
                transitoRechazado.FotoFrontal = Convert.ToString(oDR["FotoFrontal"]);

            if (oDR["FotoLateral1"] != DBNull.Value)
                transitoRechazado.FotoLateral1 = Convert.ToString(oDR["FotoLateral1"]);

            if (oDR["FotoLateral2"] != DBNull.Value)
                transitoRechazado.FotoLateral2 = Convert.ToString(oDR["FotoLateral2"]);

            if (oDR["Estado"] != DBNull.Value)
                transitoRechazado.EstadoPex = Convert.ToString(oDR["Estado"]);

            if (oDR["EjesAdicDetectados"] != DBNull.Value)
                transitoRechazado.EjeAdicionalDectectado = Convert.ToInt32(oDR["EjesAdicDetectados"]);

            if (oDR["EjesAdicTabulados"] != DBNull.Value)
                transitoRechazado.EjeAdicionalManual = Convert.ToInt32(oDR["EjesAdicTabulados"]);

            if (oDR["EjesAdicCobrados"] != DBNull.Value)
                transitoRechazado.EjeAdicionalConsolidado = Convert.ToInt32(oDR["EjesAdicCobrados"]);

                
            return transitoRechazado;
        }

        public static PexCambioTarifaL getCambioTarifas(Conexion oConn, int estacion, int admtag)
        {
            PexCambioTarifaL oCambioTarifas = new PexCambioTarifaL();

            try
            {
                SqlDataReader oDR;
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getCambioTarifa";

                oCmd.Parameters.Add("@coest", SqlDbType.Char, 3).Value = estacion;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = admtag;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 3000;
                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                {
                    oCambioTarifas.Add(CargarCambioTarifaPex(oDR));
                }

                // Cerramos el objeto
                oDR.Close();
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oCambioTarifas;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="tipo"></param>
        /// <param name="administradora"></param>
        /// <returns></returns>
        public static int getSeclis(Conexion oConn, string tipo, int administradora)
        {
            int secuencia = 0;

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getSeclis";
                oCmd.Parameters.Add("@tipo", SqlDbType.Char, 3).Value = tipo;
                oCmd.Parameters.Add("@admin", SqlDbType.TinyInt).Value = administradora;

                SqlParameter parSecuencia = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parSecuencia.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;
                if (parSecuencia.Value != DBNull.Value)
                {
                    secuencia = Convert.ToInt32(parSecuencia.Value);
                }

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al buscar el registro") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return secuencia;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="estacion"></param>
        /// <param name="fecha"></param>
        /// <returns></returns>
        public static PexTarifaL getTarifas(Conexion oConn, int estacion, DateTime fecha)
        {
            PexTarifaL oTarifas = new PexTarifaL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getTarifas";
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                oCmd.CommandTimeout = 3000;
                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                {
                    oTarifas.Add(CargarTarifaPex(oDR));
                }

                // Cerramos el objeto
                oDR.Close();
                oCmd = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oTarifas;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        public static PexTarifa CargarTarifaPex(SqlDataReader oDR)
        {
            PexTarifa oTarifa = new PexTarifa();

            oTarifa.TipoEstablecimiento = Convert.ToString(oDR["tipEsta"]);
            oTarifa.CodEstablecimiento = Convert.ToString(oDR["codConce"]);
            oTarifa.CodPlaza = Convert.ToString(oDR["codPlaza"]);
            oTarifa.Estacion = new Estacion { Numero = Convert.ToInt32(oDR["tar_estac"]) };
            oTarifa.Categoria = new CategoriaManual{ Categoria = Convert.ToByte(oDR["tar_tarif"]) };
            oTarifa.DiaSemana = Convert.ToString(oDR["diaSemana"]);
            oTarifa.Valor = Convert.ToDecimal(oDR["tar_valor"]);
            oTarifa.TipoInformacion = Convert.ToString(oDR["tipoInfo"]);
            oTarifa.FechaInicio = Convert.ToDateTime(oDR["tar_fecha"]);

            return oTarifa;
        }

        public static PexCambioTarifa CargarCambioTarifaPex(SqlDataReader oDR)
        {
            PexCambioTarifa oTarifa = new PexCambioTarifa();
            oTarifa.Estacion = new Estacion { Numero = Convert.ToInt32(oDR["tar_estac"]) };
            oTarifa.Fecha = Convert.ToDateTime(oDR["tar_fecha"]);

            return oTarifa;
        }

        public static void addAlerta(Conexion oConn, int secuencia, int proxSecuencia, string extArchivo, string descr)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_addAlerta";

                oCmd.Parameters.Add("@secuencia", SqlDbType.Int).Value = secuencia;
                oCmd.Parameters.Add("@proxSecuencia", SqlDbType.Int).Value = proxSecuencia;
                oCmd.Parameters.Add("@extArchivo", SqlDbType.Char, 3).Value = extArchivo;
                oCmd.Parameters.Add("@descr", SqlDbType.VarChar, 50).Value = descr;

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
        }

        public static void clrAlerta(Conexion oConn, int secuenciaRec, string extArchivo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_clrAlerta";

                oCmd.Parameters.Add("@secuenciaRec", SqlDbType.Int).Value = secuenciaRec;
                oCmd.Parameters.Add("@extArchivo", SqlDbType.Char, 3).Value = extArchivo;

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
        }

        public static void prepararProcesoTags(Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_prepararProcesoTags";

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al procesar tags ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void finalizarProcesoTags(Conexion oConn, int secuencia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_finalizarProcesoTags";

                oCmd.Parameters.Add("@secuencia", SqlDbType.Int).Value = secuencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al procesar tags ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void prepararProcesoListaNegra(Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAS_prepararProcesoListaNegra";

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al procesar tags ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void finalizarProcesoListaNegra(Conexion oConn, int secuencia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Pex_finalizarProcesoListaNegra";

                oCmd.Parameters.Add("@secuencia", SqlDbType.Int).Value = secuencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al procesar tags ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="secuencia"></param>
        /// <returns></returns>
        public static int getSecuenciaCompleta(Conexion oConn, int secuencia, DateTime fecha)
        {
            int secuenciaCompleta = 1;

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getSecuenciaCompleta";
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = secuencia;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;

                SqlParameter parSecuencia = oCmd.Parameters.Add("@completa", SqlDbType.Int);
                parSecuencia.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();

                int retval = (int)parRetVal.Value;
                if (parSecuencia.Value != DBNull.Value)
                {
                    secuenciaCompleta = Convert.ToInt32(parSecuencia.Value);
                }

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al buscar el registro ") + retval.ToString();
                    if (retval == (int)EjecucionSP.enmErrorSP.enmFALTA_PARAMETRO)
                        msg = Traduccion.Traducir("Faltan pasar parámetros al Stored Procedure");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return secuenciaCompleta;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="secuencia"></param>
        /// <param name="administradora"></param>
        public static void delTRFCCO(Conexion oConn, int secuencia, int administradora)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_delTRFCCO";

                oCmd.Parameters.Add("@secu", SqlDbType.Int).Value = secuencia;
                oCmd.Parameters.Add("@admtag", SqlDbType.TinyInt).Value = administradora;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// *******************************************************************************************************
        /// <summary>
        /// Actualiza el status de los tags eliminados (que no vienen en el nuevo archivo) cuando es un archivo TOTAL
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="administradora">Numero de Administradora</param>
        /// <param name="secuencia">Codigo de secuencia recibida</param>
        /// *******************************************************************************************************
        public static void finalizarProcesoTagsListaCompleta(Conexion oConn, int administradora, int secuencia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_finalizarProcesoTagsListaCompleta";

                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = administradora;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = secuencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// *******************************************************************************************************
        /// <summary>
        /// Actualiza el status de los dados de baja en la lista negra (que no vienen en el nuevo archivo) cuando es un archivo TOTAL
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="administradora">Numero de Administradora</param>
        /// <param name="secuencia">Codigo de secuencia recibida</param>
        /// *******************************************************************************************************
        public static void finalizarProcesoListaNegraListaCompleta(Conexion oConn, int administradora, int secuencia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_finalizarProcesoListaNegraListaCompleta";

                oCmd.Parameters.Add("@Administradora", SqlDbType.TinyInt).Value = administradora;
                oCmd.Parameters.Add("@Secuencia", SqlDbType.Int).Value = secuencia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
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

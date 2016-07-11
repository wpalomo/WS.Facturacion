using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Peaje;
using System.IO;
using System.Xml;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Tesoreria
{
    public static class RecuentoDt
    {

        public static RecuentoL getRecuentos(Conexion oConn, DateTime? Desde, DateTime? Hasta, int Estacion)
        {
            RecuentoL oRecuentoL = new RecuentoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuentos_GetRecuentos";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;
                oCmd.Parameters.Add("@Desde", SqlDbType.DateTime).Value = Desde;
                oCmd.Parameters.Add("@Hasta", SqlDbType.DateTime).Value = Hasta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRecuentoL.Add(CargarRecuento(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRecuentoL;
        }

        private static Recuento CargarRecuento(System.Data.IDataReader oDR)
        {
            Recuento oRecuento = new Recuento();

            oRecuento.Estacion = new Estacion((byte)oDR["rec_coest"], oDR["est_nombr"].ToString());
            oRecuento.Fecha = (DateTime)oDR["rec_fecha"];
            if (oDR["rec_archivo"] != null && oDR["rec_archivo"].ToString() != "")
            {
                oRecuento.NombreArchivo = (string)oDR["rec_archivo"];
            }
            oRecuento.NumeroRecuento = (int)(oDR["rec_numer"]);
            oRecuento.Usuario = new Usuario(oDR["rec_id"].ToString(), oDR["use_nombr"].ToString());
            oRecuento.Recontado = (decimal)oDR["totalValorRecontado"];

            return oRecuento;
        }

        
        private static RecuentoDetalle CargarRecuentoDetalle(System.Data.IDataReader oDR)
        {
            RecuentoDetalle oRecuentoDetalle = new RecuentoDetalle();

            oRecuentoDetalle.estacion = new Estacion((byte)oDR["dec_coest"], oDR["est_nombr"].ToString());
            oRecuentoDetalle.CodigoEstacion = Convert.ToInt16(oDR["dec_coest"]);
            oRecuentoDetalle.Ingresado = (decimal)oDR["dec_valoringresado"];
            oRecuentoDetalle.Diferencia = (decimal)oDR["dec_diferencia"];
            oRecuentoDetalle.Observacion = (string)oDR["dec_observacion"];
            oRecuentoDetalle.Recontado = (decimal)oDR["dec_valorrecontado"];

            // Numero de deposito
            oRecuentoDetalle.NumeroDeposito = (int)oDR["dec_iddep"];

            // NUMERO DE DETDEPMOC
            oRecuentoDetalle.NumeroDepositoMocaja = (int)oDR["dem_idmoc"];
            oRecuentoDetalle.ParteRecuento = new Parte();
            oRecuentoDetalle.ParteRecuento.Numero = (int)oDR["moc_parte"];
            //NUMERO DE REGISTRO DE RECUENTO
            //oRecuentoDetalle.NumeroRegistro = (int)oDR["dec_idrec"];

            return oRecuentoDetalle;
        }
        

        
        public static RecuentoDetalleL getDetalleRecuentos(Conexion oConn, int iNumeroRecuento)
        {
            RecuentoDetalleL oRecuentoDetalleL = new RecuentoDetalleL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuentos_GetRecuentosDetalle";
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = iNumeroRecuento;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRecuentoDetalleL.Add(CargarRecuentoDetalle(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRecuentoDetalleL;
        }
        

        private static RecuentosDepositos CargarRecuentosDepositos(System.Data.IDataReader oDR)
        {
            RecuentosDepositos oRecuentosDepositos = new RecuentosDepositos();

            // Cargamos los datos del recuento si existe: (Lado archivo)
            if (oDR["Recuento"] != null && oDR["Recuento"].ToString() != "")
            {
                oRecuentosDepositos.oRecuento.CodigoRecuento = oDR["Recuento"].ToString();
                oRecuentosDepositos.oRecuento.Fecha = (DateTime)oDR["Fecha"];
                oRecuentosDepositos.oRecuento.Letra = oDR["Letra"].ToString();
                oRecuentosDepositos.oRecuento.F22Recuento = (int)oDR["F22"];
                oRecuentosDepositos.oRecuento.Sucursal = oDR["Sucursal"].ToString();
                oRecuentosDepositos.oRecuento.SobreRecuento = (int)oDR["Sobre"];
                oRecuentosDepositos.oRecuento.MedioDePago = oDR["MedioDePago"].ToString();
                oRecuentosDepositos.oRecuento.Moneda = oDR["Moneda"].ToString();
                oRecuentosDepositos.oRecuento.Ingresado = (decimal)oDR["Ingresado"];
                oRecuentosDepositos.oRecuento.Recontado = (decimal)oDR["Recontado"];
                oRecuentosDepositos.oRecuento.Diferencia = (decimal)oDR["Diferencia"];
                oRecuentosDepositos.oRecuento.Observacion = oDR["Observaciones"].ToString();


                //oRecuentosDepositos.oRecuento.Estacion = new Estacion((byte)oDR["EstacionArchivo"], oDR["est_nombr"].ToString());
                //oRecuentosDepositos.oRecuento.CodigoEstacion = (byte)oDR["EstacionArchivo"];
                //oRecuentosDepositos.oRecuento.FechaRegistro = (byte)oDR["rec_coest"];
                //oRecuentosDepositos.oRecuento.IdUsuario = (byte)oDR["rec_coest"];
                //oRecuentosDepositos.oRecuento.NombreArchivo = (byte)oDR["rec_coest"];
                //oRecuentosDepositos.oRecuento.NumeroRecuento = (byte)oDR["rec_coest"];
                //oRecuentosDepositos.oRecuento.usuario = (byte)oDR["rec_coest"];
                //oRecuentosDepositos.oRecuento.NumeroDeposito = "";
                //oRecuentosDepositos.oRecuento.NumeroDetalleDeposito = "";
                //oRecuentosDepositos.oRecuento.NumeroMocaja = "";
            }

            // Cargamos los datos del deposito si existe: (Lado Bolsas sin recontar)
            if (oDR["Estacion"] != null && oDR["Estacion"].ToString() != "")
            {
                oRecuentosDepositos.oBolsaDeposito.Estacion = new Estacion((byte)oDR["Estacion"], oDR["NombreEstacion"].ToString());
                oRecuentosDepositos.oBolsaDeposito.Jornada = (DateTime)oDR["Jornada"];
                oRecuentosDepositos.oBolsaDeposito.Turno = Convert.ToInt32(oDR["Turno"]);
                oRecuentosDepositos.oBolsaDeposito.MontoEquivalente = (decimal)oDR["Monto"];
                oRecuentosDepositos.oBolsaDeposito.Parte = Convert.ToInt32(oDR["Parte"]);
                if (oDR["Bolsa"] != null && oDR["Bolsa"].ToString() != "")
                {
                    if (oDR["Guia"] != null && oDR["Guia"].ToString() != "")
                    {
                        oRecuentosDepositos.oBolsaDeposito.Remito = Convert.ToInt32(oDR["Guia"]);
                    }
                    //oRecuentosDepositos.oBolsaDeposito.Remito = Utilitarios.Util.DbValueToNullable<int>(oDR["Guia"]);
                    //oRecuentosDepositos.oBolsaDeposito.Remito = (int?)oDR["Guia"];
                    oRecuentosDepositos.oBolsaDeposito.Bolsa = (int?)oDR["Bolsa"];
                    oRecuentosDepositos.oBolsaDeposito.NumeroDeposito = Utilitarios.Util.DbValueToNullable<int>(oDR["dep_numer"]);
                    oRecuentosDepositos.oBolsaDeposito.NumeroApropiacion = (int)oDR["apr_numer"];
                }

                //oRecuentosDepositos.oBolsaDeposito.Movimiento = (string)oDR["Moneda"];
                //oRecuentosDepositos.oBolsaDeposito.MovimientoDescripcion = (string)oDR["Moneda"];
                //oRecuentosDepositos.oBolsaDeposito.NumeroMovimiento = (string)oDR["Moneda"];
                //oRecuentosDepositos.oBolsaDeposito.Parte = (string)oDR["Moneda"];
                //oRecuentosDepositos.oBolsaDeposito.Tipo = (string)oDR["Moneda"];
                //oRecuentosDepositos.oBolsaDeposito.TipoDescripcion = (string)oDR["Moneda"];
            }

            // El status se actualiza en Business:
            //oRecuentosDepositos.Status = "";

            return oRecuentosDepositos;
        }

        public static RecuentosDepositosL getDepositosRecuentos(Conexion oConn, XmlDocument Arbol, DateTime FechaDesde, int? Estacion)
        {

            RecuentosDepositosL oRecuentosDepositos = new RecuentosDepositosL();

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlDataReader oDR;
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuentos_GetDepositosRecuentos";
                oCmd.Parameters.Add("@Archivo", SqlDbType.Xml).Value = Arbol.InnerXml;
                oCmd.Parameters.Add("@FechaDesde", SqlDbType.DateTime).Value = FechaDesde;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oRecuentosDepositos.Add(CargarRecuentosDepositos(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oRecuentosDepositos;
        }

        public static bool addRecuentoCabecera(Conexion oConn, Recuento oRecuento)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuento_addRecuento";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRecuento.Estacion.Numero;
                oCmd.Parameters.Add("@user", SqlDbType.VarChar, 10).Value = oRecuento.Usuario.ID;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oRecuento.Fecha;
                oCmd.Parameters.Add("@archivo", SqlDbType.VarChar, 1000).Value = oRecuento.NombreArchivo;

                SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parNumero.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (parNumero.Value != DBNull.Value)
                    oRecuento.NumeroRecuento = Convert.ToInt32(parNumero.Value);

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
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

        public static bool addRecuentoDetalle(Conexion oConn, RecuentoDetalle oRecuentoDetalle, Recuento oRecuento)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuento_addRecuentoDetalle";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRecuento.Estacion.Numero;
                oCmd.Parameters.Add("@idrec", SqlDbType.Int).Value = oRecuento.NumeroRecuento;
                oCmd.Parameters.Add("@iddep", SqlDbType.Int).Value = oRecuentoDetalle.NumeroDeposito;

                // De donde obtengo el Id Apropiación???
                oCmd.Parameters.Add("@idapr", SqlDbType.Int).Value = oRecuentoDetalle.NumeroApropiacion;

                oCmd.Parameters.Add("@ingre", SqlDbType.Decimal).Value = oRecuentoDetalle.Ingresado;
                oCmd.Parameters.Add("@recon", SqlDbType.Decimal).Value = oRecuentoDetalle.Recontado;
                oCmd.Parameters.Add("@difer", SqlDbType.Decimal).Value = oRecuentoDetalle.Diferencia;
                oCmd.Parameters.Add("@obser", SqlDbType.VarChar, 1000).Value = oRecuentoDetalle.Observacion;

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
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
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


        /// ***********************************************************************************************************
        /// <summary>
        /// Elimina el recuento
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oRecuento"></param>
        /// <returns></returns>
        /// ***********************************************************************************************************
        public static bool delRecuento(Conexion oConn, Recuento oRecuento)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuento_delRecuento";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRecuento.Estacion.Numero;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oRecuento.NumeroRecuento;

                

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

        /// ***********************************************************************************************************
        /// <summary>
        /// Verifico si el recuento posee alguna reposicion financiera paga
        /// </summary>
        /// <param name="oRecuento"></param>
        /// <returns></returns>
        /// ***********************************************************************************************************
        public static bool VerificarReposicionPaga(Recuento oRecuento, Conexion oConn, out string verif)
        {
            bool SinReposicionPaga = true;
            verif = "";
            try
            { 
                SqlDataReader oDR;
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuentos_GetReposicionesPagas";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oRecuento.Estacion.Numero;
                oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oRecuento.NumeroRecuento;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    SinReposicionPaga = false;
                    verif = Traduccion.Traducir("El recuento posee alguna reposición financiera paga");
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return SinReposicionPaga;
        }

        ///*****************************************************************************************************
        /// <summary>
        /// Elimina el detalle del recuento
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="estacion"></param>
        /// <param name="parte"></param>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static bool delDetalleRecuento(Conexion oConn, Estacion estacion, Parte parte)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Recuento_delRecuentoDetalle";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;

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
    }
}


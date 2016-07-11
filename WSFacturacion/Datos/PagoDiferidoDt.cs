using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Facturacion;

namespace Telectronica.Peaje
{
    public class PagoDiferidoDt
    {
        #region JORNADA: Clase de Datos de JornadaDt.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los pagos diferidos generados en la via
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Fecha Desde</param>
        /// <param name="jornadaHasta">DateTime - Fecha Hasta</param>
        /// <param name="estado">string - Estado del pago diferido (A: Aceptado, R:Rechazado)</param>
        /// <returns>Lista de Pagos Diferidos</returns>
        /// ***********************************************************************************************
        public static PagoDiferidoSupervisorL getPagosDiferidos(Conexion oConn, int estacion, DateTime fechaDesde, DateTime fechaHasta, string estado)
        {
            PagoDiferidoSupervisorL oPagosDiferidos = new PagoDiferidoSupervisorL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_GetPagosDiferidos";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = estado;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPagosDiferidos.Add(CargarPagoDiferidoSup(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPagosDiferidos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un pago diferido
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Objeto PagoDiferido</returns>
        /// ***********************************************************************************************
        //public static PagoDiferidoSupervisor getPagoDiferido(Conexion oConn, int estacion, int numPago)
        //{
        //    PagoDiferidoSupervisor oPagoDiferidoSup = new PagoDiferidoSupervisor();
        //    try
        //    {
        //        SqlDataReader oDR;

        //        // Creamos, cargamos y ejecutamos el comando
        //        SqlCommand oCmd = new SqlCommand();
        //        oCmd.Connection = oConn.conection;
        //        oCmd.Transaction = oConn.transaction;

        //        oCmd.CommandType = System.Data.CommandType.StoredProcedure;
        //        oCmd.CommandText = "Peaje.usp_PagoDiferido_GetPagoDiferidoSup";
        //        oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
        //        oCmd.Parameters.Add("@numPago", SqlDbType.Int).Value = numPago;

                
        //        oDR = oCmd.ExecuteReader();
        //        oDR.Read();
        //        oPagoDiferidoSup = (CargarPagoDiferidoSup(oDR));

        //        // Cerramos el objeto
        //        oCmd = null;
        //        oDR.Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return oPagoDiferidoSup;
        //}

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el reconocimiento de deuda de pago diferido Como un DataSet
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroPago">Int - Numero del pago diferido</param>
        /// <param name="patente">String - Patente del vehiculo</param>
        /// <param name="cantidadCopias">int - Cantidad de copias a imprimir</param>
        /// <returns>DataSet con el reconocimiento</returns>
        /// ***********************************************************************************************
        public static DataSet getReconocimientoDeuda(Conexion oConn, int numeroPago, int estacion, DateTime fecge, int cantidadCopias)
        {
            DataSet dsReconocimiento = new DataSet();
            dsReconocimiento.DataSetName = "PagoDiferido_DetalleDS";
            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_getReconocimientoDeuda";
                oCmd.Parameters.Add("@numero", SqlDbType.Int).Value = numeroPago;
                oCmd.Parameters.Add("@estacion", SqlDbType.VarChar, 10).Value = estacion;
                oCmd.Parameters.Add("@fecge", SqlDbType.DateTime).Value = fecge; // Agregado
                oCmd.Parameters.Add("@nCopias", SqlDbType.TinyInt).Value = cantidadCopias;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsReconocimiento, "DetalleReconocimiento");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsReconocimiento;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Pagos Diferidos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Jornadas</param>
        /// <returns>elemento PagoDiferido</returns>
        /// ***********************************************************************************************
        //private static PagoDiferido CargarPagoDiferido(System.Data.IDataReader oDR)
        //{
        //    PagoDiferido oPagoDiferido = new PagoDiferido();

        //    oPagoDiferido.Estacion = new Estacion((Byte)oDR["est_codig"], oDR["est_nombr"].ToString());
        //    oPagoDiferido.Categoria = new CategoriaManual { Categoria = (Byte)oDR["cat_tarif"], Descripcion = oDR["cat_descr"].ToString() };
        //    oPagoDiferido.Estado = oDR["pag_estad"].ToString();
        //    if (oPagoDiferido.Estado == "A")
        //        oPagoDiferido.DescEstado = "Aceptado";
        //    else if (oPagoDiferido.Estado == "R")
        //        oPagoDiferido.DescEstado = "Rechazado";
        //    oPagoDiferido.Evento = (int)oDR["pag_numev"];
        //    oPagoDiferido.FechaGeneracion = (DateTime)oDR["pag_fecge"];
        //    oPagoDiferido.FechaVencimiento = Utilitarios.Util.DbValueToNullable<DateTime>(oDR["pag_feven"]);
        //    oPagoDiferido.Importe = (decimal)oDR["pag_monge"];
        //    oPagoDiferido.NumeroPagoDiferido = (int)oDR["pag_numer"];
        //    oPagoDiferido.NumeroParte = (int)oDR["pag_parge"];
        //    oPagoDiferido.NumeroVia = (Byte)oDR["pag_viage"];
        //    oPagoDiferido.Operador = new Usuario(oDR["OperId"].ToString(), oDR["OperNombr"].ToString());
        //    oPagoDiferido.Patente = oDR["pag_paten"].ToString();
        //    oPagoDiferido.Documento = oDR["pag_docum"].ToString();
        //    oPagoDiferido.Supervisor = new Usuario(oDR["SupId"].ToString(), oDR["SupNombr"].ToString());
            
        //    return oPagoDiferido;
        //}

        private static PagoDiferidoSupervisor CargarPagoDiferidoSup(System.Data.IDataReader oDR)
        {
            PagoDiferidoSupervisor oPagoDiferidoSup = new PagoDiferidoSupervisor();

            oPagoDiferidoSup.Estacion = new Estacion((Byte)oDR["est_codig"], oDR["est_nombr"].ToString());
            oPagoDiferidoSup.Categoria = new CategoriaManual { Categoria = (Byte)oDR["cat_tarif"], Descripcion = oDR["cat_descr"].ToString() };
            oPagoDiferidoSup.Estado = oDR["pag_estad"].ToString();
            if (oPagoDiferidoSup.Estado == "A")
                oPagoDiferidoSup.DescEstado = "Aceptado";
            else if (oPagoDiferidoSup.Estado == "R")
                oPagoDiferidoSup.DescEstado = "Rechazado";
            oPagoDiferidoSup.Evento = (int)oDR["pag_numev"];
            oPagoDiferidoSup.FechaVencimiento = Utilitarios.Util.DbValueToNullable<DateTime>(oDR["pag_feven"]);
            oPagoDiferidoSup.Importe = (decimal)oDR["pag_monge"];
            oPagoDiferidoSup.NumeroPagoDiferido = (int)oDR["pag_numer"];
            oPagoDiferidoSup.NumeroParte = (int)oDR["pag_parge"];
            oPagoDiferidoSup.NumeroVia = (Byte)oDR["pag_viage"];
            oPagoDiferidoSup.Operador = new Usuario(oDR["OperId"].ToString(), oDR["OperNombr"].ToString());
            oPagoDiferidoSup.FechaGeneracion = (DateTime)oDR["pag_fecge"];
            oPagoDiferidoSup.Patente = oDR["pag_paten"].ToString();

            oPagoDiferidoSup.Documento = oDR["pag_docum"].ToString();
            oPagoDiferidoSup.Direccion = oDR["pag_direc"].ToString();            
            oPagoDiferidoSup.Localidad = oDR["pag_local"].ToString();
            oPagoDiferidoSup.Nombre = oDR["pag_nombr"].ToString();
            oPagoDiferidoSup.Comentario = oDR["pag_obser"].ToString();
            if(oDR["pag_provi"] != DBNull.Value)
                oPagoDiferidoSup.Provincia = (int)oDR["pag_provi"];
            oPagoDiferidoSup.Supervisor = new Usuario(oDR["SupId"].ToString(), oDR["SupNombr"].ToString());
            oPagoDiferidoSup.Telefono = oDR["pag_telef"].ToString();
            oPagoDiferidoSup.TitularVehiculo = oDR["pag_titul"].ToString();
            oPagoDiferidoSup.Vehiculo = new Vehiculo();
            oPagoDiferidoSup.Vehiculo.Categoria = new CategoriaManual();
            oPagoDiferidoSup.Vehiculo.Color=new VehiculoColor();
            oPagoDiferidoSup.Vehiculo.Marca=new VehiculoMarca();
            oPagoDiferidoSup.Vehiculo.Modelo = new VehiculoModelo();
            if (oDR["pag_color"] != DBNull.Value)
                oPagoDiferidoSup.Vehiculo.Color.Codigo = (int)oDR["pag_color"];
            if (oDR["pag_marca"] != DBNull.Value)
                oPagoDiferidoSup.Vehiculo.Marca.Codigo = (int)oDR["pag_marca"];
            if (oDR["pag_model"] != DBNull.Value)
                oPagoDiferidoSup.Vehiculo.Modelo.Codigo = (int)oDR["pag_model"];
            oPagoDiferidoSup.Vehiculo.Patente = oDR["pag_paten"].ToString();
            oPagoDiferidoSup.Causa = new CausaPagoDiferido();
            if (oDR["pag_causa"] != DBNull.Value)
                oPagoDiferidoSup.Causa.Codigo = Convert.ToInt16(oDR["pag_causa"]);            

            return oPagoDiferidoSup;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda el pago diferido aceptado o rechazado por el supervisor
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="pdSupervisor">PagoDiferidoSupervisor - pdSupervisor</param>
        /// ***********************************************************************************************
        public static void addPagoDiferidoSupervisor(Conexion oConn, PagoDiferidoSupervisor pdSupervisor)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_addPagoDiferidoSupervisor";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = pdSupervisor.Estacion.Numero;
                oCmd.Parameters.Add("@nroPago", SqlDbType.Int).Value = pdSupervisor.NumeroPagoDiferido;
                oCmd.Parameters.Add("@patente", SqlDbType.VarChar, 10).Value = pdSupervisor.Vehiculo.Patente;
                oCmd.Parameters.Add("@docum", SqlDbType.VarChar, 15).Value = pdSupervisor.Documento;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now; // Modificado. Fecha de aceptación
                oCmd.Parameters.Add("@categoria", SqlDbType.TinyInt).Value = pdSupervisor.Vehiculo.Categoria.Categoria;
                oCmd.Parameters.Add("@supid", SqlDbType.VarChar, 10).Value = pdSupervisor.Supervisor.ID;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 30).Value = pdSupervisor.Nombre;
                oCmd.Parameters.Add("@direccion", SqlDbType.VarChar, 30).Value = pdSupervisor.Direccion;
                oCmd.Parameters.Add("@localidad", SqlDbType.VarChar, 20).Value = pdSupervisor.Localidad;
                oCmd.Parameters.Add("@provincia", SqlDbType.Int).Value = pdSupervisor.Provincia;
                oCmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value = pdSupervisor.Telefono;
                if (pdSupervisor.Vehiculo.Marca != null)
                    oCmd.Parameters.Add("@marca", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Marca.Codigo;
                if (pdSupervisor.Vehiculo.Modelo != null)
                    oCmd.Parameters.Add("@modelo", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Modelo.Codigo;
                if (pdSupervisor.Vehiculo.Color != null)
                    oCmd.Parameters.Add("@color", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Color.Codigo;
                oCmd.Parameters.Add("@importe", SqlDbType.Money).Value = pdSupervisor.Importe;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = pdSupervisor.Estado;
                oCmd.Parameters.Add("@fechaGenPD", SqlDbType.DateTime).Value = pdSupervisor.FechaGeneracion;
                oCmd.Parameters.Add("@comentario", SqlDbType.VarChar, 300).Value = pdSupervisor.Comentario;
                oCmd.Parameters.Add("@causa", SqlDbType.TinyInt).Value = pdSupervisor.Causa.Codigo;
                oCmd.Parameters.Add("@titular", SqlDbType.VarChar, 30).Value = pdSupervisor.TitularVehiculo;

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static void updPagoDiferidoSupervisor(Conexion oConn, PagoDiferidoSupervisor pdSupervisor)
        {
            try
            {
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_updPagoDiferidoSupervisor";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = pdSupervisor.Estacion.Numero;
                oCmd.Parameters.Add("@nroPago", SqlDbType.Int).Value = pdSupervisor.NumeroPagoDiferido;
                oCmd.Parameters.Add("@fecge", SqlDbType.DateTime).Value = pdSupervisor.FechaGeneracion; // Agregado
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = DateTime.Now; // Agregado. Actualizar fecha de modificación?
                oCmd.Parameters.Add("@patente", SqlDbType.VarChar, 10).Value = pdSupervisor.Vehiculo.Patente;
                oCmd.Parameters.Add("@docum", SqlDbType.VarChar, 15).Value = pdSupervisor.Documento;                
                oCmd.Parameters.Add("@categoria", SqlDbType.TinyInt).Value = pdSupervisor.Vehiculo.Categoria.Categoria;
                oCmd.Parameters.Add("@supid", SqlDbType.VarChar, 10).Value = pdSupervisor.Supervisor.ID;
                oCmd.Parameters.Add("@nombr", SqlDbType.VarChar, 30).Value = pdSupervisor.Nombre;
                oCmd.Parameters.Add("@direccion", SqlDbType.VarChar, 30).Value = pdSupervisor.Direccion;
                oCmd.Parameters.Add("@localidad", SqlDbType.VarChar, 20).Value = pdSupervisor.Localidad;
                oCmd.Parameters.Add("@provincia", SqlDbType.Int).Value = pdSupervisor.Provincia;
                oCmd.Parameters.Add("@telefono", SqlDbType.VarChar, 15).Value = pdSupervisor.Telefono;
                if (pdSupervisor.Vehiculo.Marca != null)
                    oCmd.Parameters.Add("@marca", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Marca.Codigo;
                if (pdSupervisor.Vehiculo.Modelo != null)
                    oCmd.Parameters.Add("@modelo", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Modelo.Codigo;
                if (pdSupervisor.Vehiculo.Color != null)
                    oCmd.Parameters.Add("@color", SqlDbType.Int).Value = pdSupervisor.Vehiculo.Color.Codigo;
                oCmd.Parameters.Add("@importe", SqlDbType.Money).Value = pdSupervisor.Importe;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = pdSupervisor.Estado;
                
                oCmd.Parameters.Add("@comentario", SqlDbType.VarChar, 300).Value = pdSupervisor.Comentario;
                oCmd.Parameters.Add("@causa", SqlDbType.TinyInt).Value = pdSupervisor.Causa.Codigo;
                oCmd.Parameters.Add("@titular", SqlDbType.VarChar, 30).Value = pdSupervisor.TitularVehiculo;

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
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de causas de pago diferido
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Objeto CausaPagoDiferido</returns>
        /// ***********************************************************************************************
        public static CausaPagoDiferidoL getCausasPagoDiferido(Conexion oConn)
        {
            CausaPagoDiferidoL oCausasPagoDiferido = new CausaPagoDiferidoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_GetCausasPagoDiferido";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCausasPagoDiferido.Add(new CausaPagoDiferido((Byte)oDR["tip_codig"], oDR["tip_descr"].ToString()));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCausasPagoDiferido;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los pagos diferidos generados en la via POR PLACA Y OPERADOR
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Fecha Desde</param>
        /// <param name="jornadaHasta">DateTime - Fecha Hasta</param>
        /// <param name="estado">string - operador</param>
        /// <param name="estado">string - placa</param>
        /// ***********************************************************************************************
        public static DataSet getPagosDiferidosPorPlaca(Conexion oConn, int? zona, int? estacion, int? via, DateTime fechaDesde, DateTime fechaHasta, string operador, string placa)
        {

            DataSet dsPagosDif = new DataSet();
            dsPagosDif.DataSetName = "rptPagosDiferidos_PagosDiferidos"; 

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;
                oCmd.CommandTimeout = 360;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_PagoDiferido_GetPagosDiferidosPorPlaca";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@via", SqlDbType.TinyInt).Value = via;
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = fechaDesde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = fechaHasta;
                oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 10).Value = placa;
                oCmd.Parameters.Add("@operador", SqlDbType.VarChar, 10).Value = operador;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPagosDif, "PagosDiferidos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPagosDif;
        }


        #endregion

        
    }
}

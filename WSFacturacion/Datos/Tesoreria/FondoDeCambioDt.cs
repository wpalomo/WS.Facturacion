using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Microsoft.SqlServer.Server;

namespace Telectronica.Tesoreria
{
    public class FondoDeCambioDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los peajistas habilitados para fondo de cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada Desde</param>
        /// <param name="turno">int? - Turno Desde</param>
        /// <returns>Lista de Fondo de Cambio</returns>
        /// ***********************************************************************************************
        public static FondoDeCambioL getFondoDeCambio(Conexion oConn,
                                int estacion, DateTime jornada,
                                int? turno, string Estado)
        {
            FondoDeCambioL oFondo = new FondoDeCambioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getFondoDeCambio";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = turno;
                oCmd.Parameters.Add("@Estado", SqlDbType.Char,2).Value = Estado;
                //oCmd.Parameters.Add("@Operador", SqlDbType.VarChar,10).Value = Operador; //QUITAR ESTE CAMPO
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFondo.Add(CargarFondoDeCambio(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFondo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los peajistas habilitados para fondo de cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada Desde</param>
        /// <param name="turno">int? - Turno Desde</param>
        /// <returns>Lista de Fondo de Cambio</returns>
        /// ***********************************************************************************************
        public static FondoDeCambio getUltimoFondoDeCambio(Conexion oConn,
                                int estacion, DateTime jornada,
                                int? turno, string Operador)
        {
            FondoDeCambio oFondo = new FondoDeCambio();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getUltimoFondoDeCambio";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = turno;
                oCmd.Parameters.Add("@Operador", SqlDbType.VarChar, 10).Value = Operador;
                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oFondo= CargarFondoDeCambio(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oFondo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Fondo de Cambio
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader</param>
        /// <returns>elemento FondoDeCambio</returns>
        /// ***********************************************************************************************
        private static FondoDeCambio CargarFondoDeCambio(System.Data.IDataReader oDR)
        {
            FondoDeCambio fondo = new FondoDeCambio();
            fondo.Estacion = new Estacion();
            fondo.Estacion.Numero = Convert.ToInt32(oDR["est_codig"]);
            fondo.Estacion.Nombre = oDR["est_nombr"].ToString();
            fondo.Parte = new Parte();
            fondo.Parte.Numero = (int)oDR["par_parte"];
            fondo.Parte.Jornada = (DateTime)oDR["par_fejor"];
            fondo.Parte.Turno = Convert.ToInt16(oDR["par_testu"]);
            fondo.Parte.Peajista = new Usuario();
            fondo.Parte.Peajista.ID = oDR["use_id"].ToString();
            fondo.Parte.Peajista.Nombre = oDR["use_nombr"].ToString();
            fondo.Parte.Peajista.PerfilActivo = new Perfil();
            fondo.Parte.Peajista.PerfilActivo.Codigo = oDR["use_grupo"].ToString();
            fondo.Parte.Peajista.PerfilActivo.Descripcion = oDR["gru_visua"].ToString();
            fondo.Parte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S" ? true : false);
            fondo.Parte.FondodeCambio = oDR["par_fondo"].ToString();
            fondo.FechaAsignacion = Utilitarios.Util.DbValueToNullable < DateTime > (oDR["fon_fecas"]);
            fondo.TesoreroEntrega = new Usuario();
            fondo.TesoreroEntrega.ID = oDR["fon_idten"].ToString();
            fondo.TesoreroEntrega.Nombre = oDR["fon_nomtesen"].ToString();
            fondo.Estado = oDR["fon_estad"].ToString();
            fondo.Monto = Utilitarios.Util.DbValueToNullable < decimal > (oDR["fon_monto"]);
            fondo.Confirmado = oDR["fon_confi"].ToString();
            fondo.FechaDevolucion = Utilitarios.Util.DbValueToNullable<DateTime>(oDR["fon_fecde"]);
            fondo.TesoreroDevolucion = new Usuario();
            fondo.TesoreroDevolucion.ID = oDR["fon_idtde"].ToString();
            fondo.TesoreroDevolucion.Nombre = oDR["fon_nomtesdev"].ToString();
            fondo.Turno = Convert.ToInt16(oDR["par_testu"]);
            fondo.Jornada = (DateTime)oDR["par_fejor"];
            return fondo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Fondo de Cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFondo">FondoDeCambio - Fondo de Cambio a eliminar</param>
        /// ***********************************************************************************************
        public static void delFondoDeCambio(Conexion oConn, FondoDeCambio oFondo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_delFondoDeCambio";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFondo.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oFondo.Parte.Numero;
                oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = oFondo.FechaAsignacion;

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

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Fondo de Cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oFondo">FondoDeCambio - Objeto con la informacion del Fondo de Cambio a agregar
        /// ***********************************************************************************************
        public static void addFondoDeCambio(Conexion oConn, FondoDeCambio oFondo)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_addFondoDeCambio";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFondo.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oFondo.Parte.Numero;
                oCmd.Parameters.Add("@fAsig", SqlDbType.DateTime).Value = oFondo.FechaAsignacion;
                oCmd.Parameters.Add("@idTesoEnt", SqlDbType.VarChar, 10).Value = oFondo.TesoreroEntrega.ID;
                oCmd.Parameters.Add("@monto", SqlDbType.Money).Value = oFondo.Monto;                               

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
        /// Modifica un fondo de cambio en la base de datos
        /// </summary>
        /// <param name="oFondo">FondoDeCambio - Objeto con la informacion del valor de fondo de cambio a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static bool updFondoDeCambio(FondoDeCambio oFondo, Conexion oConn)
        {

            bool ret = false;

            try
            {

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_updFondoDeCambio";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oFondo.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oFondo.Parte.Numero;
                oCmd.Parameters.Add("@fAsig", SqlDbType.DateTime).Value = oFondo.FechaAsignacion;
                oCmd.Parameters.Add("@estado", SqlDbType.Char, 1).Value = oFondo.Estado;
                oCmd.Parameters.Add("@conf", SqlDbType.Char, 1).Value = oFondo.Confirmado;
                if (oFondo.TesoreroDevolucion != null)
                    oCmd.Parameters.Add("@TesoDev", SqlDbType.VarChar, 10).Value = oFondo.TesoreroDevolucion.ID;
                oCmd.Parameters.Add("@fecDev", SqlDbType.DateTime).Value = oFondo.FechaDevolucion;

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
                else
                    ret = true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        public static DataSet getDetalleUltimaEntregaFondoDeCambio(Conexion oConn, FondoDeCambio entrega)
        {
            DataSet dsApr = new DataSet();
            dsApr.DataSetName = "FondoDeCambio_DetalleDS";

            // cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getUltimoFondoDeCambio";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = entrega.Estacion.Numero;
            oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = entrega.Jornada;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = entrega.Turno;
            oCmd.Parameters.Add("@Operador", SqlDbType.VarChar, 10).Value = entrega.TesoreroEntrega.ID;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsApr, "DetalleEntregas");

            // Lo cerramos 
            oCmd = null;
            oDA.Dispose();

            return dsApr;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Prepara el Dataset para mostrar la lista de las Devolucion de fonde de cambio que fuenron confirmados
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="Retiros"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getDetalleDevolucionesConfirmadas(Conexion oConn, FondoDeCambioL Fondos)
        {
            DataSet dsConfirmaDevolucion = new DataSet();
            dsConfirmaDevolucion.DataSetName = "FondoDeCambio_DetalleDS";

            try
            {

                List<SqlDataRecord> NumRet = new List<SqlDataRecord>();
                
                SqlMetaData[] tvp_definition = { new SqlMetaData("parte", SqlDbType.Int),
                                                 new SqlMetaData("esta", SqlDbType.Int),
                                                 new SqlMetaData("fech", SqlDbType.DateTime)};

                foreach (FondoDeCambio item in Fondos)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                    rec.SetInt32(0, item.Parte.Numero);
                    rec.SetInt32(1, item.Estacion.Numero);
                    rec.SetDateTime(2, (DateTime)item.FechaAsignacion);

                    NumRet.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getDetalleDevolucionFondoDeCambio";

                oCmd.Parameters.Add("@lista", SqlDbType.Structured);
                oCmd.Parameters["@lista"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@lista"].TypeName = "ListaFondoDeCambio";
                oCmd.Parameters["@lista"].Value = NumRet;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsConfirmaDevolucion, "DetalleEntregas");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsConfirmaDevolucion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve dataset con los datos de un deposito para imprimir
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="entregas">FondoDeCambioL - entregas de fondo de cambio</param>
        /// <returns>Lista de Depositos</returns>
        /// ***********************************************************************************************
        public static DataSet getDetalleEntregaFondoDeCambio(Conexion oConn, FondoDeCambioL entregas)
        {
            DataSet dsEntregasFC = new DataSet();
            dsEntregasFC.DataSetName = "FondoDeCambio_DetalleDS";

            try
            {

                List<SqlDataRecord> partes = new List<SqlDataRecord>();

                SqlMetaData[] tvp_definition = { new SqlMetaData("n", SqlDbType.Int) };

                foreach (FondoDeCambio item in entregas)
                {
                    SqlDataRecord rec = new SqlDataRecord(tvp_definition);
                    rec.SetInt32(0, item.Parte.Numero);
                    partes.Add(rec);
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getDetalleEntregaFondoDeCambio";

                oCmd.Parameters.Add("@partes", SqlDbType.Structured);
                oCmd.Parameters["@partes"].Direction = ParameterDirection.Input;
                oCmd.Parameters["@partes"].TypeName = "listaPartes";
                oCmd.Parameters["@partes"].Value = partes;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsEntregasFC, "DetalleEntregas");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsEntregasFC;
        }

    }


    public class ValorFondoDeCambioDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los valores para fondo de cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int? - Estacion</param>
        /// <param name="codigo">int? - codigo</param>
        /// <returns>Lista de Valores de Fondo de Cambio</returns>
        /// ***********************************************************************************************
        public static ValorFondoDeCambioL getValoresFondoDeCambio(Conexion oConn, int? estacion, int? codigo, bool conEstaciones)
        {
            ValorFondoDeCambioL valores = new ValorFondoDeCambioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getValoresFondoDeCambio";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@cod", SqlDbType.Int).Value = codigo;
                oCmd.Parameters.Add("@conEstacion", SqlDbType.Char, 1).Value = conEstaciones ? "S" : "N";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    ValorFondoDeCambio oValor = CargarValorFondoDeCambio(oDR);
                    if (oValor != null)
                        valores.Add(oValor);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return valores;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Fondo de Cambio
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader</param>
        /// <returns>elemento FondoDeCambio</returns>
        /// ***********************************************************************************************
        private static ValorFondoDeCambio CargarValorFondoDeCambio(System.Data.IDataReader oDR)
        {
            ValorFondoDeCambio valor = null;
            if (oDR["vfc_ident"] != DBNull.Value)
            {
                valor = new ValorFondoDeCambio();
                valor.Codigo = Convert.ToInt16(oDR["vfc_ident"]);  
                valor.Valor = Convert.ToDecimal(oDR["vfc_valor"]);
            }

            return valor;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Valor de fondo de cambio en la base de datos
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto con la informacion del valor de fondo de cambio a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addValorFondoDeCambio(ValorFondoDeCambio oValorFC, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_addValorFondoDeCambio";

                oCmd.Parameters.Add("@valor", SqlDbType.Money).Value = oValorFC.Valor;

                SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
                parNumero.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (parNumero.Value != DBNull.Value)
                    oValorFC.Codigo = Convert.ToInt32(parNumero.Value);

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
        /// Modifica un Valor de fondo de cambio en la base de datos
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto con la informacion del valor de fondo de cambio a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updValorFondoDeCambio(ValorFondoDeCambio oValorFC, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_updValorFondoDeCambio";

                oCmd.Parameters.Add("@valor", SqlDbType.Money).Value = oValorFC.Valor;
                oCmd.Parameters.Add("@cod", SqlDbType.Int).Value = oValorFC.Codigo;

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
        /// Elimina un Valor de fondo de cambio en la base de datos
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto con la informacion del valor de fondo de cambio a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delValorFondoDeCambio(ValorFondoDeCambio oValorFC, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_delValorFondoDeCambio";

                oCmd.Parameters.Add("@cod", SqlDbType.Int).Value = oValorFC.Codigo;

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
            return;
        }

        public static DataSet getRptEstacionesPorValorFondoDeCambio(Conexion oConn)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptTesoreria_EstacionesValorFondoDeCambioDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getRptEstacionesPorValorFondoDeCambio";

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsExentos, "ValoresEstaciones");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsExentos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// habilita un valor de fondo de cambio para una estacion en la base de datos
        /// </summary>
        /// <param name="int">codEst - estacion donde se habilita el valor de fondo de cambio</param>
        /// <param name="codValor">int - codigo del valor de fondo de cambio</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addEstacionPorValorFondoDeCambio(int codEst, int codValor, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_addEstacionPorValorFondoDeCambio";

                oCmd.Parameters.Add("@codValor", SqlDbType.Int).Value = codValor;
                oCmd.Parameters.Add("@codEst", SqlDbType.Int).Value = codEst;

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
        /// deshabilita un valor de fondo de cambio para una estacion en la base de datos
        /// </summary>
        /// <param name="int">codEst - estacion donde se habilita el valor de fondo de cambio</param>
        /// <param name="codValor">int - codigo del valor de fondo de cambio</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEstacionPorValorFondoDeCambio(int? codEst, int codValor, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_delEstacionesPorValorFondoDeCambio";

                oCmd.Parameters.Add("@codValor", SqlDbType.Int).Value = codValor;
                oCmd.Parameters.Add("@codEst", SqlDbType.Int).Value = codEst;

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
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todas las habilitaciones por estacion para un valor de fondo de cambio
        /// </summary>
        /// <param name="codValor">int - codigo del valor de fondo de cambio</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delEstacionesPorValorFondoDeCambio(int codValor, Conexion oConn)
        {
            delEstacionPorValorFondoDeCambio(null, codValor, oConn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los valores para fondo de cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigo">int? - codigo</param>
        /// <returns>Lista de estaciones por valor de Fondo de Cambio</returns>
        /// ***********************************************************************************************
        public static ValorFCEstacionL getEstacionesPorValorFondoDeCambio(Conexion oConn, int codigo)
        {
            ValorFCEstacionL oEstaciones = new ValorFCEstacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_FondoDeCambio_getValoresFondoDeCambio";
                oCmd.Parameters.Add("@cod", SqlDbType.Int).Value = codigo;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oEstaciones.Add(CargarEstacionesValorFondoDeCambio(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oEstaciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de estaciones de Fondo de Cambio
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader</param>
        /// <returns>elemento FondoDeCambio</returns>
        /// ***********************************************************************************************
        private static ValorFCEstacion CargarEstacionesValorFondoDeCambio(System.Data.IDataReader oDR)
        {
            ValorFCEstacion estacion = new ValorFCEstacion();
            estacion.Estacion = new Estacion();
            estacion.Estacion.Numero = Convert.ToInt32(oDR["est_codig"]);
            estacion.Estacion.Nombre = oDR["est_nombr"].ToString();
            estacion.Habilitado = oDR["Habilitado"].ToString();
            return estacion;
        }
    }
}

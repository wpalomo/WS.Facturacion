using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Validacion
{
    public class ParteValidacionDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroParte">int - numeroParte</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteValidacionL getPartesValidacionPorNumero(Conexion oConn, int numeroParte)
        {
            ParteValidacionL partes = new ParteValidacionL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_getPartes_PorNumero";
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = numeroParte;

                oCmd.CommandTimeout = 60000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    partes.Add(CargarParteValidacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return partes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada</param>     
        /// <param name="estadoParte">string - EstadoParte</param>
        /// <param name="estadoValidacion">string - EstadoValidacion</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteValidacionL getPartesValidacion(Conexion oConn, Estacion estacion, DateTime jornada, string estadoParte, string estadoValidacion,string estadoFallo)
        {
            ParteValidacionL partes = new ParteValidacionL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_getPartes";
                if (estacion.Numero != 0)
                    oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                else
                    oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = null;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@estadParte", SqlDbType.Char, 1).Value = estadoParte;
                oCmd.Parameters.Add("@estadValid", SqlDbType.Char, 1).Value = estadoValidacion;
                oCmd.Parameters.Add("@estadFallo", SqlDbType.Char, 1).Value = estadoFallo;

                oCmd.CommandTimeout = 60000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    partes.Add(CargarParteValidacion(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return partes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static ParteValidacion CargarParteValidacion(System.Data.IDataReader oDR)
        {
            ParteValidacion oParte = new ParteValidacion();
            oParte.EstadoValidacion = new EstadoValidacion { Estado = oDR["EstadoValidacion"].ToString(), EstadoParte = oDR["Estado"].ToString() };           
            oParte.EstadoFallo = oDR["EstadoFallo"].ToString();
            oParte.EstadoReposicion = oDR["EstadoReposicion"].ToString();
            oParte.ReposicionEconomicaPedida = Convert.ToDouble(oDR["ReposicionEconomica"]);
            oParte.ReposicionEconomicaPaga = Convert.ToDouble(oDR["ReposicionEconomicaPaga"]);
            oParte.Numero = (int)oDR["Parte"];
            oParte.Estacion = new Estacion((Byte)oDR["Estacion"], oDR["NombreEstacion"].ToString());
            oParte.Jornada = (DateTime)oDR["Jornada"];
            oParte.Turno = (byte)oDR["Turno"];
            oParte.Peajista = new Usuario(oDR["Peajista"].ToString(), oDR["NombrePeajista"].ToString());
            oParte.TipoParte = oDR["TipoParte"].ToString();
            oParte.NivelParte = oDR["NivelUsuario"].ToString();
            oParte.Validador = new Usuario(oDR["Validador"].ToString(), oDR["NombreValidador"].ToString());
            oParte.AnomaliasSinValidar = Convert.ToInt32(oDR["AnomaliasSinValidar"]);
            oParte.Transitos = Utilitarios.Util.DbValueToNullable<int>(oDR["Transitos"]);
            oParte.SIPs = Convert.ToInt32(oDR["Sips"]);
            oParte.Cancelaciones = Convert.ToInt32(oDR["Cancelaciones"]);
            oParte.DACs = Convert.ToInt32(oDR["DACs"]);
            oParte.Exentos = Convert.ToInt32(oDR["Franquicias"]);
            oParte.TicketsManuales = Convert.ToInt32(oDR["TicketsManuales"]);
            oParte.Violaciones = Convert.ToInt32(oDR["Violaciones"]);
            oParte.PagosDiferidos = Convert.ToInt32(oDR["PagosDiferidos"]);
            oParte.AutorizacionesPaso = Convert.ToInt32(oDR["AutorizacionesDePaso"]);
            oParte.Sobrante = Convert.ToDouble(oDR["Sobrante"]);
            oParte.Faltante = Convert.ToDouble(oDR["Faltante"]);
            oParte.Fallo = Convert.ToDouble(oDR["Fallo"]);
            oParte.PedidoFacturacionFallo = Convert.ToDouble(oDR["FalloAFacturarPedido"]);
            oParte.ReposicionAdicional = Convert.ToDouble(oDR["ReposicionAdicional"]);
            oParte.ReposicionAdicionalPaga = Convert.ToDouble(oDR["ReposicionAdicionalPaga"]);
            oParte.FalloFacturado = Convert.ToDouble(oDR["FalloFacturado"]);
            oParte.Observaciones = oDR["Observaciones"].ToString();
            oParte.EstaLiquidado = oDR["Liquidado"].ToString() == "S" ? true : false;
            oParte.Mante = oDR["Mante"].ToString();
            string vias = "";
            byte minVia =0, maxVia=0;
            if (oDR["MinViaAVI"] != DBNull.Value)
            {
                minVia = (byte)oDR["MinViaAVI"];
                maxVia = (byte)oDR["MaxViaAVI"];
            }
            else if (oDR["MinViaManual"] != DBNull.Value)
            {
                minVia = (byte)oDR["MinViaManual"];
                maxVia = (byte)oDR["MaxViaManual"];
            }
            if (minVia != maxVia)
                vias = minVia.ToString() + "-" + maxVia.ToString();
            else
                vias = minVia.ToString();
            oParte.Vias = vias;
            return oParte;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista las anomalias del parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Estacion - estacion</param>
        /// <param name="parte">int - Parte</param>   
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getTiposAnomalias(Conexion oConn, Estacion estacion, int parte,char verInvisible, char SoloPendientes)
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
                oCmd.CommandText = "Validacion.usp_Parte_getTodasAnomalias";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@VerInvisible", SqlDbType.Char, 1).Value = verInvisible;
                oCmd.Parameters.Add("@SoloPendientes", SqlDbType.Char, 1).Value = SoloPendientes;

                oCmd.CommandTimeout = 60000;

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

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista las anomalias Pendientes del parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">Estacion - estacion</param>
        /// <param name="parte">int - Parte</param>   
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getTodasAnomaliasPendientes(Conexion oConn, Estacion estacion, int parte, char verInvisible)
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
                oCmd.CommandText = "Validacion.usp_Parte_getTodasAnomaliasPendientes";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@VerInvisible", SqlDbType.Char, 1).Value = verInvisible;

                oCmd.CommandTimeout = 60000;

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
        */

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Anomalia</returns>
        /// ***********************************************************************************************
        private static Anomalia CargarAnomalias(System.Data.IDataReader oDR)
        {
            Anomalia oAnomalia = new Anomalia();
            oAnomalia.Codigo = Convert.ToInt32(oDR["anom_codig"]);
            oAnomalia.Descripcion = oDR["anom_descr"].ToString();
            oAnomalia.Pendientes = Convert.ToInt32(oDR["Pendientes"]);
            oAnomalia.Total = Convert.ToInt32(oDR["Total"]);
            return oAnomalia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de bloques de un parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>  
        /// <param name="parte">int - parte</param>
        /// ***********************************************************************************************
        public static DataSet getBloquesParte(Conexion oConn, ParteValidacion parte, bool? noConsultaEvento)
        {
            DataSet dsBloques = new DataSet();
            dsBloques.DataSetName = "Parte_BloquesDS";
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_getBloquesParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                if (noConsultaEvento != null)
                    oCmd.Parameters.Add("@NoConsultaEvento", SqlDbType.Int).Value = (bool)noConsultaEvento ? 1 : 0;

                oCmd.CommandTimeout = 6000;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsBloques, "Bloques");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Calcula los totales por anomalia de los partes, y los totales de los partes por primera vez
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>  
        /// ***********************************************************************************************
        public static void setCalcularJornadaInicio(Conexion oConn, int? estacion, DateTime? jornada, int? parte)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_setCalcularJornadaInicio";  
                if(estacion > 0)
                    oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                if(jornada != null)
                    oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("No existe el registro");
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
        /// permite agregar un comentario al parte
        /// </summary>
        /// ***********************************************************************************************
        
        public static void setComentarParte(Conexion oConn, ParteValidacion parte)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_SetComentarParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@sComentario", SqlDbType.VarChar, 4000).Value = parte.Observaciones;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro") + retval.ToString();
                    if (retval == -101)
                        msg = Traduccion.Traducir("No existe el registro");
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
        /// verifica el estado de validacion del parte
        /// </summary>
        /// ***********************************************************************************************

        public static void getEstadoValidacion(Conexion oConn, ParteValidacion parte)
        {
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_GetEstadoValidacion";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;

                oCmd.CommandTimeout = 60000;

                oDR = oCmd.ExecuteReader();

                if (oDR.Read())
                {
                    parte.EstadoValidacion.Estado = oDR["EstadoValidacion"].ToString();
                    parte.EstadoValidacion.EstadoParte = oDR["EstadoParte"].ToString();
                    parte.EstadoValidacion.Terminal = oDR["Terminal"].ToString();
                    if (oDR["FechaHora"] != DBNull.Value)
                        parte.EstadoValidacion.Fecha = Convert.ToDateTime(oDR["FechaHora"]);
                    parte.EstadoValidacion.IDValidador = oDR["IDVal"].ToString();
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
          
          

        /// ***********************************************************************************************
        /// <summary>
        /// verifica si un parte esta abierto
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>  
        /// <param name="estacion">Int - Codigo de la estacion</param>
        /// <param name="parte">Int - Numero de parte</param>
        /// ***********************************************************************************************
        public static bool esParteAbierto(Conexion oConn, int parte, int estacion)
        {
            bool res = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_EsParteAbierto";
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                //Si es igual a 0, el parte esta abierto
                res = retval == 0;                    

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return res;
        }


        public static void setValidarInvisible(Conexion conn, ParteValidacion parte, string validador)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_setValidarInvisible";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    throw new ErrorSPException(Traduccion.Traducir("No se pudo realizar la validacion invisible"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void setValidarInvisible(Conexion conn, ParteValidacion parte, string validador, int anomalia)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_setValidarInvisible";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@anomalia", SqlDbType.Int).Value = anomalia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    throw new ErrorSPException(Traduccion.Traducir("No se pudo realizar la validacion invisible"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetParteEnValidacion(Conexion conn, ParteValidacion parte, string validador, string host)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_SetParteEnValidacion";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@host", SqlDbType.VarChar, 30).Value = host;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    throw new ErrorSPException(Traduccion.Traducir("No se pudo realizar la validacion del parte"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void SetFinalizarParte(Conexion conn, ParteValidacion parte, string validador, string host)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_SetFinalizarParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = parte.Estacion.Numero;
                oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = parte.Jornada;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte.Numero;
                oCmd.Parameters.Add("@idval", SqlDbType.VarChar, 10).Value = validador;
                oCmd.Parameters.Add("@host", SqlDbType.VarChar, 30).Value = host;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.CommandTimeout = 60000;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    throw new ErrorSPException(Traduccion.Traducir("No se pudo realizar la finalizacion de validacion del parte"));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<string> getListaFotos(Conexion oConn, int idEvento)
        {
            try
            {
                SqlDataReader oDR;
                List<string> ListaFotos = new List<string>();

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Parte_getListaFotos";
                oCmd.Parameters.Add("@event", SqlDbType.Int).Value = idEvento;

                oCmd.CommandTimeout = 60000;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    ListaFotos.Add(oDR["trf_nombr"].ToString());
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

                return ListaFotos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///********************************************************************************
        /// <summary>
        /// Setea la reposicion economica para los partes que poseen faltante
        /// </summary>
        /// <param name="conn">Conexion a Gestion</param>
        /// <param name="oParte">Objeto ParteValidacion</param>
        /// <returns>Bool</returns>
        ///********************************************************************************
        public static bool setReposicionEconomica(Conexion conn, ParteValidacion oParte)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_setReposicion";

                oCmd.Parameters.Add("@Plaza", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oParte.Numero;
                oCmd.Parameters.Add("@LegajoPeajista", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = "E";
                oCmd.Parameters.Add("@Valor", SqlDbType.Money).Value = oParte.Faltante;
                oCmd.Parameters.Add("@LegajoUsuario", SqlDbType.VarChar, 10).Value = oParte.Validador.ID;


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


        ///*************************************************************************************
        /// <summary>
        /// Elimina la reposicion economica que posee un parte especifico, siempre que no se encuentre pagada
        /// </summary>
        /// <param name="conn">Conexion con Gestion</param>
        /// <param name="oParte">Objeto ParteValidacion</param>
        /// <returns>Bool</returns>
        /// ************************************************************************************
        public static bool delReposicionEconomica(Conexion conn, ParteValidacion oParte)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = conn.conection;
                oCmd.Transaction = conn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Rendicion_delReposicionPedida";

                oCmd.Parameters.Add("@Plaza", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oParte.Numero;
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char, 1).Value = "E";
                oCmd.Parameters.Add("@Bolsa", SqlDbType.Int, 1).Value = null; // En la economica no hay bolsa

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
                    if (retval == -101)
                    {
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                    }
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("La Reposición ya se encuentra paga");
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
    }
}

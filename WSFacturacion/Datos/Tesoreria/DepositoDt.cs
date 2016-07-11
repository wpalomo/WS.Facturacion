using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    #region DEPOSITO: Clase de Datos de DepositoDt.

    public class DepositoDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Depositos de ciertas fechas o por guia o numero
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="desde">DateTime? - Desde</param>
        /// <param name="hasta">DateTime? - Hasta</param>
        /// <param name="numero">int? - Numero Deposito</param>
        /// <param name="remito">int? - Numero Remito</param>
        /// <returns>Lista de Depositos</returns>
        /// ***********************************************************************************************
        public static DepositoL getDepositos(Conexion oConn, int estacion, DateTime? desde, DateTime? hasta, string remito, string funda)
        {
            DepositoL oDepositos = new DepositoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_GetDepositos";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Desde", SqlDbType.DateTime).Value = desde;
            oCmd.Parameters.Add("@Hasta", SqlDbType.DateTime).Value = hasta;
            //oCmd.Parameters.Add("@numer", SqlDbType.VarChar).Value = numero;
            oCmd.Parameters.Add("@remit", SqlDbType.VarChar).Value = remito;
            oCmd.Parameters.Add("@funda", SqlDbType.VarChar).Value = funda; 
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oDepositos.Add(CargarDeposito(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oDepositos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Depositos de cierta jornada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="fejor">DateTime? - Desde</param>
        /// <returns>Lista de Depositos</returns>
        /// ***********************************************************************************************
        public static DepositoL getDepositosByJornada(Conexion oConn, int estacion, DateTime? dtJornada)
        {
            DepositoL oDepositos = new DepositoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_getDepositosByJornada";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = dtJornada;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oDepositos.Add(CargarDeposito(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            return oDepositos;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Depositos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Depositos</param>
        /// <returns>elemento Deposito</returns>
        /// ***********************************************************************************************
        private static Deposito CargarDeposito(System.Data.IDataReader oDR)
        {
            Deposito oDeposito = new Deposito();

            oDeposito.Estacion = new Estacion((byte)oDR["dep_coest"], oDR["est_nombr"].ToString());
            oDeposito.Fecha = (DateTime)oDR["dep_fecha"];
            oDeposito.Numero = (int)oDR["dep_numer"];
            oDeposito.FechaJornada = (DateTime)oDR["dep_fejor"];
            
            if (oDR["dep_remit"] != DBNull.Value)
            {
                oDeposito.sRemito = (oDR["dep_remit"]).ToString();
            }
            else
            {
                oDeposito.sRemito = "";
            }

            oDeposito.Funda = oDR["dep_funda"].ToString();
            oDeposito.FundaCheque = oDR["dep_funch"].ToString();
            oDeposito.FundaRepEconomica = oDR["dep_funre"].ToString();
            oDeposito.FundaRepFinanciera = oDR["dep_funrf"].ToString();
            oDeposito.Tesorero = new Usuario(oDR["dep_id"].ToString(), oDR["use_nombr"].ToString());
            oDeposito.Tipo = oDR["dep_tipod"].ToString();
            oDeposito.Monto = (decimal)oDR["dep_total"];
            oDeposito.Llegada = Util.DbValueToNullable<DateTime>(oDR["dep_llega"]);
            oDeposito.Salida = Util.DbValueToNullable<DateTime>(oDR["dep_salid"]);
            oDeposito.EstaRecontado = (oDR["Recontado"].ToString() == "S" ? true : false);
            oDeposito.Verificado = (oDR["dep_verif"].ToString() == "S" ? true : false);


            return oDeposito;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Bolsas no Depositadas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="tipo">string - Tipo Efectivo o Cheque</param>
        /// <param name="agrupado">string - Agrupacion (Turno, Parte, Bolsa)</param>
        /// <returns>Lista de BolsaDepositos</returns>
        /// ***********************************************************************************************
        public static BolsaDepositoL getBolsasNoDepositadas(Conexion oConn, int estacion, string tipo, string agrupado)
        {
            BolsaDepositoL oBolsas = new BolsaDepositoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_GetBolsasNoDepositadas";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@tipo", SqlDbType.Char,1).Value = tipo;
            oCmd.Parameters.Add("@agrupado", SqlDbType.Char, 1).Value = agrupado;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBolsas.Add(CargarBolsaDeposito(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oBolsas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene las bolsas que fueron depositadas pertenecientes a un deposito especifico
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oDeposito">Objeto Deposito</param>
        /// <returns>BolsaDepositoL</returns>
        /// ***********************************************************************************************
        public static BolsaDepositoL getBolsasDepositadas(Conexion oConn, Deposito oDeposito)
        {
            BolsaDepositoL oBolsas = new BolsaDepositoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_getBolsasDepositadas";
            //oCmd.CommandText = "Tesoreria.usp_Deposito_getDetalleTurnoDeposito";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oDeposito.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oDeposito.Numero;
            oCmd.Parameters.Add("@Agrupado", SqlDbType.Char, 1).Value = "";



            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBolsas.Add(CargarBolsaDeposito(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oBolsas; ;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de BolsasDepositos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bolsas sin Depositar</param>
        /// <returns>elemento BolsaDeposito</returns>
        /// ***********************************************************************************************
        private static BolsaDeposito CargarBolsaDeposito(System.Data.IDataReader oDR)
        {
            BolsaDeposito oBolsa = new BolsaDeposito();

            oBolsa.Estacion = new Estacion((byte)oDR["est_codig"], oDR["est_nombr"].ToString());
            oBolsa.Jornada = (DateTime)oDR["par_fejor"];
            oBolsa.Turno = (byte)oDR["par_testu"];
            oBolsa.Tipo = oDR["Tipo"].ToString();
            oBolsa.TipoDescripcion = oDR["TipoDescr"].ToString();
            oBolsa.FechaMovimiento = (DateTime)oDR["moc_fecin"];
            if (oDR["Bolsa"] != DBNull.Value)
            {
                oBolsa.Bolsa = (int)oDR["Bolsa"];
            }
            if (oDR["NumeroApropiacion"] != DBNull.Value)
            {
                oBolsa.NumeroApropiacion = (int)oDR["NumeroApropiacion"];
            }
            oBolsa.MontoEquivalente = (decimal)oDR["MontoEquivalente"];

            // Esta invertido el bool porq si esta Anulada la bolsa no debe estar verificada y esta como No anulada debe quedar verificada
            if (oDR["dem_anulado"] != DBNull.Value)
                oBolsa.Enviada = (oDR["dem_anulado"].ToString() == "N" ? true : false);

            return oBolsa;

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve dataset con los datos de un deposito para imprimir
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="numero">int - Numero Deposito</param>
        /// <param name="agrupado">string - Agrupacion (Turno, Parte, Bolsa)</param>
        /// <returns>Lista de Depositos</returns>
        /// ***********************************************************************************************
        public static DataSet getDepositoDetalle(Conexion oConn, int estacion, int numero, string agrupado)
        {
            DataSet dsDeposito = new DataSet();
            dsDeposito.DataSetName = "Deposito_DetalleDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_getDetalleDeposito";
            //oCmd.CommandText = "Tesoreria.usp_Deposito_getDetalleTurnoDeposito";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = numero;
            oCmd.Parameters.Add("@Agrupado", SqlDbType.Char, 1).Value = agrupado;
            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsDeposito, "DetalleDeposito");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
            return dsDeposito;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Deposito (Cabecera)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oDeposito">Deposito - Objeto con la informacion del deposito a agregar
        ///                     le asigna el numero de movimiento</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addDeposito(Conexion oConn, Deposito oDeposito)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_addDeposito";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oDeposito.Estacion.Numero;
            oCmd.Parameters.Add("@tipod", SqlDbType.Char,1).Value = oDeposito.Tipo;
            oCmd.Parameters.Add("@login", SqlDbType.VarChar, 10).Value = oDeposito.Tesorero.ID;
            oCmd.Parameters.Add("@remit", SqlDbType.VarChar, 11).Value = oDeposito.sRemito;
            oCmd.Parameters.Add("@total", SqlDbType.Money).Value = oDeposito.Monto;
            oCmd.Parameters.Add("@funda", SqlDbType.VarChar).Value = oDeposito.Funda;
            oCmd.Parameters.Add("@llega", SqlDbType.DateTime).Value = oDeposito.Llegada;
            oCmd.Parameters.Add("@salid", SqlDbType.DateTime).Value = oDeposito.Salida;
            oCmd.Parameters.Add("@fejor", SqlDbType.DateTime).Value = oDeposito.FechaJornada;
            oCmd.Parameters.Add("@funch", SqlDbType.VarChar).Value = oDeposito.FundaCheque;
            oCmd.Parameters.Add("@funre", SqlDbType.VarChar).Value = oDeposito.FundaRepEconomica;
            oCmd.Parameters.Add("@funrf", SqlDbType.VarChar).Value = oDeposito.FundaRepFinanciera;
            
            SqlParameter parNumero = oCmd.Parameters.Add("@numer", SqlDbType.Int);
            parNumero.Direction = ParameterDirection.Output;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parNumero.Value != DBNull.Value)
            {
                oDeposito.Numero = Convert.ToInt32(parNumero.Value);
            }

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
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// 
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oDeposito"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool updDeposito(Conexion oConn, Deposito oDeposito)
        {
            bool ret = false;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_updDeposito";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oDeposito.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oDeposito.Numero;
            oCmd.Parameters.Add("@tesorero", SqlDbType.VarChar,10).Value = oDeposito.Tesorero.ID;
            oCmd.Parameters.Add("@fechaMod", SqlDbType.DateTime).Value = DateTime.Now;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;
            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    ret = false;
                }
                else
                {
                    ret = true;
                }

            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Pone El estado de las Bolsas seleccionadas como verificaadas
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="oBolsa">BolsaDeposito</param>
        /// <returns>bool</returns>
        /// ***********************************************************************************************
        public static bool updEstadoBolsaDepositada(Conexion oConn, BolsaDeposito oBolsa, int NumerDep)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_updDepositoDetalle";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oBolsa.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oBolsa.NumeroApropiacion;
            oCmd.Parameters.Add("@numDepo", SqlDbType.Int).Value = NumerDep;
            string anulada = "N";
            if (!oBolsa.Enviada)
                anulada = "S";

            oCmd.Parameters.Add("@anulada", SqlDbType.Char, 1).Value = anulada;

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
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Detalle del Deposito 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oBolsa">BolsaDeposito - Objeto con la informacion del detaalle del deposito a agregar
        ///                     marca depositado todas las bolsas que coincidan con este registro</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addDepositoDetalle(Conexion oConn, BolsaDeposito oBolsa)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_addDepositoDetalle";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oBolsa.Estacion.Numero;
            oCmd.Parameters.Add("@iddep", SqlDbType.Int).Value = oBolsa.NumeroDeposito;
            oCmd.Parameters.Add("@tipod", SqlDbType.Char, 1).Value = oBolsa.Tipo;
            oCmd.Parameters.Add("@jornada", SqlDbType.DateTime).Value = oBolsa.Jornada;
            oCmd.Parameters.Add("@turno", SqlDbType.TinyInt).Value = oBolsa.Turno;
            //oCmd.Parameters.Add("@tipom", SqlDbType.Char,1).Value = oBolsa.Movimiento;
            //oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = oBolsa.Parte;
            oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = oBolsa.Bolsa;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oBolsa.NumeroApropiacion;
                
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
            return ret;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Deposito
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oDeposito">Deposito - Deposito a anular</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delDeposito(Conexion oConn, Deposito oDeposito)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_delDeposito";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oDeposito.Estacion.Numero;
            oCmd.Parameters.Add("@numer", SqlDbType.Int).Value = oDeposito.Numero;

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
            return ret;
        }



        ///*****************************************************************************************************
        /// <summary>
        /// Obtiene las bolsas faltantes de depositar para jornadas en las que ya se realizo un deposito
        /// </summary>
        /// <param name="oConn">Conexion</param>
        /// <param name="iEstacion">Numero de Estacion</param>
        /// <param name="sEstado">Estado</param>
        /// <param name="FechaDesde">Fecha Desde</param>
        /// <param name="FechaHasta">Fecha Hasta</param>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static BolsaDepositoL getBolsasFaltantesDeDepositar(Conexion oConn, int iEstacion, string sEstadoDeposito, string sEstadoReposicion, DateTime? FechaDesde, DateTime? FechaHasta, int? bolsa)
        {
            BolsaDepositoL oBolsas = new BolsaDepositoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Deposito_getBolsasFaltantesDeposito";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;
            oCmd.Parameters.Add("@estadoDep", SqlDbType.Char, 1).Value = sEstadoDeposito;
            oCmd.Parameters.Add("@estadoRep", SqlDbType.Char, 1).Value = sEstadoReposicion;
            oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = FechaDesde;
            oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = FechaHasta;
	        oCmd.Parameters.Add("@bolsa", SqlDbType.Int).Value = bolsa;


            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBolsas.Add(CargarBolsasFaltantes(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oBolsas; 
        }

        ///*************************************************************************
        /// <summary>
        /// Carga Objeto Con las bolsas Faltantes
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        ///*************************************************************************
        private static BolsaDeposito CargarBolsasFaltantes(SqlDataReader oDR)
        {
            BolsaDeposito oBolsaDeposito = new BolsaDeposito();

            oBolsaDeposito.Estacion = new Estacion(Convert.ToInt32(oDR["est_codig"]), Convert.ToString(oDR["est_nombr"]));
            oBolsaDeposito.Jornada = Convert.ToDateTime(oDR["par_fejor"]);
            oBolsaDeposito.Turno = Convert.ToInt32(oDR["par_testu"]);
            oBolsaDeposito.FechaMovimiento = Convert.ToDateTime(oDR["moc_fecin"]);
            oBolsaDeposito.Tipo = Convert.ToString(oDR["Tipo"]); 
            oBolsaDeposito.TipoDescripcion = Convert.ToString(oDR["TipoDescr"]);
            oBolsaDeposito.Parte = Convert.ToInt32(oDR["Parte"]);
            oBolsaDeposito.Bolsa = Convert.ToInt32(oDR["Bolsa"]);
            oBolsaDeposito.NumeroApropiacion = Convert.ToInt32(oDR["NumeroApropiacion"]);
            oBolsaDeposito.MontoEquivalente = Convert.ToDecimal(oDR["MontoEquivalente"]);
            oBolsaDeposito.Peajista = Convert.ToString(oDR["Peajista"]);
            oBolsaDeposito.BolsaDepositada = (Convert.ToString(oDR["EstadoDeposito"]) == "S" ? true : false);

            
            //DATOS DE LA REPOSICION PEDIDA            
            if(oDR["EstReposicion"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion = new ReposicionPedida();
                oBolsaDeposito.Reposicion.Estacion = new Estacion(Convert.ToInt32(oDR["EstReposicion"]),Convert.ToString(oDR["EstRepoNombre"]));                
            }           
            if(oDR["FechaReposicion"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.FechaIngreso =  Convert.ToDateTime(oDR["FechaReposicion"]);
            }

            if(oDR["MontoReposicion"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.Monto =  Convert.ToDecimal(oDR["MontoReposicion"]);
            }

            if (oDR["BolsaReposicion"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.bolsa = Convert.ToString(oDR["BolsaReposicion"]);
            }

            if (oDR["ParteReposicion"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.Parte = new Parte();
                oBolsaDeposito.Reposicion.Parte.Numero = Convert.ToInt32(oDR["ParteReposicion"]);
            }

            //DATOS DEL PAGO DE LA REPOSICION           
            if(oDR["EstReposicionPaga"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.PagoReposicion = new MovimientoCajaReposicion();
                oBolsaDeposito.Reposicion.PagoReposicion.Estacion = new Estacion(Convert.ToInt32(oDR["EstReposicionPaga"]),Convert.ToString(oDR["EstRepoPagaNombre"]));                
            }
            if(oDR["FechaReposiPaga"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.PagoReposicion.FechaHoraIngreso = Convert.ToDateTime(oDR["FechaReposiPaga"]);
            }
            if(oDR["MontoReposiPaga"] != DBNull.Value)
            {
                oBolsaDeposito.Reposicion.PagoReposicion.MontoTotal = Convert.ToDecimal(oDR["MontoReposiPaga"]);
            }


            return oBolsaDeposito;
                        
        }


    }

    #endregion
}
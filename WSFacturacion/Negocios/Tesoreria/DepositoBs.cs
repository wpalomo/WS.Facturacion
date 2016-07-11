using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Validacion;
using Telectronica.Utilitarios;
using System.Data.SqlClient;


namespace Telectronica.Tesoreria
{
    #region DEPOSITOBS: Metodos de la Clase DepositoBs.

    public class DepositoBs
    {
        #region DEPOSITO: Metodos de Deposito.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Depositos
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="desde">DateTime? - Desde</param>
        /// <param name="hasta">DateTime? - Hasta</param>
        /// <param name="numero">int? - Numero de Deposito</param>
        /// <param name="remito">int? - Numero de Remito</param>
        /// <returns>Lista de Depositos</returns>
        /// ***********************************************************************************************
        public static DepositoL getDepositos(int estacion, DateTime? desde, DateTime? hasta, string remito, string funda )
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    DepositoL oDepositos =  DepositoDt.getDepositos(conn, estacion, desde, hasta, remito, funda);
                    llenarEstadoJornada(oDepositos);
                    return oDepositos;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Para saber en tesoreria si la jornada estuvo cerrada o no.
        /// Preguntar desde el cliente (Silverlight), me sobrecarga de callbacks la pantalla.  
        /// </summary>
        /// <param name="oDepositos"></param>
        private static void llenarEstadoJornada(DepositoL oDepositos)
        {
            foreach (Deposito deposito in oDepositos)
            {
                try
                {
                    deposito.jornadaCerrada = JornadaBs.EstaCerrada(deposito.Estacion.Numero, deposito.FechaJornada);
                }
                catch (SqlException)//Si tenemos un Error SqlException significa que se produjo un error de conexion o desde el sql
                {
                    deposito.jornadaCerrada = null; 
                }
                
                
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Bolsas sin Depositar
        /// Agrupadas de acuerdo a la configuracion del cliente de tesoreria
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="tipo">string - Tipo (Efectivo, Cheque)</param>
        /// <returns>Lista de BolsasDepositos sin Depositar</returns>
        /// ***********************************************************************************************
        public static BolsaDepositoL getBolsasSinDepositar(int estacion, string tipo)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    string agrupado;
                    switch (ConfiguracionClienteTesoreria.AgrupacionDeposito)
                    {
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmTurno:
                            agrupado = "T";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmParte:
                            agrupado = "P";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmBolsa:
                            agrupado = "B";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmNada:
                            agrupado = "";
                            break;
                        default:
                            agrupado = "T";
                            break;
                    }
                    return DepositoDt.getBolsasNoDepositadas(conn, estacion, tipo, agrupado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        ///*********************************************************************************************************
        /// <summary>
        /// Obtiene las bolsas faltantes de depositar para jornadas en las que ya se realizo un deposito
        /// </summary>
        /// <param name="iEstacion">Estacion</param>
        /// <param name="sEstado">Estado</param>
        /// <param name="FechaDesde">Fecha Desde</param>
        /// <param name="FechaHasta">Fecha Hasta</param>
        /// <returns></returns>
        ///*********************************************************************************************************
        public static BolsaDepositoL getBolsasFaltantesDeDepositar(int iEstacion , string sEstadoDeposito , string sEstadoReposicion , DateTime? FechaDesde , DateTime? FechaHasta, int? bolsa)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return DepositoDt.getBolsasFaltantesDeDepositar(conn, iEstacion, sEstadoDeposito, sEstadoReposicion, FechaDesde, FechaHasta,bolsa);               
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Bolsas Depositadas
        /// ***********************************************************************************************
        public static BolsaDepositoL getBolsasDepositadas(Deposito oDeposito)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return DepositoDt.getBolsasDepositadas(conn, oDeposito);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de un deposito
        /// </summary>
        /// <param name="oDeposito">Deposito</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleDeposito(Deposito oDeposito)
        {          
            return getDetalleDeposito(oDeposito.Estacion.Numero, oDeposito.Numero);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de un deposito
        /// </summary>
        /// <param name="estacion">int - Codigo de Estacion</param>
        /// <param name="numero">int - numero deposito</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleDeposito( int estacion, int numero)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    string agrupado;
                    switch (ConfiguracionClienteTesoreria.AgrupacionDeposito)
                    {
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmTurno:
                            agrupado = "T";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmParte:
                            agrupado = "P";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmBolsa:
                            agrupado = "B";
                            break;
                        case ConfiguracionClienteTesoreria.enmAgrupacionDeposito.enmNada:
                            agrupado = "";
                            break;
                        default:
                            agrupado = "T";
                            break;
                    }
                    return DepositoDt.getDepositoDetalle(conn, estacion, numero, agrupado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Deposito
        /// </summary>
        /// <param name="oDeposito">Deposito - Deposito a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addDeposito(Deposito oDeposito)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Asignamos Tesorero
                    oDeposito.Tesorero = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre()); 

                    DepositoDt.addDeposito(conn, oDeposito);

                    //Grabamos Bolsas
                    foreach (BolsaDeposito item in oDeposito.Bolsas)
                    {
                        //Asignamos el numero de deposito
                        item.NumeroDeposito = oDeposito.Numero;
                        if (item.MontoEquivalente > 0)
                        {
                            DepositoDt.addDepositoDetalle(conn, item);
                        }
                    }

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaDeposito(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oDeposito),
                                                           getAuditoriaDescripcion(oDeposito)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

         /// ***********************************************************************************************
        /// <summary>
        /// Agregar Deposito
        /// </summary>
        /// <param name="oDeposito">Deposito - Deposito a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void VerificarDeposito(Deposito oDeposito)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);
                    
                    DepositoDt.updDeposito(conn, oDeposito);

                    //Confirmamos las bolsas que fueron confirmadas en la pantalla 
                    foreach (BolsaDeposito item in oDeposito.Bolsas)
                    {
                        DepositoDt.updEstadoBolsaDepositada(conn, item, oDeposito.Numero);
                        
                    }

                    //Grabamos auditoria
                    //AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaDeposito(),
                    //                                       "A",
                    //                                       getAuditoriaCodigoRegistro(oDeposito),
                    //                                       getAuditoriaDescripcion(oDeposito)),
                    //                                       conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminar Deposito
        /// </summary>
        /// <param name="oDeposito">Deposito - Deposito a eliminar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delDeposito(Deposito oDeposito)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);
                    
                    DepositoDt.delDeposito(conn, oDeposito);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaDeposito(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oDeposito),
                                                           getAuditoriaDescripcion(oDeposito)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene un valor booleando que indica si para la fecha pasada por parámetro existen partes sin liquidar
        /// </summary>
        /// <param name="iEstacion"></param>
        /// <param name="dtFechaJornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static string ExistenPartesSinLiquidar(int? iEstacion, DateTime dtFechaJornada, Conexion conn)
        {
            ParteL partesSinLiquidar = (RendicionDt.getPartesSinLiquidarPorJornada(conn, iEstacion, dtFechaJornada));

            string sMensaje = "";

            if (partesSinLiquidar.Count > 0)
            {
                sMensaje = Traduccion.Traducir("Los siguientes cajeros tienen partes sin liquidar:") + Environment.NewLine;

                foreach (Parte unParte in partesSinLiquidar)
                {
                    sMensaje += (unParte.Numero + " - " + unParte.Peajista.Nombre + Environment.NewLine);
                }
            }

            BloqueL bloquesSinLiquidar = (RendicionDt.getBloquesSinLiquidarPorJornada(conn, iEstacion, dtFechaJornada));

            if (bloquesSinLiquidar.Count > 0)
            {
                sMensaje += Traduccion.Traducir("Los siguientes cajeros tienen bloques sin liquidar:") + Environment.NewLine;

                foreach (Bloque unBloque in bloquesSinLiquidar)
                {
                    sMensaje += (unBloque.Peajista.Nombre + Environment.NewLine);
                }
            }
            return sMensaje;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene un valor que indica si existen depositos para la jornada especificada por parámetros
        /// </summary>
        /// <param name="iEstacion"></param>
        /// <param name="dtFechaJornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static string ExistenDepositosParaLaJornada(int iEstacion, DateTime dtFechaJornada, Conexion conn)
        {
            DepositoL depositos = DepositoDt.getDepositosByJornada(conn, iEstacion, dtFechaJornada);
            string sMensaje = "";

            if (depositos.Count > 0)
            {
                sMensaje = Traduccion.Traducir("La jornada") + " " + dtFechaJornada + " " + Traduccion.Traducir("ya fue depositada.");
            }

            return sMensaje;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Verifica si la jornada es apta para realizar un depósito
        /// </summary>
        /// <param name="iEstacion"></param>
        /// <param name="dtFechaJornada"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string VerificarJornada(int? iEstacion, DateTime dtFechaJornada)
        {
            string sResult;
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                sResult = ExistenPartesSinLiquidar(iEstacion, dtFechaJornada, conn);
                //Si no existieron partes sin liquidar, verifico la existencia de depositos para la jornada dada
                if(string.IsNullOrEmpty(sResult))
                {
                    if(iEstacion == null)
                    {
                        throw new Exception("Error interno: La estación no puede ser null");
                    }
                    
                    //sResult = ExistenDepositosParaLaJornada((int)iEstacion, dtFechaJornada, conn);
                }
            }
            return sResult;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista con todos los estados de Deposito posibles
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static EstadoDepositoL ObtenerEstadosDeDepositosPosibles()
        {
            var estadosDeDeposito = new EstadoDepositoL();
			//Sin deposito
            var estado = new EstadoDeposito("S"); 
            estadosDeDeposito.Add(estado);
			//Depositado
            estado = new EstadoDeposito("D");
            estadosDeDeposito.Add(estado);
			

			return estadosDeDeposito;
        }
        
        #endregion

        #region AUDITORIA
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaDeposito()
        {
            return "DEB";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Deposito oDeposito)
        {
            return oDeposito.Numero.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Deposito oDeposito)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Remisión en Banco."));

            AuditoriaBs.AppendCampo(sb, "Tipo", oDeposito.TipoDescripcion);
            //AuditoriaBs.AppendCampo(sb, "Moneda", oDeposito.Moneda.Desc_Moneda);
            if (oDeposito.sRemito != string.Empty)
                AuditoriaBs.AppendCampo(sb, "Remito", oDeposito.sRemito);
            if (oDeposito.Funda != string.Empty && oDeposito.Funda != null)
                AuditoriaBs.AppendCampo(sb, "Funda", oDeposito.Funda.ToString());
            if (oDeposito.Monto > 0)
                AuditoriaBs.AppendCampo(sb, "Monto Total", oDeposito.Monto.ToString("C"));
            if (oDeposito.Bolsas != null)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (BolsaDeposito item in oDeposito.Bolsas)
                {
                    AuditoriaBs.AppendCampo(sb2, "", item.Jornada.ToShortDateString());
                    AuditoriaBs.AppendCampo(sb2, "Turno", item.Turno.ToString());
                    //AuditoriaBs.AppendCampo(sb2, "", item.MovimientoDescripcion);
                    //if (item.Parte != null)
                    //    AuditoriaBs.AppendCampo(sb2, "Parte", item.Parte.ToString());
                    if (item.Bolsa != null)
                        AuditoriaBs.AppendCampo(sb2, "Bolsa", item.Bolsa.ToString());
                    sb2.Append("\n");
                }
                AuditoriaBs.AppendCampo(sb, "Bolsas", sb2.ToString());
            }


            return sb.ToString();
        }
        #endregion

    }
    #endregion
}

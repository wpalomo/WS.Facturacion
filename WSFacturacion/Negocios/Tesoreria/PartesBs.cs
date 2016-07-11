using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Validacion;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    public class PartesBs
    {
        #region PARTES: Metodos de la Clase Partes.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes de un peajista en un periodo
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="operador">string - ID del peajista</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="status">string - Status de los Partes</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteL getPartes(int estacion, DateTime jornadaDesde, DateTime jornadaHasta, string operador, int? turnoDesde, int? turnoHasta, string status)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getPartes(conn, estacion, jornadaDesde, jornadaHasta, operador, turnoDesde, turnoHasta, status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un Parte
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int - Numero de Parte</param>
        /// <returns>Objeto Parte</returns>
        /// ***********************************************************************************************
        public static Parte getParte(int estacion, int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getParte(conn, estacion, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Peajistas que trabajaron en un periodo
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="status">string - Status de los Partes</param>
        /// <returns>Lista de Operadores</returns>
        /// ***********************************************************************************************
        public static UsuarioL getOperadores(int estacion, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, string status)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getOperadores(conn, estacion, jornadaDesde, jornadaHasta, turnoDesde, turnoHasta, status);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static UsuarioL getOperadores(int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getOperadores(conn, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Peajistas Habilitado con el parte que trabajaron en un dia
        /// </summary>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <param name="turno">int? - Turno</param>
        /// <returns>Lista de Operadores con su parte</returns>
        /// ***********************************************************************************************
        public static UsuarioParteL getOperadoresParte(DateTime jornada, int? turno)
        {
            return getOperadoresParte(ConexionBs.getNumeroEstacion(), jornada, turno);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Peajistas Habilitado con el parte que trabajaron en un dia
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <param name="turno">int? - Turno</param>
        /// <returns>Lista de Operadores con su parte</returns>
        /// ***********************************************************************************************
        public static UsuarioParteL getOperadoresParte(int estacion, DateTime jornada, int? turno)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getOperadoresParte(conn, estacion, jornada, turno);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Asignar Partes
        /// </summary>
        /// <param name="oPartes">ParteL - Lista de los partes a asignar
        /// <returns>Lista de Partes con problemas</returns>
        /// ***********************************************************************************************
        public static ParteL addPartes(ParteL oPartes)
        {
            ParteL oPartesMal = new ParteL();
            //Si la lista es vacia no hacemos nada
            if (oPartes.Count > 0)
            {
                try
                {
                    //iniciamos una transaccion
                    using (Conexion conn = new Conexion())
                    {
                        //Siempre en la plaza, con transaccion
                        conn.ConectarPlaza(true);

                        //Verficamos Jornada Cerrada
                        JornadaBs.VerificaJornadaAbierta(conn, oPartes[0].Jornada);

                        //Verficamos Supervisor a Cargo
                        if (!CambiosBs.verificaSupervisorACargo())
                        {
                            throw new ErrorJornadaCerrada(Traduccion.Traducir("La Asiganción de Partes debe ser hecha por el Supervisior a Cargo de la Estación"));
                        }

                        //Por cada parte asignamos el parte
                        foreach (Parte oParte in oPartes)
                        {
                            if (PartesDt.addParte(conn, oParte, ConexionBs.getUsuario()))
                            {
                                //Grabamos auditoria
                                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaParte(),
                                                                       "A",
                                                                       getAuditoriaCodigoRegistro(oParte),
                                                                       getAuditoriaDescripcion(oParte)),
                                                                       conn);
                            }
                            else
                            {
                                //Agregamos el parte a la lista de problemas
                                oPartesMal.Add(oParte);
                            }
                        }

                        //Grabo OK hacemos COMMIT
                        conn.Finalizar(true);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return oPartesMal;
        }

		public static bool delCruceDinero(int coest, int identity)
		{
			bool result;
			try
			{
				using (Conexion con = new Conexion())
				{
					con.ConectarPlaza(true);
					result = PartesDt.delCruceDinero(con, coest, identity);
					con.Finalizar(result);
				}
			}
			catch (Exception e)
			{
				throw e;
				return false;
				
			}
			return result;
		}
		public static MovimientoCajaAjusteL getCrucesDinero(int Coest, DateTime? desde, DateTime? hasta)
		{
			MovimientoCajaAjusteL movs = new MovimientoCajaAjusteL();
			try
			{
				using (Conexion con = new Conexion())
				{
					con.ConectarPlaza(false);
					movs = PartesDt.getCrucesDinero(con, Coest, desde, hasta);

				}
			}
			catch (Exception e)
			{
				
				throw e;
			}
			return movs;
		}

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza el campo que indica si se mostro la advertencia de diferencia entre lo liquidado y lo facturado
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="parte"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool NotificarMostroAdvertencia(int estacion, int parte)
        {            
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);
                PartesDt.updPartesMostroDiferencia(conn, estacion, parte);
                conn.Finalizar(true);
                return true;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaParte()
        {
            return "PAR";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Parte oParte)
        {
            return oParte.Numero.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Parte oParte)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Jornada", oParte.JornadaString);
            AuditoriaBs.AppendCampo(sb, "Turno", oParte.Turno.ToString());
            AuditoriaBs.AppendCampo(sb, "Peajista", oParte.Peajista.Nombre);

            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region PARTESTOTALES: Obtener total de partes por estado.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los totales de Partes por cada estado
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de PartesTotales</returns>
        /// ***********************************************************************************************
        public static PartesTotalesL getPartesTotales(int estacion, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getPartesTotales(conn, estacion, jornadaDesde, jornadaHasta, turnoDesde, turnoHasta);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


		public static void ObtenerFaltanteParte(int estacion, int parte, out decimal sobrante, out decimal faltante, out decimal reposicion, out bool jornadaCerrada)
		{
			try
			{
				using (Conexion con = new Conexion())
				{
					
					con.ConectarConsolidado(false);
					//con.ConectarPlaza(false);
					PartesDt.obtenerFaltanteParte(con, estacion,parte, out sobrante, out faltante,out reposicion, out jornadaCerrada);
					
				}
			}
			catch (Exception ex)
			{
				throw ex;
			
			}
		}

		/// <summary>
		/// Agrega el cruce de dinero a mocaja (j)
		/// </summary>
		public static bool addCruceDinero(int coest, string usuario, int parteOrigen, int parteDestino, decimal monto, string comentario)
		{
			bool result;
			try
			{
				using (Conexion con = new Conexion())
				{

					con.ConectarPlaza(true);
					 result =PartesDt.addCruceDinero(con,coest,usuario,parteOrigen,parteDestino,monto,comentario);
					con.Finalizar(result);
				}
			}
			catch (Exception ex)
			{
				throw ex;

			}
			return result;
		}
        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes pendientes (no liquidados o no validados)
        /// </summary>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="operador">string - id de Operador</param>
        /// ***********************************************************************************************
        public static ParteL getPartesPendientes( int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta,
                int? zona, int? estacion, int? turnoDesde, int? turnoHasta,
                string operador)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return PartesDt.getPartesPendientes(conn, parte, jornadaDesde, jornadaHasta, zona, estacion, turnoDesde, turnoHasta, operador,
                        ConfigValidacion.getEstadoPartePendiente(ConexionBs.getGSToEstacion()));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes para liqudiacion (los del periodo y los no liquidados del peajista
        /// </summary>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="cantidadMaxima">int? - Maxima Cantidad de Registros (para que no de error si son demasiados)</param>
        /// ***********************************************************************************************
        public static ParteL getPartesLiquidacion(DateTime? jornadaDesde, DateTime? jornadaHasta,
                int? estacion, int? turnoDesde, int? turnoHasta,
                string operador, int? cantidadMaxima)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);
                    return PartesDt.getPartesLiquidacion(conn, jornadaDesde, jornadaHasta, estacion, turnoDesde, turnoHasta, operador, cantidadMaxima);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

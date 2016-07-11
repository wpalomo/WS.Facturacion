using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    public class ApropiacionBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la cabecera de una apropiacion.
        /// </summary>
        /// <param name="oApropiacion">Apropiacion que se grabara</param>
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        public static void AddApropiacion(Apropiacion oApropiacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    if (oApropiacion.Estacion == null)
                        oApropiacion.Estacion = new Estacion();
                    if (oApropiacion.Estacion.Numero == 0)
                        oApropiacion.Estacion.Numero = ConexionBs.getNumeroEstacion();

                    // Controlamos unicidad del numero de bolsa
                    ApropiacionL oAprs = ApropiacionDt.getApropiaciones(conn, null, null, oApropiacion.Bolsa, oApropiacion.Estacion.Numero);
                    if (oAprs.Count > 0)
                        throw new Telectronica.Errores.ErrorParteStatus("El número de Depósito ya Existe");

                    //Agregamos la Cabecera de la apropiacion
                    ApropiacionDt.addApropiacion(conn, oApropiacion);


                    //Agregamos el detalle de la apropiacion
                    foreach (BolsaApropiacion oApropDet in oApropiacion.apropiacionDetalleL)
                    {
                        ApropiacionDt.addApropiacionDetalle(conn, oApropiacion.Estacion, oApropiacion.NumeroApropiacion, oApropDet);
                    }

                    if (oApropiacion.Detalle != null)
                    {
                        foreach (MovimientoCajaDetalle item in oApropiacion.Detalle)
                        {
                            if (item.Cantidad > 0)
                                ApropiacionDt.addApropiacionDetalle(conn, item, oApropiacion);
                        }
                    }

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaApropiacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oApropiacion),
                                                           getAuditoriaDescripcion(oApropiacion)),
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
        /// Agrega la apropiacion de un movimiento de caja
        /// </summary>
        /// <param name="oApropiacion">Apropiacion que se grabara</param>
        /// <returns>nada</returns>
        /// ***********************************************************************************************
        public static void AddApropiacion(MovimientoCaja oMovim, int bolsa, Conexion conn, string tipoApropiacion)
        {
            AddApropiacion(oMovim, bolsa, conn, tipoApropiacion, oMovim.MontoTotal);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// grega la apropiacion de un movimiento de caja (Sobrecargado)
        /// </summary>
        /// <param name="oMovim"></param>
        /// <param name="bolsa"></param>
        /// <param name="conn"></param>
        /// <param name="tipoApropiacion"></param>
        /// <param name="montoTotal"></param>
        /// ***********************************************************************************************
        public static void AddApropiacion(MovimientoCaja oMovim, int bolsa, Conexion conn, string tipoApropiacion, decimal montoTotal)
        {
            // Preparamos y grabamos la cabecera y el detalle de la apropiacion:
            Apropiacion oApropiacion = new Apropiacion();
            oApropiacion.Bolsa = bolsa;
            oApropiacion.Estacion = oMovim.Estacion;
            oApropiacion.Fecha = DateTime.Now;
            oApropiacion.Total = montoTotal;
            oApropiacion.Usuario = oMovim.Peajista;
            oApropiacion.Jornada = oMovim.Parte.Jornada;
            oApropiacion.Turno = oMovim.ParteTurno;
            oApropiacion.TipoApropiacion = tipoApropiacion;

            BolsaApropiacion oApropDet = new BolsaApropiacion();
            oApropDet.NumeroMovimientoCaja = oMovim.NumeroMovimiento;
            oApropDet.MontoEquivalente = montoTotal;
            oApropDet.Jornada = oMovim.Parte.Jornada;
            oApropDet.Turno = oMovim.ParteTurno;
            oApropDet.Bolsa = bolsa;
            oApropDet.Estacion = oMovim.Estacion;
            oApropDet.Parte = oMovim.Parte.Numero;
            //TODO ver detalle de la descripcion
            oApropDet.MovimientoDescripcion = oMovim.Tipo.ToString();

            oApropiacion.apropiacionDetalleL = new BolsaApropiacionL();
            oApropiacion.apropiacionDetalleL.Add(oApropDet);
            
            // Cabecera
            ApropiacionDt.addApropiacion(conn, oApropiacion);
            
            // Detalle
            ApropiacionDt.addApropiacionDetalle(conn, oApropiacion.Estacion, oApropiacion.NumeroApropiacion, oApropiacion.apropiacionDetalleL[0]);
        }

        public static void UpdApropiacion(Apropiacion oApropiacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Agregamos la Cabecera de la apropiacion
                    ApropiacionDt.updApropiacion(conn, oApropiacion);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaApropiacion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oApropiacion),
                                                           getAuditoriaDescripcion(oApropiacion, false)),
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

        public static ApropiacionL GetApropiaciones(DateTime? Desde, DateTime? Hasta, int? NumeroBolsa, int? Estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return ApropiacionDt.getApropiaciones(conn, Desde, Hasta, NumeroBolsa, Estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static MovimientoCajaDetalleL getApropiacionDetalles(Apropiacion oApr)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    int? numero= null;
                    int estacion = ConexionBs.getNumeroEstacion();
                    if (oApr != null)
                    {
                        numero = oApr.NumeroApropiacion;
                        estacion = oApr.Estacion.Numero;
                    }         
                    
                    return ApropiacionDt.getApropiacionDetalleDt(conn, estacion, numero);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public static BolsaApropiacionL GetBolsasSinApropiar(int? Estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return ApropiacionDt.GetBolsasSinApropiar(conn, Estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DelApropiacion(Apropiacion oApropiacion, int? Estacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    ApropiacionDt.delApropiacionBolsa(conn, Estacion, oApropiacion.NumeroApropiacion);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaApropiacion(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oApropiacion),
                                                           getAuditoriaDescripcion(oApropiacion, false)),
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

        public static int GetProximoNUmeroBolsa()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);
                    return ApropiacionDt.getProximoNumeroBolsa(conn, ConexionBs.getNumeroEstacion());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetProximoNumeroConsignacion(int intTurno, DateTime dtJornada)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return ApropiacionDt.getProximoNumeroConsignacion(conn, intTurno, dtJornada);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool GetExisteNumeroConsignacion(int intTurno, DateTime dtJornada,int intConsignacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);
                    return ApropiacionDt.getExisteNumeroConsignacion(conn, intTurno, dtJornada,intConsignacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool getExisteNumeroDeposito(int intBolsa)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);
                    return ApropiacionDt.getExisteNumeroDeposito(conn, intBolsa);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.
        
        ///****************************************************************************************************<summary>
        ///         CABECERA DE LA APROPIACION            
        ///****************************************************************************************************<summary>


        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaApropiacion()
        {
            return "APB";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Apropiacion oApropiacion)
        {
            return oApropiacion.NumeroApropiacion.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Apropiacion oApropiacion, bool ConDetalle = true)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Número Depósito", oApropiacion.NumeroApropiacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Usuario", oApropiacion.Usuario.ID);
            AuditoriaBs.AppendCampo(sb, "Fecha", oApropiacion.Fecha.ToString());
            AuditoriaBs.AppendCampo(sb, "Bolsa", oApropiacion.Bolsa.ToString());
            AuditoriaBs.AppendCampo(sb, "Monto Total", oApropiacion.Total.ToString());


            // Auditamos el detalle de las bolsas que apropiamos
            if (ConDetalle)
            {
                foreach (BolsaApropiacion oAproDet in oApropiacion.apropiacionDetalleL)
                {
                    AuditoriaBs.AppendCampo(sb, "Movimiento", oAproDet.NumeroMovimientoCaja.ToString());
                    AuditoriaBs.AppendCampo(sb, "Monto", oAproDet.MontoEquivalente.ToString("C"));
                }
            }

            return sb.ToString();
        }
        
        #endregion
    }
}

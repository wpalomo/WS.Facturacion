using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Validacion;
using Telectronica.Utilitarios;
using System.Data;

namespace Telectronica.Tesoreria
{
    /// <summary>
    /// RENDICION: Metodos de la Clase Rendicion.
    /// </summary>
    public class RendicionBs
    {
        #region RETIROS: Metodos de Retiros.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Retiros de un parte o jornada
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int? - Parte</param>
        /// <param name="numeroMovimiento">int? - Numero de Movimiento de Caja</param>
        /// <param name="desde">DateTime? - Jornada Desde</param>
        /// <param name="hasta">DateTime? - Jornada Hasta</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Retiros</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetiros(int estacion, int? parte, int? numeroMovimiento, DateTime? desde, DateTime? hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RendicionDt.getRetiros(conn, estacion, parte, numeroMovimiento, desde, hasta, operador, turnoDesde, turnoHasta);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los Retiros anticipados sin Confirmar
        /// </summary>
        /// <param name="estacion">Numero de Estacion</param>
        /// <param name="Jornada">Jornada Seleccionada</param>
        /// <param name="Turno">Turno Seleccionado</param>
        /// <returns>MovimientoCajaRetiroL</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetirosSinConfirmar(int estacion, DateTime Jornada, int Turno, string Estado)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RendicionDt.getRetirosSinConfirmar(conn, estacion, Jornada, Turno, null, null, Estado);
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de los Retiros que han sido entregados
        /// </summary>
        /// <param name="entregas">MovimientoCajaRetiroL - Lista de Retiros Confirmados</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleRetirosConfirmados(MovimientoCajaRetiroL Retiros)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                //conn.ConectarConsolidadoThenPlaza();
                return RendicionDt.getDetalleRetirosConfirmados(conn, Retiros);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Retiros de un parte y/o un unico retiro
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <param name="numeroMovimiento">int? - Numero de Movimiento de Caja</param>
        /// <returns>Lista de Retiros</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetiros(int estacion, int parte, int? numeroMovimiento)
        {
            return getRetiros(estacion, parte, numeroMovimiento, null, null, null, null, null);
        }




        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Retiros de una jornada y/o turno
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="desde">DateTime - Jornada Desde</param>
        /// <param name="hasta">DateTime - Jornada Hasta</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Retiros</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL getRetiros(int estacion, DateTime desde, DateTime hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            return getRetiros(estacion, null, null, desde, hasta, operador, turnoDesde, turnoHasta);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Retiro
        /// </summary>
        /// <param name="oRetiro">MovimientoCajaRetiro - Retiro a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addRetiro(MovimientoCajaRetiro oRetiro)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos Jornada Cerrada
                JornadaBs.VerificaJornadaAbierta(conn, oRetiro.Parte.Jornada);

                //Asignamos liquidador
                oRetiro.Liquidador = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());
                ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, oRetiro.Bolsa, oRetiro.Estacion.Numero);
                if (oApropiaciones.Count > 0)
                {
                    string causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0]);
                    throw new ErrorParteStatus(causa);
                }
                //Agregamos el retiro
                RendicionDt.addRetiro(conn, oRetiro);

                //Si en la estacion no hay tesorero, entonces se hace la apropiacion de forma transparente                   
                if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    ApropiacionBs.AddApropiacion(oRetiro, (int)oRetiro.Bolsa, conn, "R");
                }

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRetiro(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oRetiro),
                                                        getAuditoriaDescripcion(oRetiro)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Confirmar Retiros
        /// </summary>
        /// <param name="oRetiros">MovimientoCajaRetiroL - Lista de los retiros a confirmar
        /// <returns>Lista de retiros con problemas</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaRetiroL updConfRetiros(MovimientoCajaRetiroL oRetiros)
        {
            MovimientoCajaRetiroL oRetirosMal = new MovimientoCajaRetiroL();

            //Si la lista es vacia no hacemos nada
            if (oRetiros.Count > 0)
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Verficamos Jornada Cerrada
                    JornadaBs.VerificaJornadaAbierta(conn, oRetiros[0].Parte.Jornada);

                    //Por cada retiro lo confirmams
                    foreach (MovimientoCajaRetiro oRetiro in oRetiros)
                    {
                        if (RendicionDt.updConfRetiro(conn, oRetiro))
                        {
                            //Si en la estacion no hay tesorero, entonces se hace la apropiacion de forma transparente                   
                            if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                            {
                                ApropiacionBs.AddApropiacion(oRetiro, (int)oRetiro.Bolsa, conn, "R");
                            }

                            //Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRetiro(),
                                                                    "M",
                                                                    getAuditoriaCodigoRegistro(oRetiro),
                                                                    getAuditoriaDescripcion(oRetiro)),
                                                                    conn);
                        }
                        else
                        {
                            //Agregamos el parte a la lista de problemas
                            oRetirosMal.Add(oRetiro);
                        }
                    }

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }

            return oRetirosMal;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede anular un Movimiento de Caja
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oMovimiento">MovimientoCaja - Movimiento a anular</param>
        /// <param name="causa">out string - causa por la que no puede liquidar</param>
        /// <returns>true si puede anular un retiro, reposicion o liquidacion </returns>
        /// ***********************************************************************************************
        public static bool PuedeAnularMovimientoCaja(Conexion conn, MovimientoCaja oMovimiento, out string causa)
        {
            bool puedeAnular = true;
            causa = "";

            if (puedeAnular)
            {
                //Jornada Abierta
                if (JornadaBs.EstaCerrada(oMovimiento.Parte.Jornada))
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("La jornada se encuentra cerrada");
                }
            }

            //Esta apropiado (si hay tesorero)
            if (puedeAnular)
            {
                if (EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    if (ApropiacionDt.getBolsaApropiada(conn, oMovimiento))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("El movimiento ya fue apropiado por un tesorero.") + "\n" + Traduccion.Traducir("Debe anular el depósito de bolsa para poder anular este movimiento");
                    }
                }
            }

            //Está depositado
            if (puedeAnular)
            {
                if (RendicionDt.getBolsaDepositada(conn, oMovimiento))
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("El movimiento ya está depositado.") + "\n" + Traduccion.Traducir("Debe anular el depósito para poder anular este movimiento");
                }
            }

            //Fallo pedido
            if (puedeAnular)
            {
                if (RendicionDt.getHayFalloPedido(conn, oMovimiento.Parte))
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("El parte tiene fallo pedido.") + "\n" + Traduccion.Traducir("Se debe anular el pedido del fallo para poder anular la liquidación");
                }
            }

            return puedeAnular;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminar Retiro
        /// </summary>
        /// <param name="oRetiro">MovimientoCajaRetiro - Retiro a eliminar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delRetiro(MovimientoCajaRetiro oRetiro)
        {
            string causa = "";
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos que puede anular liquidacion
                if (!PuedeAnularMovimientoCaja(conn, oRetiro, out causa))
                {
                    throw new ErrorParteStatus(causa);
                }

                // Si trabajamos sin tesorero anulamos la apropiacion, que es transparente al usuario
                if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    if (oRetiro.NumeroApropiacionCabecera != null)
                        ApropiacionDt.delApropiacionBolsa(conn, oRetiro.Estacion.Numero, (int)oRetiro.NumeroApropiacionCabecera);
                }

                // Eliminamos el retiro
                RendicionDt.delRetiro(conn, oRetiro);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRetiro(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oRetiro),
                                                        getAuditoriaDescripcion(oRetiro)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #endregion

        #region LIQUIDACION: Metodos de Liquidacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de una Liquidacion de un parte
        /// </summary>
        /// <param name="oParte">Parte - Parte</param>
        /// <returns>Liquidacion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaLiquidacion getLiquidacion(Parte oParte, bool paraLiquidar, int cantBloques, int cantViolaciones)
        {
            MovimientoCajaLiquidacion oLiquidacion = null;

            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                string causa = "";
                if (paraLiquidar)
                {
                    //Verficamos que puede liquidar
                    if (!PuedeLiquidar(conn, oParte, null, out causa))
                    {
                        throw new ErrorParteStatus(causa);
                    }
                }

                oLiquidacion = RendicionDt.getLiquidacion(conn, oParte);

                if (oLiquidacion != null)
                {
                    //items de la liquidacion
                    oLiquidacion.Detalle = RendicionDt.getDetalleLiquidacion(conn, oParte);

                    //items de los cupones
                    oLiquidacion.DetalleCupon = RendicionDt.getDetalleCuponLiquidacion(conn, oParte);

                    //Tickets Abortados
                    oLiquidacion.TicketsAbortados = RendicionDt.getTicketsAbortadosLiquidacion(conn, oParte);

                    //Vales Prepagos
                    //oLiquidacion.Valesprepagos = RendicionDt.getValesPrepagosLiquidacion(conn, oParte);

                    //Retiros del Parte
                    oLiquidacion.Retiros = RendicionDt.getRetiros(conn, oParte.Estacion.Numero, oParte.Numero, null, null, null, null, null, null);

                    //Bloques que van a ir en este parte
                    //TODO Ver los bloques cruzados y avisar
                    oLiquidacion.Bloques = RendicionDt.getBloquesLiquidacion(conn, oParte, cantBloques);

                    oLiquidacion.MontoVisaIntegrado = (from unBloque in oLiquidacion.Bloques
                                                       select unBloque.VisaIntegrado).Sum();

                    //Violaciones para el supervisor
                    if (oLiquidacion.EsParteSpervisor)
                    {
                        oLiquidacion.Violaciones = RendicionDt.getViolacionesViaCerrada(conn, oParte, cantViolaciones);
                    }
                }
            }

            return oLiquidacion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede liquidar este arte y la causa
        /// </summary>
        /// <param name="oParte">Parte - Parte</param>
        /// <param name="causa">out string - causa por la que no puede liquidar</param>
        /// <returns>true si puede liquidar</returns>
        /// ***********************************************************************************************
        public static bool PuedeLiquidar(Parte oParte, out string causa)
        {
            bool puedeLiquidar = true;
            causa = "";

            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                puedeLiquidar = PuedeLiquidar(conn, oParte, null, out causa);
            }

            return puedeLiquidar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede liquidar este parte y la causa
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oParte">Parte - Parte</param>
        /// <param name="bolsa">int? - Numero de Bolsa</param>
        /// <param name="causa">out string - causa por la que no puede liquidar</param>
        /// <returns>true si puede liquidar</returns>
        /// ***********************************************************************************************
        public static bool PuedeLiquidar(Conexion conn, Parte oParte, int? bolsa, out string causa)
        {
            return PuedeLiquidar(conn, oParte, bolsa, null, null, out causa);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede liquidar este parte y la causa (Sobrecargado)
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oParte">Parte - Parte</param>
        /// <param name="bolsa">int? - Numero de Bolsa</param>
        /// <param name="bolsaCheque">int? - Numero de Bolsa de los Cheques</param>
        /// <param name="causa">out string - causa por la que no puede liquidar</param>
        /// <returns>true si puede liquidar</returns>
        /// ***********************************************************************************************
        public static bool PuedeLiquidar(Conexion conn, Parte oParte, int? bolsa, int? bolsaCheque, int? bolsaAbTroco, out string causa)
        {
            bool puedeLiquidar = true;
            causa = "";

            if (puedeLiquidar)
            {
                //Jornada Abierta
                if (JornadaBs.EstaCerrada(oParte.Jornada))
                {
                    puedeLiquidar = false;
                    causa = Traduccion.Traducir("La jornada se encuentra cerrada");
                }
            }

            if (puedeLiquidar)
            {
                //Tiene partes anteriores sin liquidar
                ParteL oPartes = RendicionDt.getParteAnteriorSinLiquidar(conn, oParte);
                //if (oPartes.Count > 0)
                //{
                //    puedeLiquidar = false;
                //    causa = Traduccion.Traducir("El peajista tiene partes anteriores aún sin liquidar") + "\n";
                //    foreach (Parte parAnt in oPartes)
                //    {
                //        causa = causa + parAnt.Numero.ToString() + " del " + parAnt.Jornada.ToShortDateString() + " turno " + parAnt.Turno.ToString() + "\n";
                //    }
                //    causa = causa + Traduccion.Traducir("Debe liquidar esos partes antes de liquidar este");
                //}
            }

            if (puedeLiquidar)
            {
                //Hay una via abierta
                BloqueL oBloques = RendicionDt.getViasAbiertas(conn, oParte);
                if (oBloques.Count > 0)
                {
                    puedeLiquidar = false;
                    causa = Traduccion.Traducir("En las siguientes vías se encontraron bloques abiertos del parte que intenta rendir") + "\n";
                    foreach (Bloque bloque in oBloques)
                    {
                        causa = causa + bloque.Via.NumeroVia.ToString() + " ";
                    }
                    causa = causa + "\n" + Traduccion.Traducir("Proceda a cerrar la vía e inténtelo nuevamente");
                }
            }

            //Parte es del supervisor y está asignada la plaza
            if (puedeLiquidar)
            {
                if (RendicionDt.getEstacionAbierta(conn, oParte))
                {
                    puedeLiquidar = false;
                    causa = Traduccion.Traducir("El parte a liquidar corresponde con la asignación actual de la estación.")
                            + "\n" + Traduccion.Traducir("Reasigne la estación a otro parte e inténtelo nuevamente.");
                }
            }

            //Parte es de terminal de ventas abierta
            if (puedeLiquidar)
            {
                if (RendicionDt.getTerminalAbierta(conn, oParte))
                {
                    puedeLiquidar = false;
                    causa = Traduccion.Traducir("El parte a liquidar corresponde con la asignación actual de una terminal de ventas.")
                            + "\n" + Traduccion.Traducir("Cierre la terminal de ventas e inténtelo nuevamente.");
                }
            }

            //Retiros sin confirmar
            if (puedeLiquidar)
            {
                if (RendicionDt.getRetirosSinConfirmar(conn, oParte))
                {
                    puedeLiquidar = false;
                    causa = Traduccion.Traducir("El parte a liquidar tiene retiros anticipados sin confirmar.")
                            + "\n" + Traduccion.Traducir("Para poder liquidar estos retiros deben ser confirmados o anulados.");
                }
            }

            //Bolsa de Efectivo Repetida
            if (puedeLiquidar && bolsa != null)
            {
                ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, bolsa, oParte.Estacion.Numero);
                if (oApropiaciones.Count > 0)
                {
                    if (HayApropiacionesDeHoy(oApropiaciones))
                    {
                        puedeLiquidar = false;
                        causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0]);
                    }
                }
            }

            //Bolsa Cheque Repetida
            if (puedeLiquidar && bolsaCheque != null)
            {
                ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, bolsaCheque, oParte.Estacion.Numero);
                if (oApropiaciones.Count > 0)
                {
                    if (HayApropiacionesDeHoy(oApropiaciones))
                    {
                        puedeLiquidar = false;
                        causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0], "El número de Bolsa de cheques ya ha sido usado");
                    }
                }
            }

            //Bolsa Ab. Troco Repetida
            if (puedeLiquidar && bolsaAbTroco != null)
            {
                ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, bolsaAbTroco, oParte.Estacion.Numero);
                if (oApropiaciones.Count > 0)
                {
                    if (HayApropiacionesDeHoy(oApropiaciones))
                    {
                        puedeLiquidar = false;
                        causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0], "El número de Bolsa de abandono de cambio ya ha sido usado");
                    }
                }
            }

            return puedeLiquidar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Verifica si la lista de apropiaciones 
        /// </summary>
        /// <param name="apropiaciones"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static bool HayApropiacionesDeHoy(ApropiacionL apropiaciones)
        {
            //return apropiaciones.Any(unaApropiacion => unaApropiacion.FechaHoraIngreso.Date == DateTime.Now.Date);
            foreach (var unaApropiacion in apropiaciones)
            {
                if (unaApropiacion.FechaHoraIngreso.Date == DateTime.Now.Date)
                {
                    return true;
                }
            }
            return false;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda las denominaciones de una liquidación
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="liquidacion"></param>
        /// ***********************************************************************************************
        private static void SaveDenominaciones(Conexion conn, MovimientoCajaLiquidacion liquidacion)
        {
            foreach (MovimientoCajaDetalle item in from unItem in liquidacion.Detalle
                                                   where unItem.Cantidad > 0
                                                   select unItem)
            {
                RendicionDt.addLiquidacionDetalle(conn, item, liquidacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda los cupones de una liquidación
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="liquidacion"></param>
        /// ***********************************************************************************************
        private static void SaveCupones(Conexion conn, MovimientoCajaLiquidacion liquidacion)
        {
            foreach (MovimientoCajaDetalleCupon item in from unItem in liquidacion.DetalleCupon
                                                        where unItem.montoTotal > 0
                                                        select unItem)
            {
                RendicionDt.addDetalleCupon(conn, item, liquidacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina los cupones de una liquidación
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="liquidacion"></param>
        /// ***********************************************************************************************
        private static void DeleteCupones(Conexion conn, MovimientoCajaLiquidacion liquidacion)
        {
            foreach (MovimientoCajaDetalleCupon item in liquidacion.DetalleCupon)
            {
                RendicionDt.delDetalleCupon(conn, item, liquidacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda los tickets abortados de una liquidación
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="liquidacion"></param>
        /// ***********************************************************************************************
        private static void SaveTicketsAbortados(Conexion conn, MovimientoCajaLiquidacion liquidacion)
        {
            foreach (MovimientoCajaTicketsAbortados item in from unItem in liquidacion.TicketsAbortados
                                                            where unItem.Cantidad > 0
                                                            select unItem)
            {
                RendicionDt.addLiquidacionTicketsAbortados(conn, item, liquidacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda los bloques de una liquidación
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="liquidacion"></param>
        /// ***********************************************************************************************
        private static void SaveBloques(Conexion conn, MovimientoCajaLiquidacion liquidacion)
        {
            //Asignamos Bloques
            liquidacion.Bloques = RendicionDt.getBloquesLiquidacion(conn, liquidacion.Parte, null);

            foreach (Bloque bloque in from unBloque in liquidacion.Bloques
                                      where !unBloque.HayBloqueConflictivo
                                      select unBloque)
            {
                RendicionDt.updBloqueSetParte(conn, bloque, liquidacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Liquidacion
        /// </summary>
        /// <param name="oLiquidacion">MovimientoCajaLiquidacion - Liquidacion a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addLiquidacion(MovimientoCajaLiquidacion oLiquidacion)
        {
            string causa = "";
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos que puede liquidar
                if (!PuedeLiquidar(conn, oLiquidacion.Parte, oLiquidacion.Bolsa, oLiquidacion.BolsaCheques, oLiquidacion.BolsaAbTroco, out causa))
                {
                    throw new ErrorParteStatus(causa);
                }

                //Asignamos liquidador
                oLiquidacion.Liquidador = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());

                RendicionDt.addLiquidacion(conn, oLiquidacion);

                //Grabamos Denominaciones
                SaveDenominaciones(conn, oLiquidacion);

                //Grabamos los Cupones
                SaveCupones(conn, oLiquidacion);

                //Grabamos Tickets Abortados
                SaveTicketsAbortados(conn, oLiquidacion);

                //Asignamo violaciones supervisor
                if (oLiquidacion.EsParteSpervisor)
                {
                    RendicionDt.updViolacionesViaCerrada(conn, oLiquidacion.Parte);
                }

                //Actualizamos los bloques
                SaveBloques(conn, oLiquidacion);

                //Si la validacion no es centralizada marcamos el parte como liquidado
                //Aprovechamos para grabar la observacion
                //TODO si validacion es centralizada no estamos grabando la observacion
                if (!ConfigValidacion.ValidacionCentralizada)
                {
                    //Finalizar validacion y Recalcular totales del parte
                    RendicionDt.updParteFinalizarLiquidacion(conn, oLiquidacion.Parte, ConexionBs.getUsuario(), ConexionBs.getTerminal());
                }

                //Si en la estacion no hay tesorero, entonces se hace la apropiacion de forma transparente                   
                if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    //Grabamos el numero de bolsa aun si no hay dinero
                    //if (oLiquidacion.MontoEfectivo > 0)
                    //{


                    if (oLiquidacion.AbandonoDeTroco > 0)
                    {
                        decimal liquidacion = oLiquidacion.MontoEfectivo + oLiquidacion.AbandonoDeTroco;
                        ApropiacionBs.AddApropiacion(oLiquidacion, (int)oLiquidacion.Bolsa, conn, "X", liquidacion);

                    }
                    else
                    {
                        ApropiacionBs.AddApropiacion(oLiquidacion, (int)oLiquidacion.Bolsa, conn, "L", oLiquidacion.MontoEfectivo);
                    }

                    //   ApropiacionBs.AddApropiacion(oLiquidacion, (int)oLiquidacion.Bolsa, conn, "L", oLiquidacion.MontoEfectivo);
                    //}

                    //Grabamos el numero de bolsa de cheque si hay cheques
                    if (oLiquidacion.MontoCheque > 0)
                    {
                        ApropiacionBs.AddApropiacion(oLiquidacion, (int)oLiquidacion.BolsaCheques, conn, "C", oLiquidacion.MontoCheque);
                    }

                    //Grabamos el numero de bolsa del abandono de cambio si hay cheques

                }

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaLiquidacion(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oLiquidacion),
                                                        getAuditoriaDescripcion(oLiquidacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede anular la liquidacion de un parte y la causa
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oParte">Parte - Parte</param>
        /// <param name="causa">out string - causa por la que no puede liquidar</param>
        /// <returns>true si puede anular la liquidacion </returns>
        /// ***********************************************************************************************
        public static bool PuedeAnularLiquidacion(Conexion conn, Parte oParte, out string causa)
        {
            bool puedeAnular = true;
            causa = "";

            if (puedeAnular)
            {
                var fechaHoy = DateTime.Now;
                var fechaJornada = oParte.Jornada;

                var dias = (fechaHoy - fechaJornada).TotalDays;

                if (dias > 1)
                {
                    //Jornada Abierta
                    if (JornadaBs.EstaCerrada(oParte.Jornada))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La jornada se encuentra cerrada");
                    }
                }
            }

            /*
            //Solo el supervisor a cargo puede anular una liquidacion
            if (puedeAnular)
            {
                if (!CambiosBs.verificaSupervisorACargo())
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("Solo el supervisor a cargo puede anular una liquidación");
                }
            }
            */

            //Hay reposicion
            if (puedeAnular)
            {
                if (RendicionDt.getReposicion(conn, oParte) != null)
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("El parte tiene reposición asignada.") + "\n" + Traduccion.Traducir("Debe anular la reposición para poder anular la liquidación");
                }
            }

            //Esta apropiado (si hay tesorero)
            if (puedeAnular)
            {
                if (EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    if (ApropiacionDt.getBolsaApropiada(conn, oParte, null))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("El movimiento ya fue apropiado por un tesorero.") + "\n" + Traduccion.Traducir("Debe anular el depósito de bolsa para poder anular este movimiento");
                    }
                }
            }

            //Está Depositado
            if (puedeAnular)
            {
                if (RendicionDt.getBolsaDepositada(conn, oParte, null))
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("El movimiento ya está depositado.") + "\n" + Traduccion.Traducir("Debe anular el depósito para poder anular este movimiento");
                }
            }

            //Fallo pedido
            if (puedeAnular)
            {
                if (RendicionDt.getHayFalloPedido(conn, oParte))
                {
                    puedeAnular = false;
                    causa = Traduccion.Traducir("El parte tiene fallo pedido.") + "\n" + Traduccion.Traducir("Se debe anular el pedido del fallo para poder anular la liquidación");
                }
            }

            return puedeAnular;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminar Liquidacion
        /// </summary>
        /// <param name="oParte">Parte - Parte con la Liquidacion a eliminar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delLiquidacion(Parte oParte)
        {
            string causa = "";
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos que puede anular liquidacion
                if (!PuedeAnularLiquidacion(conn, oParte, out causa))
                {
                    throw new ErrorParteStatus(causa);
                }

                MovimientoCajaLiquidacion oLiquidacion = RendicionDt.getLiquidacion(conn, oParte);
                oLiquidacion.DetalleCupon = RendicionDt.getDetalleCuponLiquidacion(conn, oLiquidacion.Parte);

                // Si trabajamos sin tesorero anulamos la apropiacion, que es transparente al usuario
                if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    // Efectivo
                    if (oLiquidacion.NumeroApropiacionCabecera > 0)
                    {
                        ApropiacionDt.delApropiacionBolsa(conn, oLiquidacion.Estacion.Numero, (int)oLiquidacion.NumeroApropiacionCabecera);
                    }
                    //Cheque
                    if (oLiquidacion.NumeroApropiacionChequeCabecera > 0)
                    {
                        ApropiacionDt.delApropiacionBolsa(conn, oLiquidacion.Estacion.Numero, (int)oLiquidacion.NumeroApropiacionChequeCabecera);
                    }

                    //Ab. Troco
                    if (oLiquidacion.NumeroApropiacionAbTrocoCabecera > 0)
                    {
                        ApropiacionDt.delApropiacionBolsa(conn, oLiquidacion.Estacion.Numero, (int)oLiquidacion.NumeroApropiacionAbTrocoCabecera);
                    }
                }

                //Eliminamos los Cupones
                DeleteCupones(conn, oLiquidacion);

                RendicionDt.delLiquidacion(conn, oParte);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaLiquidacion(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oParte),
                                                        getAuditoriaDescripcion(oParte)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza el estado que indica si se mostro la advertencia de diferencia entre lo liquidado y lo facturado
        /// </summary>
        /// <param name="oParte">Liquidacion - liquidación del peajista </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool NotificarMostroAdvertencia(MovimientoCajaLiquidacion liquidacion)
        {
            PartesBs.NotificarMostroAdvertencia(liquidacion.Estacion.Numero, liquidacion.ParteNumero);
            return true;
        }

        #endregion

        #region REPOSICIONES: Metodos de Reposiciones.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista con todos los estados de reposiciones posibles
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static EstadoReposicionL ObtenerEstadosDeReposicionPosibles()
        {
            var estadosDeRep = new EstadoReposicionL();

            //Sin Reposicion pedida 
            var estado = new EstadoReposicion("S");
            estadosDeRep.Add(estado);

            //Reposicion Pedida pendiente de pago
            estado = new EstadoReposicion("P");
            estadosDeRep.Add(estado);

            //Reposicion Pedida Paga
            estado = new EstadoReposicion("G");
            estadosDeRep.Add(estado);

            //Reposicion Anulada
            estado = new EstadoReposicion("X");
            estadosDeRep.Add(estado);

            return estadosDeRep;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene una lista de los pagos de reposiciones
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="sEstado"></param>
        /// <param name="dtFechaDesde"></param>
        /// <param name="dtFechaHasta"></param>
        /// <param name="iMalote"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ReposicionPedidaL ObtenerReposicionesPedidas(int? estacion, string sEstado, DateTime? dtFechaDesde, DateTime? dtFechaHasta, int? iMalote)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RendicionDt.getReposicionesPedidas(conn, estacion, sEstado, dtFechaDesde, dtFechaHasta, iMalote);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Reposicion de un parte 
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <returns>Objeto Reposicion</returns>
        /// ***********************************************************************************************
        public static MovimientoCajaReposicion getReposicion(int estacion, int parte)
        {
            //Un unico registro
            return getPartesReposiciones(estacion, parte, null, null, null, null, null)[0].Reposicion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Parets con sus Reposiciones de una jornada y/o turno
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="desde">DateTime - Jornada Desde</param>
        /// <param name="hasta">DateTime - Jornada Hasta</param>
        /// <param name="operador">string - operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Partes y Reposiciones</returns>
        /// ***********************************************************************************************
        public static ParteReposicionL getPartesReposiciones(int estacion, DateTime desde, DateTime hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            return getPartesReposiciones(estacion, null, desde, hasta, operador, turnoDesde, turnoHasta);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes y Reposiciones de un parte o jornada
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int? - Parte</param>
        /// <param name="numeroMovimiento">int? - Numero de Movimiento de Caja</param>
        /// <param name="desde">DateTime? - Jornada Desde</param>
        /// <param name="hasta">DateTime? - Jornada Hasta</param>
        /// <param name="operador">string - operador</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Partes y sus Reposiciones</returns>
        /// ***********************************************************************************************
        public static ParteReposicionL getPartesReposiciones(int estacion, int? parte, DateTime? desde, DateTime? hasta, string operador, int? turnoDesde, int? turnoHasta)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return RendicionDt.getPartesReposiciones(conn, estacion, parte, desde, hasta, operador, turnoDesde, turnoHasta);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Reposiciones
        /// </summary>
        /// <param name="oReposiciones">MovimientoCajaReposicionL - Reposiciones a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addReposiciones(ReposicionPedidaL oReposiciones)
        {
            if (oReposiciones.Count > 0)
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Verficamos Jornada Cerrada
                    JornadaBs.VerificaJornadaAbierta(conn, oReposiciones[0].PagoReposicion.Parte.Jornada);

                    foreach (ReposicionPedida item in oReposiciones)
                    {
                        addReposicion(conn, item.PagoReposicion, item.Identity);
                    }

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agregar PedidoReposiciones
        /// </summary>
        /// <param name="oReposiciones">MovimientoCajaReposicionL - Reposiciones a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addPedidoReposiciones(BolsaDepositoL oBolsas)
        {
            if (oBolsas.Count > 0)
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true, true);

                    using (Conexion connGst = new Conexion())
                    {
                        //Siempre en la plaza, con transaccion
                        try { connGst.ConectarGST(true, false); }
                        catch (Exception) { throw new Exception("conGST"); }


                        foreach (BolsaDeposito item in oBolsas)
                        {
                            RendicionDt.addReposicionFinanciera(connGst, item.Reposicion);

                            ////Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicionPedida(),
                                                                   "A",
                                                                   getAuditoriaCodigoRegistro(item.Reposicion),
                                                                   getAuditoriaDescripcion(item.Reposicion)),
                                                     conn);
                        }

                        connGst.Finalizar(true);
                    }
                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Reposicion
        /// </summary>
        /// <param name="oReposicion">MovimientoCajaReposicion - Reposicion a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addReposicion(MovimientoCajaReposicion oReposicion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos Jornada Cerrada
                JornadaBs.VerificaJornadaAbierta(conn, oReposicion.Parte.Jornada);

                ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, oReposicion.Bolsa, oReposicion.Estacion.Numero);
                if (oApropiaciones.Count > 0)
                {
                    string causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0]);
                    throw new ErrorParteStatus(causa);
                }

                addReposicion(conn, oReposicion, null);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        private static void addReposicion(Conexion conn, MovimientoCajaReposicion oReposicion, int? numeroReposicionPedida)
        {
            oReposicion.NumeroMovimiento = -1;

            //Asignamos liquidador
            oReposicion.Liquidador = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());
            ApropiacionL oApropiaciones = ApropiacionDt.getApropiaciones(conn, null, null, oReposicion.Bolsa, oReposicion.Estacion.Numero);

            if (oApropiaciones.Count > 0)
            {
                string causa = getDescripcionBolsaYaUtiliza(oApropiaciones[0]);
                throw new ErrorParteStatus(causa);
            }

            // Insertamos la reposicion, que inserta el registro de MOCAJA de la reposicion y el pago mismo, para luego apropiarlo si es necesario.
            RendicionDt.addReposicion(conn, oReposicion, numeroReposicionPedida);

            //Si en la estacion no hay tesorero, entonces se hace la apropiacion de forma transparente                   
            if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
            {
                ApropiacionBs.AddApropiacion(oReposicion, (int)oReposicion.Bolsa, conn, oReposicion.TipoDeReposicion.TipoCodigo == "E" ? "P" : "F");
            }

            //Grabamos auditoria
            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicion(),
                                                    "A",
                                                    getAuditoriaCodigoRegistro(oReposicion),
                                                    getAuditoriaDescripcion(oReposicion)),
                                                    conn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Centralizamos el mensaje que informa que una bolsa ya fue usada, en los diferentes movimientos 
        /// que se requiere una bolsa.
        /// Sobrecargamos la llamada, donde el parametro opcional es el movimiento que se esta realizando 
        /// (donde se esta utilizando la bolsa: Retiro, Liquidacion), en esta ultima hay dos tipos de bolsas
        /// </summary>
        /// <param name="oReposicion">Apropiacion - Reposicion de la que tomaran los datos</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string getDescripcionBolsaYaUtiliza(Apropiacion oApropiacion)
        {
            return getDescripcionBolsaYaUtiliza(oApropiacion, "");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Centralizamos el mensaje que informa que una bolsa ya fue usada, en los diferentes movimientos 
        /// que se requiere una bolsa
        /// </summary>
        /// <param name="oReposicion">Apropiacion - Reposicion de la que tomaran los datos</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string getDescripcionBolsaYaUtiliza(Apropiacion oApropiacion, string sDescripcionInicial)
        {
            string sBolsaUtilizada = "";

            if (sDescripcionInicial.Trim() != "")
            {
                sBolsaUtilizada = Traduccion.Traducir(sDescripcionInicial);
            }
            else
            {
                sBolsaUtilizada = Traduccion.Traducir("El número de Bolsa ya ha sido usado: ");
            }

            sBolsaUtilizada += "\n" +
                               "          " + Traduccion.Traducir("Usuario") + ": " + oApropiacion.Usuario.Nombre + "\n" +
                               "          " + Traduccion.Traducir("Jornada") + ": " + oApropiacion.Fecha.ToShortDateString() + "\n" +
                               "          " + Traduccion.Traducir("Turno") + ": " + oApropiacion.Turno.ToString();

            return sBolsaUtilizada;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminar Reposicion
        /// </summary>
        /// <param name="oReposicion">MovimientoCajaReposicion - Reposicion a eliminar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delReposicion(MovimientoCajaReposicion oReposicion)
        {
            string causa = "";
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                //Verficamos que puede anular liquidacion
                if (!PuedeAnularMovimientoCaja(conn, oReposicion, out causa))
                {
                    throw new ErrorParteStatus(causa);
                }

                // Si trabajamos sin tesorero anulamos la apropiacion, que es transparente al usuario
                if (!EstacionConfiguracionDT.getConfig(conn, ConexionBs.getNumeroEstacion()).esEstacionConTesorero)
                {
                    ApropiacionDt.delApropiacionBolsa(conn, oReposicion.Estacion.Numero, (int)oReposicion.NumeroApropiacionCabecera);
                }

                RendicionDt.delReposicion(conn, oReposicion);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicion(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oReposicion),
                                                        getAuditoriaDescripcion(oReposicion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaLiquidacion()
        {
            return "MOC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaRetiro()
        {
            return "MOC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaReposicion()
        {
            return "MOC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaPedidodeReposicion()
        {
            return "MOC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(MovimientoCajaLiquidacion oLiquidacion)
        {
            return oLiquidacion.Parte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Parte oParte)
        {
            return oParte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(MovimientoCajaRetiro oRetiro)
        {
            return oRetiro.Parte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(MovimientoCajaReposicion oReposicion)
        {
            return oReposicion.Parte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(MovimientoCajaLiquidacion oLiquidacion)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Liquidación del Parte."));

            AuditoriaBs.AppendCampo(sb, "Peajista", oLiquidacion.Peajista.ID);
            AuditoriaBs.AppendCampo(sb, "Hora", oLiquidacion.HoraIngreso);
            if (oLiquidacion.MontoEfectivo > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Monto Efectivo", oLiquidacion.MontoEfectivo.ToString("C"));
            }
            if (oLiquidacion.MontoCheque > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Monto Cheque", oLiquidacion.MontoCheque.ToString("C"));
            }
            if (oLiquidacion.MontoVales > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Monto Vales", oLiquidacion.MontoVales.ToString("C"));
            }
            if (oLiquidacion.MontoTicketManuales > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Monto Ticket Manuales", oLiquidacion.MontoTicketManuales.ToString("C"));
            }
            if (oLiquidacion.CantidadTicketAbortados > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Cantidad Tickets Abortados", oLiquidacion.CantidadTicketAbortados.ToString());
            }
            if (oLiquidacion.Bolsa > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Bolsa", oLiquidacion.Bolsa.ToString());
            }
            if (oLiquidacion.Precinto > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Precinto", oLiquidacion.Precinto.ToString());
            }
            if (oLiquidacion.EsParteSpervisor)
            {
                if (oLiquidacion.Violaciones.Count > 0)
                {
                    AuditoriaBs.AppendCampo(sb, "Violaciones Vía Cerrada", oLiquidacion.Violaciones.Count.ToString());
                }
            }

            return sb.ToString();
        }

        ///**********************************************************************************
        /// <summary>
        /// Codigo de la audigoria
        /// </summary>
        /// <returns></returns>
        ///***********************************************************************************
        private static string getAuditoriaCodigoAuditoriaReposicionPedida()
        {
            return "REP";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ReposicionPedida oRep)
        {
            return oRep.Parte.Numero.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ReposicionPedida oRep)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estacion", oRep.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Parte", oRep.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha Movimiento", oRep.FechaIngreso.ToString("yyyyMMdd hh:mm:ss"));
            AuditoriaBs.AppendCampo(sb, "Bolsa", oRep.bolsa.ToString());

            if (oRep.Peajista != null)
                AuditoriaBs.AppendCampo(sb, "Peajista", oRep.Peajista.ToString());
            if (oRep.Monto != null)
                AuditoriaBs.AppendCampo(sb, "Monto a Reponer", oRep.Monto.ToString());
            if (oRep.TipoDeReposicion != null)
                AuditoriaBs.AppendCampo(sb, "Tipo", oRep.TipoDeReposicion.TipoCodigo.ToString());

            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Parte oParte)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Liquidación del Parte."));
            AuditoriaBs.AppendCampo(sb, Traduccion.Traducir("Peajista"), oParte.Peajista.ID);
            if (oParte.ComentarioEliminacion != null)
                AuditoriaBs.AppendCampo(sb, "Motivo Eliminacióm", oParte.ComentarioEliminacion);
            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(MovimientoCajaRetiro oRetiro)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Retiro Anticipado."));

            AuditoriaBs.AppendCampo(sb, "Peajista", oRetiro.Peajista.ID);
            AuditoriaBs.AppendCampo(sb, "Hora", oRetiro.HoraIngreso);
            AuditoriaBs.AppendCampo(sb, "Moneda", oRetiro.Moneda.Simbolo);
            AuditoriaBs.AppendCampo(sb, "Monto", oRetiro.MontoMoneda.ToString("F2"));
            if (oRetiro.ComentarioEliminacion != null)
                AuditoriaBs.AppendCampo(sb, "Motivo Eliminación", oRetiro.ComentarioEliminacion.ToString());
            if (oRetiro.Bolsa > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Bolsa", oRetiro.Bolsa.ToString());
            }
            if (oRetiro.Precinto > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Precinto", oRetiro.Precinto.ToString());
            }

            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(MovimientoCajaReposicion oReposicion)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Reposicion."));

            AuditoriaBs.AppendCampo(sb, "Peajista", oReposicion.Peajista.ID);
            AuditoriaBs.AppendCampo(sb, "Hora", oReposicion.HoraIngreso);
            AuditoriaBs.AppendCampo(sb, "Moneda", oReposicion.Moneda.Simbolo);
            AuditoriaBs.AppendCampo(sb, "Monto", oReposicion.MontoMoneda.ToString("F2"));

            if (oReposicion.Bolsa > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Bolsa", oReposicion.Bolsa.ToString());
            }
            if (oReposicion.Precinto > 0)
            {
                AuditoriaBs.AppendCampo(sb, "Precinto", oReposicion.Precinto.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista con todos los estados de reposiciones posibles
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static EstadoRetirosL ObtenerEstadosDeRetirosPosibles()
        {
            var estadosDeRetiro = new EstadoRetirosL();

            var estado = new EstadoRetiros("N");
            estadosDeRetiro.Add(estado);

            estado = new EstadoRetiros("S");
            estadosDeRetiro.Add(estado);

            return estadosDeRetiro;
        }

        ///********************************************************************************
        /// <summary>
        /// Elimina la reposicion economica
        /// </summary>
        /// <param name="ListaPartes"></param>
        ///********************************************************************************
        public static void delReposicionFinanciera(BolsaDepositoL lBolsas)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    //Por cada parte asignamos el parte
                    foreach (BolsaDeposito item in lBolsas)
                    {
                        if (RendicionDt.delReposicionFinanciera(conn, item.Reposicion))
                        {
                            ////Grabamos auditoria
                            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaReposicionPedida(),
                                                                   "B",
                                                                   getAuditoriaCodigoRegistro(item.Reposicion),
                                                                   getAuditoriaDescripcion(item.Reposicion)),
                                                                   conn);
                        }
                        else
                        {
                            ;
                            //Agregamos el parte a la lista de problemas
                            //oPartesMal.Add(oParte);
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
    }
}

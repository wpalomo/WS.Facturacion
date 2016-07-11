using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Validacion;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.Web;

namespace Telectronica.Facturacion
{
    public class ClienteFacturacionBs
    {

        #region IMPRESORA: Clase de Datos de Impresora

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las impersoras de facturacion definidas, sin filtro
        /// </summary>
        /// <returns>Lista de Impresoras de Facturacion</returns>
        /// ***********************************************************************************************
        public static ImpresoraL getImpresoras()
        {
            return getImpresoras(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Impresoras de Facturacion definidas
        /// </summary>
        /// <param name="numeroImpresora">byte? - Numero de impresora</param>
        /// <returns>Lista de Impresoras de Facturacion</returns>
        /// ***********************************************************************************************
        public static ImpresoraL getImpresoras(byte? numeroImpresora)
        {
            return getImpresoras(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), numeroImpresora);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Impresoras de Facturacion definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Estacion o Consolidado</param>
        /// <param name="estacion">int? - Estacion de la cual consulto</param>
        /// <param name="numeroImpresora">byte? - Numero de impresora</param>
        /// <returns>Lista de Impresoras de Facturacion</returns>
        /// ***********************************************************************************************
        public static ImpresoraL getImpresoras(bool bConsolidado, int estacion, byte? numeroImpresora)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return ClienteFacturacionDt.getImpresoras(conn, estacion, numeroImpresora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Impresora 
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto de una impresora de facturacion a insertar</param>
        /// ***********************************************************************************************
        public static void addImpresora(Impresora oImpresora)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Verificamos punto de venta no esxiste
                    ListaPuntosVenta oPV = ViaBs.VerificaPuntoVenta(conn, oImpresora.PuntoVenta, "FAC", oImpresora.Codigo);
                    if (oPV != null)
                    {
                        string msg = string.Format("El punto de venta ya está siendo usado por la {0} {1}",
                            (oPV.Origen == "VIA") ? "Vía" : "Impresora de Facturación", oPV.Numvia);
                        throw new Telectronica.Errores.ErrorSPException(msg);
                    }
                    //Agregamos la Impresora de Facturacion
                    ClienteFacturacionDt.addImpresora(oImpresora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaImpresora(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oImpresora),
                                                           getAuditoriaDescripcion(oImpresora)),
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
        /// Actualizacion de una Impresora de Facturacion 
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto de una impresora de facturacion a modificar</param>
        /// ***********************************************************************************************
        public static void updImpresora(Impresora oImpresora)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Verificamos punto de venta no esxiste
                    ListaPuntosVenta oPV = ViaBs.VerificaPuntoVenta(conn, oImpresora.PuntoVenta, "FAC", oImpresora.Codigo);
                    if (oPV != null)
                    {
                        string msg = string.Format("El punto de venta ya está siendo usado por la {0} {1}",
                            (oPV.Origen == "VIA") ? "Vía" : "Impresora de Facturación", oPV.Numvia);
                        throw new Telectronica.Errores.ErrorSPException(msg);
                    }

                    //Modificamos la impresora de fcturacion 
                    ClienteFacturacionDt.updImpresora(oImpresora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaImpresora(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oImpresora),
                                                           getAuditoriaDescripcion(oImpresora)),
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
        /// Baja de una Impresora de Facturacion 
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto de una impresora de facturacion a eliminar</param>
        /// ***********************************************************************************************
        public static void delImpresora(Impresora oImpresora)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Eliminamos la Impresora de facturacion 
                    ClienteFacturacionDt.delImpresora(oImpresora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaImpresora(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oImpresora),
                                                           getAuditoriaDescripcion(oImpresora)),
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


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaImpresora()
        {
            return "IMF";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Impresora oImpresora)
        {
            return oImpresora.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Impresora oImpresora)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", oImpresora.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Número de Impresora", oImpresora.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Punto de Venta", oImpresora.PuntoVenta.ToString());
            AuditoriaBs.AppendCampo(sb, "Cantidad de Copias de Factura", oImpresora.CantidadCopias.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

        #region CAMBIOVENDEDOR: Clase de Datos de Cambios de Vendedores

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el vendedor a Cargo de la Terminal de Facturacion
        /// </summary>
        /// <returns>Objeto con el vendedor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioVendedor getVendedorACargo(short terminal)
        {
            return getVendedorACargo(ConexionBs.getNumeroEstacion(), terminal);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el vendedor a Cargo de la Terminal de Facturacion
        /// </summary>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <param name="terminal">short - Numero de terminal de la que se quiere saber quien esta a cargo</param>
        /// <returns>Objeto con el vendedor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioVendedor getVendedorACargo(int estacion,
                                                       short terminal)
        {
            return getVendedorACargo(ConexionBs.getGSToEstacion(), estacion, terminal);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el Vendedor a Cargo de la Terminal de Facturacion
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="terminal">short - Numero de terminal de la que se quiere saber quien esta a cargo</param>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <returns>Objeto con el vendedor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioVendedor getVendedorACargo(bool bGST,
                                                       int estacion,
                                                       short terminal)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                    return ClienteFacturacionDt.getVendedorACargo(conn, estacion, terminal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Asigna la Terminal de Facturacion
        /// El usuario logueado es el vendedor a asignar
        /// Verifica que la terminal estaba cerrada
        /// Si no tiene parte le asigna parte
        /// Graba la asignacion
        /// </summary>
        /// <param name="oParte">Parte - Parte de la aasignacion (contiene el peajista y la jornada y turno</param>
        /// <param name="oTerminal">TerminalFacturacion - Terminal a asignar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setVendedorACargo(Parte oParte, TerminalFacturacion oTerminal, out bool bMismoParteAnterior)
        {
            try
            {
                bMismoParteAnterior = false;

                CambioVendedor oCambio = new CambioVendedor();
                oCambio.Parte = oParte;
                oCambio.TerminalFacturacion = oTerminal;

                //Completamos datos en el objeto
                oCambio.Estacion = oParte.Estacion;
                oCambio.FechaInicio = DateTime.Now;
                oCambio.Supervisor = oParte.Peajista;
                oCambio.Parte.ModoMantenimiento = false;
                oCambio.NuevoParte = false;


                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Verficamos jornada abierta
                    if (JornadaBs.EstaCerrada(oCambio.Parte.Jornada))
                        throw new ErrorJornadaCerrada(Traduccion.Traducir("la Jornada se encuentra cerrada"));

                    //Verificamos si tiene parte
                    ParteL oPartes = PartesDt.getPartes(conn, oCambio.Estacion.Numero,
                                                        oCambio.Parte.Jornada.Date, oCambio.Parte.Jornada.Date, oCambio.Supervisor.ID,
                                                        oCambio.Parte.Turno, oCambio.Parte.Turno, null);

                    if (oPartes.Count > 0)
                    {
                        oCambio.Parte = oPartes[0];

                        //Verificamos que el parte no este liquidado
                        if (oCambio.Parte.Status != Parte.enmStatus.enmNoLiquidado)
                            throw new ErrorParteStatus(Traduccion.Traducir("El Parte ya está liquidado"));
                        else
                            bMismoParteAnterior = true;
                    }
                    else
                    {
                        oCambio.NuevoParte = true;
                        //asignamos un parte
                        PartesDt.addParte(conn, oCambio.Parte, ConexionBs.getUsuario());
                    }

                    //Agregamos la terminal de facturacion
                    oCambio.TerminalFacturacion = ClienteFacturacionBs.getTerminalActual();

                    //Agregamos el cambio de vendedor
                    ClienteFacturacionDt.setVendedorACargo(oCambio.Parte, oCambio.TerminalFacturacion, false, conn);

                    //Grabamos auditoria: Siempre grabamos la asignacion de la terminal. Si es un nuevo parte generamos una auditoria adicional
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAsignacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroAsignacion(oParte),
                                                           getAuditoriaDescripcionAsignacion(oParte, true, oCambio.NuevoParte)),
                                                           conn);

                    if (oCambio.NuevoParte)
                    {
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNuevoParte(),
                                                               "A",
                                                               getAuditoriaCodigoRegistroNuevoParte(oParte),
                                                               getAuditoriaDescripcionNuevoParte(oParte)),
                                                               conn);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Cierra la Terminal de Facturacion
        /// Verifico que la terminal esta abierta
        /// Si es Propia
        ///     Verifico que el usuario logueado es el vendedor asignado
        /// Si no 
        ///      Verifico que el usuario logueado no es el vendedor asignado
        /// Si es Propia o no esta confirmado
        ///     Verifico que no hay operaciones pendientes
        /// Elimina todas las operaciones pendientes de facturar del parte cerrado
        /// Termina todas las asignaciones
        /// 
        /// Si falla una verificacion genera un error
        /// </summary>
        /// <param name="bPropio">bool - El usuario que cierra es el que abrio terminal
        /// <param name="bConfirmado">bool - El usuario confirmo que quiere eliminar las operaciones pendientes
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void CerrarTerminal(bool bPropia, bool bConfirmado, Parte oParte)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);


                    // Terminal actual
                    TerminalFacturacion oTerminalActual = ClienteFacturacionBs.getTerminalActual();

                    string causa = "";
                    bool puedeConfirmar = false;
                    if (!PuedeCerrarTerminal(conn, bPropia, bConfirmado, oParte, oTerminalActual, out puedeConfirmar, out causa))
                    {
                        //si es operaciones ajenas es otro error (y verificamos la confirmacion)
                        if (puedeConfirmar)
                            throw new WarningFacturacionStatus(causa);
                        else
                            throw new ErrorFacturacionStatus(causa);
                    }

                    // TODO BON: Elimina todas las operaciones pendientes de facturar del parte cerrado
                    if (bConfirmado && !bPropia)
                    {
                        AnularOperacionesPendientes(conn, oParte);

                    }

                    // cierra la terminal 
                    ClienteFacturacionDt.setVendedorACargo(oParte, oTerminalActual, true, conn);



                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAsignacion(),
                                                           "C",
                                                           getAuditoriaCodigoRegistroAsignacion(oParte),
                                                           getAuditoriaDescripcionAsignacion(oParte, false, false)),
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
        /// Devuelve si puede cerrar la terminal de facturacion
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="propia">bool - Está cerrando la propia terminal</param>
        /// <param name="confirmado">bool - Está cerrando la propia terminal</param>
        /// <param name="parte">Parte - datos del parte que estamos cerrando</param>
        /// <param name="terminal">TerminalFacturacion - datos de la terminal</param>
        /// <param name="puedeConfiramr">out bool - true si va a poder confirmar</param>
        /// <param name="causa">out string - causa por la que no puede cerrar</param>
        /// <returns>true si puede cerrar</returns>
        /// ***********************************************************************************************
        public static bool PuedeCerrarTerminal(Conexion conn, bool propia, bool confirmado, Parte parte, TerminalFacturacion terminal, out bool puedeConfirmar, out string causa)
        {
            bool puedeCerrar = true;
            puedeConfirmar = false;
            causa = "";

            try
            {



                // Vendedor a cargo de la terminal (vendedor actual)
                CambioVendedor oVendedorACargo = ClienteFacturacionDt.getVendedorACargo(conn, EstacionBs.getEstacionActual().Numero, terminal.Numero);


                // Verifico que la terminal esta abierta 
                if (puedeCerrar)
                {
                    if (!oVendedorACargo.TerminalAbierta)
                    {
                        puedeCerrar = false;
                        causa = Traduccion.Traducir("La terminal de Facturación ya se encuentra cerrada");
                    }
                }

                // Verifico consistencia entre terminal propia y el usuario que tiene la terminal
                //(por si el Silverlight tenia los datos desactualizados
                if (puedeCerrar)
                {
                    if (propia)
                    {
                        if (ConexionBs.getUsuario() != oVendedorACargo.Supervisor.ID)
                        {
                            puedeCerrar = false;
                            causa = Traduccion.Traducir("El usuario logueado NO es el usuario asignado en la terminal de Facturación");
                        }
                    }
                }

                if (puedeCerrar)
                {
                    if (!propia)
                    {
                        if (ConexionBs.getUsuario() == oVendedorACargo.Supervisor.ID)
                        {
                            puedeCerrar = false;
                            causa = Traduccion.Traducir("El usuario logueado es el usuario asignado en la terminal de Facturación");
                        }
                    }
                }

                //El parte a cerrar debe corresponderse con el que tiene el vendedor
                if (puedeCerrar)
                {
                    if (propia)
                    {
                        if (parte.Numero != oVendedorACargo.Parte.Numero)
                        {
                            puedeCerrar = false;
                            causa = Traduccion.Traducir("El parte que intenta cerrar no corresponde con el parte del vendedor a cargo");
                        }
                    }
                }

                //Verifico si hay operaciones pendientes
                if (puedeCerrar)
                {
                    if (propia || !confirmado)
                    {
                        ClienteL oClientes = FacturacionDt.getClientesNoFacturados(conn, parte.Numero, 'N');
                        if (oClientes.Count > 0)
                        {
                            puedeCerrar = false;
                            causa = Traduccion.Traducir("Hay operaciones pendientes de facturar de los clientes") + "\n";
                            foreach (Cliente cliente in oClientes)
                            {
                                causa = causa + cliente.NumeroCliente.ToString() + " " + cliente.RazonSocial + Environment.NewLine;
                            }
                            if (propia)
                            {
                                causa = causa + Traduccion.Traducir("Debe facturar o anular las operaciones pendientes para poder cerrar la terminal");
                            }
                            else
                            {
                                causa = causa + Traduccion.Traducir("Si cierra la terminal se anularán todas estas operaciones");
                                puedeConfirmar = true;
                            }

                        }
                    }
                }







            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeCerrar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anula las operaciones pendientes
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="parte">Parte - datos del parte que estamos cerrando</param>
        /// ***********************************************************************************************
        public static void AnularOperacionesPendientes(Conexion conn, Parte parte)
        {
            //TODO Nuevas operaciones
            //Entrega de Tag
            EntregaTagL oEntregasTag = TagDT.getEntregasNoFacturadas(conn, parte.Numero, null, false);
            foreach (EntregaTag item in oEntregasTag)
            {
                TagDT.AnularEntrega(conn, item);
            }
            //Entrega de Chip
            EntregaChipL oEntregasChip = ChipDT.getEntregasNoFacturadas(conn, parte.Numero, null, false);
            foreach (EntregaChip item in oEntregasChip)
            {
                ChipDT.AnularEntrega(conn, item);
            }
            //Recargas
            RecargaSupervisionL oRecargas = PrepagoDT.getRecargasNoFacturadas(conn, parte.Numero);
            foreach (RecargaSupervision item in oRecargas)
            {
                PrepagoDT.AnularRecarga(conn, item);
            }
            //Venta Vales
            ValePrepagoVentaL oVentasVale = ValePrepagoDT.getVentaValesNoFacturadas(conn, parte.Numero);
            foreach (ValePrepagoVenta item in oVentasVale)
            {
                ValePrepagoDT.AnularVentaVale(conn, item);
            }
        }

        #region AUDITORIA_ASIGNACIONTERMINAL: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc. (CUANDO SE ASIGNA LA TERMINAL)

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaAsignacion()
        {
            return "TFA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroAsignacion(Parte oParte)
        {
            return oParte.Numero.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionAsignacion(Parte oParte, bool bApertura, bool bNuevoParte)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Vendedor", oParte.Peajista.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", oParte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada", oParte.Jornada.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Turno", oParte.Turno.ToString());

            if (bApertura)
            {
                if (bNuevoParte)
                    AuditoriaBs.AppendCampo(sb, "Se generó un nuevo Parte", "");
                else
                    AuditoriaBs.AppendCampo(sb, "Se usó Parte existente", "");
            }

            return sb.ToString();
        }

        #endregion


        #region AUDITORIA_NUEVOPARTE: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc. (CUANDO ES UN NUEVO PARTE)

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaNuevoParte()
        {
            return "PAR";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroNuevoParte(Parte oParte)
        {
            return oParte.Numero.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionNuevoParte(Parte oParte)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Vendedor", oParte.Peajista.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", oParte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada", oParte.Jornada.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Turno", oParte.Turno.ToString());


            return sb.ToString();
        }

        #endregion


        #endregion

        #region TERMINALFACTURACION: Clase de Datos de TerminalFacturacion


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Configuracion completa de la terminal de facturacion
        /// </summary>
        /// <returns>Configuracion de Terminales de Facturacion</returns>
        /// ***********************************************************************************************
        public static ConfiguracionClienteFacturacion getConfiguracionClienteFacturacion(bool local)
        {
            ConfiguracionClienteFacturacion oConfig = new ConfiguracionClienteFacturacion();

            try
            {

                // Asignamos el objeto estacion de la estacion actual
                if (local)
                {
                    oConfig.EstacionActual = EstacionBs.getEstacionActual();
                }
                else
                {
                    oConfig.EstacionActual = new Estacion(0,"CCA");
                }
                


                // Asignamos el objeto usuario del usuario logueado
                oConfig.UsuarioActual = UsuarioBs.getUsuarioLogueado();



                // Indica si todos los comprobantes utilizan o no la misma numeracion
                //oConfig.MismaNumeracion = ClienteFacturacionBs.getMismaNumeracion();


                // Terminal de Facturacion logica actual
                //oConfig.TerminalActual = ClienteFacturacionBs.getTerminalActual();


                // Parte de facturacion actual
                if (oConfig.TerminalActual != null)
                {
                    CambioVendedor oCambio = ClienteFacturacionBs.getVendedorACargo(oConfig.TerminalActual.Numero);
                    if (oCambio != null)
                    {
                        oConfig.ParteActual = oCambio.Parte;
                        oConfig.FechaAperturaTerminal = oCambio.FechaInicio;
                    }
                }

                //Traemmos los permisos
                oConfig.Permisos = PermisosBs.GetPermisosCliente("FAC", null, false);

                //Maximo Items
                oConfig.MaximoItemsPorFactura = ConfiguracionClienteFacturacion._MaximoItemsPorFactura;


                return oConfig;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las terminales de facturacion definidas, sin filtro
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <returns>Lista de las ultimas Terminales abiertas</returns>
        /// ***********************************************************************************************
        public static CambioVendedorL getTerminalesAbiertas(bool bGST, int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                   return ClienteFacturacionDt.getTerminalesAbiertas(conn, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las terminales de facturacion definidas, sin filtro
        /// </summary>
        /// <returns>Lista de Terminales de Facturacion</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacionL getTerminales()
        {
            return getTerminales(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los lectores de tarjetas chip definidos
        /// </summary>
        /// <returns>Lista de lectores de tarjetas chip</returns>
        /// ***********************************************************************************************
        public static LectorChipL getLectoresChip()
        {
            return getLectoresChip(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Terminales de Facturacion definidas
        /// </summary>
        /// <param name="numeroTerminal">Int16 - Numero de terminal</param>
        /// <returns>Lista de Terminales de Facturacion</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacionL getTerminales(Int16? numeroTerminal)
        {
            return getTerminales(ConexionBs.getGSToEstacion(), ConexionBs.getNumeroEstacion(), numeroTerminal);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Terminales de Facturacion definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Estacion o Consolidado</param>
        /// <param name="estacion">int? - Estacion de la cual consulto</param>
        /// <param name="numeroTerminal">Int16? - Numero de terminal</param>
        /// <returns>Lista de Terminales de Facturacion</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacionL getTerminales(bool bConsolidado, int estacion, Int16? numeroTerminal)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return ClienteFacturacionDt.getTerminales(conn, estacion, numeroTerminal);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Lectores de Tarjetas Chip
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Estacion o Consolidado</param>
        /// <param name="estacion">int? - Estacion de la cual consulto</param>
        /// <param name="numeroLector">Byte? - Numero de lector</param>
        /// <returns>Lista de Lectores de Tarjetas Chip</returns>
        /// ***********************************************************************************************
        public static LectorChipL getLectoresChip(bool bConsolidado, int estacion, byte? numeroLector)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bConsolidado, false);

                    return ClienteFacturacionDt.getLectoresChip(conn, estacion, numeroLector);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Terminal 
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Objeto de una terminal de facturacion a insertar</param>
        /// ***********************************************************************************************
        public static void addTerminal(TerminalFacturacion oTerminal)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Agregamos la Terminal de Facturacion
                    ClienteFacturacionDt.addTerminal(oTerminal, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTerminal(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oTerminal),
                                                           getAuditoriaDescripcion(oTerminal)),
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
        /// Actualizacion de una Terminal de Facturacion 
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Objeto de una terminal de facturacion a modificar</param>
        /// ***********************************************************************************************
        public static void updTerminal(TerminalFacturacion oTerminal)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Modificamos la impresora de fcturacion 
                    ClienteFacturacionDt.updTerminal(oTerminal, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTerminal(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oTerminal),
                                                           getAuditoriaDescripcion(oTerminal)),
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
        /// Baja de una Terminal de Facturacion 
        /// </summary>
        /// <param name="oTerminal">Terminal - Objeto de una terminal de facturacion a eliminar</param>
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// ***********************************************************************************************
        public static void delTerminal(TerminalFacturacion oTerminal, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "TERFAC",
                                                       new string[] { oTerminal.Estacion.Numero.ToString(), oTerminal.Numero.ToString() },
                                                       new string[] { },
                                                       nocheck);

                    //Eliminamos la Impresora de facturacion 
                    ClienteFacturacionDt.delTerminal(oTerminal, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTerminal(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oTerminal),
                                                           getAuditoriaDescripcion(oTerminal)),
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




        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTerminal()
        {
            return "TER";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(TerminalFacturacion oTerminal)
        {
            return oTerminal.Numero.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(TerminalFacturacion oTerminal)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", oTerminal.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Número de Terminal", oTerminal.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Descripción", oTerminal.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Número de Impresora", oTerminal.ImpresoraFacturacion.Codigo.ToString());

            return sb.ToString();
        }

        #endregion


        #endregion

        #region TERMINALFACTURACIONFISICA: Clase de Datos de TerminalFacturacionFisica

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la terminal logica asignada a la terminal fisica actual
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Numero de terminal logica actual</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacion getTerminalActual()
        {
            try
            {
                // Verificamos si la terminal actual se encuentra en la sesion. Si no esta, la buscamos en la base de datos
                TerminalFacturacion oTerminal = null;
                oTerminal = (TerminalFacturacion)HttpContext.Current.Session["TerminalFacturacion"];

                if (oTerminal == null)
                {
                    using (Conexion conn = new Conexion())
                    {
                        //sin transaccion
                        conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                        oTerminal = ClienteFacturacionDt.getTerminalActual(conn, ConexionBs.getTerminal());
                        HttpContext.Current.Session["TerminalFacturacion"] = oTerminal;
                    }
                }
                return oTerminal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion la Terminal de Facturacion fisica actual 
        /// </summary>
        /// <param name="estacion">int - Estacion en la que seteo la terminal fisica</param>
        /// <param name="oTerminal">TerminalFacturacion - Objeto de terminal logica a relacionar con la fisica</param>
        /// ***********************************************************************************************
        public static void updTerminalActual(int estacion, short terminal, LectorChip lectorChip)
        {
            try
            {
                string host = ConexionBs.getTerminal();


                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    // Si el codigo del lector chip es 0, entonces mandamos NULL a datos:
                    if (lectorChip.Numero == 0)
                        lectorChip = null;

                    //Modificamos la impresora de fcturacion 
                    ClienteFacturacionDt.updTerminalActual(conn, estacion, terminal, lectorChip, host);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTerminal(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(host),
                                                           getAuditoriaDescripcion(estacion, terminal, lectorChip, host)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);

                    //Para que la sesion la levante de nuevo
                    HttpContext.Current.Session["TerminalFacturacion"] = null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTerminalFisica()
        {
            return "TFI";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(string host)
        {
            return host;
        }


        ///****************************************************************************************************<summary>
        /// Descripcion  a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(int estacion, short terminal, LectorChip lectorChip, string host)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", estacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Número de Terminal", terminal.ToString());
            if( lectorChip != null )
                AuditoriaBs.AppendCampo(sb, "Lector Chip", lectorChip.Descripcion);

            AuditoriaBs.AppendCampo(sb, "Host", host);

            return sb.ToString();
        }

        #endregion

        #endregion

        #region CONFIGURACIONCLIENTEFACTURACION: Configuracion general del cliente de facturacion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la configuracion de la misma numeracion para todos los comprobantes
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Configuracion de numeracion</returns>
        /// ***********************************************************************************************
        public static bool getMismaNumeracion()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ClienteFacturacionDt.getMismaNumeracion(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region LECTOR CHIP

        public static LectorChip getLectorChip(byte numeroLector)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return ClienteFacturacionDt.getLectorChip(conn, numeroLector);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void addLectorChip(LectorChip oLectorChip)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Agregamos El Lector Chip
                    ClienteFacturacionDt.addLectorChip(conn, oLectorChip);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaLectorChip(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oLectorChip),
                                                           getAuditoriaDescripcion(oLectorChip)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                //TODO: Controlar los errores producidos por id duplicada.
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un lector de tarjetas chip
        /// </summary>
        /// <param name="oLectorChip">Lector que se eliminara</param>
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        public static void delLectorChip(LectorChip oLectorChip, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "LECCHI",
                                                       new string[] { oLectorChip.Estacion.Numero.ToString(), oLectorChip.Numero.ToString() },
                                                       new string[] { "TERFAC_FISICA" },
                                                       nocheck);

                    //Eliminamos El Lector Chip
                    ClienteFacturacionDt.delLectorChip(conn, oLectorChip);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaLectorChip(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oLectorChip),
                                                           getAuditoriaDescripcion(oLectorChip)),
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

        public static void updLectorChip(LectorChip oLectorChip)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la Estacion, con transaccion
                    conn.ConectarPlaza(true);

                    //Modificamos El Lector Chip
                    ClienteFacturacionDt.updLectorChip(conn, oLectorChip);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaLectorChip(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oLectorChip),
                                                           getAuditoriaDescripcion(oLectorChip)),
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

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a los lectores Chip
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaLectorChip()
        {
            return "TER";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(LectorChip oTerminal)
        {
            return oTerminal.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(LectorChip oTerminal)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Estación", oTerminal.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Número de Lector", oTerminal.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Descripción", oTerminal.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Puerto Com", oTerminal.PuertoCOM.ToString());
            AuditoriaBs.AppendCampo(sb, "URL", oTerminal.UrlServicio.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

    }
}

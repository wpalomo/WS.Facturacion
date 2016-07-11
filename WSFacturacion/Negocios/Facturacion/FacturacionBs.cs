using System;
using System.Collections.Generic;
using System.Data;
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
    public class FacturacionBs
    {
        #region FACTURAS: Clase de Datos de Facturas

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Facturas de un Parte
        /// </summary>
        /// <param name="estacion">byte - estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <returns>Lista de Facturas del Parte</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasParte(byte estacion, int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getFacturasParte(conn, estacion, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes con Factura Pendiente
        /// </summary>
        /// <param name="parte">int - Parte</param>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesNoFacturados(int parte, char IncluyeFallos)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getClientesNoFacturados(conn, parte, IncluyeFallos);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de items no facturados de un cliente
        /// </summary>
        /// <param name="parte">int - Parte</param>
        /// <param name="cliente">Cliente - objeto cliente</param>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getItemsNoFacturados(int parte, Cliente cliente, char IncluyeFallos)
        {
            FacturaItemL oItems = new FacturaItemL();
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //Entrega de Tags
                    oItems.AddOperaciones((OperacionL)TagDT.getEntregasNoFacturadas(conn, parte, cliente, false));

                    //Entrega de Chips
                    oItems.AddOperaciones((OperacionL)ChipDT.getEntregasNoFacturadas(conn, parte, cliente, false));

                    //Recargas
                    oItems.AddOperaciones((OperacionL)PrepagoDT.getRecargasNoFacturadas(conn, parte, cliente));

                    //Venta de Vales
                    oItems.AddOperaciones((OperacionL)ValePrepagoDT.getVentaValesNoFacturadas(conn, parte, cliente));

                    //Si el cliente es "CONSUMIDOR FINAL" entonces cargamos los fallos no facturados: 
                    if (cliente.NumeroCliente == 0 && IncluyeFallos == 'S')
                    {
                        //Fallos
                        oItems.AddOperaciones((OperacionL)FacturacionDt.getFallosNoFacturados(conn, parte, cliente, Operacion.enmTipo.enmFallos));
                        //Violaciones
                        oItems.AddOperaciones((OperacionL)FacturacionDt.getFallosNoFacturados(conn, parte, cliente, Operacion.enmTipo.enmViolaciones));
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oItems;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de items de una factura
        /// </summary>
        /// <param name="factura">Factura - objeto factura</param>
        /// <returns>Lista de Items</returns>
        /// ***********************************************************************************************
        public static FacturaItemL getItemsFacturados(Factura factura)
        {
            FacturaItemL oItems = new FacturaItemL();
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //Entrega de Tags
                    oItems.AddRange(TagDT.getEntregasFacturadas(conn, factura));

                    //Entrega de Chips
                    oItems.AddRange(ChipDT.getEntregasFacturadas(conn, factura));

                    //Recargas
                    oItems.AddRange(PrepagoDT.getRecargasFacturadas(conn, factura));

                    //Venta de Vales
                    oItems.AddRange(ValePrepagoDT.getVentaValesFacturadas(conn, factura));

                    //Fallos
                    oItems.AddRange(FacturacionDt.getFallosFacturados(conn, factura, Operacion.enmTipo.enmFallos));

                    //Evasiones
                    oItems.AddRange(FacturacionDt.getFallosFacturados(conn, factura, Operacion.enmTipo.enmViolaciones));

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oItems;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los datos completos de una factura (para impresion)
        /// </summary>
        /// <param name="factura">Factura - objeto factura</param>
        /// <returns>DataSet con el detalle de la factura</returns>
        /// ***********************************************************************************************
        public static DataSet getFacturaDetalle(Factura factura)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getFacturaDetalle(conn, (byte)factura.Estacion.Numero, factura.TipoFactura, factura.Serie, factura.PuntoVenta, factura.NumeroFactura);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el proximo numero de factura
        /// </summary>
        /// <param name="tipoFactura">TipoFactura - Tipo de Factura</param>
        /// <param name="impresora>int - Numero de Impresora</param>
        /// <returns>Numero de Proxima Factura</returns>
        /// ***********************************************************************************************
        public static int getNumeroFactura(TipoFactura tipoFactura, int impresora)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getNumeroFactura(conn, ConexionBs.getNumeroEstacion(), tipoFactura, impresora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Factura
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static TipoFacturaL getTiposFactura()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getTiposFactura(conn, null);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Factura
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oFactura">Factura - Factura a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void  addFactura(TerminalFacturacion oTerminal, Factura oFactura)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Vemos si hay que grabar en gestion
                    bool bHayItemsdeGestion = false;
                    foreach (FacturaItem item in oFactura.Items)
                    {
                        if (item.TipoOperacion == Operacion.enmTipo.enmEntregaChip
                            || item.TipoOperacion == Operacion.enmTipo.enmEntregaTag)
                            bHayItemsdeGestion = true;
                    }
                    //Siempre en la plaza, 
                    //Si hay items de gestion con transaccion distribuida
                    conn.ConectarPlaza(true, bHayItemsdeGestion);

                    //Verficamos que puede facturar
                    if (!PuedeFacturar(conn, oFactura, out causa))
                        throw new ErrorFacturacionStatus(causa);

                    //Abrimos la conexion con Gestion
                    using (Conexion connGST = new Conexion())
                    {
                        if (bHayItemsdeGestion)
                        {
                            // Abrimos una conexion con gestion, la transaccion ya la tenemos
                            connGST.ConectarGST(true, true);
                        }

                        FacturacionDt.addVenta(conn, oTerminal, oFactura);

                        FacturacionDt.addFactura(conn, oFactura);

                        //Grabamos Items
                        foreach (FacturaItem item in oFactura.Items)
                        {
                            //Asignamos datos de la factura
                            item.NumeroFactura = oFactura.NumeroFactura;
                            item.PuntoVenta = oFactura.PuntoVenta;
                            item.TipoFactura = oFactura.TipoFactura;
                            item.TipoFacturaDescr = oFactura.TipoFacturaDescr;
                            item.Serie = oFactura.Serie;
                            item.Cliente = oFactura.Cliente;

                            if (item.Operacion.TipoOperacion != Operacion.enmTipo.enmFallos)
                                item.Operacion.Cliente = oFactura.Cliente;

                            if (item.Monto > 0)
                            {
                                //Habilitar el resultado de la operacion
                                HabilitarItem(conn, connGST, item);

                                FacturacionDt.addFacturaItem(conn, item);

                                //Agregar info de la factura a la operacion
                                FacturarItem(conn, item);
                            }
                        }

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaFactura(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oFactura),
                                                               getAuditoriaDescripcion(oFactura)),
                                                               conn);

                        if (bHayItemsdeGestion)
                        {
                            connGST.Finalizar(true);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede facturar con los datos de esta factura
        /// </summary>
        /// <param name="oFactura">Factura - Datos de la Factura</param>
        /// <param name="causa">out string - causa por la que no puede facturar</param>
        /// <returns>true si puede facturar</returns>
        /// ***********************************************************************************************
        public static bool PuedeFacturar(Factura oFactura, out string causa)
        {
            bool puedeFacturar = true;
            causa = "";

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    puedeFacturar = PuedeFacturar(conn, oFactura, out causa);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeFacturar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede facturar con los datos de esta factura
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oFactura">Factura - Datos de la factura</param>
        /// <param name="causa">out string - causa por la que no puede facturar</param>
        /// <returns>true si puede facturar</returns>
        /// ***********************************************************************************************
        public static bool PuedeFacturar(Conexion conn, Factura oFactura, out string causa)
        {
            bool puedeFacturar = true;
            causa = "";

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeFacturar)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oFactura.Parte))
                    {
                        puedeFacturar = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura inexistente
                if (puedeFacturar)
                {
                    if (FacturacionDt.getFactura(conn, (byte)oFactura.Estacion.Numero, oFactura.TipoFactura, oFactura.Serie, oFactura.NumeroFactura, oFactura.PuntoVenta) != null)
                    {
                        puedeFacturar = false;
                        causa = Traduccion.Traducir("El número de factura indicado ya ha sido usado.")
                                + "\n" + Traduccion.Traducir("Verifique los datos de Tipo de Comprobante, Punto de Venta y Número de Factura.");
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeFacturar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba los datos de la factura en un item
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oItem">FacturaItem - Item de la Factura</param>
        /// ***********************************************************************************************
        public static void FacturarItem(Conexion conn, FacturaItem oItem)
        {
            //TODO faltan algunas operaciones
            bool bOK = false;
            switch (oItem.Operacion.TipoOperacion)
            {
                case Operacion.enmTipo.enmEntregaTag:
                    bOK = TagDT.FacturarEntrega(conn, oItem);
                    break;
                case Operacion.enmTipo.enmRecargaSupervision:
                    bOK = PrepagoDT.FacturarRecarga(conn, oItem);
                    break;
                case Operacion.enmTipo.enmEntregaChip:
                    bOK = ChipDT.FacturarEntrega(conn, oItem);
                    break;
                case Operacion.enmTipo.enmReemplazoTicket:
                    break;
                case Operacion.enmTipo.enmCuotaAbono:
                    break;
                case Operacion.enmTipo.enmVinculacionChip:
                    break;
                case Operacion.enmTipo.enmValePrepagoVenta:
                    bOK = ValePrepagoDT.FacturarVenta(conn, oItem);
                    break;
                case Operacion.enmTipo.enmFallos:
                    bOK =  FacturacionDt.FacturarFallo(conn,oItem);  // ValePrepagoDT.FacturarVenta(conn, oItem);
                    break;
                case Operacion.enmTipo.enmViolaciones:
                    bOK = FacturacionDt.FacturarFallo(conn, oItem);  // ValePrepagoDT.FacturarVenta(conn, oItem);
                    break;
                default:
                    break;
            }

            if (!bOK)
                throw new ErrorFacturacionStatus("Operación de Venta no implementada");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Habilita el resultado de la operación
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD de la Plaza</param>
        /// <param name="connGST">Conexion - Conxion a la BD de Gestión</param>
        /// <param name="oItem">FacturaItem - Item de la Factura</param>
        /// ***********************************************************************************************
        public static void HabilitarItem(Conexion conn, Conexion connGST, FacturaItem oItem)
        {
            //TODO faltan algunas operaciones
            switch (oItem.Operacion.TipoOperacion)
            {
                case Operacion.enmTipo.enmEntregaTag:
                    TagDT.GrabarEnMaestro(connGST, oItem.Operacion.EntregaTag);
                    break;
                case Operacion.enmTipo.enmRecargaSupervision:
                    PrepagoDT.GrabarMovimientoPrepago(conn, oItem.Operacion.RecargaSupervision, false);
                    break;
                case Operacion.enmTipo.enmEntregaChip:
                    ChipDT.GrabarEnMaestro(connGST, oItem.Operacion.EntregaChip);
                    break;
                case Operacion.enmTipo.enmReemplazoTicket:
                    break;
                case Operacion.enmTipo.enmCuotaAbono:
                    break;
                case Operacion.enmTipo.enmVinculacionChip:
                    break;
                case Operacion.enmTipo.enmValePrepagoVenta:
                    ValePrepagoVenta oVenta = oItem.Operacion.ValePrepagoVenta;
                    //TODO ver si corresponde grabar en el maestro (si está habilitado solo en esta plaza)
                    string numeroVale = "";
                    //Grabamos cada vale
                    for (int serie = oVenta.SerieInicial; serie <= oVenta.SerieFinal; serie++)
                    {
                        numeroVale = ValePrepagoBS.getCodigoValeSinCS(serie, oVenta.Cliente.NumeroCliente, oVenta.Categoria.Categoria, (byte)oVenta.Estacion.Numero, (byte)oVenta.TipoTarifa.CodigoTarifa);
                        ValePrepagoDT.GrabarEnMaestro(conn, oVenta, serie, numeroVale);
                    }
                    break;
                case Operacion.enmTipo.enmFallos:
                    // Aca no se hace nada porque los fallos no tienen habilitacion.
                    break;
                case Operacion.enmTipo.enmViolaciones:
                    // Aca no se hace nada porque los fallos no tienen habilitacion.
                    break;
                default:
                    break;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anular Factura
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oFactura">Factura - Factura a Anular </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void anularFactura(TerminalFacturacion oTerminal, Factura oFactura)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Vemos si hay que grabar en gestion
                    bool bHayItemsdeGestion = false;
                    foreach (FacturaItem item in oFactura.Items)
                    {
                        if (item.TipoOperacion == Operacion.enmTipo.enmEntregaChip
                            || item.TipoOperacion == Operacion.enmTipo.enmEntregaTag)
                            bHayItemsdeGestion = true;
                    }
                    //Siempre en la plaza, 
                    //Si hay items de gestion con transaccion distribuida
                    conn.ConectarPlaza(true, bHayItemsdeGestion);


                    //Verficamos que puede Anular
                    if (!PuedeAnular(conn, oFactura, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }

                    //Abrimos la conexion con Gestion
                    using (Conexion connGST = new Conexion())
                    {
                        if (bHayItemsdeGestion)
                        {
                            // Abrimos una conexion con gestion, la transaccion ya la tenemos
                            connGST.ConectarGST(true, true);
                        }

                        //TODO Para sumar como anulado en el Cierre Z
                        //FacturacionDt.anulaVenta(conn, oTerminal, oFactura);


                        //Anulamos Items
                        foreach (FacturaItem item in oFactura.Items)
                        {
                            //Asignamos datos de la factura
                            item.NumeroFactura = oFactura.NumeroFactura;
                            item.PuntoVenta = oFactura.PuntoVenta;
                            item.TipoFactura = oFactura.TipoFactura;
                            item.TipoFacturaDescr = oFactura.TipoFacturaDescr;
                            item.Serie = oFactura.Serie;
                            item.Cliente = oFactura.Cliente;
                            item.Operacion.Cliente = oFactura.Cliente;

                            if (item.Monto > 0)
                            {
                                //Inhabilitar el resultado de la operacion
                                InhabilitarItem(conn, connGST, item);


                                //Eliminar info de la factura a la operacion
                                AnularItem(conn, item, false);
                            }
                        }

                        FacturacionDt.anularFactura(conn, oFactura, false);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaFactura(),
                                                               "B",
                                                               getAuditoriaCodigoRegistro(oFactura),
                                                               getAuditoriaDescripcion(oFactura)),
                                                               conn);

                        if (bHayItemsdeGestion)
                        {
                            connGST.Finalizar(true);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede Anular esta factura
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oFactura">Factura - Datos de la factura</param>
        /// <param name="causa">out string - causa por la que no puede anular</param>
        /// <returns>true si puede anular</returns>
        /// ***********************************************************************************************
        public static bool PuedeAnular(Conexion conn, Factura oFactura, out string causa)
        {
            bool puedeAnular = true;
            causa = "";
            Factura oFacturaAux = null;

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeAnular)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oFactura.Parte))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura existente
                if (puedeAnular)
                {
                    oFacturaAux = FacturacionDt.getFactura(conn, (byte)oFactura.Estacion.Numero, oFactura.TipoFactura, oFactura.Serie, oFactura.NumeroFactura, oFactura.PuntoVenta);
                    if (oFacturaAux == null)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura no existe.");
                    }
                }

                //Factura no debe estar anulada
                if (puedeAnular)
                {
                    if (oFacturaAux.Anulada)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura está anulada.");
                    }
                }

                //Factura no debe estar anulada por Nota de Crédito
                if (puedeAnular)
                {
                    if (oFacturaAux.NotaCredito)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura está anulada por una Nota de Crédito.");
                    }
                }

                //Solo podemos anular la ultima factura de este punto de venta
                if (puedeAnular)
                {
                    FacturaL oFacturasAux = FacturacionDt.getFacturasPorFecha(conn, null, oFactura.FechaGeneracion, DateTime.Now.AddDays(5), 0);
                    foreach (Factura item in oFacturasAux)
                    {
                        if (item.PuntoVenta == oFactura.PuntoVenta && item.NumeroFactura > oFactura.NumeroFactura)
                        {
                            puedeAnular = false;
                            causa = Traduccion.Traducir("Sólo puede anularse la última factura");
                            break;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeAnular;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina los datos de la factura en un item
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oItem">FacturaItem - Item de la Factura</param>
        /// <param name="porNC">bool - Por Nota de Credito</param>
        /// ***********************************************************************************************
        public static void AnularItem(Conexion conn, FacturaItem oItem, bool porNC)
        {
            //TODO faltan algunas operaciones
            bool bOK = false;
            switch (oItem.Operacion.TipoOperacion)
            {
                case Operacion.enmTipo.enmEntregaTag:
                    bOK = TagDT.AnularFacturaEntrega(conn, oItem, porNC);
                    break;
                case Operacion.enmTipo.enmRecargaSupervision:
                    bOK = PrepagoDT.AnularFacturaRecarga(conn, oItem, porNC);
                    break;
                case Operacion.enmTipo.enmEntregaChip:
                    bOK = ChipDT.AnularFacturaEntrega(conn, oItem, porNC);
                    break;
                case Operacion.enmTipo.enmReemplazoTicket:
                    break;
                case Operacion.enmTipo.enmCuotaAbono:
                    break;
                case Operacion.enmTipo.enmVinculacionChip:
                    break;
                case Operacion.enmTipo.enmValePrepagoVenta:
                    bOK = ValePrepagoDT.AnularFacturaVenta(conn, oItem, porNC);
                    break;
                case Operacion.enmTipo.enmFallos:
                    bOK = FacturacionDt.AnularFacturaFallo(conn, oItem, porNC);
                    break;
                case Operacion.enmTipo.enmViolaciones:
                    bOK = FacturacionDt.AnularFacturaFallo(conn, oItem, porNC);
                    break;
                default:
                    break;
            }

            if (!bOK)
                throw new ErrorFacturacionStatus("Operación de Venta no implementada");
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Inhabilita el resultado de la operación
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD de la Plaza</param>
        /// <param name="connGST">Conexion - Conxion a la BD de Gestión</param>
        /// <param name="oItem">FacturaItem - Item de la Factura</param>
        /// ***********************************************************************************************
        public static void InhabilitarItem(Conexion conn, Conexion connGST, FacturaItem oItem)
        {
            //TODO faltan algunas operaciones
            switch (oItem.Operacion.TipoOperacion)
            {
                case Operacion.enmTipo.enmEntregaTag:
                    TagDT.EliminarEnMaestro(connGST, oItem.Operacion.EntregaTag);
                    break;
                case Operacion.enmTipo.enmRecargaSupervision:
                    PrepagoDT.GrabarMovimientoPrepago(conn, oItem.Operacion.RecargaSupervision, true);
                    break;
                case Operacion.enmTipo.enmEntregaChip:
                    ChipDT.EliminarEnMaestro(connGST, oItem.Operacion.EntregaChip);
                    break;
                case Operacion.enmTipo.enmReemplazoTicket:
                    break;
                case Operacion.enmTipo.enmCuotaAbono:
                    break;
                case Operacion.enmTipo.enmVinculacionChip:
                    break;
                case Operacion.enmTipo.enmValePrepagoVenta:
                    ValePrepagoVenta oVenta = oItem.Operacion.ValePrepagoVenta;
                    //TODO ver si corresponde grabar en el maestro (si está habilitado solo en esta plaza)
                    string numeroVale = "";
                    //Grabamos cada vale
                    for (int serie = oVenta.SerieInicial; serie <= oVenta.SerieFinal; serie++)
                    {
                        numeroVale = ValePrepagoBS.getCodigoValeSinCS(serie, oVenta.Cliente.NumeroCliente, oVenta.Categoria.Categoria, (byte)oVenta.Estacion.Numero, (byte)oVenta.TipoTarifa.CodigoTarifa);
                        ValePrepagoDT.EliminarEnMaestro(conn, oVenta, serie, numeroVale);
                    }
                    break;
                case Operacion.enmTipo.enmFallos:
                    break;
                case Operacion.enmTipo.enmViolaciones:
                    break;
                default:
                    break;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Cobros A Cuenta de un Parte
        /// </summary>
        /// <param name="estacion">byte - estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <returns>Lista de Cobros A Cuenta del Parte</returns>
        /// ***********************************************************************************************
        public static CobroACuentaL getCobrosACuentaParte(byte estacion, int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getCobrosACuentaParte(conn, estacion, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Facturas con Cobro Pendiente
        /// </summary>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasPendientes()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getFacturasPendientes(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Cobro A Cuenta
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oCobroACuenta">CobroACuenta - Cobro A Cuenta a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addCobroACuenta(TerminalFacturacion oTerminal, CobroACuenta oCobroACuenta)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, 
                    conn.ConectarPlaza(true, false);


                    //Verficamos que puede Cobrar
                    if (!PuedeCobrar(conn, oCobroACuenta, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }


                    FacturacionDt.addCobroACuenta(conn, oCobroACuenta);


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCobroACuenta(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oCobroACuenta),
                                                           getAuditoriaDescripcion(oCobroACuenta)),
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
        /// Devuelve si puede Cobrar esta factura
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oCobroACuenta">CobroACuenta - Datos del Cobro A Cuenta</param>
        /// <param name="causa">out string - causa por la que no puede Cobrar</param>
        /// <returns>true si puede cobrar</returns>
        /// ***********************************************************************************************
        public static bool PuedeCobrar(Conexion conn, CobroACuenta oCobroACuenta, out string causa)
        {
            bool puedeCobrar = true;
            causa = "";
            Factura oFacturaAux = null;

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeCobrar)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oCobroACuenta.Parte))
                    {
                        puedeCobrar = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura existente
                if (puedeCobrar)
                {
                    oFacturaAux = FacturacionDt.getFactura(conn, (byte)oCobroACuenta.Factura.Estacion.Numero, oCobroACuenta.Factura.TipoFactura, oCobroACuenta.Factura.Serie, oCobroACuenta.Factura.NumeroFactura, oCobroACuenta.Factura.PuntoVenta);
                    if (oFacturaAux == null)
                    {
                        puedeCobrar = false;
                        causa = Traduccion.Traducir("La factura no existe.");
                    }
                }

                //Factura no debe estar anulada
                if (puedeCobrar)
                {
                    if (oFacturaAux.Anulada)
                    {
                        puedeCobrar = false;
                        causa = Traduccion.Traducir("La factura está anulada.");
                    }
                }

                //Factura no debe estar anulada por Nota de Crédito
                if (puedeCobrar)
                {
                    if (oFacturaAux.NotaCredito)
                    {
                        puedeCobrar = false;
                        causa = Traduccion.Traducir("La factura está anulada por una Nota de Crédito.");
                    }
                }

                //Factura no debe estar Cobrada
                if (puedeCobrar)
                {
                    if (oFacturaAux.Cobrada)
                    {
                        puedeCobrar = false;
                        causa = Traduccion.Traducir("La factura ya está cobrada.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeCobrar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminar Cobro A Cuenta
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oCobroACuenta">CobroACuenta - Cobro A Cuenta a eliminar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCobroACuenta(TerminalFacturacion oTerminal, CobroACuenta oCobroACuenta)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, 
                    conn.ConectarPlaza(true, false);


                    //Verficamos que puede Anular el Cobro
                    if (!PuedeAnularCobro(conn, oCobroACuenta, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }


                    FacturacionDt.delCobroACuenta(conn, oCobroACuenta);


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCobroACuenta(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oCobroACuenta),
                                                           getAuditoriaDescripcion(oCobroACuenta)),
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
        /// Devuelve si puede Anular el Cobro de esta factura
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oCobroACuenta">CobroACuenta - Datos del Cobro A Cuenta</param>
        /// <param name="causa">out string - causa por la que no puede Anular</param>
        /// <returns>true si puede anular el cobro</returns>
        /// ***********************************************************************************************
        public static bool PuedeAnularCobro(Conexion conn, CobroACuenta oCobroACuenta, out string causa)
        {
            bool puedeAnular = true;
            causa = "";
            Factura oFacturaAux = null;

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeAnular)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oCobroACuenta.Parte))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura existente
                if (puedeAnular)
                {
                    oFacturaAux = FacturacionDt.getFactura(conn, (byte)oCobroACuenta.Factura.Estacion.Numero, oCobroACuenta.Factura.TipoFactura, oCobroACuenta.Factura.Serie, oCobroACuenta.Factura.NumeroFactura, oCobroACuenta.Factura.PuntoVenta);
                    if (oFacturaAux == null)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura no existe.");
                    }
                }

                //Factura no debe estar anulada
                if (puedeAnular)
                {
                    if (oFacturaAux.Anulada)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura está anulada.");
                    }
                }

                //Factura no debe estar anulada por Nota de Crédito
                if (puedeAnular)
                {
                    if (oFacturaAux.NotaCredito)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura está anulada por una Nota de Crédito.");
                    }
                }

                //Factura  debe estar Cobrada
                if (puedeAnular)
                {
                    if (!oFacturaAux.Cobrada)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura no está cobrada.");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeAnular;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Notas de Credito de un Parte
        /// </summary>
        /// <param name="estacion">byte - estacion</param>
        /// <param name="parte">int - Parte</param>
        /// <returns>Lista de Notas de Credito del Parte</returns>
        /// ***********************************************************************************************
        public static NotaCreditoL getNotasCreditoParte(byte estacion, int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getNotasCreditoParte(conn, estacion, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve las ultimas Facturas de un Cliente
        /// </summary>
        /// <param name="cliente">string - Nombre parcial del cliente</param>
        /// <param name="excluirParte">int - Numero de Parte a excluir</param>
        /// <param name="ultimas">int - Cantidad de Facturas a Devolver</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getUltimasFacturas(string cliente, int excluirParte, int ultimas)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getUltimasFacturas(conn, cliente, excluirParte, ultimas);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve las Facturas de un Cliente de un rango de fechas
        /// </summary>
        /// <param name="cliente">string - Nombre parcial del cliente</param>
        /// <param name="desde">DateTime - Fecha Desde</param>
        /// <param name="hasta">DateTime - Fecha Hasta</param>
        /// <param name="excluirParte">int - Numero de Parte a excluir</param>
        /// <returns>Lista de Facturas</returns>
        /// ***********************************************************************************************
        public static FacturaL getFacturasPorFecha(string cliente, DateTime desde, DateTime hasta, int excluirParte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getFacturasPorFecha(conn, cliente, desde, hasta, excluirParte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar Nota de Credito
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oNotaCredito">NotaCredito - Nota de Credito a Salvar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addNotaCredito(TerminalFacturacion oTerminal, NotaCredito oNotaCredito)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Vemos si hay que grabar en gestion
                    bool bHayItemsdeGestion = false;
                    foreach (FacturaItem item in oNotaCredito.Factura.Items)
                    {
                        if (item.TipoOperacion == Operacion.enmTipo.enmEntregaChip
                            || item.TipoOperacion == Operacion.enmTipo.enmEntregaTag)
                            bHayItemsdeGestion = true;
                    }
                    //Siempre en la plaza, 
                    //Si hay items de gestion con transaccion distribuida
                    conn.ConectarPlaza(true, bHayItemsdeGestion);


                    //Verficamos que puede Hacer la Nota de Credito
                    if (!PuedeNotaCredito(conn, oNotaCredito, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }

                    //Abrimos la conexion con Gestion
                    using (Conexion connGST = new Conexion())
                    {
                        if (bHayItemsdeGestion)
                        {
                            // Abrimos una conexion con gestion, la transaccion ya la tenemos
                            connGST.ConectarGST(true, true);
                        }

                        //TODO Para sumar como anulado en el Cierre Z
                        //FacturacionDt.anulaVenta(conn, oTerminal, oFactura);


                        //Anulamos Items
                        foreach (FacturaItem item in oNotaCredito.Factura.Items)
                        {
                            //Asignamos datos de la factura
                            item.NumeroFactura = oNotaCredito.Factura.NumeroFactura;
                            item.PuntoVenta = oNotaCredito.Factura.PuntoVenta;
                            item.TipoFactura = oNotaCredito.Factura.TipoFactura;
                            item.TipoFacturaDescr = oNotaCredito.Factura.TipoFacturaDescr;
                            item.Serie = oNotaCredito.Factura.Serie;
                            item.Cliente = oNotaCredito.Factura.Cliente;
                            item.Operacion.Cliente = oNotaCredito.Factura.Cliente;

                            if (item.Monto > 0)
                            {
                                //Inhabilitar el resultado de la operacion
                                InhabilitarItem(conn, connGST, item);


                                //Eliminar info de la factura a la operacion indicando que fue por NC
                                AnularItem(conn, item, true);
                            }
                        }

                        //Anulamos la factura indicando que es por nota de credito
                        FacturacionDt.anularFactura(conn, oNotaCredito.Factura, true);

                        //Actualizamos la numeracion
                        FacturacionDt.addVenta(conn, oTerminal, oNotaCredito);

                        //Grabamos la nota de credito
                        FacturacionDt.addNotaCredito(conn, oNotaCredito);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNotaCredito(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oNotaCredito),
                                                               getAuditoriaDescripcion(oNotaCredito)),
                                                               conn);

                        if (bHayItemsdeGestion)
                        {
                            connGST.Finalizar(true);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede generar una nota de credito
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oNotaCredito">NotaCredito - Datos de la Nota de Credito</param>
        /// <param name="causa">out string - causa por la que no puede generar</param>
        /// <returns>true si puede generar</returns>
        /// ***********************************************************************************************
        public static bool PuedeNotaCredito(Conexion conn, NotaCredito oNotaCredito, out string causa)
        {
            bool puedeNotaCredito = true;
            causa = "";
            Factura oFacturaAux = null;

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeNotaCredito)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oNotaCredito.Parte))
                    {
                        puedeNotaCredito = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura existente
                if (puedeNotaCredito)
                {
                    oFacturaAux = FacturacionDt.getFactura(conn, (byte)oNotaCredito.Factura.Estacion.Numero, oNotaCredito.Factura.TipoFactura, oNotaCredito.Factura.Serie, oNotaCredito.Factura.NumeroFactura, oNotaCredito.Factura.PuntoVenta);
                    if (oFacturaAux == null)
                    {
                        puedeNotaCredito = false;
                        causa = Traduccion.Traducir("La factura no existe.");
                    }
                }

                //Factura no debe estar anulada
                if (puedeNotaCredito)
                {
                    if (oFacturaAux.Anulada)
                    {
                        puedeNotaCredito = false;
                        causa = Traduccion.Traducir("La factura está anulada.");
                    }
                }

                //Factura no debe estar anulada por Nota de Crédito
                if (puedeNotaCredito)
                {
                    if (oFacturaAux.NotaCredito)
                    {
                        puedeNotaCredito = false;
                        causa = Traduccion.Traducir("La factura está anulada por una Nota de Crédito.");
                    }
                }

                //Número de NC inexistente
                if (puedeNotaCredito)
                {
                    if (FacturacionDt.getNotaCredito(conn, (byte)oNotaCredito.Estacion.Numero, oNotaCredito.Serie, oNotaCredito.PuntoVenta, oNotaCredito.NumeroNC) != null)
                    {
                        puedeNotaCredito = false;
                        causa = Traduccion.Traducir("El número de Nota de Crédito indicado ya ha sido usado.")
                                + "\n" + Traduccion.Traducir("Verifique los datos de Punto de Venta y Número de Nota de Crédito.");
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeNotaCredito;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anular Nota Credito
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Datos de la Terminal</param>
        /// <param name="oNotaCredito">NotaCredito - NC a Anular </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void anularNotaCredito(TerminalFacturacion oTerminal, NotaCredito oNotaCredito)
        {
            try
            {
                string causa = "";
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Vemos si hay que grabar en gestion
                    bool bHayItemsdeGestion = false;
                    foreach (FacturaItem item in oNotaCredito.Factura.Items)
                    {
                        if (item.TipoOperacion == Operacion.enmTipo.enmEntregaChip
                            || item.TipoOperacion == Operacion.enmTipo.enmEntregaTag)
                            bHayItemsdeGestion = true;
                    }
                    //Siempre en la plaza, 
                    //Si hay items de gestion con transaccion distribuida
                    conn.ConectarPlaza(true, bHayItemsdeGestion);


                    //Verficamos que puede Anular
                    if (!PuedeAnularNC(conn, oNotaCredito, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }

                    //Abrimos la conexion con Gestion
                    using (Conexion connGST = new Conexion())
                    {
                        if (bHayItemsdeGestion)
                        {
                            // Abrimos una conexion con gestion, la transaccion ya la tenemos
                            connGST.ConectarGST(true, true);
                        }

                        //TODO Para sumar como anulado en el Cierre Z
                        //FacturacionDt.anulaVenta(conn, oTerminal, oFactura);


                        //Rehabilitamos Items
                        foreach (FacturaItem item in oNotaCredito.Factura.Items)
                        {
                            //Asignamos datos de la factura
                            item.NumeroFactura = oNotaCredito.Factura.NumeroFactura;
                            item.PuntoVenta = oNotaCredito.Factura.PuntoVenta;
                            item.TipoFactura = oNotaCredito.Factura.TipoFactura;
                            item.TipoFacturaDescr = oNotaCredito.Factura.TipoFacturaDescr;
                            item.Serie = oNotaCredito.Factura.Serie;
                            item.Cliente = oNotaCredito.Factura.Cliente;
                            item.Operacion.Cliente = oNotaCredito.Factura.Cliente;

                            if (item.Monto > 0)
                            {
                                //Habilitar el resultado de la operacion
                                HabilitarItem(conn, connGST, item);

                                //Agregar info de la factura a la operacion
                                FacturarItem(conn, item);
                            }
                        }

                        FacturacionDt.anularNotaCredito(conn, oNotaCredito);

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNotaCredito(),
                                                               "B",
                                                               getAuditoriaCodigoRegistro(oNotaCredito),
                                                               getAuditoriaDescripcion(oNotaCredito)),
                                                               conn);

                        if (bHayItemsdeGestion)
                        {
                            connGST.Finalizar(true);
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

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si puede Anular esta Nota de Credito
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oNotaCredito">NotaCredito - Datos de la NC</param>
        /// <param name="causa">out string - causa por la que no puede anular</param>
        /// <returns>true si puede anular</returns>
        /// ***********************************************************************************************
        public static bool PuedeAnularNC(Conexion conn, NotaCredito oNotaCredito, out string causa)
        {
            bool puedeAnular = true;
            causa = "";
            Factura oFacturaAux = null;

            try
            {

                //Parte debe ser de terminal de ventas abierta
                if (puedeAnular)
                {
                    if (!RendicionDt.getTerminalAbierta(conn, oNotaCredito.Parte))
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("El parte no se corresponde con la asignación actual de una terminal de ventas.")
                                + "\n" + Traduccion.Traducir("Ingrese nuevamente a la terminal de ventas e inténtelo nuevamente.");
                    }
                }

                //Número de factura existente
                if (puedeAnular)
                {
                    oFacturaAux = FacturacionDt.getFactura(conn, (byte)oNotaCredito.Factura.Estacion.Numero, oNotaCredito.Factura.TipoFactura, oNotaCredito.Factura.Serie, oNotaCredito.Factura.NumeroFactura, oNotaCredito.Factura.PuntoVenta);
                    if (oFacturaAux == null)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura no existe.");
                    }
                }

                //Factura no debe estar anulada
                if (puedeAnular)
                {
                    if (oFacturaAux.Anulada)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura está anulada.");
                    }
                }

                //Factura debe estar anulada por Nota de Crédito
                if (puedeAnular)
                {
                    if (!oFacturaAux.NotaCredito)
                    {
                        puedeAnular = false;
                        causa = Traduccion.Traducir("La factura no está anulada por una Nota de Crédito.");
                    }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeAnular;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los datos completos de una Nota de Credito (para impresion)
        /// </summary>
        /// <param name="notacredito">NotaCredito - objeto Nota de Credito</param>
        /// <returns>DataSet con el detalle de la Nota de Credito</returns>
        /// ***********************************************************************************************
        public static DataSet getNotaCreditoDetalle(NotaCredito notacredito)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getNotaCreditoDetalle(conn, (byte)notacredito.Estacion.Numero, notacredito.Serie, notacredito.NumeroNC);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de items de ventas
        /// </summary>
        /// <returns>Lista de Tipos de items de Ventas</returns>
        /// ***********************************************************************************************
        public static TipoItemVentaL getTiposItemsVentas()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return FacturacionDt.getTiposItemsVentas(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region AUDITORIA
        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaFactura()
        {
            return "FAC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Factura oFactura)
        {
            return oFactura.PuntoVenta + "-" + oFactura.NumeroFactura.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Factura oFactura)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(oFactura.TipoFacturaDescr);

            AuditoriaBs.AppendCampo(sb, "Parte", oFactura.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cliente", oFactura.Cliente.NumeroCliente.ToString() + "-" + oFactura.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Monto Total", oFactura.MontoTotal.ToString());
            if (oFactura.CobroACuenta)
                AuditoriaBs.AppendCampo(sb, "Cobro a Cuenta", Traduccion.getSi());

            if (oFactura.FacturaReemplazoTicket)
                AuditoriaBs.AppendCampo(sb, "Reemplazo", Traduccion.getSi());
            if (oFactura.Items != null)
            {
                StringBuilder sb2 = new StringBuilder();
                foreach (FacturaItem item in oFactura.Items)
                {
                    AuditoriaBs.AppendCampo(sb2, "", item.DescripcionVenta);

                    AuditoriaBs.AppendCampo(sb2, "Monto", item.Monto.ToString());
                    sb2.Append("\n");
                }
                AuditoriaBs.AppendCampo(sb, "Items", sb2.ToString());
            }


            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCobroACuenta()
        {
            return "CFC";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(CobroACuenta oCobroACuenta)
        {
            return oCobroACuenta.Factura.PuntoVenta + "-" + oCobroACuenta.Factura.NumeroFactura.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(CobroACuenta oCobroACuenta)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Parte", oCobroACuenta.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cliente", oCobroACuenta.Factura.Cliente.NumeroCliente.ToString() + "-" + oCobroACuenta.Factura.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Monto Total", oCobroACuenta.Monto.ToString());

            sb.Append(oCobroACuenta.Factura.TipoFacturaDescr);


            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaNotaCredito()
        {
            return "NCR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(NotaCredito oNotaCredito)
        {
            return oNotaCredito.PuntoVenta + "-" + oNotaCredito.NumeroNC.ToString();
        }
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(NotaCredito oNotaCredito)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Parte", oNotaCredito.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cliente", oNotaCredito.Cliente.NumeroCliente.ToString() + "-" + oNotaCredito.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Monto Total", oNotaCredito.MontoTotal.ToString());
            AuditoriaBs.AppendCampo(sb, "Comprobante Anulado", oNotaCredito.Factura.TipoFacturaDescr + " " + oNotaCredito.Factura.PuntoVenta + "-" + oNotaCredito.Factura.NumeroFactura.ToString("D08"));
            if (oNotaCredito.MontoTotal != oNotaCredito.Factura.MontoTotal)
                AuditoriaBs.AppendCampo(sb, "Monto Factura", oNotaCredito.Factura.MontoTotal.ToString());



            return sb.ToString();
        }
        #endregion

        #region IMPUESTOS
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Impuestos
        /// </summary>
        /// <returns>Datos de Impuestos</returns>
        /// ***********************************************************************************************
        public static Impuesto getImpuestos()
        {
            Impuesto oImpuestos = new Impuesto();
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    //GestionConfiguracion oConfig = GestionConfiguracionDt.getConfigcco(conn);
                    ConfiguracionTributaria oConfig = GestionConfiguracionDt.getConfigtrb(conn);

                    Tarifa oTarifa = TarifaBs.getTarifaVigente(conn,ConexionBs.getNumeroEstacion(), DateTime.Now);
                    oImpuestos.PorcentajeIva = oTarifa.PorcentajeIva;
                    oImpuestos.PorcentajeRetencion = oConfig.PorcentajeRetencionServicios;
                    oImpuestos.PorcentajeRetencionBienes = oConfig.PorcentajeRetencionBienes;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oImpuestos;
        }

        #endregion
    }
}
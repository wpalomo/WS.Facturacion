using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.Transactions;

namespace Telectronica.Facturacion
{
    public class ChipBS
    {
        #region ENTREGA_CHIPS: Metodos de la Clase de Negocios de la entidad CHIPS.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de entregas de Chips no facturadas 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las entregas
        /// <returns>Lista de entregas de Chips NO facturadas</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getEntregasNoFacturadas(int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ChipDT.getEntregasNoFacturadas(conn, parte, null, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de entregas de Chip no facturadas que coinciden con la patente y el numero de tarjeta (es para validar)
        /// </summary>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene entregas de Chip pendiente de facturar</param>
        /// <returns>Lista de Entregas de Chips pendientes para la patente y numero de tarjeta indicados</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getEntregasNoFacturadasValid(string patente)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return ChipDT.getEntregasNoFacturadasValid(conn, patente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si se puede hacer esta entrega
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oEntrega">EntregaChip - Datos de la Entrega</param>
        /// <param name="causa">out string - causa por la que no puede entregar el tag</param>
        /// <returns>true si puede entregar la tarjeta</returns>
        /// ***********************************************************************************************
        public static bool PuedeEntregarChip(EntregaChip oEntrega, out string causa)
        {
            using (Conexion conn = new Conexion())
            {
                //con transaccion distribuida
                conn.ConectarPlaza(true, true);

                //Si el importe es 0 entonces no debe abonar
                if (oEntrega.Monto <= 0)
                    oEntrega.Abona = "N";

                //Verficamos que puede hacer la entrega
                if (!PuedeEntregarChip(conn, oEntrega, out causa))
                {
                    throw new ErrorFacturacionStatus(causa);
                }

                return true;
            }
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si se puede hacer esta entrega
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oEntrega">EntregaChip - Datos de la Entrega</param>
        /// <param name="causa">out string - causa por la que no puede entregar el tag</param>
        /// <returns>true si puede entregar la tarjeta</returns>
        /// ***********************************************************************************************
        public static bool PuedeEntregarChip(Conexion conn, EntregaChip oEntrega, out string causa)
        {
            bool puedeEntregar = true;
            causa = "";

            try
            {
                if (puedeEntregar)
                {
                    //Parte abierto en una terminal de ventas
                    if (!RendicionDt.getTerminalAbierta(conn, oEntrega.Parte))
                    {
                        puedeEntregar = false;
                        causa = Traduccion.Traducir("El parte no está abierto en una terminal de ventas");
                    }
                }

                //El numero de dispositivo no debe repetirse

                /*if (puedeEntregar)
                {
                    //El tag está asignado a un cliente 
                    ClienteL oClientes = ClienteDT.getDatosClienteInt(conn, null, null, null, null, null, null, oEntrega.Dispositivo, null, null);
                    if (oClientes.Count > 0)
                    {
                        puedeEntregar = false;
                        causa = Traduccion.Traducir("El numero interno ingresado se encuentra habilitado para el cliente: ") + "\n";
                        foreach (Cliente cliente in oClientes)
                        {
                            causa = causa + cliente.NumeroCliente.ToString() + " - " + cliente.RazonSocial + "\n";
                        }
                    }
                }*/


                if (puedeEntregar)
                {
                    //La patente  o tag tiene una entrega pendiente de facturar
                    EntregaChipL oEntregas = ChipDT.getEntregasNoFacturadasValid(conn, oEntrega.Patente);
                    if (oEntregas.Count > 0)
                    {
                        puedeEntregar = false;
                        causa = Traduccion.Traducir("El Vehículo tenía operaciones pendientes de facturar: ") + "\n";
                        foreach (EntregaChip entrega in oEntregas)
                        {
                            causa = causa + entrega.Patente + " - " + Traduccion.Traducir("Tarjeta") + ":" + entrega.NumeroExterno +
                                    " - " + Traduccion.Traducir("Cliente") + ":" + entrega.Cliente.NumeroCliente.ToString() + " - " + entrega.Cliente.RazonSocial + "\n";
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeEntregar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba la entrega. Si no tiene costo, invocamos metodo para grabarlo en el maestro.
        /// </summary>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarEntrega(EntregaChip oEntrega, Vehiculo oVehiculo)
        {
            try
            {
                string causa = "";
                bool hayGST = false;

                using (Conexion conn = new Conexion())
                {
                    //con transaccion distribuida
                    conn.ConectarPlaza(true, true);

                    //Si el importe es 0 entonces no debe abonar
                    if (oEntrega.Monto <= 0)
                        oEntrega.Abona = "N";

                    //Verficamos que puede hacer la entrega
                    if (!PuedeEntregarChip(conn, oEntrega, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }

                    //Agregamos un vehiculo al cliente en la cuenta solicitada
                    using (Conexion connGST = new Conexion())
                    {
                        if (!oVehiculo.VehiculoExistente ||
                                !oEntrega.esDebeAbonar || oEntrega.esPospago)
                        {
                            hayGST = true;
                            // Abrimos una conexion con gestion, con transaccion
                            connGST.ConectarGST(true, false);

                        }

                        if (!oVehiculo.VehiculoExistente)
                        {
                            //Nueva patente
                            oVehiculo.Patente = nuevaPatente(oEntrega.NumeroExterno, oVehiculo.Cuenta.Numero);
                            oEntrega.Patente = oVehiculo.Patente;
                            VehiculoHabilitado vehiHabil = new VehiculoHabilitado();
                            vehiHabil.Vehiculo = oVehiculo;
                            vehiHabil.Habilitado = true;


                            ClienteDT.addVehiculo(connGST, oVehiculo);
                            ClienteDT.updCuentaVehiculo(connGST, oVehiculo.Cuenta, vehiHabil);
                            ClienteDT.RecalcularCuentas(connGST, oVehiculo.Cliente, false);

                        }


                        //Ya viene desde arriba el número interno
                        //oEntrega.NumeroInterno = getProximoNumeroTarjeta(conn);

                        // Guardamos la entrega
                        ChipDT.GuardarEntrega(conn, oEntrega);

                        //Grabamos auditoria de la entrega
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaChip(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oEntrega),
                                                               getAuditoriaDescripcion(oEntrega)),
                                                               conn);

                        // Si no abona el medio, lo grabo en la tabla de maestros de Chips (en GESTION)
                        if (!oEntrega.esDebeAbonar || oEntrega.esPospago)
                        {
                            ChipDT.GrabarEnMaestro(connGST, oEntrega);
                        }

                        if (hayGST)
                            connGST.Finalizar(true);
                    }

                    // Termino toda la transaccion
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
        /// Genera una patente nueva para los vehiculos truchos
        /// </summary>
        /// <param name="numero externo">numero externo de la tarjeta</param>
        /// <param name="numero cuenta">numero cuenta</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static string nuevaPatente(int nuext, int cuenta)
        {
            return nuext.ToString("D04") + cuenta.ToString("D06").Substring(3, 3) + "X";
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anula (elimina) la entrega pendiente de facturar.
        /// </summary>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularEntrega(EntregaChip oEntrega)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarPlaza(true);

                    // Guardamos la entrega
                    ChipDT.AnularEntrega(conn, oEntrega);

                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaChip(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oEntrega),
                                                           getAuditoriaDescripcion(oEntrega)),
                                                           conn);

                    // Termino toda la transaccion
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
        /// Devuelve un DataSet con los datos completos de una entrega (para impresion)
        /// </summary>
        /// <param name="entrega">EntregaChip - objeto de la entrega</param>
        /// <returns>DataSet con el detalle de la entrega</returns>
        /// ***********************************************************************************************
        public static DataSet getEntregaDetalle(EntregaChip entrega, int cantidadCopias)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    return ChipDT.getEntregaDetalle(conn, entrega.Parte.Numero, entrega.Cliente, entrega.Identity, cantidadCopias);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Graba en la tabla de maestros la entrega realizada. 
        /// Es un metodo privado para ser llamado cuando una entrega no tiene costo. En este caso se graba la entrega y en el maestro
        /// </summary>
        /// <param name="oEntrega">EntregaChip - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <param name="conn">Conexion - Coneccion con la base de datos en la que impactara la entrega. Ya contiene la transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void GrabarEnMaestro(Conexion conn, EntregaChip oEntrega)
        {
            try
            {
                // Grabamos en la tabla de maestros
                ChipDT.GrabarEnMaestro(conn, oEntrega);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Chip
        /// </summary>
        /// <param name="oChip">Chip - Objeto del chip que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularChip(Chip oChip)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarGST(true);

                    // Eliminamos el chip
                    ChipDT.EliminarEnMaestro(conn, oChip);

                    //Grabamos auditoria de la anulación
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaChip(),
                                                           "E",
                                                           getAuditoriaCodigoRegistro(oChip),
                                                           getAuditoriaDescripcion(oChip)),
                                                           conn);

                    // Termino toda la transaccion
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
        /// Devuelve el numero interno de la proxima tarjeta chip a generar. La numeracion la concentra en Gestion.
        /// </summary>
        /// <returns>Numero interno de la proxima tarjeta chip a generar</returns>
        /// ***********************************************************************************************
        public static string getProximoNumeroTarjeta()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return getProximoNumeroTarjeta(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string getProximoNumeroTarjeta(Conexion conn)
        {
            int iNumeroTarjeta = ChipDT.getProximoNumeroTarjeta(conn );
            return getLetra(ConexionBs.getNumeroEstacion()) + ConexionBs.getNumeroEstacion().ToString("00") + iNumeroTarjeta.ToString("00000000");
        }

        private static string vLetras = "CABDEHIJKMNOPQRSTVWYZ";
        private static string getLetra(int estacion)
        {
            return vLetras.Substring((estacion-1)/16, 1);
        }
            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaEntregaChip()
            {
                return "ENT";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(EntregaChip oEntregaChip)
            {
                return oEntregaChip.Dispositivo;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(EntregaChip oEntregaChip)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Patente", oEntregaChip.Patente);
                AuditoriaBs.AppendCampo(sb, "Número Externo", oEntregaChip.NumeroExterno.ToString());
                AuditoriaBs.AppendCampo(sb, "Número Interno", oEntregaChip.NumeroInterno.ToString());
                AuditoriaBs.AppendCampo(sb, "Fecha de Entrega", oEntregaChip.FechaOperacion.ToString());
                AuditoriaBs.AppendCampo(sb, "Estación", oEntregaChip.Estacion.Numero.ToString());
                AuditoriaBs.AppendCampo(sb, "Cliente", oEntregaChip.Cliente.NumeroCliente.ToString() + "-" + oEntregaChip.Cliente.RazonSocial);
                AuditoriaBs.AppendCampo(sb, "Cuenta", oEntregaChip.Cuenta.Numero + "-" + oEntregaChip.Cuenta.TipoCuenta.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Costo del Dispositivo", oEntregaChip.MontoString);
                AuditoriaBs.AppendCampo(sb, "Abona Medio", oEntregaChip.esDebeAbonarString);
                AuditoriaBs.AppendCampo(sb, "Reposición", oEntregaChip.esReposicionString);


                return sb.ToString();
            }

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaChip()
            {
                return "CLV";
            }
            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(Chip oChip)
            {
                return oChip.Patente;
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(Chip oChip)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Eliminar Chip", oChip.Dispositivo);
                AuditoriaBs.AppendCampo(sb, "Número Externo", oChip.NumeroExterno.ToString());
                AuditoriaBs.AppendCampo(sb, "Número Interno", oChip.NumeroInterno.ToString());


                return sb.ToString();
            }
            #endregion

        #endregion


        #region VINCULACION_CHIPS: Metodos de la Clase de Negocios de la entidad CHIPS.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de vinculaciones de Chips no facturadas 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las vinculaciones
        /// <returns>Lista de vinculaciones de Chips NO facturadas</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getVinculacionesNoFacturadas(int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ChipDT.getVinculacionesNoFacturadas(conn, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de vinculaciones de Chip no facturadas que coinciden con la patente y el numero de tarjeta (es para validar)
        /// </summary>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene vinculaciones de Chip pendiente de facturar</param>
        /// <param name="nChip">int - Numero de Chip para el cual deseo saber si tiene vinculaciones de Chip pendiente de facturar</param>
        /// <returns>Lista de vinculaciones de Chips pendientes para la patente y numero de tarjeta indicados</returns>
        /// ***********************************************************************************************
        public static EntregaChipL getVinculacionesNoFacturadasValid(string patente, int nChip)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return ChipDT.getVinculacionesNoFacturadasValid(conn, patente, nChip);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Graba la vinculacion. Si no tiene costo, invocamos metodo para grabarlo en el maestro.
        /// </summary>
        /// <param name="oEntrega">EntregaChip - Objeto de la vinculacion que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarVinculacion(EntregaChip oEntrega)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion distribuida
                    conn.ConectarPlaza(true, true);

                    // Guardamos la entrega
                    ChipDT.GuardarVinculacion(conn, oEntrega);

                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVinculacionChip(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroVinculacion(oEntrega),
                                                           getAuditoriaDescripcionVinculacion(oEntrega)),
                                                           conn);

                    // Si no abona el medio, lo grabo en la tabla de maestros de Chips (en GESTION)
                    if (!oEntrega.esDebeAbonar)
                    {
                        using (Conexion connGST = new Conexion())
                        {
                            // Abrimos una conexion con gestion, la transaccion ya esta abierta
                            connGST.ConectarGST(false, false);
                            ChipDT.GrabarEnMaestro(connGST, oEntrega);
                            connGST.Finalizar(true);
                        }
                    }


                    // Termino toda la transaccion
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
        /// Anula (elimina) la vinculacion pendiente de facturar.
        /// </summary>
        /// <param name="oEntrega">EntregaChip - Objeto de la vinculacion que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularVinculacion(EntregaChip oEntrega)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarPlaza(true);

                    // Guardamos la entrega
                    ChipDT.AnularVinculacion(conn, oEntrega);

                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVinculacionChip(),
                                                           "B",
                                                           getAuditoriaCodigoRegistroVinculacion(oEntrega),
                                                           getAuditoriaDescripcionVinculacion(oEntrega)),
                                                           conn);

                    // Termino toda la transaccion
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
        private static string getAuditoriaCodigoAuditoriaVinculacionChip()
        {
            return "ENT";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroVinculacion(EntregaChip oEntregaChip)
        {
            return oEntregaChip.Dispositivo;
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionVinculacion(EntregaChip oEntregaChip)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Patente", oEntregaChip.Patente);
            AuditoriaBs.AppendCampo(sb, "Número Externo", oEntregaChip.NumeroExterno.ToString());
            AuditoriaBs.AppendCampo(sb, "Número Interno", oEntregaChip.NumeroInterno.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha de Entrega", oEntregaChip.FechaOperacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Estación", oEntregaChip.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cliente", oEntregaChip.Cliente.NumeroCliente.ToString() + "-" + oEntregaChip.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Cuenta", oEntregaChip.Cuenta.Numero + "-" + oEntregaChip.Cuenta.TipoCuenta.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Costo del Dispositivo", oEntregaChip.MontoString);
            AuditoriaBs.AppendCampo(sb, "Abona Medio", oEntregaChip.esDebeAbonarString);
            AuditoriaBs.AppendCampo(sb, "Reposición", oEntregaChip.esReposicionString);


            return sb.ToString();
        }

        #endregion

    #endregion

        #region CHIPSLISTANEGRA


        public static void GuardarChipListaNegra(ChipListaNegra oChipListaNegra)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //Conectamos siempre con gestion
                    conn.ConectarGST(true, false);

                    oChipListaNegra.Usuario = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());
                    oChipListaNegra.FechaInhabilitacion = DateTime.Now;


                    // Guardamos el tag en la lista negra
                    ChipDT.addChipListaNegra(conn, oChipListaNegra);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria de la entrega
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaChipListaNegra(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oChipListaNegra),
                                                               getAuditoriaDescripcion(oChipListaNegra)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }


                    // Termino toda la transaccion
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void EliminarChipListaNegra(ChipListaNegra oChipListaNegra)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //Conectamos siempre con gestion
                    conn.ConectarGST(true, false);


                    // Guardamos el tag en la lista negra
                    ChipDT.delChipListaNegra(conn, oChipListaNegra);

                    //Auditoria grabamos donde lo hacemos
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                        //Grabamos auditoria de la entrega
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaChipListaNegra(),
                                                               "B",
                                                               getAuditoriaCodigoRegistro(oChipListaNegra),
                                                               getAuditoriaDescripcion(oChipListaNegra)),
                                                               connAud);

                        connAud.Finalizar(true);
                    }

                    // Termino toda la transaccion
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
        private static string getAuditoriaCodigoAuditoriaChipListaNegra()
        {
            return "LNE";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ChipListaNegra oChipListaNegra)
        {
            return oChipListaNegra.Patente;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ChipListaNegra oChipListaNegra)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Tarjeta", oChipListaNegra.NumeroExterno.ToString());
            AuditoriaBs.AppendCampo(sb, "Número Interno", oChipListaNegra.NumeroChip);
            AuditoriaBs.AppendCampo(sb, "Fecha Inhabilitación", oChipListaNegra.FechaInhabilitacion.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Usuario", oChipListaNegra.Usuario.Nombre);
            AuditoriaBs.AppendCampo(sb, "Comentario", oChipListaNegra.Comentario);

            return sb.ToString();
        }

        #endregion


        #endregion
    }
}

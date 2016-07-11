using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.Transactions;

namespace Telectronica.Facturacion
{
    public class PrepagoBS
    {
        #region RECARGA_SUPERVISION: Metodos de la Clase de Negocios de la entidad de Recargas de supervision.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Recargas no facturadas 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las recargas
        /// <returns>Lista de recargas NO facturadas</returns>
        /// ***********************************************************************************************
        public static RecargaSupervisionL getRecargasNoFacturadas(int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return PrepagoDT.getRecargasNoFacturadas(conn, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si se puede hacer esta recarga
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oRecarga">RecargaSupervision - Datos de la Recarga</param>
        /// <param name="causa">out string - causa por la que no puede recargar</param>
        /// <returns>true si puede recargar</returns>
        /// ***********************************************************************************************
        public static bool PuedeRecargar(Conexion conn, RecargaSupervision oRecarga, out string causa)
        {
            bool puedeRecargar = true;
            causa = "";

            try
            {
                if (puedeRecargar)
                {
                    //Parte abierto en una terminal de ventas
                    if (!RendicionDt.getTerminalAbierta(conn, oRecarga.Parte))
                    {
                        puedeRecargar = false;
                        causa = Traduccion.Traducir("El parte no está abierto en una terminal de ventas");
                    }
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }

            return puedeRecargar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba la recarga de supervision en la base de datos. 
        /// </summary>
        /// <param name="oRecarga">RecargaSupervision - Objeto de la recarga que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarRecarga(RecargaSupervision oRecarga)
        {
            try
            {
                string causa = "";

                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarPlaza(true, true);

                    //Verficamos que puede hacer la entrega
                    if (!PuedeRecargar(conn, oRecarga, out causa))
                    {
                        throw new ErrorFacturacionStatus(causa);
                    }

                    // Guardamos la entrega
                    PrepagoDT.GuardarRecarga(conn, oRecarga);

                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecargaPrepago(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oRecarga),
                                                           getAuditoriaDescripcion(oRecarga)),
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
        /// Anula (elimina) la recarga pendiente de facturar.
        /// </summary>
        /// <param name="oRecarga">RecargaSupervision - Objeto de la recarga que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularRecarga(RecargaSupervision oRecarga)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarPlaza(true);

                    // Guardamos la entrega
                    PrepagoDT.AnularRecarga(conn, oRecarga);

                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecargaPrepago(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oRecarga),
                                                           getAuditoriaDescripcion(oRecarga)),
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
        private static string getAuditoriaCodigoAuditoriaRecargaPrepago()
        {
            return "RPR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(RecargaSupervision oRecarga)
        {
            return oRecarga.Identity.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(RecargaSupervision oRecarga)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Identity", oRecarga.Identity.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha de recarga", oRecarga.FechaOperacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Estación", oRecarga.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cuenta", oRecarga.Cuenta.Numero + "-" + oRecarga.Cuenta.TipoCuenta.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Monto", oRecarga.MontoString);


            return sb.ToString();
        }

        #endregion

        #endregion

        #region SALDO_PREPAGO: Clase de datos de saldos de prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista del saldo prepago de un cliente en una estacion
        /// </summary>
        /// <param name="numeroCliente">int - Numero de cliente que se desea averiguar el saldo</param>
        /// <param name="estacion">int - Numero de estacion de la que se desea saber el saldo</param>
        /// <returns>Objeto con el saldo prepago del cliente</returns>
        /// ***********************************************************************************************
        public static SaldoPrepagoL getSaldoCliente(int numeroCliente)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return PrepagoDT.getSaldoCliente(conn, numeroCliente, ConexionBs.getNumeroEstacion());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todos los registros de saldo que tiene un cliente para las diferentes zonas
        /// </summary>
        /// <param name="numeroCliente">int - Numero de cliente que se desea averiguar el saldo</param>
        /// <returns>Lista de los saldos prepagos del cliente</returns>
        /// ***********************************************************************************************
        public static SaldoPrepagoL getSaldosClientes(int numeroCliente)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return PrepagoDT.getSaldosCliente(conn, numeroCliente);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region RECARGAPOSIBLE: Metodos de la clase de negocios de las recargas posibles

        /// <summary>
        /// Devuelve todas las posibles recargas que se pueden realizar en la via
        /// </summary>
        /// <param name="estacion">Id de la estacion</param>
        /// <param name="TipoCuenta">Id del tipo de cuenta</param>
        /// <param name="Agrupacion">Id de la agrupacion</param>
        /// <returns>Retorna una lista de objetos del tipo RecargaPosible</returns>
        public static RecargaPosible getRecargaPosible(int estacion, int TipoCuenta, int Agrupacion, int CodigoRecarga)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return PrepagoDT.getRecargasPosibles(conn, estacion, TipoCuenta, Agrupacion, CodigoRecarga)[0];
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Devuelve todas las posibles recargas que se pueden realizar en la via
        /// </summary>
        /// <param name="estacion">Id de la estacion</param>
        /// <param name="TipoCuenta">Id del tipo de cuenta</param>
        /// <param name="Agrupacion">Id de la agrupacion</param>
        /// <returns>Retorna una lista de objetos del tipo RecargaPosible</returns>
        public static RecargaPosibleL getRecargasPosibles(int? estacion, int? TipoCuenta, int? Agrupacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return PrepagoDT.getRecargasPosibles(conn, estacion, TipoCuenta, Agrupacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza un registro de posible recarga
        /// </summary>
        /// <param name="oRecargaPosible"></param>
        /// ***********************************************************************************************
        public static void updRecargaPosible(RecargaPosible oRecargaPosible)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la configuracion
                    PrepagoDT.updRecargaPosible(oRecargaPosible, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecargaPosible(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oRecargaPosible),
                                                           getAuditoriaDescripcion(oRecargaPosible)),
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
        /// Agrega un registro de posible recarga
        /// </summary>
        /// <param name="oRecargaPosible"></param>
        /// ***********************************************************************************************
        public static void addRecargaPosible(RecargaPosible oRecargaPosible)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la configuracion
                    PrepagoDT.addRecargaPosible(oRecargaPosible, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecargaPosible(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oRecargaPosible),
                                                           getAuditoriaDescripcion(oRecargaPosible)),
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
        /// Elimina un registro de posible recarga
        /// </summary>
        /// <param name="oRecargaPosible"></param>
        /// ***********************************************************************************************
        public static void delRecargaPosible(RecargaPosible oRecargaPosible)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la configuracion
                    PrepagoDT.delRecargaPosible(oRecargaPosible, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaRecargaPosible(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oRecargaPosible),
                                                           getAuditoriaDescripcion(oRecargaPosible)),
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
        private static string getAuditoriaCodigoAuditoriaRecargaPosible()
        {
            return "CPR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(RecargaPosible oRecargaPosible)
        {
            return oRecargaPosible.ID.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(RecargaPosible oRecargaPosible)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "ID", oRecargaPosible.ID.ToString());
            AuditoriaBs.AppendCampo(sb, "Agrupacion", oRecargaPosible.DescripcionAgrupacion);
            AuditoriaBs.AppendCampo(sb, "Estacion", oRecargaPosible.DescripcionEstacion);
            AuditoriaBs.AppendCampo(sb, "Tipo de Cuenta", oRecargaPosible.DescripcionTipoCuenta);
            AuditoriaBs.AppendCampo(sb, "En Via", oRecargaPosible.EnVia.ToString());
            AuditoriaBs.AppendCampo(sb, "Monto", oRecargaPosible.Monto.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

        #region CONFIGURACION_PREPAGOS: Metodos de la clase de negocios de la configuracion de prepagos

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza la configuracion de los prepagos
        /// </summary>
        /// <param name="oPrepagoConfiguracion"></param>
        /// ***********************************************************************************************
        public static void updPrepagoConfiguracion(PrepagoConfiguracion oPrepagoConfiguracion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos la configuracion
                    PrepagoDT.updPrepagoConfiguracion(oPrepagoConfiguracion, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfiguracionPrepagos(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oPrepagoConfiguracion),
                                                           getAuditoriaDescripcion(oPrepagoConfiguracion)),
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
        /// Obtiene la configuracion de los prepagos
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static PrepagoConfiguracion getPrepagoConfiguracion()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    PrepagoConfiguracionL oPrepagoConfiguracion;
                    oPrepagoConfiguracion = PrepagoDT.getPrepagosConfiguraciones(conn);

                    if (oPrepagoConfiguracion.Count > 0)
                        return oPrepagoConfiguracion[0];
                    else
                        return null;

                        //return PrepagoDT.getPrepagosConfiguraciones(conn)[0];
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
        private static string getAuditoriaCodigoAuditoriaConfiguracionPrepagos()
        {
            return "CPR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(PrepagoConfiguracion oPrepagoConfiguracion)
        {
            return ""; //oPrepagoConfiguracion.ID.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(PrepagoConfiguracion oPrepagoConfiguracion)
        {
            StringBuilder sb = new StringBuilder();

            //AuditoriaBs.AppendCampo(sb, "ID", oPrepagoConfiguracion.ID.ToString());
            AuditoriaBs.AppendCampo(sb, "Habilitado", oPrepagoConfiguracion.Habilitado.ToString());

            return sb.ToString();
        }

        #endregion

        #endregion

    }
}

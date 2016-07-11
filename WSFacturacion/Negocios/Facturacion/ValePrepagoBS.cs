using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Transactions;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    public class ValePrepagoBS
    {
        #region VALESPREPAGOS_VENTA: Metodos de la Clase de Negocios de venta de Vales Prepagos.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Ventas de Vales Prepagos no facturadas 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las ventas
        /// <returns>Lista de Ventas de Vales Prepagos NO facturadas</returns>
        /// ***********************************************************************************************
        public static ValePrepagoVentaL getVentaValesNoFacturadas(int parte)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ValePrepagoDT.getVentaValesNoFacturadas(conn, parte);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Ventas de Vales Prepagos vendidos
        /// </summary>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <returns>Lista de Vales Prepagos Vendidos</returns>
        /// ***********************************************************************************************
        public static ValePrepagoVentaL getVentaValesVendidos(int cliente,      int serieDesde,   
                                                              int serieHasta,   byte categoria)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ValePrepagoDT.getVentaValesVendidos(conn, cliente, serieDesde, serieHasta, categoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Graba la venta de vales. Por cada elemento de la lista de vales, los graba en la base de datos
        /// </summary>
        /// <param name="oVales">ValePrepagoL - Lista de objetos de venta de vales</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarVentaValesPrepagos(ValePrepagoVentaL oVales)
        {           
            try
            {
                StringBuilder sb = new StringBuilder();


                using (Conexion conn = new Conexion())
                {
                    //con transaccion distribuida
                    conn.ConectarPlaza(true, true);

                    sb.Append(getAuditoriaDescripcionCabecera(oVales[0]));

                    // Para que tengan todos la misma fecha
                    DateTime dAuxFechaVenta = DateTime.Now;


                    // Guardamos la entrega
                    foreach (ValePrepagoVenta oVale in oVales)
                    {
                        oVale.FechaOperacion = dAuxFechaVenta;
                        ValePrepagoDT.GuardarVentaValePrepago(conn, oVale);

                        sb.Append(getAuditoriaDescripcionDetalle(oVale));
                    }
   
                    //Grabamos auditoria de la entrega
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaTag(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oVales[0]), 
                                                           sb.ToString()),
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
        /// Anula (elimina) la venta de vale pendiente de facturar.
        /// </summary>
        /// <param name="oVale">ValePrepago - Objeto de la venta que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularVentaVale(ValePrepagoVenta oVale)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //con transaccion
                    conn.ConectarPlaza(true);

                    // Guardamos la entrega
                    ValePrepagoDT.AnularVentaVale(conn, oVale);

                    //Grabamos auditoria de la venta
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaTag(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oVale),
                                                           getAuditoriaDescripcionCabecera(oVale) + " - " + getAuditoriaDescripcionDetalle(oVale)),
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


        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de habilitaciones de un lote con su precio para ser vendido 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="lote">int - Numero de lote que se consultar la habilitacion</param>
        /// <returns>Habilitacion de los vales con su precio para ser vendidos</returns>
        /// ***********************************************************************************************
        public static ValePrepagoHabilitacionL getHabilitacionVentaVale(int lote)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ValePrepagoDT.getHabilitacionVentaVale(conn, lote);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el precio de una tabda de vales para ser vendido 
        /// Verificando que la tanda pueda ser vendida
        /// </summary>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <param name="cuenta">out Cuenta - Devuelve la cuenta de los vales</param>
        /// <param name="estacionesHabilitadas">out string - Devuelve la lista de estaciones habilitadas</param>
        /// <returns> precio para ser vendidos</returns>
        /// ***********************************************************************************************
        public static decimal getPrecioTandaVales(int cliente, int serieDesde, int serieHasta, byte categoria, 
                                                  out Cuenta cuenta, out string estacionesHabilitadas)
        {
            try
            {
                decimal precio = 0;
                estacionesHabilitadas = "";
                cuenta = null;
                using (Conexion conn = new Conexion())
                {
                    string error = "";
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    //Debe haber una personalizacion que los abarque
                    ValePrepagoPersonalizacionL oPersonalizacion = ValePrepagoDT.getPersonalizacionValesPrepagos(conn, cliente, serieDesde, serieHasta, categoria);
                    if (oPersonalizacion.Count <= 0)
                    {
                        error = Traduccion.Traducir("No existen Personalizaciones de Vales para este cliente con estas Características");
                        throw new ErrorFacturacionStatus(error);
                    }
                    else
                    {
                        cuenta = oPersonalizacion[0].Cuenta;
                    }

                    //Ninguno de ellos debe haberse vendido
                    ValePrepagoVentaL oVentas = ValePrepagoDT.getVentaValesVendidos(conn, cliente, serieDesde, serieHasta, categoria);
                    if (oVentas.Count > 0)
                    {
                        error = Traduccion.Traducir("Los siguientes vales prepagos ya se han vendido") + "\n";
                        foreach (ValePrepagoVenta item in oVentas)
                        {
                            error += item.SerieInicial.ToString() + " - " + item.SerieFinal.ToString() + "\n";
                        }
                        throw new ErrorFacturacionStatus(error);
                    }

                    //Ninguno debe estar en Lista Negra
                    ValePrepagoListaNegraL oListaNegra = ValePrepagoDT.getListaNegraValesPrepagos(conn, cliente, serieDesde, serieHasta, categoria);
                    if (oListaNegra.Count > 0)
                    {
                        error = Traduccion.Traducir("Los siguientes vales prepagos están en Lista Negra") + "\n";
                        foreach (ValePrepagoListaNegra item in oListaNegra)
                        {
                            error += item.SerieInicial.ToString() + " - " + item.SerieFinal.ToString() + " " + item.Comentario + "\n";
                        }
                        throw new ErrorFacturacionStatus(error);
                    }

                    //Si no estamos en una estacion administrativa deben estar habilitados solo en esta estacvión
                    ValePrepagoHabilitacionL oHabil = ValePrepagoDT.getHabilitacionVentaVale(conn, cliente, serieDesde, serieHasta, categoria);
                    foreach (ValePrepagoHabilitacion item in oHabil)
                    {
                      
                        if (ConexionBs.getNumeroEstacion() < ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
                        {
                            if (item.EstacionHabilitada.Numero != ConexionBs.getNumeroEstacion())
                            {
                                error = Traduccion.Traducir("Solo la estación administrativa puede vender vales habilitados en otra estación");
                                throw new ErrorFacturacionStatus(error);
                            }
                        }
                        estacionesHabilitadas += item.EstacionHabilitada.Nombre + "-";
                        //Tomamos el precio mas alto
                        if (item.MontoTarifa > precio)
                        {
                            precio = item.MontoTarifa;
                        }
                    }
                    //Quitamos el ultimo guion
                    estacionesHabilitadas = estacionesHabilitadas.Substring(0, estacionesHabilitadas.Length - 1);
                    return precio * (serieHasta - serieDesde + 1) ;
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
            private static string getAuditoriaCodigoAuditoriaEntregaTag()
            {
                return "VEV";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(ValePrepagoVenta oVale)
            {
                return oVale.FechaOperacion.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del lote afectado (cabecera)
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcionCabecera(ValePrepagoVenta oVale)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Cliente", oVale.Cuenta.Cliente.NumeroCliente.ToString() + "-" + oVale.Cuenta.Cliente.RazonSocial);
                AuditoriaBs.AppendCampo(sb, "Cuenta", oVale.Cuenta.Agrupacion.DescrAgrupacion);
                AuditoriaBs.AppendCampo(sb, "Monto", oVale.MontoString);

                return sb.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado (detalle)
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcionDetalle(ValePrepagoVenta oVale)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Vale", oVale.SerieInicial.ToString() + "-" + oVale.SerieFinal.ToString());
                AuditoriaBs.AppendCampo(sb, "Categoría", oVale.Categoria.Categoria.ToString());
                
                // Cuando se agregan venta de vales esta este dato, cuando se eliminan no.
                if (oVale.EstacionesHabilitadasString != null)
                    AuditoriaBs.AppendCampo(sb, "Est.Habilitadas", oVale.EstacionesHabilitadasString);


                return sb.ToString();
            }

            #endregion

        #endregion


        #region VALESPREPAGOS_PERSONALIZACION: Metodos de la Clase de Negocios de personalizacion de Vales Prepagos.

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Personalizacions de Vales Prepagos 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las ventas
        /// <returns>Lista de Ventas de Vales Prepagos NO facturadas</returns>
        /// ***********************************************************************************************
        public static ValePrepagoPersonalizacionL getPersonalizacionesVales(int cliente,        int serieDesde, 
                                                                            int serieHasta,     byte categoria)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ValePrepagoDT.getPersonalizacionValesPrepagos(conn, cliente, serieDesde, serieHasta, categoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        public static string getCodigoVale(int serie, int cliente, byte categoria, byte estacion, byte tipoTarifa)
        {
            string vale = getCodigoValeSinCS(serie, cliente, categoria, estacion, tipoTarifa);

            //TODO Calcular CheckSum

            return vale;
        }
        public static string getCodigoValeSinCS(int serie, int cliente, byte categoria, byte estacion, byte tipoTarifa)
        {
            return string.Format("0{0:D06}{1:D05}{2:D02}{3:D02}{4:D02}",serie,cliente, categoria, 99, tipoTarifa);
        }
        #endregion


        #region VALEPREPAGO_LISTANEGRA: Metodos de la Clase de Negocios de Lista Negra de Vales Prepagos
        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de vales prepagos en lista negra que coinciden con los parametros
        /// </summary>
        /// <param name="cliente">int - Numero de cliente al que se le personalizaron los vales</param>
        /// <param name="serieDesde">int - Numero de serie inicial</param>
        /// <param name="serieHasta">int - Numero de serie final</param>
        /// <param name="categoria">byte - Categoria de la personalizacion</param>
        /// <returns>Lista de Vales Prepagos en Lista Negra</returns>
        /// ***********************************************************************************************
        public static ValePrepagoListaNegraL getListaNegraValesPrepagos(int cliente,    int serieDesde, 
                                                                        int serieHasta, byte categoria)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false);

                    return ValePrepagoDT.getListaNegraValesPrepagos(conn, cliente, serieDesde, serieHasta, categoria);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */
        #endregion

    }
}

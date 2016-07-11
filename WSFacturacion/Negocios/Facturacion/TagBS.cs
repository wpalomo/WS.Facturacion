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
    public class TagBS
    {
        #region ENTREGA_TAGS: Metodos de la Clase de Negocios de la entidad TAGS.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de entregas de tag no facturadas 
        /// </summary>
        /// <param name="parte">int - Numero de parte del que se consultan las entregas
        /// <returns>Lista de entregas de tags NO facturadas</returns>
        /// ***********************************************************************************************
        public static EntregaTagL getEntregasNoFacturadas(int parte)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(false, false);
                return TagDT.getEntregasNoFacturadas(conn, parte, null, true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de entregas de tag no facturadas que coinciden con la patente y el tag (es para validar)
        /// </summary>
        /// <param name="patente">string - Patente para la cual deseo saber si tiene entregas de tag pendiente de facturar</param>
        /// <param name="tag">string - Numero de tag para el cual deseo saber si tiene entregas de tag pendiente de facturar</param>
        /// <returns>Lista de Entregas de Tags pendientes para la patente y tag indicados</returns>
        /// ***********************************************************************************************
        public static EntregaTagL getEntregasNoFacturadasValid(string patente, string tag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false);
                return TagDT.getEntregasNoFacturadasValid(conn, patente, tag);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si se puede hacer esta entrega
        /// </summary>
        /// <param name="conn">Conexion - Conxion a la BD</param>
        /// <param name="oEntrega">EntregaTag - Datos de la Entrega</param>
        /// <param name="causa">out string - causa por la que no puede entregar el tag</param>
        /// <returns>true si puede entregar el tag</returns>
        /// ***********************************************************************************************
        public static bool PuedeEntregarTag(Conexion conn, EntregaTag oEntrega, out string causa)
        {
            bool puedeEntregar = true;
            causa = "";

            if (puedeEntregar)
            {
                //Parte abierto en una terminal de ventas
                if (!RendicionDt.getTerminalAbierta(conn, oEntrega.Parte))
                {
                    puedeEntregar = false;
                    causa = Traduccion.Traducir("El parte no está abierto en una terminal de ventas");
                }
            }

            if (puedeEntregar)
            {
                //El tag está asignado a un cliente 
                ClienteL oClientes = ClienteDT.getDatosClienteInt(conn, null, null, null, null, null, oEntrega.NumeroTag, null, null, null, null);
                if (oClientes.Count > 0)
                {
                    puedeEntregar = false;
                    causa = Traduccion.Traducir("El tag ingresado se encuentra habilitado para el cliente: ") + "\n";
                    foreach (Cliente cliente in oClientes)
                    {
                        causa = causa + cliente.NumeroCliente.ToString() + " - " + cliente.RazonSocial + "\n";
                    }
                }
            }

            if (puedeEntregar)
            {
                //La patente  o tag tiene una entrega pendiente de facturar
                EntregaTagL oEntregas = TagDT.getEntregasNoFacturadasValid(conn, oEntrega.Patente, oEntrega.NumeroTag);
                if (oEntregas.Count > 0)
                {
                    puedeEntregar = false;
                    causa = Traduccion.Traducir("El Tag o el Vehículo tenía operaciones pendientes de facturar: ") + "\n";
                    foreach (EntregaTag entrega in oEntregas)
                    {
                        causa = causa + entrega.Patente + " - " + Traduccion.Traducir("Tag") + ":" + entrega.NumeroTag +
                                " - " + Traduccion.Traducir("Cliente") + ":" + entrega.Cliente.NumeroCliente.ToString() + " - " + entrega.Cliente.RazonSocial + "\n";
                    }
                }
            }

            return puedeEntregar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Graba la entrega. Si no tiene costo, invocamos metodo para grabarlo en el maestro.
        /// </summary>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void GuardarEntrega(EntregaTag oEntrega)
        {
            string causa = "";

            using (Conexion conn = new Conexion())
            {
                //con transaccion distribuida
                conn.ConectarPlaza(true, true);

                //Si el importe es 0 entonces no debe abonar
                if (oEntrega.Monto <= 0)
                    oEntrega.Abona = "N";

                //Verficamos que puede hacer la entrega
                if (!PuedeEntregarTag(conn, oEntrega, out causa))
                {
                    throw new ErrorFacturacionStatus(causa);
                }

                // Guardamos la entrega
                TagDT.GuardarEntrega(conn, oEntrega);

                //Grabamos auditoria de la entrega
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaTag(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oEntrega),
                                                        getAuditoriaDescripcion(oEntrega)),
                                                        conn);

                // Si no abona el medio, lo grabo en la tabla de maestros de tags (en GESTION)
                if (!oEntrega.esDebeAbonar || oEntrega.esPospago)
                {
                    using (Conexion connGST = new Conexion())
                    {
                        // Abrimos una conexion con gestion, la transaccion ya la tenemos
                        connGST.ConectarGST(true, true);
                        TagDT.GrabarEnMaestro(connGST, oEntrega);
                        connGST.Finalizar(true);
                    }
                }

                conn.Finalizar(true);

            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Anula (elimina) la entrega pendiente de facturar.
        /// </summary>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularEntrega(EntregaTag oEntrega)
        {
            using (Conexion conn = new Conexion())
            {
                //con transaccion
                conn.ConectarPlaza(true);

                // Guardamos la entrega
                TagDT.AnularEntrega(conn, oEntrega);

                //Grabamos auditoria de la entrega
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEntregaTag(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oEntrega),
                                                        getAuditoriaDescripcion(oEntrega)),
                                                        conn);

                // Si no abona el medio, lo sacamos en la tabla de maestros de tags (en GESTION)
                if (!oEntrega.esDebeAbonar || oEntrega.esPospago)
                {
                    using (Conexion connGST = new Conexion())
                    {
                        // Abrimos una conexion con gestion, la transaccion ya la tenemos
                        connGST.ConectarGST(true, true);
                        TagDT.EliminarEnMaestro(connGST, oEntrega);
                        connGST.Finalizar(true);
                    }
                }

                // Termino toda la transaccion
                conn.Finalizar(true);

            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los datos completos de una entrega (para impresion)
        /// </summary>
        /// <param name="entrega">EntregaTag - objeto de la entrega</param>
        /// <returns>DataSet con el detalle de la entrega</returns>
        /// ***********************************************************************************************
        public static DataSet getEntregaDetalle(EntregaTag entrega, int cantidadCopias)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return TagDT.getEntregaDetalle(conn, entrega.Parte.Numero, entrega.Cliente, entrega.Identity, cantidadCopias);

            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un tag
        /// </summary>
        /// <param name="oTag">Tag - Objeto del tag que contiene la informacion a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void AnularTag(Tag oTag)
        {
            using (Conexion conn = new Conexion())
            {
                //con transaccion
                conn.ConectarGST(true);

                // Eliminamos el Tag
                TagDT.EliminarEnMaestro(conn, oTag);

                //Grabamos auditoria de la anulación
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTag(),
                                                        "E",
                                                        getAuditoriaCodigoRegistro(oTag),
                                                        getAuditoriaDescripcion(oTag)),
                                                        conn);

                // Termino toda la transaccion
                conn.Finalizar(true);

            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el Tag de una patente
        /// </summary>
        /// <param name="patente">Patente</param>
        /// <returns>Objeto Tag correspondiente a la patente en cuestión</returns>
        /// ***********************************************************************************************
        public static Tag getTagPatente(string patente)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTThenPlaza();
                return TagDT.getTagPatente(conn, patente);
            }
        }

        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Graba en la tabla de maestros la entrega realizada. 
        /// Es un metodo privado para ser llamado cuando una entrega no tiene costo. En este caso se graba la entrega y en el maestro
        /// </summary>
        /// <param name="oEntrega">EntregaTag - Objeto de la entrega que contiene la informacion a grabar</param>
        /// <param name="conn">Conexion - Coneccion con la base de datos en la que impactara la entrega. Ya contiene la transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void GrabarEnMaestro(Conexion conn, EntregaTag oEntrega)
        {
            try
            {
                // Grabamos en la tabla de maestros
                TagDT.GrabarEnMaestro(conn, oEntrega);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaEntregaTag()
        {
            return "ENT";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(EntregaTag oEntregaTag)
        {
            return oEntregaTag.NumeroTag;
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(EntregaTag oEntregaTag)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Patente", oEntregaTag.Patente);
            AuditoriaBs.AppendCampo(sb, "Fecha de Entrega", oEntregaTag.FechaOperacion.ToString());
            AuditoriaBs.AppendCampo(sb, "Estación", oEntregaTag.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Cliente", oEntregaTag.Cliente.NumeroCliente.ToString() + "-" + oEntregaTag.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Cuenta", oEntregaTag.Cuenta.Numero + "-" + oEntregaTag.Cuenta.TipoCuenta.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Costo del Dispositivo", oEntregaTag.MontoString);
            AuditoriaBs.AppendCampo(sb, "Abona Medio", oEntregaTag.esDebeAbonarString);
            AuditoriaBs.AppendCampo(sb, "Reposición", oEntregaTag.esReposicionString);


            return sb.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTag()
        {
            return "CLV";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Tag oTag)
        {
            return oTag.Patente;
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Tag oTag)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Eliminar Tag", oTag.NumeroTag);
            return sb.ToString();
        }

        #endregion

        #endregion

        #region TAGLISTANEGRA: Metodos de la Clase de Negocios de la entidad TAGS en LISTA NEGRA.

        public static void GuardarTagListaNegra(TagListaNegra oTagListaNegra)
        {
            using (Conexion conn = new Conexion())
            {
                //Conectamos siempre con gestion
                conn.ConectarGST(true, false);

                oTagListaNegra.Usuario = new Usuario(ConexionBs.getUsuario(), ConexionBs.getUsuarioNombre());
                oTagListaNegra.FechaInhabilitacion = DateTime.Now;

                // Guardamos el tag en la lista negra
                TagDT.addTagListaNegra(conn, oTagListaNegra);

                //Grabamos auditoria de la entrega
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTagListaNegra(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oTagListaNegra),
                                                        getAuditoriaDescripcion(oTagListaNegra)),
                                                        conn);

                // Termino toda la transaccion
                conn.Finalizar(true);
            }
        }

        public static void EliminarTagListaNegra(TagListaNegra oTagListaNegra)
        {
            using (Conexion conn = new Conexion())
            {
                //Conectamos siempre con gestion
                conn.ConectarGST(true, false);

                // Guardamos el tag en la lista negra
                TagDT.delTagListaNegra(conn, oTagListaNegra);

                //Grabamos auditoria de la entrega
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTagListaNegra(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oTagListaNegra),
                                                        getAuditoriaDescripcion(oTagListaNegra)),
                                                        conn);

                // Termino toda la transaccion
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tags en lista negra que coinciden con el buscado
        /// </summary>
        /// <param name="numeroTag">string - Numero de Tag por el cual buscar</param>
        /// <returns>Lista de Tags en lista negra que coinciden con el buscado</returns>
        /// ***********************************************************************************************
        public static TagListaNegraL getTagListaNegra(string numeroTag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return TagDT.getTagListaNegra(conn, numeroTag);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tags en lista negra que coinciden con el buscado, pero busca en Gestion
        /// </summary>
        /// <param name="numeroTag">string - Numero de Tag por el cual buscar</param>
        /// <returns>Lista de Tags en lista negra que coinciden con el buscado</returns>
        /// ***********************************************************************************************
        public static TagListaNegraL getTagListaNegra(bool bGST, string numeroTag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);
                return TagDT.getTagListaNegra(conn, numeroTag);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTagListaNegra()
        {
            return "LNE";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(TagListaNegra oTagListaNegra)
        {
            return oTagListaNegra.Patente;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(TagListaNegra oTagListaNegra)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Número de Tag", oTagListaNegra.NumeroTag);
            AuditoriaBs.AppendCampo(sb, "Fecha Inhabilitación", oTagListaNegra.FechaInhabilitacion.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Usuario", oTagListaNegra.Usuario.Nombre);
            AuditoriaBs.AppendCampo(sb, "Comentario", oTagListaNegra.Comentario);

            return sb.ToString();
        }

        #endregion

        #endregion
    }
}

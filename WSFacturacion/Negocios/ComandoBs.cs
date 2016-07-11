using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;
using System.Web;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class ComandoBs
    {

        #region CodigoComando

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todos los Codigos de comandos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 20/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...:
        // ----------------------------------------------------------------------------------------------

        public static CodigoComandoL getCodigosComando()
        {
            return getCodigosComando(null);
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de todos los Codigos de comandos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 20/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xTipo - string - Permite filrar por tipo de comando
        // ----------------------------------------------------------------------------------------------

        public static CodigoComandoL getCodigosComando(string xsTipoComando)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return ComandoDt.getCodigosComando(conn, xsTipoComando);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Comando: Metodos de la Clase Comando.

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve la lista de comandos existentes
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 29/10/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parametros:
        //                                  xiEstacion - string? - Permite filrar por código de estación
        // ----------------------------------------------------------------------------------------------

        public static ComandoL getComandos(int xiEstacion, string xsStatus, string xVias, DateTime dtFechaHoraDesde, DateTime dtFechaHoraHasta)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    ViaL lListaVias = new ViaL();

                    // PASAR DE LA LISTA DE STRING A LA LISTA DE OBJETOS CORRESPONDIENTES

                    // VIAS
                    string[] auxVias = xVias.Split(',');

                    if (auxVias[0] != "")
                    {
                        foreach (string word in auxVias)
                        {
                            lListaVias.Add(new Via(xiEstacion, Convert.ToByte(word),""));
                        }
                    }

                    return ComandoDt.getComandos(conn, xiEstacion, xsStatus, lListaVias, dtFechaHoraDesde, dtFechaHoraHasta);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Comando
        /// </summary>
        /// <param name="oComando">Comando - Estructura del Comando a insertar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addComando(Comando oComando)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGSTPlaza(false, true);

                //Agregamos el Objeto
                ComandoDt.addComando(oComando, conn);

                //////Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaComando(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oComando),
                                                        getAuditoriaDescripcion(oComando)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        //////public static ViaComandoL getViasComandos(int? xiEstacion)
        //////{
        //////    try
        //////    {
        //////        using (Conexion conn = new Conexion())
        //////        {
        //////            //sin transaccion                    
        //////            conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
        //////            return ViaDt.getViasComandos(conn, xiEstacion);
        //////        }
        //////    }
        //////    catch (Exception ex)
        //////    {
        //////        throw ex;
        //////    }
        //////}


        ///////////////// ***********************************************************************************************
        ///////////////// <summary>
        ///////////////// Modificacion de un TipoDiaHora
        ///////////////// </summary>
        ///////////////// <param name="oTipoDiaHora"></param>

        ///////////////// <returns></returns>
        ///////////////// ***********************************************************************************************
        //////////////public static void updTipoDiaHora(TipoDiaHora oTipoDiaHora)
        //////////////{
        //////////////    try
        //////////////    {
        //////////////        //iniciamos una transaccion
        //////////////        using (Conexion conn = new Conexion())
        //////////////        {
        //////////////            //Siempre en Gestion, con transaccion
        //////////////            conn.ConectarGST(true);

        //////////////            //Verificamos que ya exista el TipoDiaHora

        //////////////            //Modificamos el TipoDiaHora

        //////////////            TipoDiaHoraDt.updTipoDiaHora(oTipoDiaHora, conn);

        //////////////            //Grabamos auditoria
        //////////////            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoDiaHora(),
        //////////////                                                   "M",
        //////////////                                                   getAuditoriaCodigoRegistro(oTipoDiaHora),
        //////////////                                                   getAuditoriaDescripcion(oTipoDiaHora)),
        //////////////                                                   conn);

        //////////////            //Grabo OK hacemos COMMIT
        //////////////            conn.Finalizar(true);
        //////////////        }
        //////////////    }
        //////////////    catch (Exception ex)
        //////////////    {
        //////////////        throw ex;
        //////////////    }
        //////////////}


        ///////////////// ***********************************************************************************************
        ///////////////// <summary>
        ///////////////// Eliminacion de un TipoDiaHora
        ///////////////// </summary>
        ///////////////// <param name="oTipoDiaHora">TipoDiaHora - Estructura del TipoDiaHora a eliminar
        ///////////////// <returns></returns>
        ///////////////// ***********************************************************************************************
        //////////////public static void delTipoDiaHora(TipoDiaHora oTipoDiaHora, bool nocheck)
        //////////////{
        //////////////    try
        //////////////    {
        //////////////        //iniciamos una transaccion
        //////////////        using (Conexion conn = new Conexion())
        //////////////        {
        //////////////            //Siempre en Gestion, con transaccion
        //////////////            conn.ConectarGST(true);

        //////////////            //Verificar que no haya registros con FK a este
        //////////////            MantenimientoBS.checkReferenciasFK(conn, "Peaje.tipdho", oTipoDiaHora.Codigo, "", nocheck);

        //////////////            //eliminamos el TipoDiaHora
        //////////////            TipoDiaHoraDt.delTipoDiaHora(oTipoDiaHora, conn);

        //////////////            //Grabamos auditoria
        //////////////            AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoDiaHora(),
        //////////////                               "B",
        //////////////                               getAuditoriaCodigoRegistro(oTipoDiaHora),
        //////////////                               getAuditoriaDescripcion(oTipoDiaHora)),
        //////////////                               conn);

        //////////////            //Grabo OK hacemos COMMIT
        //////////////            conn.Finalizar(true);
        //////////////        }
        //////////////    }
        //////////////    catch (Exception ex)
        //////////////    {
        //////////////        throw ex;
        //////////////    }
        //////////////}



        #endregion


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaComando()
        {
            return "COA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Comando oComando)
        {
            return oComando.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Comando oComando)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "FechaPedido", oComando.FechaPedido.ToString().Trim());
            AuditoriaBs.AppendCampo(sb, "TipoComando", oComando.Tipo);
            AuditoriaBs.AppendCampo(sb, "CodigoComando", oComando.Codigo);
            AuditoriaBs.AppendCampo(sb, "Estacion", oComando.Estacion.ToString().Trim());
            AuditoriaBs.AppendCampo(sb, "NroVia", oComando.NumeroVia.ToString().Trim());
            AuditoriaBs.AppendCampo(sb, "UsuarioSolicitante", oComando.Usuario);
            AuditoriaBs.AppendCampo(sb, "FechaEjecucion", oComando.FechaEjecucion.ToString().Trim());
            AuditoriaBs.AppendCampo(sb, "FechaVencimiento", oComando.FechaVencimiento.ToString().Trim());
            AuditoriaBs.AppendCampo(sb, "Status", oComando.Status);
            AuditoriaBs.AppendCampo(sb, "Parametros", oComando.Parametros.Trim());

            return sb.ToString();
        }

        #endregion

    }
}

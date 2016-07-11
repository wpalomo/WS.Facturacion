using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class CodAutorizacionElectronicaBs
    {
        #region CODCAE:CREATE/READ/UPDATE/DELETE CODCAE (CRUD)



        ///*******************************************************************************************************
        /// <summary>
        /// Get lista completa de codigos CAE, TRUE implica que el rango total esta disponible.
        /// </summary>
        /// <param name="contItems">Cantidad de Items</param>
        /// <param name="cantxVia">Vias</param>
        /// <param name="inicioRango">Rango Inicio</param>
        /// <param name="idCae">ID CAE</param>
        /// <returns></returns>
        ///*******************************************************************************************************
        public static bool comprobarNuevoVCE(int? contItems, int? cantxVia, int inicioRango, int idCae, int? finalRango, int? identity, string Comprobante)
        {
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarGST(false);
                    return CodAutorizacionElectronicaDt.comprobarNuevoVCE(con, contItems, cantxVia, inicioRango, idCae, finalRango, identity, Comprobante);

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        ///*****************************************************************************************************
        /// <summary>
        /// Sugiere el proximo numero de bloque segun el CODCAE seleccionado
        /// </summary>
        /// <param name="Idcodcae">Codigo CAE</param>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static int getProximoBloque(int Idcodcae)
        {
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarGST(false);
                    return CodAutorizacionElectronicaDt.getProximoBloque(con, Idcodcae);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        ///*****************************************************************************************************
        /// <summary>
        /// Obtiene el detalle de los CAE
        /// </summary>
        /// <param name="ident">Identity CODCAE</param>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static CodAutorizacionElectronica getCodigoCaeDetalle(int ident)
        {
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarGST(false);
                    return CodAutorizacionElectronicaDt.getCodigoCaeDetalle(con, ident);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }



        ///*****************************************************************************************************
        /// <summary>
        /// Lista de codigos cae para el DROPDOWN de la busqueda, trae lo codigos sin  repetirse
        /// </summary>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static List<string> getCodigosCAEdiferente()
        {
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarGST(false);
                    return CodAutorizacionElectronicaDt.getCodigosCaeDiferente(con);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }


        ///*****************************************************************************************************
        /// <summary>
        /// Obtiene los codigo de cae
        /// </summary>
        /// <returns></returns>
        ///*****************************************************************************************************
        public static CodAutorizacionElectronicaL getCodigosCAE()
        {
            try
            {
                using (Conexion con = new Conexion())
                {
                    con.ConectarGST(false);

                    return CodAutorizacionElectronicaDt.getCodigosCae(con);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        ///*******************************************************************************************************
        /// <summary>
        /// Elimina CAE
        /// </summary>
        /// <param name="oCod"></param>
        ///*******************************************************************************************************
        public static void delCodigoCAE(CodAutorizacionElectronica oCod)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGST(false);
                    CodAutorizacionElectronicaDt.delCodigoCae(conn, oCod);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                    "B",
                    getAuditoriaCodigoRegistro(oCod),
                    getAuditoriaDescripcion(oCod)),
                    conn);
                }
            }
            catch (Exception e)
            {

                throw e;
            }
            return;

        }

        ///*******************************************************************************************************
        /// <summary>
        /// Actualiza codigo cae, solo en el caso que no haya sido asignado a una vía.
        /// </summary>
        /// <param name="oCod"></param>
        ///*******************************************************************************************************
        public static void updCodigoCAE(CodAutorizacionElectronica oCod)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGST(false);
                    CodAutorizacionElectronicaDt.updCodigoCae(conn, oCod);

                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                    "U",
                    getAuditoriaCodigoRegistro(oCod),
                    getAuditoriaDescripcion(oCod)),
                    conn);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return;
        }



        ///*******************************************************************************************************
        /// <summary>
        /// Agrega codigocae a CODCAE, Tambien verifica que no se solapen los codigos
        /// </summary>
        /// <param name="oCod"></param>
        ///*******************************************************************************************************
        public static void addCodigoCAE(CodAutorizacionElectronica oCod)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarGST(false);
                    CodAutorizacionElectronicaDt.addCodigoCae(conn, oCod);


                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                        "A",
                                        getAuditoriaCodigoRegistro(oCod),
                                        getAuditoriaDescripcion(oCod)),
                                        conn);

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return;
        }
        #endregion

        #region AUDITORIA

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "CAE";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(CodAutorizacionElectronica oCod)
        {
            return oCod.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(CodAutorizacionElectronica oCod)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Usuario", UsuarioBs.getUsuarioLogueado().ID.ToString());

            if (!string.IsNullOrEmpty(oCod.Codigo))
                AuditoriaBs.AppendCampo(sb, "Codigo", oCod.Codigo.ToString());
            if (!string.IsNullOrEmpty(oCod.Comprobante))
                AuditoriaBs.AppendCampo(sb, "Comprobante", oCod.Comprobante.ToString());
            if (!string.IsNullOrEmpty(oCod.sTipo))
                AuditoriaBs.AppendCampo(sb, "Tipo", oCod.sTipo.ToString());
            if (!string.IsNullOrEmpty(oCod.Serie.ToString()))
                AuditoriaBs.AppendCampo(sb, "Serie", oCod.Serie.ToString());
            if (!string.IsNullOrEmpty(oCod.NumeroInicial.ToString()))
                AuditoriaBs.AppendCampo(sb, "Número Inicial", oCod.NumeroInicial.ToString());
            if (!string.IsNullOrEmpty(oCod.NumeroFinal.ToString()))
                AuditoriaBs.AppendCampo(sb, "Número Final", oCod.NumeroFinal.ToString());


            return sb.ToString();
        }

     


        #endregion


    }
}

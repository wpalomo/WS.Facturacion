using System;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class TipoCuponBs
    {
        #region TipoCupon: Metodos de la Clase de Negocios de la entidad TipoCupon.
        
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static TipoCuponL getTiposCupones()
        {
            return getTiposCupones(ConexionBs.getGSToEstacion(), null);
        }

        /// <summary>
        /// Devuelve un elemento
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static TipoCupon getTipoCupon(int codigo)
        {
            return (getTiposCupones(ConexionBs.getGSToEstacion(), codigo))[0];
        }

        /// <summary>
        /// Devuelve la lista 
        /// </summary>
        /// <param name="bGST"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static TipoCuponL getTiposCupones(bool bGST, int? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return TipoCuponDt.getTiposCupones(conn, codigo);
            }
        }

        /// <summary>
        /// Agrega un TipoCupon
        /// </summary>
        /// <param name="tipoCupon"></param>
        public static void addTipoCupon(TipoCupon tipoCupon)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos TipoCupon
                TipoCuponDt.addTipoCupon(tipoCupon, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoCupon(),
                                                       "A",
                                                       getAuditoriaCodigoRegistro(tipoCupon),
                                                       getAuditoriaDescripcion(tipoCupon)),
                                                       conn);
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// <summary>
        /// Actualizacion de un tipo de cupon
        /// </summary>
        /// <param name="oEstacion"></param>
        /// <returns></returns>
        public static void updTipoCupon(TipoCupon tipoCupon)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                TipoCuponDt.updTipoCupon(tipoCupon, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoCupon(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(tipoCupon),
                                                        getAuditoriaDescripcion(tipoCupon)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Eliminación de un TipoCupon
        /// </summary>
        /// <param name="oEstacion"></param>
        /// <returns></returns>
        public static void delTipoCupon(TipoCupon tipoCupon)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);
                
                //Modificamos estacion
                TipoCuponDt.delTipoCupon(tipoCupon, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoCupon(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(tipoCupon),
                                                        getAuditoriaDescripcion(tipoCupon)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>
        private static string getAuditoriaCodigoAuditoriaTipoCupon()
        {
            return "TCU";
        }

        /// <summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>
        private static string getAuditoriaCodigoRegistro(TipoCupon tipoCupon)
        {
            return tipoCupon.Codigo.ToString();
        }

        /// <summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>
        private static string getAuditoriaDescripcion(TipoCupon tipoCupon)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", tipoCupon.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Vale aceptado en la via", Traduccion.getSiNo(tipoCupon.UsoEnVia));
            AuditoriaBs.AppendCampo(sb, "Posee código de barra", Traduccion.getSiNo(tipoCupon.ConCodigoDeBarra));
            AuditoriaBs.AppendCampo(sb, "Tipo de Tarifa", tipoCupon.TipoTarifa.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Registrar los cupones recibidos", Traduccion.getSiNo(tipoCupon.RegistraCupones));

            return sb.ToString();
        }

        #endregion        
    }
}

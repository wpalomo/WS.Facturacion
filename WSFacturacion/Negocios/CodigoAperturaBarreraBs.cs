using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class CodigoAperturaBarreraBs
    {
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static CodigoAperturaBarreraL getCodigosAperturaBarrera()
        {
            return getCodigosAperturaBarrera(ConexionBs.getGSToEstacion(), null);
        }

        /// <summary>
        /// Devuelve un elemento
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static CodigoAperturaBarrera getCodigoAperturaBarrera(byte codigo)
        {
            return (getCodigosAperturaBarrera(ConexionBs.getGSToEstacion(), codigo))[0];
        }

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cupones. 
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="Fecha">byte - Permite filtrar por código</param>
        /// <returns>Lista de Tipos Cupones</returns>
        public static CodigoAperturaBarreraL getCodigosAperturaBarrera(bool bGST, byte? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return CodigoAperturaBarreraDt.getCodigosAperturaBarrera(conn, codigo);
            }
        }

        /// <summary>
        /// Agrega un TipoCupon
        /// </summary>
        /// <param name="tipoCupon"></param>
        public static void addCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos TipoCupon
                CodigoAperturaBarreraDt.addCodigoAperturaBarrera(codigoAperturaBarrera, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoAperturaBarrera(),
                                                       "A",
                                                       getAuditoriaCodigoRegistro(codigoAperturaBarrera),
                                                       getAuditoriaDescripcion(codigoAperturaBarrera)),
                                                       conn);
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Actualizacion de un tipo de cupon
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto Estacion de Peaje
        /// <returns></returns>
        public static void updCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                CodigoAperturaBarreraDt.updCodigoAperturaBarrera(codigoAperturaBarrera, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoAperturaBarrera(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoAperturaBarrera),
                                                        getAuditoriaDescripcion(codigoAperturaBarrera)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Eliminaciòn de un TipoCupon
        /// </summary>
        /// <param name="oEstacion">Estacion - Objeto Estacion de Peaje
        /// <returns></returns>
        public static void delCodigoAperturaBarrera(CodigoAperturaBarrera codigoAperturaBarrera)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                CodigoAperturaBarreraDt.delCodigoAperturaBarrera(codigoAperturaBarrera, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoAperturaBarrera(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoAperturaBarrera),
                                                        getAuditoriaDescripcion(codigoAperturaBarrera)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>
        private static string getAuditoriaCodigoAuditoriaCodigoAperturaBarrera()
        {
            return "CAB";
        }

        /// <summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>
        private static string getAuditoriaCodigoRegistro(CodigoAperturaBarrera codigoAperturaBarrera)
        {
            return codigoAperturaBarrera.Codigo.ToString();
        }

        /// <summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>
        private static string getAuditoriaDescripcion(CodigoAperturaBarrera codigoAperturaBarrera)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Código", codigoAperturaBarrera.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Descripción", codigoAperturaBarrera.Descripcion);

            return sb.ToString();
        }
    }
}

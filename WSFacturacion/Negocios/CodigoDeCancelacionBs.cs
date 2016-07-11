using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class CodigoDeCancelacionBs
    {
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static CodigoDeCancelacionL getCodigosDeCancelacion()
        {
            return getCodigosDeCancelacion(ConexionBs.getGSToEstacion(), null);
        }

        /// <summary>
        /// Devuelve un elemento
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static CodigoDeCancelacion getCodigoDeCancelacion(byte codigo)
        {
            return (getCodigosDeCancelacion(ConexionBs.getGSToEstacion(), codigo))[0];
        }

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cupones. 
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="Fecha">byte - Permite filtrar por código</param>
        /// <returns>Lista de Tipos Cupones</returns>
        public static CodigoDeCancelacionL getCodigosDeCancelacion(bool bGST, byte? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return CodigoDeCancelacionDt.getCodigosDeCancelacion(conn, codigo);
            }
        }

        /// <summary>
        /// Agrega un TipoCupon
        /// </summary>
        /// <param name="tipoCupon"></param>
        public static void addCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos TipoCupon
                CodigoDeCancelacionDt.addCodigoDeCancelacion(codigoDeCancelacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeCancelacion(),
                                                       "A",
                                                       getAuditoriaCodigoRegistro(codigoDeCancelacion),
                                                       getAuditoriaDescripcion(codigoDeCancelacion)),
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
        public static void updCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                CodigoDeCancelacionDt.updCodigoDeCancelacion(codigoDeCancelacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeCancelacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoDeCancelacion),
                                                        getAuditoriaDescripcion(codigoDeCancelacion)),
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
        public static void delCodigoDeCancelacion(CodigoDeCancelacion codigoDeCancelacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                CodigoDeCancelacionDt.delCodigoDeCancelacion(codigoDeCancelacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeCancelacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoDeCancelacion),
                                                        getAuditoriaDescripcion(codigoDeCancelacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>
        private static string getAuditoriaCodigoAuditoriaCodigoDeCancelacion()
        {
            return "CCN";
        }

        /// <summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>
        private static string getAuditoriaCodigoRegistro(CodigoDeCancelacion codigoDeCancelacion)
        {
            return codigoDeCancelacion.Codigo.ToString();
        }

        /// <summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>
        private static string getAuditoriaDescripcion(CodigoDeCancelacion codigoDeCancelacion)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Código", codigoDeCancelacion.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Descripción", codigoDeCancelacion.Descripcion);

            return sb.ToString();
        }
    }
}

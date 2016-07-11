using System;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class CodigoDeSimulacionBs
    {        
        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static CodigoDeSimulacionL getCodigosDeSimulaciones()
        {
            return getCodigosDeSimulaciones(ConexionBs.getGSToEstacion(), null);
        }

        /// <summary>
        /// Devuelve un elemento
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static CodigoDeSimulacion getCodigoDeSimulacion(byte codigo)
        {
            return (getCodigosDeSimulaciones(ConexionBs.getGSToEstacion(), codigo))[0];
        }

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cupones. 
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="Fecha">byte - Permite filtrar por código</param>
        /// <returns>Lista de Tipos Cupones</returns>
        public static CodigoDeSimulacionL getCodigosDeSimulaciones(bool bGST, byte? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return CodigoDeSimulacionDt.getCodigosDeSimulaciones(conn, codigo);
            }
        }

        /// <summary>
        /// Agrega un TipoCupon
        /// </summary>
        /// <param name="tipoCupon"></param>
        public static void addCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos TipoCupon
                CodigoDeSimulacionDt.addCodigoDeSimulacion(codigoDeSimulacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeSimulacion(),
                                                       "A",
                                                       getAuditoriaCodigoRegistro(codigoDeSimulacion),
                                                       getAuditoriaDescripcion(codigoDeSimulacion)),
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
        public static void updCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                CodigoDeSimulacionDt.updCodigoDeSimulacion(codigoDeSimulacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeSimulacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoDeSimulacion),
                                                        getAuditoriaDescripcion(codigoDeSimulacion)),
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
        public static void delCodigoDeSimulacion(CodigoDeSimulacion codigoDeSimulacion)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);
                
                //Modificamos estacion
                CodigoDeSimulacionDt.delCodigoDeSimulacion(codigoDeSimulacion, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCodigoDeSimulacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(codigoDeSimulacion),
                                                        getAuditoriaDescripcion(codigoDeSimulacion)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>
        private static string getAuditoriaCodigoAuditoriaCodigoDeSimulacion()
        {
            return "CSM";
        }

        /// <summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>
        private static string getAuditoriaCodigoRegistro(CodigoDeSimulacion codigoDeSimulacion)
        {
            return codigoDeSimulacion.Codigo.ToString();
        }

        /// <summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>
        private static string getAuditoriaDescripcion(CodigoDeSimulacion codigoDeSimulacion)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Código", codigoDeSimulacion.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Descripción", codigoDeSimulacion.Descripcion);

            return sb.ToString();
        }
    }
}

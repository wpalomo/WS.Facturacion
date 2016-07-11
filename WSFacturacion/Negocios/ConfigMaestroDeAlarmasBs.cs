using System;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class ConfigMaestroDeAlarmasBs
    {
        #region Métodos

        /// <summary>
        /// Devuelve la lista
        /// </summary>
        /// <returns></returns>
        public static ConfigMaestroDeAlarmasL getConfigMaestroDeAlarmas()
        {
            return getConfigMaestroDeAlarmas(ConexionBs.getGSToEstacion(), null);
        }

        /// <summary>
        /// Devuelve un elemento
        /// </summary>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static ConfigMaestroDeAlarmas getConfigMaestroAlarma(byte codigo)
        {
            return (getConfigMaestroDeAlarmas(ConexionBs.getGSToEstacion(), codigo))[0];
        }

        /// <summary>
        /// Devuelve la lista 
        /// </summary>
        /// <param name="bGST"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>
        public static ConfigMaestroDeAlarmasL getConfigMaestroDeAlarmas(bool bGST, byte? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return ConfigMaestroDeAlarmasDt.getConfigMaestroDeAlarmas(conn, codigo);
            }
        }
        
        /// <summary>
        /// Actualizacion de un tipo de cupon
        /// </summary>
        /// <param name="oEstacion"></param>
        /// <returns></returns>
        public static void updConfigMaestroDeAlarma(ConfigMaestroDeAlarmas alarma)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos estacion
                ConfigMaestroDeAlarmasDt.updConfigMaestroDeAlarmas(alarma, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaConfigMaestroDeAlarmas(),
                                                        "M",
                                                        getAuditoriaCodigoConfigMaestroDeAlarmas(alarma),
                                                        getAuditoriaDescripcion(alarma)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// <summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>
        private static string getAuditoriaCodigoAuditoriaConfigMaestroDeAlarmas()
        {
            return "CAL";
        }

        /// <summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>
        private static string getAuditoriaCodigoConfigMaestroDeAlarmas(ConfigMaestroDeAlarmas alarma)
        {
            return alarma.Codigo.ToString();
        }

        /// <summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>
        private static string getAuditoriaDescripcion(ConfigMaestroDeAlarmas alarma)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", alarma.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Repeticiones para Alarma", alarma.Veces.ToString());
            AuditoriaBs.AppendCampo(sb, "Tipo de sonido", Traduccion.Traducir(alarma.Sonido.Descripcion));

            return sb.ToString();
        }

        /// <summary>
        /// Obtiene todos los tipos de sonido
        /// </summary>
        /// <returns></returns>
        public static TipoDeSonidoL getTiposDeSonidos()
        {
            var tiposDeSonidos = new TipoDeSonidoL();

            tiposDeSonidos.Add(new TipoDeSonido(null));
            tiposDeSonidos.Add(new TipoDeSonido(1));

            return tiposDeSonidos;
        }

        #endregion        
    }
}

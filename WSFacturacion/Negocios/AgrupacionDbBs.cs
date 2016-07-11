using System;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Agrupacion de base de datos
    /// </summary>
    ///****************************************************************************************************
    public static class AgrupacionDbBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las agrupaciones de base de datos definidas. 
        /// </summary>
        /// <returns>Lista de agrupaciones de base de datos</returns>
        /// ***********************************************************************************************
        /// 
        public static AgrupacionDbL getAgrupaciones()
        {
            return getAgrupaciones(null);
        }

        public static AgrupacionDbL getAgrupaciones(int? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTThenPlaza();
                return AgrupacionDbDt.getAgrupaciones(conn,codigo);
            }
        }


        public static void addAgrupacionDb(AgrupacionDb oAgrupacionDb)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);
                AgrupacionDbDt.addAgrupacionDb(conn, oAgrupacionDb);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacion(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oAgrupacionDb),
                                                        getAuditoriaDescripcion(oAgrupacionDb)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        public static void updAgrupacionDb(AgrupacionDb oAgrupacionDb)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);
                AgrupacionDbDt.updAgrupacionDb(conn, oAgrupacionDb);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacion(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oAgrupacionDb),
                                                        getAuditoriaDescripcion(oAgrupacionDb)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        public static void delAgrupacionDb(AgrupacionDb oAgrupacionDb, bool nocheck)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "agrudb",
                                                    new string[] { oAgrupacionDb.Codigo.ToString() },
                                                    new string[] { },
                                                    nocheck);

                AgrupacionDbDt.delAgrupacionDb(conn, oAgrupacionDb);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacion(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oAgrupacionDb),
                                                        getAuditoriaDescripcion(oAgrupacionDb)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaAgrupacion()
        {
            return "AUD";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(AgrupacionDb oAgrupacionDb)
        {
            return oAgrupacionDb.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(AgrupacionDb oAgrupacionDb)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Base de Datos", oAgrupacionDb.BaseDatos);
            AuditoriaBs.AppendCampo(sb, "Servidor de Datos", oAgrupacionDb.ServidorDatos);
            AuditoriaBs.AppendCampo(sb, "Dirección URL", oAgrupacionDb.URL);
            AuditoriaBs.AppendCampo(sb, "Descripcion", oAgrupacionDb.Descripcion);

            return sb.ToString();
        }

        #endregion
    }
}


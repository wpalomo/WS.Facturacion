using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;
using System.Data;

namespace Telectronica.Peaje
{
    public class ExentoBs
    {
        #region EXENTO: Clase de Negocios de tipo de Exentos definidos

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene en una estructura del tipo Dictionary los valores posibles para los típos de código
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static TipoAutorizacionExentoL getTiposCodigos()
        {
            TipoAutorizacionExentoL oTipoAutorizacionExentoL = new TipoAutorizacionExentoL();

            oTipoAutorizacionExentoL.Add(new TipoAutorizacionExento("A"));
            oTipoAutorizacionExentoL.Add( new TipoAutorizacionExento("R"));
            oTipoAutorizacionExentoL.Add(new TipoAutorizacionExento("M"));

            return oTipoAutorizacionExentoL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los exentos por estaciones para reportes
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptExentosEstaciones()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ExentoDt.getRptExentosEstaciones(conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los exentos para reportes
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptExentos()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ExentoDt.getRptExentos(conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el Exento asociado a una patente dada, si la misma no fue encontrada en la base de datos
        /// se devolverá null
        /// </summary>
        /// <param name="sPatente"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ExentoCodigo getExentoByPatente(string sPatente)
        {
            var bEsGestion = ConexionBs.getGSToEstacion();

            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bEsGestion, false);
                return ExentoDt.getExentoByPatente(conn, sPatente).FirstOrDefault();
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODOS los tipos de Exentos.
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static ExentoCodigoL getExentos()
        {
            return getExentos(null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Tipos de Exentos definidos.
        /// </summary>
        /// <param name="Codigo">int - Codigo por el cual filtrar la busqueda</param>
        /// <returns>Lista de Tipo de Exentos</returns>
        /// ***********************************************************************************************
        public static ExentoCodigoL getExentos(int? Codigo)
        {
            return getExentos(ConexionBs.getGSToEstacion(), Codigo);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los tipos de Exentos definidos.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="Codigo">int - Codigo por el cual filtrar la busqueda</param>
        /// <returns>Lista de Tipo de Exentos</returns>
        /// ***********************************************************************************************
        private static ExentoCodigoL getExentos(bool bGST, int? Codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return ExentoDt.getExentos(conn, Codigo);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Tipo de Exento
        /// </summary>
        /// <param name="oExento">ExentoCodigo - Objeto ExentoCodigo
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addExento(ExentoCodigo oExento)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Valida los datos dentro del objeto
                ValidarDatosDelExento(oExento);

                //Agregamos Tipo de Exento
                ExentoDt.addExento(oExento, conn);

                //Grabamos las habilitaciones por estacion del exento
                ExentoDt.updExentoCodigo(oExento.EstacionesHabilitadas, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oExento),
                                                        getAuditoriaDescripcion(oExento)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Valida los datos dentro del objeto exento
        /// </summary>
        /// <param name="exento"></param>
        /// ***********************************************************************************************
        private static void ValidarDatosDelExento(ExentoCodigo exento)
        {
            if (exento.TipCodigoAutorizacion == null)
            {
                exento.esCodigoAutorizacionObligatoria = false;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de un tipo de Exento.
        /// </summary>
        /// <param name="oExento">ExentoCodigo - Objeto ExentoCodigo
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updExento(ExentoCodigo oExento)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Valida los datos dentro del objeto
                ValidarDatosDelExento(oExento);

                //Modificamos Tipo de Exento
                ExentoDt.updExento(oExento, conn);

                //Grabamos las habilitaciones por estacion del exento
                ExentoDt.updExentoCodigo(oExento.EstacionesHabilitadas, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oExento),
                                                        getAuditoriaDescripcion(oExento)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Baja de un Tipo de Exento
        /// </summary>
        /// <param name="oExento">ExentoCodigo - Objeto ExentoCodigo
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delExento(ExentoCodigo oExento, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "CODFRA",
                                                    new string[] { oExento.CodigoExento.ToString() },
                                                    new string[] { "FRAEST", "FORPAG_VALID" },
                                                    nocheck);

                //Eliminamos todas las habilitaciones por estacion para el exento dado.
                ExentoDt.delExentosEstaciones(oExento, conn);

                //eliminamos un Tipo de Exento
                ExentoDt.delExento(oExento, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oExento),
                                                        getAuditoriaDescripcion(oExento)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "COD";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ExentoCodigo oExentoCodigo)
        {
            return oExentoCodigo.CodigoExento.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ExentoCodigo oExentoCodigo)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", oExentoCodigo.DescrExento);
            AuditoriaBs.AppendCampo(sb, "Motivo de Exento", oExentoCodigo.ExentoMotivo.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Mostrar en Display", Traduccion.getSiNo(oExentoCodigo.MuestraDiplay == "S"));
            AuditoriaBs.AppendCampo(sb, "Habilita Supervisor", Traduccion.getSiNo(oExentoCodigo.RequiereAutorizacion == "S"));
            AuditoriaBs.AppendCampo(sb, "Código de Autorización", oExentoCodigo.TipCodigoAutorizacion.Descripcion);

            return sb.ToString();
        }

        #endregion
        
        #endregion
        
        #region EXENTOESTACION: Clase de Negocios de Estacion por tipo de Exentos
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones anexada a la lista de tipo de exentos recibido por parametro.
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static void getExentoEstacion(ExentoCodigoL oExentoL)
        {
            foreach (ExentoCodigo oExen in oExentoL)
            {
                ExentoBs.getExentoEstacion(oExen);
            }
        }
        
        /// ************************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones anexada a un objeto Exento
        /// </summary>
        /// <returns>Lista de Estaciones</returns>
        /// ***********************************************************************************************
        public static void getExentoEstacion(ExentoCodigo oExento)
        {
            ExentoBs.getExentoEstacion(ConexionBs.getGSToEstacion(), oExento);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Estaciones (con su estado de habilitacion) anexada a un objeto Exento
        /// METODO MAS INTERNO, EL QUE ACCEDE A LA DATA
        /// </summary>
        /// <returns>Lista de tipo de Exentos</returns>
        /// ***********************************************************************************************
        public static void getExentoEstacion(bool bGST, ExentoCodigo oExentoCodigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);

                // En la coleccion de formas de pago habilitadas le asigno la lista de formas de pago (metodo de data)
                oExentoCodigo.EstacionesHabilitadas = ExentoDt.getExentosEstacion(conn, oExentoCodigo.CodigoExento);
            }
        }
        
        #endregion

        #region EXENTOSMOTIVOS: Clase de Negocios de la Entidad MotivosDeExentos

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto de la entidad Motivo de Exentos
        /// </summary>
        /// <param name="sCodigo"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ExentoMotivo getMotivoDeExento(string sCodigo)
        {
            var bEsGestion = ConexionBs.getGSToEstacion();
            return (getMotivosDeExento(bEsGestion, sCodigo)).FirstOrDefault();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de la entidad Motivo de Exentos
        /// </summary>
        /// <param name="sCodigo"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ExentoMotivoL getMotivosDeExento()
        {
            var bEsGestion = ConexionBs.getGSToEstacion();
            return getMotivosDeExento(bEsGestion, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de la entidad Motivo de Exentos
        /// </summary>
        /// <param name="sCodigo"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ExentoMotivoL getMotivosDeExento(bool bGST, string sCodigo)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarGSTPlaza(bGST, false);
                return ExentoDt.getMotivoDeExento(conn, sCodigo);
            }
        }

        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las Causas de Supervisión Remota.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>        
        /// <returns>Lista de Causas</returns>
        /// ***********************************************************************************************
        public static CausaSupervisionL getCausasSupervision()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return ExentoDt.getCausasSupervision(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
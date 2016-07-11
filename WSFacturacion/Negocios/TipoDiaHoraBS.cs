using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class TipoDiaHoraBs
    {

        #region Feriados: Metodos de la Clase de Negocios de la entidad Feriado.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Feriados definidos. 
        /// </summary>
        /// <returns>Lista de Feriados</returns>
        /// ***********************************************************************************************
        public static FeriadoL getFeriados()
        {
            return getFeriados(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Feriados definidos. 
        /// </summary>
        /// <param name="Fecha">Datetime - Permite filtrar por un Feriado determinado
        /// <returns>Lista de Feriados</returns>
        /// ***********************************************************************************************
        public static FeriadoL getFeriados(DateTime? Fecha)
        {
            return getFeriados(ConexionBs.getGSToEstacion(), Fecha);
        }
        

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Feriados definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="Fecha">DateTime - Permite filtrar por un Feriado determinado
        /// <returns>Lista de Feriados</returns>
        /// ***********************************************************************************************
        public static FeriadoL getFeriados(bool bGST, DateTime? Fecha)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                     FeriadoL DiaHoraDt = TipoDiaHoraDt.getFeriados(conn, Fecha);
                    

                    return DiaHoraDt;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Feriado
        /// </summary>
        /// <param name="oFeriado">Feriado - Estructura con los datos del feriado a agregar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addFeriado(Feriado oFeriado)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que no exista la estacion

                    //Agregamos estacion
                    TipoDiaHoraDt.addFeriado(oFeriado, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oFeriado),
                                                           getAuditoriaDescripcion(oFeriado)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Feriado
        /// </summary>
        /// <param name="oFeriado">Feriado - Estructura del Feriado a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updFeriado(Feriado oFeriado)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que ya exista la estacion

                    //Modificamos estacion
                    TipoDiaHoraDt.updFeriado(oFeriado, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oFeriado),
                                                           getAuditoriaDescripcion(oFeriado)),
                                                           conn);


                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Feriado
        /// </summary>
        /// <param name="oFeriado">Feriado - Estructura del Feriado a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delFeriado(Feriado oFeriado)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verficamos que haya registros para la estacion

                    //eliminamos la estacion
                    TipoDiaHoraDt.delFeriado(oFeriado, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaEstacion(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oFeriado),
                                                           getAuditoriaDescripcion(oFeriado)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaEstacion()
            {
                return "FER";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(Feriado oFeriado)
            {
                return oFeriado.Fecha.ToShortDateString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(Feriado oFeriado)
            {
                StringBuilder sb = new StringBuilder();

                AuditoriaBs.AppendCampo(sb, "Día de Feriado",oFeriado.DiaSemana.Descripcion);

                return sb.ToString();
            }

            #endregion


        #endregion


        #region DIASEMANA: Metodos de la Clase de Negocios de la entidad DiaSemana.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Dias de Semana. 
        /// </summary>
        /// <returns>Lista de Dia Semana</returns>
        /// ***********************************************************************************************
        public static DiaSemanaL getDiasSemana()
        {
            DiaSemanaL oDiaSemana = new DiaSemanaL();
            for (int i = 1; i <= 8; i++)
            {
                oDiaSemana.Add(getDiaSemana(i));
            }

            return oDiaSemana;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Objeto Dia de la Semana.
        /// </summary>
        /// <param name="codigo">int - Codigo de dia de la semana que deseamos obtener</param>
        /// <returns>Objeto DiaSemana</returns>
        /// ***********************************************************************************************
        public static DiaSemana getDiaSemana(int codigo)
        {
            return TipoDiaHoraDt.getDiaSemana(codigo);
            
        }
        

        #endregion


        #region TipoDiaHora: Metodos de la Clase TipoDiaHora.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los TipoDiaHora definidos, sin filtro
        /// </summary>
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static TipoDiaHoraL getTiposDiaHora()
        {
            return getTiposDiaHora(ConexionBs.getGSToEstacion(), null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los TipoDiaHora definidos. 
        /// </summary>
        /// <param name="codigoTipoDiaHora">String - Permite filtrar por un TipoDiaHora determinado
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static TipoDiaHoraL getTiposDiaHora(string codigoTipoDiaHora)
        {
            return getTiposDiaHora(ConexionBs.getGSToEstacion(), codigoTipoDiaHora);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TipoDiaHora definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoTipoDiaHora">String - Permite filtrar por un TipoDiaHora determinado
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static TipoDiaHoraL getTiposDiaHora(bool bGST, string codigoTipoDiaHora)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return TipoDiaHoraDt.getTiposDiaHora(conn, codigoTipoDiaHora);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un TipoDiaHora
        /// </summary>
        /// <param name="oTipoDiaHora">TipoDiaHoa - Estructura del TipoDiaHora a insertar
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static void addTipoDiaHora(TipoDiaHora oTipoDiaHora)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos el tipo de dia y hora
                    TipoDiaHoraDt.addTipoDiaHora(oTipoDiaHora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoDiaHora(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oTipoDiaHora),
                                                           getAuditoriaDescripcion(oTipoDiaHora)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un TipoDiaHora
        /// </summary>
        /// <param name="oTipoDiaHora"></param>
        
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updTipoDiaHora(TipoDiaHora oTipoDiaHora)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificamos que ya exista el TipoDiaHora

                    //Modificamos el TipoDiaHora

                    TipoDiaHoraDt.updTipoDiaHora(oTipoDiaHora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoDiaHora(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oTipoDiaHora),
                                                           getAuditoriaDescripcion(oTipoDiaHora)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un TipoDiaHora
        /// </summary>
        /// <param name="oTipoDiaHora">TipoDiaHora - Estructura del TipoDiaHora a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delTipoDiaHora(TipoDiaHora oTipoDiaHora, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "TIPDHO", 
                                                       new string[] { oTipoDiaHora.Codigo },
                                                       new string[] { "TARIFADET","RELDIADET" },
                                                       nocheck);

                    //eliminamos el TipoDiaHora
                    TipoDiaHoraDt.delTipoDiaHora(oTipoDiaHora, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaTipoDiaHora(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oTipoDiaHora),
                                       getAuditoriaDescripcion(oTipoDiaHora)),
                                       conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaTipoDiaHora()
        {
            return "TDA";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(TipoDiaHora oTipoDiaHora)
        {
            return oTipoDiaHora.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(TipoDiaHora oTipoDiaHora)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", oTipoDiaHora.Descripcion);

            return sb.ToString();
        }

        #endregion


        #endregion


        #region BANDASHORARIAS: Metodos de negocios de la Clase de Bandas Horarias.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Bandas Horarias definidas para mostrar en la grilla, con los filtros ingresados
        /// </summary>
        /// <param name="codigoEstacion">byte - Codigo de estacion de la que deseo conocer las bandas horarias</param>
        /// <param name="sentidoCirculacion">string - Sentido de circulacion de la banda</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia de la banda</param>
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static BandaHorariaL getBandasHorariasCabecera(int? codigoEstacion, string sentidoCirculacion, 
                                                              DateTime fechaInicial, int? identity)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return TipoDiaHoraDt.getBandasHorariasCabecera(conn, codigoEstacion, sentidoCirculacion, fechaInicial, identity);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Bandas Horarias de una definicion especifica, es decir el detalle
        /// </summary>
        /// <returns>Lista de Bandas horarias pertenecientes a la cabecera</returns>
        /// ***********************************************************************************************
        public static BandaHoraria getBandasHorariasDetalle(int identity)
        {

            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    // Objeto cabecera
                    BandaHoraria oBandaCabecera = TipoDiaHoraDt.getBandasHorariasCabecera(conn, null, null, null, identity)[0];

                    // Le enlazamos las bandas horarias detalladas
                    oBandaCabecera.BandasHorarias = TipoDiaHoraDt.getBandasHorariasDetalle(conn, identity);


                    return oBandaCabecera;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una banda horaria 
        /// </summary>
        /// <param name="oBandaHoraria">BandaHoraria - Estructura con la banda horaria a insertar, con el detalle
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static void addBandaHoraria(BandaHoraria oBandaHoraria)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);


                    // Insertamos la cabecera
                    TipoDiaHoraDt.addBandaHorariaCabecera(oBandaHoraria, conn);


                    //Agregamos el detalle de las Bandas Horarias
                    foreach (BandaHorariaDetalle oBandaDetalle in oBandaHoraria.BandasHorarias)
                    {
                        TipoDiaHoraDt.addBandaHorariaDetalle(oBandaHoraria.Identity, oBandaDetalle, conn);
                    }

                    
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaBandasHorarias(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oBandaHoraria),
                                                           getAuditoriaDescripcion(oBandaHoraria)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion del detalle de tipos de dia de una banda horaria
        /// </summary>
        /// <param name="oBandaHoraria">BandaHoraria - Estructura con la banda horaria a insertar, con el detalle
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static void updBandaHoraria(BandaHoraria oBandaHoraria)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);


                    // Eliminamos el detalle para volver a insertarlo. 
                    // No tocamos la cabecera de la banda
                    TipoDiaHoraDt.delBandaHorariaDetalle(oBandaHoraria, conn);


                    //Agregamos el detalle de las Bandas Horarias
                    foreach (BandaHorariaDetalle oBandaDetalle in oBandaHoraria.BandasHorarias)
                    {
                        TipoDiaHoraDt.addBandaHorariaDetalle(oBandaHoraria.Identity, oBandaDetalle, conn);
                    }


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaBandasHorarias(),
                                                           "A",
                                                           getAuditoriaCodigoRegistro(oBandaHoraria),
                                                           getAuditoriaDescripcion(oBandaHoraria)),
                                                           conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Banda Horaria
        /// </summary>
        /// <param name="codigoEstacion">int - Codigo de estacion de la banda horaria a eliminar</param>
        /// <param name="sentidoCirculacion">string - Sentido de circulacion de la banda a eliminar</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia de la banda a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delBandaHoraria(BandaHoraria oBandaHoraria)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el detalle de laa bandas horarias
                    TipoDiaHoraDt.delBandaHorariaDetalle(oBandaHoraria, conn);

                    //Eliminamos la cabecera de la banda horaria
                    TipoDiaHoraDt.delBandaHorariaCabecera(oBandaHoraria, conn);


                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaBandasHorarias(),
                                       "B",
                                       getAuditoriaCodigoRegistro(oBandaHoraria),
                                       getAuditoriaDescripcion(oBandaHoraria)),
                                       conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


            #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

            ///****************************************************************************************************<summary>
            /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoAuditoriaBandasHorarias()
            {
                return "RDA";
            }


            ///****************************************************************************************************<summary>
            /// Codigo que identifica a la PK del registro auditado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaCodigoRegistro(BandaHoraria oBandaHoraria)
            {
                return oBandaHoraria.Identity.ToString();
            }


            ///****************************************************************************************************<summary>
            /// Descripcion a grabar con la informacion del registro afectado
            /// </summary>****************************************************************************************************
            private static string getAuditoriaDescripcion(BandaHoraria oBandaHoraria)
            {
                StringBuilder sb = new StringBuilder();

                // Cabecera del registro de auditoria
                AuditoriaBs.AppendCampo(sb, "<< CABECERA >>", "");
                AuditoriaBs.AppendCampo(sb, "Estación", oBandaHoraria.Estacion.Nombre);
                AuditoriaBs.AppendCampo(sb, "Sentido", oBandaHoraria.SentidoCirculacion.Descripcion);
                AuditoriaBs.AppendCampo(sb, "Válido Desde", oBandaHoraria.FechaInicialVigencia.ToString());
                if (oBandaHoraria.FechaFinalVigencia != null)
                    AuditoriaBs.AppendCampo(sb, "Válido Hasta", oBandaHoraria.FechaFinalVigencia.ToString());
                AuditoriaBs.AppendCampo(sb, "Intervalo", oBandaHoraria.Intervalo.Intervalo.ToString());
                
                // Detalle (bandas)
                AuditoriaBs.AppendCampo(sb, "<< DETALLE >>", "");
                foreach (BandaHorariaDetalle oBandaDetalle in oBandaHoraria.BandasHorarias)
                {
                    AuditoriaBs.AppendCampo(sb, "Día", oBandaDetalle.DiaSemana.Descripcion);
                    AuditoriaBs.AppendCampo(sb, "Tipo", oBandaDetalle.TipoDiaHora.DescripcionCorta);
                    AuditoriaBs.AppendCampo(sb, "Inicio", oBandaDetalle.HoraInicial);
                    AuditoriaBs.AppendCampo(sb, "Fin", oBandaDetalle.HoraFinal);
                }


                return sb.ToString();
            }

            #endregion


        #endregion


        #region INTERVALOS: Metodos de la Clase de Negocios de la entidad Intervalos.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los intervalos posibles entre las bandas horarias. 
        /// </summary>
        /// <returns>Lista de Intervalos</returns>
        /// ***********************************************************************************************
        public static BandaHorariaIntervaloL getIntervalosBandasHorarias()
        {
            BandaHorariaIntervaloL oIntervaloL = new BandaHorariaIntervaloL();

            oIntervaloL.Add(new BandaHorariaIntervalo(20, "20"));
            oIntervaloL.Add(new BandaHorariaIntervalo(30, "30"));
            oIntervaloL.Add(new BandaHorariaIntervalo(40, "40"));
            oIntervaloL.Add(new BandaHorariaIntervalo(60, "60"));

            return oIntervaloL;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    public class FondoDeCambioBs
    {
        #region FondoDeCambio: Metodos de la Clase FondoDeCambio.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Peajistas Habilitados para fondo de cambio
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada</param>
        /// <param name="turno">int? - Turno</param>
        /// <returns>Lista de fondo de cambio</returns>
        /// ***********************************************************************************************
        public static FondoDeCambioL getFondoDeCambio(int estacion, DateTime jornada, int? turno, string Estado)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return FondoDeCambioDt.getFondoDeCambio(conn, estacion, jornada, turno,Estado);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Trae el ultimo Fondo de Cambio hecho para un Operador
        /// </summary>
        /// <param name="p"></param>
        /// <param name="jornada"></param>
        /// <param name="turno"></param>
        /// <param name="Operador"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static FondoDeCambio getUltimoFondoDeCambio(int estacion, DateTime jornada, int turno, string Operador)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return FondoDeCambioDt.getUltimoFondoDeCambio(conn, estacion, jornada, turno,Operador);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un fondo de cambio
        /// </summary>
        /// <param name="oFondo">FondoDeCambio - Fondo De Cambio a eliminar </param>
        /// ***********************************************************************************************
        public static void delFondoDeCambio(FondoDeCambio oFondo)
        {             
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                FondoDeCambioDt.delFondoDeCambio(conn, oFondo);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oFondo),
                                                        getAuditoriaDescripcion(oFondo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agregar un fondo de Cambio
        /// </summary>
        /// <param name="oFondo">FondoDeCambio - Fondo de Cambio a agregar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addFondoDeCambio(FondoDeCambio oFondo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);


                // Si el fondo de cambio no tiene un parte o el mismo ya esta liquidado
                if (oFondo.Parte == null || oFondo.Parte.EstaLiquidado)
                { 
                    //GENERAR NUEVO PARTE PARA EL USUARIO
                    
                    Parte oParte = new Parte();
                    oParte.Apertura = DateTime.Now;
                    oParte.Estacion = EstacionBs.getEstacionActual();
                    oParte.Numero = -1; //El numero lo asigna el SP
                    oParte.Jornada = oFondo.Jornada;
                    oParte.Turno = oFondo.Turno;
                    oParte.ModoMantenimiento = false;
                    oParte.Peajista = oFondo.TesoreroEntrega;

                    // Genero el nuevo parte y lo asigno al objeto de fondo de cambio, para generarlo con este parte
                    if (PartesDt.addParte(conn, oParte, oParte.Peajista.ID))
                    {
                        oFondo.Parte = oParte;

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaParte(),
                                                               "A",
                                                               getAuditoriaCodigoRegistro(oParte),
                                                               getAuditoriaDescripcion(oParte)),
                                                               conn);
                    }
                }

                
                FondoDeCambioDt.addFondoDeCambio(conn, oFondo);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oFondo),
                                                        getAuditoriaDescripcion(oFondo)),
                                                        conn);
                    
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaParte()
        {
            return "PAR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Parte oParte)
        {
            return oParte.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Parte oParte)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Jornada", oParte.JornadaString);
            AuditoriaBs.AppendCampo(sb, "Turno", oParte.Turno.ToString());
            AuditoriaBs.AppendCampo(sb, "Peajista", oParte.Peajista.Nombre);

            return sb.ToString();
        }

        #endregion

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un fondo de Cambio
        /// </summary>
        /// <param name="oFondo">FondoDeCambio - Fondo de Cambio a modificar </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updFondoDeCambio(FondoDeCambio oFondo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en la plaza, con transaccion
                conn.ConectarPlaza(true);

                FondoDeCambioDt.updFondoDeCambio(oFondo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oFondo),
                                                        getAuditoriaDescripcion(oFondo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Realizo la confirmacion de un fondo de cambio desde una lista
        /// </summary>
        /// <param name="oFondos">Lista de Fondo de Cambios</param>
        /// ***********************************************************************************************
        public static FondoDeCambioL updVariosFondosDeCambio(FondoDeCambioL oFondos)
        {

            FondoDeCambioL oFondosMal = new FondoDeCambioL();

            if (oFondos.Count > 0)
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);
                    
                    //Por cada Fondo lo confirmams
                    foreach (FondoDeCambio oFondo in oFondos)
                    {
                        if (FondoDeCambioDt.updFondoDeCambio(oFondo, conn))
                        {
                        

                        //Grabamos auditoria
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                                "M",
                                                                getAuditoriaCodigoRegistro(oFondo),
                                                                getAuditoriaDescripcion(oFondo)),
                                                                conn);
                        }
                        else
                            oFondosMal.Add(oFondo);
                    }
                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }

            return oFondosMal;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de las devoluciones que han sido confirmadas
        /// </summary>
        /// <param name="entregas">FondoDeCambioL</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleDebolucionConfirmados(FondoDeCambioL oFondos)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return FondoDeCambioDt.getDetalleDevolucionesConfirmadas(conn, oFondos);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de las entregas de fondo de cambio
        /// </summary>
        /// <param name="entregas">FondoDeCambioL - Lista de entregas de fondo de cambio</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleEntregaFondoDeCambio(FondoDeCambioL entregas)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);                    
                return FondoDeCambioDt.getDetalleEntregaFondoDeCambio(conn, entregas);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el detalle de las entregas de fondo de cambio
        /// </summary>
        /// <param name="entregas">FondoDeCambioL - Lista de entregas de fondo de cambio</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleUltimaEntregaFondoDeCambio(FondoDeCambio entrega)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion                    
                conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                return FondoDeCambioDt.getDetalleUltimaEntregaFondoDeCambio(conn, entrega);
            }
        }

        #endregion

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "FOC";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(FondoDeCambio oFondo)
        {
            return oFondo.Parte.Numero.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(FondoDeCambio oFondo)
        {
            StringBuilder sb = new StringBuilder();
            string estado = "";

            if (oFondo.Estado == "E")
            {
                estado = "Entregado";
            }
            else if (oFondo.Estado == "D" && oFondo.Confirmado == "S")
            {
                estado = "Devuelto";
            }
            else if (oFondo.Estado == "D" && (oFondo.Confirmado == "N" || String.IsNullOrEmpty(oFondo.Confirmado)))
            {
                estado = "Devolución sin confirmar";
            }

            AuditoriaBs.AppendCampo(sb, "Estación", oFondo.Estacion.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Parte", oFondo.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha Asignación", oFondo.FechaAsignacion.ToString());

            if (oFondo.TesoreroEntrega != null)
            {
                AuditoriaBs.AppendCampo(sb, "Tesorero Entrega", oFondo.TesoreroEntrega.ID);
            }

            AuditoriaBs.AppendCampo(sb, "Estado", estado);
            AuditoriaBs.AppendCampo(sb, "Monto", oFondo.Monto.ToString());
            AuditoriaBs.AppendCampo(sb, "Fecha Devolución", oFondo.FechaDevolucion.ToString());

            if (oFondo.TesoreroDevolucion != null)
            {
                AuditoriaBs.AppendCampo(sb, "Tesorero Devolución", oFondo.TesoreroDevolucion.ID);
            }

            AuditoriaBs.AppendCampo(sb, "Devolución confirmada", Traduccion.getSiNo(oFondo.Confirmado == "S"));
            if(oFondo.ComentarioEliminar != null)
                AuditoriaBs.AppendCampo(sb, "Motivo Eliminación", oFondo.ComentarioEliminar);
            
            return sb.ToString();
        }

        #endregion



        public static EstadoFondoDeCambioL ObtenerEstadosDeFondoDeCambioPosibles()
        {
            var EstadoFondodeCambio = new EstadoFondoDeCambioL();

            var estado = new EstadoFondoDeCambio("EN");
            EstadoFondodeCambio.Add(estado);

            estado = new EstadoFondoDeCambio("DS");
            EstadoFondodeCambio.Add(estado);

            estado = new EstadoFondoDeCambio("DC");
            EstadoFondodeCambio.Add(estado);

            return EstadoFondodeCambio;
        }
    }

    public class ValorFondoDeCambioBs
    {
        #region ValorFondoDeCambio: Metodos de la Clase ValorFondoDeCambio.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Valores Habilitados para fondo de cambio junto con el objeto que indica que
        /// no se entregó el fondo de cambio;
        /// </summary>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="codigo">int - codigo</param>
        /// <returns>Lista de valores de fondo de cambio</returns>
        /// ***********************************************************************************************
        public static ValorFondoDeCambioL getValoresFondoDeCambio(int? estacion, int? codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                var valoresFondosDeCambios = ValorFondoDeCambioDt.getValoresFondoDeCambio(conn, estacion, codigo, false);

                //La siguiente linea representa el fondo de cambio "No Entregado"
                //valoresFondosDeCambios.Insert(0, new ValorFondoDeCambio());

                return valoresFondosDeCambios;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Valores Habilitados para fondo de cambio
        /// </summary>
        /// <param name="codigo">int - codigo</param>
        /// <returns>Lista de valores de fondo de cambio</returns>
        /// ***********************************************************************************************
        public static ValorFondoDeCambioL getValoresFondoDeCambio(int? codigo)
        {
            return getValoresFondoDeCambio(null, codigo);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Valores Habilitados para fondo de cambio
        /// </summary>
        /// <returns>Lista de valores de fondo de cambio</returns>
        /// ***********************************************************************************************
        public static ValorFondoDeCambioL getValoresFondoDeCambio()
        {
            return getValoresFondoDeCambio(null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un valor de fondo de cambio
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto ValorFondoDeCambio
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addValorFondoDeCambio(ValorFondoDeCambio oValorFC)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos valor de fondo de cambio
                ValorFondoDeCambioDt.addValorFondoDeCambio(oValorFC, conn); 
                    
                //Grabamos las habilitaciones por estacion del valor de fondo de cambio
                foreach (ValorFCEstacion est in oValorFC.EstacionesHabilitadas)
                {
                    if (est.EsHabilitado)
                    {
                        ValorFondoDeCambioDt.addEstacionPorValorFondoDeCambio(est.Estacion.Numero, oValorFC.Codigo, conn);
                    }
                }

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oValorFC),
                                                        getAuditoriaDescripcion(oValorFC)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un valor de fondo de cambio
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto ValorFondoDeCambio
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updValorFondoDeCambio(ValorFondoDeCambio oValorFC)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos valor de fondo de cambio
                ValorFondoDeCambioDt.updValorFondoDeCambio(oValorFC, conn);

                //Modificamos las habilitaciones por estacion del valor de fondo de cambio
                foreach (ValorFCEstacion est in oValorFC.EstacionesHabilitadas)
                {
                    if (est.EsHabilitado)
                    {
                        ValorFondoDeCambioDt.addEstacionPorValorFondoDeCambio(est.Estacion.Numero, oValorFC.Codigo, conn);
                    }
                    else
                    {
                        ValorFondoDeCambioDt.delEstacionPorValorFondoDeCambio(est.Estacion.Numero, oValorFC.Codigo, conn);
                    }
                }

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oValorFC),
                                                        getAuditoriaDescripcion(oValorFC)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Baja de un valor de fondo de cambio
        /// </summary>
        /// <param name="oValorFC">ValorFondoDeCambio - Objeto ValorFondoDeCambio
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delValorFondoDeCambio(ValorFondoDeCambio oValorFC)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Borramos las habilitaciones por estacion del valor de fondo de cambio                    
                ValorFondoDeCambioDt.delEstacionesPorValorFondoDeCambio(oValorFC.Codigo, conn);                    

                //Borramos valor de fondo de cambio
                ValorFondoDeCambioDt.delValorFondoDeCambio(oValorFC, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "B",
                                                        getAuditoriaCodigoRegistro(oValorFC),
                                                        getAuditoriaDescripcion(oValorFC)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene un dataset para el reporte
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptEstacionesPorValorFondoDeCambio()
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ValorFondoDeCambioDt.getRptEstacionesPorValorFondoDeCambio(conn);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de estaciones habilitadas de un valor para fondo de cambio
        /// </summary>
        /// <param name="codigo">int - codigo</param>
        /// <returns>Lista de estaciones para un valor de fondo de cambio</returns>
        /// ***********************************************************************************************
        public static ValorFCEstacionL getEstacionesPorValorFondoDeCambio(int codigo)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ValorFondoDeCambioDt.getEstacionesPorValorFondoDeCambio(conn, codigo);
            }
        }
        
        #endregion

        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "TFO";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ValorFondoDeCambio oValorFC)
        {
            return oValorFC.Codigo.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(ValorFondoDeCambio oValorFC)
        {
            StringBuilder sb = new StringBuilder();

            string estaciones = "";
            AuditoriaBs.AppendCampo(sb, "Codigo", oValorFC.Codigo.ToString());
            AuditoriaBs.AppendCampo(sb, "Valor", oValorFC.Valor.ToString());
            
            foreach (ValorFCEstacion item in oValorFC.EstacionesHabilitadas)
            {
                if (item.Habilitado == "S")
                {
                    estaciones = estaciones + item.NumeroEstacion + " ";
                }
            }

            AuditoriaBs.AppendCampo(sb, "Estaciones Habilitadas", estaciones.Trim());
            return sb.ToString();
        }

        #endregion
    }
}

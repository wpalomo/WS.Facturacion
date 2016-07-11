using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class AccionBS
    {

        #region ACCIONES: Metodos de la Accion.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las acciones pendientes de supervisión 
        /// </summary>
        /// <returns>Lista de Alarmas</returns>
        /// ***********************************************************************************************
        public static AccionL getAccionesPendientes(int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false, false);

                    return AccionDT.getAccionesPendientes(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las acciones historico de supervisión 
        /// </summary>
        /// <returns>Lista de Alarmas</returns>
        /// ***********************************************************************************************
        public static AccionL getAccionesHistorico(int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(false, false, false);

                    return AccionDT.getAccionesHistorico(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de accion a "En Curso"
        /// </summary>
        /// <param name="oAccion">Accion - Objeto de accion
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setAccionEnCurso(Accion oAccion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false, true);

                    oAccion.TerminalAutorizo = ConexionBs.getTerminal();

                    AccionDT.setAccionEnCurso(oAccion, conn);

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
        /// Modificacion de accion a "Autorizada"
        /// </summary>
        /// <param name="oAccion">Accion - Objeto de accion
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool setAccionAutorizada(Accion oAccion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false, true);

                    oAccion.TerminalAutorizo = ConexionBs.getTerminal();

                    AccionDT.setAccionAutorizada(oAccion, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Inserta la accion como "Finalizada"
        /// </summary>
        /// <param name="oAccion">Accion - Objeto de accion
        /// <returns></returns>
        /// ***********************************************************************************************
        public static int setAccionFinalizada(Accion oAccion)
        {
            int retval = -1;
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false, true);

                    oAccion.TerminalAutorizo = ConexionBs.getTerminal();

                    if (!oAccion.SinTag)
                        // Guarda el Transito
                        AccionDT.setAccionSiguioConTag(conn, oAccion);
                    else
                    {
                        //Agrega la observacion del supervisor con la patente 
                        ComentarioSupervisor oComent = new ComentarioSupervisor();
                        oComent.fecha = DateTime.Now;
                        oComent.estacion = oAccion.Estacion;
                        oComent.transito = oAccion.Numtran;
                        oComent.comentario = oAccion.Patente + " - " + "SIN TAG";
                        oComent.id = oAccion.UsuarioAutorizo;
                        oComent.via = oAccion.Via;
                        ComentarioSupervisorDt.addComentarioSupervisor(oComent, conn);
                    }
                    retval = AccionDT.setAccionFinalizada(oAccion, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retval;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la accion como "Pendiente"
        /// </summary>
        /// <param name="oAccion">Accion - Objeto de accion
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool setAccionPendiente(Accion oAccion)
        {
            
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false, true);
                    
                    AccionDT.setAccionPendiente(oAccion.ID, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Tesoreria;
using Telectronica.Validacion;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de los Cambios de personas a cargo de una estación, terminal, etc
    /// </summary>
    ///****************************************************************************************************
    public class CambiosBs
    {
        #region CAMBIOSUPERVISOR: Metodos relacionados con el supervisor a cargo.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve si el usuario logueado es el supervisor a Cargo de la Estacion
        /// </summary>
        /// <returns>true si coincide</returns>
        /// ***********************************************************************************************
        public static bool verificaSupervisorACargo()
        {
            bool bRet = false;
            CambioSupervisor cambio = getSupervisorACargo(ConexionBs.getNumeroEstacion());
            if (cambio != null)
                if (cambio.Supervisor.ID.ToLower() == ConexionBs.getUsuario().ToLower())
                    bRet = true;

            return bRet;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el supervisor a Cargo de la Estacion
        /// </summary>
        /// <returns>Objeto con el supervisor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioSupervisor getSupervisorACargo()
        {
            return getSupervisorACargo(ConexionBs.getNumeroEstacion());
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el supervisor a Cargo de la Estacion
        /// </summary>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <returns>Objeto con el supervisor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioSupervisor getSupervisorACargo(int estacion)
        {
            return getSupervisorACargo(ConexionBs.getGSToEstacion(), estacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el supervisor a Cargo de la Estacion
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <returns>Objeto con el supervisor a cargo</returns>
        /// ***********************************************************************************************
        public static CambioSupervisor getSupervisorACargo(bool bGST,
                                                     int estacion)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(bGST, false);

                    return CambiosDt.getSupervisorACargo(conn, estacion);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Asigna la estacion
        /// El usuario logueado es el supervisor a asignar
        /// Si no tiene parte le asigna parte
        /// Termina todas las asignaciones
        /// Graba la asignacion
        /// Agrega el comando de runners a las vias D
        /// </summary>
        /// <param name="oCambio">CambioSupervisor - Estructura de la asignacion</param>
        /// <param name="oUsuario">Usuario del cambio</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setSupervisorACargo(CambioSupervisor oCambio, Usuario oUsuario)
        {
            try
            {
                //Completamos datos en el objeto
                oCambio.Estacion = new Estacion(ConexionBs.getNumeroEstacion(),"");
                oCambio.FechaInicio = DateTime.Now;
                oCambio.Supervisor = new Usuario(oUsuario.ID,oUsuario.Nombre);
                oCambio.Parte.Peajista = oCambio.Supervisor;
                oCambio.Parte.Estacion = oCambio.Estacion;
                oCambio.Parte.ModoMantenimiento = false;
                oCambio.NuevoParte = true;

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en la plaza, con transaccion
                    conn.ConectarPlaza(true);

                    //Verficamos jornada abierta
                    if( JornadaBs.EstaCerrada(oCambio.Parte.Jornada) )
                        throw new ErrorJornadaCerrada(Traduccion.Traducir("la Jornada se encuentra cerrada"));

                    //Verificamos si tiene parte
                    ParteL oPartes = PartesDt.getPartes(conn, oCambio.Estacion.Numero,
                            oCambio.Parte.Jornada, oCambio.Parte.Jornada, oCambio.Supervisor.ID,
                            oCambio.Parte.Turno, oCambio.Parte.Turno,  "N");

                    foreach (Parte parte in oPartes)
                    {
                        //Verificamos que el parte no este liquidado
                        if (parte.Status == Parte.enmStatus.enmNoLiquidado)
                        {
                            oCambio.Parte = parte;
                            oCambio.NuevoParte = false;
                            break;
                        }
                    }
                    if( oCambio.NuevoParte )
                    //if( oCambio.Parte == null || oCambio.Parte.Numero == 0)
                    {
                      //  oCambio.NuevoParte = true;
                        //asignamos un parte
                        PartesDt.addParte(conn, oCambio.Parte, ConexionBs.getUsuario());
                    }

                    //Agregamos el cambio de supervisor
                    CambiosDt.setSupervisorACargo(oCambio, conn);

                    //Agregamos Comandos para las vias D
                    Comando oComando = new Comando(DateTime.Now, "3", "CONF", oCambio.Estacion.Numero,
                        0, ConexionBs.getUsuario(), null, null, "P", "R", "", "", ConexionBs.getUsuarioNombre(), Traduccion.Traducir("Cambios de Runner"), 0);

                    ComandoDt.addComandoVias(oComando, "D", conn);

                    //Marcamos la estacion como valida para validar
                    ConfiguracionValidacionDt.setEstacionValida(oCambio.Estacion, conn);
                    
                    //Grabamos auditoria: Siempre grabamos la asignacion de la estacion. Si es un nuevo parte generamos una auditoria adicional
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAsignacion(),
                                                           "A",
                                                           getAuditoriaCodigoRegistroAsignacion(oCambio),
                                                           getAuditoriaDescripcionAsignacion(oCambio)),
                                                           conn);

                    if (oCambio.NuevoParte)
                    {
                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNuevoParte(),
                                                               "A",
                                                               getAuditoriaCodigoRegistroNuevoParte(oCambio),
                                                               getAuditoriaDescripcionNuevoParte(oCambio)),
                                                               conn);
                    }


                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region AUDITORIA_ASIGNACIONESTACION: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc. (CUANDO SE ASIGNA LA ESTACION)

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaAsignacion()
        {
            return "TSU";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroAsignacion(CambioSupervisor oCambio)
        {
            return oCambio.Parte.Numero.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionAsignacion(CambioSupervisor oCambio)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Supervisor", oCambio.Supervisor.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", oCambio.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada", oCambio.Parte.Jornada.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Turno", oCambio.Parte.Turno.ToString());

            if (oCambio.NuevoParte)
                AuditoriaBs.AppendCampo(sb, "Se generó un nuevo Parte", "");
            else
                AuditoriaBs.AppendCampo(sb, "Se usó Parte existente", "");

            return sb.ToString();
        }

        #endregion


        #region AUDITORIA_NUEVOPARTE: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc. (CUANDO ES UN NUEVO PARTE)

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaNuevoParte()
        {
            return "PAR";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistroNuevoParte(CambioSupervisor oCambio)
        {
            return oCambio.Parte.Numero.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcionNuevoParte(CambioSupervisor oCambio)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Supervisor", oCambio.Supervisor.Nombre);
            AuditoriaBs.AppendCampo(sb, "Parte", oCambio.Parte.Numero.ToString());
            AuditoriaBs.AppendCampo(sb, "Jornada", oCambio.Parte.Jornada.ToShortDateString());
            AuditoriaBs.AppendCampo(sb, "Turno", oCambio.Parte.Turno.ToString());


            return sb.ToString();
        }

        #endregion


        #endregion
    }
}

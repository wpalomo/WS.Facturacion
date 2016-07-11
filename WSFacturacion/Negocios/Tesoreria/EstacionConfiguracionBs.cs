using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje.Tesoreria
{
    public class EstacionConfiguracionBs
    {
        #region DISPLAY: Metodos de la Clase de Negocios de  VELOCIDAD.

        public static DisplayL getMensajesDisplay()
        {
            return getMensajesDisplay(ConexionBs.getGSToEstacion(), null, null);

        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas mensajes Display definidos. 
        /// </summary>
        /// <param name="Codigo">String - Permite filtrar por un mensaje Display
        /// <param name="Sentido">String - Permite filtrar por un mensaje Display
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        public static DisplayL getMensajesDisplay(string Codigo, string Sentido)
        {
            return getMensajesDisplay(ConexionBs.getGSToEstacion(), Codigo, Sentido);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes del Display
        /// </summary>
        /// <param name="bGST">bool - Permite conectarse a GST o la Estación
        /// <param name="Codigo">String - Permite filtrar por un mensaje Display
        /// <param name="Sentido">String - Permite filtrar por un mensaje Display
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        public static DisplayL getMensajesDisplay(bool bGST, string Codigo, string Sentido)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarPlaza(false);

                    return EstacionConfiguracionDT.getMensajesDisplay(conn,Codigo,Sentido);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de Mensajes del Display
        /// </summary>
        /// <param name="oDisplay">Display - Estructura de Mensajes del Display
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMensajeDisplay(Display oDisplay)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre con transaccion
                    conn.ConectarPlaza(true);

                    //Modificamos Config
                    EstacionConfiguracionDT.updMensajeDisplay(oDisplay, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMensajeDisplay(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oDisplay),
                                                           getAuditoriaDescripcion(oDisplay)),
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
        private static string getAuditoriaCodigoAuditoriaMensajeDisplay()
        {
            return "GIS";
        }


        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Display oDisplay)
        {
             return oDisplay.Codigo.ToString();
        }


        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Display oDisplay)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Activo", oDisplay.Activo.ToString());
            AuditoriaBs.AppendCampo(sb, "Titilante", oDisplay.Titilante);
            AuditoriaBs.AppendCampo(sb, "Letra Ancha", oDisplay.LetraAncha.ToString());
            AuditoriaBs.AppendCampo(sb, "Velocidad", oDisplay.Velocidad.ToString());
            AuditoriaBs.AppendCampo(sb, "Efectos", oDisplay.Efectos.ToString());
            AuditoriaBs.AppendCampo(sb, "Mensaje", oDisplay.Texto.ToString());
            AuditoriaBs.AppendCampo(sb, "Tipo de Mensaje", oDisplay.Codigo.ToString());
            
            return sb.ToString();
        }

        #endregion

       #endregion

        #region VELOCIDAD: Metodos de la Clase de Negocios de la entidad Display.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los tipos de Velocidad. 
        /// </summary>
        /// <returns>Lista de tipos de Velocidad</returns>
        /// ***********************************************************************************************
        public static VelocidadL getVelocidad()
        {
            VelocidadL oVelocidad = new VelocidadL();
            for (int i = 1; i <= 3; i++)
            {
                  oVelocidad.Add(getVelocidad(i));
            }

            return oVelocidad;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Objeto de la Velocidad.
        /// </summary>
        /// <param name="codigo">int - Codigo de la Velocidad que deseamos obtener</param>
        /// <returns>Objeto Velocidad</returns>
        /// ***********************************************************************************************
        public static Velocidad getVelocidad(int codigo)
        {
            return EstacionConfiguracionDT.getVelocidad(codigo);
        }

        #endregion

    }

}
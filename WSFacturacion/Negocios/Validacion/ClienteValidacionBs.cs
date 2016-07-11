using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Validacion
{
    public class ClienteValidacionBs
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Configuracion completa de validacion
        /// </summary>
        /// <returns>Configuracion de validacion</returns>
        /// ***********************************************************************************************
        public static ConfigValidacion getConfiguracionClienteValidacion()
        {
            ConfigValidacion oConfig = new ConfigValidacion();

            try
            {

                // Asignamos el objeto estacion de la estacion actual
                oConfig.EstacionActual = EstacionBs.getEstacionActual();

                // Asignamos el objeto usuario del usuario logueado
                oConfig.UsuarioActual = UsuarioBs.getUsuarioLogueado();

                //Traemmos los permisos
                oConfig.Permisos = PermisosBs.GetPermisosCliente("VAL", null, false);

                oConfig.esValidacionCentralizada = ConfigValidacion.ValidacionCentralizada;

                oConfig.esCierreJornadaPorEstacion = ConfigValidacion.CierreJornadaPorEstacion;

                oConfig.esValidacionLocal = ConfigValidacion.getEsValidacionLocal(ConexionBs.getGSToEstacion());

                return oConfig;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}

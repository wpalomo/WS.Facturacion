using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region PERMISO: Clase para entidad de Permisos en las paginas.
    /// <summary>
    /// Estructura de una entidad Permiso
    /// </summary>

    [Serializable]
    
    public class Permiso
    {
        public Permiso()
        {
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de una zona en particular
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public Permiso(string perfil, string modulo, bool gst, bool cli,
                    string pagina, string control, bool habilitado, bool autorizacion, bool nuevaVentana)
        {
            this.Perfil = perfil;
            this.EsGestion = gst;
            this.EsCliente = cli;
            this.Modulo = modulo;
            this.Pagina = pagina;
            this.Control = control;
            this.Habilitado = habilitado;
            this.Autorizacion = autorizacion;
            this.EnNuevaVentana = nuevaVentana;
        }

        //Es de Gestion?
        public bool EsGestion { get; set; }

        //Es Cliente?
        public bool EsCliente { get; set; }

        //Modulo
        public string Modulo { get; set; }

        //Perfil
        public string Perfil { get; set; }

        // Nombre Pagina
        public string Pagina { get; set; }

        // Nombre del Control
        public string Control { get; set; }

        // Descripcion del Control
        public string Descripcion { get; set; }

        // Habilitado?
        public bool Habilitado { get; set; }

        // Autorizacion?
        public bool Autorizacion { get; set; }

        // Abrir en Nueva Ventana?
        public bool EnNuevaVentana { get; set; }

        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Control;
        }
    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Permiso.
    /// </summary>
    public class PermisoL : List<Permiso>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un permiso de la pagina solicitada
        /// </summary>
        /// <param name="pagina">string - Codigo de pagina a buscar
        /// <returns>objeto Permiso que corresponda a la pagina buscada</returns>
        /// ***********************************************************************************************
        public Permiso FindPagina(string pagina)
        {
            Permiso oPermiso = null;
            foreach (Permiso oPer in this)
            {
                if (oPer.Pagina == pagina)
                {
                    oPermiso = oPer;
                    break;
                }
            }
            return oPermiso;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve todos los permisos de la pagina solicitada
        /// </summary>
        /// <param name="pagina">string - Codigo de pagina a buscar
        /// <returns>objeto PermisoL con todos los permisos de la pagina buscada</returns>
        /// ***********************************************************************************************
        public PermisoL FindPaginaTodos(bool bGst, bool bCli, string modulo, string pagina)
        {
            PermisoL oPermisos = new PermisoL();
            foreach (Permiso oPer in this)
            {
                if (oPer.Pagina == pagina && oPer.Modulo == modulo
                    && oPer.EsGestion == bGst && oPer.EsCliente == bCli)
                {
                    oPermisos.Add(oPer);
                }
            }
            return oPermisos;
        }
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region PERFIL: 
    /// <summary>
    /// Estructura de una entidad Perfil de acceso al sistema
    /// </summary>

    [Serializable]

    public class Perfil
    {
        public Perfil()
        {
        }
        public Perfil(string codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }

        public string Codigo { get; set; }
        public string Descripcion { get; set; }
        public PerfilNivel NivelVia { get; set; }
        //Indica si el perfil se puede dar de baja o modificar
        public string OpcionesModificacion { get; set; }

        public bool PuedeEliminar
        {
            get
            {
                return OpcionesModificacion == "S";
            }
        }

        public bool PuedeModificar
        {
            get
            {
                return OpcionesModificacion != "N";
            }
        }

        // Es el perfil administrador?
        public bool EsAdministrador()
        {
            return (Codigo == "Administrador");
        }

        //Permisos de los formularios
        public PermisoL Permisos { get; set; }
        //Permisos de los eventos
        public EventoPerfilL PermisosEventos { get; set; } 
        //Jerarquia entre perfiles
        //Son los perfiles de los usuarios que podemos modificar o dar de alta
        public PerfilJerarquiaL Jerarquia { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }


    }


    [Serializable]

    public class PerfilL : List<Perfil>
    {
    }
    #endregion
}

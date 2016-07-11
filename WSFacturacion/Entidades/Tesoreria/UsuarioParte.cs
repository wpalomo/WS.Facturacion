using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region USUARIOPARTE: Clase para entidad de UsuarioParte.
    /// <summary>
    /// Combinacion de Usuarios y Partes
    /// </summary>
    [Serializable]
    public class UsuarioParte
    {

        public Usuario Usuario { get; set; }
        public Parte Parte { get; set; }
        public bool PuedeAsignarParte { get; set; }
        //public bool TieneParte { get; set; }

        public string ID
        {
            get
            {
                return Usuario.ID;
            }
        }
    }
    /// <summary>
    /// Lista de objetos UsuarioParte.
    /// </summary>
    /// 
    [Serializable]
    public class UsuarioParteL : List<UsuarioParte>
    {
    }
    #endregion
}

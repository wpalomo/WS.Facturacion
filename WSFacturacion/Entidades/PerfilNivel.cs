using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region PERFILNIVEL: 
    /// <summary>
    /// Estructura de una entidad PerfilNivel Niveles del perfil para la via
    /// </summary>

    [Serializable]
    
    public class PerfilNivel
    {
        public PerfilNivel()
        {
        }
        public PerfilNivel(byte codigo, string descripcion)
        {
            Codigo = codigo;
            Descripcion = descripcion;
        }
        public byte Codigo { get; set; }
        public string Descripcion { get; set; }
        //Para mostrar en las grillas, etc
        public override string ToString()
        {
            return Descripcion;
        }
    }


    [Serializable]

    public class PerfilNivelL:List<PerfilNivel>
    {
    }
    #endregion

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region PERFILJERARQUIA: 
    /// <summary>
    /// Estructura de una entidad PerfilJerarquia son los perfiles de los usuarios 
    /// que un perfil puede modificar
    /// </summary>

    [Serializable]
    
    public class PerfilJerarquia
    {
        public string Perfil { get; set; }
        public Perfil PerfilMenor { get; set; }
        public bool Controlado { get; set; }

        public string CodigoMenor
        {
            get
            {
                return PerfilMenor.Codigo;
            }
        }

        public string DescripcionMenor
        {
            get
            {
                return PerfilMenor.Descripcion;
            }
        }

    }


    [Serializable]

    public class PerfilJerarquiaL:List<PerfilJerarquia>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Busca en la jerarquia un determinado perfil
        /// </summary>
        /// <param name="perfil">string - perfil a buscar
        /// <returns>objeto PerfilJerarquia del perfil buscado</returns>
        /// ***********************************************************************************************
        public PerfilJerarquia FindJerarquia(string perfil)
        {
            PerfilJerarquia oJerarquia=null;
            foreach (PerfilJerarquia item in this)
            {
                if (item.PerfilMenor.Codigo == perfil)
                {
                    oJerarquia = item;
                    break;
                }
            }
            return oJerarquia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Convierte la lista de Jerarquias en una lista de Perfiles Controlados
        /// </summary>
        /// <param name="oJerarquias">PerfilJerarquiaL - Lista a convertir
        /// <returns>Lista de Perfiles controlados</returns>
        /// ***********************************************************************************************
        public static explicit operator PerfilL(PerfilJerarquiaL oJerarquias)
        {
            PerfilL oPerfiles = new PerfilL();
            foreach (PerfilJerarquia item in oJerarquias)
            {
                if (item.Controlado)
                {
                    oPerfiles.Add(item.PerfilMenor);
                }
            }

            return oPerfiles;
        }

    }
    
    #endregion
}

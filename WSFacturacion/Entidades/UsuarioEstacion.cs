using System;
using System.Collections.Generic;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]

    public class UsuarioEstacion
    {
        public Usuario Usuario { get; set; }
        public Estacion Estacion { get; set; }

        public Perfil Perfil { get; set; }

        public bool Local { get; set; }

        public int Codigo
        {
            get
            {
                return Estacion.Numero;
            }
        }
    }


    [Serializable]

    public class UsuarioEstacionL : List<UsuarioEstacion>
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la habilitacion en una estacion
        /// </summary>
        /// <param name="estacion">int - Estacion a buscar
        /// <returns>objeto UsuarioEstacion que corresponda a la estacion buscada</returns>
        /// ***********************************************************************************************
        public UsuarioEstacion FindEstacion(int estacion)
        {
            UsuarioEstacion oEstacion = null;
            foreach (UsuarioEstacion oEst in this)
            {
                if (oEst.Estacion.Numero == estacion)
                {
                    oEstacion = oEst;
                    break;
                }
            }
            return oEstacion;
        }

    }
}

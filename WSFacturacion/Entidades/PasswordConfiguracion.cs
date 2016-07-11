using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region CONFIGURACION DE CONTRASEÑA: Clase para entidad Configuración de Contraseñas.
    /// <summary>
    /// Estructura de una entidad Configuración de Contraseñas
    /// </summary>

    [Serializable]

    public class PasswordConfiguracion
     
    {

    public PasswordConfiguracion()
        {

        }

        // Id de la Configuración de Contraseñas
        public int IdContraseña { get; set; }

        // Vencimiento de la Configuración de Contraseñas
        public Int16 Vencimiento { get; set; }

        // ControlRepeticion de la Configuración de Contraseñas
        public String ControlRepeticion { get; set; }
        public bool ControlarRepeticion
        {
            get
            {
                return ControlRepeticion == "S";
            }
        }

        // LargoMinimo de la Configuración de Contraseñas
        public Int16 LargoMinimo { get; set; }

        // CaracteresRepetidos de la Configuración de Contraseñas
        public Int16 CaracteresRepetidos { get; set; }

    }


    [Serializable]

    /// <summary>
    /// Lista de objetos Configuración de Contraseñas.
    /// </summary>
    /// 
    public class PasswordConfiguracionL : List<PasswordConfiguracion>
    {
    }

    #endregion
}

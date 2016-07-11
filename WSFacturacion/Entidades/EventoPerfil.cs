using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region EVENTOPERFIL:
    /// <summary>
    /// Estructura de una entidad EventoPerfil son los eventos que un perfil puede ver
    /// </summary>

    [Serializable]

    public class EventoPerfil
    {
        public string Perfil { get; set; }
        public ClaveEvento Evento { get; set; }
        public bool Habilitado { get; set; }
    }


    [Serializable]

    public class EventoPerfilL:List<EventoPerfil>
    {
    }
    #endregion
}

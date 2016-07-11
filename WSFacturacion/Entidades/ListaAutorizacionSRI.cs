using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Telectronica.Peaje
{
    class ListaAutorizacionSRI
    {
        public string m_numaut { get; set; }
        public string m_doc { get; set; }
        public DateTime m_fechainicio { get; set; }
        public DateTime m_fechafin { get; set; }
        public DateTime m_fechaingreso { get; set; }

        public ListaAutorizacionSRI() { }
    }

    /// <summary>
    /// Lista de objetos ListaContable.
    /// </summary>
    [Serializable]
    public class ListaAutorizacionSRIL : List<ListaAutorizacionSRI>
    {
    }


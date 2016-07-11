using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    [Serializable]

    public class ReferenciaFK
    {
        public string Tabla { get; set; }
        public string[] Campos { get; set; }
        public int CantidadRegistros { get; set; }
        public bool Excluyente { get; set; }
        public int CantidadCampos
        {
            get
            {
                return Campos.Length;
            }
        }
    }


    [Serializable]

    public class ReferenciaFKL : List<ReferenciaFK>
    {
    }
}

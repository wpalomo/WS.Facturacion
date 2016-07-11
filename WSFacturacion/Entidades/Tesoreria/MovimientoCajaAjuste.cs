using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Tesoreria
{
	[Serializable]
	public class MovimientoCajaAjuste
	{
		public int identity { get; set; }
		public int parteOrigen { get; set; }
		public int parteDestino { get; set; }
		public string validador { get; set; }
		public DateTime fecha { get; set; }
		public decimal monto { get; set; }
		public string comentario { get; set; }
		public int mocOrigen { get; set; }
		public int mocDestino { get; set; }
		public char estadoJornadaOrigen { get; set; }
		public char estadoJornadaDestino { get; set; }
		public char reposicionOrigen { get; set; }
		public char reposicionDestino { get; set; }
	}

	[Serializable]
	public class MovimientoCajaAjusteL : List<MovimientoCajaAjuste>
	{
		
	}
}

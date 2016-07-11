using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    #region PARTEREPOSICION: Clase para entidad de ParteReposicion.
    /// <summary>
    /// Combinacion de Usuarios y Partes
    /// </summary>
    [Serializable]
    public class ParteReposicion
    {
        public Parte Parte { get; set; }
        public MovimientoCajaReposicion Reposicion { get; set; }

        public bool JornadaCerrada
        {
            get
            {
                return Parte.JornadaCerrada;
            }
        }
        public int ParteNumero
        {
            get
            {
                return Parte.Numero;
            }
        }

        public int ParteTurno
        {
            get
            {
                return Parte.Turno;
            }
        }

        public DateTime Jornada
        {
            get
            {
                return Parte.Jornada;
            }
        }

        public string JornadaString
        {
            get
            {
                return Parte.JornadaString;
            }
        }

        public Parte.enmStatus Status
        {
            get
            {
                return Parte.Status;
            }
        }

        public string StatusDesc
        {
            get
            {
                return Parte.StatusDesc;
            }
        }

        public string Peajista
        {
            get
            {
                return Parte.Peajista.ToString();
            }
        }

        public int Turno
        {
            get
            {
                return Parte.Turno;
            }
        }
        public string PeajistaID
        {
            get
            {
                return Parte.PeajistaID;
            }
        }

        //El objeto Reposicion puede ser null
        public int NumeroMovimiento
        {
            get
            {
                int numero=0;
                if( Reposicion != null)
                    numero = Reposicion.NumeroMovimiento;
                return numero;
            }
        }

        public string HoraIngreso
        {
            get
            {
                string hora = null;
                if (Reposicion != null)
                    hora = Reposicion.HoraIngreso;
                return hora;
            }
        }

        public Decimal MontoMoneda
        {
            get
            {
                Decimal monto = 0;
                if( Reposicion != null )
                    monto = Reposicion.MontoMoneda;
                return monto;
            }
        }

        public string sMontoMoneda
        {
            get
            {
                string monto = null;
                if( Reposicion != null )
                    monto = Reposicion.sMontoMoneda;
                return monto;
            }
        }

    }
    /// <summary>
    /// Lista de objetos UsuarioParte.
    /// </summary>
    /// 
    [Serializable]
    public class ParteReposicionL : List<ParteReposicion>
    {
    }
    #endregion
}


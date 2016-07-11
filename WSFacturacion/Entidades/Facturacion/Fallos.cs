using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{

    [Serializable]
    public class Fallos
    {

        // Constructor vacio
        public Fallos() { }

        // Constructor basico
        public Fallos(Int16 Estacion, int Parte)
        {
            this.Estacion = Estacion;
            this.Parte = Parte;
        }

        //fpg_coest tinyint not null	/* Estacion */
        public Int16 Estacion { get; set; }
        public Estacion oEstacion { get; set; }

        //fpg_parte int not null		/* Parte */
        public int Parte { get; set; }

        //fpg_ident int identity		/* Identity */
        public int identity { get; set; }

        //fpg_morep money not null		/* Monto a Reponer */
        public decimal MontoAReponer { get; set; }

        //fpg_mofac money not null		/* Monto a Facturar */
        public decimal MontoAFacturar { get; set; }

        //fpg_supid varchar(10) not null    /* Supervisor */
        public string Supervisor { get; set; }
        public Usuario oSupervisor { get; set; }

        //fpg_fecha datetime not null	/* Fecha de Pedido */
        public DateTime FechaPedido { get; set; }

        //fpg_anula char(1) not null	/* Anulado (S/N) */
        public char Anulado { get; set; }

        //fpg_numer int null			/* Clave del registro de Mocaja correspondiente */
        public int ClaveRegistroMocaja { get; set; }

        //fpg_pendi char(1) not null	/* Pendiente de Facturacion */
        public char PendienteFacturacion { get; set; }

        //fpg_itmid int null	        /* id de itmfac */
        public int IdDeItmFC { get; set; }

        //fpg_factu int null			/* Numero de Factura */
        public int NumeroFactura { get; set; }

        //fpg_moanu money not null		/* Monto a Anular por NC */
        public decimal MontoAAnularPorNC { get; set; }

        //fpg_pennc char(1) not null	/* Pendiente de NC */
        public char PendienteNC { get; set; }

        //fpg_itmnc int null	        /* id de item NC */
        public int IdDeItmNC { get; set; }

        //fpg_ncnum int null			/* Numero de NC */
        public int NumeroNC { get; set; }

        //fpd_valid varchar(10) not null    /* Validador Solicitante */
        public string ValidadorSolicitante { get; set; }

        // Descripcion del fallo
        public string DescripcionVenta { get; set; }

        //// Parte del fallo
        //public Tesoreria.Parte oParte { get; set; }

        public Operacion.enmTipo TipoItem { get; set; }

        public static implicit operator Operacion(Fallos f)
        {
            Operacion operacion = new Operacion();
            operacion.TipoOperacion = f.TipoItem;
            operacion.DescripcionVenta = f.DescripcionVenta;
            operacion.Anulada = f.Anulado.ToString();
            operacion.Cliente = null ;
            operacion.Cuenta = null;
            operacion.Estacion = f.oEstacion;
            operacion.FechaOperacion = f.FechaPedido;
            operacion.Identity = f.identity;
            operacion.ItemFactura = null; // f.IdDeItmFC;
            operacion.Monto = f.MontoAFacturar;
            operacion.Parte = null; // f.oParte;
            operacion.Patente = null;
            operacion.EntregaTag = null;
            operacion.Fallo = f;
            return operacion;
        }

    }

    [Serializable]
    public class FallosL : List<Fallos>
    {
        // Para convertir esta clase a una lista de OperacionL
        public static implicit operator OperacionL(FallosL f)
        {
            OperacionL operaciones = new OperacionL();
            foreach (Fallos item in f)
            {
                operaciones.Add(item);
            }

            return operaciones;
        }
    }
}

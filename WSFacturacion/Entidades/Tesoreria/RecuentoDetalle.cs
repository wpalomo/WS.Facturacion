using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    [Serializable]
    public class RecuentoDetalle
    {

        // Constructor vacio
        public RecuentoDetalle() { }

        //Estacion
        public Estacion estacion { get; set; }

        //CÓDIGO DE ESTACIÓN tinyint
        public Int16 CodigoEstacion { get; set; }

        //NUMERO DE REGISTRO DE RECUENTO(int)
        public Int32 NumeroApropiacion { get; set; }

        //dec_iddep(int)
        public Int32 NumeroDeposito { get; set; }

        //Remito del Recuento
        public int F22Recuento { get; set; }

        //Sobre del Recuento
        public int SobreRecuento { get; set; }

        //NUMERO DE DETDEPMOC(int)
        public int? NumeroDepositoMocaja { get; set; }

        //VALOR DECLARADO COMO DEPOSITADO  (money)
        public decimal Ingresado { get; set; }

        //VALOR RECONTADO POR EL BANCO (ES EL QUE VALE) (money)
        public decimal Recontado { get; set; }

        //VALOR DE DIFERENCIA INFORMADO POR EL BANCO, NO TOMARLO COMO DEFINITIVO, RECALCULARLO SIEMPRE (money)
        public decimal Diferencia { get; set; }

        //OBSERVACION REGISTRADA POR LA ENTIDAD RECAUDADORA EN EL ARCHIVO QUE RETORNA varchar (1000)
        public string Observacion { get; set; }

        public BolsaDeposito BolsaDepositada { get; set; }

        // DATOS DEL ARCHIVO:


        public string CodigoRecuento { get; set; }
        public string Letra { get; set; }
        public string Sucursal { get; set; }
        public DateTime Fecha { get; set; }
        public string Moneda { get; set; }
        public string MedioDePago { get; set; }
        public Parte ParteRecuento {get; set;}

    }

    [Serializable]
    public class RecuentoDetalleL : List<RecuentoDetalle>
    {
    }

}


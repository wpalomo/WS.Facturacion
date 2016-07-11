using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    public class VISA
    {
        
        // Tipo de Registro
        public string Tipo { get; set; }

        #region HEADER

        // Nombre del archivo
        public string NombreArchivo { get; set; }

        // Fecha de Procesamiento
        public DateTime FechaProcesamiento { get; set; }

        // Periodo Inicial
        public DateTime PeriodoInicial { get; set; }

        // Periodo Final
        public DateTime PeriodoFinal { get; set; }

        // Secuencia que viene en el archivo
        public string Secuencia { get; set; }

        // Cantidad de transacciones
        public int Transacciones {get; set; }

        // Usuario
        public string Usuario { get; set; }

        #endregion

        #region RO

        // Numero del RO
        public string NumeroRO { get; set; }

        //Establecimiento VISA (Lo usamos para buscar el numero de estacion)
        public string EstabVisa { get; set; }

        // Fecha del deposito de la venta (Lo usamos para la JORNADA)    
        public DateTime FechaDeposito { get; set; }

        // Fecha Prevista de Pago
        public DateTime FechaPago { get; set; }

        // Fecha en la que se envio al banco
        public DateTime? FechaEnvioBanco { get; set; }

        // Valor Bruto
        public decimal ValorBruto { get; set; }

        // Valor de Comision
        public decimal ValorComision { get; set; }

        // Valor Rechazado
        public decimal ValorRechazado { get; set; }

        // Valor Neto
        public decimal ValorNeto {get; set;}

        // Cantidad de CV aceptadas
        public int CantCVAcetpadas { get; set; }

        // Cantidad de CV rechazadas 
        public int CantCVRechazadas { get; set; }

        // Status de venta (credito o debito)
        public int StatusVenta { get; set; }

        #endregion

        #region CV

        // Fecha de compra
        public DateTime? FechaCompra { get; set; }

        //Numero de Tarjeta (Cartão)
        public string NumeroCartao { get; set; }

        //Estado de Operacion
        public string EstadoOperacion { get; set; }

        //Monto Operacion
        public decimal Monto { get; set; } 

        //Motivo rechazo
        public string MotivoRechazo {get; set;}

        
        #endregion

        #region VisaTxt

        //Fecha de Transaccion del pinpad
        public DateTime FechaVisaPinPad { get; set; }

        //Numero de Transaccion Visa
        public int NumeroTran { get; set; }

        //Numero de pinpad
        public int NumeroPinPad { get; set; }

        //Fecha de compra de Visa
        public DateTime FechaCompraVisa { get; set; }

        //Numero Concentrador
        public int NumeroConcentrador { get; set; }

        //Numero de Registro
        public int NumeroRegistro { get; set; }
        #endregion
    }
}

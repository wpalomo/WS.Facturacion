using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Tesoreria
{
    [Serializable]
    public class RecuentosDepositos
    {
        // Constructor vacio
        public RecuentosDepositos()
        {
            oRecuento = new RecuentoDetalle();
            oBolsaDeposito = new BolsaDeposito();
        }

        // Recuento
        public RecuentoDetalle oRecuento { get; set; }

        // Bolsas Deposito
        public BolsaDeposito oBolsaDeposito { get; set; }

        // Codigo de Status
        public int Status { get; set; }

        //Causa
        public string Causa { get; set; }


    }

    [Serializable]
    public class RecuentosDepositosL : List<RecuentosDepositos>
    {
    }

}
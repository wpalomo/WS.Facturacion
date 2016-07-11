using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{
    #region TICKET: Clase para entidad Tickets

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TipoIVA
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class Ticket
    {
        public Ticket()
        {

        }

        //DATOS DE UN TICKET

        // Fecha del ticket
        public DateTime FechaTicket { get; set; }

        // Estacion
        public Estacion Estacion { get; set; }

        // Via
        public Via Via { get; set; }

        // Turno
        public int NumeroTurno { get; set; }
       
        // Monto
        public decimal Monto { get; set; }

        // Categoria
        public CategoriaManual Categoria { get; set; }

        // Ticket Anulado
        public bool Anulado { get; set; }

        // Ticket Reemplazado
        public bool Reemplazado { get; set; }

        // Tiene Ruc?
        public bool TieneRUC { get; set; }

        // Expirado
        public bool Expirado { get; set; }

        // El Ticket es de Categoria Diferente al ingresado
        public bool CategoriaDiferente { get; set; }

        // Descripcion
        public string Descripcion { get; set; }

        //Razon Social
        public string RazonSocial { get; set; }

        //Punto de Venta
        public string PuntoVenta { get; set; }

        // Numero de Ticket
        public string NumeroTicket { get; set; }

       
    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Tickets
    /// </summary>*********************************************************************************************
    public class TicketL : List<Ticket>
    {
        public void AddOperaciones(OperacionL operaciones)
        {
        }

    }
    #endregion
}

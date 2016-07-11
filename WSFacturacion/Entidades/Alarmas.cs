using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region ALARMAS RECIBIDAS DE LA VIA: Clase para entidad de AlarmasConfirmar.
    /// <summary>
    /// Estructura de una entidad AlarmasConfirmar
    /// </summary>

    [Serializable]

    public class Alarmas
    {

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un alarma en particular
        /// </summary>
        /// ***********************************************************************************************

        public Alarmas()
        {
        }

        public Alarmas(int ID, int estacion, int via, string tipoAlarma, char confirmado, DateTime fechaGenera, DateTime? fechaConfir, DateTime? fechaFin, string usuario, string descripcion, string criticidad)
        {
            this.ID = ID;
            this.Estacion = estacion;
            this.Via = via;
            this.tipoAlarma = tipoAlarma;
            this.confirmado = confirmado;
            this.fechaGenera = fechaGenera;
            this.fechaConfir = fechaConfir;
            this.fechaFin = fechaFin;
            this.usuario = usuario;
            this.descripcion = descripcion;
            this.Criticidad = criticidad;
            CargarEstadoCriticidad();
        }

        // Identity
        public int ID { get; set; }

        // Estacion
        public int Estacion { get; set; }

        // Via
        public int Via { get; set; }

        // Tipo de alarma
        public string tipoAlarma { get; set; }

        //Confirmado
        public char confirmado { get; set; }

        //Pendiente
        public bool pendiente { get; set; }

        // Fecha Generación
        public DateTime fechaGenera { get; set; }

        // Fecha Confirmación
        public DateTime? fechaConfir { get; set; }

        // Fecha finalización
        public DateTime? fechaFin { get; set; }

        // Usuario
        public string usuario { get; set; }

        // Descripción
        public string descripcion { get; set; }

        public string Criticidad { get; set; }

        // Criticiadad, solo para grilla, | critica o no?
        public string CriticidadDesc { get; set; }

        // Estado, solo para datagrid / pendiente, confimada, en alarma,
        public string Estado { get; set; }


        //Función para setear las propiedades estado y criticidad desc (Para datagrid)

        public void CargarEstadoCriticidad()
        {
            //Criticidad
            if (Criticidad.Length > 0)
                CriticidadDesc = "Critica";
            else
                CriticidadDesc = "";
            //Estado
            string C = Convert.ToString(confirmado);
            int Valor = (string.Compare("N", C));
            if (fechaFin != null)
            {
                Estado = "Terminada";
                pendiente = false;
            }
            else if (Valor == 0)
            {
                Estado = "Pendiente";
                pendiente = true;
            }
            else
            {
                Estado = "En alarma";
                pendiente = false;
            }
        }

    }

    [Serializable]

    /// <summary>
    /// Lista de objetos MensajePredefinido.
    /// </summary>
    public class AlarmasL : List<Alarmas>
    {
    }

    #endregion
}
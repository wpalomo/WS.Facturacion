using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{
    #region TURNOTRABAJO: Clase para entidad de Turnos de Trabajo en la estacion de Peaje

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad TurnoTrabajo
    /// </summary>*********************************************************************************************    

    [Serializable]

    public class TurnoTrabajo
    {
        //Devuelve cuantos minutos antes del comienzo de un nuevo turno
        //tomamos ese turno por defecto para los peajistas
        public static int getToleranciaAsignacion()
        {
            return 180;
        }
        //Devuelve cuantos minutos antes del comienzo de un nuevo turno
        //tomamos ese turno por defecto para la apropiacion 
        public static int getToleranciaApropiacion()
        {
            return -60;
        }
        //Devuelve cuantos minutos antes del comienzo de un nuevo turno
        //tomamos ese turno por defecto para el supervisor
        public static int getToleranciaAsignacionPlaza()
        {
            return 60;
        }
        //Devuelve cuantos minutos despues del comienzo de un nuevo turno
        //tomamos el turno anterior por defecto para la liquidacion
        public static int getToleranciaLiquidacion()
        {
            return -240;
        }
        // Constructor vacio
        public TurnoTrabajo()
        {
        }


        // Constructor utilizado para armar los objetos en la pagin y que se envian a grabar en la base de datos
        public TurnoTrabajo(Estacion oEstacion, int turno, string hora,
                            string horaAnterior, bool diaAnterior,
                            string horaPosterior, bool diaPosterior)
        {
            this.Estacion = oEstacion;
            this.NumeroTurno = turno;
            this.HoraString = hora;
            
            this.setToleranciaAnterior(horaAnterior, diaAnterior);
            this.setToleranciaPosterior(horaPosterior, diaPosterior);
        }


        // Constructor utilizado al levantar de la base de datos el registro del turno
        public TurnoTrabajo(Estacion oEstacion, int turno, string hora,
                            int toleranciaAnterior,
                            int toleranciaPosterior)
        {
            this.Estacion = oEstacion;
            this.NumeroTurno = turno;
            this.HoraString = hora;
            this.ToleranciaAnterior = toleranciaAnterior;
            this.ToleranciaPosterior = toleranciaPosterior;
        }


        // Ordinal del Numero de turno 
        public int NumeroTurno { get; set; }

        // Hora de inicio del turno (formato DateTime)
        public DateTime Hora { get; set; }
        
        // Estacion a la que pertenecen los turnos configurados
        public Estacion Estacion { get; set; }

        // Minutos de tolerancia anteriores del inicio del turno
        public int ToleranciaAnterior { get; set; }

        // Minutos de tolerancia posteriores al inicio del turno
        public int ToleranciaPosterior { get; set; }

        private DateTime horaFinal;

        //Sale del registro del turno siguiente
        public DateTime HoraFinal 
        {
            get
            {
                return horaFinal;
            }
            set
            {
                horaFinal = value;
                //La hora final es lo ultimo que se setea, asi que aqui calculamos la descripcion
                ActualizaDescripcion();
            }
        }

        // Hora de inicio del turno (formato String - hh:mm)
        public string HoraString
        {
            get
            {
                return Hora.ToString("HH:mm");
            }
            set
            {
                Hora = DateTime.Parse(value);
            }
        }


        //NOTA: Se debe setear luego de asignar la Hora
        public void setToleranciaAnterior(string hora, bool diaAnterior)
        {
            DateTime horaAnterior = DateTime.Parse( hora );
            if (diaAnterior)
                horaAnterior = horaAnterior.AddDays(-1);
            ToleranciaAnterior = (int)Hora.Subtract(horaAnterior).TotalMinutes;
        }


        //NOTA: Se debe setear luego de asignar la Hora
        public void setToleranciaPosterior(string hora, bool diaPosterior)
        {
            DateTime horaPosterior = DateTime.Parse(hora);
            if (diaPosterior)
                horaPosterior = horaPosterior.AddDays(1);
            ToleranciaPosterior = (int)horaPosterior.Subtract(Hora).TotalMinutes;

        }

        
        // Hora anterior al inicio del turno
        public string HoraAnterior
        {
            get
            {
                return Hora.AddMinutes(-ToleranciaAnterior).ToString("HH:mm");
            }
        }


        // Hora posterior al inicio del turno
        public string HoraPosterior
        {
            get
            {
                return Hora.AddMinutes(ToleranciaPosterior).ToString("HH:mm");
            }
        }
        
        
        // Marca que indica si la hora anterior pertenece al dia anterior o al dia actual
        public bool DiaAnterior
        {
            get
            {
                DateTime horaAnterior = Hora.AddMinutes(-ToleranciaAnterior);
                return (horaAnterior.Day != Hora.Day);
            }
        }

        
        // Marca que indica si la hora posterior pertenece al dia siguiente o al dia actual
        public bool DiaPosterior
        {
            get
            {
                DateTime horaPosterior = Hora.AddMinutes(ToleranciaPosterior);
                return (horaPosterior.Day != Hora.Day);
            }
        }

        
        // Descripcion que se muestra en los combos de seleccion de turnos (Informes, etc)
        public string Descripcion {get; set; }

        private void ActualizaDescripcion()
        {
            Descripcion = NumeroTurno.ToString() + " - " + HoraString + " / " + HoraFinal.ToString("HH:mm");
        }


    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos TurnoTrabajo
    /// Deberian ser todos de la misma estacion
    /// y agregarse por orden
    /// </summary>*********************************************************************************************
    public class TurnoTrabajoL : List<TurnoTrabajo>
    {

        //Al agregar un elemento le seteamos la Hora Final igual al comienzo del primero
        //y seteamos la hora final del ultimo que estaba igual a la inicial de este
        public new void Add(TurnoTrabajo oT)
        {
            int n = this.Count;
            if (n > 0)
            {
                this[n - 1].HoraFinal = oT.Hora;
                oT.HoraFinal = this[0].Hora;
            }
            else
            {
                oT.HoraFinal = oT.Hora;
            }
            base.Add(oT);
        }

        ///****************************************************************************************************<summary>
        /// Busca el turno de trabajo que abarca esta hora
        /// </summary>****************************************************************************************************
        public TurnoTrabajo Find(DateTime hora)
        {
            return this[FindPosition(hora)];
        }

        ///****************************************************************************************************<summary>
        /// Busca el turno de trabajo que abarca esta hora
        /// </summary>****************************************************************************************************
        protected int FindPosition(DateTime hora)
        {
            int i = 0;
            DateTime horasola = DateTime.Parse(hora.ToShortTimeString());
            foreach (TurnoTrabajo item in this)
            {
                if (item.Hora <= horasola && item.HoraFinal > horasola)
                {
                    break;
                }
                if (item.Hora > item.HoraFinal &&
                    (item.Hora <= horasola || item.HoraFinal > horasola))
                {
                    //Caso en que la hora final es anterior a la inicial
                    break;
                }
                i++;
            }
            return i;
        }

        ///****************************************************************************************************<summary>
        /// Busca la jornada y turno de trabajo en que estamos
        /// suma horasToler a la hora actual 
        /// por lo que el proximo turno se empieza a devolver 1 hora antes
        /// </summary>****************************************************************************************************
        public void FindTurnoActual(int minutosToler, out DateTime jornada, out int turno)
        {
            DateTime fecha = DateTime.Now.AddMinutes(minutosToler);
            int pos = FindPosition(fecha);
            TurnoTrabajo oTurno = this[pos];
            turno = oTurno.NumeroTurno;
            jornada = fecha.Date;
            if (oTurno.Hora > oTurno.HoraFinal)
            {
                if (pos == 0)
                {
                    //Estamos en el primer turno y empieza antes de medianoche
                    //Jornada siguiente
                    jornada = jornada.AddDays(1);
                }
                else if (fecha < oTurno.HoraFinal)
                {
                    //Estamos en el turno que termina despues de medianoche
                    //y es la madrugada
                    //Jornada anterior
                    jornada = jornada.AddDays(-1);
                }
            }
        }

        ///****************************************************************************************************<summary>
        /// Nos dice la minima y maxima jornada que podriamos asignar
        /// si el primer turno empieza antes de medianoche y termina despues es hoy o mañana
        /// Si el primer turno empieza a las 0:00 y son mas de las 22 tambien es mañana
        /// sino ayer y hoy
        /// </summary>****************************************************************************************************
        public void getJornadaMinMax(out DateTime jornadaMin, out DateTime jornadaMax)
        {
            jornadaMax = DateTime.Today;
            jornadaMin = jornadaMax;
            if (this[0].Hora > this[0].HoraFinal || this[0].HoraString == "00:00" && DateTime.Now.Hour >= 22)
            {
                jornadaMax = jornadaMax.AddDays(1);
            }
            else
            {
                jornadaMin = jornadaMin.AddDays(-1);
            }
        }

        public int getMaxTurno()
        {
            return this[this.Count - 1].NumeroTurno;
        }
        public int getMinTurno()
        {
            return this[0].NumeroTurno;
        }
    }

    #endregion

}

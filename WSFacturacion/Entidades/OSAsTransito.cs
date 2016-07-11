﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Tesoreria;
using Telectronica.Facturacion;
using Telectronica.Validacion;

namespace Telectronica.Peaje
{
    [Serializable]
    public class OSAsTransito
    {
        public Estacion Estacion { get; set; }
        public DateTime FechaJornada { get; set; }
        public int NumeroVia { get; set; }
        public DateTime Fecha { get; set; }
        public int? NumeroEvento { get; set; }
        public int? IdDov { get; set; }
        public string CodigoPais { get; set; }
        public string CodigoConcesionaria { get; set; }
        public string CodigoEmisorTag { get; set; }
        public string NumeroTag { get; set; }
        public string CodigoPlaza { get; set; }
        public string CodigoPista { get; set; }
        public CategoriaManual CategoriaDetectada { get; set; }
        public CategoriaManual CategoriaManual { get; set; }
        public CategoriaManual CategoriaConsolidada { get; set; }
        public int? EjeAdicionalManual { get; set; }
        public int? EjesSuspensoReal { get; set; }
        public int? EjeAdicionalDectectado { get; set; }
        public int? EjeAdicionalConsolidado { get; set; }
        public decimal Valor { get; set; }
        public string StatusCobrada { get; set; }
        public string StatusPasada { get; set; }
        public string MotivoImagen { get; set; }
        public string PathVideo { get; set; }
        public string FotoFrontal { get; set; }
        public string FotoLateral1 { get; set; }
        public string FotoLateral2 { get; set; }
        public string Placa { get; set; }
        public bool datosMal { get; set; }
        public bool impedimento { get; set; }
        public string causaDatosMal { get; set; }
        public string Estado { get; set; }

        //Datos del transito anterior grabado en el TAG
        public string PaisAnterior { get; set; }
        public string ConcesionAnterior { get; set; }
        public string PlazaAnterior { get; set; }
        public string PistaAnterior { get; set; }
        public DateTime? FechaAnterior { get; set; }
        public string SecuenciaTagAnterior { get; set; }
        

        public bool esEspecial { get; set; }
        //Atributos Necesarios para el Tratamiento de PEX

        //Modo de como la via trabaja el turno, Modo `M´ Manual,`MD´ Manual Dinamico,`D´ Dinamico
        public string ModoViaTurno { get; set; }
        public int Codigo { get; set; }
        public int NumeroSecuencia { get; set; }
        public string ModoLectura { get; set; }
        public string ModoViaCodigo { get; set; }
        public string ObservacionesInternasValidacion { get; set; }
        public string ObservacionesExternasValidacion { get; set; }
        public string ObservacionTratamiento { get; set; }
        public MotivoRechazo CausaRechazo { get; set; }
        public string Diponibilidad { get; set; }
        public string TipoDispositivo { get; set; }
        public string EstadoOSA { get; set; }
        //estado de transito sin ser tratado
        public char EstadoPexOriginal { get; set; }
        public Usuario Validador { get; set; }
        //n° de eje que en categoria especial
        public int NumeroEjes { get; set; }
        //tipo de dispositivo con el que paso el transito
        public char Tipbo { get; set; }
        public int? Parte { get; set; }

        public string TagTamperizado { get; set; }



        public OSAsTransito()
        {
        }


    }
        [Serializable]
    public class OSAsTransitoL : List<OSAsTransito> { }
    

}

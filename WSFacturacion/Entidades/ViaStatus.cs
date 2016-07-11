using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region Atributos
    [Serializable]
    [XmlRootAttribute(ElementName = "ViaStatus", IsNullable = false)]
    #endregion
    public class ViaStatus
    {

        public ViaStatus(int xiNumeroVia, string xsNombreVia, int xiNumeroCarril, string xsModelo, string xsModo, string xsEstado, string xsMonoC, string xsModoQ, int xiMinutosIncomunicada, DateTime xdUltFechaComunicacion, 
                            string xsId, int xiParte, string xsIdSup, string xsLecto, string xsTelep, string xsChip, string xsVide1, string xsVide2, 
                            string xsSemar, string xsSepas, string xsBarre, string xsBaren, string xsCarte, string xsSepen, string xsSepsd, string xsFisca,
                            string xsPed01, string xsPed02, string xsPed03, string xsPed04, string xsPed57, string xsPed68, string xsBpr, string xsBpi,
                            string xsBpa, string xsTipOp, string xsTipBo, string xsFormaPago, int xiSubFp, int xiManua, int xiDac, int xiMannu, int xiCejes, int xiCdobl, 
                            string xsAltur, int xiCodis, string xsObservacion, string xsAbort, string xsNumer, int xiUlcle, int xiLcola, int xiLprec, decimal xdPorDis)
        {

            this.NumeroVia = xiNumeroVia;
            this.NombreVia = xsNombreVia;
            this.NumeroCarril = xiNumeroCarril;
            this.Modelo = xsModelo;
            this.Modo = xsModo;
            this.Estado = xsEstado;
            this.MonoC = xsMonoC;
            this.ModoQ = xsModoQ;
            this.MinutosIncomunicada = xiMinutosIncomunicada;
            this.UltFechaComunicacion = xdUltFechaComunicacion;
            this.Id = xsId;
            this.Parte = xiParte;
            this.IdSup = xsIdSup;
            this.Lecto = xsLecto;
            this.Telep = xsTelep;
            this.Chip = xsChip;
            this.Vide1 = xsVide1;
            this.Vide2 = xsVide2;
            this.Semar = xsSemar;
            this.Sepas = xsSepas;
            this.Barre = xsBarre;
            this.Baren = xsBaren;
            this.Carte = xsCarte;
            this.Sepen = xsSepen;
            this.Sepsd = xsSepsd;
            this.Fisca = xsFisca;
            this.Ped01 = xsPed01;
            this.Ped02 = xsPed02;
            this.Ped03 = xsPed03;
            this.Ped04 = xsPed04;
            this.Ped57 = xsPed57;
            this.Ped68 = xsPed68;
            this.Bpr = xsBpr;
            this.Bpi = xsBpi;
            this.Bpa = xsBpa;
            this.TipOp = xsTipOp;
            this.TipBo = xsTipBo;
            this.FormaPago = xsFormaPago;
            this.SubFp = xiSubFp;
            this.Manua = xiManua;
            this.Dac = xiDac;
            this.Mannu = xiMannu;
            this.Cejes = xiCejes;
            this.Cdobl = xiCdobl;
            this.Altur = xsAltur;
            this.Codis = xiCodis;
            this.Observacion = xsObservacion;
            this.Abort = xsAbort;
            this.Numer = xsNumer;
            this.Ulcle = xiUlcle;
            this.Lcola = xiLcola;
            this.Lprec = xiLprec;
            this.PorDis = xdPorDis;

        }

        public ViaStatus()
        {

        }

        // DEFINICION DE PROPIEDADES

        /// <summary>
        /// NRO DE VIA
        /// </summary>
        public int NumeroVia { get; set; }

        /// <summary>
        /// NOMBRE DE LA VIA
        /// </summary>
        public string NombreVia { get; set; }

        /// <summary>
        /// NRO DE CARRIL
        /// </summary>
        public int NumeroCarril { get; set; }
       
        /// <summary>
        /// MODO DE LA VIA: M, MD, SMD, DMD, D, E
        /// </summary>
        public string Modelo { get; set; }

        /// <summary>
        /// MODO DE OPERACION: M, D, MD
        /// </summary>
        public string Modo { get; set; }

        /// <summary>
        /// ESTADO DE LA VIA
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// MONOCATEGORIA
        /// </summary>
        public string MonoC { get; set; }

        /// <summary>
        /// MODO QUIEBRE: QC = CONTROLADO, QL = LIBERADO
        /// </summary>
        public string ModoQ { get; set; }

        /// <summary>
        /// TIEMPO INCOMUNICADA
        /// </summary>
        public int MinutosIncomunicada { get; set; }

        /// <summary>
        /// ULTIMA FECHA DE COMUNICACION DE LA VIA
        /// </summary>
        public DateTime? UltFechaComunicacion { get; set; }

        /// <summary>
        /// ID DE OPERADOR
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// PARTE QUE POSEE LA VIA ASIGNADO
        /// </summary>
        public int Parte { get; set; }

        /// <summary>
        /// ID DE SUPERVISOR
        /// </summary>
        public string IdSup { get; set; }

        /// <summary>
        /// TIENE LECTOR MAGNETICO ?
        /// </summary>
        public string Lecto { get; set; }

        /// <summary>
        /// TIENE TELEPEAJE ?
        /// </summary>
        public string Telep { get; set; }

        /// <summary>
        /// TIENE LECTOR CHIP ?
        /// </summary>
        public string Chip { get; set; }

        /// <summary>
        /// ESTADO DE LA CAMARA 1
        /// </summary>
        public string Vide1 { get; set; }

        /// <summary>
        /// ESTADO DE LA CAMARA 2
        /// </summary>
        public string Vide2 { get; set; }

        /// <summary>
        /// ESTADO SEMAFORO DE MARQUESINA
        /// </summary>
        public string Semar { get; set; }

        /// <summary>
        /// ESTADO SEMAFORO DE PASO
        /// </summary>
        public string Sepas { get; set; }

        /// <summary>
        /// BARRERA DE SALIDA
        /// </summary>
        public string Barre { get; set; }

        /// <summary>
        /// BARRERA DE ENTRADA
        /// </summary>
        public string Baren { get; set; }

        /// <summary>
        /// CARTEL DE MARQUESINA
        /// </summary>
        public string Carte { get; set; }

        /// <summary>
        /// SEPARADOR DE ENTRADA
        /// </summary>
        public string Sepen { get; set; }

        /// <summary>
        /// SEPARADOR DE SALIDA
        /// </summary>
        public string Sepsd { get; set; }

        /// <summary>
        /// CODIGOS DE LA IMPRESORA
        /// </summary>
        public string Fisca { get; set; }

        /// <summary>
        /// ESTADO PEDALERA 01
        /// </summary>
        public string Ped01 { get; set; }

        /// <summary>
        /// ESTADO PEDALERA 02
        /// </summary>
        public string Ped02 { get; set; }

        /// <summary>
        /// ESTADO PEDALERA 03
        /// </summary>
        public string Ped03 { get; set; }

        /// <summary>
        /// ESTADO PEDALERA 04
        /// </summary>
        public string Ped04 { get; set; }

        /// <summary>
        /// PEDALERAS DUALES
        /// </summary>
        public string Ped57 { get; set; }

        /// <summary>
        /// PEDALERAS DUALES
        /// </summary>
        public string Ped68 { get; set; }

        /// <summary>
        /// ESTADO Ejes Levantados
        /// </summary>
        public string EjesLevantados { get; set; }

        /// <summary>
        /// ESTADO DEL LAZO BPR
        /// </summary>
        public string Bpr { get; set; }

        /// <summary>
        /// ESTADO DEL LAZO BPI
        /// </summary>
        public string Bpi { get; set; }

        /// <summary>
        /// ESTADO DEL LAZO BPA
        /// </summary>
        public string Bpa { get; set; }

        /// <summary>
        /// FORMA DE PAGO
        /// </summary>
        public string TipOp { get; set; }

        /// <summary>
        /// TIPO DE MEDIO DE PAGO
        /// </summary>
        public string TipBo { get; set; }

        /// <summary>
        /// DESCRIPCION DE LA FORMA DE PAGO
        /// </summary>
        public string FormaPago { get; set; }

        /// <summary>
        /// NUMERO DE EXENTO
        /// </summary>
        public int SubFp { get; set; }

        /// <summary>
        /// CATEGORIA POR OPERADOR
        /// </summary>
        public int Manua { get; set; }

        /// <summary>
        /// CATEGORIA POR DAC
        /// </summary>
        public int Dac { get; set; }

        /// <summary>
        /// PROXIMA CATEGORIA A COBRAR
        /// </summary>
        public int Mannu { get; set; }

        /// <summary>
        /// PROXIMA CATEGORIA EJES DE SUSPENSO A COBRAR
        /// </summary>
        public int eMannu { get; set; }

        /// <summary>
        /// CATEGORIA EJES DE SUSPENSO A COBRAR
        /// </summary>
        public int eManua { get; set; }

        /// <summary>
        /// CATEGORIA POR OPERADOR
        /// </summary>
        public string sManua { get; set; }

        /// <summary>
        /// CATEGORIA EJES DE SUSPENSO POR OPERADOR
        /// </summary>
        public string seManua { get; set; }
        
        /// <summary>
        /// CATEGORIA POR DAC
        /// </summary>
        public string sDac { get; set; }

        /// <summary>
        /// PROXIMA CATEGORIA A COBRAR
        /// </summary>
        public string sMannu { get; set; }


        /// <summary>
        /// PROXIMA CATEGORIA EJES DE SUSPENSO A COBRAR
        /// </summary>
        public string seMannu { get; set; }

        /// <summary>
        /// CANTIDAD EJES SIMPLES
        /// </summary>
        public int Cejes { get; set; }

        /// <summary>
        /// CANTIDAD RUEDAS DOBLES
        /// </summary>
        public int Cdobl { get; set; }

        /// <summary>
        /// SENSOR DE ALTURA
        /// </summary>
        public string Altur { get; set; }

        /// <summary>
        /// CODIGO DE OBSERVACION
        /// </summary>
        public int Codis { get; set; }

        /// <summary>
        /// OBSERVACION
        /// </summary>
        public string Observacion { get; set; }

        /// <summary>
        /// TIPO DE ANOMALIA
        /// </summary>
        public string Abort { get; set; }

        /// <summary>
        /// NUMERO DE MEDIO DE PAGO
        /// </summary>
        public string Numer { get; set; }

        /// <summary>
        /// ULTIMO TICKET DE AUTORIZACION DE PASO
        /// </summary>
        public int Ulcle { get; set; }

        /// <summary>
        /// LONGITUD DE LA COLA DE VEHICULOS
        /// </summary>
        public int Lcola { get; set; }

        /// <summary>
        /// LONGITUD DE LA PRECOLA DE VEHICULOS
        /// </summary>
        public int Lprec { get; set; }

        /// <summary>
        /// PORCENTAJE DE DISCREPANCIA
        /// </summary>
        public decimal PorDis { get; set; }

        
        /// <summary>
        /// Estado Sensor Óptico
        /// </summary>
        public string Sepsm { get; set; }

        /// <summary>
        /// Obtiene o establece el valor de la propiedad esModoMantenimiento como booleana
        /// </summary>
        public string ModoMantenimiento
        {
            get { return (esModoMantenimiento ? "S" : "N"); }
            set { esModoMantenimiento = (value == "S");}
        }

        /// <summary>
        /// Obtiene o establece un valor que indica si está o no la vía en modo mantenimiento
        /// </summary>
        public bool esModoMantenimiento { get; set; }
    }

    #region Atributos
    /// <summary>
    /// Clase ViaStatusL
    /// </summary>
    [Serializable]
    #endregion
    public class ViaStatusL : List<ViaStatus>
    {
    }
}


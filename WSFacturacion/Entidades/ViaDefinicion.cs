using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Telectronica.Peaje
{
    #region VIADEFINICION: Clase para entidad de las vías definidas

    #region Atributos de la Clase
    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ViaDefinicion
    /// </summary>*********************************************************************************************
    [Serializable]
    [XmlRootAttribute(ElementName = "Via", IsNullable = false)]
    #endregion
    public class ViaDefinicion
    {
        #region Constructores

        /// <summary>
        /// Constructor Vacio
        /// </summary>
        public ViaDefinicion()
        {
        }

        /// <summary>
        /// Constructor con numero de via solamente
        /// </summary>
        /// <param name="numeroVia"></param>
        public ViaDefinicion(byte numeroVia)
        {
            this.NumeroVia = numeroVia;
        }

        /// <summary>
        /// Constructor que asigna las variables a la clase
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="numeroVia"></param>
        /// <param name="nombreVia"></param>
        /// <param name="carril"></param>
        /// <param name="modelo"></param>
        /// <param name="sentido"></param>
        /// <param name="autotabulante"></param>
        /// <param name="detectorEjes"></param>
        /// <param name="ruedasDuales"></param>
        /// <param name="sensorAltura"></param>
        /// <param name="lectograbador"></param>
        /// <param name="telepeaje"></param>
        /// <param name="tarjetaChip"></param>
        /// <param name="visacash"></param>
        /// <param name="aceptaClearing"></param>
        /// <param name="imprimeClearing"></param>
        /// <param name="aceptaVentaAncicipada"></param>
        /// <param name="imprimeVentaAncicipada"></param>
        /// <param name="videoCamara1"></param>
        /// <param name="pathArchivosVideo"></param>
        /// <param name="videoCamara2"></param>
        /// <param name="distanciaSensores"></param>
        /// <param name="puntoVenta"></param>
        /// <param name="viaControladora"></param>
        public ViaDefinicion(Estacion estacion, byte numeroVia, string nombreVia, byte carril, ViaModelo modelo,
                             ViaSentidoCirculacion sentido, string autotabulante, ViaDetectorEje detectorEjes, ViaRuedasDuales ruedasDuales,
                             string sensorAltura, string lectograbador, string telepeaje, string tarjetaChip,
                             string aceptaClearing, string imprimeClearing, string aceptaVentaAncicipada, string imprimeVentaAncicipada,
                             ViaVideo videoCamara1, string pathArchivosVideo, ViaVideo videoCamara2, int distanciaSensores,
                             string puntoVenta, ViaDefinicion viaControladora,string visacash)
        {
            this.Estacion = estacion;
            this.NumeroVia = numeroVia;
            this.NombreVia = nombreVia;
            this.Carril = carril;
            this.Modelo = modelo;
            this.SentidoCirculacion = sentido;
            this.Autotabulante = autotabulante;
            this.DetectorEjes = detectorEjes;
            this.RuedasDuales = ruedasDuales;
            this.SensorAltura = sensorAltura;
            this.Lectograbador = lectograbador;
            this.Telepeaje = telepeaje;
            this.TarjetaChip = tarjetaChip;
            this.AceptaClearing = aceptaClearing;
            this.ImprimeClearing = imprimeClearing;
            this.AceptaVentaAnticipada = aceptaVentaAncicipada;
            this.ImprimeVentaAnticipada = imprimeVentaAncicipada;
            this.VideoCamara1 = videoCamara1;
            this.VideoCamara2 = videoCamara2;
            this.PathArchivosVideo = pathArchivosVideo;
            this.DistanciaSensores = distanciaSensores;
            this.PuntoVenta = puntoVenta;
            this.ViaControladora = viaControladora;
            this.VisaCash = visacash;
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Estacion a la que pertenece la via
        /// </summary>
        public Estacion Estacion { get; set; }

        /// <summary>
        /// Baja Logica
        /// </summary>
        public string Eliminado { get; set; }

        /// <summary>
        /// Numero de Via
        /// </summary>
        public byte NumeroVia { get; set; }

        /// <summary>
        /// Numero de Carril en el que se encuentra la via
        /// </summary>
        public byte Carril { get; set; }

        /// <summary>
        /// Modelo de Via
        /// </summary>
        public ViaModelo Modelo { get; set; }

        /// <summary>
        /// Sentido de Circulacion
        /// </summary>
        public ViaSentidoCirculacion SentidoCirculacion { get; set; }

        /// <summary>
        /// Si la via tiene habilitado el modo autotabulante
        /// </summary>
        public string Autotabulante { get; set; }

        /// <summary>
        /// Si la via tiene habilitado el modo autotabulante (booleano, para la UI)
        /// </summary>
        public bool esAutotabulante
        {
            get { return Autotabulante == "S"; }
            set { Autotabulante = value ? "S" : "N"; }
        }

        /// <summary>
        /// Cantidad de Detectores de Ejes 
        /// </summary>
        public ViaDetectorEje DetectorEjes { get; set; }

        /// <summary>
        /// Cantidad de Detectores de Ruedas Dobles
        /// </summary>
        public ViaRuedasDuales RuedasDuales { get; set; }

        /// <summary>
        /// Sensor de Altura (S/N)
        /// </summary>
        public string SensorAltura { get; set; }

        /// <summary>
        /// Sensor de Altura (booleano para la UI)
        /// </summary>
        public bool esSensorAltura
        {
            get { return SensorAltura == "S"; }
            set { SensorAltura = value ? "S" : "N"; }
        }

        /// <summary>
        /// Lectograbador Magnetico (S/N)
        /// </summary>
        public string Lectograbador { get; set; }

        /// <summary>
        /// Lectograbador Magnetico (booleano para la UI)
        /// </summary>
        public bool esLectograbador
        {
            get { return Lectograbador == "S"; }
            set { Lectograbador = value ? "S" : "N"; }
        }

        /// <summary>
        /// Antena de Telepeaje (S/N) 
        /// </summary>
        public string Telepeaje { get; set; }

        /// <summary>
        /// Antena de Telepeaje (booleano para la UI)
        /// </summary>
        public bool esTelepeaje
        {
            get { return Telepeaje == "S"; }
            set { Telepeaje = value ? "S" : "N"; }
        }
        
        /// <summary>
        /// Lectograbador Chip (S/N)
        /// </summary>
        public string TarjetaChip { get; set; }

        /// <summary>
        /// Lectograbador Chip (booleano para la UI)
        /// </summary>
        public bool esTarjetaChip
        {
            get { return TarjetaChip == "S"; }
            set { TarjetaChip = value ? "S" : "N"; }
        }

        /// <summary>
        /// Si Acepta o no ('S'/'N') ticket de Autorizacion de paso
        /// </summary>
        public string AceptaClearing { get; set; }

        /// <summary>
        /// Si Acepta o no ticket de Autorizacion de paso (booleano para la UI)
        /// </summary>
        public bool esAceptaClearing
        {
            get { return AceptaClearing == "S"; }
            set { AceptaClearing = value ? "S" : "N"; }
        }

        /// <summary>
        /// Si Imprime o no ('S'/'N') ticket de Autorizacion de paso
        /// </summary>
        public string ImprimeClearing { get; set; }

        /// <summary>
        /// Si Imprime o no ticket de Autorizacion de paso (booleano para la UI)
        /// </summary>
        public bool esImprimeClearing
        {
            get { return ImprimeClearing == "S"; }
            set { ImprimeClearing = value ? "S" : "N"; }
        }

        /// <summary>
        /// Si Acepta Venta Anticipada
        /// </summary>
        public string AceptaVentaAnticipada { get; set; }

        /// <summary>
        /// Si Acepta Venta Anticipada (booleano para la UI)
        /// </summary>
        public bool esAceptaVentaAnticipada
        {
            get { return AceptaVentaAnticipada == "S"; }
            set { AceptaVentaAnticipada = value ? "S" : "N"; }
        }

        /// <summary>
        /// Si Imprime o no Venta Anicipada
        /// </summary>
        public string ImprimeVentaAnticipada { get; set; }

        /// <summary>
        /// Si Imprime o no Venta Anicipada (booleano para la UI)
        /// </summary>
        public bool esImprimeVentaAnticipada
        {
            get { return ImprimeVentaAnticipada == "S"; }
            set { ImprimeVentaAnticipada = value ? "S" : "N"; }
        }

        /// <summary>
        /// Accion que realiza con la camara1
        /// </summary>
        public ViaVideo VideoCamara1 { get; set; }

        /// <summary>
        /// Ubicacion de los archivos de Video
        /// </summary>
        public string PathArchivosVideo { get; set; }

        /// <summary>
        /// Accion que realiza con la camara2
        /// </summary>
        public ViaVideo VideoCamara2 { get; set; }

        /// <summary>
        /// Distancia entre el SVE y el BPR 
        /// </summary>
        public int DistanciaSensores { get; set; }

        /// <summary>
        /// Número de Punto de Venta
        /// </summary>
        public string PuntoVenta { get; set; }

        /// <summary>
        /// Numero de Via que controla a la presente via de escape
        /// </summary>
        public ViaDefinicion ViaControladora { get; set; }

        /// <summary>
        /// Nombre Corto de la vía
        /// </summary>
        public string NombreVia { get; set; }

        /// <summary>
        /// Nombre Largo de la vía
        /// </summary>
        public string NombreViaLargo { get; set; }

        /// <summary>
        /// Por defecto, el metodo ToString devuelve el nombre de via
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return NumeroVia.ToString();
        }

        /// <summary>
        /// Devolvemos el login de Sql de la via
        /// </summary>
        public string UsuarioSQL
        {
            get
            {
                return string.Format("Via{0:D3}{1:D2}", NumeroVia, Estacion.Numero);
            }
        }

        /// <summary>
        /// Indica si la vía acepta cobro a exento (S/N)
        /// </summary>
        public string CobroExento { get; set; }

        /// <summary>
        /// Indica si la vía acepta modo comboi (S/N)
        /// </summary>
        public string ModoComboio { get; set; }

        /// <summary>
        /// Indica si la vía acepta modo comboi (true/false)
        /// </summary>
        public bool esModoComboio
        {
            get { return (ModoComboio == "S");}
            set { ModoComboio = (value ? "S" : "N"); }
        }

        /// <summary>
        /// Indica si la vía cuenta o no con sensores Eixos (S/N)
        /// </summary>
        public string ConSensoresEixos { get; set; }

        /// <summary>
        /// Indica si la vía acepta cobro a exento (true / false)
        /// </summary>
        public bool esCobroExento
        {
            get { return (CobroExento == "S"); }
            set { CobroExento = (value ? "S" : "N"); }
        }

        /// <summary>
        /// Indica si la vía cuenta o no con sensores Eixos (true / false)
        /// </summary>
        public bool esConSensoresEixos
        {
            get { return (ConSensoresEixos == "S"); }
            set { ConSensoresEixos = (value ? "S" : "N"); }
        }

        /// <summary>
        /// Aloja la ruta donde se guardan las fotos
        /// </summary>
        public string PathFotos { get; set; }

        /// <summary>
        /// Tiene VisaCash (S/N) 
        /// </summary>
        public string VisaCash { get; set; }

        /// <summary>
        /// Pinpad de visacash
        /// </summary>
        public bool esVisaCash
        {
            get { return VisaCash == "S"; }
            set { VisaCash = value ? "S" : "N"; }
        }

        /// <summary>
        /// Obtiene o establece el número de antena en la vía
        /// </summary>
        public string ViaAntena { get; set; }

        #endregion
    }

    #region Atributos de la Clase
    [Serializable]
    [XmlRootAttribute(ElementName = "Via", IsNullable = false)]
    #endregion
    public class Via
    {
        #region Constructores

        /// <summary>
        /// Constructor por Defecto
        /// </summary>
        public Via()
        {
        }

        /// <summary>
        /// Constructor con 3 parámetros
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="numeroVia"></param>
        /// <param name="nombreVia"></param>
        public Via(int estacion, byte numeroVia, string nombreVia)
        {
            this.Estacion = estacion;
            this.NumeroVia = numeroVia;
            this.NombreVia = nombreVia;
        }

        /// <summary>
        /// Constructor con 2 parámetros
        /// </summary>
        /// <param name="estacion"></param>
        /// <param name="numeroVia"></param>
        public Via(int estacion, byte numeroVia)
        {
            this.Estacion = estacion;
            this.NumeroVia = numeroVia;
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Estacion a la que pertenece la via
        /// </summary>
        public int Estacion { get; set; }

        /// <summary>
        /// Numero de Via
        /// </summary>
        public byte NumeroVia { get; set; }

        /// <summary>
        /// Nombre de vía 
        /// </summary>
        public string NombreVia { get; set; }

        #endregion
    }

    #region Atributos de la Clase
    [Serializable]
    [XmlRootAttribute(ElementName = "ViaComando", IsNullable = false)]
    #endregion
    public class ViaComando
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public ViaComando()
        {
        }

        /// <summary>
        /// Constructor con muchos parámetros
        /// </summary>
        /// <param name="xiEstacion"></param>
        /// <param name="xbNumeroVia"></param>
        /// <param name="xsNombreVia"></param>
        /// <param name="xsSentido"></param>
        /// <param name="xsModelo"></param>
        /// <param name="xsModo"></param>
        /// <param name="xsEstado"></param>
        /// <param name="xsEstadoOpuesta"></param>
        /// <param name="xModosPosiblesApertura"></param>
        /// <param name="xsModoQuiebre"></param>
        public ViaComando(int xiEstacion, byte xbNumeroVia, string xsNombreVia, string xsSentido, string xsModelo, string xsModo, string xsEstado, string xsEstadoOpuesta, ViaModoL xModosPosiblesApertura, string xsModoQuiebre)
        {
            Estacion = xiEstacion;
            NumeroVia = xbNumeroVia;
            NombreVia = xsNombreVia;
            Sentido = xsSentido;
            Modelo = xsModelo;
            Modo = xsModo;
            Estado = xsEstado;
            EstadoOpuesta = xsEstadoOpuesta;
            ModosPosiblesApertura = xModosPosiblesApertura;
            ModoQuiebre = xsModoQuiebre;
        }

        #endregion

        #region Atributos

        /// <summary>
        /// ESTACION A LA QUE PERTENECE
        /// </summary>
        public int Estacion { get; set; }

        /// <summary>
        /// NRO. DE VIA
        /// </summary>
        public int NumeroVia { get; set; }

        /// <summary>
        /// NOMBRE DE LA VIA
        /// </summary>
        public string NombreVia { get; set; }

        /// <summary>
        /// NRO. DE CARRIL
        /// </summary>
        public int NumeroCarril { get; set; }

        /// <summary>
        /// SENTIDO
        /// </summary>
        public string Sentido { get; set; }

        /// <summary>
        /// MODELO
        /// </summary>
        public string Modelo { get; set; }

        /// <summary>
        /// MODO
        /// </summary>
        public string Modo { get; set; }

        /// <summary>
        /// ESTADO
        /// </summary>
        public string Estado { get; set; }

        /// <summary>
        /// ESTADO DE LA OPUESTA, PUEDE SER QUE NO TENGA OPUESTA
        /// </summary>
        public string EstadoOpuesta { get; set; }

        /// <summary>
        /// POSIBLES MODOS EN QUE PUEDE SER ABIERTA LA VIA, DEPENDERA DEL MODELO DE LA MISMA
        /// </summary>
        public ViaModoL ModosPosiblesApertura { get; set; }

        /// <summary>
        /// MODO QUIEBRE, C = CONTROLADO, L = LIBERADO
        /// </summary>
        public string ModoQuiebre { get; set; }

        /// <summary>
        /// INDICA SI LA VÍA PERMITE EXENTOS
        /// </summary>
        public string CobroExento { get; set; }

        /// <summary>
        /// INDICA SI LA VÍA PERMITE MODO COMBOIO
        /// </summary>
        public string ModoComboio { get; set; }

        /// <summary>
        /// Obtiene o establece el color del semáforo de marquesina
        /// </summary>
        public string SemaforoMarquesina { get; set; }

        #endregion
    }

    #region Atributos de la clase
    [Serializable]
    [XmlRootAttribute(ElementName = "Carril", IsNullable = false)]
    #endregion
    public class Carril
    {
        #region Constructores

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public Carril()
        {
        }

        /// <summary>
        /// Constructor con el número de carril
        /// </summary>
        /// <param name="numeroCarril"></param>
        public Carril(byte numeroCarril)
        {
            this.NumeroCarril = numeroCarril;
        }

        #endregion

        /// <summary>
        /// Numero de Carril
        /// </summary>
        public byte NumeroCarril { get; set; }
    }

    #region Atributos de la clase
    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaDefinicion
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class ViaDefinicionL : List<ViaDefinicion>
    {
        public ViaL getViasSolas()
        {
            ViaL oVias = new ViaL();
            foreach (ViaDefinicion item in this)
            {
                oVias.Add(new Via(item.Estacion.Numero, item.NumeroVia, item.NombreVia));
            }
            return oVias;
        }
    }

    #region Atributos de la clase
    /// *********************************************************************************************<summary>
    /// Lista de objetos Via
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class ViaL : List<Via>
    {
    }
    
    #region Atributos de la clase
    /// *********************************************************************************************<summary>
    /// Lista de objetos ViaComando
    /// </summary>*********************************************************************************************
    [Serializable]
    #endregion
    public class ViaComandoL : List<ViaComando>
    {
    }

    #region Atributos de la clase
    /// *********************************************************************************************<summary>
    /// Lista de objetos Carril
    /// </summary>*********************************************************************************************
    #endregion
    public class CarrilL : List<Carril>
    {
    }

    #endregion
}

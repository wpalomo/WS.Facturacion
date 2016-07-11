using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;

namespace Telectronica.Facturacion
{
    #region VehiculoInfo

    [Serializable]
    public class VehiculoInfo
    {
        /********************************************************************************************************
         *          
         * ESTA ENTIDAD LA CREO CRISTIAN PARA USAR EN EL CLIENTE GAFICO. DADO QUE LA ESTRUCTURA QUE NECESITAMOS ES 
         * MAS COMPLEJA (COMO ELEMENTO NOMBRE DEBERIA TENER UN ELEMENTO CLIENTE() ENTONCES HACEMOS UNA ENTIDAD NUEVA
         * LLAMADA "VEHICULO" QUE ES LA QUE USAREMOS EN EL RESTO DEL SISTEMA QUE NO SEA EL CLIENTE GRAFICO.
         * 
        *********************************************************************************************************/
        public VehiculoInfo(string nombreCliente, string marca, string modelo, string color, string categoria)
        {
            this.nombreCliente = nombreCliente;
            this.marca = marca;
            this.modelo = modelo;
            this.color = color;
            this.categoria = categoria;
        }

        /// <summary>
        /// Constructor Por Defecto
        /// </summary>
        public VehiculoInfo()
        {
        }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string nombreCliente { get; set; }

        /// <summary>
        /// Marca del vehiculo
        /// </summary>
        public string marca { get; set; }

        /// <summary>
        /// Modelo del vehiculo
        /// </summary>
        public string modelo { get; set; }

        /// <summary>
        /// Color del vehiculo
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// Categoría
        /// </summary>
        public string categoria { get; set; }
    }

    [Serializable]
    public class VehiculoInfoL : List<VehiculoInfo>
    {
    }

    #endregion

    #region VehiculoEstacionesHabilitadas

    [Serializable]
    public class VehiculoEstacionesHabilitadas
    {
        /// <summary>
        /// Constructor Por Defecto
        /// </summary>
        public VehiculoEstacionesHabilitadas()
        {
        }

        /// <summary>
        /// Patente
        /// </summary>
        public string Patente { get; set; }

        /// <summary>
        /// Tag Asociado al Vehiculo
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Categoría
        /// </summary>
        public string categoria { get; set; }

        /// <summary>
        /// Obtiene o establece las estaciones habilitadas de un vehículo
        /// </summary>
        public SubestacionL SubestacionesHabilitadas { get; set; }
    }

    [Serializable]
    public class VehiculoEstacionesHabilitadasL : List<VehiculoEstacionesHabilitadas>
    {
    }

    #endregion

    #region VehiculoInfoCompleta

    [Serializable]
    public class VehiculoInfoCompleta
    {
        /// <summary>
        /// Constructor Con muchos paràmetros
        /// </summary>
        /// <param name="clienteCodigo"></param>
        /// <param name="clienteNombre"></param>
        /// <param name="patente"></param>
        /// <param name="catCodigo"></param>
        /// <param name="catDescripcion"></param>
        /// <param name="marcaCodigo"></param>
        /// <param name="marcaDescripcion"></param>
        /// <param name="modeloCodigo"></param>
        /// <param name="modeloDescripcion"></param>
        /// <param name="colorCodigo"></param>
        /// <param name="colorDescripcion"></param>
        /// <param name="tipoCuenta"></param>
        /// <param name="causaInhabilitacion"></param>
        /// <param name="saldo"></param>
        /// <param name="maxGiroEnRojo"></param>
        /// <param name="chipNumero"></param>
        /// <param name="chipNumeroExterno"></param>
        /// <param name="chipEnListaNegra"></param>
        /// <param name="tagNumero"></param>
        /// <param name="tagEnListaNegra"></param>
        public VehiculoInfoCompleta(int clienteCodigo, string clienteNombre, string patente, int catCodigo, string catDescripcion,
                                    int marcaCodigo, string marcaDescripcion, int modeloCodigo, string modeloDescripcion, int colorCodigo,
                                    string colorDescripcion, string tipoCuenta, string causaInhabilitacion, string saldo, string maxGiroEnRojo,
                                    string chipNumero, string chipNumeroExterno, string chipEnListaNegra, string tagNumero, string tagEnListaNegra)
        {
            this.clienteCodigo = clienteCodigo;
            this.clienteNombre = clienteNombre;
            this.patente = patente;
            this.catCodigo = catCodigo;
            this.catDescripcion = catDescripcion;
            this.marcaCodigo = marcaCodigo;
            this.marcaDescripcion = marcaDescripcion;
            this.modeloCodigo = modeloCodigo;
            this.modeloDescripcion = modeloDescripcion;
            this.colorCodigo = colorCodigo;
            this.colorDescripcion = colorDescripcion;
            this.tipoCuenta = tipoCuenta;
            this.causaInhabilitacion = causaInhabilitacion;
            this.saldo = saldo;
            this.maxGiroEnRojo = maxGiroEnRojo;
            this.chipNumero = chipNumero;
            this.chipNumeroExterno = chipNumeroExterno;
            this.chipEnListaNegra = chipEnListaNegra;
            this.tagNumero = tagNumero;
            this.tagEnListaNegra = tagEnListaNegra;
        }

        /// <summary>
        /// Constructor Por Defecto
        /// </summary>
        public VehiculoInfoCompleta()
        {
        }

        /// <summary>
        /// Código del cliente
        /// </summary>
        public int clienteCodigo { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string clienteNombre { get; set; }

        /// <summary>
        /// Patente
        /// </summary>
        public string patente { get; set; }

        /// <summary>
        /// Obtiene o establece el emisor del tag asociado al vehiculo
        /// </summary>
        public string Emisor { get; set; }

        /// <summary>
        /// Código de la categoría
        /// </summary>
        public int catCodigo { get; set; }

        /// <summary>
        /// Descripción de la categoría
        /// </summary>
        public string catDescripcion { get; set; }

        /// <summary>
        /// Código de la marca
        /// </summary>
        public int marcaCodigo { get; set; }

        /// <summary>
        /// Marca del vehiculo
        /// </summary>
        public string marcaDescripcion { get; set; }

        /// <summary>
        /// Código del modelo
        /// </summary>
        public int modeloCodigo { get; set; }

        /// <summary>
        /// Modelo del vehiculo
        /// </summary>
        public string modeloDescripcion { get; set; }

        /// <summary>
        /// Código del color
        /// </summary>
        public int colorCodigo { get; set; }

        /// <summary>
        /// Color del vehiculo
        /// </summary>
        public string colorDescripcion { get; set; }

        /// <summary>
        /// Tipo de cuenta
        /// </summary>
        public string tipoCuenta { get; set; }

        /// <summary>
        /// Causa inhabilitación
        /// </summary>
        public string causaInhabilitacion { get; set; }

        /// <summary>
        /// Saldo
        /// </summary>
        public string saldo { get; set; }

        /// <summary>
        /// Máximo para giro en rojo
        /// </summary>
        public string maxGiroEnRojo { get; set; }

        /// <summary>
        /// Número de chip
        /// </summary>
        public string chipNumero { get; set; }

        /// <summary>
        /// Número externo de chip
        /// </summary>
        public string chipNumeroExterno { get; set; }

        /// <summary>
        /// Chip en lista negra
        /// </summary>
        public string chipEnListaNegra { get; set; }

        /// <summary>
        /// Número de Tag
        /// </summary>
        public string tagNumero { get; set; }

        /// <summary>
        /// Tag Asociado al Vehiculo
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Tag en lista negra
        /// </summary>
        public string tagEnListaNegra { get; set; }

        /// <summary>
        /// Obtiene o establece el código de exento asociado al vehiculo
        /// </summary>
        public ExentoCodigo Exento { get; set; }

        /// <summary>
        /// Obtiene o establece el codigo de autorizacion del vehiculo (exclusivo para la ARTESP)
        /// </summary>
        public string CodigoAutorizacion { get; set; }

        // Concesionaria
        public string Concesionaria { get; set; }

        // Lista Negra
        public string EsListaNegra { get; set; }

        // List Blaca
        public string EsListaBlanca { get; set; }

        // Lista a Validar
        public string ListaAValidar { get; set; }


    }

    [Serializable]
    public class VehiculoInfoCompletaL : List<VehiculoInfoCompleta>
    {
    }

    #endregion

    #region VehiculoInfoUltTransitos

    [Serializable]
    public class VehiculoInfoUltTransitos
    {
        /// <summary>
        /// Constructor con muchos parámetros
        /// </summary>
        /// <param name="estacionCodigo"></param>
        /// <param name="estacionNombre"></param>
        /// <param name="via"></param>
        /// <param name="nombreVia"></param>
        /// <param name="sentido"></param>
        /// <param name="fechaHora"></param>
        /// <param name="catTab"></param>
        /// <param name="catDac"></param>
        /// <param name="catCons"></param>
        /// <param name="formaPago"></param>
        /// <param name="tipo"></param>
        /// <param name="clienteNombre"></param>
        /// <param name="videoFoto1"></param>
        /// <param name="videoFoto2"></param>
        public VehiculoInfoUltTransitos(int estacionCodigo, string estacionNombre, byte via, string nombreVia, string sentido, DateTime fechaHora, int catTab, int catDac, int catCons,
                                        string formaPago, string tipo, string clienteNombre, string videoFoto1, string videoFoto2)
        {
            this.estacionCodigo = estacionCodigo;
            this.estacionNombre = estacionNombre;
            this.via = new Via(estacionCodigo, via, nombreVia);
            this.sentido = sentido;
            this.fechaHora = fechaHora;
            this.catTab = catTab;
            this.catDac = catDac;
            this.catCons = catCons;
            this.formaPago = formaPago;
            this.tipo = tipo;
            this.clienteNombre = clienteNombre;
            this.videoFoto1 = videoFoto1;
            this.videoFoto2 = videoFoto2;
        }

        public VehiculoInfoUltTransitos()
        {

        }

        /// <summary>
        /// Código de la estación
        /// </summary>
        public int estacionCodigo { get; set; }

        /// <summary>
        /// Nombre de la estación
        /// </summary>
        public string estacionNombre { get; set; }

        /// <summary>
        /// Número de vía
        /// </summary>
        public Via via { get; set; }

        /// <summary>
        /// Sentido
        /// </summary>
        public string sentido { get; set; }

        /// <summary>
        /// Fecha y Hora
        /// </summary>
        public DateTime fechaHora { get; set; }

        /// <summary>
        /// Categoría tabulada
        /// </summary>
        public int catTab { get; set; }

        /// <summary>
        /// Categoría detectada
        /// </summary>
        public int catDac { get; set; }

        /// <summary>
        /// Categoría consolidada
        /// </summary>
        public int catCons { get; set; }

        /// <summary>
        /// Categoría tabulada
        /// </summary>
        public string catTabulada { get; set; }

        /// <summary>
        /// Categoría detectada
        /// </summary>
        public string catDetectada { get; set; }

        /// <summary>
        /// Categoría consolidada
        /// </summary>
        public string catConsolidada { get; set; }

        /// <summary>
        /// Forma de pago
        /// </summary>
        public string formaPago { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string clienteNombre { get; set; }

        /// <summary>
        /// Video/Foto 1
        /// </summary>
        public string videoFoto1 { get; set; }

        /// <summary>
        /// Video/Foto 2
        /// </summary>
        public string videoFoto2 { get; set; }

        /// <summary>
        /// Obtiene o establece la patente del vehículo
        /// </summary>
        public string Patente { get; set; }

        /// <summary>
        /// Obtiene o establece el tag de vehículo
        /// </summary>
        public Tag Tag { get; set; }

        /// <summary>
        /// Obtiene o establece el número de evento asociado al tránsito
        /// </summary>
        public int NumeroDeEvento { get; set; }

        /// <summary>
        /// Cantidad ejes simples
        /// </summary>
        public int cantEjesSimples { get; set; }

        /// <summary>
        /// Cantidad ejes dobles
        /// </summary>
        public int cantEjesDobles { get; set; }

        /// <summary>
        /// Cantidad ejes suspensos
        /// </summary>
        public int cantEjesSusp { get; set; }

        /// <summary>
        /// Altura
        /// </summary>
        public string Altura { get; set; }

        /// <summary>
        /// Ticket
        /// </summary>
        public int Ticket { get; set; }

        /// <summary>
        /// Tag Violado.
        /// </summary>
        public string TagTamperizado { get; set; }
    }

    [Serializable]
    public class VehiculoInfoUltTransitosL : List<VehiculoInfoUltTransitos>
    {
    }

    #endregion

    #region VehiculoInfoDeudaDetalle

    [Serializable]
    public class VehiculoInfoDeudaDetalle
    {
        /// <summary>
        /// Constructor con muchos parámetros
        /// </summary>
        /// <param name="estacionNombre"></param>
        /// <param name="via"></param>
        /// <param name="sentido"></param>
        /// <param name="fechaHora"></param>
        /// <param name="catTab"></param>
        /// <param name="catDac"></param>
        /// <param name="catCons"></param>
        /// <param name="importe"></param>
        /// <param name="tipo"></param>
        /// <param name="marca"></param>
        /// <param name="clienteNombre"></param>
        /// <param name="numeroPD"></param>
        /// <param name="videoFoto1"></param>
        /// <param name="videoFoto2"></param>
        public VehiculoInfoDeudaDetalle(string estacionNombre, int via, string sentido, DateTime fechaHora, int catTab, int catDac, int catCons,
                                        string importe, string tipo, string marca, string clienteNombre, string numeroPD, string videoFoto1, string videoFoto2)
        {
            this.estacionNombre = estacionNombre;
            this.via = via;
            this.sentido = sentido;
            this.fechaHora = fechaHora;
            this.catTab = catTab;
            this.catDac = catDac;
            this.catCons = catCons;
            this.importe = importe;
            this.tipo = tipo;
            this.marca = marca;
            this.clienteNombre = clienteNombre;
            this.numeroPD = numeroPD;
            this.videoFoto1 = videoFoto1;
            this.videoFoto2 = videoFoto2;
        }

        /// <summary>
        /// Constructor por defecto
        /// </summary>
        public VehiculoInfoDeudaDetalle()
        {

        }

        /// <summary>
        /// Nombre de la estación
        /// </summary>
        public string estacionNombre { get; set; }

        /// <summary>
        /// Número de vía
        /// </summary>
        public int via { get; set; }

        /// <summary>
        /// Sentido
        /// </summary>
        public string sentido { get; set; }

        /// <summary>
        /// Fecha y Hora
        /// </summary>
        public DateTime fechaHora { get; set; }

        /// <summary>
        /// Categoría tabulada
        /// </summary>
        public int catTab { get; set; }

        /// <summary>
        /// Categoría detectada
        /// </summary>
        public int catDac { get; set; }

        /// <summary>
        /// Categoría consolidada
        /// </summary>
        public int catCons { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        public string importe { get; set; }

        /// <summary>
        /// Tipo
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// Marca
        /// </summary>
        public string marca { get; set; }

        /// <summary>
        /// Nombre del cliente
        /// </summary>
        public string clienteNombre { get; set; }

        /// <summary>
        /// Número PD
        /// </summary>
        public string numeroPD { get; set; }

        /// <summary>
        /// Video/Foto 1
        /// </summary>
        public string videoFoto1 { get; set; }

        /// <summary>
        /// Video/Foto 2
        /// </summary>
        public string videoFoto2 { get; set; }



		public int eventoNumero { get; set; }

		public byte estacionNumero { get; set; }
	}

    [Serializable]
    public class VehiculoInfoDeudaDetalleL : List<VehiculoInfoDeudaDetalle>
    {
    }

    #endregion

    #region VehiculoInfoDeudaTotal

    [Serializable]
    public class VehiculoInfoDeudaTotal
    {

        public VehiculoInfoDeudaTotal(string tipo, int cantidad, string importe)
        {
            this.tipo = tipo;
            this.cantidad = cantidad;
            this.importe = importe;
        }

        public VehiculoInfoDeudaTotal()
        {

        }

        /// <summary>
        /// Tipo
        /// </summary>
        public string tipo { get; set; }

        /// <summary>
        /// Cantidad
        /// </summary>
        public int cantidad { get; set; }

        /// <summary>
        /// Importe
        /// </summary>
        public string importe { get; set; }
    }

    [Serializable]
    public class VehiculoInfoDeudaTotalL : List<VehiculoInfoDeudaTotal>
    {
    }

    #endregion
}


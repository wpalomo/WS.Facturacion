using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{

    /// <summary>
    /// Estructura de una identidad de Tipo ListaTarjetasRUTA
    /// </summary>
    [Serializable]
    public class ListaTarjetasRUTA
    {

        #region Propiedades

        //Numero de Lista
        public int NumeroLista { get; set; }

        //Fecha de operacion
        public DateTime FechaOperacion { get; set; }

        //Fecha del Archivo de Mensajes
        public DateTime FechaArchivoMensajes { get; set; }

        //Fecha del Archivo de Certificados
        public DateTime FechaArchivoCertificados { get; set; }

        //Fecha del Archivo de Tarjetas
        public DateTime FechaArchivoTarjetas { get; set; }

        //Status Activo, Importando, Viejo
        public char Status { get; set; }

        // Descripcion del status de la lista
        public string StatusDescripcion 
        {
            get
            {
                string descripcion = "";

                switch (Status)
                {
                    case 'A':
                            descripcion = "AAAA";
                            break;

                    case 'V':
                            descripcion = "VVVV";
                            break;

                    default:
                        descripcion = "";
                        break;
                }

                return descripcion;
            }
        }

        //Usuario
        public string Usuario { get; set; }

        //Usuario
        public Usuario oUsuario { get; set; }

        //Estacion de Origen
        public int EstacionOrigen { get; set; }

        //Estacion de Origen
        public Estacion oEstacionOrigen { get; set; }

        //Descripcion de la estacion de origen
        public string EstacionOrigenDescripcion
        {
            get
            {
                return oEstacionOrigen.Nombre;
            }
        }

        #endregion

        #region Metodos

        //Constructor vacio
        public ListaTarjetasRUTA() { }

        //Constructor basico
        public ListaTarjetasRUTA(int iNumeroLista, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }

        //Constructor completo
        public ListaTarjetasRUTA(int iNumeroLista, DateTime dtFechaOperacion, DateTime dtFechaArchivoCertificados, DateTime dtFechaArchivoTarjetas, char cStatus, Usuario oUser, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            FechaOperacion = dtFechaOperacion;
            FechaArchivoCertificados = dtFechaArchivoCertificados;
            FechaArchivoTarjetas = dtFechaArchivoTarjetas;
            Status = cStatus;
            oUsuario = oUser;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }

        #endregion

    }

    /// <summary>
    /// Lista de objetos ListaTarjetasRUTA.
    /// </summary>
    [Serializable]
    public class ListaTarjetasRUTAL : List<ListaTarjetasRUTA>
    {
    }

}

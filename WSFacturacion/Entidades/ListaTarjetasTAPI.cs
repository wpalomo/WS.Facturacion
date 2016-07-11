using System;
using System.Collections.Generic;

namespace Telectronica.Peaje
{

    /// <summary>
    /// Estructura de una identidad de Tipo ListaTarjetasTAPI
    /// </summary>
    [Serializable]
    public class ListaTarjetasTAPI
    {

        #region Propiedades

        //Numero de Lista
        public int NumeroLista { get; set; }

        //Fecha de operacion
        public DateTime FechaOperacion { get; set; }

        //Fecha del Archivo
        public DateTime FechaArchivo { get; set; }

        //Status Activo, Importando, Viejo
        public char Status { get; set; }

        //Usuario
        public string Usuario { get; set; }

        //Usuario
        public Usuario oUsuario
        {
            get 
            {
                return oUsuario;
            }
            set
            {
                oUsuario = value;
                Usuario = oUsuario.ID;

            }
        }

        //Estacion de Origen
        public int EstacionOrigen { get; set; }

        //Estacion de Origen
        public Estacion oEstacionOrigen
        {
            get
            {

                return oEstacionOrigen;
            }

            set
            {
                oEstacionOrigen = value;
                EstacionOrigen = oEstacionOrigen.Numero;
            }
        }

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
        public ListaTarjetasTAPI() { }

        //Constructor basico
        public ListaTarjetasTAPI(int iNumeroLista, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }

        //Constructor completo
        public ListaTarjetasTAPI(int iNumeroLista, DateTime dtFechaOperacion, DateTime dtFechaArchivo, char cStatus, Usuario oUser, Estacion oEstacion)
        {
            NumeroLista = iNumeroLista;
            FechaOperacion = dtFechaOperacion;
            FechaArchivo = dtFechaArchivo;
            Status = cStatus;
            oUsuario = oUser;
            oEstacionOrigen = oEstacion;
            EstacionOrigen = oEstacion.Numero;
        }

        #endregion

    }

    /// <summary>
    /// Lista de objetos ListaTarjetasTAPI.
    /// </summary>
    [Serializable]
    public class ListaTarjetasTAPIL : List<ListaTarjetasTAPI>
    {
    }

}

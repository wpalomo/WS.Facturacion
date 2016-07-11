using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Peaje
{

    #region VIDEOACCION: Clase para entidad de las Acciones configuradas para captura de video

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad VideoAccion
    /// </summary>*********************************************************************************************    

    [Serializable]
    
    public class VideoAccion
    {
        //Constructor por defecto
        public VideoAccion()
        {
        }

        /// ***********************************************************************************************
        /// <summary>
        /// En el constructor de la clase asigna los valores de un Codigo de Accion en particular
        /// </summary>
        /// ***********************************************************************************************
        public VideoAccion(ViaModelo modeloVia, VideoAccionCodigo accion, string comienzoGrabacionC1, string finalizaGrabacionC1, string comienzoGrabacionC2, string finalizaGrabacionC2)
        {
            this.ModeloVia = modeloVia;
            this.Accion = accion;
            this.ComienzaGrabacionC1 = comienzoGrabacionC1;
            this.FinalizaGrabacionC1 = finalizaGrabacionC1;
            this.ComienzaGrabacionC2 = comienzoGrabacionC2;
            this.FinalizaGrabacionC2 = finalizaGrabacionC2;
        }


        // Modelo de Via
        public ViaModelo ModeloVia { get; set; }

        // Codigo de Accion
        public VideoAccionCodigo Accion{ get; set; }

        // Exponemos el valor del codigo de accion para el DataKeyName
        public string vAccion 
        {
            get { return Accion.Codigo; } 
        }


        // Si comienza o no grabacion de camara 1
        public string ComienzaGrabacionC1 { get; set; }

        // Determina si debe comenzar la grabacion de la camara 1
        public bool esComienzaGrabacionC1
        {
            get { return ComienzaGrabacionC1 == "S"; }
            set { ComienzaGrabacionC1 = value ? "S" : "N"; }
        }


        // Si finaliza o no la grabacion de la camara 1
        public string FinalizaGrabacionC1 { get; set; }

        // Determina si debe finalizar la grabacion de la camara 1
        public bool esFinalizaGrabacionC1
        {
            get { return FinalizaGrabacionC1 == "S"; }
            set { FinalizaGrabacionC1 = value ? "S" : "N"; }
        }


        
        // Si comienza o no grabacion de camara 2
        public string ComienzaGrabacionC2 { get; set; }

        // Determina si debe comenzar la grabacion de la camara 2
        public bool esComienzaGrabacionC2
        {
            get { return ComienzaGrabacionC2 == "S"; }
            set { ComienzaGrabacionC2 = value ? "S" : "N"; }
        }



        // Si finaliza o no la grabacion de la camara 2
        public string FinalizaGrabacionC2 { get; set; }

        // Determina si debe finalizar la grabacion de la camara 2
        public bool esFinalizaGrabacionC2
        {
            get { return FinalizaGrabacionC2 == "S"; }
            set { FinalizaGrabacionC2 = value ? "S" : "N"; }
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos VideoAccion
    /// </summary>*********************************************************************************************
    public class VideoAccionL : List<VideoAccion>
    {
    }

    #endregion
}

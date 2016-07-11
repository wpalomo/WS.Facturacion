using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace Telectronica.Peaje
{
    #region THREAD: Clase para entidad de manejo de threads 


    /// *********************************************************************************************<summary>
    /// Estructura de una entidad Thread
    /// </summary>*********************************************************************************************

    [Serializable]
    
    public class ThreadProcesoSRI
    {
        public enum enmTipoThread
        {
            ACUSE_RECIBO = 0,
            GENERA_FACTURAS = 1,
            MARCAR_FACTURAS_PROCESADAS = 2
        }


        // Sobrecargamos el constructor para asignarle directamente las propiedades en el momento de crearlo
        public ThreadProcesoSRI(int numeroThread, 
                             Zona zona,
                             Estacion estacion)
        {
            this.NumeroThread = numeroThread;
            this.Zona = zona;
            this.Estacion = estacion;
        }

        
        // Programamos el constructor sin parametros
        public ThreadProcesoSRI()
        { 
        }


        // Tipo de thread (generador de facturas, proceso de acuse de recibos)
        public enmTipoThread tipoThread{ get; set; }


        // Numero de thread que se le asigna a la combinacion Zona y Estacion
        public int NumeroThread { get; set; }


        // Objeto Zona de la combinacion
        public Zona Zona { get; set; }


        // Objeto Estacion de la combinacion
        public Estacion Estacion { get; set; }


        // Ruta de salida de los comprobantes
        public string PathDeSalidaFacturas { get; set; }


        // Dias para borrado de los archivos XML
        public int DiasBorradoXML { get; set; }


        // Cantidad de registros por cada ciclo que se ejecuta
        public int RegistrosAProcesarPorCiclo { get; set; }


        // Nombre del archivo de log, donde concatena zona y thread
        public string CodigoArchivo
        {
            get
            {
                string prefijoArchivo = "";

                switch (tipoThread)
                {
                    case enmTipoThread.ACUSE_RECIBO:
                        prefijoArchivo = "AcuseRecibo";
                        break;

                    case enmTipoThread.GENERA_FACTURAS:
                        prefijoArchivo = "GeneraFactura";
                        break;

                    case enmTipoThread.MARCAR_FACTURAS_PROCESADAS:
                        prefijoArchivo = "MarcarFacturasProcesadas";
                        break;

                    default:
                        break;
                }


                return prefijoArchivo + "_Conce" + Zona.Codigo.ToString() + "_Thr" + NumeroThread.ToString() + "_";

            }
        }


        // Numero de codigo identificatorio de este thread, concatenando zona y thread
        public string Codigo
        {
            get
            {
                string prefijo = "";

                switch (tipoThread)
                {
                    case enmTipoThread.ACUSE_RECIBO:
                        prefijo = "AR";
                        break;

                    case enmTipoThread.GENERA_FACTURAS:
                        prefijo = "GF";
                        break;

                    case enmTipoThread.MARCAR_FACTURAS_PROCESADAS:
                        prefijo = "MF";
                        break;

                    default:
                        break;
                }

                return  prefijo + Zona.Codigo.ToString() + NumeroThread.ToString();
            }
        }

        public ThreadProcesoSRI Clone()
        {

            return (ThreadProcesoSRI) this.MemberwiseClone();
        }

    }


    [Serializable]

    /// *********************************************************************************************<summary>
    /// Lista de objetos Archivo (para manejar el contenido de una carpeta)
    /// </summary>*********************************************************************************************
    public class ThreadProcesoSRIL : List<ThreadProcesoSRI>
    {
    }


    #endregion
}

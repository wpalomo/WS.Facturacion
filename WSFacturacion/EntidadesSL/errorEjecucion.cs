using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Telectronica.EntidadesSL
{
    #region ERROREJECUCION: Clase para entidad que maneja los errores producidos en el servicio y que los muestra el silverlight

    /// *********************************************************************************************<summary>
    /// Estructura de una entidad ERROREJECUCION
    /// </summary>*********************************************************************************************


    //[DataContract]
    public class errorEjecucion
    {
        
        // Enumerado con los codigos de retorno de los SP    
        public enum enmCodigoError
        {
            enmOK = 0,          
            enmDESCONOCIDO = -1,
            enmJORNADACERRADA = -2,
            enmPARTELIQUIDADO = -3,
            enmESTADO_TERMINAL = -4,
            enmUSUARIO_INCORRECTO = -5,
            enmOPERACIONES_PENDIENTES_PROPIAS = -6,
            enmOPERACIONES_PENDIENTES_AJENAS = -7,
            enmFALTA_PARAMETRO = -101,
            enmINCONSISTENCIA = -102,
            enmBAJA_LOGICA = -103
        }

        
        // Constructor vacio
        public errorEjecucion()
        {
            this.CodigoError = enmCodigoError.enmDESCONOCIDO;
            this.Descripcion = "Error desconocido";
        }
        

        // Codigo de error
        public enmCodigoError CodigoError { get; set; }


        // Descripcion del error
        public string Descripcion { get; set; }


        // Booleano que indica si hubo o no error, mas alla del numero de error
        public bool HayError 
        {
            get
            {
                return CodigoError != enmCodigoError.enmOK;
            }
        }


        // Metodo que limpia el error y pone una descripcion por defecto
        public void ClearError()
        {
            CodigoError = enmCodigoError.enmOK;
            Descripcion = "Ejecución Correcta";
        }


    }

    #endregion
}

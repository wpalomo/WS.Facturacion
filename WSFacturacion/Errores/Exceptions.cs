using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Errores
{
    ///****************************************************************************************************
    /// <summary>
    /// Excepciones propias generadas para diferentes casos (usuario no esta logueado, sin permisos en la 
    /// pagina o de ejecucion de Stored Procedures)
    /// </summary>
    ///****************************************************************************************************
    

    /// ------------------------------------------------------------------------------------------ <summary>
    /// Usuario no logueado en el sistema
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class NotLoggedException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public NotLoggedException() { }
        public NotLoggedException(string message) : base(message) { }
        public NotLoggedException(string message, Exception inner) : base(message, inner) { }
        protected NotLoggedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Datos del Usuario logueado estan mal
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class BadLoggedUserException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public BadLoggedUserException() { }
        public BadLoggedUserException(string message) : base(message) { }
        public BadLoggedUserException(string message, Exception inner) : base(message, inner) { }
        protected BadLoggedUserException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }



    /// ------------------------------------------------------------------------------------------ <summary>
    /// Usuarios sin permisos en la pagina
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class SinPermisoException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public SinPermisoException() { }
        public SinPermisoException(string message) : base(message) { }
        public SinPermisoException(string message, Exception inner) : base(message, inner) { }
        protected SinPermisoException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Errores de ejecucion de Stored Procedure
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorSPException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorSPException() { }
        public ErrorSPException(string message) : base(message) { }
        public ErrorSPException(string message, Exception inner) : base(message, inner) { }
        protected ErrorSPException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Errores de FK
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorFKException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorFKException() { }
        public ErrorFKException(string message) : base(message) { }
        public ErrorFKException(string message, Exception inner) : base(message, inner) { }
        protected ErrorFKException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

        /// ***********************************************************************************************
        /// <summary>
        /// Detecta si una excepcion es por FK
        /// generado la excepcion
        /// </summary>
        /// <param name="ex">Exception - Objeto Exception generado al producirse el error</param>
        /// <param name="mensaje">string - Mensaje del error</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void EsErrorFK(Exception ex, string mensaje)
        {
            if (ex.Message.IndexOf("DELETE") > 0
                && ex.Message.IndexOf("REFERENCE") > 0
                && ex.Message.IndexOf("FK_") > 0)
                //Error FK
                throw new ErrorFKException(mensaje, ex);


        }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Warning de FK
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class WarningFKException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WarningFKException() { }
        public WarningFKException(string message) : base(message) { }
        public WarningFKException(string message, Exception inner) : base(message, inner) { }
        protected WarningFKException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    }

    /// ------------------------------------------------------------------------------------------ <summary>
    /// Warning varios
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class WarningException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WarningException() { }
        public WarningException(string message) : base(message) { }
        public WarningException(string message, Exception inner) : base(message, inner) { }
        protected WarningException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }

    }

    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error Jornada está cerrada
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorJornadaCerrada : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorJornadaCerrada() { }
        public ErrorJornadaCerrada(string message) : base(message) { }
        public ErrorJornadaCerrada(string message, Exception inner) : base(message, inner) { }
        protected ErrorJornadaCerrada(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error Parte con Status incompatible
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorParteStatus : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorParteStatus() { }
        public ErrorParteStatus(string message) : base(message) { }
        public ErrorParteStatus(string message, Exception inner) : base(message, inner) { }
        protected ErrorParteStatus(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }


    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error Operacion Facturacion con Status incompatible
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorFacturacionStatus : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorFacturacionStatus() { }
        public ErrorFacturacionStatus(string message) : base(message) { }
        public ErrorFacturacionStatus(string message, Exception inner) : base(message, inner) { }
        protected ErrorFacturacionStatus(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// ------------------------------------------------------------------------------------------ <summary>
    /// Warning Operacion Facturacion con Status incompatible, se puede confirmar
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class WarningFacturacionStatus : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public WarningFacturacionStatus() { }
        public WarningFacturacionStatus(string message) : base(message) { }
        public WarningFacturacionStatus(string message, Exception inner) : base(message, inner) { }
        protected WarningFacturacionStatus(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /*
        /// ------------------------------------------------------------------------------------------ <summary>
        /// Error Estado de terminal incompatible (Abierta cuando queria abrir o cerrada cuando queria cerrar)
        /// </summary>--------------------------------------------------------------------------------
        [global::System.Serializable]
        public class ErrorEstadoTerminalFacturacion: Exception
        {
            //
            // For guidelines regarding the creation of new exception types, see
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
            // and
            //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
            //

            public ErrorEstadoTerminalFacturacion() { }
            public ErrorEstadoTerminalFacturacion(string message) : base(message) { }
            public ErrorEstadoTerminalFacturacion(string message, Exception inner) : base(message, inner) { }
            protected ErrorEstadoTerminalFacturacion(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context)
                : base(info, context) { }
        }
    */

/*
    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error NO corresponde el usuario logueado (la terminal es propia y el usuario logueado no lo es o
    /// la terminal no es propia y el usuario logueado es el mismo)
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorNoCorrespondeUsuarioLogueado : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorNoCorrespondeUsuarioLogueado() { }
        public ErrorNoCorrespondeUsuarioLogueado(string message) : base(message) { }
        public ErrorNoCorrespondeUsuarioLogueado(string message, Exception inner) : base(message, inner) { }
        protected ErrorNoCorrespondeUsuarioLogueado(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
*/

/*
    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error Hay operaciones pendientes de facturar (propias, generadas por el mismo usuario)
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorHayOperacionesPendientesFacturarPropias : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorHayOperacionesPendientesFacturarPropias() { }
        public ErrorHayOperacionesPendientesFacturarPropias(string message) : base(message) { }
        public ErrorHayOperacionesPendientesFacturarPropias(string message, Exception inner) : base(message, inner) { }
        protected ErrorHayOperacionesPendientesFacturarPropias(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
*/

/*
    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error Hay operaciones pendientes de facturar (ajenas, generadas por otro usuario)
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class ErrorHayOperacionesPendientesFacturarAjenas : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public ErrorHayOperacionesPendientesFacturarAjenas() { }
        public ErrorHayOperacionesPendientesFacturarAjenas(string message) : base(message) { }
        public ErrorHayOperacionesPendientesFacturarAjenas(string message, Exception inner) : base(message, inner) { }
        protected ErrorHayOperacionesPendientesFacturarAjenas(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
*/
    
    
    /// ------------------------------------------------------------------------------------------ <summary>
    /// Error ya atrapado en un request de Ajax
    /// Se genera este error para que cuando lo atrapa el Aplication_Error 
    /// no lo volvamos a procesar
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class AjaxException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/cpgenref/html/cpconerrorraisinghandlingguidelines.asp
        // and
        //    http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dncscol/html/csharp07192001.asp
        //

        public AjaxException() { }
        public AjaxException(string message) : base(message) { }
        public AjaxException(string message, Exception inner) : base(message, inner) { }
        protected AjaxException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }

    /// ------------------------------------------------------------------------------------------ <summary>
    /// Objeto de Excepcion que usamos para pasar desde el server al Silverlight.
    /// Con solo los datos que nos interesan
    /// </summary>--------------------------------------------------------------------------------
    [global::System.Serializable]
    public class errorCapaSilver
    {
        public errorCapaSilver()
        {
        }

        public errorCapaSilver(Exception ex)
        {
            this.Message = ex.Message;
            this.Source = ex.Source;
            this.StackTrace = ex.StackTrace;
            
            if (ex is NotLoggedException)
            {
                this.Code = enErrorCapaSilver.ENDSESSIONERROR;
            }

            else if (ex is ErrorJornadaCerrada)
            {
                this.Code = enErrorCapaSilver.JORNADACERRADA;
            }
            
            else if (ex is ErrorParteStatus)
            {
                this.Code = enErrorCapaSilver.PARTELIQUIDADO;
            }

           
            else if (ex is ErrorFKException)
            {
                this.Code = enErrorCapaSilver.ERROR_FK;
            }

            else if (ex is WarningFKException)
            {
                this.Code = enErrorCapaSilver.WARNING_FK;
            }

            else if (ex is WarningFacturacionStatus)
            {
                this.Code = enErrorCapaSilver.WARNINGFACTURACION;
            }

            /*
            else if (ex is ErrorHayOperacionesPendientesFacturarAjenas)
            {
                this.Code = enErrorCapaSilver.WARNINGFACTURACION;
            }
            */
            /*
            else if (ex is ErrorHayOperacionesPendientesFacturarPropias)
            {
                this.Code = enErrorCapaSilver.OPERACIONES_PENDIENTES_PROPIAS;
            }*/

            /*
            else if (ex is  ErrorNoCorrespondeUsuarioLogueado)
            {
                this.Code = enErrorCapaSilver.USUARIO_INCORRECTO;
            }
            */

            else if (ex is ErrorSPException)
            {
                this.Code = enErrorCapaSilver.FALTA_PARAMETRO;
            }

            /*
            else if (ex is ErrorEstadoTerminalFacturacion)
            {
                this.Code = enErrorCapaSilver.ESTADO_TERMINAL;
            }
            */

            else if (ex is ErrorFacturacionStatus)
            {
                this.Code = enErrorCapaSilver.STATUSFACTURACION;
            }

            else
            {
                this.Code = enErrorCapaSilver.THROWEDERROR;
            }

        }
 
        public string Source { get; set; }
        public string Message { get; set; }
        public string StackTrace{ get; set; }
        public enErrorCapaSilver Code { get; set; }
    }   

    public enum enErrorCapaSilver
    {
        // Enumerado para ExceptionSilver.Code
        THROWEDERROR = 0,
        ENDSESSIONERROR = 1,
        BAJA_LOGICA = -103,
        INCONSISTENCIA = -102,
        FALTA_PARAMETRO = -101,
        ERROR_FK = -10,
        WARNING_FK = -9,
        STATUSFACTURACION = -8,
        WARNINGFACTURACION = -7,
//        OPERACIONES_PENDIENTES_PROPIAS = -6,
//        USUARIO_INCORRECTO = -5,
//        ESTADO_TERMINAL = -4,
        PARTELIQUIDADO = -3,
        JORNADACERRADA = -2,
        DESCONOCIDO = -1,
    }
}

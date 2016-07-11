using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using System.Runtime.Serialization.Formatters.Binary;

namespace Telectronica.Utilitarios
{
    /// ***********************************************************************************************
    /// <summary>
	/// Clase de asistencia de utilidades.
	/// </summary>
    /// ***********************************************************************************************
    public sealed class Util
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Helper method, to cast a potentially NULL database value into a Nullable data type
        /// </summary>
        /// <typeparam name="T">.NET structure data type</typeparam>
        /// <param name="dbValue">Value obtained from a database (might be a NULL value)</param>
        /// <returns>Nullable value containing either the value passed in, or null</returns>
        /// ***********************************************************************************************
        public static Nullable<T> DbValueToNullable<T>(object dbValue) where T : struct
        {
            Nullable<T> returnValue = null;

            if ((dbValue != null) && (dbValue != DBNull.Value))
            {
                returnValue = (T)dbValue;
            }
            return returnValue;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el maximo entre 2 enteros
        /// </summary>
        /// ***********************************************************************************************
        public static int Max(int i, int j)
        {
            if (i >= j)
                return i;
            else
                return j;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el maximo entre 2 decimales
        /// </summary>
        /// ***********************************************************************************************
        public static decimal Max(decimal i, decimal j)
        {
            if (i >= j)
                return i;
            else
                return j;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Llena el combo que se pasa por parametro con las opciones "SI" y "NO"
        /// </summary>
        /// ***********************************************************************************************
        public static void LLenaComboSINO(DropDownList ddCombo)
        {
            ddCombo.Items.Add(new ListItem("No", "N"));
            ddCombo.Items.Add(new ListItem("Si", "S"));
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Ubica el el combo en la posicion en que se encuentra el valor buscado. 
        /// En caso de no encontrarse, se puede buscar un valor por defecto.
        /// Si no encuentra ninguno de los dos valores, selecciona el primer item del combo
        /// Si el combo no tiene items no selecciona nada
        /// </summary>
        /// <param name="ddCombo">DropDownList - Combo en el que se desea buscar</param>
        /// <param name="valorBuscado">string - Valor buscado</param>
        /// ***********************************************************************************************
        public static void PosicionarCombo(DropDownList ddCombo, string valorBuscado)
        {
            PosicionarCombo(ddCombo, valorBuscado, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ubica el el combo en la posicion en que se encuentra el valor buscado. 
        /// En caso de no encontrarse, se puede buscar un valor por defecto.
        /// Si no encuentra ninguno de los dos valores, selecciona el primer item del combo
        /// Si el combo no tiene items no selecciona nada
        /// </summary>
        /// <param name="ddCombo">DropDownList - Combo en el que se desea buscar</param>
        /// <param name="valorBuscado">string - Valor buscado</param>
        /// <param name="valorDefault">string - Valor que busca por defecto si no se encuentra el dato previo</param>
        /// ***********************************************************************************************
        public static void PosicionarCombo(DropDownList ddCombo, string valorBuscado, string valorDefault)
        {
            try
            {
                ddCombo.SelectedValue = valorBuscado;
            }
            catch (Exception)
            {
                //Si me da una excepcion es porque no existe el valor
                if (valorDefault != null)
                {
                    try
                    {
                        ddCombo.SelectedValue = valorDefault;
                    }
                    catch (Exception)
                    {
                        //Si me da una excepcion es porque no existe el valor
                        ddCombo.SelectedIndex = 0;
                    }
                }
                else
                {
                    ddCombo.SelectedIndex = 0;
                }
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Ubica el combo en la primer posicion 
        /// (Si tiene algun elemento)
        /// </summary>
        /// <param name="ddCombo">DropDownList - Combo en el que se desea buscar</param>
        /// ***********************************************************************************************
        public static void PosicionarComboPrimero(DropDownList ddCombo)
        {
            if (ddCombo.Items.Count > 0)
            {
                ddCombo.SelectedIndex = 0;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Suma N minutos a una hora en una variable string 
        /// </summary>
        /// <param name="hora">string - Valor de la hora a sumar o restar</param>
        /// <param name="minutos">int - Minutos a sumar (o negativo para restar)</param>
        /// <returns>La hora sumada en formato string</returns>
        /// ***********************************************************************************************
        public static string SumarMinutos(string hora, int minutos)
        {
            DateTime dtHora = new DateTime();

            dtHora = DateTime.Parse(hora);
            dtHora = dtHora.AddMinutes(minutos);

            return dtHora.ToString("HH:mm");
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Junta una fecha y una hora
        /// </summary>
        /// <param name="fecha">DateTime - Valor de la fecha</param>
        /// <param name="hora">DateTime - Valor de la hora</param>
        /// <returns>La fecha y hora, si la hora es vacia la fecha sola</returns>
        /// ***********************************************************************************************
        public static DateTime FechayHora(DateTime fecha, DateTime? hora)
        {
            DateTime fechahora = fecha;
            if (hora != null)
            {
                DateTime hora2 = (DateTime)hora;
                fechahora = fechahora.AddHours(hora2.Hour);
                fechahora = fechahora.AddMinutes(hora2.Minute);
                fechahora = fechahora.AddSeconds(hora2.Second);

            }
            return fechahora;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la parte hora (string) de una fecha 
        /// </summary>
        /// <param name="fecha">DateTime - Valor de la fecha</param>
        /// <returns>La hora de una fecha</returns>
        /// ***********************************************************************************************
        public static string ParteHora(DateTime? fecha)
        {
            DateTime dAux;
            string sRet;

            if (fecha == null)
            {
                sRet = "";
            }
            else
            {
                dAux = (DateTime)fecha;
                sRet = dAux.ToString("HH:mm");
            }
            
            return sRet;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de minutos de una hora 
        /// </summary>
        /// <param name="fecha">DateTime - Valor de la fecha</param>
        /// <returns>La hora de una fecha</returns>
        /// ***********************************************************************************************
        public static int getCantidadMinutos(DateTime fecha)
        {
            DateTime dAux;
            int nRet;

            if (fecha == null)
            {
                nRet = 0;
            }
            else
            {
                dAux = (DateTime)fecha;
                nRet = (int)dAux.TimeOfDay.TotalMinutes;
            }

            return nRet;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que retorna una cantidad de minutos en formato hora 
        /// </summary>
        /// <param name="minutos">int - Cantidad de minutos que se desean transformar al formato "hh:mm"</param>
        /// <returns>El formato "hh:mm" de una cantidad de minutos</returns>
        /// ***********************************************************************************************
        public static string getMinutosAFormatoHora(int minutos)
        {
            string sRet;
            int nHoras;

            nHoras = minutos / 60;
            sRet = nHoras.ToString("00") + ":" + (minutos - (nHoras * 60)).ToString("00");
            
            return sRet;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Helper method, to read the isolation level specified in Web.config.
        /// </summary>
        /// 
        /// <returns>Isolation level specified in Web.config</returns>
        /// ***********************************************************************************************
        /* public static IsolationLevel GetIsolationLevel()
                {
                    IsolationLevel iso;

                    // Get the IsolationLevel element in AppSettings in Web.config
                    string isolationLevelString = ConfigurationManager.AppSettings["IsolationLevel"];

                    // Choose the specified IsolationLevel enumeration value
                    switch (isolationLevelString)
                    {
                        case "ReadUncommitted":
                            iso = IsolationLevel.ReadUncommitted;
                            break;
                        case "ReadCommitted":
                            iso = IsolationLevel.ReadCommitted;
                            break;
                        case "RepeatableRead":
                            iso = IsolationLevel.RepeatableRead;
                            break;
                        case "Serializable":
                            iso = IsolationLevel.Serializable;
                            break;
                        default:
                            iso = IsolationLevel.ReadCommitted;
                            break;
                    }
                    return iso;
                }
         */

        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que retorna la cantidad de archivos en un directorio 
        /// </summary>
        /// <param name="path">sring - Directorio</param>
        /// <param name="extensiones">sring - Extensiones a buscar (separado por |)</param>
        /// <returns>Cantidad de Archivos en el directorio</returns>
        /// ***********************************************************************************************
        public static int getCantidadArchivos(string path, string extensiones)
        {
            int cant = 0;
            if (path != "")
            {
                string[] exts = extensiones.Split('|');
                DirectoryInfo dir = new DirectoryInfo(path);

                foreach(var item in dir.EnumerateFiles())
                //string[] files = Directory.GetFiles(path);
                //foreach (string file in files)
                {
                    string file = item.Extension;
                    foreach (string ext in exts)
                    {
                        if (file.ToLower().EndsWith(ext))
                        {
                            cant++;
                        }
                    }
                }
            }
            return cant;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Funcion para clonar objetos mediante la serializacion.
        /// </summary>
        /// <param name="obj">Objeto a clonar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static Object Clonar(object obj)
        {
            MemoryStream ms = new MemoryStream();
            Object objResult = null;
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, obj);
                ms.Position = 0;
                objResult = bf.Deserialize(ms);
            }
            finally
            {
                ms.Close();
            }
            return objResult;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una cadena formateada con los ceros adelante
        /// </summary>
        /// <param name="sValue"></param>
        /// <param name="iLong"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string ObtenerNumeroFormateadoConCerosAdelante(string sValue, int iLong)
        {
            var sCero = "0000000000000000";
            var retValue = sCero + sValue;
            return (retValue.Remove(0, retValue.Length - iLong));
        }

        /// <summary>
        /// Obtiene el timeOut de los reportes
        /// </summary>
        /// <returns></returns>
        public static int getSpTimeOut()
        {
            try
            {
                return Convert.ToInt32(WebConfigurationManager.AppSettings["ReportTimeOut"]);
            }
            catch (Exception)
            {
                return 30;
                
            }
            
        }
    }
}

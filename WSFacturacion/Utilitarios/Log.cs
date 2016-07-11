using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;



namespace Telectronica.Utilitarios
{
    public static class Log
    {
        public static void Loguear(string mensaje, string codigoArchivo)
        {
            if( !Directory.Exists("Logs") )
                Directory.CreateDirectory("Logs");
            //Mombre de archivo Logcodigoyyyymmdd.log
            StreamWriter sw = new StreamWriter("Logs\\Log" + codigoArchivo + DateTime.Now.ToString("yyyyMMdd") + ".log", true);

            //Grabar primer HH:MM:SS.sss
            sw.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff")+" "+ mensaje.Trim());
            sw.Close();

        }
        public static string getDescripcionError(Exception exError)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(Traduccion.Traducir("Se produjo un error en "));
            sb.Append("\n\n");

            Exception ex = exError;
            while (ex != null)
            {
                sb.Append(Traduccion.Traducir("Error: "));
                sb.Append(ex.Message);
                sb.Append(Traduccion.Traducir(" (Origen: "));
                sb.Append(ex.Source);
                sb.Append(")\n");

                ex = ex.InnerException;
            }
            return sb.ToString();
        }
    }
}

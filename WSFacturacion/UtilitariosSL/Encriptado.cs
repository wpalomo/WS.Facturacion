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
using System.Text;

namespace Telectronica.Utilitarios.SL
{
    public class Encriptado
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Función CryptIn de VB.
        /// </summary>
        /// <param name="sCadena">string - dato a encriptar
        /// <returns>Encriptado de sCadena</returns>
        /// ***********************************************************************************************
        public static string CryptIn(string sCadena)
        {
            string sRet = string.Empty;

            //Convertimos a un array de bytes ASCII
            System.Text.Encoding encoding = Encoding.UTF8;
            byte[] bArray = encoding.GetBytes(sCadena);

            foreach (byte bChar in bArray)
            {
                sRet += (bChar ^ 255) + 255;
            }
            //                strCrypt = strCrypt & (Asc(Mid$(Cadena, i, 1)) Xor 255) + 255
            return sRet;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Función CryptOut de VB.
        /// </summary>
        /// <param name="sCadena">string - dato a Desencriptar
        /// <returns>sCadena desencriptada</returns>
        /// ***********************************************************************************************
        public static  string CryptOut(String sCadena)
        {
            string sRet = string.Empty;

            for (int i = 0; i < sCadena.Length; i+=3)
			{
			    sRet += Convert.ToChar((Convert.ToInt32(sCadena.Substring(i, 3)) - 255) ^ 255);
			}
            // strDeCrypt = strDeCrypt & chr((val(Mid$(Cadena, i, 3)) - 255) Xor 255)

            return sRet;
        }

    }
}

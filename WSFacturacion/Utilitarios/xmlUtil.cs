using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
namespace Telectronica.Utilitarios
{
    public class xmlUtil
    {
        /// <summary>

        /// Method to convert a custom Object to XML string

        /// </summary>

        /// <param name="pObject">Object that is to be serialized to XML</param>

        /// <returns>XML string</returns>

        public static String SerializeObject(Object pObject)
        {

            try
            {

                String XmlizedString = null;

                MemoryStream memoryStream = new MemoryStream();

                XmlSerializer xs = new XmlSerializer(pObject.GetType());

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.Unicode);

                xs.Serialize(xmlTextWriter, pObject);

                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;

                XmlizedString = UnicodeByteArrayToString(memoryStream.ToArray());

                return XmlizedString;

            }

            catch (Exception e)
            {

                throw e;
                //System.Console.WriteLine(e);

                //return null;

            }

        } 


            /// <summary>

        /// To convert a Byte Array of Unicode values (Unicode encoded) to a complete String.

        /// </summary>

        /// <param name="characters">Unicode Byte Array to be converted to String</param>

        /// <returns>String converted from Unicode Byte Array</returns>

        private static String UnicodeByteArrayToString ( Byte[ ] characters )

        {


            UnicodeEncoding encoding = new UnicodeEncoding();

            String constructedString = encoding.GetString ( characters );

            return ( constructedString );

        }

     

        /// <summary>

        /// Converts the String to UTF8 Byte array and is used in De serialization

        /// </summary>

        /// <param name="pXmlString"></param>

        /// <returns></returns>

        private Byte[ ] StringToUTF8ByteArray ( String pXmlString )

        {

            UTF8Encoding encoding = new UTF8Encoding ( );

            Byte[ ] byteArray = encoding.GetBytes ( pXmlString );

            return byteArray;

        } 

    }
}

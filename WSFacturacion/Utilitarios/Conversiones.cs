using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Telectronica.Utilitarios
{
    public class Conversiones
    {

        #region String: Tratamientos con tipos String

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Recibe un DataReader mas un campo que encontrar y devuelve un valor.
        // AUTOR ...........: MAC
        // FECHA CREACIÓN ..: 08/2015
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...:
        // ----------------------------------------------------------------------------------------------
        public static string tryString(IDataReader oDr,string campo)
        {
            string value="";
            
            try
            {
                
                value =  Convert.ToString(oDr[campo]);
            }
            catch (Exception ex) 
            {

            }

            return value;
        }

        public static string edt_Str(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna "", sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return "";
            }
            else
            {
                return xoValor.ToString().Trim();
            }
        }

        #endregion

        #region DateTime: Tratamientos con tipos DateTime

        public static DateTime edt_DateTime(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna un datetime nulo, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------
            DateTime? ldAux = null;

            if (xoValor == DBNull.Value)
            {
                return (DateTime)ldAux;
            }
            else
            {
                return (DateTime)xoValor;
            }
        }

        #endregion

        #region Numericos: Tratamientos con tipos Numericos

        public static Int16 edt_Int16(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna 0, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt16(xoValor);
            }
        }

        public static Int32 edt_Int32(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna 0, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 11/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(xoValor);
            }
        }

        public static int edt_Int(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna 0, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 07/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(xoValor);
            }
        }

        public static byte edt_Byte(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna 0, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 11/08/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToByte(xoValor);
            }
        }

        public static decimal edt_Decimal(object xoValor)
        {
            // ----------------------------------------------------------------------------------------------
            // FUNCIONALIDAD ...: Analiza el objeto que llega, si es nulo retorna 0, sino retorna el valor
            // AUTOR ...........: Cristian Binaghi
            // FECHA CREACIÓN ..: 28/09/2009
            // ULT.FEC.MODIF. ..:
            // OBSERVACIONES ...:
            // ----------------------------------------------------------------------------------------------

            if (xoValor == DBNull.Value)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(xoValor);
            }
        }
        #endregion
        #region Dinero: Tratamientos con tipos de dinero
        /// ***********************************************************************************************
        /// <summary>
        /// Convierte un monto a un string con 2 decimales y separador .
        /// </summary>
        /// <param name="monto">decimal monto a convertir</param>
        /// <returns>importe en string con separador .</returns>
        /// ***********************************************************************************************
        public static string MoneyToString(decimal monto)
        {
            string ret = monto.ToString("F2");
            return ret.Replace(",", ".");
        }
        #endregion

        
    }
}

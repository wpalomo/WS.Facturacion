using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje.Tesoreria
{
    public class EstacionConfiguracionDT
    {
        #region DisplayDT: Clase de Datos de Moneda.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Monedas definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Codigo">String - Codigo de Mensaje Display a filtrar</param>
        /// <param name="Sentido">String - Sentido a filtrar</param>
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        public static DisplayL getMensajesDisplay(Conexion oConn, string Codigo, string Sentido)
        {
            DisplayL oDisplay = new DisplayL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajeDisplay_getMensajeDisplay";

                oCmd.Parameters.Add("@dis_senti", SqlDbType.TinyInt).Value = Codigo;
                oCmd.Parameters.Add("@dis_codig", SqlDbType.TinyInt).Value = Sentido;

                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                {
                    oDisplay.Add (CargarDisplay(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDisplay;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes de Display
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Displa</param>
        /// <returns>Lista con el elemento Display de la base de datos</returns>
        /// ***********************************************************************************************
        private static Display CargarDisplay(System.Data.IDataReader oDR)
        {
            Display oDisplay = new Display(oDR["dis_senti"].ToString(),
                                        oDR["dis_codig"].ToString(),
                                        oDR["dis_texto"].ToString(),
                                        oDR["dis_activ"].ToString(),
                                        oDR["dis_titil"].ToString(),
                                        oDR["dis_ancha"].ToString(),
                                        oDR["dis_efect"].ToString(),
                                        (Int16)oDR["dis_veloc"]);

            return oDisplay;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un mensaje de Display en la base de datos
        /// </summary>
        /// <param name="oDisplay">Display - Objeto con la informacion del mensaje de Display a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMensajeDisplay(Display oDisplay, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_MensajeDisplay_updMensajeDisplay";

                oCmd.Parameters.Add("@dis_senti", SqlDbType.Char,1).Value = oDisplay.Sentido;
                oCmd.Parameters.Add("@dis_codig", SqlDbType.Char, 3).Value = oDisplay.Codigo;
                oCmd.Parameters.Add("@dis_texto", SqlDbType.VarChar, 100).Value = oDisplay.Texto;
                oCmd.Parameters.Add("@dis_activ", SqlDbType.Char, 1).Value = oDisplay.Activo;
                oCmd.Parameters.Add("@dis_efect", SqlDbType.Char, 1).Value = oDisplay.Efectos;
                oCmd.Parameters.Add("@dis_titil", SqlDbType.Char, 1).Value = oDisplay.Titilante;
                oCmd.Parameters.Add("@dis_veloc", SqlDbType.SmallInt).Value = oDisplay.Velocidad;
                oCmd.Parameters.Add("@dis_ancha", SqlDbType.Char, 1).Value = oDisplay.LetraAncha;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Mensaje de Display");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

        #region VELOCIDAD: Clase de Datos de VELOCIDAD.


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto Velocidad. 
        /// </summary>
        /// <param name="codigo">int - Codigo de la velocidad que deseo devolver como texto</param>
        /// <returns>Objeto de Velocidad</returns>
        /// ***********************************************************************************************
        public static Velocidad getVelocidad(int codigo)
        {
            Velocidad oVelocidad = new Velocidad();
            oVelocidad.Codigo = codigo;
            oVelocidad.Descripcion = RetornaVelocidad(codigo);

            return oVelocidad;

        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido de la Velocidad
        /// </summary>
        /// <param name="codigo">int - Codigo de la Velocidad que deseo devolver como texto</param>
        /// <returns>El texto traducido de la Velocidad</returns>
        /// ***********************************************************************************************
        protected static string RetornaVelocidad(int codigo)
        {
            string retorno = string.Empty;
            int caseSwitch = codigo;

            switch (caseSwitch)
            {
                case 1:
                    retorno = "Lenta";
                    break;
                case 2:
                    retorno = "Media";
                    break;
                case 3:
                    retorno = "Rápida";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }


        #endregion
    }
}

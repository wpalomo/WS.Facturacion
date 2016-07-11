using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class FormatoTicketDt
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a los Tipos de Tickets
        /// </summary>
        /// <returns>Objeto ViaSentidoCirculacion con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        public static TipoTicket CargaTipoTicket(IDataReader dataReader)
        {
            return new TipoTicket
            {
                Codigo = Convert.ToInt16(dataReader["tit_codig"]),
                Descripcion = Convert.ToString(dataReader["tit_descr"]),
            };
        }

        public static TipoTicketL getTiposDeTicket(Conexion oConn)
        {
            TipoTicketL oTiposTicket = new TipoTicketL();
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_FormatoTicket_GetTipoTicket";

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTiposTicket.Add(CargaTipoTicket(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTiposTicket;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a los Tipos de Tickets
        /// </summary>
        /// <returns>Objeto ViaSentidoCirculacion con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        public static FormatoTicket CargaFormatoTicket(IDataReader dataReader)
        {
            
            FormatoTicket oFormato = new FormatoTicket();
            
              oFormato.TipoTicket = new TipoTicket ();
              oFormato.TipoTicket.Descripcion = Convert.ToString(dataReader["tit_descr"]);
              oFormato.TipoTicket.Codigo = Convert.ToInt16(dataReader["tit_codig"]);             
              oFormato.CuerpoTicket = Convert.ToString(dataReader["frm_texto"]);
              oFormato.NumeroCopias = dataReader["frm_copia"] == DBNull.Value ? 0 : Convert.ToInt16(dataReader["frm_copia"]);

              return oFormato;
                       
        }
        public static FormatoTicketL getFormatosTicket(Conexion oConn, int? codFrmTicket)
        {
            FormatoTicketL oFormatoTicket = new FormatoTicketL();
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_FormatoTicket_GetFormatoTicket";
            oCmd.Parameters.Add("@codFrmTicket", SqlDbType.TinyInt).Value = codFrmTicket;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oFormatoTicket.Add(CargaFormatoTicket(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oFormatoTicket;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza el formato de Tickets
        /// </summary>
        /// <param name="oFormatoTicket"></param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updFormatoTicket(FormatoTicket oFormatoTicket, Conexion oConn)
        {
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_FormatoTicket_updFormatoTicket";
            oCmd.Parameters.Add("@TipoTicket", SqlDbType.TinyInt).Value = oFormatoTicket.TipoTicket.Codigo;
            oCmd.Parameters.Add("@CuerpoTicket", SqlDbType.VarChar, 8000).Value = oFormatoTicket.CuerpoTicket;
            oCmd.Parameters.Add("@Copias", SqlDbType.TinyInt).Value = oFormatoTicket.NumeroCopias;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Modificar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("No existe el registro de Formato de Ticket");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static DataSet getRptFormatoTicket(Conexion oConn)
        {
            DataSet rptFormatoTicket = new DataSet();
            rptFormatoTicket.DataSetName = "RptPeaje_FormatoTicket";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Tarifas_getRptFormatoTicket";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(rptFormatoTicket, "Formato de Tickets");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return rptFormatoTicket;
        }

    }

}

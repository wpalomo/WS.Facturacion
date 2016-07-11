using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.IO;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Peaje
    /// </summary>
    /// ***********************************************************************************************
    public class RptPeajeDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los bloques sin liquidar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// ***********************************************************************************************
        public static DataSet getBloquesSinLiquidar(Conexion oConn, int? zona, int? estacion, DateTime fechahoraDesde, DateTime fechahoraHasta)
        {
            DataSet dsBloques = new DataSet();
            dsBloques.DataSetName = "RptPeaje_BloquesAbiertosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getBloquesSinLiquidar";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsBloques, "Bloques");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los bloques abiertos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="modoIngreso">string - Modo de Ingreso</param>
        /// <param name="operador">string - Operador</param>
        /// <param name="sModoParte">string - Modo del parte</param>
        /// ***********************************************************************************************
        public static DataSet getViasAbiertas(Conexion oConn, int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta, string modoIngreso, int? causaCierre, string operador, string sModoParte, string sSenti)
        {
            DataSet dsBloques = new DataSet();
            dsBloques.DataSetName = "RptPeaje_ViasAbiertasDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getViasAbiertas";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@modoi", SqlDbType.Char, 1).Value = modoIngreso;
            oCmd.Parameters.Add("@codci", SqlDbType.TinyInt).Value = causaCierre;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar, 10).Value = operador;
            oCmd.Parameters.Add("@modoParte", SqlDbType.Char, 1).Value = sModoParte;
            oCmd.Parameters.Add("@senti", SqlDbType.Char, 1).Value = sSenti;
            
            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsBloques, "Bloques");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los quiebres de barrera
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="modoQuiebre">string - Modo de Quiebre</param>
        /// <param name="operador">string - Operador</param>
        /// ***********************************************************************************************
        public static DataSet getQuiebresBarrera(Conexion oConn, int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta, string modoQuiebre, string operador)
        {
            DataSet dsBloques = new DataSet();
            dsBloques.DataSetName = "RptPeaje_QuiebresBarreraDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getQuiebresBarrera";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@quieb", SqlDbType.Char, 1).Value = modoQuiebre;
            oCmd.Parameters.Add("@opeid", SqlDbType.VarChar, 10).Value = operador;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsBloques, "Bloques");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsBloques;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los cambios de estado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="via">int? - Numero de Via</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// ***********************************************************************************************
        public static DataSet getCambiosEstado(Conexion oConn, int? zona, int? estacion, int? via, DateTime fechahoraDesde, DateTime fechahoraHasta)
        {
            DataSet dsCambios = new DataSet();
            dsCambios.DataSetName = "RptPeaje_CambiosEstadoDS";
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getCambiosEstado";
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = via;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsCambios, "Cambios");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsCambios;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con los eventos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="oVias">ViaL - Numeros de Via</param>
        /// <param name="oClaveEventos">ClaveEventoL - Codigos de Evento</param>
        /// <param name="patente">string - Patente</param>
        /// <param name="usuario">string - Usuario logueado (para permisos)</param>
        /// ***********************************************************************************************
        public static DataSet getEventos(Conexion oConn, int? estacion, DateTime fechahoraDesde, DateTime fechahoraHasta, ViaL oVias, ClaveEventoL oClaveEventos, string patente, string usuario)
        {
            //Pasamos a xml la lista de objetos.
            string xmlVias = "";
            if (oVias == null) xmlVias = null; else xmlVias = Utilitarios.xmlUtil.SerializeObject(oVias); 
            string xmlClaveEventos = "";
            xmlClaveEventos = Utilitarios.xmlUtil.SerializeObject(oClaveEventos);

            DataSet dsCambios = new DataSet();
            dsCambios.DataSetName = "RptPeaje_EventosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_GetEventos";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = fechahoraDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fechahoraHasta;
            oCmd.Parameters.Add("@xmlVias", SqlDbType.NText).Value = xmlVias;
            oCmd.Parameters.Add("@xmlCodev", SqlDbType.NText).Value = xmlClaveEventos;
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 10).Value = patente;
            oCmd.Parameters.Add("@usuario", SqlDbType.VarChar, 10).Value = usuario;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsCambios, "Eventos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsCambios;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con las fechas minimas y maximas de transitos y bloques
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static DataSet getFechasMinMax(Conexion oConn)
        {
            DataSet dsFechas = new DataSet();
            dsFechas.DataSetName = "RptPeaje_FechasMinMaxDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getFechasMinMax";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsFechas, "Fechas");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsFechas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de eventos de video por dia y plaza
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="fecha">DateTime - Fecha Hasta</param>
        /// <param name="dias">int - Dias para atras</param>
        /// ***********************************************************************************************
        public static VideoCantidadesL getCantidadVideos(Conexion oConn, DateTime fecha, int dias)
        {
            VideoCantidadesL oVideos = new VideoCantidadesL();
            SqlDataReader oDR;
            byte estacion = 0;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getCantidadVideos";
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = fecha;
            oCmd.Parameters.Add("@dias", SqlDbType.Int).Value = dias;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                if (estacion != (byte)oDR["est_codig"])
                {
                    oVideos.Add(CargarVideoCantidades(oDR, dias, fecha));
                }
                estacion = (byte)oDR["est_codig"];
                CargarVideoCantidad(oDR, oVideos.Last().Videos, fecha.AddDays(-dias));
            }
                
            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oVideos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la fecha maxima de los maestros
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static DateTime getFechaMaestros(Conexion oConn)
        {
            DateTime fechaMax = new DateTime(1900, 1, 1);

            SqlDataReader oDR;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptPeaje_getFechaMaestros";


            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                if (oDR["FechaMax"] != DBNull.Value)
                {
                    fechaMax = (DateTime)oDR["FechaMax"];
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return fechaMax;
        }
        
        private static VideoCantidades CargarVideoCantidades(System.Data.IDataReader oDR, int dias, DateTime fecha)
        {
            VideoCantidades oVideo = new VideoCantidades();
            oVideo.Estacion = new Estacion((byte)oDR["est_codig"], oDR["est_nombr"].ToString());
            oVideo.Fechas = new List<DateTime>();
            oVideo.Videos = new VideoFechaL();
            for (int i = dias; i >= 0; i--)
            {
                oVideo.Fechas.Add(fecha.AddDays(-i));
                oVideo.Videos.Add(new VideoFecha());
            }
            return oVideo;
        }

        private static void CargarVideoCantidad(System.Data.IDataReader oDR, VideoFechaL oVideos, DateTime fechaBase)
        {
            DateTime fecha = (DateTime)oDR["fecde"];
            TimeSpan ts = fecha - fechaBase;
            int i = (int)ts.TotalDays;

            if (i >= 0 && i < oVideos.Count)
            {
                oVideos[i].CantidadEventos = (int)oDR["CantVideos"];
                oVideos[i].CantidadArchivos = (int)oDR["CantCopiados"];
                //oVideos[i].Path = oDR["path_video"].ToString();
            }
        }

        public static DataSet getValesPrepagos(Conexion oConn, DateTime Desde, DateTime Hasta, int? Cliente, int? Zona, int? Estacion)
        {
            DataSet dsValesPrepagos = new DataSet();
            dsValesPrepagos.DataSetName = "RptPeaje_ValesPrepagosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_RptValesPrepagos_getValesPrepagos";
            oCmd.Parameters.Add("@desde", SqlDbType.DateTime).Value = Desde;
            oCmd.Parameters.Add("@hasta", SqlDbType.DateTime).Value = Hasta;
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = Zona;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Estacion;
            oCmd.Parameters.Add("@numcl", SqlDbType.TinyInt).Value = Cliente;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsValesPrepagos, "ValesPrepagos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();

            return dsValesPrepagos;
        }

        public static DataSet getTransitosValesPrepagos(Conexion oConn, DateTime Desde, DateTime Hasta, int? Cliente, int? Zona, int? Estacion, int? SerieDesde, int? SerieHasta, int? Categoria)
        {

            DataSet dsTransitosValesPrepagos = new DataSet();
            dsTransitosValesPrepagos.DataSetName = "RptPeaje_TransValesPrepDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Peaje_rptTransitosValesPrepagos";
                oCmd.Parameters.Add("@Fecdesde", SqlDbType.DateTime).Value = Desde;
                oCmd.Parameters.Add("@FecHasta", SqlDbType.DateTime).Value = Hasta;
                oCmd.Parameters.Add("@Zona", SqlDbType.TinyInt).Value = Zona;
                oCmd.Parameters.Add("@Estaci", SqlDbType.TinyInt).Value = Estacion;
                oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = Cliente;
                oCmd.Parameters.Add("@SerDesde", SqlDbType.Int).Value = SerieDesde;
                oCmd.Parameters.Add("@SerHasta", SqlDbType.Int).Value = SerieHasta;
                oCmd.Parameters.Add("@Categoria", SqlDbType.Int).Value = Categoria;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitosValesPrepagos, "TransitosValesPrepagos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dsTransitosValesPrepagos;
        }

        public static DataSet getLogosCabeceras(string pathImagenes)
        {
            DataSet dsLogosCabeceras = new DataSet();
            dsLogosCabeceras.DataSetName = "Rpt_LogosCabecerasDS";
            dsLogosCabeceras.Tables.Add("LogosCabeceras");
         
            // Creamos las columnas para almacenar los datos de las imagenes leidas:
            dsLogosCabeceras.Tables["LogosCabeceras"].Columns.Add("imglogo1", typeof(System.Byte[]));
            dsLogosCabeceras.Tables["LogosCabeceras"].Columns.Add("imglogo2", typeof(System.Byte[]));
                
            // Creamos la nueva fila para almacenar los datos de las imagenes:
            DataRow newLogosRow = dsLogosCabeceras.Tables["LogosCabeceras"].NewRow();

            // Ruta de la primer imagen
            string pathImage = string.Concat(pathImagenes, "logo1.jpg");

            // Abrimos la primer imagen, si existe...
            if (File.Exists(pathImage))
            {
                FileStream fs1 = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                BinaryReader br1 = new BinaryReader(fs1);

                // Cargamos los datos de la imagen en el array
                byte[] arr = new byte[fs1.Length];
                br1.Read(arr, 0, (int)fs1.Length);
                br1.Close();
                fs1.Close();

                // Guardamos los datos leidos en el campo correspondiente....
                newLogosRow["imglogo1"] = arr;
            }
            else
            {
                newLogosRow["imglogo1"] = null;
            }

            // Ruta de la segunda imagen
            pathImage = string.Concat(pathImagenes, "logo2.jpg");

            // Abrimos la segunda imagen, si existe...
            if (File.Exists(pathImage))
            {
                FileStream fs2 = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                BinaryReader br2 = new BinaryReader(fs2);

                // Cargamos los datos de la imagen en el array
                byte[] arr = new byte[fs2.Length];
                br2.Read(arr, 0, (int)fs2.Length);
                br2.Close();
                fs2.Close();

                // Guardamos los datos leidos en el campo correspondiente....
                newLogosRow["imglogo2"] = arr;
            }
            else
            {
                newLogosRow["imglogo2"] = null;
            }

            // Cargamos el DataRow creado al DataTable:
            dsLogosCabeceras.Tables["LogosCabeceras"].Rows.Add(newLogosRow);

            return dsLogosCabeceras;
        }
    }
}

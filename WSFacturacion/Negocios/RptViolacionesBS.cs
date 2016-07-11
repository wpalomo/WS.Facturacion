using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using System.IO;

namespace Telectronica.Peaje
{
    /// ***********************************************************************************************
    /// <summary>
    /// Clase que trae los datos para Reportes de Detalle de Exonerados
    /// </summary>
    /// ***********************************************************************************************
    public class RptViolacionesBS
    {

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un DataSet con el Detalle de Exonerados
        /// </summary>
        /// <param name="fechahoraDesde">DateTime - Fecha y Hora Desde</param>
        /// <param name="fechahoraHasta">DateTime - Fecha y Hora Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// ***********************************************************************************************
        public static DataSet getDetalleViolaciones(DateTime fechahoraDesde, DateTime fechahoraHasta, int? zona, int? estacion,string Placa)
        {
            DataSet dsViolaciones = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    dsViolaciones = RptViolacionesDt.getDetalleViolaciones(conn, fechahoraDesde, fechahoraHasta, zona,  estacion, Placa);
                    getFotosViolaciones(dsViolaciones);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsViolaciones;
        }

        // PRUEBA -- AGRAGAR FOTOS
        public static void getFotosViolaciones(DataSet dsViolaciones)
        {
            try
            {
                // Creamos las columnas para almacenar los datos de las imagenes leidas:
                dsViolaciones.Tables["Violaciones"].Columns.Add("foto", typeof(System.Byte[]));

                //List<string> pathFoto = new List<string>();
                string pathFoto;

                //Recorro DataSet para obtener los Path
                foreach (DataRow data in dsViolaciones.Tables["Violaciones"].Rows)
                {
                    pathFoto = data["PathFoto"].ToString();
                    if (File.Exists(pathFoto))
                    {
                        FileStream fs1 = new FileStream(pathFoto, FileMode.Open, FileAccess.Read);
                        BinaryReader br1 = new BinaryReader(fs1);

                        // Cargamos los datos de la imagen en el array
                        byte[] arr = new byte[fs1.Length];
                        br1.Read(arr, 0, (int)fs1.Length);
                        br1.Close();
                        fs1.Close();

                        // Guardamos los datos leidos en el campo correspondiente....
                        data["foto"] = arr;
                    }
                    else
                    {
                        data["foto"] = null;
                    }
                }

                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        ///*********************************************************************************************************
        /// <summary>
        /// Arma el dataset con el total de transitos filtrando solo las violaciones, Agrupados por jornada
        /// </summary>
        /// <param name="fechahoraDesde"></param>
        /// <param name="fechahoraHasta"></param>
        /// <param name="zona"></param>
        /// <param name="estacion"></param>
        /// <param name="Placa"></param>
        /// <returns></returns>
        ///*********************************************************************************************************
        public static DataSet getEstadisticoViolaciones(DateTime fechahoraDesde, DateTime fechahoraHasta,
        int? zona, int? estacion, int? nuvia,string agrupacion, int? fragmentacion, int? horacorte)
        {
            DataSet dsViolaciones = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    dsViolaciones = RptViolacionesDt.getEstadisticoViolaciones(conn,fechahoraDesde,fechahoraHasta,zona,estacion,nuvia,agrupacion,fragmentacion,horacorte);
       
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsViolaciones;
        }

    }
}

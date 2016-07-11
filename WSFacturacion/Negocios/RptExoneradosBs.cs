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
    public class RptExoneradosBs
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
        public static DataSet getDetalleExonerados(DateTime fechahoraDesde, DateTime fechahoraHasta, int? zona, int? estacion, string operador)
        {
            DataSet dsExonerados = null;
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion                    
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);

                    dsExonerados = RptExoneradosDt.getDetalleExonerados(conn, fechahoraDesde, fechahoraHasta, zona, estacion, operador);
                    getFotosExonerados(dsExonerados);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsExonerados;
        }

        // PRUEBA -- AGRAGAR FOTOS
        public static void getFotosExonerados(DataSet dsExonerados)
        {
            try
            {
                // Creamos las columnas para almacenar los datos de las imagenes leidas:
                dsExonerados.Tables["Exonerados"].Columns.Add("foto", typeof(System.Byte[]));

                //List<string> pathFoto = new List<string>();
                string pathFoto;

                //Recorro DataSet para obtener los Path
                foreach (DataRow data in dsExonerados.Tables["Exonerados"].Rows)
                {
                    pathFoto = data["tra_video2"].ToString();
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
    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class StatusTratamientoDt
    {
        public static StatusTratamientoL getTransitosPexTotalesPorEstado(Conexion oConn, DateTime desde, DateTime hasta, int? administradora, int? estacion)
        {
            StatusTratamientoL listaTratamientos = new StatusTratamientoL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_OSAs_getTotalesPorEstado";
                oCmd.Parameters.Add("@fechaDesde", SqlDbType.DateTime).Value = desde;
                oCmd.Parameters.Add("@fechaHasta", SqlDbType.DateTime).Value = hasta;
                oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = estacion;
                oCmd.Parameters.Add("@admtag", SqlDbType.Int).Value = administradora;

                oCmd.CommandTimeout = 3000;

                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                {
                    listaTratamientos.Add(cargarStatusTratamiento(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return listaTratamientos;
        }


        public static StatusTratamiento cargarStatusTratamiento(System.Data.IDataReader oDR)
        {
            try
            {
                StatusTratamiento tratamiento = new StatusTratamiento();

                if (oDR["trn_status"] != DBNull.Value)
                {
                    tratamiento.CodEstado = oDR["trn_status"].ToString();
                }
                if (oDR["Status"] != DBNull.Value)
                {
                    tratamiento.Estado =  oDR["Status"].ToString();
                }                                
                if (oDR["Cantidad"] != DBNull.Value)
                {
                    tratamiento.Cantidad = Convert.ToInt32(oDR["Cantidad"]);
                }

                if (oDR["trf_codre"] != DBNull.Value)
                {
                    tratamiento.CodigoRechazo = oDR["trf_codre"].ToString();
                }

                if (oDR["Monto"] != DBNull.Value)
                {
                    tratamiento.Monto = Convert.ToDouble(oDR["Monto"]);
                }

                if (oDR["mot_descr"] != DBNull.Value)
                {
                    tratamiento.Causa = oDR["trf_codre"].ToString() + " - " + oDR["mot_descr"].ToString();
                }

                if (oDR["adt_codig"] != DBNull.Value)
                {
                    tratamiento.Administradora = new OSAsTag();
                    tratamiento.Administradora.Administradora = Convert.ToInt32(oDR["adt_codig"]);

                    if (oDR["adt_descr"] != DBNull.Value)
                    {
                        tratamiento.Administradora.Descripcion = oDR["adt_descr"].ToString();
                    }
                }

                return tratamiento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

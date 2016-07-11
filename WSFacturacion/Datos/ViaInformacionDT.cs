using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Microsoft;
using System.Globalization;
using System.IO;

namespace Telectronica.Peaje
{
    public class ViaInformacionDt
    {
        #region ViaInformacion: Clase de Datos de ViaInformacion.

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de objetos ViaInformacion
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 28/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oConn - Conexion - objeto de conexion a la base de datos correspondiente
        //                                  xiCodEst - int - Numero de estacion a filtrar
        //                                  xiVia - int - Número de vía a filtrar
        //                    Retorna: Lista de ViaInformacion: ViaInformacionL
        // ----------------------------------------------------------------------------------------------
        public static ViaInformacionL getViaInformacion(Conexion oConn, int iCodEst, int? iVia)
        {
            ViaInformacionL oViaInformacion = new ViaInformacionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getUltimoBloque";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iCodEst;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = iVia;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oViaInformacion.Add(CargarViaInformacion(oDR, oConn, iCodEst, iVia));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            //return oTiposDiaHora;
            return oViaInformacion;
        }
        
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Carga un elemento DataReader en la lista de ViaInformacion
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 28/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oDR - System.Data.IDataReader - Objeto DataReader de la tabla
        //                    Retorna: Lista de ViaInformacion: ViaInformacionL
        // ----------------------------------------------------------------------------------------------
        private static ViaInformacion CargarViaInformacion(System.Data.IDataReader oDR, Conexion oConn, int iCodEst, int? iVia)
        {
            ViaInformacion oViaInformacion = new ViaInformacion();

            oViaInformacion.CodEstacion = Conversiones.edt_Int16(oDR["via_coest"]);
            oViaInformacion.NumeroVia = Conversiones.edt_Int16(oDR["via_nuvia"]);
            oViaInformacion.NombreVia = Conversiones.edt_Str(oDR["via_nombr"]);
            oViaInformacion.OperadorId = Conversiones.edt_Str(oDR["via_id"]);
            oViaInformacion.OperadorNombre = Conversiones.edt_Str(oDR["use_nombr"]);
            oViaInformacion.Estado = Conversiones.edt_Str(oDR["via_estad"]);
            oViaInformacion.FechaApertura = Util.DbValueToNullable<DateTime>(oDR["via_fecap"]);
            //oViaInformacion.FechaApertura = (DateTime)oDR["via_fecap"];
            //oViaInformacion.FechaCierre = Conversiones.edt_DateTime(oDR["via_fecci"]);
            oViaInformacion.FechaCierre = Util.DbValueToNullable<DateTime>(oDR["via_fecci"]);
            //oUsuario.FechaEgreso = Util.DbValueToNullable<DateTime>(oDR["use_feegr"]);
            //oViaInformacion.FechaCierre = (DateTime)oDR["via_fecci"];
            oViaInformacion.Parte = Conversiones.edt_Int(oDR["via_parte"]);
            oViaInformacion.Turno = Conversiones.edt_Int(oDR["via_nturn"]);
            oViaInformacion.Senti = Conversiones.edt_Str(oDR["via_senti"]);
            oViaInformacion.Pordi = Conversiones.edt_Decimal(oDR["via_pordi"]);

            // POR CADA VIA CARGA EL TOTAL POR FORMA DE PAGO
            using (Conexion connAux = new Conexion())
            {
                connAux.ConectarGSTPlaza(false, false);
                oViaInformacion.TotalXForPago = ViaInformacionDt.getViaInformacionTotal(connAux, oViaInformacion.CodEstacion, oViaInformacion.NumeroVia);
                connAux.Dispose();
            }

            return oViaInformacion;
        }

        #endregion

        #region ViaInformacionTotal: Clase de Datos de ViaInformacionTotal.

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de objetos ViaInformacionTotal
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 29/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oConn - Conexion - objeto de conexion a la base de datos correspondiente
        //                                  xiCodEst - int - Numero de estacion a filtrar
        //                                  xiVia - int - Número de vía a filtrar
        //                    Retorna: Lista de ViaInformacionTotal: ViaInformacionTotalL
        // ----------------------------------------------------------------------------------------------
        public static ViaInformacionTotalL getViaInformacionTotal(Conexion oConn, int iCodEst, int? iVia)
        {
            ViaInformacionTotalL oViaInformacionTotal = new ViaInformacionTotalL();
            SqlDataReader oDR2;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getTotalUltimoBloque";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iCodEst;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = iVia;

            oCmd.CommandTimeout = 120;

            oDR2 = oCmd.ExecuteReader();
            while (oDR2.Read())
            {
                oViaInformacionTotal.Add(CargarViaInformacionTotal(oDR2));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR2.Close();

            //return oTiposDiaHora;
            return oViaInformacionTotal;
        }
        
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Carga un elemento DataReader en la lista de ViaInformacionTotal
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 29/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oDR - System.Data.IDataReader - Objeto DataReader de la tabla
        //                    Retorna: Lista de ViaInformacionTotal: ViaInformacionTotalL
        // ----------------------------------------------------------------------------------------------
        private static ViaInformacionTotal CargarViaInformacionTotal(IDataReader oDR)
        {
            ViaInformacionTotal oViaInformacionTotal = new ViaInformacionTotal();

            oViaInformacionTotal.CodEstacion = Conversiones.edt_Int16(oDR["via_coest"]);
            oViaInformacionTotal.NumeroVia = Conversiones.edt_Int16(oDR["via_nuvia"]);
            oViaInformacionTotal.FormaDePago = Conversiones.edt_Str(oDR["for_corto"]);
            oViaInformacionTotal.Cantidad = Conversiones.edt_Int(oDR["cantidad"]);

            return oViaInformacionTotal;
        }

        #endregion
        
        #region ESTADO DE LA LISTA NEGRA DE LAS VIAS

        /// <summary>
        /// Devuelve una lista con las vias y los estados de sus listas negras
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iCoest"></param>
        /// <param name="iVia"></param>
        /// <returns></returns>
        public static DataSet GetViaEstadosListaNegra(Conexion oConn, int? iVia)
        {
            DataSet estados = new DataSet();
            estados.DataSetName = "ViaEstadosListaNegra";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ViaEstadoListaNegra_getEstadoListaNegraPistas";

            oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = iVia;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(estados, "Estados");

            oCmd = null;
            oDA.Dispose();

            return estados;
        }

        #endregion
    }
}

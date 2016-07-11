using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    public class Nivel2Dt
    {

        #region TRANSITOS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Transitos por Forma de Pago para una Estación/Vía
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <param name="xiVia">int? - Código de la vía para la busqueda</param>
        /// <returns>Lista de Transitos por Forma de Pago</returns>
        /// ***********************************************************************************************
        public static Nivel2_TrcXFpL getTrcXFp(Conexion oConn, int xiEstacion, int xiVia)
        {
            Nivel2_TrcXFpL oTransitos = new Nivel2_TrcXFpL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Vias_getTotalUltimoBloqueDet";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = xiVia;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTransitos.Add(CargarTrcXFP(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTransitos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Nivel2_TrcXFp
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Trc x F.Pago</param>
        /// <returns>Objeto Nivel2_TrcXFp con los datos</returns>
        /// ***********************************************************************************************
        private static Nivel2_TrcXFp CargarTrcXFP(System.Data.IDataReader oDR)
        {
            try
            {
                Nivel2_TrcXFp oTransito = new Nivel2_TrcXFp();
                oTransito.TipoTrc = Conversiones.edt_Str(oDR["FormaPago"]);
                oTransito.Cantidad = Conversiones.edt_Int(oDR["Cantidad"]);
                return oTransito;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region EXENTOS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Exentos para una Estación/Vía
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <param name="xiVia">int? - Código de la vía para la busqueda</param>
        /// <returns>Lista de Exentos para el Nivel 2</returns>
        /// ***********************************************************************************************
        public static Nivel2_ExentoL getExentos(Conexion oConn, int xiEstacion, int xiVia)
        {
            Nivel2_ExentoL oExentos = new Nivel2_ExentoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Vias_getExentosUltimoBloque";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = xiVia;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oExentos.Add(CargarExento(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oExentos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Nivel2_Exento
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Exentos</param>
        /// <returns>Objeto Nivel2_Exento con los datos</returns>
        /// ***********************************************************************************************
        private static Nivel2_Exento CargarExento(System.Data.IDataReader oDR)
        {
            try
            {
                Nivel2_Exento oExento = new Nivel2_Exento();
                oExento.TipoExento = Conversiones.edt_Str(oDR["TipoExento"]);
                oExento.Cantidad = Conversiones.edt_Int(oDR["Cantidad"]);
                return oExento;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region VENTAS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Ventas para una Estación/Vía
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <param name="xiVia">int? - Código de la vía para la busqueda</param>
        /// <returns>Lista de Ventas para el Nivel 2</returns>
        /// ***********************************************************************************************
        public static Nivel2_VentasL getVentas(Conexion oConn, int xiEstacion, int xiVia)
        {
            Nivel2_VentasL oVentas = new Nivel2_VentasL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Vias_getVentasUltimoBloque";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = xiVia;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oVentas.Add(CargarVenta(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVentas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Nivel2_Venta
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Venta</param>
        /// <returns>Objeto Nivel2_Venta con los datos</returns>
        /// ***********************************************************************************************
        private static Nivel2_Venta CargarVenta(System.Data.IDataReader oDR)
        {
            try
            {
                Nivel2_Venta oVenta = new Nivel2_Venta();
                oVenta.TipoVenta = Conversiones.edt_Str(oDR["TipoVenta"]);
                oVenta.Cantidad = Conversiones.edt_Int(oDR["Cantidad"]);
                return oVenta;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region DIFERENCIAS entre lo que categoriza el Operador y el Dac

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de diferencias entre lo cat. por el operador y el dac
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <param name="xiVia">int? - Código de la vía para la busqueda</param>
        /// <returns>Lista de diferencias para el Nivel 2</returns>
        /// ***********************************************************************************************
        public static Nivel2_OpeDacL getDiferencias(Conexion oConn, int xiEstacion, int xiVia)
        {
            Nivel2_OpeDacL oDiferencias = new Nivel2_OpeDacL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Vias_getCategoriasUltimoBloque";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = xiVia;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oDiferencias.Add(CargarDiferencia(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
                //oConn.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oDiferencias;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Nivel2_OpeDac
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Diferencia</param>
        /// <returns>Objeto Nivel2_OpeDac con los datos</returns>
        /// ***********************************************************************************************
        private static Nivel2_OpeDac CargarDiferencia(System.Data.IDataReader oDR)
        {
            try
            {
                Nivel2_OpeDac oDiferencia = new Nivel2_OpeDac();

                oDiferencia.CodEstacion = Conversiones.edt_Int16(oDR["via_coest"]);
                oDiferencia.NumeroVia = Conversiones.edt_Int16(oDR["via_nuvia"]);
                oDiferencia.CatManual = Conversiones.edt_Int16(oDR["CategoriaManual"]);
                oDiferencia.CatDac = Conversiones.edt_Int16(oDR["CategoriaDac"]);
                oDiferencia.Manual = (oDR["desc_manua"]).ToString();
                oDiferencia.Dac = (oDR["desc_dac"]).ToString();

                oDiferencia.Cantidad = Conversiones.edt_Int(oDR["Cantidad"]);

                return oDiferencia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}

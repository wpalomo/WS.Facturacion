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
    public class TipoListaDt
    {

        #region TIPOS DE LISTAS

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Tipos de Lista para ser importados. Utilizado en los Comandos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Tipos de Lista</returns>
        /// ***********************************************************************************************
        public static TipoListaL getTiposLista(Conexion oConn)
        {
            TipoListaL oTiposLista = new TipoListaL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_TipLis_GetTiposLista";

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTiposLista.Add(CargarTipoLista(oDR));
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
            return oTiposLista;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto TipoLista
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de TipoLista</param>
        /// <returns>Objeto TipoLista con los datos</returns>
        /// ***********************************************************************************************
        private static TipoLista CargarTipoLista(System.Data.IDataReader oDR)
        {
            try
            {
                TipoLista oTipoLista= new TipoLista();
                oTipoLista.Codigo = Conversiones.edt_Str(oDR["tip_codig"]);
                oTipoLista.Descripcion= Conversiones.edt_Str(oDR["tip_descr"]);
                return oTipoLista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion del Template
    /// </summary>
    ///****************************************************************************************************


    public static class TemplateDt
    {
        private const string strDataTableName = "Fields";
        private const string strWhereTableName = "Datos_Where";
        private const string strValuesTableName = "Datos_Values";
        private const string strSelectClause = "SELECT Datos_Table.Table_Id,Datos_Table.Table_Name,Datos_Where.Table_Field As FieldWhere,Datos_Values.Table_Field As FieldValue FROM (Datos_Table LEFT JOIN Datos_Values ON Datos_Values.Table_Id = Datos_Table.Table_Id) \n LEFT JOIN Datos_Where ON Datos_Where.Table_Id = Datos_Table.Table_Id \n Order By Datos_Table.Table_Id,Datos_Where.Table_Field;";

        #region TEAMPLATE: Clase de Datos de Templates


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Templates
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static TemplateL getTemplates(Conexion oConn)
        {
            //TODO:Implementar.
            
            return null;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de campos por los cuales se filtrara el Excel
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string getWhereFields(Conexion oConn)
        {
            return getFields(oConn, strWhereTableName);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de campos que contendran la informacion en el Excel
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void getAllFields(Conexion oConn,ref Template File)
        {
            getFields(oConn,ref File);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de campos que contendran la informacion en el Excel
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static string getValuesFields(Conexion oConn,string strTableName)
        {
            return getFields(oConn,strTableName);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista una lista de campos segun la tabla solicitada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void getFields(Conexion oConn,ref Template File)
        {
            List<string> strReturnValue = new List<string>();
            string strValuesFields = string.Empty;
            string strTable = string.Empty;
            string strWhere = string.Empty;
            string strValue = string.Empty;

            File.Tables = new List<string>();File.CamposDatos = new List<string>();File.CamposWhere = new List<string>();

            DataSet dtValuesFields = getData(oConn);

            for (int i = 0; i < dtValuesFields.Tables["Filters"].Rows.Count; i++)
            {
                if (strTable.IndexOf(dtValuesFields.Tables["Filters"].Rows[i]["Table_Name"].ToString()) < 0)
                {
                    strTable = dtValuesFields.Tables["Filters"].Rows[i]["Table_Name"].ToString();
                    File.Tables.Add(strTable);

                    if(strWhere != string.Empty)
                        File.CamposWhere.Add(strWhere);
                    if (strValue != string.Empty)
                    File.CamposDatos.Add(strValue);
                    strWhere = string.Empty;
                    strValue = string.Empty;
                }

                if (strWhere.IndexOf(dtValuesFields.Tables["Filters"].Rows[i]["FieldWhere"].ToString()) < 0)
                {
                    strWhere += dtValuesFields.Tables["Filters"].Rows[i]["FieldWhere"].ToString() + "#" + dtValuesFields.Tables["Filters"].Rows[i]["Table_Name"].ToString() + "|";
                }

                if (strValue.IndexOf(dtValuesFields.Tables["Filters"].Rows[i]["FieldValue"].ToString()) < 0)
                {
                    strValue += dtValuesFields.Tables["Filters"].Rows[i]["FieldValue"].ToString() + "#" + dtValuesFields.Tables["Filters"].Rows[i]["Table_Name"].ToString() + "|";
                }
            }

            if (File.Tables.IndexOf(strTable) < 0)
                File.Tables.Add(strTable);

            if (File.CamposWhere.IndexOf(strWhere) < 0)
                File.CamposWhere.Add(strWhere);

            if (File.CamposDatos.IndexOf(strValue) < 0)
                File.CamposDatos.Add(strValue);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista una lista de campos segun la tabla solicitada
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static string getFields(Conexion oConn, string strTableName)
        {
            List<string> strReturnValue = new List<string>();
            string strValuesFields = string.Empty;

            DataSet dtValuesFields = getData(oConn, strTableName);

            for (int i = 0; i < dtValuesFields.Tables[strDataTableName].Rows.Count; i++)
            {
                for (int j = 0; j < dtValuesFields.Tables[strDataTableName].Columns.Count; j++)
                {
                    strValuesFields += dtValuesFields.Tables[strDataTableName].Rows[i][j].ToString() + "|";
                }
            }
            return strValuesFields.Substring(0, strValuesFields.Length - 1);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Templates
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static DataSet getData(Conexion oConn)
        {
            OleDbCommand objCmdSelect = new OleDbCommand(strSelectClause, oConn.oleConection);

            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;

            DataSet ds = new DataSet();

            objAdapter.Fill(ds, "Filters");

            return ds;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Templates
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static DataSet getData(Conexion oConn, string strTableName)
        {
            OleDbCommand objCmdSelect = new OleDbCommand("SELECT * FROM " + strTableName, oConn.oleConection);

            OleDbDataAdapter objAdapter = new OleDbDataAdapter();
            objAdapter.SelectCommand = objCmdSelect;

            DataSet ds = new DataSet();

            objAdapter.Fill(ds, strDataTableName);

            return ds;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un string con el query a ejecutar
        /// </summary>
        /// <param name="oTemplate">Template - Archivo a modificar</param>
        /// <param name="oData">Datos - Dataset con los datos a ingresar al archivo</param>
        /// ***********************************************************************************************
        public static string getUpdScript(string strTableName, string strFieldValues, string strWhereClause)
        {
            try
            {
                return "UPDATE " + strTableName + " SET " + strFieldValues + " WHERE " + strWhereClause + "; \r\n";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un template
        /// </summary>
        /// <param name="oTemplate">Template - Archivo a modificar</param>
        /// <param name="oData">Datos - Dataset con los datos a ingresar al archivo</param>
        /// ***********************************************************************************************
        public static bool updData(Conexion conn, string strScript)
        {
            try
            {
                OleDbCommand objCmdUpdate = new OleDbCommand();
                objCmdUpdate.Connection = conn.oleConection;
                objCmdUpdate.CommandText = strScript;
                objCmdUpdate.ExecuteNonQuery();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }

}

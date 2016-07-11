using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de Templates
    /// </summary>
    ///****************************************************************************************************


    public static class TemplateBs
    {
        #region TEMPLATE: Metodos de la Clase de Negocios de la entidad Template.

        /// ***********************************************************************************************
        /// <summary>
        /// Copia el template solicitado a una nueva ruta.
        /// </summary>
        /// <returns>Bool si logro copiar el archivo</returns>
        /// ***********************************************************************************************
        public static bool Copy(Template OriginalFile,string CopyDirectory)
        {
            return CopyFile(ref OriginalFile, CopyDirectory, string.Empty);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Copia el template solicitado.
        /// </summary>
        /// <returns>Bool si logro copiar el archivo</returns>
        /// ***********************************************************************************************
        public static bool CopyFile(ref Template File,string NewDirectory,string NewName)
        {
            bool bCopyOk = true;
            try
            {
                NewName = NewName + File.Extension;
                string sourceFile = System.IO.Path.Combine(File.Path, File.GetFullName);
                string destFile = System.IO.Path.Combine(NewDirectory, (NewName.Equals(string.Empty) ? File.GetFullName : NewName));

                if (!System.IO.Directory.Exists(NewDirectory))
                {
                    System.IO.Directory.CreateDirectory(NewDirectory);
                }

                // Copia el archivo a una nueva ubicacion y lo sobreescribe si existe
                System.IO.File.Copy(sourceFile, destFile, true);

                FileInfo a = new FileInfo(destFile);
                a.IsReadOnly = false;
                a.Refresh();

                File = getTemplate(NewName, NewDirectory);

                return bCopyOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }           
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Copia el template solicitado a una nueva ruta, con un nuevo nombre.
        /// </summary>
        /// <returns>Bool si logro copiar el archivo</returns>
        /// ***********************************************************************************************
        public static bool CopyAs(Template OriginalFile, string CopyDirectory, string NewFileName)
        {
            return CopyFile(ref OriginalFile, CopyDirectory, NewFileName);
        }

        public static Template getTemplate(string FileName, string Path)
        {
            string sourceFile = System.IO.Path.Combine(Path, FileName);
            string path = string.Empty;

            if (File.Exists(sourceFile))
            {
                FileInfo a = new FileInfo(sourceFile);
                path = a.DirectoryName;

                if ((path.IndexOf("\\", path.Length - 2) >= 0))
                    path = path.Remove(path.IndexOf("\\", path.Length - 2), 1);

                Template tmpArchivo = new Template(a.Name.Remove(a.Name.IndexOf(a.Extension)), path + "\\", a.Extension);

                Conexion conn = new Conexion();
                conn.ConectarExcel(tmpArchivo.GetFullFile, false, false);

                TemplateDt.getAllFields(conn,ref tmpArchivo);

                conn.oleConection.Close();
                conn.Dispose();

                return tmpArchivo;
            }

            return null;
        }

        public static bool UpdateFile(ref Template File, DataSet NewData, bool bCopy, string newDirectory,string newName)
        {
            if(bCopy)
            {
                if (!CopyFile(ref File, newDirectory, newName))
                    return false;
            }

            return UpdateFile(File, NewData);
        }

        private static bool UpdateFile(Template File, DataSet NewData)
        {
            string strValue = string.Empty;
            string strWhere = string.Empty;
            Conexion conn = new Conexion();
            conn.ConectarExcel(File.GetFullFile, false, false);
            try
            {
                for (int i = 0; i < NewData.Tables.Count; i++)
                {
                    for (int j = 0; j < NewData.Tables[i].Rows.Count; j++)
                    {
                        for (int k = 0; k < NewData.Tables[i].Columns.Count; k++)
                        {
                            if (File.CamposDatos[i].Contains(NewData.Tables[i].Columns[k].ColumnName + "#" + NewData.Tables[i].TableName))
                                strValue += NewData.Tables[i].Columns[k].ColumnName + "='" + NewData.Tables[i].Rows[j][k].ToString() + "', ";

                            if (File.CamposWhere[i].Contains(NewData.Tables[i].Columns[k].ColumnName + "#" + NewData.Tables[i].TableName))
                                strWhere += NewData.Tables[i].Columns[k].ColumnName + "='" + NewData.Tables[i].Rows[j][k].ToString() + "' AND ";
                        }
                        TemplateDt.updData(conn, TemplateDt.getUpdScript(NewData.Tables[i].TableName, strValue.Substring(0, strValue.Length - 2), strWhere.Substring(0, strWhere.Length - 5)));
                        strWhere = string.Empty;
                        strValue = string.Empty;
                    }
                }
            }
            catch(Exception ex)
            {

            }
            conn.oleConection.Close();

            return true;
        }

        /*private static bool UpdateFile(Template File,DataSet NewData)
        {
            string strValue = string.Empty;
            string strWhere = string.Empty;
            string strUpdStatement = string.Empty;
            Conexion conn = new Conexion();
            conn.ConectarExcel(File.GetFullFile, false, false);

            for(int i=0;i<NewData.Tables[0].Rows.Count;i++)
            {
                for (int j=0;j< NewData.Tables[0].Columns.Count; i++)
                {
                    if(File.CamposDatos.Contains(NewData.Tables[0].Columns[j].ColumnName + "#" + NewData.Tables[0].TableName))
                        strValue += NewData.Tables[0].Columns[j].ColumnName + "=" + NewData.Tables[0].Rows[i][j].ToString() + ", ";

                    if(File.CamposWhere.Contains(NewData.Tables[0].Columns[j].ColumnName + "#" + NewData.Tables[0].TableName))
                        strWhere += NewData.Tables[0].Columns[j].ColumnName + "=" + NewData.Tables[0].Rows[i][j].ToString() + " AND ";
                }
                strUpdStatement += TemplateDt.getUpdScript(NewData.Tables[0].TableName, strValue.Substring(0, strValue.Length - 2), strWhere.Substring(0, strValue.Length - 5));
            }

            return TemplateDt.updData(conn, strUpdStatement);
        }*/
        
        #endregion
    }

}

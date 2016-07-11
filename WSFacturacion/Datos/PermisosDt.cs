using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class PermisosDt
    {
        #region PERMISOS: Clase de Datos de Permisos

        public static DataSet rptPermisos(Conexion oConn, DataSet dsPerfiles)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Permisos_rptPermisos";                

                oCmd.CommandTimeout = 3600;

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsPerfiles, "Permisos");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
                //oConn.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsPerfiles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Permisos para un perfil
        /// de una pantalla o de todo un menu
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="grupo">string - Perfil</param>
        /// <param name="soloHabi">bool - Solo se quieren los permisos habilitados</param>
        /// <param name="gst">bool - Estamos en Gestion?</param>
        /// <param name="gst">bool - Estamos en Cliente?</param>
        /// <param name="modulo">string - Codigo del modulo</param>
        /// <param name="pagina">string - Codigo de pagina, null para todo el menu del modulo</param>
        /// <returns>Lista de Permisos</returns>
        /// ***********************************************************************************************
        public static PermisoL getPermisos(Conexion oConn, string grupo, bool soloHabi, bool? gst, bool? cli, string modulo, string pagina)
        {
            PermisoL oPermisos = new PermisoL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Permisos_getPermisos";
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar,30).Value = grupo;
                string sGST = null;
                if(cli == true)
                    sGST = "C";
                else if (gst == true)
                    sGST = "S";
                else if (gst == false)
                    sGST = "N";
                oCmd.Parameters.Add("@gst", SqlDbType.Char, 1).Value = sGST;
                oCmd.Parameters.Add("@modulo", SqlDbType.Char, 3).Value = modulo;
                oCmd.Parameters.Add("@pagina", SqlDbType.VarChar, 50).Value = pagina;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    if (!soloHabi
                        || oDR["cps_habi"].ToString() == "S"
                        || oDR["cps_habi"].ToString() == "A")
                    {
                        oPermisos.Add(CargarPermiso(oDR));
                    }
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPermisos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Crea un objeto Permiso en base al contenido del DataReader
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Permisos</param>
        /// <returns>Objeto con el elemento Permiso generado</returns>
        /// ***********************************************************************************************
        private static Permiso CargarPermiso(System.Data.IDataReader oDR)
        {
            Permiso oPermiso = new Permiso();
            oPermiso.Perfil = oDR["cps_grupo"].ToString();
            oPermiso.EsGestion = (oDR["ctr_gst"].ToString() == "S");
            oPermiso.EsCliente = (oDR["ctr_gst"].ToString() == "C");
            oPermiso.Modulo = oDR["ctr_modulo"].ToString();
            oPermiso.Pagina = oDR["ctr_pagina"].ToString();
            oPermiso.Control = oDR["ctr_control"].ToString();
            oPermiso.Descripcion = oDR["ctr_descr"].ToString();
            oPermiso.Habilitado = (oDR["cps_habi"].ToString() == "S");
            oPermiso.Autorizacion = (oDR["cps_habi"].ToString() == "A");
            oPermiso.EnNuevaVentana = (oDR["ctr_newpage"].ToString() == "S");

            return oPermiso;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Permiso en la base de datos
        /// Si no existe la inserta, si existe la modifica
        /// </summary>
        /// <param name="perfil">string - codigo del perfil</param>
        /// <param name="oPermiso">Permiso - Datos del Permiso</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updPermiso(string perfil, Permiso oPermiso, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Permisos_updPermiso";

                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
                oCmd.Parameters.Add("@gst", SqlDbType.Char, 1).Value = oPermiso.EsGestion?"S":(oPermiso.EsCliente?"C":"N");
                oCmd.Parameters.Add("@modulo", SqlDbType.VarChar, 50).Value = oPermiso.Modulo;
                oCmd.Parameters.Add("@pagina", SqlDbType.VarChar, 50).Value = oPermiso.Pagina;
                oCmd.Parameters.Add("@control", SqlDbType.VarChar, 50).Value = oPermiso.Control;
                oCmd.Parameters.Add("@habi", SqlDbType.Char, 1).Value = oPermiso.Habilitado ? "S" : 
                    (oPermiso.Autorizacion ? "A" : "N");


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);

                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todos los Permisos de un perfil en la base de datos
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delPermisos(string perfil, Conexion oConn)
        {
            delPermiso(perfil, null, oConn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un permiso en la base de datos
        /// si oPermiso es null elimina todos los permisos del perfil
        /// </summary>
        /// <param name="perfil">string - codigo de perfil a eliminar</param>
        /// <param name="oPermiso">Permiso - datos del permiso a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delPermiso(string perfil, Permiso oPermiso, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Permisos_delPermiso";

                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = perfil;
                string sGST = null;
                string modulo = null;
                string pagina = null;
                string control = null;
                if (oPermiso != null)
                {
                    sGST = oPermiso.EsGestion ? "S" : (oPermiso.EsCliente?"C":"N");
                    modulo = oPermiso.Modulo;
                    pagina = oPermiso.Pagina;
                    control = oPermiso.Control;
                }
                oCmd.Parameters.Add("@gst", SqlDbType.Char, 1).Value = sGST;
                oCmd.Parameters.Add("@modulo", SqlDbType.Char, 3).Value = modulo;
                oCmd.Parameters.Add("@pagina", SqlDbType.VarChar, 50).Value = pagina;
                oCmd.Parameters.Add("@control", SqlDbType.VarChar, 50).Value = control;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Iconos para del toolbar
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="grupo">string - Perfil</param>
        /// <param name="soloHabi">bool - Solo se quieren los permisos habilitados</param>
        /// <param name="gst">bool - Estamos en Gestion?</param>
        /// <returns>Lista de Toolbar</returns>
        /// ***********************************************************************************************
        public static ToolbarL getToolbar(Conexion oConn, string grupo, bool soloHabi, bool? gst)
        {
            ToolbarL oToolbar = new ToolbarL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Permisos_getToolbar";
                oCmd.Parameters.Add("@grupo", SqlDbType.VarChar, 30).Value = grupo;
                string sGST = null;
                if (gst == true)
                    sGST = "S";
                else if (gst == false)
                    sGST = "N";
                oCmd.Parameters.Add("@gst", SqlDbType.Char, 1).Value = sGST;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    if (!soloHabi
                        || oDR["cps_habi"].ToString() == "S")
                    {
                        oToolbar.Add(CargarToolbar(oDR));
                    }
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oToolbar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Crea un objeto Toolbar en base al contenido del DataReader
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Permisos</param>
        /// <returns>Objeto con el elemento Toolbar generado</returns>
        /// ***********************************************************************************************
        private static Toolbar CargarToolbar(System.Data.IDataReader oDR)
        {
            Toolbar oToolbar = new Toolbar();
            oToolbar.Perfil = oDR["cps_grupo"].ToString();
            oToolbar.EsGestion = (oDR["tbr_gst"].ToString() == "S");
            oToolbar.Modulo = oDR["tbr_modulo"].ToString();
            oToolbar.Pagina = oDR["tbr_pagina"].ToString();
            oToolbar.Habilitado = (oDR["cps_habi"].ToString() == "S");
            oToolbar.Icono = oDR["tbr_icono"].ToString();
            oToolbar.Orden = (Byte)oDR["tbr_orden"];
            oToolbar.EnNuevaVentana = (oDR["ctr_newpage"].ToString() == "S");

            return oToolbar;
        }

        #endregion
    }
}

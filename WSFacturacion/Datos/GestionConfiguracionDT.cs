using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;

namespace Telectronica.Peaje
{
    public class GestionConfiguracionDt
    {
        #region OBSERVACIONES: Clase de Datos de Observaciones.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Observaciones definidas
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Observaciones</returns>
        /// ***********************************************************************************************
        public static ObservacionL getObservaciones(Conexion oConn, int? codigo)
        {
            ObservacionL oObservaciones = new ObservacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Observaciones_getObservaciones";
                oCmd.Parameters.Add("@obs_obser", SqlDbType.TinyInt).Value = codigo;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oObservaciones.Add(CargarObservaciones(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oObservaciones;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// /// Carga un elemento DataReader en la la lista de Observaciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Observaciones</param>
        /// <returns>Lista con el elmento Observacion de la base de datos</returns>
        /// ***********************************************************************************************
        private static Observacion CargarObservaciones(System.Data.IDataReader oDR)
        {
            Observacion oObservacion = new Observacion();

            oObservacion.Codigo = (byte) oDR["obs_obser"];
            oObservacion.Descripcion = oDR["obs_descr"].ToString();
            return oObservacion;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Observacion en la base de datos
        /// </summary>
        /// <param name="Observacion">Observacion - Objeto con la informacion de la Observación a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addObservacion(Observacion oObservacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Observaciones_addObservacion";

                oCmd.Parameters.Add("@obs_obser", SqlDbType.TinyInt).Value = oObservacion.Codigo;
                oCmd.Parameters.Add("@obs_descr", SqlDbType.VarChar, 50).Value = oObservacion.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("Este número de Observación ya existe");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Observación en la base de datos
        /// </summary>
        /// <param name="oObservacion">Observación - Objeto con la informacion de la Observación a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updObservacion(Observacion oObservacion, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Observaciones_updObservacion";

                oCmd.Parameters.Add("@obs_obser", SqlDbType.TinyInt).Value = oObservacion.Codigo;
                oCmd.Parameters.Add("@obs_descr", SqlDbType.VarChar, 50).Value = oObservacion.Descripcion;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Observación");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Observación de la base de datos
        /// </summary>
        /// <param name="oObservacion">Int - Numero de Observación a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delObservacion(int Codigo, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Observaciones_delObservacion";

                oCmd.Parameters.Add("@obs_obser", SqlDbType.TinyInt).Value = Codigo;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Observación");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Observación no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }
        }

        public static void delObservacion(Observacion oObservacion, Conexion oConn)
        {
            delObservacion(oObservacion.Codigo, oConn);
        }
        
        #endregion
        
        #region CONFIGURACIONCCO: Clase de Datos de Configcco.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configcco definidas
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configcco</returns>
        /// ***********************************************************************************************
        public static GestionConfiguracion getConfigcco(Conexion oConn)
        {
            GestionConfiguracion oConfigcco = new  GestionConfiguracion();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ConfiguracionCCO_GetConfiguracionCCO";
                
                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oConfigcco = CargarConfigcco(oDR);
                }
                else
                {
                    //No habia datos
                    oConfigcco = new GestionConfiguracion();
                    oConfigcco.PorcentajeIva = 0;
                    oConfigcco.PorcentajeRetencionServicios = 0;
                    oConfigcco.PorcentajeRetencionBienes = 0;
                    oConfigcco.MultiplicadorViolaciones = 1;
                    oConfigcco.NombreConcesionario = "";
                    oConfigcco.CategoriaMonocategoria = 1;
                    oConfigcco.DireccionURL = "";
                    oConfigcco.CobraEixos = "N";
                    oConfigcco.DiferenciaLiquidacion = 0;
                    oConfigcco.EjesParaFotoAVI = 0;
                    oConfigcco.PaisAntena = "0000";
                    oConfigcco.ConcesionarioAntena = "00000";
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oConfigcco;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Configcco
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Configcco</param>
        /// <returns>Lista con el elmento Configcco de la base de datos</returns>
        /// ***********************************************************************************************
        private static GestionConfiguracion CargarConfigcco(System.Data.IDataReader oDR)
        {
            GestionConfiguracion oConfigcco = new GestionConfiguracion();

            if (oDR["con_conce"] != DBNull.Value)
            {
                oConfigcco.NombreConcesionario = Convert.ToString(oDR["con_conce"]);
            }

            if (oDR["con_iva"] != DBNull.Value)
            {
                oConfigcco.PorcentajeIva = Convert.ToDouble(oDR["con_iva"]);
            }

            if (oDR["con_monoc"] != DBNull.Value)
            {
                oConfigcco.CategoriaMonocategoria = Convert.ToInt16(oDR["con_monoc"]);
            }

            if (oDR["con_muvio"] != DBNull.Value)
            {
                oConfigcco.MultiplicadorViolaciones = Convert.ToDouble(oDR["con_muvio"]);
            }

            oConfigcco.DireccionURL = Convert.ToString(oDR["con_url"]);

            //ECU
            if (oDR["con_retfu"] != DBNull.Value)
            {
                oConfigcco.PorcentajeRetencionServicios = Convert.ToDouble(oDR["con_retfu"]);
            }

            if (oDR["con_retfb"] != DBNull.Value)
            {
                oConfigcco.PorcentajeRetencionBienes = Convert.ToDouble(oDR["con_retfb"]);
            }

            if (oDR["con_eixos"] != DBNull.Value)
            {
                oConfigcco.CobraEixos = Convert.ToString(oDR["con_eixos"]);
            }

            if (oDR["con_difli"] != DBNull.Value)
            {
                oConfigcco.DiferenciaLiquidacion = Convert.ToDecimal(oDR["con_difli"]);
            }

            if (oDR["con_ejfot"] != DBNull.Value)
            {
                oConfigcco.EjesParaFotoAVI = Convert.ToByte(oDR["con_ejfot"]);
            }

            if (oDR["con_monvi"] != DBNull.Value)
            {
                oConfigcco.MonedaVia = Convert.ToInt32(oDR["con_monvi"]);
            }

            if (oDR["con_pstag"] != DBNull.Value)
            {
                oConfigcco.PaisAntena = Convert.ToString(oDR["con_pstag"]);
            }

            if (oDR["con_cotag"] != DBNull.Value)
            {
                oConfigcco.ConcesionarioAntena = Convert.ToString(oDR["con_cotag"]);
            }

            if (oDR["con_alrcg"] != DBNull.Value)
            {
                oConfigcco.TiempoAlarmaClienteGrafico = Convert.ToInt32(oDR["con_alrcg"]);
            }

            return oConfigcco;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion CCO en la base de datos
        /// </summary>
        /// <param name="oConfigcco">Configuracion CCO  - Objeto con la informacion de la Configuracion CCO a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfigcco(GestionConfiguracion oConfigcco, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ConfiguracionCCO_updConfiguracionCCO";

                oCmd.Parameters.Add("@con_url", SqlDbType.VarChar, 255).Value = oConfigcco.DireccionURL;
                oCmd.Parameters.Add("@con_eixos", SqlDbType.Char, 1).Value = oConfigcco.CobraEixos;
                oCmd.Parameters.Add("@con_ejfot", SqlDbType.TinyInt).Value = oConfigcco.EjesParaFotoAVI;
                oCmd.Parameters.Add("@con_pstag", SqlDbType.Char, 4).Value = oConfigcco.PaisAntena;
                oCmd.Parameters.Add("@con_cotag", SqlDbType.Char, 5).Value = oConfigcco.ConcesionarioAntena;
                oCmd.Parameters.Add("@con_alrcg", SqlDbType.SmallInt).Value = oConfigcco.TiempoAlarmaClienteGrafico;


                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Configuracion CCO");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la configuracion de la moneda que debe usar la via en la Configuracion del  CCO 
        /// </summary>
        /// <param name="intMoneda">Int16 - Codigo de moneda que se establece que debe usar la via</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfigccoCon_monvi(Int16 intMoneda, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ConfiguracionCCO_updConfiguracionMonedaVia";

                oCmd.Parameters.Add("@con_monvi", SqlDbType.SmallInt).Value = intMoneda;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Configuracion CCO");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        #endregion        

        #region CFGTRIB: Clase de Datos de CFGTRIB.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de CFGTRIB definidas
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de CFGTRIB</returns>
        /// ***********************************************************************************************
        public static ConfiguracionTributaria getConfigtrb(Conexion oConn)
        {
            ConfiguracionTributaria oConfigtrb = new ConfiguracionTributaria();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ConfiguracionTRB_GetConfiguracionTRB";

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oConfigtrb = CargarConfigtrb(oDR);
                }
                else
                {
                    //No habia datos
                    oConfigtrb = new ConfiguracionTributaria();
                    oConfigtrb.PorcentajeIva = 0;
                    oConfigtrb.PorcentajeRetencionServicios = 0;
                    oConfigtrb.PorcentajeRetencionBienes = 0;
                    oConfigtrb.Nombre = "";
                    oConfigtrb.RazonSocial = "";
                    oConfigtrb.RUC = "";
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oConfigtrb;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de CFGTRIB
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de CFGTRIB</param>
        /// <returns>Lista con el elmento CFGTRIB de la base de datos</returns>
        /// ***********************************************************************************************
        private static ConfiguracionTributaria CargarConfigtrb(System.Data.IDataReader oDR)
        {
            ConfiguracionTributaria oConfigtrb = new ConfiguracionTributaria();

            oConfigtrb.Nombre = Convert.ToString(oDR["cfg_nombr"]);
            oConfigtrb.RazonSocial = Convert.ToString(oDR["cfg_rasoc"]);
            oConfigtrb.RUC = Convert.ToString(oDR["cfg_ruc"]);

            oConfigtrb.PorcentajeIva = Convert.ToDouble(oDR["cfg_iva"]);

            if (oDR["cfg_retfu"] != DBNull.Value)
            {
                oConfigtrb.PorcentajeRetencionServicios = Convert.ToDouble(oDR["cfg_retfu"]);
            }
            if (oDR["cfg_retfb"] != DBNull.Value)
            {
                oConfigtrb.PorcentajeRetencionBienes = Convert.ToDouble(oDR["cfg_retfb"]);
            }

            oConfigtrb.Direccion = oDR["cfg_direc"].ToString();

            oConfigtrb.ContribuyenteEspecial = oDR["cfg_conesp"].ToString();
            if (oDR["cfg_fecesp"] != DBNull.Value)
            {
                oConfigtrb.FechaContribuyenteEspecial = (DateTime)oDR["cfg_fecesp"];
            }

            return oConfigtrb;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una CFGTRIB en la base de datos
        /// </summary>
        /// <param name="oConfigcco">CFGTRIB  - Objeto con la informacion de la CFGTRIB a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfigtrb(ConfiguracionTributaria oConfigtbr, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ConfiguracionTRB_updConfiguracionTRB";
                oCmd.Parameters.Add("@cfg_iva", SqlDbType.SmallMoney).Value = oConfigtbr.PorcentajeIva;
                oCmd.Parameters.Add("@cfg_ruc", SqlDbType.VarChar, 13).Value = oConfigtbr.RUC;
                oCmd.Parameters.Add("@cfg_nombr", SqlDbType.VarChar, 255).Value = oConfigtbr.Nombre;
                oCmd.Parameters.Add("@cfg_rasoc", SqlDbType.VarChar, 255).Value = oConfigtbr.RazonSocial;
                oCmd.Parameters.Add("@cfg_retfb", SqlDbType.SmallMoney).Value = oConfigtbr.PorcentajeRetencionBienes;
                oCmd.Parameters.Add("@cfg_retfu", SqlDbType.SmallMoney).Value = oConfigtbr.PorcentajeRetencionServicios;
                oCmd.Parameters.Add("@cfg_direc", SqlDbType.VarChar, 50).Value = oConfigtbr.Direccion;
                oCmd.Parameters.Add("@cfg_conesp", SqlDbType.VarChar).Value = oConfigtbr.ContribuyenteEspecial;
                oCmd.Parameters.Add("@cfg_fecesp", SqlDbType.DateTime).Value = oConfigtbr.FechaContribuyenteEspecial;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Configuracion Tributaria");
                    }
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        #endregion

        #region CAUSAS DE CIERRE: Clase de Datos de Causas de Cierre de la vía
        
        public static causaCierreL getCausasCierre(Conexion oConn, int? codci)
        {
            causaCierreL oCausaCierre = new causaCierreL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CausasCierre_getCausasCierre";
                oCmd.Parameters.Add("@cci_codig", SqlDbType.TinyInt).Value = codci;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCausaCierre.Add(CargarCausasCierre(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCausaCierre;
        }

        private static CausaCierre CargarCausasCierre(System.Data.IDataReader oDR)
        {
            CausaCierre oCausaCierre = new CausaCierre((byte)oDR["cci_codig"], oDR["cci_descr"].ToString());
            oCausaCierre.EnVia = (oDR["cci_envia"] == DBNull.Value ? "N" : oDR["cci_envia"].ToString());
            return oCausaCierre;
        }
        
        //ABM Causas de Cierre
        public static void addCausaCierre(CausaCierre oCausaCierre, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CausasCierre_addCausasCierre";

                oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = oCausaCierre.Codigo;
                oCmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 30).Value = oCausaCierre.Descripcion;
                oCmd.Parameters.Add("@EnVia", SqlDbType.Char, 1).Value = oCausaCierre.EnVia;
                

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("Este Código de Causa de cierre ya existe");
                    }
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                    {
                        msg = Traduccion.Traducir("Este Código de Causa de cierre fue eliminado");
                    }

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void updCausaCierre(CausaCierre oCausaCierre, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CausasCierre_updCausasCierre";

                oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = oCausaCierre.Codigo;
                oCmd.Parameters.Add("@DESCRIPCION", SqlDbType.VarChar, 30).Value = oCausaCierre.Descripcion;
                oCmd.Parameters.Add("@EnVia", SqlDbType.Char, 1).Value = oCausaCierre.EnVia;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la estación");
                    }
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void delCausaCierre(int CausaCierre, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CausasCierre_delCausasCierre";

                oCmd.Parameters.Add("@CODIGO", SqlDbType.TinyInt).Value = CausaCierre;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                    if (retval == -102)
                    {
                        msg = Traduccion.Traducir("No existe el registro de la Causa de cierre");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Causa de cierre no se puede dar de baja porque está siendo utilizada"));
                throw ex;
            }
        }

        public static void delCausaCierre(CausaCierre oCausaCierre, Conexion oConn)
        {
            delCausaCierre(oCausaCierre.Codigo, oConn);
        }

        #endregion

        #region CONFIGCCO TESORERIA: Métodos encargados de registrar la configuración para el módulo de Tesoreria

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion CCO en la base de datos
        /// </summary>
        /// <param name="oConfigcco">Configuracion CCO  - Objeto con la informacion de la Configuracion CCO a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfigCCOTesoreria(GestionConfiguracion oConfigcco, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionCCO_updConfiguracionCCOTesoreria";

            oCmd.Parameters.Add("@con_difli", SqlDbType.Money).Value = oConfigcco.DiferenciaLiquidacion;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al modificar el registro ") + retval.ToString();

                if (retval == -102)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Configuracion CCO");
                }

                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}

        
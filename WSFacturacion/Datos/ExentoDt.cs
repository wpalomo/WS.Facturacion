using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;

namespace Telectronica.Peaje
{
    public class ExentoDt
    {
        #region EXENTO: Clase de Datos de Tipo de Exentos
        
        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los Exentos por Estación para un reporte
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptExentosEstaciones(Conexion oConn)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptPeaje_ExentosEstacionesDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_getRptExentosEstaciones";

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsExentos, "ExentosEstaciones");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
            return dsExentos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los exentos para el reporte
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptExentos(Conexion oConn)
        {
            DataSet dsExentos = new DataSet();
            dsExentos.DataSetName = "RptPeaje_ExentosDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_getExentos";
            oCmd.Parameters.Add("cod_codig", SqlDbType.TinyInt).Value = null;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsExentos, "Exentos");

            // Cerramos el objeto
            oCmd = null;
            oDA.Dispose();
            return dsExentos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene una lista de código de exento.
        /// Si se especifica la patente, Obtiene un código de exento asociado a la patente
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="sPatente"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ExentoCodigoL getExentoByPatente(Conexion oConn, string sPatente)
        {
            ExentoCodigoL exento = new ExentoCodigoL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getExentoByPatente";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = sPatente;

            var oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                exento.Add(CargarExentoByPatente(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return exento;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga una entidad del tipo Exento obtenido de la base de datos a través de la patente del vehículo
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ExentoCodigo CargarExentoByPatente(IDataReader oDR)
        {
            return new ExentoCodigo
            {
                CodigoExento = Convert.ToInt16(oDR["exe_tipex"]),
                DescrExento = Convert.ToString(oDR["cod_descr"])
            };
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipo de Exentos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Codigo">int - Numero de categoria por la cual filtrar la busqueda</param>
        /// <returns>Lista de Tipo de Exentos</returns>
        /// ***********************************************************************************************
        public static ExentoCodigoL getExentos(Conexion oConn, int? Codigo)
        {
            ExentoCodigoL oExento = new ExentoCodigoL();
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_getExentos";
            oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = Codigo;

            var oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oExento.Add(CargarExentos(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oExento;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tipo de Exentos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipo de Exento</param>
        /// <returns>Lista con los elementos de Tipo de Exentos de la base de datos</returns>
        /// ***********************************************************************************************
        private static ExentoCodigo CargarExentos(IDataReader oDR)
        {
            var exento = new ExentoCodigo
            {
                CodigoExento = (byte)oDR["cod_codig"],
                DescrExento = oDR["cod_descr"].ToString(),
                MuestraDiplay = oDR["cod_displ"].ToString(),
                RequiereAutorizacion = oDR["cod_habis"].ToString(),
                TipCodigoAutorizacion = new TipoAutorizacionExento(oDR["cod_tpcod"].ToString()),
                CodigoAutorizacionObligatoria = oDR["cod_codob"].ToString(),
                ExentoMotivo = new ExentoMotivo { Codigo = Convert.ToString(oDR["cod_motfr"]), Descripcion = Convert.ToString(oDR["mot_descr"]) }
            };
            
            return exento;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="exento"></param>
        /// ***********************************************************************************************
        private static void AddParametersExentoCodigoToUpdOrAdd(SqlCommand cmd, ExentoCodigo exento)
        {
            cmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = exento.CodigoExento;
            cmd.Parameters.Add("@cod_descr", SqlDbType.VarChar, 50).Value = exento.DescrExento;
            cmd.Parameters.Add("@cod_displ", SqlDbType.Char, 1).Value = exento.MuestraDiplay;
            cmd.Parameters.Add("@cod_habis", SqlDbType.Char, 1).Value = exento.RequiereAutorizacion;
            cmd.Parameters.Add("@cod_tpcod", SqlDbType.Char, 1).Value = exento.TipCodigoAutorizacion.Codigo;
            cmd.Parameters.Add("@cod_codob", SqlDbType.Char, 1).Value = exento.CodigoAutorizacionObligatoria;
            cmd.Parameters.Add("@cod_motfr", SqlDbType.Char, 1).Value = exento.ExentoMotivo.Codigo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Tipo de Exento en la base de datos
        /// </summary>
        /// <param name="oExento">Exento - Objeto con la informacion del Tipo de Exento a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addExento(ExentoCodigo oExento, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_addExento";

            AddParametersExentoCodigoToUpdOrAdd(oCmd, oExento);           
                
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
                
            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("Este Código de Exento ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este Código de Exento fue eliminado");
                }

                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Tipo de Exento en la base de datos
        /// </summary>
        /// <param name="oExento">Exento - Objeto con la informacion del Tipo de Exento a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updExento(ExentoCodigo oExento, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_updExento";

            AddParametersExentoCodigoToUpdOrAdd(oCmd, oExento);

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
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro del Exento");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Tipo de Exento en la base de datos
        /// </summary >
        /// <param name="oExento">Exento - Objeto que contiene el Exento a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delExento(ExentoCodigo oExento, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_delExento";

            oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = oExento.CodigoExento;

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
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro del Exento");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        #endregion

        #region EXENTO ESTACION: Clase de Datos de Tipo de Exentos
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipo de Exentos con sus resepctivas Estaciones*
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Codigo">int - Codigo de exento por la cual filtrar la busqueda</param>
        /// <returns>Lista de Tipo de Exentos</returns>
        /// ***********************************************************************************************
        public static ExentoEstacionL getExentosEstacion(Conexion oConn, int? Codigo)
        {
            ExentoEstacionL oExento = new ExentoEstacionL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_getExentosEstaciones";
            oCmd.Parameters.Add("@cod_codig", SqlDbType.TinyInt).Value = Codigo;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oExento.Add(CargarExentosEstacion(Codigo, oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oExento;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Estaciones por Tipo de Exento
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipo de Exento</param>
        /// <returns>Lista con el elemento Exento de la base de datos</returns>
        /// ***********************************************************************************************
        private static ExentoEstacion CargarExentosEstacion(int? Codigo, System.Data.IDataReader oDR)
        {
            // Objeto Estacion que se le anexa al objeto Estacion

           Subestacion oSubestacion = new Subestacion(new Estacion((byte)oDR["est_codig"], oDR["est_nombr"].ToString()),
                                                      new ViaSentidoCirculacion(oDR["sub_senti"].ToString(), ""), 
                                                      (int)oDR["sub_subes"],
                                                      oDR["sub_descr"].ToString()
                                                     );

            
            return new ExentoEstacion(oSubestacion, 
                                      oDR["Habilitado"].ToString()
                                     );

        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Centralizamos la grabacion de la habilitacion de la lista de habilitaciones de exento por estacion
        /// </summary>
        /// <param name="oExento">ExentoCodigo - Objeto del exento modificado
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updExentoCodigo(ExentoEstacionL oExentoEstacionL, Conexion oConn)
        {
            foreach (ExentoEstacion oExentoEstacion in  oExentoEstacionL)
            {
                // Si no esta habilitada la eliminamos, sino la insertamos (el SP de agregar se encarga de ver si esta o no)
                if (oExentoEstacion.esHabilitado)
                {
                    ExentoDt.addExentoEstacion(oExentoEstacion, oConn);
                }
                else
                {
                    ExentoDt.delExentoEstacion(oExentoEstacion, oConn);
                }
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Tipo de Exento en la base de datos
        /// </summary>
        /// <param name="oExentoEst">ExentoEstacion - Objeto con la informacion del Tipo de Exento a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addExentoEstacion(ExentoEstacion oExentoEst, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_addExentoEstacion";

            oCmd.Parameters.Add("@Subest", SqlDbType.Int).Value = oExentoEst.NumeroSubEstacion;
            oCmd.Parameters.Add("@fra_codfr", SqlDbType.TinyInt).Value = oExentoEst.CodigoExento;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todas las habilitaciones de por estacion para un exento puntual
        /// </summary>
        /// <param name="oExento">ExentoCodigo - Objeto para saber de que categoria hay 
        ///                                que eliminar las habilitaciones por forma de pago</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delExentosEstaciones(ExentoCodigo oExentoCodigo, Conexion oConn)
        {
            // Invocamos a otro metodo para que elimine todas las habilitaciones por estacion para el exento            
            ExentoEstacion oExEst = new ExentoEstacion();
            
            oExEst.CodigoExento = oExentoCodigo.CodigoExento;
            oExEst.subEstacion = null;

            delExentoEstacion(oExEst, oConn);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Estacion de un Tipo de Exento en la base de datos
        /// </summary >
        /// <param name="oExentoEst">ExentoEstacion - Objeto que contiene la Estacion del Tipo de evento a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delExentoEstacion(ExentoEstacion oExentoEst, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Exentos_delExentoEstacion";


            oCmd.Parameters.Add("@fra_codfr", SqlDbType.TinyInt).Value = oExentoEst.CodigoExento;

            if (oExentoEst.subEstacion == null)
            {
                oCmd.Parameters.Add("@Subest", SqlDbType.Int).Value = null;
            }
            else
            {
                oCmd.Parameters.Add("@Subest", SqlDbType.Int).Value = oExentoEst.NumeroSubEstacion;
            }


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
        
        #endregion

        #region MOTIVOSDEEXENTOS: Clase de Negocios de la Entidad MotivosDeExentos

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene una lista de Motivos de Exento.        
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="sPatente"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ExentoMotivoL getMotivoDeExento(Conexion oConn, string sCodigo)
        {
            var motExentos = new ExentoMotivoL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ExentoMotivo_getExentoMotivo";
            oCmd.Parameters.Add("@mot_codig", SqlDbType.Char, 1).Value = sCodigo;

            var oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                motExentos.Add(CargarMotivoDeExento(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return motExentos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga una entidad del tipo Motivos de Exento obtenido de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ExentoMotivo CargarMotivoDeExento(IDataReader oDR)
        {
            return new ExentoMotivo
            {
                Codigo = Convert.ToString(oDR["mot_codig"]),
                Descripcion = Convert.ToString(oDR["mot_descr"])
            };
        }

        #endregion


        #region SREMOTA
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Causas de Supervisión Remota
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>        
        /// <returns>Lista de Tipo de Franquicias</returns>
        /// ***********************************************************************************************
        public static CausaSupervisionL getCausasSupervision(Conexion oConn)
        {
            CausaSupervisionL oCausas = new CausaSupervisionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_getCausas";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCausas.Add(CargarCausaSupervision(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCausas;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Causas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipo de Exento</param>
        /// <returns>Lista con los elementos de Tipo de Exentos de la base de datos</returns>
        /// ***********************************************************************************************
        private static CausaSupervision CargarCausaSupervision(System.Data.IDataReader oDR)
        {

            CausaSupervision oCausa = new CausaSupervision();
                                                   
                 
            oCausa.Codigo = Convert.ToByte(oDR["cod_codig"]);
            oCausa.Descripcion = oDR["cod_descr"].ToString();
            oCausa.MedioPago =  oDR["cod_tipop"].ToString();
            oCausa.FormaPago =  oDR["cod_tipbo"].ToString();
            oCausa.SubformaPago = oDR["cod_subfp"].ToString();;
            oCausa.EsDefecto = oDR["cod_defecto"].ToString();

            return oCausa;
        }

        #endregion
    }
}



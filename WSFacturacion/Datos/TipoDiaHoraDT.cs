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

namespace Telectronica.Peaje
{
    public class TipoDiaHoraDt
    {
        #region FERIADO: Clase de Datos de Feriado.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Feriados definidos
        /// </summary>
        /// <param name="oConn">Conecion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Feriados</returns>
        /// ***********************************************************************************************
        public static FeriadoL getFeriados(Conexion oConn, DateTime? Fecha)
        {
            FeriadoL oFeriados = new FeriadoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Feriados_getFeriados";
            oCmd.Parameters.Add("@Fecha", SqlDbType.DateTime).Value = Fecha;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oFeriados.Add(CargarFeriado(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oFeriados;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Zonas 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de zonas</param>
        /// <returns>Lista de Feriados</returns>
        /// ***********************************************************************************************
        private static Feriado CargarFeriado(System.Data.IDataReader oDR)
        {
            Feriado oFeriados = new Feriado();
            oFeriados.Fecha = (DateTime) oDR["fer_fecha"];
            oFeriados.DiaSemana = getDiaSemana((byte)oDR["fer_diase"]);
            return oFeriados;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un registro de dia feriado en la base de datos
        /// </summary>
        /// <param name="oFeriado">Feriado - Objeto con la informacion del feriado a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addFeriado(Feriado oFeriado, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Feriados_addFeriado";

            oCmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = oFeriado.Fecha;
            oCmd.Parameters.Add("@DIA", SqlDbType.Int).Value = oFeriado.DiaSemana.Codigo;

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
                    msg = Traduccion.Traducir("El registro del día feriado ya existe");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una zona en la base de datos
        /// </summary>
        /// <param name="oFeriado">Feriado - Objeto con la informacion del feriado a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updFeriado(Feriado oFeriado, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Feriados_updFeriado";

            oCmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = oFeriado.Fecha;
            oCmd.Parameters.Add("@DIA", SqlDbType.Int).Value = oFeriado.DiaSemana.Codigo;

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
                    msg = Traduccion.Traducir("No existe el registro del día feriado");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un feriado de la base de datos
        /// </summary>
        /// <param name="Fecha">DateTime - Fecha (que es PK) del registro a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delFeriado(DateTime Fecha, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Feriados_DelFeriado";

                oCmd.Parameters.Add("@FECHA", SqlDbType.DateTime).Value = Fecha;

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
                        msg = Traduccion.Traducir("No existe el registro del día feriado");
                    }
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("El Feriado no se puede dar de baja porque está siendo utilizado"));
                throw;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un feriado de la base de datos, recibiendo la estructura "Feriado"
        /// </summary>
        /// <param name="oFeriado">Feriado - Contiene los datos del feriado a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delFeriado(Feriado oFeriado, Conexion oConn)
        {
            delFeriado(oFeriado.Fecha, oConn);
        }
        
        #endregion
        
        #region DIASEMANA: Clase de Datos de DiaSemana.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto Dia de Semana. 
        /// </summary>
        /// <param name="codigo">int - Codigo del dia de la semana que deseo devolver como texto</param>
        /// <returns>Objeto de Dia Semana</returns>
        /// ***********************************************************************************************
        public static DiaSemana getDiaSemana(int codigo)
        {
            DiaSemana oDia = new DiaSemana();
            oDia.Codigo = codigo;
            oDia.Descripcion = RetornaDia(codigo);
            return oDia;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido de un dia de la semana
        /// </summary>
        /// <param name="codigo">int - Codigo del dia de la semana que deseo devolver como texto</param>
        /// <returns>El texto traducido del dia de la semana</returns>
        /// ***********************************************************************************************
        protected static string RetornaDia(int codigo)
        {
            string retorno = string.Empty;
            int caseSwitch = codigo;

            switch (caseSwitch)
            {
                case 1:
                    retorno = "Lunes";
                    break;
                case 2:
                    retorno = "Martes";
                    break;
                case 3:
                    retorno = "Miércoles";
                    break;
                case 4:
                    retorno = "Jueves";
                    break;
                case 5:
                    retorno = "Viernes";
                    break;
                case 6:
                    retorno = "Sábado";
                    break;
                case 7:
                    retorno = "Domingo";
                    break;
                case 8:
                    retorno = "Feriado";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }
        
        #endregion
        
        #region TIPODIAHORA: Clase de Datos de TipoDiaHora.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TipoDiaHora definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoTipoDiaHora">string - Codigo de TipoDiaHora a filtrar</param>
        /// <returns>Lista de TipoDiaHora</returns>
        /// ***********************************************************************************************
        public static TipoDiaHoraL getTiposDiaHora(Conexion oConn, string codigoTipoDiaHora)
        {
            TipoDiaHoraL oTiposDiaHora = new TipoDiaHoraL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDiaHora_getTiposDiaHora";                   
            oCmd.Parameters.Add("@tipodiahora", SqlDbType.Char,4).Value = codigoTipoDiaHora;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTiposDiaHora.Add(CargarTiposDiaHora(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTiposDiaHora;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de TiposDiaHora
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de TipoDiaHora</param>
        /// <returns>Lista con el elemento TipoDiaHora de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoDiaHora CargarTiposDiaHora(System.Data.IDataReader oDR)
        {
            TipoDiaHora oTipoDiaHora = new TipoDiaHora(oDR["tip_codig"].ToString(),
                                                      oDR["tip_descr"].ToString(),
                                                      oDR["tip_corto"].ToString());
            return oTipoDiaHora;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un TipoDiaHora en la base de datos
        /// </summary>
        /// <param name="oTipoDiaHora">TipoDiaHora - Objeto con la informacion del TipoDiaHora a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTipoDiaHora(TipoDiaHora oTipoDiaHora, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDiaHora_addTipoDiaHora";

            oCmd.Parameters.Add("@Codigo", SqlDbType.Char, 4).Value = oTipoDiaHora.Codigo;
            oCmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = oTipoDiaHora.Descripcion;
            oCmd.Parameters.Add("@DescripcionCorta", SqlDbType.Char, 10).Value = oTipoDiaHora.DescripcionCorta;

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
                    msg = Traduccion.Traducir("Este codigo de Tipo de Día y Hora ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este codigo de Tipo de Día y Hora fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un TipoDiaHora en la base de datos
        /// </summary>
        /// <param name="oTipoDiaHora">TipoDiaHora - Objeto con la informacion del TipoDiaHora a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updTipoDiaHora(TipoDiaHora oTipoDiaHora, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDiaHora_updTipoDiaHora";

            oCmd.Parameters.Add("@Codigo", SqlDbType.Char, 4).Value = oTipoDiaHora.Codigo;
            oCmd.Parameters.Add("@Descripcion", SqlDbType.VarChar, 50).Value = oTipoDiaHora.Descripcion;
            oCmd.Parameters.Add("@DescripcionCorta", SqlDbType.Char, 10).Value = oTipoDiaHora.DescripcionCorta;


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
                    msg = Traduccion.Traducir("No existe el registro del Tipo de Día y Hora");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un TipoDiaHora de la base de datos
        /// </summary>
        /// <param name="TipoDiaHora">String - TipoDiaHora a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTipoDiaHora(string TipoDiaHora, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TiposDiaHora_delTipoDiaHora";

            oCmd.Parameters.Add("@tipodiahora", SqlDbType.Char, 4).Value = TipoDiaHora;

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
                    msg = Traduccion.Traducir("No existe el registro del Tipo de Día y Hora");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un TipoDiaHora de la base de datos
        /// </summary>
        /// <param name="TipoDiaHora">TipoDiaHora - TipoDiaHora a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTipoDiaHora(TipoDiaHora oTipoDiaHora, Conexion oConn)
        {
            delTipoDiaHora(oTipoDiaHora.Codigo, oConn);
        }

        #endregion
        
        #region BANDAHORARIA: Clase de Datos de las Bandas Horarias.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Bandas Horarias definidas para mostrar en la grilla
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoEstacion">int - Codigo de estacion de la que deseo conocer las bandas horarias</param>
        /// <param name="sentidoCirculacion">string - Sentido de circulacion de la banda</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia de la banda</param>
        /// <returns>Lista de Bandas Horarias</returns>
        /// ***********************************************************************************************
        public static BandaHorariaL getBandasHorariasCabecera(Conexion oConn, int? codigoEstacion, string sentidoCirculacion, DateTime? fechaInicial, int? identity)
        {
            BandaHorariaL oBandaHorariaL = new BandaHorariaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_BandasHorarias_getBandasHorariasCabecera";
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = codigoEstacion;
            oCmd.Parameters.Add("@Senti", SqlDbType.Char, 1).Value = sentidoCirculacion;
            oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = fechaInicial;
            oCmd.Parameters.Add("@Ident", SqlDbType.Int).Value = identity;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBandaHorariaL.Add(CargarBandaHorariaCabecera(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oBandaHorariaL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Bandas Horarias 
        /// Se utilizan en la grilla, por lo que no tienen el detalle de de cada banda, sino que estan agrupadas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bandas Horarias</param>
        /// <returns>Lista con el elemento BandaHoraria de la base de datos</returns>
        /// ***********************************************************************************************
        private static BandaHoraria CargarBandaHorariaCabecera(System.Data.IDataReader oDR)
        {
            BandaHoraria oBandaHoraria = new BandaHoraria((int)oDR["rel_ident"],
                                                          new Estacion((byte)oDR["rel_coest"], oDR["est_nombr"].ToString()),
                                                          new ViaSentidoCirculacion { Codigo = Convert.ToString(oDR["rel_senti"]), Descripcion = Convert.ToString(oDR["sub_sencar"]) }, 
                                                          (DateTime)oDR["rel_fecin"],
                                                          Util.DbValueToNullable <DateTime> (oDR["rel_fecha"]),
                                                          new BandaHorariaIntervalo((byte)oDR["rel_inter"], oDR["rel_inter"].ToString()),
                                                          null);
            return oBandaHoraria;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Bandas Horarias de una definicion especifica, es decir el detalle
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoEstacion">int - Codigo de estacion de la que deseo conocer las bandas horarias</param>
        /// <param name="sentidoCirculacion">string - Sentido de circulacion de la banda</param>
        /// <param name="fechaInicial">datetime - Fecha inicial de vigencia de la banda</param>
        /// <returns>Lista de Bandas Horarias</returns>
        /// ***********************************************************************************************
        public static BandaHorariaDetalleL getBandasHorariasDetalle(Conexion oConn, int identity)
        {
            BandaHorariaDetalleL oBandaHorariaDetalleL = new BandaHorariaDetalleL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_BandasHorarias_getBandasHorariasDetalle";
            oCmd.Parameters.Add("@Ident", SqlDbType.TinyInt).Value = identity;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oBandaHorariaDetalleL.Add(CargarBandaHorariaDetalle(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oBandaHorariaDetalleL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Bandas Horarias 
        /// Trae el detalle de todas las bandas definidas para un cambio de banda especifico
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Bandas Horarias</param>
        /// <returns>Lista con el elemento BandaHoraria de la base de datos</returns>
        /// ***********************************************************************************************
        private static BandaHorariaDetalle CargarBandaHorariaDetalle(System.Data.IDataReader oDR)
        {
            BandaHorariaDetalle oBandaHorariaDetalle = new BandaHorariaDetalle(new TipoDiaHora(oDR["rel_tipdh"].ToString(), "", ""),
                                                                               getDiaSemana((byte)oDR["rel_seman"]),
                                                                               oDR["rel_horai"].ToString(),
                                                                               oDR["rel_horaf"].ToString());
            return oBandaHorariaDetalle;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega la cabecera de una Banda Horaria en la base de datos
        /// </summary>
        /// <param name="oBandaHoraria">BandaHoraria - Objeto con la informacion de la banda a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addBandaHorariaCabecera(BandaHoraria oBandaHoraria, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.CommandText = "Peaje.usp_BandasHorarias_addBandaHorariaCabecera";
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oBandaHoraria.Estacion.Numero;
            oCmd.Parameters.Add("@Sentido", SqlDbType.Char, 1).Value = oBandaHoraria.SentidoCirculacion.Codigo;
                
            SqlParameter parFecIn = oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime);
            parFecIn.Direction = ParameterDirection.InputOutput;
            parFecIn.Value = oBandaHoraria.FechaInicialVigencia;

            oCmd.Parameters.Add("@Intervalo", SqlDbType.TinyInt).Value = oBandaHoraria.Intervalo.Intervalo;

            // Valor identity insertado en la cabecera
            SqlParameter paramIdent = oCmd.Parameters.Add("@Ident", SqlDbType.Int);
            paramIdent.Direction = ParameterDirection.Output;

            // Valor de retorno del SP
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();

            int retval = (int)parRetVal.Value;
            oBandaHoraria.FechaInicialVigencia = (DateTime)parFecIn.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            // Si es negativo (Erro -10x o un error de ejecucion, que lo retornamos negativo) generamos la excepcion, 
            // sino el valor retornado es el identity de la cabecera, que lo cargamos en el objeto de banda horaria para grabar el detalle
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                throw new ErrorSPException(msg);
            }
            else
            {
                oBandaHoraria.Identity = (int)paramIdent.Value;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega el detalle de una Banda Horaria en la base de datos
        /// </summary>
        /// <param name="IdentBanda">int - Identity de la cabecera</param>
        /// <param name="oBandaDetalle">BandaHorariaDetalle - Objeto con la informacion de la banda detallada a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addBandaHorariaDetalle(int IdentBanda, BandaHorariaDetalle oBandaDetalle, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            oCmd.CommandText = "Peaje.usp_BandasHorarias_addBandaHorariaDetalle";
            oCmd.Parameters.Add("@Banda", SqlDbType.Int).Value = IdentBanda;
            oCmd.Parameters.Add("@TipoDia", SqlDbType.Char, 4).Value = oBandaDetalle.TipoDiaHora.Codigo;
            oCmd.Parameters.Add("@Dia", SqlDbType.TinyInt).Value = oBandaDetalle.DiaSemana.Codigo;
            oCmd.Parameters.Add("@HoraIni", SqlDbType.Char, 5).Value = oBandaDetalle.HoraInicial;
            oCmd.Parameters.Add("@HoraFin", SqlDbType.Char, 5).Value = oBandaDetalle.HoraFinal;

            // Valor de retorno del SP
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
        /// Elimina el detalle de las Bandas Horarias de la base de datos
        /// </summary>
        /// <param name="oBandaHoraria">BandaHoraria - Objeto con la informacion de las bandas detalle a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delBandaHorariaDetalle(BandaHoraria oBandaHoraria, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_BandasHorarias_delBandaHorariaDetalle";
            oCmd.Parameters.Add("@IdentBanda", SqlDbType.Int).Value = oBandaHoraria.Identity;
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oBandaHoraria.Estacion.Numero;
            oCmd.Parameters.Add("@Sentido", SqlDbType.Char, 1).Value = oBandaHoraria.SentidoCirculacion.Codigo;
            oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oBandaHoraria.FechaInicialVigencia;

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
                    msg = Traduccion.Traducir("No existe el registro de la Banda Horaria");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina la cabecera de la Banda Horaria de la base de datos
        /// </summary>
        /// <param name="oBandaHoraria">BandaHoraria - Objeto con la informacion de la banda a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delBandaHorariaCabecera(BandaHoraria oBandaHoraria, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_BandasHorarias_delBandaHorariaCabecera";
            oCmd.Parameters.Add("@IdentBanda", SqlDbType.Int).Value = oBandaHoraria.Identity;
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oBandaHoraria.Estacion.Numero;
            oCmd.Parameters.Add("@Sentido", SqlDbType.Char, 1).Value = oBandaHoraria.SentidoCirculacion.Codigo;
            oCmd.Parameters.Add("@FechaIni", SqlDbType.DateTime).Value = oBandaHoraria.FechaInicialVigencia;

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
                    msg = Traduccion.Traducir("No existe el registro de la Banda Horaria");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        #endregion
    }
}

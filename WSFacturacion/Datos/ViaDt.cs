using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Metodos para administracion de las Vías definidas en el sistema
    /// </summary>
    ///****************************************************************************************************
    public class ViaDt
    {
        #region DEFINICION: Definición de las vías de la estación

        /// ***********************************************************************************************
        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="viaDefinicion"></param>
        /// ***********************************************************************************************
        private static void AddParametersViaDefinicionToUpdOrAdd(SqlCommand cmd, ViaDefinicion viaDefinicion)
        {
            cmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = viaDefinicion.Estacion.Numero;
            cmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = viaDefinicion.NumeroVia;
            cmd.Parameters.Add("@Nombr", SqlDbType.VarChar).Value = viaDefinicion.NombreVia;
            cmd.Parameters.Add("@Carril", SqlDbType.TinyInt).Value = viaDefinicion.Carril;
            cmd.Parameters.Add("@Model", SqlDbType.VarChar, 3).Value = viaDefinicion.Modelo.Modelo;
            cmd.Parameters.Add("@Sente", SqlDbType.Char, 1).Value = viaDefinicion.SentidoCirculacion.Codigo;
            cmd.Parameters.Add("@Autot", SqlDbType.Char, 1).Value = viaDefinicion.Autotabulante;
            cmd.Parameters.Add("@Detej", SqlDbType.TinyInt).Value = viaDefinicion.DetectorEjes.Codigo;
            cmd.Parameters.Add("@Detdo", SqlDbType.TinyInt).Value = viaDefinicion.RuedasDuales.Codigo;
            cmd.Parameters.Add("@Detal", SqlDbType.Char, 1).Value = viaDefinicion.SensorAltura;
            cmd.Parameters.Add("@Lecto", SqlDbType.Char, 1).Value = viaDefinicion.Lectograbador;
            cmd.Parameters.Add("@Telep", SqlDbType.Char, 1).Value = viaDefinicion.Telepeaje;
            cmd.Parameters.Add("@Chip", SqlDbType.Char, 1).Value = viaDefinicion.TarjetaChip;
            cmd.Parameters.Add("@Clrng", SqlDbType.Char, 1).Value = viaDefinicion.AceptaClearing;
            cmd.Parameters.Add("@Impcl", SqlDbType.Char, 1).Value = viaDefinicion.ImprimeClearing;
            cmd.Parameters.Add("@Impve", SqlDbType.Char, 1).Value = viaDefinicion.ImprimeVentaAnticipada;
            cmd.Parameters.Add("@Recve", SqlDbType.Char, 1).Value = viaDefinicion.AceptaVentaAnticipada;
            cmd.Parameters.Add("@Video", SqlDbType.Char, 1).Value = viaDefinicion.VideoCamara1.Codigo;
            cmd.Parameters.Add("@Video2", SqlDbType.Char, 1).Value = viaDefinicion.VideoCamara2.Codigo;
            cmd.Parameters.Add("@Vpath", SqlDbType.VarChar, 255).Value = viaDefinicion.PathArchivosVideo;
            cmd.Parameters.Add("@Dista", SqlDbType.SmallInt).Value = viaDefinicion.DistanciaSensores;
            cmd.Parameters.Add("@Puvta", SqlDbType.VarChar).Value = viaDefinicion.PuntoVenta;
            if (viaDefinicion.ViaControladora != null)
            {
                cmd.Parameters.Add("@Contr", SqlDbType.TinyInt).Value = viaDefinicion.ViaControladora.NumeroVia.ToString();
            }
            cmd.Parameters.Add("@exent", SqlDbType.VarChar, 1).Value = viaDefinicion.CobroExento;
            cmd.Parameters.Add("@combo", SqlDbType.VarChar, 1).Value = viaDefinicion.ModoComboio;
            cmd.Parameters.Add("@eixsus", SqlDbType.VarChar, 1).Value = viaDefinicion.ConSensoresEixos;
            cmd.Parameters.Add("@fotoph", SqlDbType.VarChar, 255).Value = viaDefinicion.PathFotos;
            cmd.Parameters.Add("@nomla", SqlDbType.VarChar, 30).Value = viaDefinicion.NombreViaLargo;
            cmd.Parameters.Add("@VisaCash", SqlDbType.Char, 1).Value = viaDefinicion.VisaCash;
            cmd.Parameters.Add("@pitag", SqlDbType.Char, 3).Value = viaDefinicion.ViaAntena;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Carriles
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <returns>Lista de Carriles</returns>
        /// ***********************************************************************************************
        public static CarrilL getCarriles(Conexion oConn, int? xiEstacion)
        {
            CarrilL oCarriles = new CarrilL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_GetCarrilesXEst";
            oCmd.Parameters.Add("@xiCoest", SqlDbType.TinyInt).Value = xiEstacion;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oCarriles.Add(CargarCarril(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oCarriles;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Carril
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Carril</param>
        /// <returns>Objeto Carril con los datos</returns>
        /// ***********************************************************************************************
        private static Carril CargarCarril(System.Data.IDataReader oDR)
        {
            Carril oCarril = new Carril();
            oCarril.NumeroCarril = Conversiones.edt_Byte(oDR["via_carril"]);
            return oCarril;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Vias definidas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de la que quiero consultar la via</param>
        /// <param name="numeroVia">int - Numero de via por la cual filtrar la busqueda</param>
        /// <returns>Lista de Vias definidas</returns>
        /// ***********************************************************************************************
        public static ViaDefinicionL getVias(Conexion oConn, int numeroEstacion, int? numeroVia)
        {
            ViaDefinicionL oViaDefinicionL = new ViaDefinicionL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getVias";
            oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = numeroEstacion;
            oCmd.Parameters.Add("Nuvia", SqlDbType.TinyInt).Value = numeroVia;
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oViaDefinicionL.Add(CargarVia(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oViaDefinicionL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vias
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Vias</param>
        /// <returns>Objeto Via con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViaDefinicion CargarVia(System.Data.IDataReader oDR)
        {
            ViaDefinicion oViaDefinicion = new ViaDefinicion();
            oViaDefinicion.Estacion = new Estacion((byte)oDR["via_coest"], "");
            oViaDefinicion.NumeroVia = (byte)oDR["via_nuvia"];
            oViaDefinicion.NombreVia = oDR["via_nombr"].ToString();
            oViaDefinicion.NombreViaLargo = oDR["via_nomla"].ToString();
            oViaDefinicion.Carril = (byte)oDR["via_carril"];
            oViaDefinicion.Modelo = new ViaModelo(oDR["via_model"].ToString(), oDR["mod_descr"].ToString());
            oViaDefinicion.SentidoCirculacion = new ViaSentidoCirculacion(Convert.ToString(oDR["sub_senti"]), Convert.ToString(oDR["sub_sencar"]));
            oViaDefinicion.Autotabulante = oDR["via_autot"].ToString();
            oViaDefinicion.DetectorEjes = CargaViaDetectorEje(oDR["via_detej"].ToString());
            oViaDefinicion.RuedasDuales = CargaViaRuedasDuales(oDR["via_detdo"].ToString());
            oViaDefinicion.SensorAltura = oDR["via_detal"].ToString();
            oViaDefinicion.Lectograbador = oDR["via_lecto"].ToString();
            oViaDefinicion.Telepeaje = oDR["via_telep"].ToString();
            oViaDefinicion.TarjetaChip = oDR["via_chip"].ToString();
            oViaDefinicion.VisaCash = oDR["via_visa"].ToString();
            oViaDefinicion.AceptaClearing = oDR["via_clrng"].ToString();
            oViaDefinicion.ImprimeClearing = oDR["via_impcl"].ToString();
            oViaDefinicion.AceptaVentaAnticipada = oDR["via_recve"].ToString();
            oViaDefinicion.ImprimeVentaAnticipada = oDR["via_impve"].ToString();
            oViaDefinicion.VideoCamara1 = CargaViaVideo(oDR["via_video"].ToString());
            oViaDefinicion.VideoCamara2 = CargaViaVideo(oDR["via_video2"].ToString());
            oViaDefinicion.PathArchivosVideo = oDR["via_vpath"].ToString();
            oViaDefinicion.DistanciaSensores = (short)oDR["via_dista"];
            oViaDefinicion.PuntoVenta = oDR["via_puvta"].ToString();
            oViaDefinicion.Eliminado = oDR["via_delet"].ToString();
            oViaDefinicion.CobroExento = oDR["via_exent"].ToString();
            oViaDefinicion.ModoComboio = oDR["via_combo"].ToString();
            oViaDefinicion.ConSensoresEixos = oDR["via_eixsus"].ToString();
            oViaDefinicion.ViaAntena = Convert.ToString(oDR["via_pitag"]);

            //La via puede no ser controlada por ninguna otra.
            if (oDR["via_contr"] == DBNull.Value)
            {
                oViaDefinicion.ViaControladora = null;
            }
            else
            {
                oViaDefinicion.ViaControladora = new ViaDefinicion((byte)oDR["via_contr"]);
            }

            if (oDR["via_fotoph"] != DBNull.Value)
            {
                oViaDefinicion.PathFotos = oDR["via_fotoph"].ToString();
            }
            return oViaDefinicion;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Via en la base de datos
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Objeto con la informacion de la via a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addVia(ViaDefinicion oViaDefinicion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_addVia";

            AddParametersViaDefinicionToUpdOrAdd(oCmd, oViaDefinicion);

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
                    msg = Traduccion.Traducir("Este número de vía ya existe");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Via de la base de datos
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Objeto con la informacion de la via a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updVia(ViaDefinicion oViaDefinicion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_updVia";

            AddParametersViaDefinicionToUpdOrAdd(oCmd, oViaDefinicion);
            
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
                    msg = Traduccion.Traducir("No existe el registro de la vía");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Via de la base de datos
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Objeto que contiene la via a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delVia(ViaDefinicion oViaDefinicion, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_delVia";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oViaDefinicion.Estacion.Numero;
            oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = oViaDefinicion.NumeroVia;

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
                    msg = Traduccion.Traducir("No existe el registro de la vía");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Inserta o actualiza el login de Sql correspondiente a una via
        /// </summary>
        /// <param name="Usuario">string - Usuario al que se le creara el login de Sql</param>
        /// <param name="Password">string - Clave encriptada del usuario</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addLoginSql(string Usuario, string Password, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_addLoginsSql";

            oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = Usuario;
            oCmd.Parameters.Add("@Password", SqlDbType.VarChar, 50).Value = Password;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al modificar el login de Sql") + retval.ToString();
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina el login de Sql de un usuario
        /// </summary>
        /// <param name="Usuario">string - Usuario al que se le creara el login de Sql</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delLoginSql(string Usuario, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_delLoginsSql";
            oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 50).Value = Usuario;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al eliminar el login de Sql") + retval.ToString();
                throw new ErrorSPException(msg);
            }
        }

        #endregion
        
        #region ESTADO: Estado de las vías (Todo lo relacionado al ONLINE)

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de carriles con su estado
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 17/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oConn - Conexion - objeto de conexion a la base de datos correspondiente
        //                                  xiCodEst - int - Numero de estacion a filtrar
        //                                  xdFechaDesde - datetime - Fecha Desde a filtrar
        //                                  xdFechaHasta - datetime - Fecha Hasta a filtrar
        //                                  xiMinIdent - int - Nro. de identidad a filtrar
        //                                  xVias - viaL - Vias a filtrar
        //                                  xTiposEventos - ClaveEventoL - Tipos de eventos a filtrar
        //                    Retorna: Lista de InfoEvento: InfoEventoL
        // ----------------------------------------------------------------------------------------------
        public static CarrilStatusL getEstadoEstacion(Conexion oConn, int xiCodEst, int? xiCarril, bool xbAscendenteHaciaArriba)
        {
            CarrilStatusL oCarriles = new CarrilStatusL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getEstadoVias";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiCodEst;
            oCmd.Parameters.Add("@carril", SqlDbType.TinyInt).Value = xiCarril;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oCarriles.Add(CargarCarrilStatus(oDR, xbAscendenteHaciaArriba));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oCarriles;
        }

        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Carga un elemento DataReader en la lista de CarrilStatus
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 17/09/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oDR - System.Data.IDataReader - Objeto DataReader de la tabla
        //                    Retorna: Lista de InfoEvento: InfoEventoL
        // ----------------------------------------------------------------------------------------------
        private static CarrilStatus CargarCarrilStatus(IDataReader oDR, bool xbAscendenteHaciaArriba)
        {
            // OBJETO ESTADO DEL CARRIL
            CarrilStatus oCarrilStatus = new CarrilStatus();
            oCarrilStatus.Carril = Conversiones.edt_Int16(oDR["via_carril"]);
            oCarrilStatus.Principal = Conversiones.edt_Str(oDR["via_principal"]);

            // OBJETO ESTADO DE LA VIA
            var oViaStatusAscendente = CargarViaStatus(oDR, "asc_", oCarrilStatus.Carril);
            ViaStatus oViaStatusDescendente = CargarViaStatus(oDR, "des_", oCarrilStatus.Carril);
            
            if (xbAscendenteHaciaArriba)
            {
                oCarrilStatus.StatusHaciaArriba = oViaStatusAscendente;
                oCarrilStatus.StatusHaciaAbajo = oViaStatusDescendente;
            }
            else
            {
                oCarrilStatus.StatusHaciaArriba = oViaStatusDescendente;
                oCarrilStatus.StatusHaciaAbajo = oViaStatusAscendente;
            }

            // ABIERTO SI ALGUNO DE LOS DOS SENTIDOS ESTA ABIERTO
            if (oViaStatusAscendente.Estado == "A" || oViaStatusDescendente.Estado == "A")
            {
                oCarrilStatus.CarrilAbierto = true;
            }
            else
            {
                oCarrilStatus.CarrilAbierto = false;
            }

            // BIDIRECCION
            if (oViaStatusAscendente.NumeroVia != 0 && oViaStatusDescendente.NumeroVia != 0)
            {
                oCarrilStatus.esBidi = true;
            }
            else
            {
                oCarrilStatus.esBidi = false;
            }

            return oCarrilStatus;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto ViaStatus
        /// </summary>
        /// <param name="dataReader"></param>
        /// <param name="sOrientation"></param>
        /// <param name="iCarril"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static ViaStatus CargarViaStatus(IDataReader dataReader, string sOrientation, int iCarril)
        {
            var viaStatus = new ViaStatus();

            viaStatus.NumeroVia = Conversiones.edt_Int16(dataReader[sOrientation + "nuvia"]);
            viaStatus.NombreVia = Conversiones.edt_Str(dataReader[sOrientation + "nombr"]);
            viaStatus.NumeroCarril = iCarril;
            viaStatus.Modelo = Conversiones.edt_Str(dataReader[sOrientation + "model"]);
            viaStatus.Modo = Conversiones.edt_Str(dataReader[sOrientation + "modo"]);
            viaStatus.Estado = Conversiones.edt_Str(dataReader[sOrientation + "estad"]);
            viaStatus.MonoC = Conversiones.edt_Str(dataReader[sOrientation + "monoc"]);
            viaStatus.ModoQ = Conversiones.edt_Str(dataReader[sOrientation + "modoq"]);
            viaStatus.MinutosIncomunicada = Conversiones.edt_Int(dataReader[sOrientation + "MinutosIncomunicada"]);
            viaStatus.UltFechaComunicacion = Util.DbValueToNullable<DateTime>(dataReader[sOrientation + "ultco"]);
            viaStatus.Id = Conversiones.edt_Str(dataReader[sOrientation + "id"]);
            viaStatus.Parte = Conversiones.edt_Int(dataReader[sOrientation + "parte"]);
            viaStatus.IdSup = Conversiones.edt_Str(dataReader[sOrientation + "idsup"]);
            viaStatus.Lecto = Conversiones.edt_Str(dataReader[sOrientation + "lecto"]);
            viaStatus.Telep = Conversiones.edt_Str(dataReader[sOrientation + "telep"]);
            viaStatus.Chip = Conversiones.edt_Str(dataReader[sOrientation + "chip"]);
            viaStatus.Vide1 = Conversiones.edt_Str(dataReader[sOrientation + "vide1"]);
            viaStatus.Vide2 = Conversiones.edt_Str(dataReader[sOrientation + "vide2"]);
            viaStatus.Semar = Conversiones.edt_Str(dataReader[sOrientation + "semar"]);
            viaStatus.Sepas = Conversiones.edt_Str(dataReader[sOrientation + "sepas"]);
            viaStatus.Barre = Conversiones.edt_Str(dataReader[sOrientation + "barre"]);
            viaStatus.Baren = Conversiones.edt_Str(dataReader[sOrientation + "baren"]);
            viaStatus.Carte = Conversiones.edt_Str(dataReader[sOrientation + "carte"]);
            viaStatus.Sepen = Conversiones.edt_Str(dataReader[sOrientation + "sepen"]);
            viaStatus.Sepsd = Conversiones.edt_Str(dataReader[sOrientation + "sepsd"]);
            viaStatus.Fisca = Conversiones.edt_Str(dataReader[sOrientation + "fisca"]);
            viaStatus.Ped01 = Conversiones.edt_Str(dataReader[sOrientation + "ped01"]);
            viaStatus.Ped02 = Conversiones.edt_Str(dataReader[sOrientation + "ped02"]);
            viaStatus.Ped03 = Conversiones.edt_Str(dataReader[sOrientation + "ped03"]);
            viaStatus.Ped04 = Conversiones.edt_Str(dataReader[sOrientation + "ped04"]);
            viaStatus.Ped57 = Conversiones.edt_Str(dataReader[sOrientation + "ped57"]);
            viaStatus.Ped68 = Conversiones.edt_Str(dataReader[sOrientation + "ped68"]);
            viaStatus.EjesLevantados = Conversiones.edt_Str(dataReader[sOrientation + "eixsu"]);
            viaStatus.Sepsm = Conversiones.edt_Str(dataReader[sOrientation + "sepsm"]);
            viaStatus.Bpr = Conversiones.edt_Str(dataReader[sOrientation + "bpr"]);
            viaStatus.Bpi = Conversiones.edt_Str(dataReader[sOrientation + "bpi"]);
            viaStatus.Bpa = Conversiones.edt_Str(dataReader[sOrientation + "bpa"]);
            viaStatus.TipOp = Conversiones.edt_Str(dataReader[sOrientation + "tipop"]);
            viaStatus.TipBo = Conversiones.edt_Str(dataReader[sOrientation + "tipbo"]);
            viaStatus.FormaPago = Conversiones.edt_Str(dataReader[sOrientation + "FormaPago"]);
            viaStatus.SubFp = Conversiones.edt_Int16(dataReader[sOrientation + "subfp"]);
            viaStatus.Manua = Conversiones.edt_Int16(dataReader[sOrientation + "manua"]);
            viaStatus.Dac = Conversiones.edt_Int16(dataReader[sOrientation + "dac"]);
            viaStatus.Mannu = Conversiones.edt_Int16(dataReader[sOrientation + "mannu"]);
            viaStatus.Cejes = Conversiones.edt_Int16(dataReader[sOrientation + "cejes"]);
            viaStatus.Cdobl = Conversiones.edt_Int16(dataReader[sOrientation + "cdobl"]);
            viaStatus.Altur = Conversiones.edt_Str(dataReader[sOrientation + "altur"]);
            viaStatus.Codis = Conversiones.edt_Int16(dataReader[sOrientation + "codis"]);
            viaStatus.Observacion = Conversiones.edt_Str(dataReader[sOrientation + "obser"]);
            viaStatus.Abort = Conversiones.edt_Str(dataReader[sOrientation + "abort"]);
            viaStatus.Numer = Conversiones.edt_Str(dataReader[sOrientation + "numer"]);
            viaStatus.Ulcle = Conversiones.edt_Int(dataReader[sOrientation + "ulcle"]);
            viaStatus.Lcola = Conversiones.edt_Int16(dataReader[sOrientation + "lcola"]);
            viaStatus.Lprec = Conversiones.edt_Int16(dataReader[sOrientation + "lprec"]);
            viaStatus.PorDis = Conversiones.edt_Decimal(dataReader[sOrientation + "pordi"]);
            //Para no modificar en todos los carriles, solamente modifico el sMANUA para que muestre lo correcto.
            
            //viaStatus.sManua = Conversiones.edt_Str(dataReader[sOrientation + "desc_manua"]);
            viaStatus.sManua = tratarDescripcion(sOrientation,dataReader,true);
            viaStatus.sMannu = tratarDescripcion(sOrientation, dataReader, false);
            viaStatus.sDac = Conversiones.edt_Str(dataReader[sOrientation + "desc_dac"]);
            //viaStatus.sMannu = Conversiones.edt_Str(dataReader[sOrientation + "desc_mannu"]);
            viaStatus.ModoMantenimiento = Conversiones.edt_Str(dataReader[sOrientation + "mante"]);
            viaStatus.seMannu = Conversiones.edt_Str(dataReader[sOrientation + "desc_emannu"]);
            viaStatus.seManua = Conversiones.edt_Str(dataReader[sOrientation+"desc_emanua"]);
            viaStatus.eMannu = Conversiones.edt_Int16(dataReader[sOrientation + "emannu"]);
            viaStatus.eManua= Conversiones.edt_Int16(dataReader[sOrientation + "emanua"]);
                

            return viaStatus;
        }

        private static string tratarDescripcion(string sOrientation,IDataReader dataReader,bool manua)
        {
            string ManualPart = Conversiones.edt_Str(dataReader[sOrientation + "desc_manua"]);
            int categoriaSuspenso = -1;
            if (manua)
            {
                ManualPart = Conversiones.edt_Str(dataReader[sOrientation + "desc_manua"]);
               
                try
                {
                    categoriaSuspenso = Convert.ToInt32(dataReader[sOrientation + "eManua"]);

                }
                catch (Exception)
                {

                }
            }
            else
            {
                ManualPart = Conversiones.edt_Str(dataReader[sOrientation + "desc_mannu"]);
                try
                {
                    categoriaSuspenso = Convert.ToInt32(dataReader[sOrientation + "eMannu"]);

                }
                catch (Exception)
                {

                }
            }


            if (categoriaSuspenso <= 0)
            {
                return ManualPart;
            }
            
            else
            {
                return ManualPart + "+" + categoriaSuspenso + "S";
            }
            
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de vias abiertas por sentido y modelo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion
        /// <returns>Lista con Cantidad de vias abiertas por sentido y modelo</returns>
        /// ***********************************************************************************************
        public static ViasTotalesL getViasTotales(Conexion oConn, int estacion)
        {
            ViasTotalesL oViasTotales = new ViasTotalesL();

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getViasTotales";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oViasTotales.Add(CargarViasTotales(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oViasTotales;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de ViasTotales
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader </param>
        /// <returns>objeto ViasTotales de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViasTotales CargarViasTotales(System.Data.IDataReader oDR)
        {
            ViasTotales oVias = new ViasTotales();
            oVias.Modo = oDR["vie_modo"].ToString();
            oVias.ModoDesc = oDR["mdo_descr"].ToString();
            oVias.Sentido = oDR["via_sente"].ToString();
            oVias.EstadoString = oDR["vie_estad"].ToString();
            oVias.Cantidad = (int)oDR["Cantidad"];

            return oVias;
        }

        #endregion

        #region MODELOSVIA: Modelos en los que se puede tipificar una vía
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de modelos de via
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="ModeloVia">string - Modelo de via a filtrar (o todos)
        /// <returns>Lista de Modelos de via existentes</returns>
        /// ***********************************************************************************************
        public static ViaModeloL getModelosVia(Conexion oConn, string ModeloVia)
        {
            ViaModeloL oModelosVia = new ViaModeloL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_getModelosVia";
            oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = ModeloVia;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oModelosVia.Add(CargarModeloVia(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oModelosVia;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Modelos de Via
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de modelos de via</param>
        /// <returns>Lista con el elemento ViaModelo de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViaModelo CargarModeloVia(System.Data.IDataReader oDR)
        {
            ViaModelo oModeloVia = new ViaModelo(oDR["mod_model"].ToString(), oDR["mod_descr"].ToString());
            return oModeloVia;
        }

        #endregion

        #region VIADETECTOREJES: Tipos de detectores de ejes que puede tener una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos ViaDetectorEjes (posibles tipos de detectores de ejes que puede tener una via)
        /// </summary>
        /// <returns>Lista de objetos ViaDetectorEjes</returns>
        /// ***********************************************************************************************
        public static ViaDetectorEjeL getTiposDetectorEjes()
        {
            ViaDetectorEjeL oViaDetectorEjeL = new ViaDetectorEjeL();

            oViaDetectorEjeL.Add(CargaViaDetectorEje("0"));
            oViaDetectorEjeL.Add(CargaViaDetectorEje("1"));
            oViaDetectorEjeL.Add(CargaViaDetectorEje("2"));
            oViaDetectorEjeL.Add(CargaViaDetectorEje("3"));
            oViaDetectorEjeL.Add(CargaViaDetectorEje("4"));
            oViaDetectorEjeL.Add(CargaViaDetectorEje("5"));
            
            return oViaDetectorEjeL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de detector de ejes
        /// </summary>
        /// <returns>Objetos ViaDetectorEjes con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        protected static ViaDetectorEje CargaViaDetectorEje(string codigo)
        {
            string descripcion = "";
            
            switch (codigo)
            {
                case "0":
                    descripcion = Traduccion.Traducir("Inexistente");
                    break;

                case "1":
                    descripcion = Traduccion.Traducir("Un Sensor");
                    break;

                case "2":
                    descripcion = Traduccion.Traducir("Dos Sensores paralelos");
                    break;

                case "3":
                    descripcion = Traduccion.Traducir("Dos Sensores Alineados");
                    break;

                case "4":
                    descripcion = Traduccion.Traducir("Cuatro Sensores");
                    break;

                case "5":
                    descripcion = Traduccion.Traducir("Tres Sensores");
                    break;
            }

            return new ViaDetectorEje(codigo, descripcion);
        }

        #endregion

        #region VIARUEDASDUALES: Tipos de detectores de ruedas duales que puede tener una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos ViaRuedasDuales (posibles tipos de detectores de ruedas duales que puede tener una via)
        /// </summary>
        /// <returns>Lista de objetos ViaRuedasDuales</returns>
        /// ***********************************************************************************************
        public static ViaRuedasDualesL getTiposRuedasDuales()
        {
            ViaRuedasDualesL oViaRuedasDualesL = new ViaRuedasDualesL();

            oViaRuedasDualesL.Add(CargaViaRuedasDuales("0"));
            oViaRuedasDualesL.Add(CargaViaRuedasDuales("1"));
            oViaRuedasDualesL.Add(CargaViaRuedasDuales("2"));
            
            return oViaRuedasDualesL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de detector de ruedas duales
        /// </summary>
        /// <returns>Objetos ViaDetectorEjes con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        protected static ViaRuedasDuales CargaViaRuedasDuales(string codigo)
        {
            string descripcion = "";
            
            switch (codigo)
            {
                case "0":
                    descripcion = Traduccion.Traducir("Inexistente");
                    break;

                case "1":
                    descripcion = Traduccion.Traducir("Un Sensor");
                    break;

                case "2":
                    descripcion = Traduccion.Traducir("Dos Sensores");
                    break;
            }

            return new ViaRuedasDuales(codigo, descripcion);
        }

        #endregion

        #region VIASENTIDOCIRCULACION: Los posibles sentidos de circulacion que puede tener una vía

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos ViaSentidoCirculacion (posibles sentidos de circulacion que puede tener una via)
        /// <param name="oConn"></param>
        /// <param name="sCodSentidoCirculacion"></param>
        /// <returns>Lista de objetos ViaSentidoCirculacion</returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidosCirculacion(Conexion oConn, string sCodSentidoCirculacion)
        {
            ViaSentidoCirculacionL oModelosVia = new ViaSentidoCirculacionL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ViaSentidoCirculacion_getViaSentidoCirculacion";
            oCmd.Parameters.Add("@sen_senti", SqlDbType.Char, 1).Value = sCodSentidoCirculacion;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oModelosVia.Add(CargaViaSentidoCirculacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oModelosVia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de sentido de circulacion
        /// </summary>
        /// <returns>Objeto ViaSentidoCirculacion con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacion CargaViaSentidoCirculacion(IDataReader dataReader)
        {
            return new ViaSentidoCirculacion
            {
                Codigo = Convert.ToString(dataReader["sub_senti"]),
                Descripcion = Convert.ToString(dataReader["sen_descr"])
            };
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene los sentidos de circulación posible para una vía
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iEstacion"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ViaSentidoCirculacionL getSentidoCirculacionPosible(Conexion oConn, int? iEstacion)
        {
            ViaSentidoCirculacionL oModelosVia = new ViaSentidoCirculacionL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ViaSentidoCirculacion_getViaSentidoCirculacionPosible";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iEstacion;

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oModelosVia.Add(CargaViaSentidoCirculacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oModelosVia;
        }

        #endregion

        #region SENTIDOCIRCULACION: Los sentidos de circulacion Ascendente y Descendente

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos ViaSentidoCirculacion (posibles sentidos de circulacion que puede tener una via)
        /// <param name="oConn"></param>
        /// <param name="sCodSentidoCirculacion"></param>
        /// <returns>Lista de objetos ViaSentidoCirculacion</returns>
        /// ***********************************************************************************************
        public static SentidoCirculacionL getSentidos(Conexion oConn)
        {
            SentidoCirculacionL oSentidos = new SentidoCirculacionL();

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_SentidoCirculacion_getSentidoCirculacion";

            SqlDataReader oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oSentidos.Add(CargaSentidoCirculacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oSentidos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de sentido de circulacion
        /// </summary>
        /// <returns>Objeto SentidoCirculacion con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        public static SentidoCirculacion CargaSentidoCirculacion(IDataReader dataReader)
        {
            return new SentidoCirculacion
            {
                Codigo = Convert.ToString(dataReader["sen_senti"]),
                Descripcion = Convert.ToString(dataReader["sen_descr"])
            };
        }
        #endregion


        #region VIAVIDEO: Tipos de aplicaciones que se les puede dar a las cámaras de video (Video, Foto o Ninguna)

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de objetos ViaSentidoCirculacion (posibles sentidos de circulacion que puede tener una via)
        /// </summary>
        /// <returns>Lista de objetos ViaSentidoCirculacion</returns>
        /// ***********************************************************************************************
        public static ViaVideoL getTiposUsoCamaras()
        {
            ViaVideoL oViaVideoL = new ViaVideoL();

            oViaVideoL.Add(new ViaVideo("N", Traduccion.Traducir("No")));
            oViaVideoL.Add(new ViaVideo("F", Traduccion.Traducir("Foto")));
            oViaVideoL.Add(new ViaVideo("V", Traduccion.Traducir("Video")));
            
            return oViaVideoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la descripcion correspondiente a un codigo de tipo de video
        /// </summary>
        /// <returns>Objeto ViaVideo con el codigo y descripcion</returns>
        /// ***********************************************************************************************
        protected static ViaVideo CargaViaVideo(string codigo)
        {
            string descripcion = "";
            
            switch (codigo)
            {
                case "N":
                    descripcion = Traduccion.Traducir("No");
                    break;

                case "F":
                    descripcion = Traduccion.Traducir("Foto");
                    break;

                case "V":
                    descripcion = Traduccion.Traducir("Video");
                    break;
            }

            return new ViaVideo(codigo, descripcion);
        }

        #endregion

        #region MODOSVIA

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve una lista de los modos según el modelo de una víaç
        /// /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xsModeloVia">string - Modelo de la vía para la cual buscar sus posibles modos
        /// <returns>Lista de Modos según el modelo de una vía</returns>
        /// ***********************************************************************************************
        public static ViaModoL getModosXModelo(Conexion oConn, string sModeloVia)
        {
            ViaModoL oModosVia = new ViaModoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ModosVia_GetModosXModelo";
            oCmd.Parameters.Add("@modelo", SqlDbType.VarChar, 3).Value = sModeloVia;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oModosVia.Add(CargarModoModosXModelo(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oModosVia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto ViaModo
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de modo de via</param>
        /// <returns>Lista con el elemento ViaModo de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViaModo CargarModoModosXModelo(IDataReader oDR)
        {
            var viaModo = new ViaModo();
            viaModo.Modo = Conversiones.edt_Str(oDR["mdd_modo"]);
            return viaModo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Obtiene el modo de vía especificado por parámetro
        /// </summary>
        /// <param name="oConn"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static ViaModoL getViaModos(Conexion oConn, string sModeloVia)
        {
            var modosVia = new ViaModoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ModosVia_GetModos";
            oCmd.Parameters.Add("@modo", SqlDbType.VarChar, 3).Value = sModeloVia;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                modosVia.Add(CargarViaModo(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return modosVia;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto ViaModo
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de modo de via</param>
        /// <returns>Lista con el elemento ViaModo de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViaModo CargarViaModo(IDataReader oDR)
        {
            var viaModo = new ViaModo();
            viaModo.Modo = Conversiones.edt_Str(oDR["mdo_modo"]);
            viaModo.Descripcion = Conversiones.edt_Str(oDR["mdo_descr"]);
            return viaModo;
        }

        #endregion
        
        #region ViaEst

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Vias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <returns>Lista de Vías</returns>
        /// ***********************************************************************************************
        public static ViaL getViasEstacion(Conexion oConn, int? xiEstacion)
        {
            ViaL oVias = new ViaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_GetViasXEst";
            oCmd.Parameters.Add("@xiCoest", SqlDbType.TinyInt).Value = xiEstacion;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oVias.Add(CargarViaEstacion(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVias;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Vias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiEstacion">int? - Código de la estación para la busqueda</param>
        /// <returns>Lista de Vías</returns>
        /// ***********************************************************************************************
        public static ViaComandoL getViasComandos(Conexion oConn, int? xiEstacion)
        {
            ViaComandoL oVias = new ViaComandoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_GetViasComando";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiEstacion;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oVias.Add(CargarViaComando(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVias;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Via
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de Via</param>
        /// <returns>Objeto Via con los datos</returns>
        /// ***********************************************************************************************
        private static Via CargarViaEstacion(IDataReader oDR)
        {
            Via oVia = new Via();

            oVia.Estacion = Conversiones.edt_Int16(oDR["via_coest"]);
            oVia.NumeroVia = Conversiones.edt_Byte(oDR["via_nuvia"]);
            oVia.NombreVia = Conversiones.edt_Str(oDR["via_nombr"]);
                
            return oVia;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto ViaComando
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de ViaComando</param>
        /// <returns>Objeto ViaComando con los datos</returns>
        /// ***********************************************************************************************
        private static ViaComando CargarViaComando(IDataReader oDR)
        {
            ViaComando viaComando = new ViaComando();

            viaComando.Estacion = Conversiones.edt_Int16(oDR["via_coest"]);
            viaComando.NumeroVia = Conversiones.edt_Byte(oDR["via_nuvia"]);
            viaComando.NombreVia = Conversiones.edt_Str(oDR["via_nombr"]);
            viaComando.NumeroCarril = Conversiones.edt_Byte(oDR["via_carril"]);
            viaComando.Sentido = Conversiones.edt_Str(oDR["via_sente"]);
            viaComando.Modelo = Conversiones.edt_Str(oDR["via_model"]);
            viaComando.Modo = Conversiones.edt_Str(oDR["vie_modo"]);
            viaComando.Estado = Conversiones.edt_Str(oDR["vie_estad"]);
            viaComando.EstadoOpuesta = Conversiones.edt_Str(oDR["estadoOpuesta"]);
            viaComando.ModoQuiebre = Conversiones.edt_Str(oDR["vie_modoq"]);
            viaComando.CobroExento = Conversiones.edt_Str(oDR["via_exent"]);
            viaComando.ModoComboio = Conversiones.edt_Str(oDR["via_combo"]);
            viaComando.SemaforoMarquesina = Conversiones.edt_Str(oDR["vie_semar"]);

            // POR CADA VIA CARGA EL TOTAL POR FORMA DE PAGO
            using (Conexion connAux = new Conexion())
            {
                connAux.ConectarGSTPlaza(false, false);
                viaComando.ModosPosiblesApertura = ViaDt.getModosXModelo(connAux, viaComando.Modelo);
                connAux.Dispose();
            }

            return viaComando;
        }

        #endregion

        #region Otros

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una alarma recibida de la Via de la base de datos
        /// </summary>
        /// <param name="Mensaje">Alarma - Objeto Alarma a borrar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delAlarmaRecibidoVia(Alarmas Alarma,string Usuario, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Errores_delAlarmasError";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = Alarma.Estacion;
                oCmd.Parameters.Add("@ident", SqlDbType.Int).Value = Alarma.ID;
                oCmd.Parameters.Add("@Usuario", SqlDbType.VarChar, 10).Value = Usuario;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;

                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al Eliminar el registro") + retval.ToString();
                    if (retval == -102)
                        msg = Traduccion.Traducir("No existe el registro del Número de Via");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La alarma no se puede dar de baja porque está siendo utilizada"));
                throw ex;
            }
            return;
        }

        /// <summary>
        /// Obtiene el path y el nombre de la foto para una vía
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iCodEst"></param>
        /// <param name="iVia"></param>
        /// <returns></returns>
        public static string GetPhotoPath(Conexion oConn, int iCodEst, int iVia,bool Siguio, string Nomfoto)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vias_GetPhotoPath";

            oCmd.Parameters.Add("@vie_coest", SqlDbType.TinyInt).Value = iCodEst;
            oCmd.Parameters.Add("@vie_nuvia", SqlDbType.TinyInt).Value = iVia;
            oCmd.Parameters.Add("@Siguio", SqlDbType.Char, 1).Value = (Siguio == true ? "S" : "N");
            oCmd.Parameters.Add("@nomfo", SqlDbType.VarChar, 100).Value = Nomfoto;

            oCmd.CommandTimeout = 120;

            var retValue = Convert.ToString(oCmd.ExecuteScalar());
            
            // Cerramos el objeto
            oCmd = null;

            return retValue;
        }

        #endregion

        #region Errores de la via

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMensaje">int - Codigo de mensaje a filtrar</param>
        /// <param name="codigoMensaje">char - solo alarmas no confirmadas</param>
        /// <returns>Lista de Mensajes</returns>
        /// ***********************************************************************************************
        public static AlarmasL getErrores(string xsTipo, string xsStatus, ViaL xsVias, DateTime dtFechaHoraDesde, DateTime dtFechaHoraHasta, Conexion oConn, int? codigoError)
        {
            AlarmasL oAlarmasConfirmar = new AlarmasL();
            try
            {

                SqlDataReader oDR;
                // SERIALIZA VIAS 
                string xmlVias = "";
                if (xsVias != null)
                {
                   xmlVias = Utilitarios.xmlUtil.SerializeObject(xsVias);
                }
                else 
                {
                    xmlVias = null;
                }

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Errores_GetAlarmasError";

                oCmd.Parameters.Add("@estado", SqlDbType.NChar, 1).Value = xsStatus;
                oCmd.Parameters.Add("@tipo", SqlDbType.NChar, 1).Value = xsTipo;
                oCmd.Parameters.Add("@xmlVias", SqlDbType.NText).Value = xmlVias;
                oCmd.Parameters.Add("@fecini", SqlDbType.DateTime).Value = dtFechaHoraDesde;
                oCmd.Parameters.Add("@fecfin", SqlDbType.DateTime).Value = dtFechaHoraHasta;

                oDR = oCmd.ExecuteReader();

                while (oDR.Read())
                {
                    oAlarmasConfirmar.Add(CargarAlarmasError(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oAlarmasConfirmar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la cantidad de errores sin confirmar 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Cantidad de mensajes pendientes </returns>
        /// ***********************************************************************************************
        public static int GetHayErrorSinConfirmar(Conexion oConn)
        {
            int retorno = 0;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Errores_GetHayErrorSinConfirmar";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    retorno = (int)oDR["Cantidad"];
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retorno;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Errores 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Mensajes</param>
        /// <returns>Lista con el elemento Mensaje de la base de datos</returns>
        /// ***********************************************************************************************
        private static Alarmas CargarAlarmasError(System.Data.IDataReader oDR)
        {
            DateTime? fechaFin;

            if (oDR["alv_fefin"] != DBNull.Value)
            {
                fechaFin = Convert.ToDateTime(oDR["alv_fefin"]);
            }
            else
            {
                fechaFin = null;
            }

            DateTime? fechaConf;

            if (oDR["alv_fecon"] != DBNull.Value)
            {
                fechaConf = Convert.ToDateTime(oDR["alv_fecon"]);
            }
            else
            {
                fechaConf = null;
            }

            string descr;

            if (oDR["alv_descr"] != DBNull.Value)
            {
                descr = Convert.ToString(oDR["alv_descr"]);
            }
            else
            {
                descr = "";
            }

            Alarmas oAlarmasConfirmar = new Alarmas(
                Convert.ToInt32(oDR["alv_ident"]),
                Convert.ToInt16(oDR["alv_coest"]),
                Convert.ToInt16(oDR["alv_nuvia"]),
                Convert.ToString(oDR["cal_descr"]),
                Convert.ToChar(oDR["alv_confi"]),
                Convert.ToDateTime(oDR["alv_fegen"]),
                fechaConf,
                fechaFin,
                Convert.ToString(oDR["use_nombr"]),
                descr,
                Convert.ToString(oDR["cal_tpson"]));

            return oAlarmasConfirmar;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes 
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Mensajes</param>
        /// <returns>Lista con el elemento Mensaje de la base de datos</returns>
        /// ***********************************************************************************************
        private static string[] CargarErrores(IDataReader oDR)
        {
            String[] errores = null;
            errores[0] = (oDR["barrera"].ToString() == "F") ? (Traduccion.Traducir("Falla la barrera")) : errores[0] = " ";
            return errores;
        }

        #endregion


        #region SUPERVICION REMOTA

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Via de Supervision en la base de datos
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Objeto con la informacion de la via a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addViaSupervision(ViaInformacion oViaInfo, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ViasSupervision_addVia";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oViaInfo.CodEstacion;
                oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = oViaInfo.NumeroVia;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oViaInfo.OperadorId;


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
                        msg = Traduccion.Traducir("Este número de vía ya existe");
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
        /// Elimina una Via de Supervision en la base de datos
        /// </summary>
        /// <param name="oViaDefinicion">ViaDefinicion - Objeto con la informacion de la via a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delViaSupervision(ViaInformacion oViaInfo, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ViasSupervision_delVia";

                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oViaInfo.OperadorId;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oViaInfo.CodEstacion;
                oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = oViaInfo.NumeroVia;

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
                        msg = Traduccion.Traducir("Este número de vía ya existe");
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
        /// Devuelve la lista de Vias de Supervision definidas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de la que quiero consultar la via</param>
        /// <param name="numeroVia">int - Numero de via por la cual filtrar la busqueda</param>
        /// <returns>Lista de Vias definidas</returns>
        /// ***********************************************************************************************
        public static ViaInformacionL getViasSupervision(Conexion oConn,
                                             int? numeroEstacion,
                                             int? numeroVia,
                                             string id)
        {
            ViaInformacionL oViaDefinicionL = new ViaInformacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_ViasSupervision_getVias";
                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = numeroEstacion;
                oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = numeroVia;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = id;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oViaDefinicionL.Add(CargarViaSupervision(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

                /*
                string xmlVias = Utilitarios.xmlUtil.SerializeObject(oViaDefinicionL.getViasSolas());
                StreamWriter sw = new StreamWriter("c:\\xmlVia.xml");

                sw.Write(xmlVias);
                sw.Close();
                */
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oViaDefinicionL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vias de Supervision
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Vias</param>
        /// <returns>Objeto Via con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static ViaInformacion CargarViaSupervision(System.Data.IDataReader oDR)
        {
            ViaInformacion oViaInfo = new ViaInformacion();
            oViaInfo.CodEstacion = Convert.ToInt32(oDR["vsu_coest"]);
            oViaInfo.NumeroVia = Convert.ToInt32(oDR["vsu_nuvia"]);
            oViaInfo.OperadorId = Convert.ToString(oDR["vsu_id"]);
            return oViaInfo;
        }

        #endregion
    }
}

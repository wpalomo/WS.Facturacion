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
    public class EstacionConfiguracionDT
    {
        #region Config: Clase de Datos de Config.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Estaciones definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configuracion de Estaciones</returns>
        /// ***********************************************************************************************
        public static EstacionConfiguracion getConfig(Conexion oConn, int? xiCodEst)
        {
            EstacionConfiguracion oConfig = new EstacionConfiguracion();           
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionEstacion_GetConfiguracionEstacion";
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = xiCodEst;
            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            if (oDR.Read())
            {
                // Esta el registro, pero ahora miramos que este con datos
                if (oDR["con_codes"] != DBNull.Value)
                {
                    oConfig = CargarConfig(oDR);
                }
                else
                {
                    //No habia datos
                    oConfig = LimpiarConfig();
                }
            }
            else
            {
                //No habia datos
                oConfig = LimpiarConfig();
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oConfig;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Genera un elemento vacío
        /// </summary>
        /// <returns>Objeto Configuracion de Estaciones vacío</returns>
        /// ***********************************************************************************************
        private static EstacionConfiguracion LimpiarConfig()
        {
            EstacionConfiguracion oConfig = new EstacionConfiguracion();

            oConfig.TicketAdAsc = 0;
            oConfig.TicketAdDesc = 0;
            oConfig.DiscrepanciaAlarma = 0;
            oConfig.ViaDecalada = null;
            oConfig.IdEstacion = 0;
            oConfig.PrimerCarrilalaIzquierda = "S";
            oConfig.AscendenteHaciaArriba = "S";
            oConfig.SentidoAsc = "";
            oConfig.SentidoDesc = "";
            oConfig.TiempoViaInactiva = 0;
            oConfig.esEstacionConTesorero = false;
            oConfig.PlazaAntena = "0000";
            oConfig.BeaconId = "000000";

            return oConfig;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Configuracion de Estaciones
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Config</param>
        /// <returns>Lista con el elmento Configuracion de Estaciones de la base de datos</returns>
        /// ***********************************************************************************************
        private static EstacionConfiguracion CargarConfig(IDataReader oDR)
        {
            EstacionConfiguracion oConfig = new EstacionConfiguracion();

            oConfig.IdEstacion = (byte)(oDR["con_codes"]);
            oConfig.ViaDecalada = Util.DbValueToNullable<byte>(oDR["con_decal"]);
            oConfig.PrimerCarrilalaIzquierda = oDR["con_izqui"].ToString();
            oConfig.AscendenteHaciaArriba = oDR["con_ascar"].ToString();
            oConfig.SentidoAsc = (string)oDR["con_ascen"];
            oConfig.SentidoDesc = (string)oDR["con_desce"];
            oConfig.esEstacionConTesorero = false;
            oConfig.PlazaAntena = Convert.ToString(oDR["con_pztag"]);
            oConfig.BeaconId = Convert.ToString(oDR["con_beacn"]);
            if (oDR["con_tevtr"] != DBNull.Value)
            {
                oConfig.TiempoViaInactiva = Convert.ToInt16(oDR["con_tevtr"]);
            }
            if (oDR["con_pordi"] != DBNull.Value)
            {
                oConfig.DiscrepanciaAlarma = Convert.ToDouble(oDR["con_pordi"]);
            }
            if (oDR["con_tickAdAsc"] != DBNull.Value)
            {
                oConfig.TicketAdAsc = (byte)(oDR["con_tickAdAsc"]);
            }
            if (oDR["con_tickAdDesc"] != DBNull.Value)
            {
                oConfig.TicketAdDesc = (byte)(oDR["con_tickAdDesc"]);
            }

            return oConfig;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion de Estacion en la base de datos
        /// </summary>
        /// <param name="oConfig">Configuracion de Estaciones - Objeto con la informacion de la configuracion de Estacion a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfig(EstacionConfiguracion oConfig, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionEstacion_updConfiguracionEstacion";

            oCmd.Parameters.Add("@codes", SqlDbType.Int).Value = oConfig.IdEstacion;
            oCmd.Parameters.Add("@ascen", SqlDbType.VarChar, 50).Value = oConfig.SentidoAsc;
            oCmd.Parameters.Add("@desce", SqlDbType.VarChar, 50).Value = oConfig.SentidoDesc;
            oCmd.Parameters.Add("@decal", SqlDbType.Int).Value = oConfig.ViaDecalada;
            oCmd.Parameters.Add("@tevtr", SqlDbType.Int).Value = oConfig.TiempoViaInactiva;
            oCmd.Parameters.Add("@pordi", SqlDbType.Int).Value = oConfig.DiscrepanciaAlarma;
            oCmd.Parameters.Add("@tickAdAsc", SqlDbType.Int).Value = oConfig.TicketAdAsc;
            oCmd.Parameters.Add("@tickAdDesc", SqlDbType.Int).Value = oConfig.TicketAdDesc;
            oCmd.Parameters.Add("@izqui", SqlDbType.Char,1).Value = oConfig.PrimerCarrilalaIzquierda;
            oCmd.Parameters.Add("@ascar", SqlDbType.Char, 1).Value = oConfig.AscendenteHaciaArriba;
            oCmd.Parameters.Add("@ConTes", SqlDbType.Char, 1).Value = oConfig.EstacionConTesorero;
            oCmd.Parameters.Add("@pzant", SqlDbType.Char, 4).Value = oConfig.PlazaAntena;
            oCmd.Parameters.Add("@beacn", SqlDbType.Char, 6).Value = oConfig.BeaconId;

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
                    msg = Traduccion.Traducir("No existe el registro de la Configuración de la estación");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion

        #region ConfigAlarma: Clase de Datos de Configuracion Alarma.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Configuracion de Alarmas definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Configuracion de Alarmas</returns>
        /// ***********************************************************************************************
        public static AlarmaConfiguracionL getConfigAlarma(Conexion oConn)
        {
            AlarmaConfiguracionL oConfigaAlarmaL = new AlarmaConfiguracionL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionAlarma_GetConfiguracionAlarma";

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oConfigaAlarmaL.Add(CargarConfigAlarma(oDR));
            } 

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oConfigaAlarmaL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Configuracion de Alarma
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de CFGALA</param>
        /// <returns>Lista con el elmento Configuracion de Alarmas de la base de datos</returns>
        /// ***********************************************************************************************
        private static AlarmaConfiguracion CargarConfigAlarma(System.Data.IDataReader oDR)
        {
            AlarmaConfiguracion oConfigAlarma = new AlarmaConfiguracion();

            oConfigAlarma.AnulacionTeclado = oDR["cfg_tecla"].ToString();
            oConfigAlarma.DuracionAlarmaVisual = (Int16)(oDR["cfg_durop"]);
            oConfigAlarma.DuracionSirena = (Int16)(oDR["cfg_durau"]);
            oConfigAlarma.TipoAlarma = (String)(oDR["cfg_tipo"]);
            oConfigAlarma.TipoSonido = (Int16)(oDR["cfg_tipso"]);

            return oConfigAlarma;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Configuracion de Alarmas en la base de datos
        /// </summary>
        /// <param name="oConfig">Configuracion de Alarmas - Objeto con la informacion de la configuracion de Alarmas a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updConfigAlarma(AlarmaConfiguracion oConfigAlarma, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_ConfiguracionAlarma_updConfiguracionAlarma";

            oCmd.Parameters.Add("@cfg_tipo", SqlDbType.Char, 1).Value = oConfigAlarma.TipoAlarma;
            oCmd.Parameters.Add("@cfg_durau", SqlDbType.SmallInt).Value = oConfigAlarma.DuracionSirena;
            oCmd.Parameters.Add("@cfg_durop", SqlDbType.SmallInt).Value = oConfigAlarma.DuracionAlarmaVisual;
            oCmd.Parameters.Add("@cfg_tipso", SqlDbType.SmallInt).Value = oConfigAlarma.TipoSonido;
            oCmd.Parameters.Add("@cfg_tecla", SqlDbType.Char, 1).Value = oConfigAlarma.AnulacionTeclado;


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
                    msg = Traduccion.Traducir("No existe el registro de la Configuración de Alarma");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion

        #region DisplayDT: Clase de Datos de Moneda.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Mensajes Display definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Codigo">String - Codigo de Mensaje Display a filtrar</param>
        /// <param name="Sentido">String - Sentido a filtrar</param>
        /// <returns>Lista de Mensajes del Display</returns>
        /// ***********************************************************************************************
        public static DisplayL getMensajesDisplay(Conexion oConn, string Codigo, string Sentido)
        {
            DisplayL oDisplay = new DisplayL();
            SqlDataReader oDR;
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_MensajeDisplay_getMensajeDisplay";

            oCmd.Parameters.Add("@dis_senti", SqlDbType.Char, 1).Value = Sentido;
            oCmd.Parameters.Add("@dis_codig", SqlDbType.Char, 3).Value = Codigo;

            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                oDisplay.Add(CargarDisplay(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oDisplay;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Mensajes de Display
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Displa</param>
        /// <returns>Lista con el elemento Display de la base de datos</returns>
        /// ***********************************************************************************************
        private static Display CargarDisplay(System.Data.IDataReader oDR)
        {
            return new Display(oDR["dis_senti"].ToString(), 
                                           getDescripcionSentido(oDR["dis_senti"].ToString()),
                                           oDR["dis_codig"].ToString(), 
                                           getDescripcionCodigo(oDR["dis_codig"].ToString()),
                                           oDR["dis_texto"].ToString(),
                                           oDR["dis_activ"].ToString(),
                                           oDR["dis_titil"].ToString(),
                                           oDR["dis_ancha"].ToString(),
                                           new DisplayEfectos(oDR["dis_efect"].ToString(), getDescripcionEfecto(oDR["dis_efect"].ToString())),
                                           new DisplayVelocidad((byte)(oDR["dis_veloc"]), getDescripcionVelocidad((byte)(oDR["dis_veloc"]))));
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un mensaje de Display en la base de datos
        /// </summary>
        /// <param name="oDisplay">Display - Objeto con la informacion del mensaje de Display a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMensajeDisplay(Display oDisplay, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_MensajeDisplay_updMensajeDisplay";

            oCmd.Parameters.Add("@dis_senti", SqlDbType.Char, 1).Value = oDisplay.CodigoSentido;
            oCmd.Parameters.Add("@dis_codig", SqlDbType.Char, 3).Value = oDisplay.IdCodigo;
            oCmd.Parameters.Add("@dis_texto", SqlDbType.VarChar, 100).Value = oDisplay.Texto;
            oCmd.Parameters.Add("@dis_activ", SqlDbType.Char, 1).Value = oDisplay.Activo;
            oCmd.Parameters.Add("@dis_efect", SqlDbType.Char, 1).Value = oDisplay.Efectos.Codigo;
            oCmd.Parameters.Add("@dis_titil", SqlDbType.Char, 1).Value = oDisplay.Titilante;
            oCmd.Parameters.Add("@dis_veloc", SqlDbType.SmallInt).Value = oDisplay.Velocidad.Codigo;
            oCmd.Parameters.Add("@dis_ancha", SqlDbType.Char, 1).Value = oDisplay.LetraAncha;

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
                    msg = Traduccion.Traducir("No existe el registro del Mensaje de Display");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido del Sentido
        /// </summary>
        /// <param name="codigo">int - Codigo del Sentido que deseo devolver como texto</param>
        /// <returns>El texto traducido del Sentido</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionSentido(string codigo)
        {
            string retorno = string.Empty;

            switch (codigo)
            {
                case "A":
                    retorno = "Ascendente";
                    break;
                case "D":
                    retorno = "Descendente";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }
   
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido del Codigo del Tipo de mensaje
        /// </summary>
        /// <param name="codigo">int - Codigo que deseo devolver como texto</param>
        /// <returns>El texto traducido del Codigo del Tipo de mensaje</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionCodigo(string codigo)
        {
            string retorno = string.Empty;

            switch (codigo)
            {
                case "MXP":
                    retorno = "Máxima Prioridad";
                    break;
                case "BNV":
                    retorno = "Bienvenida";
                    break;
                case "CAT":
                    retorno = "Categorización";
                    break;
                case "PAG":
                    retorno = "Pagado";
                    break;
                case "CER":
                    retorno = "Via Cerrada";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }
        
        #endregion

        #region VELOCIDAD: Clase de Datos de VELOCIDAD.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto Velocidad. 
        /// </summary>
        /// <param name="codigo">int - Codigo de la velocidad que deseo devolver como texto</param>
        /// <returns>Objeto de Velocidad</returns>
        /// ***********************************************************************************************
        public static DisplayVelocidadL getVelocidades()
        {
            DisplayVelocidadL oVelocidadL = new DisplayVelocidadL();
            
            for (byte i = 1; i <= 3; i++)
            {
                oVelocidadL.Add(new DisplayVelocidad(i, getDescripcionVelocidad(i)));
            }

            return oVelocidadL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido de la Velocidad
        /// </summary>
        /// <param name="codigo">int - Codigo de la Velocidad que deseo devolver como texto</param>
        /// <returns>El texto traducido de la Velocidad</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionVelocidad(byte codigo)
        {
            string retorno = string.Empty;

            switch (codigo)
            {
                case 1:
                    retorno = "Lenta";
                    break;
                case 2:
                    retorno = "Media";
                    break;
                case 3:
                    retorno = "Rápida";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }
        
        #endregion

        #region EFECTOS: Clase de Datos de EFECTOS.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un objeto Efecto. 
        /// </summary>
        /// <param name="codigo">int - Codigo del Efecto que deseo devolver como texto</param>
        /// <returns>Objeto de Efecto</returns>
        /// ***********************************************************************************************
        public static DisplayEfectosL getEfectos()
        {
            DisplayEfectosL oEfectoL = new DisplayEfectosL();

            oEfectoL.Add(new DisplayEfectos("F", getDescripcionEfecto("F")));
            oEfectoL.Add(new DisplayEfectos("C", getDescripcionEfecto("C")));
            oEfectoL.Add(new DisplayEfectos("R", getDescripcionEfecto("R")));

            return oEfectoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el texto traducido del Efecto
        /// </summary>
        /// <param name="codigo">int - Codigo del Efecto que deseo devolver como texto</param>
        /// <returns>El texto traducido deL Efecto</returns>
        /// ***********************************************************************************************
        protected static string getDescripcionEfecto(string codigo)
        {
            string retorno = string.Empty;

            switch (codigo)
            {
                case "F":
                    retorno = "Fijo";
                    break;
                case "C":
                    retorno = "Circular";
                    break;
                case "R":
                    retorno = "Renglones";
                    break;
            }

            return Traduccion.Traducir(retorno);
        }
        
        #endregion

        #region TURNOTRABAJO: Clase de Datos de los Turnos de Trabajo de la estacion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Turnos de Trabajo de la Estacion 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Estacion">byte - Numero de estacion de la que deseo devolver los turnos</param>
        /// <param name="Turno">byte - Numero de turno que deseo devolver</param>
        /// <returns>Lista de Turnos de trabajo</returns>
        /// ***********************************************************************************************
        public static TurnoTrabajoL getTurnosTrabajo(Conexion oConn, int? Estacion, byte? Turno)
        {
            TurnoTrabajoL oTurnoTrabajoL = new TurnoTrabajoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TurnosTrabajo_getTurnosTrabajo";
            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = Estacion;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = Turno;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTurnoTrabajoL.Add(CargarTurno(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTurnoTrabajoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el numero del proximo turno a generar (el maximo numero de turno no eliminado mas uno)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Turnos de trabajo</returns>
        /// ***********************************************************************************************
        public static byte getProximoNumeroTurno(Conexion oConn, int Estacion)
        {
            byte ProximoTurno = 0;
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TurnosTrabajo_getProximoNumeroTurno";
            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = Estacion;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                ProximoTurno = (byte)oDR["ProximoTurno"];
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return ProximoTurno;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Turnos de Trabajo
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Turnos de Trabajo</param>
        /// <returns>Lista con los elementos de Turno de Trabajo de la base de datos</returns>
        /// ***********************************************************************************************
        private static TurnoTrabajo CargarTurno(System.Data.IDataReader oDR)
        {
            return new TurnoTrabajo(new Estacion((byte)oDR["tes_coest"], ""), 
                                                          (byte)oDR["tes_testu"],
                                                          oDR["tes_horai"].ToString(),
                                                          (short)oDR["tes_toler"],
                                                          (short)oDR["tes_tolpo"]);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Turno de trabajo en la base de datos
        /// </summary>
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto con la informacion del Turno de Trabajo a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTurnoTrabajo(TurnoTrabajo oTurnoTrabajo, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TurnosTrabajo_addTurnoTrabajo";

            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = oTurnoTrabajo.Estacion.Numero;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oTurnoTrabajo.NumeroTurno;
            oCmd.Parameters.Add("@HoraInicial", SqlDbType.Char, 5).Value = oTurnoTrabajo.HoraString;
            oCmd.Parameters.Add("@ToleranciaAnterior", SqlDbType.SmallInt).Value = oTurnoTrabajo.ToleranciaAnterior;
            oCmd.Parameters.Add("@ToleranciaPosterior", SqlDbType.SmallInt).Value = oTurnoTrabajo.ToleranciaPosterior;             
                
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
                    msg = Traduccion.Traducir("Este Turno de Trabajo NO se encontraba Eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Turno de trabajo de la base de datos
        /// </summary>
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto con la informacion del Turno de Trabajo a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updTurnoTrabajo(TurnoTrabajo oTurnoTrabajo, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TurnosTrabajo_updTurnoTrabajo";

            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = oTurnoTrabajo.Estacion.Numero;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oTurnoTrabajo.NumeroTurno;
            oCmd.Parameters.Add("@HoraInicial", SqlDbType.Char, 5).Value = oTurnoTrabajo.HoraString;
            oCmd.Parameters.Add("@ToleranciaAnterior", SqlDbType.SmallInt).Value = oTurnoTrabajo.ToleranciaAnterior;
            oCmd.Parameters.Add("@ToleranciaPosterior", SqlDbType.SmallInt).Value = oTurnoTrabajo.ToleranciaPosterior;             

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
                    msg = Traduccion.Traducir("No existe el registro del Turno de Trabajo");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Turno de Trabajo de la base de datos
        /// </summary >
        /// <param name="oTurnoTrabajo">TurnoTrabajo - Objeto que contiene el Turno de Trabajo a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTurnoTrabajo(TurnoTrabajo oTurnoTrabajo, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_TurnosTrabajo_delTurnoTrabajo";

            oCmd.Parameters.Add("@Estacion", SqlDbType.TinyInt).Value = oTurnoTrabajo.Estacion.Numero;
            oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oTurnoTrabajo.NumeroTurno;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != (int)EjecucionSP.enmErrorSP.enmOK)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == (int)EjecucionSP.enmErrorSP.enmINCONSISTENCIA)
                {
                    msg = Traduccion.Traducir("No existe el registro del Turno de Trabajo");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}
    
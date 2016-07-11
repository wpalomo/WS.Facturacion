using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Tesoreria
{
    public class PartesDt
    {
        #region PARTES: Clase de Datos de PartesDt.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Partes de un peajista en un periodo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="operador">string - ID del peajista</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="status">string - Status de los Partes</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static ParteL getPartes(Conexion oConn, int estacion, DateTime jornadaDesde, DateTime jornadaHasta, string operador, int? turnoDesde, int? turnoHasta, string status)
        {
            ParteL oPartes = new ParteL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_GetPartes";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@TipoParte", SqlDbType.Char, 1).Value = status;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPartes.Add(CargarParte(oDR, true));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPartes;
        }

		/// <summary>
		/// Agrega un cruce de dinero.
		/// </summary>
		/// <param name="oConn"></param>
		/// <param name="coest"></param>
		/// <param name="usuario"></param>
		/// <param name="parteOrigen"></param>
		/// <param name="parteDestino"></param>
		/// <param name="monto"></param>
		/// <param name="comentario"></param>
		/// <returns></returns>
		public static bool addCruceDinero(Conexion oConn,int coest,string usuario,int parteOrigen,int parteDestino,decimal monto, string comentario)
		{
			bool result =true;
			try
			{
				SqlDataReader oDR;

				// Creamos, cargamos y ejecutamos el comando
				SqlCommand oCmd = new SqlCommand();
				oCmd.Connection = oConn.conection;
				oCmd.Transaction = oConn.transaction;

				oCmd.CommandType = CommandType.StoredProcedure;
				oCmd.CommandText = "Tesoreria.usp_CrucesDinero_addCruceDinero";
				oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = coest;
				oCmd.Parameters.Add("@parteOrigen", SqlDbType.Int).Value = parteOrigen;
				oCmd.Parameters.Add("@parteDestino", SqlDbType.Int).Value = parteDestino;
				oCmd.Parameters.Add("@idValidador", SqlDbType.VarChar, 10).Value = usuario;
				oCmd.Parameters.Add("@monto", SqlDbType.SmallMoney).Value = monto;
				oCmd.Parameters.Add("@Comentario", SqlDbType.VarChar,100).Value = comentario;
				
				oDR = oCmd.ExecuteReader();
				

				// Cerramos el objeto
				oCmd = null;
				oDR.Close();
			}
			catch (Exception ex)
			{
				result = false;
				throw ex;
			}
			return result;
		}

		public static bool delCruceDinero(Conexion oConn, int coest, int identity)
		{
			bool result = true;
			try
			{
				SqlDataReader oDR;

				// Creamos, cargamos y ejecutamos el comando
				SqlCommand oCmd = new SqlCommand();
				oCmd.Connection = oConn.conection;
				oCmd.Transaction = oConn.transaction;

				oCmd.CommandType = CommandType.StoredProcedure;
				oCmd.CommandText = "Tesoreria.usp_CrucesDinero_delCruceDinero";
				oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = coest;
				oCmd.Parameters.Add("@ident", SqlDbType.Int).Value = identity;

				oDR = oCmd.ExecuteReader();


				// Cerramos el objeto
				oCmd = null;
				oDR.Close();
			}
			catch (Exception ex)
			{
				result = false;
				throw ex;
			}
			return result;
			
		}
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un Parte 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="parte">int - Numero Parte</param>
        /// <returns>Parte</returns>
        /// ***********************************************************************************************
        public static Parte getParte(Conexion oConn, int estacion, int parte)
        {
            Parte oParte = null;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_GetParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oParte = CargarParte(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oParte;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static Parte CargarParte(IDataReader oDR)
        {
            return CargarParte(oDR, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Partes
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <param name="conBloque"bool - true para incluir datos del bloque asociado</param>
        /// <returns>elemento Parte</returns>
        /// ***********************************************************************************************
        private static Parte CargarParte(IDataReader oDR, bool conBloque)
        {
            Parte oParte = new Parte();
            oParte.Numero = (int)oDR["par_parte"];
            oParte.Jornada = (DateTime)oDR["par_fejor"];
            oParte.Estacion = new Estacion((Byte)oDR["par_estac"], oDR["est_nombr"].ToString());
            oParte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S");
            oParte.EstaValidado = (oDR["pvr_valid"].ToString() == "S");
            oParte.DevolvioFondo = (oDR["par_fondo"].ToString() == "S");
            oParte.Apertura = (DateTime)oDR["par_feape"];
            if (oParte.EstaLiquidado && !(oDR["par_feliq"] is System.DBNull))
            {
                oParte.Liquidacion = (DateTime)oDR["par_feliq"];
            }
            oParte.Turno = (Byte)oDR["par_testu"];
            oParte.Nivel = (Byte)oDR["par_nivel"];
            oParte.Peajista = new Usuario(oDR["par_id"].ToString(), oDR["Peajista"].ToString());
            oParte.Validador = new Usuario(oDR["pvr_idval"].ToString(), oDR["Validador"].ToString());
            oParte.ModoMantenimiento = (oDR["par_mante"].ToString() == "S");
            oParte.HoraI = oDR["tes_horai"].ToString();
            oParte.HoraF = oDR["tes_horaf"].ToString();

            if (conBloque)
            {
                if (oDR["par_nuvia"] != DBNull.Value)
                {
                    oParte.Via = (Byte)oDR["par_nuvia"];
                    oParte.ViaNombre = oDR["via_nombr"].ToString();
                    oParte.Bloque = (int)oDR["par_nturn"];

                    // Puede darse el caso de que no tengo apertura ni cierre? 
                    // Se da un caso en donde estos datos vienen vacios, asi que 
                    // Para evitar que pinche ponemos la siguiente comprobacion:
                    if (oDR["tur_fecap"] != DBNull.Value)
                    {
                        oParte.AperturaBloque = (DateTime)oDR["tur_fecap"];
                        oParte.CierreBloque = (DateTime)oDR["tur_fecci"];
                    }

                    oParte.Transito = (int)oDR["Transito"];
                }
            }
            return oParte;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes pendientes (no liquidados o no validados)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="parte">int? - Numero de Parte</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="zona">int? - Codigo de Zona</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="operador">string - id de Operador</param>
        /// <param name="tipoParte">string - N sin liquidar
        ///                                  V sin validar</param>
        /// ***********************************************************************************************
        public static ParteL getPartesPendientes(Conexion oConn, int? parte, DateTime? jornadaDesde, DateTime? jornadaHasta, int? zona, int? estacion, int? turnoDesde, int? turnoHasta, string operador, string tipoParte)
        {
            ParteL oPartes = new ParteL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_RptRecaudacion_PartesPendientes";
                oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = parte;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;
                oCmd.Parameters.Add("@TipoParte", SqlDbType.Char, 1).Value = tipoParte;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPartes.Add(CargarParte(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// lista de partes para liquidacion (los del periodo o los nno liquidados del operador)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="jornadaDesde">DateTime? - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime? - Jornada Hasta</param>
        /// <param name="estacion">int? - Codigo de Estacion</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="operador">string - id de Operador</param>
        /// ***********************************************************************************************
        public static ParteL getPartesLiquidacion(Conexion oConn, DateTime? jornadaDesde, DateTime? jornadaHasta, int? estacion, int? turnoDesde, int? turnoHasta, string operador, int? cantidadMaxima)
        {
            ParteL oPartes = new ParteL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_getPartesLiquidacion";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@idope", SqlDbType.VarChar, 10).Value = operador;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPartes.Add(CargarParte(oDR, true));
                    if (oPartes.Count >= cantidadMaxima)
                    {
                        break;
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

            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los peajistas que trabajaron en un periodo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <param name="status">string - Status de los Partes</param>
        /// <returns>Lista de Partes</returns>
        /// ***********************************************************************************************
        public static UsuarioL getOperadores(Conexion oConn, int estacion, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta, string status)
        {
            UsuarioL oUsuarios = new UsuarioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_GetOperadores";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;
                oCmd.Parameters.Add("@TipoParte", SqlDbType.Char, 1).Value = status;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oUsuarios.Add(CargarOperador(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oUsuarios;
        }



        public static UsuarioL getOperadores(Conexion oConn, int estacion)
        {
            UsuarioL oUsuarios = new UsuarioL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "usp_GetOperadores";
                oCmd.Parameters.Add("@Plaza", SqlDbType.TinyInt).Value = estacion;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oUsuarios.Add(CargarOperador(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oUsuarios;
        }

		public static MovimientoCajaAjusteL getCrucesDinero(Conexion oConn, int estacion, DateTime? desde, DateTime? hasta)
		{
			MovimientoCajaAjusteL ajustes = new MovimientoCajaAjusteL();
			try
			{
				SqlDataReader oDR;

				// Creamos, cargamos y ejecutamos el comando
				SqlCommand oCmd = new SqlCommand();
				oCmd.Connection = oConn.conection;
				oCmd.Transaction = oConn.transaction;

				oCmd.CommandType = CommandType.StoredProcedure;
				oCmd.CommandText = "Tesoreria.usp_CrucesDinero_getCrucesDinero";
				oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = estacion;
				oCmd.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = desde;
				oCmd.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = hasta;
				
				oDR = oCmd.ExecuteReader();
				while (oDR.Read())
				{
					ajustes.Add(CargarAjustes(oDR));
				}

				// Cerramos el objeto
				oCmd = null;
				oDR.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return ajustes;
		}

		private static MovimientoCajaAjuste CargarAjustes(SqlDataReader oDR)
		{
			MovimientoCajaAjuste mov = new MovimientoCajaAjuste();
			mov.identity = Convert.ToInt32(oDR["cdi_ident"]);
			mov.fecha = Convert.ToDateTime(oDR["cdi_fecha"]);
			mov.monto = Convert.ToDecimal(oDR["cdi_monto"]);
			mov.validador = oDR["cdi_usuario"].ToString();
			mov.comentario = oDR["cdi_coment"].ToString();
			mov.parteOrigen = Convert.ToInt32(oDR["cdi_origen"]);
			mov.parteDestino = Convert.ToInt32(oDR["cdi_destino"]);
			mov.mocOrigen = Convert.ToInt32(oDR["cdi_mocOrigen"]);
			mov.mocDestino = Convert.ToInt32(oDR["cdi_mocDestino"]);
			mov.estadoJornadaOrigen = Convert.ToChar(oDR["cdi_estadoJornadaOrigen"]);
			mov.estadoJornadaOrigen = Convert.ToChar(oDR["cdi_estadoJornadaDestino"]);
			mov.reposicionOrigen = Convert.ToChar(oDR["cdi_reposicionOrigen"]);
			mov.reposicionDestino = Convert.ToChar(oDR["cdi_reposicionDestino"]);
			return mov;
		}
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Usuarios
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Usuarios</param>
        /// <returns>elemento Usuario</returns>
        /// ***********************************************************************************************
        private static Usuario CargarOperador(IDataReader oDR)
        {
            Usuario oUsuario = new Usuario();
            oUsuario.ID = oDR["par_id"].ToString();
            oUsuario.Nombre = oDR["use_nombr"].ToString();
            oUsuario.ID_Nombre = oUsuario.ID + " - " + oUsuario.Nombre;

            return oUsuario;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los peajistas habilitados con el parte que tuvieron en un turno
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornada">DateTime - Jornada Desde</param>
        /// <param name="turno">int? - Turno Desde</param>
        /// <returns>Lista de UsuarioParte</returns>
        /// ***********************************************************************************************
        public static UsuarioParteL getOperadoresParte(Conexion oConn, int estacion, DateTime jornada, int? turno)
        {
            UsuarioParteL oUsuarios = new UsuarioParteL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_GetOperadoresParte";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = jornada;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = turno;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oUsuarios.Add(CargarOperadorParte(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oUsuarios;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de UsuarioParte
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Usuarios</param>
        /// <param name="bPuede">bool Puede Asignar (jornada abierta, con permisos)</param>
        /// <returns>elemento UsuarioParte</returns>
        /// ***********************************************************************************************
        private static UsuarioParte CargarOperadorParte(IDataReader oDR)
        {
            UsuarioParte oUsuario = new UsuarioParte();
            oUsuario.Usuario = new Usuario(oDR["use_id"].ToString(), oDR["use_nombr"].ToString());
            oUsuario.Usuario.PerfilActivo = new Perfil(oDR["use_grupo"].ToString(), oDR["gru_visua"].ToString());
            oUsuario.Usuario.Eliminado = (oDR["use_delet"].ToString() == "S");
            oUsuario.Usuario.FechaEgreso = Util.DbValueToNullable<DateTime>(oDR["use_feegr"]);

            if (oDR["par_parte"] != DBNull.Value)
            {
                oUsuario.Parte = new Parte();
                oUsuario.Parte.Numero = (int)oDR["par_parte"];
                oUsuario.Parte.Jornada = (DateTime)oDR["par_fejor"];
                oUsuario.Parte.Estacion = new Estacion((Byte)oDR["est_codig"], oDR["est_nombr"].ToString());
                oUsuario.Parte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S");
                oUsuario.Parte.EstaValidado = (oDR["pvr_valid"].ToString() == "S");
                oUsuario.Parte.Turno = (Byte)oDR["par_testu"];
                oUsuario.Parte.Peajista = oUsuario.Usuario;
                oUsuario.Parte.ModoMantenimiento = (oDR["par_mante"].ToString() == "S");
            }
            oUsuario.PuedeAsignarParte = (oUsuario.Parte == null)
                                            && !oUsuario.Usuario.Eliminado
                                            && (oUsuario.Usuario.FechaEgreso == null || oUsuario.Usuario.FechaEgreso >= DateTime.Today);
            return oUsuario;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Asigna un Parte
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oParte">Parte - Objeto con la informacion del parte a insertar
        ///                     le asigna el numero de parte</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addParte(Conexion oConn, Parte oParte, string Usuario)
        {
            bool ret = false;
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_addParte";

                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
                oCmd.Parameters.Add("@id", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                oCmd.Parameters.Add("@Jornada", SqlDbType.DateTime).Value = oParte.Jornada.Date;
                oCmd.Parameters.Add("@Turno", SqlDbType.TinyInt).Value = oParte.Turno;
                //lo pone el servidor
                //oCmd.Parameters.Add("@feape", SqlDbType.DateTime).Value = oParte.Apertura;
                oCmd.Parameters.Add("@mante", SqlDbType.Char, 1).Value = oParte.ModoMantenimiento ? "S" : "N";
                oCmd.Parameters.Add("@usumo", SqlDbType.VarChar, 10).Value = Usuario;

                SqlParameter parNumero = oCmd.Parameters.Add("@Parte", SqlDbType.Int);
                parNumero.Direction = ParameterDirection.Output;

                SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
                parRetVal.Direction = ParameterDirection.ReturnValue;

                oCmd.ExecuteNonQuery();
                int retval = (int)parRetVal.Value;
                if (parNumero.Value != DBNull.Value)
                {
                    oParte.Numero = Convert.ToInt32(parNumero.Value);
                }
                // Cerramos el objeto
                oCmd = null;

                // Revisamos el retorno del SP. 
                if (retval != 0)
                {
                    string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                    if (retval != -102)
                    {
                        throw new ErrorSPException(msg);
                    }
                }
                else
                {
                    ret = true;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza para cada parte el campo que indica si se mostró o no la advertencia de diferencia entre lo
        /// liquidado y lo facturado.
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static bool updPartesMostroDiferencia(Conexion oConn, int estacion, int parte)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = CommandType.StoredProcedure;
            oCmd.CommandText = "Tesoreria.usp_Partes_updMostroDiferencia";

            oCmd.Parameters.Add("@par_estac", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@par_parte", SqlDbType.Int, 10).Value = parte;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al actualizar el registro ") + retval.ToString();
                if (retval != -102)
                {
                    throw new ErrorSPException(msg);
                }
                return false;
            }

            return true;
        }

        #endregion

        #region PARTESTOTALES: Obtener el total de partes por estado.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el total de partes en cada estado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Estacion</param>
        /// <param name="jornadaDesde">DateTime - Jornada Desde</param>
        /// <param name="jornadaHasta">DateTime - Jornada Hasta</param>
        /// <param name="turnoDesde">int? - Turno Desde</param>
        /// <param name="turnoHasta">int? - Turno Hasta</param>
        /// <returns>Lista de Total de Partes</returns>
        /// ***********************************************************************************************
        public static PartesTotalesL getPartesTotales(Conexion oConn, int estacion, DateTime jornadaDesde, DateTime jornadaHasta, int? turnoDesde, int? turnoHasta)
        {
            PartesTotalesL oPartes = new PartesTotalesL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = CommandType.StoredProcedure;
                oCmd.CommandText = "Tesoreria.usp_Partes_GetPartesTotales";
                oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@JornadaDesde", SqlDbType.DateTime).Value = jornadaDesde;
                oCmd.Parameters.Add("@JornadaHasta", SqlDbType.DateTime).Value = jornadaHasta;
                oCmd.Parameters.Add("@TurnoIni", SqlDbType.TinyInt).Value = turnoDesde;
                oCmd.Parameters.Add("@TurnoFin", SqlDbType.TinyInt).Value = turnoHasta;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oPartes.Add(CargarPartesTotales(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oPartes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de PartesTotales
        /// </summary>
        /// <param name="oDR">IDataReader - Objeto DataReader de la tabla de Partes</param>
        /// <returns>elemento PartesTotales</returns>
        /// ***********************************************************************************************
        private static PartesTotales CargarPartesTotales(IDataReader oDR)
        {
            PartesTotales oParte = new PartesTotales();
            oParte.Jornada = (DateTime)oDR["par_fejor"];
            oParte.EstaLiquidado = (oDR["par_liqui"].ToString() == "S");
            oParte.EstaValidado = (oDR["pvr_valid"].ToString() == "S");
            oParte.DevolvioFondo = (oDR["par_fondo"].ToString() == "S");
            oParte.Turno = (Byte)oDR["par_testu"];
            oParte.Cantidad = (int)oDR["Cantidad"];

            return oParte;
        }




		public static void obtenerFaltanteParte(Conexion oConn, int estacion, int parte, out decimal sobrante,
		                                        out decimal faltante, out decimal reposicion, out bool jornadaCerrada)
		{

			sobrante = 0;
			faltante = 0;
			reposicion = 0;
			jornadaCerrada = true;
			try
			{
				SqlDataReader oDR;

				// Creamos, cargamos y ejecutamos el comando
				SqlCommand oCmd = new SqlCommand();
				oCmd.Connection = oConn.conection;
				oCmd.Transaction = oConn.transaction;

				oCmd.CommandType = CommandType.StoredProcedure;
				oCmd.CommandText = "Tesoreria.usp_CrucesDinero_getTotalesPartes";
				oCmd.Parameters.Add("@estac", SqlDbType.TinyInt).Value = estacion;
				oCmd.Parameters.Add("@parte", SqlDbType.Int).Value = parte;
				
				oDR = oCmd.ExecuteReader();


				if (oDR.Read())
				{
					sobrante = Convert.ToDecimal(oDR["Sobrante"]);
					faltante = Convert.ToDecimal(oDR["Faltante"]);
					reposicion = Convert.ToDecimal(oDR["ReposicionEconomica"]);
					
					//if (Convert.ToChar(oDR["jornadaCerrada"])=='S')
					//{
					//    jornadaCerrada = true;
					//}
					//else
					//{
					//    jornadaCerrada = false;
					//}
				}

				// Cerramos el objeto
				oCmd = null;
				oDR.Close();
			}
			catch (Exception ex)
			{
				throw ex;
			}
			
		}
        #endregion
    }
}
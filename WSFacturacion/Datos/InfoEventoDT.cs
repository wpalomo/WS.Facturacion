using System;
using Microsoft;
using System.IO;
using System.Web;
using System.Linq;
using System.Text;
using System.Data;
using Telectronica.Errores;
using System.Globalization;
using System.Data.SqlClient;
using Telectronica.Facturacion;
using Telectronica.Utilitarios;
using System.Collections.Generic;

namespace Telectronica.Peaje
{
    public class InfoEventoDt
    {
        #region InfoEvento: Clase de Datos de InfoEvento.

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos
        /// </summary>
        /// <param name="oConn">objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiCodEst">Numero de estacion a filtrar</param>
        /// <param name="xdFechaDesde">Fecha Desde a filtrar</param>
        /// <param name="xdFechaHasta">Fecha Hasta a filtrar</param>
        /// <param name="xiMinIdent">Nro. de identidad a filtrar</param>
        /// <param name="xVias">Vias a filtrar</param>
        /// <param name="xTiposEventos">Tipos de eventos a filtrar</param>
        /// <param name="xiCantRows">Cantidad de filas a mostrar</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static InfoEventoL getInfoEventos(Conexion oConn, int xiCodEst, DateTime? xdFechaDesde, DateTime? xdFechaHasta, int? xiMinIdent, ViaL xVias, TipoEventoL xTiposEventos, int xiCantRows, ref bool llegoAlTope)
        {
            InfoEventoL oInfoEventos = new InfoEventoL();

            // SERIALIZA VIAS 
            string xmlVias = "";
            xmlVias = Utilitarios.xmlUtil.SerializeObject(xVias);

            // SERIALIZA EVENTOS
            string xmlEventos = "";
            xmlEventos = Utilitarios.xmlUtil.SerializeObject(xTiposEventos);

            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetEventos";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = xiCodEst;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = xdFechaDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = xdFechaHasta;
            oCmd.Parameters.Add("@minIdent", SqlDbType.Int).Value = xiMinIdent;
            oCmd.Parameters.Add("@xmlVias", SqlDbType.NText).Value = xmlVias;
            oCmd.Parameters.Add("@xmlTipom", SqlDbType.NText).Value = xmlEventos;
            oCmd.Parameters.Add("@usuario", SqlDbType.VarChar, 10).Value = (string)HttpContext.Current.Session["Permisos_Usuario"];
            oCmd.Parameters.Add("@CantRows", SqlDbType.Int).Value = xiCantRows;

            oCmd.CommandTimeout = 120;
            oDR = oCmd.ExecuteReader();
            int i = 0;
            while (oDR.Read())
            {
                i++;
                oInfoEventos.Add(CargarInfoEventos(oDR));

                //Verificamos de no pasarnos de xiCantRows para que no pinche el Silverlight
                if (i == xiCantRows)
                {
                    llegoAlTope = true;
                    break;
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oInfoEventos;
        }


        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de imagenes que pertenecen a un evento
        /// </summary>
        /// <param name="oConn">objeto de conexion a la base de datos correspondiente</param>
        /// ----------------------------------------------------------------------------------------------
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static ImagenL getImagenesCCOEstacion(Conexion oConn, int iCoest, int iNroVia, int iNroEvento, string sFotos)
        {
            ImagenL imagenes = new ImagenL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetImagenesCCOEstacion";

            oCmd.Parameters.Add("@evento", SqlDbType.Int).Value = iNroEvento;
            oCmd.Parameters.Add("@via", SqlDbType.Int).Value = iNroVia;
            oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = iCoest;
            oCmd.Parameters.Add("@SoloFotos", SqlDbType.Char, 1).Value = sFotos;

            oCmd.CommandTimeout = 120;
            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                imagenes.Add(cargarImagen(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return imagenes;
        }


        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Imagenes
        /// </summary>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static Imagen cargarImagen(System.Data.IDataReader oDR)
        {
            Imagen imagen = new Imagen();
            if (oDR["Ruta"] != DBNull.Value)
            {
                imagen.RutaFisica = oDR["Ruta"].ToString();
            }
            if (oDR["RutaURL"] != DBNull.Value)
            {
                imagen.RutaURL = oDR["RutaURL"].ToString();
            }
            if (oDR["trf_tipim"] != DBNull.Value)
            {
                imagen.TipoImagen = oDR["trf_tipim"].ToString();
            }
            if (oDR["trf_camar"] != DBNull.Value)
            {
                imagen.TipoCamara = oDR["trf_camar"].ToString();
            }
            if (oDR["trf_nombr"] != DBNull.Value)
            {
                imagen.Nombre = oDR["trf_nombr"].ToString();
            }
            if (oDR["Seleccion"] != DBNull.Value)
            {
                imagen.Seleccion = oDR["Seleccion"].ToString();
            }
            return imagen;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de imagenes que pertenecen a un evento
        /// </summary>
        /// <param name="oConn">objeto de conexion a la base de datos correspondiente</param>
        /// ----------------------------------------------------------------------------------------------
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static ImagenL getImagenes(Conexion oConn, int iCoest, int iNroVia, int iNroEvento, string sFotos)
        {
            ImagenL imagenes = new ImagenL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetImagenes";

            oCmd.Parameters.Add("@evento", SqlDbType.Int).Value = iNroEvento;
            oCmd.Parameters.Add("@via", SqlDbType.Int).Value = iNroVia;
            oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = iCoest;
            oCmd.Parameters.Add("@SoloFotos", SqlDbType.Char, 1).Value = sFotos;

            oCmd.CommandTimeout = 500;
            oDR = oCmd.ExecuteReader();

            while (oDR.Read())
            {
                imagenes.Add(cargarImagen(oDR));

            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return imagenes;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos para que el supervisor ingrese los comentarios
        /// </summary>
        /// <param name="oConn">objeto de conexion a la base de datos correspondiente</param>
        /// <param name="iCodEst">Numero de estacion a filtrar</param>
        /// <param name="dFechaDesde">Fecha Desde a filtrar</param>
        /// <param name="dFechaHasta">Fecha Hasta a filtrar</param>
        /// <param name="iMinIdent">Nro. de identidad a filtrar</param>
        /// <param name="vias">Vias a filtrar</param>
        /// <param name="tiposEventos">Tipos de eventos a filtrar</param>
        /// <param name="iCantRows">Cantidad de filas a mostrar</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static InfoEventoL getInfoEventosComentariosSup(Conexion oConn, int iCodEst, DateTime? dFechaDesde, DateTime? dFechaHasta, int? iMinIdent, ViaL vias, string sEstadoComent, int iCantRows, ref bool llegoAlTope)
        {
            InfoEventoL oInfoEventos = new InfoEventoL();

            // SERIALIZA VIAS 
            string xmlVias = "";
            xmlVias = Utilitarios.xmlUtil.SerializeObject(vias);
            
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetEventosComentariosSup";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = iCodEst;
            oCmd.Parameters.Add("@fecde", SqlDbType.DateTime).Value = dFechaDesde;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = dFechaHasta;
            oCmd.Parameters.Add("@minIdent", SqlDbType.Int).Value = iMinIdent;
            oCmd.Parameters.Add("@xmlVias", SqlDbType.NText).Value = xmlVias;
            oCmd.Parameters.Add("@EstadoComent", SqlDbType.Char, 1).Value = sEstadoComent;
            oCmd.Parameters.Add("@usuario", SqlDbType.VarChar, 10).Value = (string)HttpContext.Current.Session["Permisos_Usuario"];
            oCmd.Parameters.Add("@CantRows", SqlDbType.Int).Value = iCantRows;

            oCmd.CommandTimeout = 120;
            oDR = oCmd.ExecuteReader();
            int i = 0;
            while (oDR.Read())
            {
                i++;
                oInfoEventos.Add(CargarInfoEventos(oDR));

                //Verificamos de no pasarnos de xiCantRows para que no pinche el Silverlight
                if (i == iCantRows)
                {
                    llegoAlTope = true;
                    break;
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oInfoEventos;
        }

        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Carga un elemento DataReader en la lista de InfoEventos
        /// </summary>
        /// <param name="oDR">Objeto DataReader de la tabla</param>
        /// <returns>Lista de InfoEvento: InfoEventoL</returns>
        /// ----------------------------------------------------------------------------------------------
        private static InfoEvento CargarInfoEventos(System.Data.IDataReader oDR)
        {
            InfoEvento oInfoEvento = new InfoEvento();

            oInfoEvento.Fecha = Conversiones.edt_DateTime(oDR["eve_fecha"]);
            oInfoEvento.VideoFoto1 = Conversiones.edt_Str(oDR["tra_video"]);
            oInfoEvento.VideoFoto2 = Conversiones.edt_Str(oDR["tra_video2"]);
            oInfoEvento.TipoEvento = Conversiones.edt_Str(oDR["cla_tipom"]);
            oInfoEvento.DescTipoEvento = Conversiones.edt_Str(oDR["cla_descr"]);
            oInfoEvento.Manual = Conversiones.edt_Str(oDR["desc_manua"]);
            oInfoEvento.Dac = Conversiones.edt_Str(oDR["desc_dac"]);
            oInfoEvento.EjesLevantados = Conversiones.edt_Str(oDR["EjesLevantados"]);
            oInfoEvento.MontoTransito = Conversiones.edt_Decimal(oDR["Tarifa"]);

            oInfoEvento.SentidoCardinal = Conversiones.edt_Str(oDR["SentidoCardinal"]);
            if (Conversiones.edt_Str(oDR["eve_senti"]) == "A")
            {
                oInfoEvento.Sentido = "Asc";
            }
            else
            {
                if (Conversiones.edt_Str(oDR["eve_senti"]) == "D")
                {
                    oInfoEvento.Sentido = "Des";
                }
                else
                {
                    oInfoEvento.Sentido = Conversiones.edt_Str(oDR["eve_senti"]);
                }
            }

            oInfoEvento.FormaPagoCorta = Conversiones.edt_Str(oDR["for_corto"]);
            oInfoEvento.Exento = "";

            if (oDR["cod_descr"] != DBNull.Value)
            {
                oInfoEvento.Exento = Conversiones.edt_Str(oDR["cod_descr"]);
            }

            if (oDR["ctg_descr"] != DBNull.Value)
            {
                oInfoEvento.Exento = Conversiones.edt_Str(oDR["ctg_descr"]);
            }

            if (oDR["tcu_descr"] != DBNull.Value)
            {
                oInfoEvento.Cupom = Conversiones.edt_Str(oDR["tcu_descr"]);
            }

            oInfoEvento.Bloque = Conversiones.edt_Str(oDR["eve_nturn"]);
            oInfoEvento.Ticket = Conversiones.edt_Str(oDR["tra_ticke"]);
            oInfoEvento.Operador = Conversiones.edt_Str(oDR["use_nombr"]);
            oInfoEvento.NroTransito = Conversiones.edt_Str(oDR["tra_numtr"]);
            oInfoEvento.Ejes = Conversiones.edt_Str(oDR["tra_ejers"]);
            oInfoEvento.DobleEje = Conversiones.edt_Str(oDR["tra_ejerd"]);
            oInfoEvento.Altura = Conversiones.edt_Str(oDR["tra_altur"]);
            oInfoEvento.Matricula = Conversiones.edt_Str(oDR["trc_paten"]);
            oInfoEvento.Observaciones = Conversiones.edt_Str(oDR["eve_obser"]);
            oInfoEvento.CodEst = Conversiones.edt_Str(oDR["eve_coest"]);
            oInfoEvento.Nuvia = Conversiones.edt_Str(oDR["eve_nuvia"]);
            oInfoEvento.Ident = Conversiones.edt_Str(oDR["eve_ident"]);
            oInfoEvento.IdTran = Conversiones.edt_Str(oDR["eve_traid"]);
            oInfoEvento.CodEve = Conversiones.edt_Str(oDR["eve_codev"]);
            oInfoEvento.FormaPagoInicial = Conversiones.edt_Str(oDR["tra_tipop"]);
            oInfoEvento.SubTipoFormaPago = Conversiones.edt_Str(oDR["tra_tipbo"]);
            oInfoEvento.FormaPagoDesc = Conversiones.edt_Str(oDR["for_descr"]);
            oInfoEvento.CtaAgrupacion = Conversiones.edt_Str(oDR["ctg_descr"]);
            oInfoEvento.IdUsuario = Conversiones.edt_Str(oDR["eve_id"]);
            oInfoEvento.obsSupervisor = Conversiones.edt_Str(oDR["obs_comen"]);
            oInfoEvento.NombreVia = Conversiones.edt_Str(oDR["via_nombr"]);
            oInfoEvento.Velocidad = Conversiones.edt_Int32(oDR["Velocidad"]);
            oInfoEvento.SuspTab = Conversiones.edt_Str(oDR["EjesSuspensoTabulado"]);

            if (oInfoEvento.SuspTab == "1")
            {
                oInfoEvento.SuspTab += " Eixo";
            }
            else if (oInfoEvento.SuspTab != "")
            {

                oInfoEvento.SuspTab += " Eixos";
            }
            oInfoEvento.Tag = new Tag()
            {
                EmisorTag = Conversiones.edt_Str(oDR["Emisor"]),
                NumeroTag = Conversiones.edt_Str(oDR["Tag"]),
            };

            return oInfoEvento;
        }
        
        /// ----------------------------------------------------------------------------------------------
        /// <summary>
        /// Devuelve una lista de eventos
        /// </summary>
        /// <param name="oConn">objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xiCodEst">Numero de estacion a filtrar</param>
        /// <param name="xdFechaDesde">Fecha Desde a filtrar</param>
        /// <param name="xdFechaHasta">Fecha Hasta a filtrar</param>
        /// <param name="xiMinIdent">Nro. de identidad a filtrar</param>
        /// <param name="xVias">Vias a filtrar</param>
        /// <param name="xTiposEventos">Tipos de eventos a filtrar</param>
        /// <param name="xiCantRows">Cantidad de filas a mostrar</param>
        /// <param name="llegoAlTope"></param>
        /// <returns></returns>
        /// ----------------------------------------------------------------------------------------------
        public static List<string> getImagenes(Conexion oConn, int iCoest, int iNroVia, int iNroEvento)
        {
            List<string> sImagenes = new List<string>();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Evento_GetImagenes";

            oCmd.Parameters.Add("@evento", SqlDbType.Int).Value = iNroEvento;
            oCmd.Parameters.Add("@via", SqlDbType.Int).Value = iNroVia;
            oCmd.Parameters.Add("@coest", SqlDbType.Int).Value = iCoest;

            oCmd.CommandTimeout = 120;
            oDR = oCmd.ExecuteReader();
               
            while (oDR.Read())
            {
                sImagenes.Add(Conversiones.edt_Str(oDR["Ruta"]));                   
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return sImagenes;
        }

        #endregion

        #region INFOEVENTOS SUPERVISION REMOTA
        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Devuelve una lista de eventos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 05/08/2009
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

        public static InfoEventoL getInfoEventosSupervision(Conexion oConn, int estacion, int nuvia)
        {
            InfoEventoL oInfoEventos = new InfoEventoL();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                //oCmd.CommandText = "Facturacion.usp_Colores_getColores";
                oCmd.CommandText = "Peaje.usp_SupervisionRemota_getEventosVia";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@Nuvia", SqlDbType.TinyInt).Value = nuvia;

                oCmd.CommandTimeout = 120;

                oDR = oCmd.ExecuteReader();
                int i = 0;
                while (oDR.Read())
                {
                    i++;
                    oInfoEventos.Add(CargarInfoEventosSupervision(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return oInfoEventos;
        }


        // ----------------------------------------------------------------------------------------------
        // FUNCIONALIDAD ...: Carga un elemento DataReader en la lista de InfoEventos
        // AUTOR ...........: Cristian Binaghi
        // FECHA CREACIÓN ..: 05/08/2009
        // ULT.FEC.MODIF. ..:
        // OBSERVACIONES ...: Parámetros: 
        //                                  oDR - System.Data.IDataReader - Objeto DataReader de la tabla
        //                    Retorna: Lista de InfoEvento: InfoEventoL
        // ----------------------------------------------------------------------------------------------

        private static InfoEvento CargarInfoEventosSupervision(System.Data.IDataReader oDR)
        {
            InfoEvento oInfoEvento = new InfoEvento();

            oInfoEvento.Fecha = Conversiones.edt_DateTime(oDR["eve_fecha"]);
            oInfoEvento.DescTipoEvento = Conversiones.edt_Str(oDR["cla_descr"]);
            oInfoEvento.Observaciones = Conversiones.edt_Str(oDR["eve_obser"]);

            /*
            oInfoEvento.VideoFoto1 = Conversiones.edt_Str(oDR["tra_video"]);
            oInfoEvento.VideoFoto2 = Conversiones.edt_Str(oDR["tra_video2"]);
            oInfoEvento.TipoEvento = Conversiones.edt_Str(oDR["cla_tipom"]);
            oInfoEvento.DescTipoEvento = Conversiones.edt_Str(oDR["cla_descr"]);
            oInfoEvento.Manual = Conversiones.edt_Str(oDR["tra_manua"]);
            oInfoEvento.Dac = Conversiones.edt_Str(oDR["tra_dac"]);
            

            if (Conversiones.edt_Str(oDR["eve_senti"]) == "A")
            {
                oInfoEvento.Sentido = "Asc";
            }
            else
            {
                if (Conversiones.edt_Str(oDR["eve_senti"]) == "D")
                {
                    oInfoEvento.Sentido = "Des";
                }
                else
                {
                    oInfoEvento.Sentido = Conversiones.edt_Str(oDR["eve_senti"]);
                }
            }

            oInfoEvento.FormaPagoCorta = Conversiones.edt_Str(oDR["for_corto"]);

            oInfoEvento.Exento = "";

            if (oDR["cod_descr"] != DBNull.Value)
            {
                oInfoEvento.Exento = Conversiones.edt_Str(oDR["cod_descr"]);
            }

            if (oDR["ctg_descr"] != DBNull.Value)
            {
                oInfoEvento.Exento = Conversiones.edt_Str(oDR["ctg_descr"]);
            }

            oInfoEvento.Bloque = Conversiones.edt_Str(oDR["eve_nturn"]);
            oInfoEvento.Ticket = Conversiones.edt_Str(oDR["tra_ticke"]);
            oInfoEvento.Operador = Conversiones.edt_Str(oDR["use_nombr"]);     //(String)oDR["use_nombr"];
            oInfoEvento.NroTransito = Conversiones.edt_Str(oDR["tra_numtr"]);
            oInfoEvento.Ejes = Conversiones.edt_Str(oDR["tra_ejers"]);
            oInfoEvento.DobleEje = Conversiones.edt_Str(oDR["tra_ejerd"]);
            oInfoEvento.Altura = Conversiones.edt_Str(oDR["tra_altur"]);
            oInfoEvento.Matricula = Conversiones.edt_Str(oDR["trc_paten"]);
            oInfoEvento.Observaciones = Conversiones.edt_Str(oDR["eve_obser"]);

            oInfoEvento.CodEst = Conversiones.edt_Str(oDR["eve_coest"]);
            oInfoEvento.Nuvia = Conversiones.edt_Str(oDR["eve_nuvia"]);
            oInfoEvento.Ident = Conversiones.edt_Str(oDR["eve_ident"]);
            oInfoEvento.IdTran = Conversiones.edt_Str(oDR["eve_traid"]);
            oInfoEvento.CodEve = Conversiones.edt_Str(oDR["eve_codev"]);
            oInfoEvento.FormaPagoInicial = Conversiones.edt_Str(oDR["tra_tipop"]);
            oInfoEvento.SubTipoFormaPago = Conversiones.edt_Str(oDR["tra_tipbo"]);
            oInfoEvento.FormaPagoDesc = Conversiones.edt_Str(oDR["for_descr"]);

            //oInfoEvento.ExentoDesc = (String)oDR["cod_descr"];
            oInfoEvento.CtaAgrupacion = Conversiones.edt_Str(oDR["ctg_descr"]);

            oInfoEvento.IdUsuario = Conversiones.edt_Str(oDR["eve_id"]);

            oInfoEvento.obsSupervisor = Conversiones.edt_Str(oDR["obs_comen"]);
             */
            return oInfoEvento;

        }
        #endregion
    }
}

using System;
using System.Data;
using Telectronica.Errores;
using System.Data.SqlClient;
using Telectronica.Utilitarios;

namespace Telectronica.Peaje
{
    /// <summary>
    /// Clase de la capa de datos para la entidad AbandonoDeTroco
    /// </summary>
    public class AbandonoDeTrocoDt
    {
        #region MÉTODO QUE CARGA UNA ENTIDAD

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static AbandonoDeTroco CargarAbandonoDeTroco(IDataReader oDR)
        {
            var abandonoDeTroco = new AbandonoDeTroco();

            abandonoDeTroco.Identity = (int)oDR["abt_ident"];
            abandonoDeTroco.Estacion = new Estacion((byte)oDR["est_codig"], Convert.ToString(oDR["est_nombr"]));
            abandonoDeTroco.FechaRegAbandono = Convert.ToDateTime(oDR["abt_fecha"]);
            abandonoDeTroco.Monto = Convert.ToDecimal(oDR["abt_monto"]);
            abandonoDeTroco.Via = new Via((byte)oDR["via_coest"], (byte)oDR["via_nuvia"], Convert.ToString(oDR["via_nombr"]));
            abandonoDeTroco.Supervisor = new Usuario(Convert.ToString(oDR["IDSupervisor"]), Convert.ToString(oDR["NombreSupervisor"]));
            abandonoDeTroco.Peajista = new Usuario(Convert.ToString(oDR["IDCajero"]), Convert.ToString(oDR["NombreCajero"]));
            abandonoDeTroco.Turno = new TurnoTrabajo
            {
                NumeroTurno = (short)oDR["abt_nturn"]
            };
            abandonoDeTroco.Sentido = new ViaSentidoCirculacion(Convert.ToString(oDR["sen_senti"]), Convert.ToString(oDR["sen_descr"]));

            return abandonoDeTroco;
        }

        /// <summary>
        /// Carga una entidad con los datos recuperados de la base de datos
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        private static AbandonoDeTroco CargarAbandonoDeTrocoOrigen(IDataReader oDR)
        {
            var abandonoDeTroco = new AbandonoDeTroco();

            abandonoDeTroco.Peajista = new Usuario(Convert.ToString(oDR["IDCajero"]), Convert.ToString(oDR["NombreCajero"]));
            abandonoDeTroco.Turno = new TurnoTrabajo
            {
                NumeroTurno = (int)oDR["via_nturn"]
            };
            abandonoDeTroco.Sentido = new ViaSentidoCirculacion(Convert.ToString(oDR["sen_senti"]), Convert.ToString(oDR["sen_descr"]));

            return abandonoDeTroco;
        }

        #endregion

        #region MÉTODOS ENCARGADOS DE OPERAR EN LA BASE DE DATOS

        /// <summary>
        /// Devuelve la lista de Abandono de Trocos
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iIdentity"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="dFechaDesde"></param>
        /// <param name="dFechaHasta"></param>
        /// <param name="sSentido"></param>
        /// <param name="iNueroVia"></param>
        /// <param name="sSupervisor"></param>
        /// <param name="sPeajista"></param>
        /// <param name="sRendidos"></param>
        /// <returns></returns>
        public static AbandonoDeTrocoL getAbandonoDeTroco(Conexion oConn, int? iIdentity, int? iNumeroEstacion, DateTime? dFechaDesde, DateTime? dFechaHasta, string sSentido, int? iNumeroVia, string sSupervisor, string sPeajista, string sRendidos)
        {
            AbandonoDeTrocoL abandonos = new AbandonoDeTrocoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AbandonoDeTroco_getAbandonoDeTroco";

            oCmd.Parameters.Add("@abt_ident", SqlDbType.Int).Value = iIdentity;
            oCmd.Parameters.Add("@abt_coest", SqlDbType.TinyInt).Value = iNumeroEstacion;
            oCmd.Parameters.Add("@fechaDESDE", SqlDbType.DateTime).Value = dFechaDesde;
            oCmd.Parameters.Add("@fechaHASTA", SqlDbType.DateTime).Value = dFechaHasta;
            oCmd.Parameters.Add("@abt_senti", SqlDbType.Char, 1).Value = sSentido;
            oCmd.Parameters.Add("@abt_nuvia", SqlDbType.TinyInt).Value = iNumeroVia;
            oCmd.Parameters.Add("@abt_super", SqlDbType.VarChar, 10).Value = sSupervisor;
            oCmd.Parameters.Add("@peajista", SqlDbType.VarChar, 10).Value = sPeajista;
            oCmd.Parameters.Add("@Rendidos", SqlDbType.Char, 1).Value = sRendidos;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                abandonos.Add(CargarAbandonoDeTroco(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return abandonos;
        }

        /// <summary>
        /// Devuelve la lista de Abandono de Trocos para el reporte
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iIdentity"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="dFechaDesde"></param>
        /// <param name="dFechaHasta"></param>
        /// <param name="sSentido"></param>
        /// <param name="iNueroVia"></param>
        /// <param name="sSupervisor"></param>
        /// <param name="sPeajista"></param>
        /// <param name="sRendidos"></param>
        /// <returns></returns>
        public static DataSet getRptAbandonoDeTroco(Conexion oConn, int? iIdentity, int? iNumeroEstacion, DateTime? dFechaDesde, DateTime? dFechaHasta, string sSentido, int? iNumeroVia, string sSupervisor, string sPeajista, string sRendidos)
        {
            DataSet dsApr = new DataSet();
            dsApr.DataSetName = "RptAbandonoDeTrocoDS";

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AbandonoDeTroco_getAbandonoDeTroco";

            oCmd.Parameters.Add("@abt_ident", SqlDbType.Int).Value = iIdentity;
            oCmd.Parameters.Add("@abt_coest", SqlDbType.TinyInt).Value = iNumeroEstacion;
            oCmd.Parameters.Add("@fechaDESDE", SqlDbType.DateTime).Value = dFechaDesde;
            oCmd.Parameters.Add("@fechaHASTA", SqlDbType.DateTime).Value = dFechaHasta;
            oCmd.Parameters.Add("@abt_senti", SqlDbType.Char, 1).Value = sSentido;
            oCmd.Parameters.Add("@abt_nuvia", SqlDbType.TinyInt).Value = iNumeroVia;
            oCmd.Parameters.Add("@abt_super", SqlDbType.VarChar, 10).Value = sSupervisor;
            oCmd.Parameters.Add("@peajista", SqlDbType.VarChar, 10).Value = sPeajista;
            oCmd.Parameters.Add("@Rendidos", SqlDbType.Char, 1).Value = sRendidos;

            SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
            oDA.Fill(dsApr, "AbandonoDeTroco");

            // Lo cerramos 
            oCmd = null;
            oDA.Dispose();

            return dsApr;
        }

        /// <summary>
        /// Devuelve un objeto AbandonoDeTroco que contiene la información necesaria donde se produjo el mismo, para luego registrarla.
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="iNumeroEstacion"></param>
        /// <param name="iNumeroVia"></param>
        /// <returns></returns>
        public static AbandonoDeTroco getAbandonoDeTrocoOrigen(Conexion oConn, int iNumeroEstacion, int iNumeroVia)
        {
            AbandonoDeTroco abandono = new AbandonoDeTroco();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AbandonoDeTroco_getOrigenAbandonoDeTroco";

            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = iNumeroEstacion;
            oCmd.Parameters.Add("@nuvia", SqlDbType.TinyInt).Value = iNumeroVia;

            oDR = oCmd.ExecuteReader();

   

            if(oDR.Read())
            {
                abandono = CargarAbandonoDeTrocoOrigen(oDR);
            }
            
            

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return abandono;
        }

        /// <summary>
        /// Método encargado de agregar en la base de datos
        /// </summary>
        /// <param name="abandonoDeTroco"></param>
        /// <param name="oConn"></param>
        public static void addAbandonoDeTroco(Conexion oConn, AbandonoDeTroco abandonoDeTroco)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AbandonoDeTroco_addAbandonoDeTroco";

            oCmd.Parameters.Add("@abt_coest", SqlDbType.TinyInt).Value = abandonoDeTroco.Estacion.Numero;
            oCmd.Parameters.Add("@abt_monto", SqlDbType.Money).Value = abandonoDeTroco.Monto;
            oCmd.Parameters.Add("@abt_nuvia", SqlDbType.TinyInt).Value = abandonoDeTroco.Via.NumeroVia;
            oCmd.Parameters.Add("@abt_super", SqlDbType.VarChar, 10).Value = abandonoDeTroco.Supervisor.ID;
            oCmd.Parameters.Add("@abt_nturn", SqlDbType.SmallInt).Value = abandonoDeTroco.Turno.NumeroTurno;
            oCmd.Parameters.Add("@abt_senti", SqlDbType.Char, 1).Value = abandonoDeTroco.Sentido.Codigo;

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
                    msg = Traduccion.Traducir("Este Código de Simulación ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este Código de Causa de cierre fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// <summary>
        /// Método encargado de eliminar de la base de datos
        /// </summary>
        /// <param name="tipoCupon"></param>
        /// <param name="oConn"></param>
        public static void delAbandonoDeTroco(Conexion oConn, AbandonoDeTroco abandonoDeTroco)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_AbandonoDeTroco_delAbandonoDeTroco";

            oCmd.Parameters.Add("@abt_ident", SqlDbType.Int).Value = abandonoDeTroco.Identity;
            oCmd.Parameters.Add("@abt_coest", SqlDbType.TinyInt).Value = abandonoDeTroco.Estacion.Numero;

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
                    msg = Traduccion.Traducir("No existe el registro del código de simulación");
                }
                throw new ErrorSPException(msg);
            }
        }

        #endregion
    }
}

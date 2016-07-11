using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Tesoreria;

namespace Telectronica.Facturacion
{
    public class ClienteFacturacionDt
    {

        #region IMPRESORA: Clase de Datos de Impresora


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Impresoras definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de la que quiero consultar la impresora</param>
        /// <param name="numeroImpresora">byte? - Numero de impresora por la cual filtrar la busqueda</param>
        /// <returns>Lista de Impresoras definidas</returns>
        /// ***********************************************************************************************
        public static ImpresoraL getImpresoras(Conexion oConn,
                                               int numeroEstacion,
                                               byte? numeroImpresora)
        {
            ImpresoraL oImpresoraL = new ImpresoraL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Impresoras_getImpresoras";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = numeroEstacion;
                oCmd.Parameters.Add("Impre", SqlDbType.TinyInt).Value = numeroImpresora;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oImpresoraL.Add(CargarImpresora(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oImpresoraL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Impresoras
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Impresoras</param>
        /// <returns>Objeto Impresora con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static Impresora CargarImpresora(System.Data.IDataReader oDR)
        {
            Impresora oImpresora = new Impresora();
            oImpresora.Estacion = new Estacion((byte)oDR["imp_coest"], "");
            oImpresora.Codigo = (byte)oDR["imp_impre"];
            oImpresora.PuntoVenta = oDR["imp_puvta"].ToString();
            oImpresora.CantidadCopias = (byte)oDR["imp_copfa"];

            oImpresora.TipoImpresora = oDR["imp_modo"].ToString();
            oImpresora.PuertoCOM = (byte)oDR["imp_com"];
            oImpresora.UrlServicio = oDR["imp_svurl"].ToString();

            return oImpresora;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Impresora en la base de datos
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto con la informacion de la impresora a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addImpresora(Impresora oImpresora, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Impresoras_addImpresora";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oImpresora.Estacion.Numero;
                oCmd.Parameters.Add("@Impre", SqlDbType.TinyInt).Value = oImpresora.Codigo;
                oCmd.Parameters.Add("@Puvta", SqlDbType.VarChar).Value = oImpresora.PuntoVenta;
                oCmd.Parameters.Add("@Copfa", SqlDbType.TinyInt).Value = oImpresora.CantidadCopias;

                oCmd.Parameters.Add("@COM", SqlDbType.TinyInt).Value = oImpresora.PuertoCOM;
                oCmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 1).Value = oImpresora.TipoImpresora;
                oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oImpresora.UrlServicio;

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
                        msg = Traduccion.Traducir("Este número de impresora ya existe");
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
        /// Modifica una Impresora de la base de datos
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto con la informacion de la Impresora a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updImpresora(Impresora oImpresora, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Impresoras_updImpresora";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oImpresora.Estacion.Numero;
                oCmd.Parameters.Add("@Impre", SqlDbType.TinyInt).Value = oImpresora.Codigo;
                oCmd.Parameters.Add("@Puvta", SqlDbType.VarChar).Value = oImpresora.PuntoVenta;
                oCmd.Parameters.Add("@Copfa", SqlDbType.TinyInt).Value = oImpresora.CantidadCopias;
                
                oCmd.Parameters.Add("@COM", SqlDbType.TinyInt).Value = oImpresora.PuertoCOM;
                oCmd.Parameters.Add("@Tipo", SqlDbType.VarChar, 1).Value = oImpresora.TipoImpresora;
                oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oImpresora.UrlServicio;

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
                        msg = Traduccion.Traducir("No existe el registro de la Impresora");
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
        /// Elimina una Impresora de la base de datos
        /// </summary>
        /// <param name="oImpresora">Impresora - Objeto que contiene la Impresora a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delImpresora(Impresora oImpresora, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Impresoras_delImpresora";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oImpresora.Estacion.Numero;
                oCmd.Parameters.Add("@Impre", SqlDbType.TinyInt).Value = oImpresora.Codigo;

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
                        msg = Traduccion.Traducir("No existe el registro de la Impresora");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                ErrorFKException.EsErrorFK(ex, Traduccion.Traducir("La Impresora no se puede dar de baja porque está siendo utilizado"));
                throw ex;
            }

            /*catch (Exception ex)
            {
                throw ex;
            }*/
            return;
        }

        #endregion

        #region CAMBIOVENDEDOR: Clase de Datos de Cambios de Vendedores

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el vendedor a cargo de la terminal
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="estacion">int - Codigo de estacion</param>
        /// <param name="terminal">Int16 - Numero de terminal por la que consulto</param>
        /// <returns>Objeto de Vendedor a Cargo de la terminal</returns>
        /// ***********************************************************************************************
        public static CambioVendedor getVendedorACargo(Conexion oConn,
                                                       int estacion,
                                                       Int16 terminal)
        {
            CambioVendedor oCambioVendedor = null;
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_CambiosVendedor_GetVendedorACargo";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("Termi", SqlDbType.SmallInt).Value = terminal;

                oDR = oCmd.ExecuteReader();
                if (oDR.Read())
                {
                    oCambioVendedor = CargarCambioVendedor(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCambioVendedor;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un CambioVendedor
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cambios de Vendedor</param>
        /// <returns>Objeto CambioVendedor</returns>
        /// ***********************************************************************************************
        private static CambioVendedor CargarCambioVendedor(System.Data.IDataReader oDR)
        {
            CambioVendedor oCambioVendedor = new CambioVendedor();
            oCambioVendedor.Estacion = new Estacion((byte)oDR["cam_coest"], oDR["est_nombr"].ToString());
            oCambioVendedor.FechaInicio = (DateTime)oDR["cam_fecin"];
            oCambioVendedor.FechaFinal = Util.DbValueToNullable<DateTime>(oDR["cam_fecfi"]);
            oCambioVendedor.Identity = (int)oDR["cam_ident"];
            oCambioVendedor.Supervisor = new Usuario(oDR["cam_supid"].ToString(), oDR["use_nombr"].ToString());
            oCambioVendedor.Parte = new Parte((int)oDR["cam_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);
            oCambioVendedor.Parte.Estacion = oCambioVendedor.Estacion;

            // Tengo que copiar este dato porque no lo tengo en el parte, ya que lo cargue a mano
            oCambioVendedor.Parte.Peajista = oCambioVendedor.Supervisor;


            return oCambioVendedor;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un CambioVendedor
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cambios de Vendedor</param>
        /// <returns>Objeto CambioVendedor</returns>
        /// ***********************************************************************************************
        private static CambioVendedor CargarTerminalAbierta(System.Data.IDataReader oDR)
        {
           CambioVendedor oCambioVendedor = new CambioVendedor();
          
            oCambioVendedor.Estacion = String.IsNullOrEmpty(oDR["est_nombr"].ToString()) ? new Estacion() : new Estacion((byte)oDR["cam_coest"], oDR["est_nombr"].ToString());
            oCambioVendedor.FechaInicio = String.IsNullOrEmpty(oDR["cam_fecin"].ToString()) ? DateTime.Today.AddYears(-100) : (DateTime)oDR["cam_fecin"];
            oCambioVendedor.FechaFinal = String.IsNullOrEmpty(oDR["cam_fecfi"].ToString()) ? DateTime.Today.AddYears(-100) : (DateTime)oDR["cam_fecfi"];
            oCambioVendedor.Identity = String.IsNullOrEmpty(oDR["cam_ident"].ToString()) ? -1 : (int)oDR["cam_ident"];
            oCambioVendedor.Supervisor = String.IsNullOrEmpty(oDR["use_nombr"].ToString()) ? new Usuario() : new Usuario(oDR["cam_supid"].ToString(), oDR["use_nombr"].ToString());
            oCambioVendedor.Parte = String.IsNullOrEmpty(oDR["cam_parte"].ToString()) ? new Parte() : new Parte((int)oDR["cam_parte"], (DateTime)oDR["par_fejor"], (byte)oDR["par_testu"]);
            oCambioVendedor.Parte.Estacion = oCambioVendedor.Estacion;
            oCambioVendedor.TerminalFacturacion = new TerminalFacturacion(oCambioVendedor.Estacion, (short)oDR["tfa_numer"], oDR["tfa_descr"].ToString(), new Impresora());

            // Tengo que copiar este dato porque no lo tengo en el parte, ya que lo cargue a mano
            oCambioVendedor.Parte.Peajista = oCambioVendedor.Supervisor;          
            
            return oCambioVendedor;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una asignacion de vendedor en la base de datos
        /// </summary>
        /// <param name="oCambio">CambioVendedor - Objeto con la informacion de la asignacion a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void setVendedorACargo(Parte oParte, TerminalFacturacion oTerminal, bool bCierraTerminal, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_CambiosVendedor_setVendedorACargo";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oParte.Estacion.Numero;
                oCmd.Parameters.Add("@Termi", SqlDbType.SmallInt).Value = oTerminal.Numero;
                oCmd.Parameters.Add("@ID", SqlDbType.VarChar, 10).Value = oParte.Peajista.ID;
                oCmd.Parameters.Add("@Parte", SqlDbType.Int).Value = oParte.Numero;
                oCmd.Parameters.Add("@Cierra", SqlDbType.Char, 1).Value = (bCierraTerminal ? "S" : "N");

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

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        #endregion

        #region TERMINALFACTURACION: Clase de Datos de TerminalFacturacion

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de las ultimas Terminales de Facturacion abiertas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de ultimas Terminales abiertas</returns>
        /// ***********************************************************************************************
        public static CambioVendedorL getTerminalesAbiertas(Conexion oConn, int estacion)
        {
            CambioVendedorL oTerminalL = new CambioVendedorL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_getUltimasTerminalesAbiertas";
               // oCmd.Parameters.Add("coest", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTerminalL.Add(CargarTerminalAbierta(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTerminalL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Terminales de Facturacion definidas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de la que quiero consultar la impresora</param>
        /// <param name="numeroImpresora">byte? - Numero de terminal por la cual filtrar la busqueda</param>
        /// <returns>Lista de Terminales definidas</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacionL getTerminales(Conexion oConn,
                                                         int numeroEstacion,
                                                         Int16? numeroTerminal)
        {
            TerminalFacturacionL oTerminalL = new TerminalFacturacionL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_getTerminales";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = numeroEstacion;
                oCmd.Parameters.Add("Termi", SqlDbType.SmallInt).Value = numeroTerminal;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTerminalL.Add(CargarTerminal(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oTerminalL;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Terminales
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Terminales</param>
        /// <returns>Objeto TerminalFacturacion con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static TerminalFacturacion CargarTerminal(System.Data.IDataReader oDR)
        {
            TerminalFacturacion oTerminalFacturacion = new TerminalFacturacion();
            oTerminalFacturacion.Estacion = new Estacion((byte)oDR["tfa_coest"], "");
            oTerminalFacturacion.Numero = (short)oDR["tfa_numer"];
            oTerminalFacturacion.Descripcion = oDR["tfa_descr"].ToString();
            oTerminalFacturacion.ImpresoraFacturacion = new Impresora(new Estacion((byte)oDR["tfa_coest"], ""), (byte)oDR["tfa_impre"], "", 0);
            oTerminalFacturacion.ImprimeFallos = oDR["tfa_ifalls"].ToString();

            return oTerminalFacturacion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Terminal de Facturacion en la base de datos
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Objeto con la informacion de la terminal a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addTerminal(TerminalFacturacion oTerminal, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_addTerminal";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oTerminal.Estacion.Numero;
                oCmd.Parameters.Add("@Termi", SqlDbType.SmallInt).Value = oTerminal.Numero;
                oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oTerminal.Descripcion;
                oCmd.Parameters.Add("@Impre", SqlDbType.TinyInt).Value = oTerminal.ImpresoraFacturacion.Codigo;

                oCmd.Parameters.Add("@Ifa", SqlDbType.VarChar, 1).Value = oTerminal.ImprimeFallos;

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
                        msg = Traduccion.Traducir("Este número de terminal ya existe");
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
        /// Modifica una Terminal de Facturacion de la base de datos
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Objeto con la informacion de la Terminal a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updTerminal(TerminalFacturacion oTerminal, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_updTerminal";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oTerminal.Estacion.Numero;
                oCmd.Parameters.Add("@Termi", SqlDbType.SmallInt).Value = oTerminal.Numero;
                oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 50).Value = oTerminal.Descripcion;
                oCmd.Parameters.Add("@Impre", SqlDbType.TinyInt).Value = oTerminal.ImpresoraFacturacion.Codigo;
                oCmd.Parameters.Add("@Ifa", SqlDbType.VarChar, 1).Value = oTerminal.ImprimeFallos;
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
                        msg = Traduccion.Traducir("No existe el registro de la Terminal");
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
        /// Elimina una Terminal de Facturacion de la base de datos
        /// </summary>
        /// <param name="oTerminal">TerminalFacturacion - Objeto que contiene la Terminal a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delTerminal(TerminalFacturacion oTerminal, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_delTerminal";

                oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = oTerminal.Estacion.Numero;
                oCmd.Parameters.Add("@Termi", SqlDbType.SmallInt).Value = oTerminal.Numero;

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
                        msg = Traduccion.Traducir("No existe el registro de la Terminal");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }


        #endregion

        #region TERMINALFACTURACIONFISICA: Clase de Datos de TerminalFacturacionFisica


        #endregion

        #region LECTORESCHIP: Clase de Lectores de Tarjetas Chip

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Lectores de Tarjetas Chip
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Lectores</param>
        /// <returns>Objeto LectorChip con los elementos de la base de datos</returns>
        /// ***********************************************************************************************
        private static LectorChip CargarLectoresChip(System.Data.IDataReader oDR)
        {
            LectorChip oLectorChip = new LectorChip();
            oLectorChip.Descripcion = oDR["lec_descr"].ToString();
            oLectorChip.Estacion = new Estacion((byte)oDR["lec_coest"], oDR["est_nombr"].ToString());
            oLectorChip.Numero = (byte)oDR["lec_codig"];
            oLectorChip.PuertoCOM = (byte)oDR["lec_com"];
            oLectorChip.UrlServicio = oDR["lec_svurl"].ToString();

            return oLectorChip;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Lectores de Tarjetas Chip
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroEstacion">int - Numero de estacion de la que quiero consultar el lector</param>
        /// <param name="numeroLector">byte? - Numero de lector de tarjetas chip</param>
        /// <returns>Lista de Lectores de Tarjetas Chip</returns>
        /// ***********************************************************************************************
        public static LectorChipL getLectoresChip(Conexion oConn, int numeroEstacion, byte? numeroLector)
        {
            LectorChipL oLectorChipL = new LectorChipL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_LectoresChip_getLectores";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = numeroEstacion;
                oCmd.Parameters.Add("Codig", SqlDbType.TinyInt).Value = numeroLector;


                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oLectorChipL.Add(CargarLectoresChip(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oLectorChipL;
        }

        /// <summary>
        /// Modifica un registro de lector chip
        /// </summary>
        /// <param name="oConn">Conexion con la base de datos</param>
        /// <param name="oLectorChip">Lector Chip que se modificara</param>
        public static void updLectorChip(Conexion oConn, LectorChip oLectorChip)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_LectoresChip_updLectores";
                oCmd.Parameters.Add("@estac", SqlDbType.TinyInt).Value = oLectorChip.Estacion.Numero;
                oCmd.Parameters.Add("@COM", SqlDbType.TinyInt).Value = oLectorChip.PuertoCOM;
                oCmd.Parameters.Add("@descr", SqlDbType.VarChar, 255).Value = oLectorChip.Descripcion;
                oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oLectorChip.UrlServicio;
                oCmd.Parameters.Add("@codig", SqlDbType.TinyInt).Value = oLectorChip.Numero;

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
                        msg = Traduccion.Traducir("No existe el registro del lector chip");
                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Elimina un registro de Lector Chip
        /// </summary>
        /// <param name="oConn">Conexion actual con la base de datos</param>
        /// <param name="oLectorChip">Objeto Lector Chip que se eliminara</param>
        public static void delLectorChip(Conexion oConn, LectorChip oLectorChip)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_LectoresChip_delLectores";
                oCmd.Parameters.Add("@codig", SqlDbType.TinyInt).Value = oLectorChip.Numero;
                oCmd.Parameters.Add("@estac", SqlDbType.TinyInt).Value = oLectorChip.Estacion.Numero;

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
                    if (retval == -101)
                        msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                    else if (retval == -102)
                        msg = Traduccion.Traducir("El lector especificado no existe");
                    
                    throw new ErrorSPException(msg);

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Agrega un registro de Lectoy Chip
        /// </summary>
        /// <param name="oConn">Conexion actual</param>
        /// <param name="oLectorChip">Lector Chip que se ingresara</param>
        public static void addLectorChip(Conexion oConn, LectorChip oLectorChip)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_LectoresChip_addLectores";
                oCmd.Parameters.Add("@estac", SqlDbType.TinyInt).Value = oLectorChip.Estacion.Numero;
                oCmd.Parameters.Add("@COM", SqlDbType.TinyInt).Value = oLectorChip.PuertoCOM;
                oCmd.Parameters.Add("@descr", SqlDbType.VarChar, 255).Value = oLectorChip.Descripcion;
                oCmd.Parameters.Add("@URL", SqlDbType.VarChar, 255).Value = oLectorChip.UrlServicio;
                oCmd.Parameters.Add("@codig", SqlDbType.TinyInt).Value = oLectorChip.Numero;

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
                    if (retval == -101)
                        msg = Traduccion.Traducir("Error de Parametrización");
                    else if (retval == -102)
                        msg = Traduccion.Traducir("Ya existe un registro con ese código");

                    throw new ErrorSPException(msg);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Obtiene los datos del lector Chip
        /// </summary>
        /// <param name="oConn">Conexion actual con la base de datos</param>
        /// <param name="numeroLector">Codigo del lector</param>
        /// <returns></returns>
        public static LectorChip getLectorChip(Conexion oConn, byte numeroLector)
        {
            LectorChip oLectorChip = new LectorChip();

            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_LectoresChip_getLector";
                oCmd.Parameters.Add("Codig", SqlDbType.TinyInt).Value = numeroLector;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oLectorChip = CargarLectoresChip(oDR);
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oLectorChip;
        }

        #endregion

        #region CONFIGURACIONCLIENTEFACTURACION

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la configuracion de la misma numeracion para todos los comprobantes
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Configuracion de numeracion</returns>
        /// ***********************************************************************************************
        public static bool getMismaNumeracion(Conexion oConn)
        {
            bool bMismaNumeracion = false;

            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Configuracion_getConfiguracion";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    bMismaNumeracion = (oDR["cfg_misnu"].ToString() == "S");
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return bMismaNumeracion;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la terminal de facturacion logica que esta asignada a la terminal fisica 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="Host">string - Host (terminal fisica) de la que deseo averiguar la terminal logica</param>
        /// <returns>Numero de terminal logica</returns>
        /// ***********************************************************************************************
        public static TerminalFacturacion getTerminalActual(Conexion oConn, string Host)
        {
            TerminalFacturacion oTerminal = null;

            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_getTerminalActual";
                oCmd.Parameters.Add("Host", SqlDbType.VarChar, 200).Value = Host;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oTerminal = CargarTerminal(oDR);
                    //oTerminal = new TerminalFacturacion();
                    //oTerminal.Estacion = new Estacion((byte)oDR["tfa_coest"], "");
                    //oTerminal.Numero = (short)oDR["tfa_numer"];
                    //oTerminal.Descripcion = oDR["tfa_descr"].ToString();
                    //oTerminal.ImpresoraFacturacion = new Impresora(oTerminal.Estacion, (byte)oDR["imp_impre"], (int)oDR["imp_puvta"], (byte)oDR["imp_copfa"]);
                    oTerminal.ImpresoraFacturacion.PuntoVenta = oDR["imp_puvta"].ToString();
                    oTerminal.ImpresoraFacturacion.CantidadCopias = (byte)oDR["imp_copfa"];
                    oTerminal.ImpresoraFacturacion.TipoImpresora = oDR["imp_modo"].ToString();
                    oTerminal.ImpresoraFacturacion.PuertoCOM = (byte)oDR["imp_com"];
                    oTerminal.ImpresoraFacturacion.UrlServicio = oDR["imp_svurl"].ToString();
                    oTerminal.TerminalFisica = new TerminalFacturacionFisica(oDR["tff_host"].ToString());
                    if (oDR["lec_codig"].ToString() != String.Empty)
                    {
                        oTerminal.LectorChip = new LectorChip(oTerminal.Estacion, (byte)oDR["lec_codig"], oDR["lec_descr"].ToString(), (byte)oDR["lec_com"], oDR["lec_svurl"].ToString());
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
            return oTerminal;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Graba el numero de terminal logica actual para la presente terminal fisica 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="terminal">short - Numero de terminal logica</param>
        /// <param name="Host">string - Terminal fisica (host) que asigno para la terminal logica</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updTerminalActual(Conexion oConn, int estacion, short terminal, LectorChip lectorChip, string Host)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Facturacion.usp_Terminales_updTerminalActual";
                oCmd.Parameters.Add("Coest", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("Termi", SqlDbType.SmallInt).Value = terminal;
                if( lectorChip != null )
                    oCmd.Parameters.Add("Lechi", SqlDbType.TinyInt).Value = lectorChip.Numero;
                oCmd.Parameters.Add("Host", SqlDbType.VarChar, 200).Value = Host;

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
                        msg = Traduccion.Traducir("No existe el registro de la Terminal Lógica");
                    throw new ErrorSPException(msg);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion

    }
}

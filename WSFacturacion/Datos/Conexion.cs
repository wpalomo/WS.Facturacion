using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using System.Transactions;
using System.Globalization;
using System.Threading;

namespace Telectronica.Peaje
{

    ///****************************************************************************************************
    /// <summary>
    /// Metodos para manejo de conexiones con las diferentes bases de datos del sistema
    /// </summary>
    ///****************************************************************************************************


    #region CONEXION: Clase de manejo de conexiones pero utilizando instancias de clases

    public class Conexion : System.IDisposable
    {
        //TODO En produccion debe ser false
        public static bool bSinTransaccionesDistribuidas = true;

        public SqlConnection conection { get; set; }

        public OleDbConnection oleConection { get; set; }

        public SqlTransaction transaction { get; set; }

        public TransactionScope transactionScope { get; set; }


        /// ***********************************************************************************************
        /// <summary>
        /// Constructor de la clase "Conexion". Inicializa las variables miembro coneccion y transaccion
        /// </summary>
        /// ***********************************************************************************************
        public Conexion()
        {
            conection = null;
            transaction = null;
            transactionScope = null;

            //TODO usar Transacciones Distribuidas cuando encontremos como configurarlo
            /*
            //Forzamos que se usen transacciones distribuidas en produccion
            if( bSinTransaccionesDistribuidas )
                if (!System.Diagnostics.Debugger.IsAttached)
                    bSinTransaccionesDistribuidas = false;
             * */
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de la estacion.
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarPlaza(bool bTransaccion)
        {
            ConectarPlaza(bTransaccion, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de la estacion.
        /// Se establece como parametro si se desea o no transaccion
        /// Se establece como parametro si se trata de una transaccion distribuida o no 
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <param name="bDistribuida">bool - Si se trata de una transaccion distribuida o no </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarPlaza(bool bTransaccion, bool bDistribuida)
        {
            try
            {
                if (bSinTransaccionesDistribuidas)
                    bDistribuida = false;

                // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
                if (bDistribuida)
                {
                    //Ya hay una transaccion?
                    if (Transaction.Current == null)
                    //if (transactionScope == null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = IsolationLevel.ReadCommitted;
                        options.Timeout = new TimeSpan(0, 1, 0);

                        transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
                    }
                }


                // Conectamos con la estacion
                conection = ConexionStatic.getConexionPlaza();

                if (bTransaccion && !bDistribuida)
                {
                    transaction = conection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Gestion.
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarGST(bool bTransaccion)
        {
            ConectarGST(bTransaccion, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Gestion.
        /// Se establece como parametro si se desea o no transaccion
        /// Se establece como parametro si se trata de una transaccion distribuida o no 
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <param name="bDistribuida">bool - Si se trata de una transaccion distribuida o no </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarGST(bool bTransaccion, bool bDistribuida)
        {
            try
            {

                if (bSinTransaccionesDistribuidas)
                    bDistribuida = false;

                // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
                if (bDistribuida)
                {
                    if (Transaction.Current == null)
                    //if (transactionScope == null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = IsolationLevel.ReadCommitted;
                        options.Timeout = new TimeSpan(0, 1, 0);

                        transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
                    }
                }


                // Nos conectamos con Gestion
                conection = ConexionStatic.getConexionGST(true);

                if (bTransaccion && !bDistribuida)
                {
                    transaction = conection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Consolidado
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarConsolidado(bool bTransaccion)
        {
            ConectarConsolidado(bTransaccion, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Consolidado
        /// Se establece como parametro si se desea o no transaccion
        /// Se establece como parametro si se trata de una transaccion distribuida o no 
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <param name="bDistribuida">bool - Si se trata de una transaccion distribuida o no </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarConsolidado(bool bTransaccion, bool bDistribuida)
        {
            try
            {
                if (bSinTransaccionesDistribuidas)
                    bDistribuida = false;

                // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
                if (bDistribuida)
                {
                    if (Transaction.Current == null)
                    //if (transactionScope == null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = IsolationLevel.ReadCommitted;
                        options.Timeout = new TimeSpan(0, 1, 0);

                        transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
                    }
                }


                // Nos conectamos con Consolidado
                conection = ConexionStatic.getConexionConsolidado();
                if (bTransaccion && !bDistribuida)
                {
                    transaction = conection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Consolidado o de la Estacion, dependiendo del parametro.
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarConsolidadoPlaza(bool bConsolidado, bool bTransaccion)
        {
            ConectarConsolidadoPlaza(bConsolidado, bTransaccion, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Consolidado o de la Estacion, dependiendo del parametro.
        /// Se establece como parametro si se desea o no transaccion
        /// Se establece como parametro si se trata de una transaccion distribuida o no 
        /// </summary>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <param name="bConsolidado">bool - Si se desea conectar con Estacion o Consolidado</param>
        /// <param name="bDistribuida">bool - Si se trata de una transaccion distribuida o no </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarConsolidadoPlaza(bool bConsolidado, bool bTransaccion, bool bDistribuida)
        {
            try
            {
                if (bSinTransaccionesDistribuidas)
                    bDistribuida = false;

                // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
                if (bDistribuida)
                {
                    if (Transaction.Current == null)
                    //if (transactionScope == null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = IsolationLevel.ReadCommitted;
                        options.Timeout = new TimeSpan(0, 1, 0);

                        transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
                    }
                }


                // Nos conectamos a Consolidado o a la estacion de acuerdo al parametro
                conection = ConexionStatic.getConexionConsolidadoPlaza(bConsolidado);
                if (bTransaccion && !bDistribuida)
                {
                    transaction = conection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///// ***********************************************************************************************
        ///// <summary>
        ///// Se conecta al archivo de Excel Establecido por parametro.
        ///// </summary>
        ///// <param name="strDataSource">string - Archivo Excel al cual se conectara.</param>
        ///// <returns></returns>
        ///// ***********************************************************************************************
        //public void ConectarExcel(string strDataSource, bool bTransaccion, bool bDistribuida)
        //{
        //    try
        //    {
        //        if (bSinTransaccionesDistribuidas)
        //            bDistribuida = false;

        //        // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
        //        if (bDistribuida)
        //        {
        //            if (Transaction.Current == null)
        //            //if (transactionScope == null)
        //            {
        //                TransactionOptions options = new TransactionOptions();
        //                options.IsolationLevel = IsolationLevel.ReadCommitted;
        //                options.Timeout = new TimeSpan(0, 1, 0);

        //                transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
        //            }
        //        }

        //        // Nos conectamos a Consolidado o a la estacion de acuerdo al parametro
        //        oleConection = ConexionStatic.getConexionExcel(strDataSource);
        //        if (bTransaccion && !bDistribuida)
        //        {
        //            transaction = conection.BeginTransaction();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Gestion, si falla o no anda nos conectamos con la plaza
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <returns>true si pudo con GST</returns>
        /// ***********************************************************************************************
        public bool ConectarGSTThenPlaza()
        {
            bool bOK = false;
            try
            {
                // Nos conectamos con Gestion
                conection = ConexionStatic.getConexionGST(false);
                bOK = true;
            }
            catch
            {
                bOK = false;
            }
            if (!bOK)
            {
                //Fallo GST tratamos con la plaza
                conection = ConexionStatic.getConexionPlaza();
            }
            return bOK;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Consolidado, si falla o no anda nos conectamos con la plaza
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <returns>true si pudo con Consolidado</returns>
        /// ***********************************************************************************************
        public bool ConectarConsolidadoThenPlaza()
        {
            bool bOK = false;
            try
            {
                // Nos conectamos con Consolidado
                conection = ConexionStatic.getConexionConsolidado(false, 4);
                bOK = true;
            }
            catch
            {
                bOK = false;
            }
            if (!bOK)
            {
                //Fallo Consolidado tratamos con la plaza
                conection = ConexionStatic.getConexionPlaza();
            }
            return bOK;
        }


        /// <summary>
        /// Conecta a consolidado, si no puede se conecta a la plaza.
        /// </summary>
        /// <returns>Devuelve en un string si la conexion fue a la plaza o a consolidado o a ninguna</returns>
        public string ConectarConsolidadoThenPlaza_str()
        {
            bool bOK = false;
            string sCon = "";
            try
            {
                // Nos conectamos con Consolidado
                conection = ConexionStatic.getConexionConsolidado(false, 4);
                sCon = "CONSOLIDADO";
                bOK = true;
            }
            catch
            {
                bOK = false;
            }
            if (!bOK)
            {
                //Fallo Consolidado tratamos con la plaza

                conection = ConexionStatic.getConexionPlaza();
                sCon = "ESTACION";


            }
            return sCon;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Gestion o de la Estacion, dependiendo del parametro.
        /// Se establece como parametro si se desea o no transaccion
        /// </summary>
        /// <param name="bGST">bool - Si deseamos conectarnos con Gestion o con la estacion</param>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarGSTPlaza(bool bGST, bool bTransaccion)
        {
            ConectarGSTPlaza(bGST, bTransaccion, false);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Se conecta a la base de datos de Gestion o de la Estacion, dependiendo del parametro.
        /// Se establece como parametro si se desea o no transaccion
        /// Se establece como parametro si se trata de una transaccion distribuida o no 
        /// </summary>
        /// <param name="bGST">bool - Si deseamos conectarnos con Gestion o con la estacion</param>
        /// <param name="bTransaccion">bool - Si se desea o no transaccion</param>
        /// <param name="bDistribuida">bool - Si se trata de una transaccion distribuida o no </param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void ConectarGSTPlaza(bool bGST, bool bTransaccion, bool bDistribuida)
        {
            try
            {
                if (bSinTransaccionesDistribuidas)
                    bDistribuida = false;

                // Si realizamos una transaccion distribuida creamos un nuevo TransactionScope (si no estaba creado)
                if (bDistribuida)
                {
                    if (Transaction.Current == null)
                    //if (transactionScope == null)
                    {
                        TransactionOptions options = new TransactionOptions();
                        options.IsolationLevel = IsolationLevel.ReadCommitted;
                        options.Timeout = new TimeSpan(0, 1, 0);

                        transactionScope = new TransactionScope(TransactionScopeOption.Required, options);
                    }
                }


                // Nos conectamos a Gestion o a la estacion de acuerdo al parametro
                conection = ConexionStatic.getConexionGSTPlaza(bGST);
                if (bTransaccion && !bDistribuida)
                {
                    transaction = conection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Finaliza la transaccion actual de la conexion, sin cerrar la conexion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************       
        public void Commit()
        {
            Finalizar(true);

            ////try
            ////{
            ////    if (transaction != null)
            ////    {
            ////        transaction.Commit();
            ////        transaction = null;
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw ex;
            ////}
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Aborta la transaccion actual de la conexion, sin cerrar la conexion
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************       
        public void Rollback()
        {
            Finalizar(false);

            ////try
            ////{
            ////    if (transaction != null)
            ////    {
            ////        transaction.Rollback();
            ////        transaction = null;
            ////    }
            ////}
            ////catch (Exception ex)
            ////{
            ////    throw ex;
            ////}
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Metodo que realiza cierra la conexion actual
        /// </summary>
        /// <returns></returns>
        /// ***********************************************************************************************
        public void Dispose()
        {
            Finalizar(false);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Finaliza la transaccion actual de la conexion, cierra la conexion y libera punteros de memoria    
        /// Maneja tambien la transaccion distribuida, grabandola si el puntero NO es nulo.
        /// </summary>
        /// <param name="success">bool - Si se realizo correctamente el ultimo comando ejecutado y estamos 
        ///                              dentro de una transaccion, la graba, sino la aborta</param>
        /// <returns></returns>
        /// ***********************************************************************************************       
        public void Finalizar(bool success)
        {
            try
            {

                if (transaction != null)
                {
                    if (success)
                        transaction.Commit();
                    else
                        transaction.Rollback();

                    transaction = null;
                }
                if (conection != null)
                {
                    conection.Close();
                    conection = null;
                }

                if (transactionScope != null)
                {
                    if (success)
                    {
                        transactionScope.Complete();
                    }
                    transactionScope.Dispose();
                    transactionScope = null;
                }

                /////ORIGINAL
                ////if (transaction != null)
                ////{
                ////    if (success)
                ////        transaction.Commit();
                ////    else
                ////        transaction.Rollback();

                ////    transaction = null;
                ////}
                ////if (conection != null)
                ////{
                ////    conection.Close();
                ////    conection = null;
                ////}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

    #endregion



    #region CONEXIONSTATIC: Clase de manejo de conexiones pero con metodos estaticos

    static class ConexionStatic
    {
        //Hay conexion con Gestion?
        public static bool ConexionGestionOK = true;

        private static ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (HttpContext.Current == null)
                    return ConfigurationManager.ConnectionStrings;
                return WebConfigurationManager.ConnectionStrings;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Estacion 
        /// </summary>
        /// <returns>SqlConnection - Conexion con la base de datos de la estacion</returns>
        /// ***********************************************************************************************
        public static SqlConnection getConexionPlaza()
        {
            // Creamos el objeto de conexion
            SqlConnection oConn = new SqlConnection();

            oConn.ConnectionString = WebConfigurationManager.ConnectionStrings["ESTACION"].ConnectionString;

            try
            {
                oConn.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return oConn;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Consolidado
        /// </summary>
        /// <param name="bTest">bool - true si intentamos aun cuando sepamos que anda mal</param>
        /// <returns>SqlConnection - Conexion con la base de datos de consolidado</returns>
        /// ***********************************************************************************************                
        public static SqlConnection getConexionConsolidado(bool bTest)
        {
            return getConexionConsolidado(bTest, 15);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Consolidado
        /// </summary>
        /// <param name="bTest">bool - true si intentamos aun cuando sepamos que anda mal</param>
        /// <param name="timeOut">int - tiempo para dar taimeout de conexion en segundos</param>
        /// <returns>SqlConnection - Conexion con la base de datos de consolidado</returns>
        /// ***********************************************************************************************                
        public static SqlConnection getConexionConsolidado(bool bTest, int timeOut)
        {
            // Creamos el objeto de conexion
            SqlConnection oConn = new SqlConnection();

            oConn.ConnectionString =
                ConnectionStrings["CONSOLIDADO"].ConnectionString + "Connection Timeout=" + timeOut.ToString() + ";";

            try
            {
                if (bTest || ConexionGestionOK)
                {
                    oConn.Open();
                    ConexionGestionOK = true;
                }
                else
                {
                    throw new Exception("No hay conexión con Consolidado");
                }

            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return oConn;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Gestion
        /// </summary>
        /// <param name="bTest">bool - true si intentamos aun cuando sepamos que anda mal</param>
        /// <returns>SqlConnection - Conexion con la base de datos de gestion</returns>
        /// ***********************************************************************************************        
        public static SqlConnection getConexionGST(bool bTest)
        {
            // Creamos el objeto de conexion
            SqlConnection oConn = new SqlConnection();


            oConn.ConnectionString =
                WebConfigurationManager.ConnectionStrings["GESTION"].ConnectionString;


            try
            {
                if (bTest || ConexionGestionOK)
                {
                    oConn.Open();
                    ConexionGestionOK = true;
                }
                else
                {
                    throw new Exception("No hay conexión con GST");
                }

            }
            catch (SqlException ex)
            {
                ConexionGestionOK = false;
                throw ex;
            }

            return oConn;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Consolidado
        /// </summary>
        /// <returns>SqlConnection - Conexion con la base de datos de consolidado</returns>
        /// ***********************************************************************************************                
        public static SqlConnection getConexionConsolidado()
        {
            // Creamos el objeto de conexion
            SqlConnection oConn = new SqlConnection();

            oConn.ConnectionString =
                WebConfigurationManager.ConnectionStrings["CONSOLIDADO"].ConnectionString;

            try
            {
                oConn.Open();
            }
            catch (SqlException ex)
            {
                
                throw ex;
            }

            return oConn;
        }



        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la planilla excel establecida
        /// </summary>
        /// <param name="DataSource">string - Archivo el cual queremos abrir</param>
        /// <returns>OleDbConnection - Conexion con la base de datos de consolidado</returns>
        /// ***********************************************************************************************                
        public static OleDbConnection getConexionExcel(string DataSource)
        {
            // Creamos el objeto de conexion
            OleDbConnection oConn = new OleDbConnection();

            oConn.ConnectionString = WebConfigurationManager.ConnectionStrings["EXCEL"].ConnectionString.Replace("[*SOURCE*]", DataSource);

            try
            {
                oConn.Open();
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            return oConn;
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Gestion o de la Estacion, dependiendo del parametro
        /// </summary>
        /// <param name="bGST">bool - Si se desea o no conectar con GST (si es false, se conecta con la estacion)</param>
        /// <returns>SqlConnection - Conexion con la base de datos de GST o la estacion</returns>
        /// ***********************************************************************************************                       
        public static SqlConnection getConexionGSTPlaza(bool bGST)
        {
            if (bGST)
                return getConexionGST(true);
            else
                return getConexionPlaza();
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Retorna un objeto de conexion con la base de datos de Consolidado o de la Estacion, dependiendo del parametro
        /// </summary>
        /// <param name="bGST">bool - Si se desea o no conectar con Consolidado (si es false, se conecta con la estacion)</param>
        /// <returns>SqlConnection - Conexion con la base de datos de Consolidado o la estacion</returns>
        /// ***********************************************************************************************                              
        public static SqlConnection getConexionConsolidadoPlaza(bool bConsolidado)
        {
            if (bConsolidado)
                return getConexionConsolidado();
            else
                return getConexionPlaza();
        }


        ///// ***********************************************************************************************
        ///// <summary>
        ///// Retorna el string de conexion con el servidor y la base de datos que indican los parametros.
        ///// Utiliza el modo, usuario y password que se reciben por parametro.
        ///// </summary>
        ///// <param name="servidor">string - Nombre del servidor de datos</param>
        ///// <param name="database">string - Nombre de la base de datos a la que me deseo conectar</param>
        ///// <param name="seguridadintegrada">bool - Si se desea seguridad integrada o con usuario y password</param>
        ///// <param name="usuario">string - ID del usuario con el que me conecto con la base de datos</param>
        ///// <param name="password">string - Password del usuario con el que me conecta a la base de datos</param>
        ///// <returns>string - String de conexion con la base de datos utilizando los parametros recibidos</returns>
        ///// ***********************************************************************************************                              
        //private static string getConnectionString(string servidor, 
        //                                          string database, 
        //                                          bool seguridadintegrada, 
        //                                          string usuario, 
        //                                          string password)
        //{
        //    if (seguridadintegrada)
        //    {
        //        return "Persist Security Info=False;Integrated Security=SSPI;Initial Catalog=" + database + ";Data Source=" + servidor;
        //    }
        //    else
        //    {
        //        //return "Persist Security Info=False;User ID=" + usuario + ";Password=" + password + ";Initial Catalog=" + database + ";Data Source=" + servidor;
        //        return "Persist Security Info=False;User ID=" + usuario + ";Address=192.168.1.11:1238;Password=" + password + ";Initial Catalog=" + database + ";Data Source=" + servidor;
        //    }
        //}

    }

    #endregion

}

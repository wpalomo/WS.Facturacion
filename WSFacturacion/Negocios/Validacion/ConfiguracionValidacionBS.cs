using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;
using Telectronica.Validacion;


namespace Telectronica.Validacion
{
    public class ConfiguracionValidacionBS
    {
        #region Anomalia: Metodos de la Clase Anomalia.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una Anomalia determinada
        /// <returns>Lista de Anomalias</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getAnomalias(bool bGST, int? codigoAnomalia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getAnomalias(conn, codigoAnomalia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias definidas
        /// </summary>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una anomalia determinado
        /// <returns>Lista de Anomalias</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getAnomalias(int? codigoAnomalia)
        {
            return getAnomalias(ConexionBs.getGSToEstacion(), codigoAnomalia);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias definidas, sin filtro
        /// </summary>
        /// <returns>Lista de Anomalias</returns>
        /// ***********************************************************************************************
        public static AnomaliaL getAnomalias()
        {
            return getAnomalias(ConexionBs.getGSToEstacion(), null);
        }


        #endregion

        #region CodigoValidacion: Metodos de CodigoValidacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un codigo de validacion definido
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una Anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo de validacion
        /// <param name="codigo">Int - Permite filtrar por un codigo de validacion
        /// <returns>objeto codigo de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacion getCodigoValidacion(bool bGST, int? codigoAnomalia, string tipo, int? codigo)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getCodigoValidacion(conn, codigoAnomalia, tipo, codigo);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un codigo de validacion definido
        /// </summary>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo determinada
        /// <param name="codigo">Int - Permite filtrar por un codigo de validacion
        /// <returns>objeto codigo de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacion getCodigoValidacion(int? codigoAnomalia, string tipo, int? codigo)
        {
            return getCodigoValidacion(ConexionBs.getGSToEstacion(), codigoAnomalia, tipo, codigo);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de aceptacion y rechazo por tipo de anomalia y forma de pago
        /// </summary>        
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacionGeneral()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                    return ConfiguracionValidacionDt.getCodigosValidacionGeneral(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion definidos por forma de pago
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una Anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo de validacion
        /// <param name="medioPago">string - Tipo de medio de pago</param>
        /// <param name="formaPago">string - Subtipo de medio de pago</param>
        /// <param name="subformaPago">tinyint - Tipo de forma de pago</param>
        /// <returns>Lista de codigos de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion(bool bGST, int? codigoAnomalia, string tipo, String medioPago, String formaPago, int? subformaPago)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getCodigosValidacion(conn, codigoAnomalia, tipo, medioPago, formaPago, subformaPago);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion definidos por forma de pago
        /// </summary>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una Anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo de validacion
        /// <param name="medioPago">string - Tipo de medio de pago</param>
        /// <param name="formaPago">string - Subtipo de medio de pago</param>
        /// <param name="subformaPago">tinyint - Tipo de forma de pago</param>
        /// <returns>Lista de codigos de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion(int? codigoAnomalia, string tipo, String medioPago, String formaPago, int? subformaPago)
        {
            return getCodigosValidacion(ConexionBs.getGSToEstacion(), codigoAnomalia, tipo, medioPago, formaPago, subformaPago);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion definidos
        /// </summary>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una anomalia determinada
        /// <param name="tipo">String - Permite filtrar por un tipo determinada
        /// <returns>Lista de codigos de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion(int? codigoAnomalia, string tipo)
        {
            return getCodigosValidacion(ConexionBs.getGSToEstacion(), codigoAnomalia, tipo, null, null, null);
        }


        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion definidos
        /// </summary>
        /// <param name="codigoAnomalia">Int - Permite filtrar por una anomalia determinada
        /// <returns>Lista de codigos de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion(int? codigoAnomalia)
        {
            return getCodigosValidacion(ConexionBs.getGSToEstacion(), codigoAnomalia, null, null, null, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de codigos de validacion definidos, sin filtro
        /// </summary>
        /// <returns>Lista de codigos de validacion</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionL getCodigosValidacion()
        {
            return getCodigosValidacion(ConexionBs.getGSToEstacion(), null, null, null, null, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Codigo de Validacion
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacion - Estructura del Codigo de validacion a insertar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addCodigoValidacion(CodigoValidacion oCodigoValidacion)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos CodigoValidacion
                    ConfiguracionValidacionDt.addCodigoValidacion(oCodigoValidacion, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Codigo de Validacion
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacion - Estructura del Codigo de validacion a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCodigoValidacion(CodigoValidacion oCodigoValidacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos CodigoValidacion
                    ConfiguracionValidacionDt.updCodigoValidacion(oCodigoValidacion, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Codigo de Validacion
        /// </summary>
        /// <param name="Codigo">Int - codigo a eliminar</param>
        /// <param name="Tipo">String - tipo a eliminar</param>
        /// <param name="Anomalia">Int - anomalia a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCodigoValidacion(int? codigo, string tipo, int codAnomalia)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el Codigo de Validacion
                    ConfiguracionValidacionDt.delCodigoValidacion(conn, codigo, tipo, codAnomalia);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region CodigoValidacionFormaPago: Clase de Datos de CodigoValidacion.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias, formas de pago y tipo exento
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de anomalias, formas de pago y tipo exento</returns>
        /// ***********************************************************************************************
        public static AnomaliaFormaPagoL getAnomaliasFormaPago(bool bGST, int? codAnomalia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getAnomaliasFormaPago(conn, codAnomalia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias, formas de pago y tipo exento
        /// </summary>
        /// <returns>Lista de anomalias, formas de pago y tipo exento</returns>
        /// ***********************************************************************************************
        public static AnomaliaFormaPagoL getAnomaliasFormaPago()
        {
            return getAnomaliasFormaPago(ConexionBs.getGSToEstacion(), null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de anomalias, formas de pago y tipo exento
        /// </summary>
        /// <returns>Lista de anomalias, formas de pago y tipo exento</returns>
        /// ***********************************************************************************************
        public static AnomaliaFormaPagoL getAnomaliasFormaPago(int? codAnomalia)
        {
            return getAnomaliasFormaPago(ConexionBs.getGSToEstacion(), codAnomalia);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de formas de pago
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionFormaPagoL getCodigosValidacionFormaPago(bool bGST, int? codAnomalia, String medioPago, String formaPago, int? subformaPago)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getCodigosValidacionFormaPago(conn, codAnomalia, medioPago, formaPago, subformaPago);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de forma de pago
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static CodigoValidacionFormaPagoL getCodigosValidacionFormaPago(int? codAnomalia, String medioPago, String formaPago, int? subformaPago)
        {
            return getCodigosValidacionFormaPago(ConexionBs.getGSToEstacion(), codAnomalia, medioPago, formaPago, subformaPago);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de formas de pago
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static DataSet rptCodigosValidacionFormaPago(bool bGST, int? codAnomalia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.rptCodigosValidacionFormaPago(conn, codAnomalia);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de forma de pago
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static DataSet rptCodigosValidacionFormaPago(int? codAnomalia)
        {
            return rptCodigosValidacionFormaPago(ConexionBs.getGSToEstacion(), codAnomalia);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de forma de pago
        /// </summary>
        /// <returns>Lista de formas de pago</returns>
        /// ***********************************************************************************************
        public static DataSet rptCodigosValidacionFormaPago()
        {
            return rptCodigosValidacionFormaPago(ConexionBs.getGSToEstacion(), null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Codigo de Validacion por forma de pago
        /// </summary>
        /// <param name="oCodigoValidacion">CodigoValidacion - Estructura del Codigo de validacion a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCodigoValidacionFormaPago(CodigoValidacionFormaPago codigosValidacion)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos CodigoValidacion
                    ConfiguracionValidacionDt.updCodigoValidacionFormaPago(codigosValidacion, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Controles: Clase para entidad de Controles_Gral.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Controles
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="id">int - identidad de controles</param>
        /// <returns>Lista de Controles</returns>
        /// ***********************************************************************************************
        public static ControlesL getControles(bool bGST, int? id)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getControles(conn, id);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve un control
        /// </summary>
        /// <param name="id">int - identidad de controles</param>
        /// <returns>objeto controles</returns>
        /// ***********************************************************************************************
        public static Controles getControles(int? id)
        {
            return getControles(ConexionBs.getGSToEstacion(), id).FirstOrDefault();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de controles
        /// </summary>
        /// <returns>Lista de controles</returns>
        /// ***********************************************************************************************
        public static ControlesL getControles()
        {
            return getControles(ConexionBs.getGSToEstacion(), null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Validacion
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de Tipos de Validacion</returns>
        /// ***********************************************************************************************
        public static TipoValidacionL getTiposValidacion(bool bGST)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getTiposValidacion(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tipos de validacion
        /// </summary>
        /// <returns>Lista de Tipos de Validacion</returns>
        /// ***********************************************************************************************
        public static TipoValidacionL getTiposValidacion()
        {
            return getTiposValidacion(ConexionBs.getGSToEstacion());
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Validacion Invisible
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de Tipos de Validacion Invisible</returns>
        /// ***********************************************************************************************
        public static TipoValidacionInvisibleL getTiposValidacionInvisible(bool bGST, int? codAnomalia, String tipo, String invisible)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);

                    return ConfiguracionValidacionDt.getTiposValidacionInvisible(conn, codAnomalia, tipo, invisible);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de tipos de validacion Invisible
        /// </summary>
        /// <returns>Lista de Tipos de Validacion Invisible</returns>
        /// ***********************************************************************************************
        public static TipoValidacionInvisibleL getTiposValidacionInvisible(int? codAnomalia, String tipo, String invisible)
        {
            return getTiposValidacionInvisible(ConexionBs.getGSToEstacion(), codAnomalia, tipo, invisible);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de Controles
        /// </summary>
        /// <param name="oControles">Controles - Estructura del Control a insertar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addControles(Controles oControles)
        {
            try
            {

                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Agregamos CodigoValidacion
                    ConfiguracionValidacionDt.addControles(oControles, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de Controles
        /// </summary>
        /// <param name="oCodigoValidacion">Controles - Estructura del Control a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updControles(Controles oControles)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos CodigoValidacion
                    ConfiguracionValidacionDt.updControles(oControles, conn);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Control
        /// </summary>
        /// <param name="Codigo">Int - Identity a eliminar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delControles(int id)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //eliminamos el Codigo de Validacion
                    ConfiguracionValidacionDt.delControles(conn, id);

                    //Grabo OK hacemos COMMIT
                    conn.Finalizar(true);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}

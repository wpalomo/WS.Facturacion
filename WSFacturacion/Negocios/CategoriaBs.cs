using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Clase para administracion de Categorias 
    /// </summary>
    ///****************************************************************************************************
    public static class CategoriaBs
    {
        #region CATEGORIA: Clase de Negocios de Categorias manuales definidas
        
        public static DataSet getRptCategorias() {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return CategoriaDt.getRptCategorias(conn, null, false, mdlGeneral.getPathImagenesCatego());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de TODAS Categorias manuales definidas.
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static CategoriaManualL getCategorias()
        {
            return getCategorias(null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Categorias manuales definidas.
        /// </summary>
        /// <param name="numeroCategoria">int - Numero de categoria por la cual filtrar la busqueda</param>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static CategoriaManualL getCategorias(int? numeroCategoria)
        {
            bool PudoGST;
            return getCategorias(ConexionBs.getGSToEstacion(), numeroCategoria, true, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Categorias manuales definidas, permitiendo filtrar la categoria 0 o no
        /// </summary>
        /// <param name="incluirCategoriaCero">bool - Indica si se incluye o no la categoria cero</param>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static CategoriaManualL getCategorias(bool incluirCategoriaCero)
        {
            bool PudoGST;
            return getCategorias(ConexionBs.getGSToEstacion(), null, incluirCategoriaCero, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Categorias manuales definidas.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="numeroCategoria">int - Numero de categoria por la cual filtrar la busqueda</param>
        /// <param name="incluirCategoriaCero">bool - Permite filtrar, trayendo o no la categoria cero</param>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static CategoriaManualL getCategorias(bool bGST, int? numeroCategoria, bool incluirCategoriaCero, out bool PudoGST)
        {
            PudoGST = false;
            try
            {
                //Seteamos path base de las imagenes
                CategoriaManual.PathImagenes = mdlGeneral.getPathImagenesCatego();

                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    if (bGST)
                        PudoGST = conn.ConectarGSTThenPlaza();
                    else
                        conn.ConectarPlaza(false);

                    return CategoriaDt.getCategorias(conn, numeroCategoria, incluirCategoriaCero);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Categoria Manual 
        /// </summary>
        /// <param name="oCateogriaManual">CategoriaManual - Objeto CategoriaManual
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addCategoriaManual(CategoriaManual oCategoriaManual)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos Categoria Manual
                CategoriaDt.addCategoriaManual(oCategoriaManual, conn);

                //Grabamos la habilitacion de las formas de pago para la categoria dada
                updCategoriaFormaPago(oCategoriaManual.FormasPagoHabilitadas, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oCategoriaManual),
                                                        getAuditoriaDescripcion(oCategoriaManual)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de una Categoria Manual.
        /// </summary>
        /// <param name="oCategoriaManual">CategoriaManual - Objeto Categoria Manual
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCategoriaManual(CategoriaManual oCategoriaManual)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Modificamos Categoria Manual
                    CategoriaDt.updCategoriaManual(oCategoriaManual, conn);

                    //Grabamos la habilitacion de las formas de pago para la categoria dada
                    updCategoriaFormaPago(oCategoriaManual.FormasPagoHabilitadas, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "M",
                                                           getAuditoriaCodigoRegistro(oCategoriaManual),
                                                           getAuditoriaDescripcion(oCategoriaManual)),
                                                           conn);

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
        /// Baja de una Categoria Manual.
        /// </summary>
        /// <param name="oCategoriaManual">Categoria Manual - Objeto Categoria Manual
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCategoriaManual(CategoriaManual oCategoriaManual, bool nocheck)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    //Verificar que no haya registros con FK a este
                    MantenimientoBS.checkReferenciasFK(conn, "CATMAN", 
                                                       new string[] {  oCategoriaManual.Categoria.ToString() },
                                                       new string[] { "FORCAT", "TARIFADET" },
                                                       nocheck);

                    //Grabamos la habilitacion de las formas de pago para la categoria dada
                    CategoriaDt.delCategoriasFormaPago(oCategoriaManual, conn);

                    //eliminamos la Categoria Manual
                    CategoriaDt.delCategoriaManual(oCategoriaManual, conn);

                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoria(),
                                                           "B",
                                                           getAuditoriaCodigoRegistro(oCategoriaManual),
                                                           getAuditoriaDescripcion(oCategoriaManual)),
                                                           conn);

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
        /// Centralizamos la grabacion de la habilitacion de la lista de forma de pago por categoria 
        /// </summary>
        /// <param name="oCategoriaManual">Categoria Manual - Objeto Categoria Manual
        /// <returns></returns>
        /// ***********************************************************************************************
        private static void updCategoriaFormaPago(CategoriaFormaPagoL oCategoriaFormaPagoL, Conexion oConn)
        {
            foreach (CategoriaFormaPago oCategoriaFormaPago in oCategoriaFormaPagoL)
            {
                // Si no esta habilitada la eliminamos, sino la insertamos (el SP de agregar se encarga de ver si esta o no)
                if (oCategoriaFormaPago.Habilitada)
                {
                    CategoriaDt.addCategoriaFormaPago(oCategoriaFormaPago, oConn);
                }
                else
                {
                    CategoriaDt.delCategoriaFormaPago(oCategoriaFormaPago, oConn);
                }                
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoria()
        {
            return "TAR";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(CategoriaManual oCategoriaManual)
        {
            return oCategoriaManual.Categoria.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(CategoriaManual oCategoriaManual)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Descripción", oCategoriaManual.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Descripción Larga", oCategoriaManual.DescripcionLarga);
            AuditoriaBs.AppendCampo(sb, "Equivalente", Convert.ToString(oCategoriaManual.Equivalente));
            AuditoriaBs.AppendCampo(sb, "Equivalente CGMP", Convert.ToString(oCategoriaManual.EquivalenteCGMP));
            AuditoriaBs.AppendCampo(sb, "Equivalente ANTT", Convert.ToString(oCategoriaManual.EquivalenteANTT));
            AuditoriaBs.AppendCampo(sb, "Imagen Asociada", oCategoriaManual.Imagen);
            AuditoriaBs.AppendCampo(sb, "Formas de Pago Habilitadas", "");
            foreach (CategoriaFormaPago item in oCategoriaManual.FormasPagoHabilitadas)
            {
                if (item.Habilitada)
                {
                    sb.Append(item.CodigoFormaPago + " ");
                }
            }

            return sb.ToString();
        }

        #endregion
        
        #endregion
        
        #region CATEGORIADAC: Metodos de la Clase de Negocios de la entidad Categorias del DAC (automaticas).
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Categorias del DAC definidas.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de Categorias DAC</returns>
        /// ***********************************************************************************************
        public static CategoriaDACL getCategoriasDAC()
        {
            return getCategoriasDAC(ConexionBs.getGSToEstacion());
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Categorias del DAC definidas.
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <returns>Lista de Categorias DAC</returns>
        /// ***********************************************************************************************
        public static CategoriaDACL getCategoriasDAC(bool bGST)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    CategoriaDACL retCateg = CategoriaDt.getCategoriasDAC(conn);
                    return retCateg;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Actualizacion de las categorias del DAC (automaticas).
        /// </summary>
        /// <param name="oCategoriasDAC">CategoriaDACL - Lista de categorias automaticas
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCategoriasDAC(CategoriaDACL oCategoriasDAC)
        {
            try
            {
                //iniciamos una transaccion
                using (Conexion conn = new Conexion())
                {
                    //Siempre en Gestion, con transaccion
                    conn.ConectarGST(true);

                    // Recorremos la lista de categorias del DAC y grabamos en la base de datos
                    foreach (CategoriaDAC oCateg in oCategoriasDAC)
                    {
                        CategoriaDt.updCategoriaDAC(oCateg, conn);
                    }

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
        
        #region CATEGORIAFORMAPAGO: Clase de Negocios de Formas de Pago habilitadas por Categoria

        public static DataSet getRptCategoriasFormasPago()
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                    return CategoriaDt.getRptCategoriasFormaPago(conn);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago (con su estado de habilitacion) anexada a la lista de 
        /// Categoria manuales recibida por parametro.
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static void getCategoriasFormasPago(CategoriaManualL oCategoriaManualL)
        {
            foreach (CategoriaManual oCategManual in oCategoriaManualL)
            {
                CategoriaBs.getCategoriasFormasPago(oCategManual, true);
            }        
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago (con su estado de habilitacion) anexada a la lista de 
        /// Categoria manuales recibida por parametro.
        /// Por defecto, 
        /// </summary>
        /// <returns>Lista de Formas de pago para la Categoria manual</returns>
        /// ***********************************************************************************************
        public static void getCategoriasFormasPago(CategoriaManualL oCategoriaManualL, bool? cobraVia)
        {
            foreach (CategoriaManual oCategManual in oCategoriaManualL)
            {
                CategoriaBs.getCategoriasFormasPago(oCategManual, cobraVia);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago (con su estado de habilitacion) anexada a un objeto CategoriaManual
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static void getCategoriasFormasPago(CategoriaManual oCategoriaManual)
        {
            CategoriaBs.getCategoriasFormasPago(ConexionBs.getGSToEstacion(), oCategoriaManual, true);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago (con su estado de habilitacion) anexada a un objeto CategoriaManual
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static void getCategoriasFormasPago(CategoriaManual oCategoriaManual, bool? cobraVia)
        {
            CategoriaBs.getCategoriasFormasPago(ConexionBs.getGSToEstacion(), oCategoriaManual, cobraVia);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Formas de Pago (con su estado de habilitacion) anexada a un objeto CategoriaManual
        /// METODO MAS INTERNO, EL QUE ACCEDE A LA DATA
        /// </summary>
        /// <returns>Lista de Categorias manuales</returns>
        /// ***********************************************************************************************
        public static void getCategoriasFormasPago(bool bGST, CategoriaManual oCategoriaManual, bool? cobraVia)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarGSTPlaza(bGST, false);
                    // En la coleccion de formas de pago habilitadas le asigno la lista de formas de pago (metodo de data)
                    oCategoriaManual.FormasPagoHabilitadas = CategoriaDt.getCategoriasFormaPago(conn, oCategoriaManual.Categoria, cobraVia);
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

using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Utilitarios;

namespace Telectronica.Facturacion
{
    public class ClienteBS
    {
        #region MARCA: Metodos de la Clase Marca.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Marcas definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Marcas</returns>
        /// ***********************************************************************************************
        public static VehiculoMarcaL getMarcas()
        {
            bool PudoGST;
            return getMarcas(ConexionBs.getGSToEstacion(), null, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas las Marcas definidas. 
        /// </summary>
        /// <param name="codigoMarcas">Int - Permite filtrar por una marca determinada
        /// <returns>Lista de Marcas</returns>
        /// ***********************************************************************************************
        public static VehiculoMarcaL getMarcas(int? codigoMarcas)
        {
            bool PudoGST;
            return getMarcas(ConexionBs.getGSToEstacion(), codigoMarcas, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Marcas definidas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMarca    ">Int - Permite filtrar por una Marca determinada
        /// <returns>Lista de Marcas</returns>
        /// ***********************************************************************************************
        public static VehiculoMarcaL getMarcas(bool bGST, int? codigoMarcas, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                    PudoGST = conn.ConectarGSTThenPlaza();
                else
                    conn.ConectarPlaza(false);

                return ClienteDT.getMarcas(conn, codigoMarcas);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Marca
        /// </summary>
        /// <param name="oMarca">Marcas - Estructura de la Marca a insertar
        /// <returns>Lista de Marcas</returns>
        /// ***********************************************************************************************
        public static void addMarcas(VehiculoMarca oMarca)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos Marca
                ClienteDT.addMarcas(oMarca, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMarca(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oMarca),
                                                        getAuditoriaDescripcion(oMarca)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Marca
        /// </summary>
        /// <param name="oMarca">Marcas - Estructura de la Marca a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updMarca(VehiculoMarca oMarca)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Marca

                //Modificamos Marca

                ClienteDT.updMarcas(oMarca, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMarca(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oMarca),
                                                        getAuditoriaDescripcion(oMarca)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Marca
        /// </summary>
        /// <param name="oMarca">Marca - Estructura de la Marca a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delMarca(VehiculoMarca oMarca, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "MARCAS", 
                                                    new string[] { oMarca.Codigo.ToString() },
                                                    new string[] { },
                                                    nocheck);

                //eliminamos la Moneda
                ClienteDT.delMarca(oMarca, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaMarca(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oMarca),
                                    getAuditoriaDescripcion(oMarca)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaMarca()
        {
            return "MAR";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(VehiculoMarca oMarca)
        {
            return oMarca.Codigo.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(VehiculoMarca oMarca)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción", oMarca.Descripcion);
            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region MODELO: Metodos de la Clase Modelo.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Modelos definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Modelos</returns>
        /// ***********************************************************************************************
        public static VehiculoModeloL getModelos()
        {
            bool PudoGST;
            return getModelos(ConexionBs.getGSToEstacion(), null, null, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Modelos definidas. 
        /// </summary>
        /// <param name="codigoMarca>Int - Permite filtrar por una marca determinada
        /// <param name="codigoModelo">Int - Permite filtrar por un modelo determinado
        /// <returns>Lista de Modelos</returns>
        /// ***********************************************************************************************
        public static VehiculoModeloL getModelos(int? CodigoMarca, int? CodigoModelo)
        {
            bool PudoGST;
            return getModelos(ConexionBs.getGSToEstacion(), CodigoMarca, CodigoModelo,out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Modelos definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoMarca">Int - Permite filtrar por una Marca determinada
        /// <param name="codigoModelo">Int - Permite filtrar por un Modelo determinado
        /// <returns>Lista de Modelos</returns>
        /// ***********************************************************************************************
        public static VehiculoModeloL getModelos(bool bGST, int? CodigoMarca, int? CodigoModelo, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return ClienteDT.getModelos(conn, CodigoMarca,CodigoModelo);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Modelo
        /// </summary>
        /// <param name="oModelo">Modelos - Estructura del Modelo a insertar
        /// <returns>Lista de Modelos</returns>
        /// ***********************************************************************************************
        public static void addModelos(VehiculoModelo oModelo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos Modelo
                ClienteDT.addModelos(oModelo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaModelo(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oModelo),
                                                        getAuditoriaDescripcion(oModelo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Modelo
        /// </summary>
        /// <param name="oModelo">Modelo - Estructura del Modelo a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updModelo(VehiculoModelo oModelo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista el Modelo

                //Modificamos Modelo

                ClienteDT.updModelos(oModelo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaModelo(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oModelo),
                                                        getAuditoriaDescripcion(oModelo)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Modelo
        /// </summary>
        /// <param name="oModelo">Modelo - Estructura del Modelo a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delModelo(VehiculoModelo oModelo, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "MODELOS",
                                                    new string[] { oModelo.Marca.Codigo.ToString(), oModelo.Codigo.ToString() }, 
                                                    new string[] { }, 
                                                    nocheck);

                //eliminamos la Moneda
                ClienteDT.delModelo(oModelo, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaModelo(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oModelo),
                                    getAuditoriaDescripcion(oModelo)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaModelo()
        {
            return "MOD";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(VehiculoModelo oModelo)
        {
            return oModelo.Codigo.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(VehiculoModelo oModelo)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Marca", oModelo.Marca.Descripcion );
            AuditoriaBs.AppendCampo(sb, "Descripción", oModelo.Descripcion);
            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region COLOR: Metodos de la Clase Color.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Colores definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static VehiculoColorL getColores()
        {
            bool PudoGST;
            return getColores(ConexionBs.getGSToEstacion(), null, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todas los Colores definidos. 
        /// </summary>
        /// <param name="codigoColor">Int - Permite filtrar por un Color determinado
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static VehiculoColorL getColores(int? codigoColor)
        {
            bool PudoGST;
            return getColores(ConexionBs.getGSToEstacion(), codigoColor, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Colores definidos
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigoColor">Int - Permite filtrar por un Color determinada
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static VehiculoColorL getColores(bool bGST, int? codigoColor, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                    PudoGST = conn.ConectarGSTThenPlaza();
                else
                    conn.ConectarPlaza(false);

                return ClienteDT.getColores(conn, codigoColor);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Color
        /// </summary>
        /// <param name="oColor">Colores - Estructura del Color a insertar a insertar
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static void addColores(VehiculoColor oColor)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos el Color
                ClienteDT.addColor(oColor, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaColor(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oColor),
                                                        getAuditoriaDescripcion(oColor)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Color
        /// </summary>
        /// <param name="oColor">Color - Estructura del Color a modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updColor(VehiculoColor oColor)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista el Color

                //Modificamos el Color

                ClienteDT.updColores(oColor, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaColor(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oColor),
                                                        getAuditoriaDescripcion(oColor)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Color
        /// </summary>
        /// <param name="oColor">Color - Estructura del Color a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delColor(VehiculoColor oColor, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "COLOR", 
                                                    new string[] { oColor.Codigo.ToString() },
                                                    new string[] { }, 
                                                    nocheck);

                //eliminamos el Color
                ClienteDT.delColor(oColor, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaColor(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oColor),
                                    getAuditoriaDescripcion(oColor)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaColor()
        {
            return "COL";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(VehiculoColor oColor)
        {
            return oColor.Codigo.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(VehiculoColor oColor)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción", oColor.Descripcion);
            return sb.ToString();
        }

        #endregion

        #endregion

        #region AGRUPACIONCUENTA: Metodos de la Clase Agrupaciones de Cuentas.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve lista de Agrupación de Cuentas, sin filtro
        /// </summary>
        /// <returns>Lista de Agrupación de Cuentas</returns>
        /// ***********************************************************************************************
        public static AgrupacionCuentaL getAgrupacionesCuentas()
        {
            bool PudoGST;
            return getAgrupacionesCuentas(ConexionBs.getGSToEstacion(), null, null, out PudoGST);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve lista de Agrupación de Cuentas
        /// </summary>
        /// <param name="TipoCuenta - Permite filtrar por una Agrupación de Cuentas determinado
        /// <param name="FormaPagos - Permite filtrar por una Agrupación de Cuentas determinado
        /// <returns>Lista de Agrupación de Cuentas</returns>
        /// ***********************************************************************************************
        public static AgrupacionCuentaL getAgrupacionesCuentas(int? TipoCuenta, int? FormaPagos)
        {
            bool PudoGST;
            return getAgrupacionesCuentas(ConexionBs.getGSToEstacion(), TipoCuenta, FormaPagos, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve lista de Agrupación de Cuentas
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="codigo">Int - Permite filtrar por una Agrupación de Cuentas determinada
        /// <returns>Lista de Agrupación de Cuentas</returns>
        /// ***********************************************************************************************
        public static AgrupacionCuentaL getAgrupacionesCuentas(bool bGST, int? TipoCuenta, int? FormaPagos, out bool PudoGST)
        {
            PudoGST = false;

            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return ClienteDT.getAgrupacionesCuentas(conn, TipoCuenta, FormaPagos);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Agrupación de Cuentas
        /// </summary>
        /// <param name="oAgrupacionCuenta">AgrupacionCuenta -  Estructura de la Agrupacion de Cuenta a eliminar
        /// <returns>Lista de Agrupación de Cuentas</returns>
        /// ***********************************************************************************************
        public static void addAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos Agrupación de Cuentas 
                ClienteDT.addAgrupacionCuenta(oAgrupacionCuenta, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacionCuenta(),
                                                        "A",
                                                        getAuditoriaCodigoRegistro(oAgrupacionCuenta),
                                                        getAuditoriaDescripcion(oAgrupacionCuenta)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Agrupación de Cuentas
        /// </summary>
        /// <param name="oAgrupacionCuenta">AgrupacionCuenta -  Estructura de la Agrupacion de Cuenta a eliminar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos que ya exista la Agrupación de Cuentas 

                //Modificamos Agrupación de Cuentas 

                ClienteDT.updAgrupacionCuenta(oAgrupacionCuenta, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacionCuenta(),
                                                        "M",
                                                        getAuditoriaCodigoRegistro(oAgrupacionCuenta),
                                                        getAuditoriaDescripcion(oAgrupacionCuenta)),
                                                        conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Agrupación de Cuentas 
        /// </summary>
        /// <param name="oAgrupacionCuenta">AgrupacionCuenta - Estructura de la Agrupacion de Cuenta a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta, bool nocheck)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "CTAGRU", 
                                                    new string[] { oAgrupacionCuenta.TipoCuenta.CodigoTipoCuenta.ToString(), oAgrupacionCuenta.SubTipoCuenta.ToString() },
                                                    new string[] { },
                                                    nocheck);


                //eliminamos la Agrupación de Cuentas 
                ClienteDT.delAgrupacionCuenta(oAgrupacionCuenta, conn);

                //Grabamos auditoria
                AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaAgrupacionCuenta(),
                                    "B",
                                    getAuditoriaCodigoRegistro(oAgrupacionCuenta),
                                    getAuditoriaDescripcion(oAgrupacionCuenta)),
                                    conn);

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaAgrupacionCuenta()
        {
            return "COA";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(AgrupacionCuenta oAgrupacionCuenta)
        {
            return oAgrupacionCuenta.TipoCuenta.ToString() + " " + oAgrupacionCuenta.SubTipoCuenta.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(AgrupacionCuenta oAgrupacionCuenta)
        {
            StringBuilder sb = new StringBuilder();
            AuditoriaBs.AppendCampo(sb, "Descripción", oAgrupacionCuenta.DescrAgrupacion);
            AuditoriaBs.AppendCampo(sb, "Tipo Tarifa", oAgrupacionCuenta.DescripcionTipoTarifa);
            if (oAgrupacionCuenta.TipoTarifaVenta != null)
            {
                AuditoriaBs.AppendCampo(sb, "Tipo Tarifa Ventas", oAgrupacionCuenta.TipoTarifaVenta.ToString());
            }
            if (oAgrupacionCuenta.DiasDuracionCuenta != null)
            {
                AuditoriaBs.AppendCampo(sb, "Días Beneficio", oAgrupacionCuenta.DiasDuracionCuenta.ToString());
            }
            if (oAgrupacionCuenta.DiasAntesVencimiento != null)
            {
                AuditoriaBs.AppendCampo(sb, "Días Papeles", oAgrupacionCuenta.DiasAntesVencimiento.ToString());
            }
            return sb.ToString();
        }

        #endregion
        
        #endregion

        #region TIPOCUENTA: Metodos de la Clase TipoCuenta.

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cuentas. 
        /// </summary>
        /// <returns>Lista de tipos de Cuentas</returns>
        /// ***********************************************************************************************
        public static TipoCuentaL getTipoCuenta()
        {
            bool PudoGST;
            return getTipoCuenta(ConexionBs.getGSToEstacion(), out PudoGST, false);
        }

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cuentas. 
        /// </summary>
        /// <returns>Lista de tipos de Cuentas</returns>
        /// ***********************************************************************************************
        public static TipoCuentaL getTipoCuenta(bool SoloConPrepago)
        {
            bool PudoGST;
            return getTipoCuenta(ConexionBs.getGSToEstacion(), out PudoGST, SoloConPrepago);
        }

        /// <summary>
        /// Devuelve la lista de todos los Tipos de Cuentas. 
        /// </summary>
        /// <returns>Lista de tipos de Cuentas</returns>
        /// ***********************************************************************************************
        public static TipoCuentaL getTipoCuenta(bool bGST, out bool PudoGST, bool SoloConPrepago)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }
                
                TipoCuentaL oTipoCuentaL = new TipoCuentaL();
                oTipoCuentaL = ClienteDT.getTipoCuentas(conn, SoloConPrepago);
                return oTipoCuentaL;
            }
        } 

        #endregion

        #region CLIENTE: Metodos de la clase CLIENTE
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Clientes
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientes()
        {
            return getClientes(null, null, null, null, null, null, null, null, null, false, null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes que poseen la placa recibida por parametro
        /// <param name="patente">string - Patente del cliente que deseo buscar</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesPorPatente(string patente)
        {
            return getClientes(null, patente, null, null, null, null, null, null, null, false, null);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes cuyo documento es
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientePorDocumento(string documento)
        {
            return getClientes(null, null, null, documento, null, null, null, null, null, false, null);
        }
                
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes cuyo nombre incluye el parametro
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientes(string nombre)
        {
            return getClientes(null, null, null, null, nombre, null, null, null, null, false, null);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes. Segun los parametros recibidos utiliza diferentes metodos de data.
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="patente">string - Patente de algun vehiculo del cliente</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="numeroTag">string - Numero de tag que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjeta">string - Numero de tarjeta chip que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjetaExterno">int - Numero externo de una tarjeta chip del cliente</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <param name="locales">bool - Si se desean los clientes locales de la estacion o solo de gestion</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientes(int? numeroCliente,
                                           string patente, int? tipoDocumento,
                                           string documento, string nombre,
                                           string numeroTag, string numeroTarjeta,
                                           int? numeroTarjetaExterno, string expediente,
                                           bool locales, int? numeroTagExterno)
        {
            bool PudoGST;
            return getClientes(ConexionBs.getGSToEstacion(), numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales, false, numeroTagExterno, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes. Le anexa la lista de vehiculos, la lista de cuentas y/o la 
        /// lista de saldos por estacion a demanda (por parametro). 
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesCompleto(bool bGST, int numeroCliente,
                                                   bool incluirListaVehiculos,
                                                   bool incluirListaCuentas,
                                                   bool incluirListaSaldosPrepagos, out bool PudoGST)
        {
            ClienteL oClientes = null;
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {

                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                oClientes = getClientes(conn, numeroCliente, null, null, null, null, null, null, null, null, true, false, null);

                foreach (Cliente oCli in oClientes)
                {
                    if (!oCli.esClienteLocal)
                    {
                        // Le anexamos la lista de vehiculos del cliente
                        if (incluirListaVehiculos)
                        {
                            oCli.Vehiculos = ClienteDT.getVehiculosCliente(conn, oCli.NumeroCliente);
                        }

                        // Le anexamos la lista de Cuentas
                        if (incluirListaCuentas)
                        {
                            oCli.Cuentas = ClienteDT.getCuentasCliente(conn, oCli.NumeroCliente);                     
                            //Estaciones habilitadas de cada cuenta
                            ClienteDT.getEstacionesPorCuentas(conn, oCli);

                            //Tomamos la cuenta prepaga habilitada en la estacion (o cualquiera si es administrativa)
                            foreach (Cuenta item in oCli.Cuentas)
                            {
                                bool bOK = false;
                                if (item.TipoCuenta.EsPrepago)
                                {
                                    if (ConexionBs.getNumeroEstacion() >= ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
                                    {
                                        bOK = true;
                                    }
                                    else
                                    {
                                        CuentaEstacion oEst = item.EstacionesHabilitadas.FindEstacion(ConexionBs.getNumeroEstacion());
                                        if (oEst != null && oEst.Habilitado)
                                        {
                                            bOK = true;
                                        }
                                    }
                                }
                                if (bOK)
                                {
                                    oCli.CuentaPrepaga = item;

                                    //Obtenemos Montos Minimo y Maximo
                                    //En la estacion administrativa el minimo y maximo de todas las estaciones
                                    int? estacion = null;
                                    if (ConexionBs.getNumeroEstacion() < ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
                                    {
                                        estacion = ConexionBs.getNumeroEstacion();
                                    }
                                    RecargaPosibleL oRecargas = PrepagoDT.getRecargasPosibles(conn, estacion, item.TipoCuenta.CodigoTipoCuenta, item.Agrupacion.SubTipoCuenta);
                                    if (oRecargas.Count > 0)
                                    {
                                        oCli.CuentaPrepaga.MontoMinimoRecarga = oRecargas.FindMinimo().Monto;
                                        oCli.CuentaPrepaga.MontoMaximoRecarga = oRecargas.FindMaximo().Monto;
                                    }
                                    break;
                                }
                            }

                            oCli.TieneCuentaPospago = false;

                            foreach (Cuenta item in oCli.Cuentas)
                            {
                                bool bOK = false;
                                if (item.TipoCuenta.EsPospago)
                                {
                                    if (ConexionBs.getNumeroEstacion() >= ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
                                    {
                                        bOK = true;
                                    }
                                    else
                                    {
                                        CuentaEstacion oEst = item.EstacionesHabilitadas.FindEstacion(ConexionBs.getNumeroEstacion());
                                        if (oEst != null && oEst.Habilitado)
                                        {
                                            bOK = true;
                                        }
                                    }
                                }
                                if (bOK)
                                {
                                    oCli.TieneCuentaPospago = true;
                                    break;
                                }
                            }
                        }

                        //Vehiculos Habilitados por Cuenta
                        if (incluirListaCuentas && incluirListaVehiculos)
                        {
                            ClienteDT.getVehiculosPorCuentas(conn, oCli);
                        }

                        // Le anexamos la lista de Saldos Prepagos
                        if (incluirListaSaldosPrepagos)
                        {
                            oCli.SaldosPrepagos = ClienteDT.getSaldosCliente(conn, oCli.NumeroCliente, null);
                            foreach (SaldoPrepago item in oCli.SaldosPrepagos)
                            {
                                if (item.Zona.Codigo == ConexionBs.getZonaActual())
                                {
                                    oCli.SaldoPrepago = item;
                                }
                            }
                        }
                    }
                }
            }
            return oClientes;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes. Segun los parametros recibidos utiliza diferentes metodos de data.
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="patente">string - Patente de algun vehiculo del cliente</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="numeroTag">string - Numero de tag que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjeta">string - Numero de tarjeta chip que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjetaExterno">int - Numero externo de una tarjeta chip del cliente</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <param name="locales">bool - Si se desean los clientes locales de la estacion o solo de gestion</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        /* public static ClienteL getClientes(bool bGST, int? numeroCliente,
                                            string patente, int? tipoDocumento,
                                            string documento, string nombre,
                                            string numeroTag, string numeroTarjeta,
                                            int? numeroTarjetaExterno, string expediente,
                                            bool locales, out bool PudoGST , out bool hayMas)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                    PudoGST = conn.ConectarGSTThenPlaza();
                else
                    conn.ConectarPlaza(false);

                return getClientes(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales ,hayMas);
            }
        }*/
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes. Prueba en GST y si falla en la plaza. sin limite de registros
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="patente">string - Patente de algun vehiculo del cliente</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="numeroTag">string - Numero de tag que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjeta">string - Numero de tarjeta chip que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjetaExterno">int - Numero externo de una tarjeta chip del cliente</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <param name="locales">bool - Si se desean los clientes locales de la estacion o solo de gestion</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientes(bool bGST, int? numeroCliente, 
                                           string patente, int?tipoDocumento, 
                                           string documento, string nombre, 
                                           string numeroTag, string numeroTarjeta, 
                                           int? numeroTarjetaExterno, string expediente, 
                                           bool locales, bool conConsumidorFinal, int? numeroTagExterno, out bool PudoGST)                
        {
            bool llegoAlTope = false;

            return getClientes(bGST, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales, out PudoGST, null, out llegoAlTope, conConsumidorFinal, numeroTagExterno);

            /*
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                    PudoGST = conn.ConectarGSTThenPlaza();
                else
                    conn.ConectarPlaza(false);

                return getClientes(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales , conConsumidorFinal, numeroTagExterno);
                   
            }
            */
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Clientes. cantidad limitada de registros
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="patente">string - Patente de algun vehiculo del cliente</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="numeroTag">string - Numero de tag que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjeta">string - Numero de tarjeta chip que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjetaExterno">int - Numero externo de una tarjeta chip del cliente</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <param name="locales">bool - Si se desean los clientes locales de la estacion o solo de gestion</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// </summary>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientes(bool bGST, int? numeroCliente,
                                           string patente, int? tipoDocumento,
                                           string documento, string nombre,
                                           string numeroTag, string numeroTarjeta,
                                           int? numeroTarjetaExterno, string expediente,
                                           bool locales, out bool PudoGST, int? xiCantRows, out bool llegoAlTope, bool conConsumidorFinal, int? numeroTagExterno)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return getClientesLimitado(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales, xiCantRows, out  llegoAlTope, conConsumidorFinal, numeroTagExterno);
            }
        }

        /// <summary>
        /// Obtiene los clientes, recibe conexion abierta pero sin cantidad de filas maxima
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="numeroCliente"></param>
        /// <param name="patente"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="documento"></param>
        /// <param name="nombre"></param>
        /// <param name="numeroTag"></param>
        /// <param name="numeroTarjeta"></param>
        /// <param name="numeroTarjetaExterno"></param>
        /// <param name="expediente"></param>
        /// <param name="locales"></param>
        /// <param name="conConsumidorFinal"></param>
        /// <param name="numeroTagExterno"></param>
        /// <returns></returns>
        public static ClienteL getClientes(Conexion conn, int? numeroCliente,
                                           string patente, int? tipoDocumento,
                                           string documento, string nombre,
                                           string numeroTag, string numeroTarjeta,
                                           int? numeroTarjetaExterno, string expediente,
                                           bool locales, bool conConsumidorFinal,
                                           int? numeroTagExterno)
        {
            bool llegoAlTope = false;
            return getClientesLimitado(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, locales, null, out llegoAlTope, conConsumidorFinal, numeroTagExterno);

            /*
            ClienteL oClienteL;

            // Si me pidieron patente, tarjeta o tag uso getdatoscliente
            // sino getlistacliente que es mucho mas rapido

            if (patente != null || numeroTag != null || numeroTarjeta != null || numeroTarjetaExterno != null)
            {
                oClienteL = ClienteDT.getDatosClienteInt(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente);
            }
            else
            {
                oClienteL = ClienteDT.getListaCliente(conn, numeroCliente, tipoDocumento, documento, nombre, expediente, conConsumidorFinal);
            }
            // @TODO: VER QUE MODELO DEJAMOS, WEB ADAPTADO A OPEVIAL U OPEVIAL ADAPTADO A WEB

            //Conexion para obtener los locales
            ////    Conexion connLocal = conn;
            //////Locales. Si me piden patente tarjeta o tag no hay
            ////if (locales) 
            ////{
            ////    if (bGST)
            ////    {
            ////        //Los locales estan en consolidado
            ////        connLocal = new Conexion();
            ////        conn.ConectarConsolidado(false);
            ////    }
            ////    ClienteDT.getClientesLocales(connLocal, oClienteL, numeroCliente, tipoDocumento, documento, nombre);
            ////    if (bGST)
            ////    {
            ////        connLocal.Dispose();
            ////    }
            ////}

            return oClienteL;
             */
        }

        /// <summary>
        /// Obtiene los clientes recibe la conexion abierta y la cantidad de filas maxima
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="numeroCliente"></param>
        /// <param name="patente"></param>
        /// <param name="tipoDocumento"></param>
        /// <param name="documento"></param>
        /// <param name="nombre"></param>
        /// <param name="numeroTag"></param>
        /// <param name="numeroTarjeta"></param>
        /// <param name="numeroTarjetaExterno"></param>
        /// <param name="expediente"></param>
        /// <param name="locales"></param>
        /// <param name="xiCantRows"></param>
        /// <param name="llegoAlTope"></param>
        /// <param name="conConsumidorFinal"></param>
        /// <param name="numeroTagExterno"></param>
        /// <returns></returns>
        public static ClienteL getClientesLimitado(Conexion conn, int? numeroCliente,
                                     string patente, int? tipoDocumento,
                                     string documento, string nombre,
                                     string numeroTag, string numeroTarjeta,
                                     int? numeroTarjetaExterno, string expediente,
                                     bool locales, int? xiCantRows, out bool llegoAlTope, bool conConsumidorFinal, int? numeroTagExterno)
        {
            ClienteL oClienteL;

            if (patente != null || numeroTag != null || numeroTarjeta != null || numeroTarjetaExterno != null || numeroTagExterno != null)
            {
                oClienteL = ClienteDT.getDatosClienteInt(conn, numeroCliente, patente, tipoDocumento, documento, nombre, numeroTag, numeroTarjeta, numeroTarjetaExterno, expediente, numeroTagExterno);
                llegoAlTope = false;
            }
            else
            {
                oClienteL = ClienteDT.getListaCliente(conn, numeroCliente, tipoDocumento, documento, nombre, expediente, xiCantRows, out llegoAlTope, conConsumidorFinal);
            }
            return oClienteL;
        }

        /// ***********************************************************************************************
        /// <summary>
        ///     asdkfjasdkfñsdksdfj
        /// </summary>
        /// <returns>Lista de clientes agrupados por vehiculo (1 cliente - 1 vehiculo)</returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesAgrupadoPorVehiculos(string patente, string razonSocial)
        {
            bool EstoyGST = ConexionBs.getGSToEstacion();
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(EstoyGST, false);
                return ClienteDT.getClientesAgrupadoPorVehiculos(conn, patente, razonSocial);
            }  
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Cliente
        /// </summary>
        /// <param name="oCliente">Cliente -  Estructura del Cliente a Agregar
        /// <returns>Numero de Cliente agregado</returns>
        /// ***********************************************************************************************
        public static int addCliente(Cliente oCliente)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos repeticiones
                bool llegoAlTope = false;
                ClienteL oClieAux = ClienteDT.getListaCliente(conn, null, oCliente.TipoDocumento.Codigo, oCliente.NumeroDocumento, null, null, null, out llegoAlTope, false);
                if (oClieAux != null && oClieAux.Count > 0)
                {
                    throw new Errores.ErrorFacturacionStatus(Traduccion.Traducir("El tipo y número de documento ya existen"));
                }
                //Agregamos Cliente
                ClienteDT.addCliente(conn, oCliente);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oCliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCliente(),
                                                            "A",
                                                            getAuditoriaCodigoRegistro(oCliente),
                                                            getAuditoriaDescripcion(oCliente)),
                                                            connAud);
                    connAud.Finalizar(true);
                }
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                return oCliente.NumeroCliente;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Cliente
        /// </summary>
        /// <param name="oCliente">Cliente -  Estructura del Cliente a Modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCliente(Cliente oCliente)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos repeticiones
                bool llegoAlTope = false;
                ClienteL oClieAux = ClienteDT.getListaCliente(conn, null, oCliente.TipoDocumento.Codigo, oCliente.NumeroDocumento, null, null, null, out llegoAlTope, false);
                if (oClieAux != null && oClieAux.Count > 0)
                {
                    //Si es el mismo está OK
                    foreach (Cliente item in oClieAux)
                    {
                        if (item.NumeroCliente != oCliente.NumeroCliente)
                        {
                            throw new Errores.ErrorFacturacionStatus(Traduccion.Traducir("El tipo y número de documento ya existen"));
                        }
                    }
                }

                //Modificamos el Cliente
                ClienteDT.updCliente(conn, oCliente);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oCliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCliente(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oCliente),
                                                            getAuditoriaDescripcion(oCliente)),
                                                            connAud);

                    connAud.Finalizar(true);
                }
                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Cliente
        /// </summary>
        /// <param name="oCliente">Cliente - Estructura del Cliente a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCliente(Cliente oCliente, bool nocheck, string NewLine)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "CLIENT",
                                                    new string[] { oCliente.NumeroCliente.ToString() },
                                                    new string[] { "PREPAG" },
                                                    nocheck, NewLine);


                //Borramos las cuentas por estacion para poder modificar por la FK
                ClienteDT.RecalcularCuentas(conn, oCliente, true);

                //eliminamos el Cliente
                ClienteDT.delCliente(conn, oCliente);


                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCliente(),
                                        "B",
                                        getAuditoriaCodigoRegistro(oCliente),
                                        getAuditoriaDescripcion(oCliente)),
                                        connAud);

                    connAud.Finalizar(true);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        #endregion
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCliente()
        {
            return "CLI";
        }
        
        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Cliente oCliente)
        {
            return oCliente.NumeroCliente.ToString();
        }
        
        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Cliente oCliente)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre o Razón Social", oCliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Domicilio", oCliente.Domicilio);
            AuditoriaBs.AppendCampo(sb, "Localidad", oCliente.Localidad);
            AuditoriaBs.AppendCampo(sb, "Provincia", oCliente.Provincia.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Documento", oCliente.TipoDocumento.Descripcion + " " + oCliente.NumeroDocumento);
            AuditoriaBs.AppendCampo(sb, "Teléfono", oCliente.Telefono);
            AuditoriaBs.AppendCampo(sb, "E-Mail", oCliente.Email);
            AuditoriaBs.AppendCampo(sb, "Expediente", oCliente.Expediente);

            return sb.ToString();
        }

        #endregion

        #region CUENTAS: Metodos de la clase CUENTA

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de una Cuenta
        /// 
        /// </summary>
        /// <param name="oCuenta">Cuenta -  Estructura de la Cuenta a Agregar
        /// <returns>Numero de Cuenta agregada</returns>
        /// ***********************************************************************************************
        public static int addCuenta(Cuenta oCuenta)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Agregamos Cuenta
                ClienteDT.addCuenta(conn, oCuenta);

                //Agregamos vehiculos
                foreach (VehiculoHabilitado oVehiculo in oCuenta.VehiculosHabilitados)
                {
                    ClienteDT.updCuentaVehiculo(conn, oCuenta, oVehiculo);
                }

                //Agregamos estaciones
                foreach (CuentaEstacion oEstacion in oCuenta.EstacionesHabilitadas)
                {
                    ClienteDT.updCuentaEstacion(conn, oCuenta, oEstacion);
                }

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oCuenta.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCuenta(),
                                                            "A",
                                                            getAuditoriaCodigoRegistro(oCuenta),
                                                            getAuditoriaDescripcion(oCuenta)),
                                                            connAud);

                    connAud.Finalizar(true);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);

                return oCuenta.Numero;
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una Cuenta
        /// </summary>
        /// <param name="oCuenta">Cuenta -  Estructura de la Cuenta a Modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updCuenta(Cuenta oCuenta)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);


                //Borramos las cuentas por estacion para poder modificar por la FK
                ClienteDT.RecalcularCuentas(conn, oCuenta.Cliente, true);

                //Modificamos la Cuenta
                ClienteDT.updCuenta(conn, oCuenta);

                //Modificamos vehiculos
                foreach (VehiculoHabilitado oVehiculo in oCuenta.VehiculosHabilitados)
                {
                    ClienteDT.updCuentaVehiculo(conn, oCuenta, oVehiculo);
                }

                //Modificamos estaciones
                foreach (CuentaEstacion oEstacion in oCuenta.EstacionesHabilitadas)
                {
                    ClienteDT.updCuentaEstacion(conn, oCuenta, oEstacion);
                }
                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oCuenta.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCuenta(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oCuenta),
                                                            getAuditoriaDescripcion(oCuenta)),
                                                            connAud);

                    connAud.Finalizar(true);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de una Cuenta
        /// </summary>
        /// <param name="oCuenta">Cuenta - Estructura de la Cuenta a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delCuenta(Cuenta oCuenta, bool nocheck, string NewLine)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "CUENTA",
                                                    new string[] { oCuenta.Numero.ToString() },
                                                    new string[] { "CTAEST" },
                                                    nocheck, NewLine);


                //Borramos las cuentas por estacion para poder modificar por la FK
                ClienteDT.RecalcularCuentas(conn, oCuenta.Cliente, true);

                //eliminamos la Cuenta
                ClienteDT.delCuenta(conn, oCuenta);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oCuenta.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaCuenta(),
                                        "B",
                                        getAuditoriaCodigoRegistro(oCuenta),
                                        getAuditoriaDescripcion(oCuenta)),
                                        connAud);

                    connAud.Finalizar(true);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }
        #endregion
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaCuenta()
        {
            return "CTA";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Cuenta oCuenta)
        {
            return oCuenta.Numero.ToString();
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Cuenta oCuenta)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", oCuenta.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Tipo", oCuenta.TipoCuenta.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Agrupación", oCuenta.Agrupacion.DescrAgrupacion);
            AuditoriaBs.AppendCampo(sb, "Descripción", oCuenta.Descripcion);
            if (oCuenta.FechaEgreso != null)
            {
                AuditoriaBs.AppendCampo(sb, "Vencimiento", ((DateTime)oCuenta.FechaEgreso).ToShortDateString());
            }
            //Vehiculos Habil
            bool bHay = false;
            StringBuilder sb2 = new StringBuilder();
            foreach (VehiculoHabilitado item in oCuenta.VehiculosHabilitados)
            {
                if (item.Habilitado)
                {
                    sb2.Append(item.Vehiculo.Patente + " ");
                    bHay = true;
                }
            }
            if (bHay)
            {
                AuditoriaBs.AppendCampo(sb, "Vehículos", sb2.ToString());
            }

            //Estaciones Habil
            bHay = false;
            sb2 = new StringBuilder();
            foreach (CuentaEstacion item in oCuenta.EstacionesHabilitadas)
            {
                if (item.Habilitado)
                {
                    sb2.Append(item.Estacion.Numero.ToString()+" ");
                    bHay = true;
                }
            }
            if (bHay)
            {
                AuditoriaBs.AppendCampo(sb, "Estaciones", sb2.ToString());
            }

            return sb.ToString();
        }

        #endregion

        #region VEHICULO INFO: Metodos de la VehiculoInfo.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la información del cliente y del vehículo para la patente recibida
        /// /// </summary>
        /// <returns>VehiculoInfo</returns>
        /// ***********************************************************************************************
        public static VehiculoInfo getVehiculoInfo(string patente)
        {
            return getVehiculoInfo(ConexionBs.getGSToEstacion(), patente);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la información del cliente y del vehículo para la patente recibida
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="patente">String - Permite filtrar por una patente determinada
        /// <returns>Informacion del vehiculo</returns>
        /// ***********************************************************************************************
        public static VehiculoInfo getVehiculoInfo(bool bGST, string patente)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);

                return ClienteDT.getVehiculoInfo(conn, patente);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la información del cliente y del vehículo para la patente recibida - COMPLETA
        /// /// </summary>
        /// <returns>VehiculoInfoCompleta</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoCompleta getVehiculoInfoCompleta(string patente, int? codEstacion)
        {
            return getVehiculoInfoCompleta(ConexionBs.getGSToEstacion(), patente, codEstacion);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la información del cliente y del vehículo para la patente recibida - COMPLETA
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="patente">String - Permite filtrar por una patente determinada
        /// <param name="codEstacion">int? - Permite filtrar por una estación determinada
        /// <returns>Informacion del vehiculo completa</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoCompleta getVehiculoInfoCompleta(bool bGST, string patente, int? codEstacion)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(bGST, false);
                return ClienteDT.getVehiculoInfoCompleta(conn, patente, codEstacion);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la información del cliente y del vehículo para la patente recibida - COMPLETA
        /// </summary>
        /// <param name="bGST">bool - Indica si se debe conectar con Gestion o Estacion</param>
        /// <param name="patente">String - Permite filtrar por una patente determinada</param>
        /// <param name="sEmisor">string - Emisor del Tag</param>
        /// <param name="sTag">string - Tag del auto</param>
        /// <returns>Informacion del vehiculo completa</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoCompletaL getVehiculoPatenteTag(string sPatente, string sEmisor, string sTag)
        {

            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarPlaza(false);
                return ClienteDT.getVehiculoPatenteTag(conn, sPatente, sEmisor, sTag);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los últimos tránsitos para una patente
        /// </summary>
        /// <param name="sPatente">String - Permite filtrar por una patente determinada
        /// <returns>Devuelve los últimos tránsitos para una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoUltTransitosL getVehiculoUltTransitos(string sPatente,int? top)
        {
            bool local;
            return getVehiculoUltTransitos(sPatente, null, null,null,top,out local);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los últimos tránsitos para una patente
        /// </summary>
        /// <param name="sPatente">String - Permite filtrar por una patente determinada
        /// <param name="sEmisor">String - Permite filtrar por el emisor de un tag determinada
        /// <param name="sTag">String - Permite filtrar por un tag determinada
        /// <returns>Devuelve los últimos tránsitos para una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoUltTransitosL getVehiculoUltTransitos(string sPatente, string sEmisor, string sTag, string sCPF,int? top,out bool local)
        {
            using (Conexion conn = new Conexion())
            {
                string sLocal = conn.ConectarConsolidadoThenPlaza_str();

                local = (sLocal == "ESTACION");
                 
                return ClienteDT.getVehiculoUltTransitos(conn, sPatente, sEmisor, sTag, sCPF, top);
            }
        }
   

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los vehiculos con las estaciones habilitadas
        /// </summary>
        /// <param name="sPatente">String - Permite filtrar por una patente determinada
        /// <param name="sEmisor">String - Permite filtrar por el emisor
        /// <param name="sTag">String - Permite filtrar por un tag determinada
        /// <returns>Devuelve los vehiculos con las estaciones habilitadas</returns>
        /// ***********************************************************************************************
        public static VehiculoEstacionesHabilitadasL getEstacionesHabilitadasVehiculo(string sPatente, string sEmisor, string sTag)
        {
            using (Conexion conn = new Conexion())
            {
                conn.ConectarPlaza(false);
                return ClienteDT.getEstacionesHabilitadasVehiculo(conn, sPatente, sEmisor, sTag);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el detalle de deuda para una patente
        /// </summary>
        /// <param name="patente">String - Permite filtrar por una patente determinada
        /// <returns>Devuelve el detalle de deuda para una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoDeudaDetalleL getVehiculoDeudaDetalle(string patente,string cpf)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                //conn.ConectarGSTPlaza(bGST, false);

                conn.ConectarConsolidadoPlaza(true, false);

                return ClienteDT.getVehiculoDeudaDetalle(conn, patente,cpf);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el total de deuda para una patente
        /// </summary>
        /// <param name="patente">String - Permite filtrar por una patente determinada
        /// <returns>Devuelve el total de deuda para una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoDeudaTotalL getVehiculoDeudaTotal(string patente ,string cpf)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                //conn.ConectarGSTPlaza(bGST, false);
                //conn.ConectarConsolidadoPlaza(true, false);
                conn.ConectarConsolidadoThenPlaza();
                return ClienteDT.getVehiculoDeudaTotal(conn, patente,cpf);
            }
        }

        #endregion

        #region VEHICULO: Metodos de la clase Vehiculo
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los vehiculos (y datos asociados) que coindicen con la patente buscada
        /// Basicamente es para localizar un vehiculo con dato de cliente asociado, cuenta, agrupacion, etc
        /// </summary>
        /// <returns>Lista de Vehiculos</returns>
        /// 
        //
        /// ***********************************************************************************************
        public static VehiculoL getVehiculosPatenteEstacionCliente(string patente, string nombre,  int? nuext ,int xiCantRows, out bool llegoAlTope)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ClienteDT.getVehiculosPatenteEstacionCliente(conn, patente, nombre, ConexionBs.getNumeroEstacion(), nuext, xiCantRows, out llegoAlTope);
            }
        }
        
        public static VehiculoL getVehiculosPatenteEstacionCliente(string patente, string nombre, int? nuext)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ClienteDT.getVehiculosPatenteEstacionCliente(conn, patente, nombre, ConexionBs.getNumeroEstacion(), nuext);
            }
        }

        public static VehiculoL getVehiculosPatenteEstacion(string patente, int? nuext)
        {
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                conn.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);
                return ClienteDT.getVehiculosPatenteEstacion(conn, patente, ConexionBs.getNumeroEstacion(), nuext);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Alta de un Vehiculo
        /// </summary>
        /// <param name="oVehiculo">Vehiculo -  Estructura del Vehiculo a Agregar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void addVehiculo(Vehiculo oVehiculo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificamos repeticiones
                ClienteL oClieAux = ClienteDT.getDatosClienteInt(conn, null, oVehiculo.Patente, null, null, null, null, null, null, null, null);
                if (oClieAux != null && oClieAux.Count > 0)
                {
                    throw new Errores.ErrorFacturacionStatus(string.Format(Traduccion.Traducir("Un vehículo con esta patente existe para el cliente [0] [1]"),oClieAux[0].NumeroCliente, oClieAux[0].RazonSocial));
                }
                //Agregamos Vehiculo
                ClienteDT.addVehiculo(conn, oVehiculo);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oVehiculo.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVehiculo(),
                                                            "A",
                                                            getAuditoriaCodigoRegistro(oVehiculo),
                                                            getAuditoriaDescripcion(oVehiculo)),
                                                            connAud);

                    connAud.Finalizar(true);
                }

                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de un Vehiculo
        /// </summary>
        /// <param name="oVehiculo">Vehiculo -  Estructura del Vehiculo a Modificar
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void updVehiculo(Vehiculo oVehiculo)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Modificamos el Vehiculo
                ClienteDT.updVehiculo(conn, oVehiculo);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oVehiculo.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVehiculo(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oVehiculo),
                                                            getAuditoriaDescripcion(oVehiculo)),
                                                            connAud);

                    connAud.Finalizar(true);
                }


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Eliminacion de un Vehiculo
        /// </summary>
        /// <param name="oVehiculo">Vehiculo - Estructura del Vehiculo a eliminar
        /// <param name="nocheck">bool - Si ya se realizo el chequeo de FK</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void delVehiculo(Vehiculo oVehiculo, bool nocheck, string NewLine)
        {
            //iniciamos una transaccion
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                //Verificar que no haya registros con FK a este
                MantenimientoBS.checkReferenciasFK(conn, "CLIVEH",
                                                    new string[] { oVehiculo.Patente },
                                                    new string[] { "tags" },
                                                    nocheck, NewLine);


                //Borramos las cuentas por estacion para poder modificar por la FK
                ClienteDT.RecalcularCuentas(conn, oVehiculo.Cliente, true);

                //eliminamos el Vehiculo
                ClienteDT.delVehiculo(conn, oVehiculo);

                //Recalculamos las cuentas por estacion
                ClienteDT.RecalcularCuentas(conn, oVehiculo.Cliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVehiculo(),
                                        "B",
                                        getAuditoriaCodigoRegistro(oVehiculo),
                                        getAuditoriaDescripcion(oVehiculo)),
                                        connAud);

                    connAud.Finalizar(true);
                }


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modificacion de una patente, que implica cambiar la patente en todas las tablas (previa copia de registro de CLIVEH) y graba 
        /// un log con el cambio para replicar a la plaza y un script actualiza las tablas de abonos. Cambio de matricula = mismo auto, distinta patente
        /// </summary>
        /// <param name="nCoest">Codigo Estacion - Codigo de la estacion correspondiente</param>
        /// <param name="sUsrId">Usuario Id - Id del Usuario que ejecuta la modificacion</param>
        /// <param name="oCliente">Cliente - Objeto que representa al cliente del vehiculo</param>
        /// <param name="oVehiculo">Vehiculo - Estructura del Vehiculo a eliminar</param>
        /// <param name="sNuevaPatente">Patente - Nueva patente a guardar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setPatenteVehiculo(int? nCoest, string sUsrId, Cliente oCliente, Vehiculo oVehiculo, string sNuevaPatente)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                ClienteDT.RecalcularCuentas(conn, oCliente, true);

                //Modificamos la patente del vehiculo
                ClienteDT.setClienteVehiculo(conn, oCliente.NumeroCliente, oVehiculo.Patente, sNuevaPatente);
                ClienteDT.updTagVehiculo(conn, oCliente.NumeroCliente, oVehiculo.Patente, sNuevaPatente);
                ClienteDT.updChipVehiculo(conn, oCliente.NumeroCliente, oVehiculo.Patente, sNuevaPatente);
                ClienteDT.updPatente(conn,nCoest, oCliente.NumeroCliente, oVehiculo.Patente, sNuevaPatente);
                ClienteDT.addCambioPatente(conn, nCoest,sUsrId, oCliente.NumeroCliente, oVehiculo.Patente, sNuevaPatente);

                ClienteDT.RecalcularCuentas(conn, oCliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVehiculo(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oVehiculo),
                                                            getAuditoriaDescripcion(oVehiculo,sNuevaPatente,oCliente.RazonSocial)),
                                                            connAud);
                    connAud.Finalizar(true);
                }


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Cambio de Vehicuo de un cliente, que implica cambiar la patente en todas las tablas (previa copia de registro de CLIVEH)
        /// y graba un log con el cambio para replicar a la plaza y un script actualiza las tablas de abonos. 
        /// Cambio de vehiculo = otro auto totalmente distinto
        /// </summary>
        /// <param name="nCoest">Codigo Estacion - Codigo de la estacion correspondiente</param>
        /// <param name="sUsrId">Usuario Id - Id del Usuario que ejecuta la modificacion</param>
        /// <param name="oCliente">Cliente - Objeto que representa al cliente del vehiculo</param>
        /// <param name="oVehiculo">Vehiculo - Estructura del Vehiculo a eliminar</param>
        /// <param name="sNuevaPatente">Patente - Nueva patente a guardar</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void setVehiculo(int? intCoest, string strUsrId, Cliente oCliente, Vehiculo oVehiculo, Vehiculo oNewVehiculo)
        {
            using (Conexion conn = new Conexion())
            {
                //Siempre en Gestion, con transaccion
                conn.ConectarGST(true);

                ClienteDT.RecalcularCuentas(conn, oCliente, true);

                //Modificamos la patente del vehiculo
                ClienteDT.setNewVehiculo(conn, oCliente,oNewVehiculo);
                ClienteDT.updChipVehiculo(conn, oCliente.NumeroCliente, oVehiculo.Patente, oNewVehiculo.Patente);
                ClienteDT.updPatente(conn, intCoest, oCliente.NumeroCliente, oVehiculo.Patente, oNewVehiculo.Patente);
                ClienteDT.addAuditoriaCambioVehiculo(conn,intCoest,strUsrId,oCliente,oVehiculo,oNewVehiculo);

                ClienteDT.RecalcularCuentas(conn, oCliente, false);

                //Auditoria grabamos donde lo hacemos
                using (Conexion connAud = new Conexion())
                {
                    connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), true);
                    //Grabamos auditoria
                    AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaVehiculo(),
                                                            "M",
                                                            getAuditoriaCodigoRegistro(oVehiculo),
                                                            getAuditoriaDescripcion(oVehiculo, oNewVehiculo, oCliente.RazonSocial)),
                                                            connAud);
                    connAud.Finalizar(true);
                }


                //Grabo OK hacemos COMMIT
                conn.Finalizar(true);
            }
        }

        #endregion
        
        #region AUDITORIA: Metodos para realizar la auditoria que retornan la descripcion, codigo, etc.

        ///****************************************************************************************************<summary>
        /// Codigo de Auditoria correspondiente a la tabla de tipos de auditoria
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoAuditoriaVehiculo()
        {
            return "CLV";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(Vehiculo oVehiculo)
        {
            return oVehiculo.Patente;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(Vehiculo oVehiculo)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", oVehiculo.Cliente.RazonSocial);
            AuditoriaBs.AppendCampo(sb, "Marca", oVehiculo.Marca.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Modelo", oVehiculo.Modelo.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Color", oVehiculo.Color.Descripcion);
            AuditoriaBs.AppendCampo(sb, "Categoría", oVehiculo.Categoria.Descripcion);
            if (oVehiculo.FechaVencimiento != null)
            {
                AuditoriaBs.AppendCampo(sb, "Vencimiento", ((DateTime)oVehiculo.FechaVencimiento).ToShortDateString());
            }

            return sb.ToString();
        }

        private static string getAuditoriaDescripcion(Vehiculo oVehiculo, string sPatente, string razonSocial)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", razonSocial);
            AuditoriaBs.AppendCampo(sb, "Marca", oVehiculo.Marca.Descripcion);
            if (oVehiculo.Modelo != null)
            {
                AuditoriaBs.AppendCampo(sb, "Modelo", oVehiculo.Modelo.Descripcion);
            }
            AuditoriaBs.AppendCampo(sb, "Color", oVehiculo.Color.Descripcion);
            if (oVehiculo.Categoria != null)
            {
                AuditoriaBs.AppendCampo(sb, "Categoría", oVehiculo.Categoria.Descripcion);
            }
            if (oVehiculo.FechaVencimiento != null)
            {
                AuditoriaBs.AppendCampo(sb, "Vencimiento", ((DateTime)oVehiculo.FechaVencimiento).ToShortDateString());
            }

            AuditoriaBs.AppendCampo(sb, "PatenteAnterior", oVehiculo.Patente);
            AuditoriaBs.AppendCampo(sb, "PatenteNueva", sPatente);

            return sb.ToString();
        }

        private static string getAuditoriaDescripcion(Vehiculo oVehiculo,Vehiculo oNewVehiculo,string razonSocial)
        {
            StringBuilder sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Nombre", razonSocial);
            AuditoriaBs.AppendCampo(sb, "MarcaAnterior", oVehiculo.Marca.Descripcion);
            if (oVehiculo.Modelo != null)
            {
                AuditoriaBs.AppendCampo(sb, "ModeloAnterior", oVehiculo.Modelo.Descripcion);
            }
            AuditoriaBs.AppendCampo(sb, "ColorAnterior", oVehiculo.Color.Descripcion);
            if (oVehiculo.Categoria != null)
            {
                AuditoriaBs.AppendCampo(sb, "CategoríaAnterior", oVehiculo.Categoria.Descripcion);
            }
            if (oVehiculo.FechaVencimiento != null)
            {
                AuditoriaBs.AppendCampo(sb, "VencimientoAnterior", ((DateTime)oVehiculo.FechaVencimiento).ToShortDateString());
            }
            AuditoriaBs.AppendCampo(sb, "PatenteAnterior", oVehiculo.Patente);

            AuditoriaBs.AppendCampo(sb, "MarcaNueva", oNewVehiculo.Marca.Descripcion);
            if (oNewVehiculo.Modelo != null)
            {
                AuditoriaBs.AppendCampo(sb, "ModeloNueva", oNewVehiculo.Modelo.Descripcion);
            }
            AuditoriaBs.AppendCampo(sb, "ColorNueva", oNewVehiculo.Color.Descripcion);
            if (oNewVehiculo.Categoria != null)
            {
                AuditoriaBs.AppendCampo(sb, "CategoríaNueva", oNewVehiculo.Categoria.Descripcion);
            }
            if (oNewVehiculo.FechaVencimiento != null)
            {
                AuditoriaBs.AppendCampo(sb, "VencimientoNueva", ((DateTime)oNewVehiculo.FechaVencimiento).ToShortDateString());
            }
            AuditoriaBs.AppendCampo(sb, "PatenteNueva", oNewVehiculo.Patente);
            AuditoriaBs.AppendCampo(sb, "NuevaNueva", oNewVehiculo.Patente);

            return sb.ToString();
        }

        #endregion

        #region TIPODOCUMENTO: Metodos de la Clase Tipos de Documento.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de Documento definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Tipos de Documento</returns>
        /// ***********************************************************************************************
        public static TipoDocumentoL getTiposDocumento()
        {
            bool PudoGST;
            return getTiposDocumento(ConexionBs.getGSToEstacion(), null, out PudoGST);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de Documento definidos, sin filtro
        /// </summary>
        /// <param name="codigoTipoDoc">int - Codigo de Tipo de Documento</param>
        /// <returns>Lista de Tipos de Documento</returns>
        /// ***********************************************************************************************
        public static TipoDocumentoL getTiposDocumento(int? codigoTipoDoc)
        {
            bool PudoGST;
            return getTiposDocumento(ConexionBs.getGSToEstacion(), codigoTipoDoc, out PudoGST);
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de Documento definidos, sin filtro
        /// </summary>
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="codigoTipoDoc">int - Codigo de Tipo de Documento</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// <returns>Lista de Tipos de Documento</returns>
        /// ***********************************************************************************************
        public static TipoDocumentoL getTiposDocumento(bool bGST, int? codigoTipoDoc, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return ClienteDT.getTiposDocumento(conn, codigoTipoDoc);
            }
        }

        #endregion

        #region TIPOIVA: Metodos de la Clase Tipos de IVA.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de IVA definidos, sin filtro
        /// </summary>
        /// <returns>Lista de Tipos de IVA</returns>
        /// ***********************************************************************************************
        public static TipoIVAL getTiposIVA()
        {
            bool PudoGST;
            return getTiposIVA(ConexionBs.getGSToEstacion(), null, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de IVA definidos, sin filtro
        /// </summary>
        /// <param name="codigo">int - Codigo de Tipo de IVA</param>
        /// <returns>Lista de Tipos de IVA</returns>
        /// ***********************************************************************************************
        public static TipoIVAL getTiposIVA(int? codigo)
        {
            bool PudoGST;
            return getTiposIVA(ConexionBs.getGSToEstacion(), codigo, out PudoGST);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Tipos de IVA definidos, sin filtro
        /// </summary>
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="codigo">int - Codigo de Tipo de IVA</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// <returns>Lista de Tipos de IVA</returns>
        /// ***********************************************************************************************
        public static TipoIVAL getTiposIVA(bool bGST, int? codigo, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }

                return ClienteDT.getTiposIVA(conn, codigo);
            }
        }

        #endregion

        #region PROVINCIAS: Metodos de Provincias.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de todos los Provincias
        /// </summary>
        /// <param name="bGST">bool - Si se desea que intente traer de GST</param>
        /// <param name="PudoGST">out bool - true si pudo traer de GST</param>
        /// <returns>Lista de Tipos de Documento</returns>
        /// ***********************************************************************************************
        public static ProvinciaL getProvincias(bool bGST, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }
                return ClienteDT.getProvincias(conn);
            }
        }


        /// <summary>
        /// Obtiene los estados y capitales de brasil
        /// </summary>
        /// <param name="bGST"></param>
        /// <param name="PudoGST"></param>
        /// <returns></returns>
        public static EstadoL getEstados(bool bGST, out bool PudoGST)
        {
            PudoGST = false;
            using (Conexion conn = new Conexion())
            {
                //sin transaccion
                if (bGST)
                {
                    PudoGST = conn.ConectarGSTThenPlaza();
                }
                else
                {
                    conn.ConectarPlaza(false);
                }
                return ClienteDT.getEstados(conn);
            }
        }

        

        #endregion
    }
}
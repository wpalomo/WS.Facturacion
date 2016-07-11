using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using Telectronica.Peaje;
using Telectronica.Facturacion;

namespace Telectronica.Facturacion
{
    public class ClienteDT
    {
        #region MARCA: Clase de Datos de Marca.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Marcas definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMarca">int - Codigo de Marca a filtrar</param>
        /// <returns>Lista de Marcas</returns>
        /// ***********************************************************************************************
        public static VehiculoMarcaL getMarcas(Conexion oConn, int? codigoMarca)
        {
            VehiculoMarcaL oMarcas = new VehiculoMarcaL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Marcas_getMarcas";
            oCmd.Parameters.Add("@mar_codig", SqlDbType.TinyInt).Value = codigoMarca;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oMarcas.Add(CargarMarcas(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oMarcas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Marcas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Marcas</param>
        /// <returns>Lista con el elemento Marcas de la base de datos</returns>
        /// ***********************************************************************************************
        private static VehiculoMarca CargarMarcas(System.Data.IDataReader oDR)
        {
            VehiculoMarca oMarca = new VehiculoMarca((int)oDR["mar_codig"], oDR["mar_descr"].ToString(), oDR["mar_delet"].ToString());
            return oMarca;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Marca en la base de datos
        /// </summary>
        /// <param name="oMarca">Marca - Objeto con la informacion de la Marca a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addMarcas(VehiculoMarca oMarca, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Marcas_addMarcas";

            oCmd.Parameters.Add("@mar_descr", SqlDbType.VarChar, 30).Value = oMarca.Descripcion;
                
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
                    msg = Traduccion.Traducir("Este codigo de Marca ya existe");
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                    msg = Traduccion.Traducir("Este codigo de Marca fue eliminado");
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Marca en la base de datos
        /// </summary>
        /// <param name="oMarca">Marca - Objeto con la informacion de la Marca a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updMarcas(VehiculoMarca oMarca, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Marcas_updMarcas";

            oCmd.Parameters.Add("@mar_codig", SqlDbType.Int).Value = oMarca.Codigo;
            oCmd.Parameters.Add("@mar_descr", SqlDbType.VarChar, 30).Value = oMarca.Descripcion;

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
                    msg = Traduccion.Traducir("No existe el registro de la Marca");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Marca de la base de datos
        /// </summary>
        /// <param name="Marcas">Int - Marca a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delMarca(int Marca, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Marcas_delMarcas";

            oCmd.Parameters.Add("@mar_codig", SqlDbType.SmallInt).Value = Marca;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("No existe el registro de la Marca");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static void delMarca(VehiculoMarca oMarca, Conexion oConn)
        {
            delMarca(oMarca.Codigo, oConn);
        }

        #endregion

        #region MODELO: Clase de Datos de Modelo.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los  Modelo definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoMarca">int - Codigo de Marca a filtrar</param>
        /// <param name="codigoModelo">int - Codigo de Modelo a filtrar</param>
        /// <returns>Lista de Modelos</returns>
        /// ***********************************************************************************************
        public static VehiculoModeloL getModelos(Conexion oConn, int? codigoMarca, int? CodigoModelo)
        {
            VehiculoModeloL oModelos = new VehiculoModeloL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Modelos_getModelos";

            oCmd.Parameters.Add("@mod_codig", SqlDbType.Int).Value = CodigoModelo;
            oCmd.Parameters.Add("@mod_marca", SqlDbType.Int).Value = codigoMarca;
               
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oModelos.Add(CargarModelos(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oModelos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Modelos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Modelos</param>
        /// <returns>Lista con el elemento Modelos de la base de datos</returns>
        /// ***********************************************************************************************
        private static VehiculoModelo CargarModelos(System.Data.IDataReader oDR)
        {
            VehiculoModelo oModelo = new VehiculoModelo(new VehiculoMarca((int)oDR["mar_codig"],
                                                                          oDR["mar_descr"].ToString(),
                                                                          oDR["mar_delet"].ToString()),
                                                                          (int)oDR["mod_codig"],
                                                                          oDR["mod_descr"].ToString(),
                                                                          oDR["mod_delet"].ToString());
            return oModelo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Modelo en la base de datos
        /// </summary>
        /// <param name="oModelo">Modelo - Objeto con la informacion del Modelo a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addModelos(VehiculoModelo oModelo, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Modelos_addModelos";

            oCmd.Parameters.Add("@mod_marca", SqlDbType.Int).Value = oModelo.Marca.Codigo;
            oCmd.Parameters.Add("@mod_descr", SqlDbType.VarChar, 30).Value = oModelo.Descripcion;

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
                    msg = Traduccion.Traducir("Este codigo de Modelo ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este codigo de Modelo fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Modelo la base de datos
        /// </summary>
        /// <param name="oModelo">Modelo - Objeto con la informacion del Modelo a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updModelos(VehiculoModelo oModelo, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Modelos_updModelos";

            oCmd.Parameters.Add("@mod_codig", SqlDbType.Int).Value = oModelo.Codigo;
            oCmd.Parameters.Add("@mod_descr", SqlDbType.VarChar, 30).Value = oModelo.Descripcion;
            oCmd.Parameters.Add("@mod_marca", SqlDbType.VarChar, 30).Value = oModelo.Marca.Codigo;

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
                    msg = Traduccion.Traducir("No existe el registro del Modelo");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina todos los Modelo de una marca determinada de la base de datos
        /// </summary>
        /// <param name="Modelos">Int - Modelo a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delModelos(int Modelo, int Marca, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Modelos_delModelos";

            oCmd.Parameters.Add("@mod_codig", SqlDbType.Int).Value = Modelo;
            oCmd.Parameters.Add("@mod_marca", SqlDbType.Int).Value = Marca;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("No existe el registro del Modelo");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static void delModelo(VehiculoModelo oModelo, Conexion oConn)
        {
            delModelos(oModelo.Codigo, oModelo.Marca.Codigo, oConn);
        }

        #endregion

        #region COLOR: Clase de Datos de Color.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Colores definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoColor">int - Codigo de Color a filtrar</param>
        /// <returns>Lista de Colores</returns>
        /// ***********************************************************************************************
        public static VehiculoColorL getColores(Conexion oConn, int? codigoColor)
        {
            VehiculoColorL oColores = new VehiculoColorL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Colores_getColores";
            oCmd.Parameters.Add("@col_codig", SqlDbType.Int).Value = codigoColor;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oColores.Add(CargarColores(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oColores;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Colores
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Colores</param>
        /// <returns>Lista con el elemento Colores de la base de datos</returns>
        /// ***********************************************************************************************
        private static VehiculoColor CargarColores(System.Data.IDataReader oDR)
        {
            VehiculoColor oColor = new VehiculoColor((int)oDR["col_codig"],
                                                      oDR["col_descr"].ToString(),
                                                      oDR["col_delet"].ToString());
            return oColor;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Color en la base de datos
        /// </summary>
        /// <param name="oColor">Color - Objeto con la informacion del Color a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addColor(VehiculoColor oColor, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Colores_addColor";

            oCmd.Parameters.Add("@col_descr", SqlDbType.VarChar, 30).Value = oColor.Descripcion;

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
                    msg = Traduccion.Traducir("Este codigo de Color ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este codigo de Color fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Color en la base de datos
        /// </summary>
        /// <param name="oColor">Color - Objeto con la informacion del Color a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updColores(VehiculoColor oColor, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Colores_updColor";

            oCmd.Parameters.Add("@col_codig", SqlDbType.Int).Value = oColor.Codigo;
            oCmd.Parameters.Add("@col_descr", SqlDbType.VarChar, 30).Value = oColor.Descripcion;

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
                    msg = Traduccion.Traducir("No existe el registro del Color");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Color de la base de datos
        /// </summary>
        /// <param name="Colores">Int - Color a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delColor(int Color, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Colores_delColor";

            oCmd.Parameters.Add("@col_codig", SqlDbType.SmallInt).Value = Color;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("No existe el registro del Color");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static void delColor(VehiculoColor oColor, Conexion oConn)
        {
            delColor(oColor.Codigo, oConn);
        }

        #endregion

        #region AGRUPACIONCUENTA: Clase de Datos de Color.
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Agrupacion de Cuentas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="TipoCuenta">int - Codigo de Agrupacion de Cuentas a filtrar</param>
        /// <param name="FormaPago">int - Codigo de Agrupacion de Cuentas a filtrar</param>
        /// <returns>Lista de Agrupacion de Cuentas</returns>
        /// ***********************************************************************************************
        public static AgrupacionCuentaL getAgrupacionesCuentas(Conexion oConn, int? TipoCuenta, int? FormaPago)
        {
            AgrupacionCuentaL oGrupacionCuenta = new AgrupacionCuentaL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_AgrupacionCuentas_getAgrupacionCuentas";

            oCmd.Parameters.Add("@ctg_tipcu", SqlDbType.Int).Value = TipoCuenta;
            oCmd.Parameters.Add("@ctg_subfp", SqlDbType.Int).Value = FormaPago;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oGrupacionCuenta.Add(CargaroGrupacionCuenta(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oGrupacionCuenta;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Agrupación de Cuentas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de CTAGRU</param>
        /// <returns>Lista con el elemento Agrupacion de Cuenta de la base de datos</returns>
        /// ***********************************************************************************************
        private static AgrupacionCuenta CargaroGrupacionCuenta(System.Data.IDataReader oDR)
        {
            AgrupacionCuenta oAgrupacionCuenta = new AgrupacionCuenta(new TipoCuenta((int)oDR["ctg_tipcu"], oDR["tic_descr"].ToString()),
                                                                      (byte)oDR["ctg_subfp"],
                                                                      oDR["ctg_descr"].ToString(),
                                                                      new TarifaDiferenciada(Util.DbValueToNullable<byte>(oDR["ctg_titar"]), oDR["tit_descr"].ToString(), 0),
                                                                      new TarifaDiferenciada(Util.DbValueToNullable<byte>(oDR["ctg_titav"]), oDR["tit_descrv"].ToString(), 0),
                                                                      (Util.DbValueToNullable<int>(oDR["ctg_ducta"])),
                                                                      (Util.DbValueToNullable<int>(oDR["ctg_dipap"])),
                                                                      (Util.DbValueToNullable<int>(oDR["ctg_vimax"])),
                                                                      new TarifaDiferenciada(Util.DbValueToNullable<int>(oDR["ctg_titar2"]), oDR["tit_descr2"].ToString(), 0),
                                                                      Convert.ToChar((oDR["ctg_ctrcat"] is DBNull) ? 'N' : oDR["ctg_ctrcat"]),
                                                                      Convert.ToChar((oDR["ctg_ctrpat"] is DBNull) ? 'N' : oDR["ctg_ctrpat"]));
            return oAgrupacionCuenta;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Agrupacion de Cuenta en la base de datos
        /// </summary>
        /// <param name="oAgrupacionCuenta">AgrupacionCuenta - Objeto con la informacion del mensaje a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            int? v_TipoTarifaVta;
            int? v_TipoTarifa2;

            if (oAgrupacionCuenta.TipoTarifaVenta == null)
            {
                v_TipoTarifaVta = null;
            }
            else
            {
                v_TipoTarifaVta = oAgrupacionCuenta.TipoTarifaVenta.CodigoTarifa;
            }

            if (oAgrupacionCuenta.TipoTarifa2 == null)
            {
                v_TipoTarifa2 = null;
            }
            else
            {
                v_TipoTarifa2 = oAgrupacionCuenta.TipoTarifa2.CodigoTarifa;
            }

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_AgrupacionCuentas_addAgrupacionCuenta";

            oCmd.Parameters.Add("@ctg_tipcu", SqlDbType.Int).Value = oAgrupacionCuenta.TipoCuenta.CodigoTipoCuenta;
            oCmd.Parameters.Add("@ctg_descr", SqlDbType.VarChar, 50).Value = oAgrupacionCuenta.DescrAgrupacion;
            oCmd.Parameters.Add("@ctg_titar", SqlDbType.TinyInt).Value = oAgrupacionCuenta.TipoTarifa.CodigoTarifa;
            oCmd.Parameters.Add("@ctg_titav", SqlDbType.TinyInt).Value = v_TipoTarifaVta;
            oCmd.Parameters.Add("@ctg_ducta", SqlDbType.Int).Value = oAgrupacionCuenta.DiasDuracionCuenta;
            oCmd.Parameters.Add("@ctg_dipap", SqlDbType.Int).Value = oAgrupacionCuenta.DiasAntesVencimiento;
            oCmd.Parameters.Add("@ctg_vimax", SqlDbType.Int).Value = oAgrupacionCuenta.CantidadViajesMaximos;
            oCmd.Parameters.Add("@ctg_titar2", SqlDbType.Int).Value = v_TipoTarifa2;
            oCmd.Parameters.Add("@ctg_ctrcat", SqlDbType.Char, 1).Value = oAgrupacionCuenta.ControlaCategoria;
            oCmd.Parameters.Add("@ctg_ctrpat", SqlDbType.Char, 1).Value = oAgrupacionCuenta.ControlaPatente;

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
                    msg = Traduccion.Traducir("Este codigo de Agrupación de Cuentas ya existe");
                }
                else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                {
                    msg = Traduccion.Traducir("Este codigo de Agrupación de Cuentas fue eliminado");
                }
                throw new ErrorSPException(msg);
            }
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Agrupacion de Cuenta en la base de datos
        /// </summary>
        /// <param name="oAgrupacionCuenta">AgrupacionCuenta - Objeto con la informacion del mensaje a modificar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_AgrupacionCuentas_updAgrupacionCuenta";

            int? v_TipoTarifaVta;
            int? v_TipoTarifa2;

            if (oAgrupacionCuenta.TipoTarifaVenta == null)
            {
                v_TipoTarifaVta = null;
            }
            else
            {
                v_TipoTarifaVta = oAgrupacionCuenta.TipoTarifaVenta.CodigoTarifa;
            }
                
            if (oAgrupacionCuenta.TipoTarifa2 == null)
            {
                v_TipoTarifa2 = null;
            }
            else
            {
                v_TipoTarifa2 = oAgrupacionCuenta.TipoTarifa2.CodigoTarifa;
            }

            oCmd.Parameters.Add("@ctg_tipcu", SqlDbType.Int).Value = oAgrupacionCuenta.TipoCuenta.CodigoTipoCuenta;
            oCmd.Parameters.Add("@ctg_subfp", SqlDbType.TinyInt).Value = oAgrupacionCuenta.SubTipoCuenta;
            oCmd.Parameters.Add("@ctg_descr", SqlDbType.VarChar, 50).Value = oAgrupacionCuenta.DescrAgrupacion;
            oCmd.Parameters.Add("@ctg_titar", SqlDbType.TinyInt).Value = oAgrupacionCuenta.TipoTarifa.CodigoTarifa;
            oCmd.Parameters.Add("@ctg_titav", SqlDbType.TinyInt).Value = v_TipoTarifaVta;
            oCmd.Parameters.Add("@ctg_ducta", SqlDbType.Int).Value = oAgrupacionCuenta.DiasDuracionCuenta;
            oCmd.Parameters.Add("@ctg_dipap", SqlDbType.Int).Value = oAgrupacionCuenta.DiasAntesVencimiento;
            oCmd.Parameters.Add("@ctg_vimax", SqlDbType.Int).Value = oAgrupacionCuenta.CantidadViajesMaximos;
            oCmd.Parameters.Add("@ctg_titar2", SqlDbType.Int).Value = v_TipoTarifa2;
            oCmd.Parameters.Add("@ctg_ctrcat", SqlDbType.Char, 1).Value = oAgrupacionCuenta.ControlaCategoria;
            oCmd.Parameters.Add("@ctg_ctrpat", SqlDbType.Char, 1).Value = oAgrupacionCuenta.ControlaPatente;

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
                    msg = Traduccion.Traducir("No existe el registro de la Agrupación de Cuenta");
                }
                throw new ErrorSPException(msg);
            }
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Agrupacion de Cuenta de la base de datos
        /// </summary>
        /// <param name="AgrupacionCuenta">Int - AgrupacionCuenta a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delAgrupacionCuenta(int CodigoTipoCuenta, int CodigoTipoTarifas, Conexion oConn)
        {
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_AgrupacionCuentas_delAgrupacionCuenta";

            oCmd.Parameters.Add("@ctg_tipcu", SqlDbType.Int).Value = CodigoTipoCuenta;
            oCmd.Parameters.Add("@ctg_subfp", SqlDbType.TinyInt).Value = CodigoTipoTarifas;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al Eliminar el registro ") + retval.ToString();
                if (retval == -102)
                {
                    msg = Traduccion.Traducir("No existe el registro de Agrupación de Cuentas");
                }
                throw new ErrorSPException(msg);
            }
        }

        public static void delAgrupacionCuenta(AgrupacionCuenta oAgrupacionCuenta, Conexion oConn)
        {
            delAgrupacionCuenta(oAgrupacionCuenta.TipoCuenta.CodigoTipoCuenta, oAgrupacionCuenta.SubTipoCuenta, oConn);
        }

        #endregion

        #region TIPOCUENTA: Clase de Datos de Tipo de Cuenta

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Tipos de Cuentas
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Tipo de Cuentas</returns>
        /// ***********************************************************************************************
        public static TipoCuentaL getTipoCuentas(Conexion oConn, bool SoloConPrepago)
        {
            TipoCuentaL oTipoCuentaL = new TipoCuentaL();
            TipoCuenta oTipoCuenta = new TipoCuenta();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_TipoCuentas_getTipoCuentas";
                
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTipoCuenta = CargarTipoCuenta(oDR);

                if (oTipoCuenta.EsPrepago == true || SoloConPrepago == false )
                {
                    oTipoCuentaL.Add(oTipoCuenta);
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();

            return oTipoCuentaL;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tipo de Cuentas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla TIPCUE</param>
        /// <returns>Lista de Tipo de Cuentas</returns>
        /// ***********************************************************************************************
        private static TipoCuenta CargarTipoCuenta(System.Data.IDataReader oDR)
        {
            TipoCuenta oTipoCuenta = new TipoCuenta((int)oDR["tic_codig"], oDR["tic_descr"].ToString());
            return oTipoCuenta;
        }

        #endregion

        #region CLIENTE: Clase de Datos de Clientes
                
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes. Es un metodo interno que se utiliza cuando necesitamos todos 
        /// los datos del cliente (luego de filtrar la lista inicial que se muestra en la barra de navegacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="patente">string - Patente de algun vehiculo del cliente</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="numeroTag">string - Numero de tag que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjeta">string - Numero de tarjeta chip que posee un vehiculo del cliente</param>
        /// <param name="numeroTarjetaExterno">int - Numero externo de una tarjeta chip del cliente</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <returns>Lista de Clientes que coinciden con los filtros. </returns>
        /// ***********************************************************************************************
        public static ClienteL getDatosClienteInt(Conexion oConn, int? numeroCliente, 
                                                  string patente, int?tipoDocumento, 
                                                  string documento, string nombre, 
                                                  string numeroTag, string numeroTarjeta, 
                                                  int? numeroTarjetaExterno, string expediente,
                                                  int? numeroTagExterno)
        {
            ClienteL oClientes = new ClienteL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                
            //TODO: Este es el SP Puro de WEB, pero por ahora usamos el de OPEVIAL
            //oCmd.CommandText = "Facturacion.usp_Clientes_getClientesDetalle";

            oCmd.CommandText = "Facturacion.usp_Clientes_getDatosCliente";                
            oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;
            oCmd.Parameters.Add("@Paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@Tidoc", SqlDbType.Int).Value = tipoDocumento;
            oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = documento;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = nombre;
            oCmd.Parameters.Add("@Tag", SqlDbType.VarChar, 12).Value = numeroTag;
            oCmd.Parameters.Add("@Tarjeta", SqlDbType.VarChar, 12).Value = numeroTarjeta;
            oCmd.Parameters.Add("@TarjetaExt", SqlDbType.Int).Value = numeroTarjetaExterno;
            oCmd.Parameters.Add("@Exped", SqlDbType.Char, 10).Value = expediente;
            oCmd.Parameters.Add("@TagExt", SqlDbType.Int).Value = numeroTagExterno;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oClientes.Add(CargarCliente(oDR, false));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oClientes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes. Es un metodo que devuelve la lista de clientes pero solo con 
        /// el numero de cliente para desplazarnos por la barra de navegacion.Luego se busca el detalle de cada uno
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <returns>Lista de Clientes que coinciden con los filtros. </returns>
        /// ***********************************************************************************************
        public static ClienteL getListaCliente(Conexion oConn, int? numeroCliente,
                                               int? tipoDocumento, string documento,
                                               string nombre, string expediente, int? xiCantRows, out bool llegoAlTope, bool conConsumidorFinal)
        {
            ClienteL oClientes = new ClienteL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            //TODO: Este es el SP Puro de WEB, pero por ahora usamos el de OPEVIAL
            oCmd.CommandText = "Facturacion.usp_Clientes_getClientesLista";

            //oCmd.CommandText = "usp_Clientes_getListaCliente";                
            oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;
            oCmd.Parameters.Add("@Tidoc", SqlDbType.Int).Value = tipoDocumento;
            oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = documento;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = nombre;
            oCmd.Parameters.Add("@Exped", SqlDbType.Char, 10).Value = expediente;
            oCmd.Parameters.Add("@CantRows", SqlDbType.Int).Value = xiCantRows;
            oCmd.Parameters.Add("@ConConsFinal", SqlDbType.Char, 1).Value = conConsumidorFinal ? "S" : "N";

            //oCmd.Parameters.Add("@Locales", SqlDbType.Char, 1).Value = locales;

            oDR = oCmd.ExecuteReader();
            int i = 0;
            llegoAlTope = false;
            while (oDR.Read())
            {
                i++;
                oClientes.Add(CargarCliente(oDR, false));
                if (i == xiCantRows)
                {
                    llegoAlTope = true;
                    break;
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oClientes;
        }
        
        /*
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes. Es un metodo que devuelve la lista de clientes pero solo con 
        /// el numero de cliente para desplazarnos por la barra de navegacion.Luego se busca el detalle de cada uno
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <param name="expediente">string - Expediente con el que se registra el pedido de cliente</param>
        /// <returns>Lista de Clientes que coinciden con los filtros. </returns>
        /// ***********************************************************************************************
        public static ClienteL getListaCliente(Conexion oConn, int? numeroCliente,
                                               int? tipoDocumento, string documento,
                                               string nombre, string expediente, bool conConsumidorFinal)
        {
            ClienteL oClientes = new ClienteL();
            try
            {
                SqlDataReader oDR;


                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;

                //TODO: Este es el SP Puro de WEB, pero por ahora usamos el de OPEVIAL
                oCmd.CommandText = "Facturacion.usp_Clientes_getClientesLista";

                //oCmd.CommandText = "usp_Clientes_getListaCliente";                
                oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;
                oCmd.Parameters.Add("@Tidoc", SqlDbType.Int).Value = tipoDocumento;
                oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = documento;
                oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = nombre;
                oCmd.Parameters.Add("@Exped", SqlDbType.Char, 10).Value = expediente;
                oCmd.Parameters.Add("@ConConsFinal", SqlDbType.Char, 1).Value = conConsumidorFinal ? "S" : "N";
                //oCmd.Parameters.Add("@Locales", SqlDbType.Char, 1).Value = locales;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oClientes.Add(CargarCliente(oDR, false));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();


            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oClientes;
        }
        */
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los Clientes agrupados por vehiculo (1 cliente, 1 vehiulo)
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente a filtrar</param>
        /// <param name="razonSocial">string - Nombre del cliente a filtrar</param>
        /// <returns>Lista de Clientes que coinciden con los filtros. </returns>
        /// ***********************************************************************************************
        public static ClienteL getClientesAgrupadoPorVehiculos(Conexion oConn, string patente, string razonSocial)
        {
            ClienteL oClientes = new ClienteL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;

            //TODO: Este es el SP Puro de WEB, pero por ahora usamos el de OPEVIAL
            oCmd.CommandText = "Facturacion.usp_Clientes_getClientesAgrupadoPorVehiculos";

            //oCmd.CommandText = "usp_Clientes_getListaCliente";                
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = razonSocial;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oClientes.Add(CargarClientePorVehiculo(oDR, false));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oClientes;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega a la lista de clientes la lista de los Clientes Locales. Es un metodo que devuelve la lista de clientes pero solo con 
        /// el numero de cliente para desplazarnos por la barra de navegacion.Luego se busca el detalle de cada uno
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oCXlientes">ClienteL - Lista con los clientes globales ya encontrados</param>
        /// <param name="numeroCliente">int - Numero de cliente que se desea buscar</param>
        /// <param name="tipoDocumento">int - Tipo de documento del cliente</param>
        /// <param name="documento">string - Numero de documento del cliente</param>
        /// <param name="nombre">string - Parte del nombre del cliente por el cual buscar</param>
        /// <returns> </returns>
        /// ***********************************************************************************************
        public static void getClientesLocales(Conexion oConn, ClienteL oClientes, int? numeroCliente, int? tipoDocumento, string documento, string nombre)
        {
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "usp_Clientes_getClientesLocales";
            oCmd.Parameters.Add("@Numcl", SqlDbType.Int).Value = numeroCliente;
            oCmd.Parameters.Add("@Tidoc", SqlDbType.Int).Value = tipoDocumento;
            oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = documento;
            oCmd.Parameters.Add("@Nombr", SqlDbType.VarChar, 50).Value = nombre;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oClientes.Add(CargarClientePorVehiculo(oDR, true));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Clientes
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla CLIENT</param>
        /// <returns>Lista de Clientes</returns>
        /// ***********************************************************************************************
        private static Cliente CargarCliente(System.Data.IDataReader oDR, bool bLocal)
        {
            Cliente oCliente = new Cliente();
            
            oCliente.NumeroCliente = (int)oDR["cli_numcl"];
            oCliente.RazonSocial = oDR["cli_nombr"].ToString();
            oCliente.NumeroDocumento = oDR["cli_docum"].ToString();
            oCliente.Domicilio = oDR["cli_domic"].ToString();
            oCliente.Localidad = oDR["cli_local"].ToString();
            oCliente.Telefono = oDR["cli_telef"].ToString();
            oCliente.ClienteLocal = (oDR["OrigenCuenta"].ToString() == "G" ? "N" : "S");
            if (!bLocal)
            {
                oCliente.Comentario = oDR["cli_comen"].ToString();
                oCliente.Expediente = oDR["cli_exped"].ToString();
                oCliente.Email = oDR["cli_email"].ToString();
                oCliente.Eliminado = false;  //(oDR["cli_delet"].ToString() == "S") ? true : false;
            }
            if (oDR["cli_tidoc"] != DBNull.Value)
            {
                oCliente.TipoDocumento = new TipoDocumento((int)oDR["cli_tidoc"], oDR["tid_descr"].ToString());
            }

            if (oDR["cli_provi"] != DBNull.Value)
            {
                oCliente.Provincia = new Provincia((int)oDR["cli_provi"], oDR["pro_descr"].ToString());
            }


            if (oDR["cli_tiiva"] != DBNull.Value)
            {
                oCliente.TipoIVA = new TipoIVA(Convert.ToInt32(oDR["cli_tiiva"]), Convert.ToString(oDR["tip_descr"]));

                //// Modificacion de Ezequiel: Estas lineas se comentaron temporalmente porque el campo "tif_codig" no es devuelto por el SP "Facturacion.usp_Clientes_getClientesLista"
                if (oDR["tif_codig"] != DBNull.Value)
                {
                    oCliente.TipoFactura = new TipoFactura(oDR["tif_codig"].ToString(), oDR["tif_descr"].ToString());
                }
            }

            //DataTable dtN = oDR.GetSchemaTable();
            //bool Encontro = false;

            //foreach (DataRow dRitem in dtN.Select("ColumnName = 'cli_ntarj'"))
            //    Encontro = true;

            //if (Encontro)
            //{
            if (oDR["cli_ntarj"] != DBNull.Value)
            {
                oCliente.NumeroTarjeta = Convert.ToString(oDR["cli_ntarj"]);
            }
            else
            {
                oCliente.NumeroTarjeta = "";
            }
            //}
            //else
            //{
            //    oCliente.NumeroTarjeta = "";
            //}

            return oCliente;
        }

        private static Cliente CargarClientePorVehiculo(System.Data.IDataReader oDR, bool bLocal)
        {
            Cliente oCliente = new Cliente();

            oCliente.NumeroCliente = (int)oDR["cli_numcl"];
            oCliente.RazonSocial = oDR["cli_nombr"].ToString();
            oCliente.NumeroDocumento = oDR["cli_docum"].ToString();
            oCliente.Domicilio = oDR["cli_domic"].ToString();
            oCliente.Localidad = oDR["cli_local"].ToString();
            oCliente.Telefono = oDR["cli_telef"].ToString();
            
            if (!bLocal)
            {
                oCliente.Comentario = oDR["cli_comen"].ToString();
                oCliente.Expediente = oDR["cli_exped"].ToString();
                oCliente.Email = oDR["cli_email"].ToString();
                oCliente.Eliminado = false;  //(oDR["cli_delet"].ToString() == "S") ? true : false;
            }
            if (oDR["cli_tidoc"] != DBNull.Value)
            {
                oCliente.TipoDocumento = new TipoDocumento((int)oDR["cli_tidoc"], oDR["tid_descr"].ToString());
            }

            if (oDR["cli_provi"] != DBNull.Value)
            {
                oCliente.Provincia = new Provincia((int)oDR["cli_provi"], oDR["pro_descr"].ToString());
            }


            if (oDR["cli_tiiva"] != DBNull.Value)
            {
                oCliente.TipoIVA = new TipoIVA(Convert.ToInt32(oDR["cli_tiiva"]), Convert.ToString(oDR["tip_descr"]));

                //// Modificacion de Ezequiel: Estas lineas se comentaron temporalmente porque el campo "tif_codig" no es devuelto por el SP "Facturacion.usp_Clientes_getClientesLista"
                //if (oDR["tif_codig"] != DBNull.Value)
                //{
                //    oCliente.TipoFactura = new TipoFactura(oDR["tif_codig"].ToString(), oDR["tif_descr"].ToString());
                //}
            }

            //DataTable dtN = oDR.GetSchemaTable();
            //bool Encontro = false;

            //foreach (DataRow dRitem in dtN.Select("ColumnName = 'cli_ntarj'"))
            //    Encontro = true;

            //if (Encontro)
            //{
            if (oDR["cli_ntarj"] != DBNull.Value)
            {
                oCliente.NumeroTarjeta = Convert.ToString(oDR["cli_ntarj"]);
            }
            else
            {
                oCliente.NumeroTarjeta = "";
            }
            //}
            //else
            //{
            //    oCliente.NumeroTarjeta = "";
            //}
       
            //Nicolas Tarlao 29/04/2011
            oCliente.Vehiculos.Add(CargarVehiculoCliente(oDR));

            return oCliente ;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Cliente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addCliente(Conexion oConn, Cliente oCliente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_Agregar";

            SqlParameter parCliente = oCmd.Parameters.Add("@Client", SqlDbType.Int);
            parCliente.Direction = ParameterDirection.Output;

            oCmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = oCliente.RazonSocial;
            oCmd.Parameters.Add("@Tipdoc", SqlDbType.Int).Value = oCliente.TipoDocumento.Codigo;
            oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = oCliente.NumeroDocumento;
            oCmd.Parameters.Add("@Direc", SqlDbType.VarChar, 100).Value = oCliente.Domicilio;
            oCmd.Parameters.Add("@Provi", SqlDbType.Int).Value = oCliente.Provincia.Codigo;
            oCmd.Parameters.Add("@Locali", SqlDbType.VarChar, 30).Value = oCliente.Localidad;
            oCmd.Parameters.Add("@Telef", SqlDbType.VarChar, 15).Value = oCliente.Telefono;
            oCmd.Parameters.Add("@Comen", SqlDbType.VarChar, 3000).Value = oCliente.Comentario;
            oCmd.Parameters.Add("@TipoIva", SqlDbType.Int).Value = oCliente.TipoIVA.Codigo;
            oCmd.Parameters.Add("@Exped", SqlDbType.Char,10).Value = oCliente.Expediente;
            oCmd.Parameters.Add("@EMail", SqlDbType.VarChar,100).Value = oCliente.Email;
            //oCmd.Parameters.Add("@NroTarjeta", SqlDbType.VarChar, 300).Value = oCliente.NumeroTarjeta;
                
            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parCliente.Value != DBNull.Value)
            {
                oCliente.NumeroCliente = Convert.ToInt32(parCliente.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cliente ya existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Cliente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updCliente(Conexion oConn, Cliente oCliente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_Guardar";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCliente.NumeroCliente;
            oCmd.Parameters.Add("@Nombre", SqlDbType.VarChar, 50).Value = oCliente.RazonSocial;
            oCmd.Parameters.Add("@Tipdoc", SqlDbType.Int).Value = oCliente.TipoDocumento.Codigo;
            oCmd.Parameters.Add("@Docum", SqlDbType.VarChar, 15).Value = oCliente.NumeroDocumento;
            oCmd.Parameters.Add("@Direc", SqlDbType.VarChar, 100).Value = oCliente.Domicilio;
            oCmd.Parameters.Add("@Provi", SqlDbType.Int).Value = oCliente.Provincia.Codigo;
            oCmd.Parameters.Add("@Locali", SqlDbType.VarChar, 30).Value = oCliente.Localidad;
            oCmd.Parameters.Add("@Telef", SqlDbType.VarChar, 15).Value = oCliente.Telefono;
            oCmd.Parameters.Add("@Comen", SqlDbType.VarChar, 3000).Value = oCliente.Comentario;
            oCmd.Parameters.Add("@TipoIva", SqlDbType.Int).Value = oCliente.TipoIVA.Codigo;
            oCmd.Parameters.Add("@Exped", SqlDbType.Char, 10).Value = oCliente.Expediente;
            oCmd.Parameters.Add("@EMail", SqlDbType.VarChar, 100).Value = oCliente.Email;
            //oCmd.Parameters.Add("@NroTarjeta", SqlDbType.VarChar, 300).Value = oCliente.NumeroTarjeta;

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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cliente no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Cliente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente a eliminar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delCliente(Conexion oConn, Cliente oCliente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_Eliminar";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCliente.NumeroCliente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cliente no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        #endregion

        #region VEHICULO: Metodos de Vehiculos
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega un Vehiculo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oVehiculo">Vehiculo - Objeto con la informacion del vehiculo a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addVehiculo(Conexion oConn, Vehiculo oVehiculo)
        {
            return guardaVehiculo(true, oConn, oVehiculo);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Vehiculo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oVehiculo">Vehiculo - Objeto con la informacion del vehiculo a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updVehiculo(Conexion oConn, Vehiculo oVehiculo)
        {
            return guardaVehiculo(false, oConn, oVehiculo);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega o Modifica un Vehiculo
        /// </summary>
        /// <param name="alta">bool - true para el alta</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oVehiculo">Vehiculo - Objeto con la informacion del vehiculo a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        private static bool guardaVehiculo(bool alta, Conexion oConn, Vehiculo oVehiculo)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_Guardar";

            oCmd.Parameters.Add("@alta", SqlDbType.Char, 1).Value = alta?"S":"N";
            oCmd.Parameters.Add("@patente", SqlDbType.VarChar, 8).Value = oVehiculo.Patente.Trim();
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oVehiculo.Cliente.NumeroCliente;
            oCmd.Parameters.Add("@Marca", SqlDbType.Int).Value = oVehiculo.Marca.Codigo;
            if( oVehiculo.Modelo != null )
            {
                oCmd.Parameters.Add("@Modelo", SqlDbType.Int).Value = oVehiculo.Modelo.Codigo;
            }
            oCmd.Parameters.Add("@Color", SqlDbType.Int).Value = oVehiculo.Color.Codigo;
            oCmd.Parameters.Add("@Categ", SqlDbType.Int).Value = oVehiculo.Categoria.Categoria;
            oCmd.Parameters.Add("@Fecve", SqlDbType.DateTime).Value = oVehiculo.FechaVencimiento;
            oCmd.Parameters.Add("@clv_ntag", SqlDbType.NVarChar, 10).Value = oVehiculo.Tag.NumeroTag;
            oCmd.Parameters.Add("@clv_emitg", SqlDbType.NVarChar, 5).Value = oVehiculo.Tag.EmisorTag;
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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    if( alta )
                    {
                        msg = Traduccion.Traducir("La patente del Vehículo ya existe");
                    }
                    else
                    {
                        msg = Traduccion.Traducir("La patetne de Vehículo no existe");
                    }
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina un Vehiculo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oVehiculo">Vehiculo - Objeto con la informacion del vehiculo a eliminar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delVehiculo(Conexion oConn, Vehiculo oVehiculo)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_Eliminar";

            oCmd.Parameters.Add("@patente", SqlDbType.VarChar, 8).Value = oVehiculo.Patente.Trim();


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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("La patetne de Vehículo no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega que patente cambio y por cual cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="nCoest">Codigo de Estacion - El codigo de la estacion a la cual pertenece</param>
        /// <param name="sUsrID">Usuario Id - El Id del usuario que realiza el cambio de la patente</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente que posee el vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addCambioPatente(Conexion oConn, int? intCodEst, string strUsrId, int? intCliente, string strPatente, string sNuevaPatente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_addAuditoriaCambioMatricula";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = intCodEst;
            oCmd.Parameters.Add("@UsrId", SqlDbType.VarChar).Value = strUsrId;
            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = intCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = strPatente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = sNuevaPatente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega que vehiculo cambio y por cual cambio
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="nCoest">Codigo de Estacion - El codigo de la estacion a la cual pertenece</param>
        /// <param name="sUsrID">Usuario Id - El Id del usuario que realiza el cambio de la patente</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente que posee el vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addAuditoriaCambioVehiculo(Conexion oConn, int? intCodEst, string strUsrId, Cliente oCliente, Vehiculo oVehiculo, Vehiculo oNewVehiculo)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_addAuditoriaCambioVehiculo";

            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = intCodEst;
            oCmd.Parameters.Add("@UsrId", SqlDbType.VarChar).Value = strUsrId;
            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oCliente.NumeroCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = oVehiculo.Patente;
            oCmd.Parameters.Add("@OldDescMarca", SqlDbType.VarChar).Value = oVehiculo.Marca.Descripcion;
            oCmd.Parameters.Add("@OldDescModelo", SqlDbType.VarChar).Value = (oVehiculo.Modelo == null?string.Empty:oVehiculo.Modelo.Descripcion);
            oCmd.Parameters.Add("@OldDescColor", SqlDbType.VarChar).Value = (oVehiculo.Color == null ? string.Empty : oVehiculo.Color.Descripcion);
            oCmd.Parameters.Add("@OldCatego", SqlDbType.TinyInt).Value = (oVehiculo.Categoria == null ? 0 : oVehiculo.Categoria.Categoria);
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = oNewVehiculo.Patente;
            oCmd.Parameters.Add("@NewDescMarca", SqlDbType.VarChar).Value = oNewVehiculo.Marca.Descripcion;
            oCmd.Parameters.Add("@NewDescModelo", SqlDbType.VarChar).Value = (oNewVehiculo.Modelo == null ? string.Empty : oNewVehiculo.Modelo.Descripcion);
            oCmd.Parameters.Add("@NewDescColor", SqlDbType.VarChar).Value = (oNewVehiculo.Color == null ? string.Empty : oNewVehiculo.Color.Descripcion);
            oCmd.Parameters.Add("@NewCatego", SqlDbType.TinyInt).Value = (oNewVehiculo.Categoria == null ? 0 : oNewVehiculo.Categoria.Categoria);
            oCmd.Parameters.Add("@NewFecVenc", SqlDbType.DateTime).Value = (oNewVehiculo.FechaVencimiento == null ? null : oNewVehiculo.FechaVencimiento); ;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda la nueva relacion cliente-patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intCliente">Cliente - El cliente al cual pretenece el vehiculo</param>
        /// <param name="strPatente">Patente - La patente del vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool setClienteVehiculo(Conexion oConn, int? intCliente , string strPatente, string sNuevaPatente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_SetClienteVehiculo";

            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = intCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = strPatente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = sNuevaPatente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda la nueva relacion cliente-vehiculo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intCliente">Cliente - El cliente al cual pretenece el vehiculo</param>
        /// <param name="strPatente">Patente - La patente del vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool setNewVehiculo(Conexion oConn, Cliente oCliente,Vehiculo newVehiculo)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_SetNewVehiculo";

            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = oCliente.NumeroCliente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = newVehiculo.Patente;
            oCmd.Parameters.Add("@NewCodMarca", SqlDbType.Int).Value = newVehiculo.Marca.Codigo;
            if (newVehiculo.Modelo == null)
            {
                oCmd.Parameters.Add("@NewCodModelo", SqlDbType.Int).Value = null;
            }
            else
            {
                oCmd.Parameters.Add("@NewCodModelo", SqlDbType.Int).Value = newVehiculo.Modelo.Codigo;
            }

            oCmd.Parameters.Add("@NewCodColor", SqlDbType.Int).Value = newVehiculo.Color.Codigo;
            oCmd.Parameters.Add("@NewCatego", SqlDbType.Int).Value = (newVehiculo.Categoria==null?0:newVehiculo.Categoria.Categoria);
            oCmd.Parameters.Add("@NewFecVenc", SqlDbType.DateTime).Value = newVehiculo.FechaVencimiento;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica la relacion cuenta-cliente-patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intCliente">Cliente - El cliente al cual pretenece el vehiculo</param>
        /// <param name="strPatente">Patente - La patente del vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updPatente(Conexion oConn, int? intCoest ,int? intCliente, string strPatente, string sNuevaPatente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Vehiculo_updPatente";

            oCmd.Parameters.Add("@Coest", SqlDbType.Int).Value = intCoest;
            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = intCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = strPatente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = sNuevaPatente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        #endregion

        #region PREPAGOS: Metodos de tarjetas prepagas

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda la nueva relacion tag-patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intCliente">Cliente - El cliente al cual pretenece el vehiculo</param>
        /// <param name="strPatente">Patente - La patente del vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updTagVehiculo(Conexion oConn, int? intCliente, string strPatente, string sNuevaPatente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaTags_updTag";

            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = intCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = strPatente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = sNuevaPatente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Guarda la nueva relacion chip-patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="intCliente">Cliente - El cliente al cual pretenece el vehiculo</param>
        /// <param name="strPatente">Patente - La patente del vehiculo</param>
        /// <param name="sNuevaPatente">Patente - La nueva patente del vehiculo</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updChipVehiculo(Conexion oConn, int? intCliente, string strPatente, string sNuevaPatente)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_EntregaChips_updChip";

            oCmd.Parameters.Add("@Cliente", SqlDbType.Int).Value = intCliente;
            oCmd.Parameters.Add("@OldPaten", SqlDbType.VarChar).Value = strPatente;
            oCmd.Parameters.Add("@NewPaten", SqlDbType.VarChar).Value = sNuevaPatente;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval == 0)
            {
                ret = true;
            }
            return ret;
        }
       
        #endregion

        #region VEHICULO INFO: Metodos de la VehiculoInfo.

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve Información de un vehículo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente a filtrar</param>
        /// <returns>Información del vehículo</returns>
        /// ***********************************************************************************************
        public static VehiculoInfo getVehiculoInfo(Conexion oConn, string patente)
        {
            VehiculoInfo oVehiculoInfo = new VehiculoInfo();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cliente_getVehiculoPatente";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();

            //if (oDR.HasRows)
            if (oDR.Read())
            {
                oVehiculoInfo.nombreCliente = Conversiones.edt_Str(oDR["cli_nombr"]);
                oVehiculoInfo.marca = Conversiones.edt_Str(oDR["mar_descr"]);
                oVehiculoInfo.modelo = Conversiones.edt_Str(oDR["mod_descr"]);
                oVehiculoInfo.color = Conversiones.edt_Str(oDR["col_descr"]);
                oVehiculoInfo.categoria = Conversiones.edt_Str(oDR["cat_descr"]);
            }
            else
            {
                oVehiculoInfo.nombreCliente = "";
                oVehiculoInfo.marca = "";
                oVehiculoInfo.modelo = "";
                oVehiculoInfo.color = "";
                oVehiculoInfo.categoria = "";
            }

            //while (oDR.Read())
            //{
            //    oColores.Add(CargarColores(oDR));
            //}

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculoInfo;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve Información de un vehículo - COMPLETA
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente a filtrar</param>
        /// <returns>Información del vehículo completa</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoCompleta getVehiculoInfoCompleta(Conexion oConn, string patente, int? codEstacion)
        {
            VehiculoInfoCompleta oVehiculoInfoCompleta = new VehiculoInfoCompleta();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cliente_getVehiculoPatenteEstacion";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt).Value = codEstacion;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();

            //if (oDR.HasRows)
            if (oDR.Read())
            {
                oVehiculoInfoCompleta.clienteCodigo = Conversiones.edt_Int(oDR["cli_numcl"]);
                oVehiculoInfoCompleta.clienteNombre = Conversiones.edt_Str(oDR["cli_nombr"]);
                oVehiculoInfoCompleta.patente = Conversiones.edt_Str(oDR["clv_paten"]);
                oVehiculoInfoCompleta.catCodigo = Conversiones.edt_Int(oDR["clv_categ"]);
                oVehiculoInfoCompleta.catDescripcion = Conversiones.edt_Str(oDR["cat_descr"]);
                oVehiculoInfoCompleta.marcaCodigo = Conversiones.edt_Int(oDR["clv_marca"]);
                oVehiculoInfoCompleta.marcaDescripcion = Conversiones.edt_Str(oDR["mar_descr"]);
                oVehiculoInfoCompleta.modeloCodigo = Conversiones.edt_Int(oDR["clv_model"]);
                oVehiculoInfoCompleta.modeloDescripcion = Conversiones.edt_Str(oDR["mod_descr"]);
                oVehiculoInfoCompleta.colorCodigo = Conversiones.edt_Int(oDR["clv_color"]);
                oVehiculoInfoCompleta.colorDescripcion = Conversiones.edt_Str(oDR["col_descr"]);
                oVehiculoInfoCompleta.tipoCuenta = Conversiones.edt_Str(oDR["ctg_descr"]);
                oVehiculoInfoCompleta.causaInhabilitacion = Conversiones.edt_Str(oDR["CausaInhabilitacion"]);
                oVehiculoInfoCompleta.saldo = Conversiones.edt_Str(oDR["pre_saldo"]);
                oVehiculoInfoCompleta.maxGiroEnRojo = Conversiones.edt_Str(oDR["pre_grojo"]);
                oVehiculoInfoCompleta.chipNumero = Conversiones.edt_Str(oDR["chi_numer"]);
                oVehiculoInfoCompleta.chipNumeroExterno = Conversiones.edt_Str(oDR["chi_nuext"]);
                oVehiculoInfoCompleta.chipEnListaNegra = Conversiones.edt_Str(oDR["ChipListaNegra"]);
                oVehiculoInfoCompleta.tagNumero = Conversiones.edt_Str(oDR["tag_numer"]);
                oVehiculoInfoCompleta.tagEnListaNegra = Conversiones.edt_Str(oDR["TagListaNegra"]);
            }
            else
            {
                oVehiculoInfoCompleta.clienteCodigo = 0;
                oVehiculoInfoCompleta.clienteNombre = "";
                oVehiculoInfoCompleta.patente = "";
                oVehiculoInfoCompleta.catCodigo = 0;
                oVehiculoInfoCompleta.catDescripcion = "";
                oVehiculoInfoCompleta.marcaCodigo = 0;
                oVehiculoInfoCompleta.marcaDescripcion = "";
                oVehiculoInfoCompleta.modeloCodigo = 0;
                oVehiculoInfoCompleta.modeloDescripcion = "";
                oVehiculoInfoCompleta.colorCodigo = 0;
                oVehiculoInfoCompleta.colorDescripcion = "";
                oVehiculoInfoCompleta.tipoCuenta = "";
                oVehiculoInfoCompleta.causaInhabilitacion = "";
                oVehiculoInfoCompleta.saldo = "";
                oVehiculoInfoCompleta.maxGiroEnRojo = "";
                oVehiculoInfoCompleta.chipNumero = "";
                oVehiculoInfoCompleta.chipNumeroExterno = "";
                oVehiculoInfoCompleta.chipEnListaNegra = "";
                oVehiculoInfoCompleta.tagNumero = "";
                oVehiculoInfoCompleta.tagEnListaNegra = "";
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculoInfoCompleta;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los vehiculos y estaciones habilitadas para un vehiculo
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="sPatente">string - Patente para la busqueda</param>
        /// <param name="sEmisor">string - Número de Emisor para la busqueda</param>
        /// <param name="sTag">string - Número de Tag para la busqueda</param>
        /// <returns>Devuelve la lista de los últimos tránsitos de una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoEstacionesHabilitadasL getEstacionesHabilitadasVehiculo(Conexion oConn, string sPatente, string sEmisor, string sTag)
        {
            VehiculoEstacionesHabilitadasL oTransitos = new VehiculoEstacionesHabilitadasL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vehiculo_getVehiculoEstacionesHabilitadas";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = sPatente;
            oCmd.Parameters.Add("@emiTg", SqlDbType.VarChar, 5).Value = sEmisor;
            oCmd.Parameters.Add("@ntag", SqlDbType.VarChar, 8).Value = sTag;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            oTransitos = CargarEstacionesHabilitadasVehiculo(oDR);

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTransitos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto VehiculoEstacionHabilitadaInfo
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info del tránsito</param>
        /// <returns>Objeto VehiculoEstacionHabilitadaInfo con los datos</returns>
        /// ***********************************************************************************************
        private static VehiculoEstacionesHabilitadasL CargarEstacionesHabilitadasVehiculo(IDataReader oDR)
        {
            VehiculoEstacionesHabilitadasL vehicEstacionesHab = new VehiculoEstacionesHabilitadasL();

            bool bLeer = oDR.Read();

            while (bLeer)
            {
                VehiculoEstacionesHabilitadas vehicEstacionHab = new VehiculoEstacionesHabilitadas();

                vehicEstacionHab.Patente = Conversiones.edt_Str(oDR["veh_paten"]);

                vehicEstacionHab.Tag = new Tag();
                vehicEstacionHab.Tag.EmisorTag = Conversiones.edt_Str(oDR["veh_emitg"]);
                vehicEstacionHab.Tag.NumeroTag = Conversiones.edt_Str(oDR["veh_ntag"]);
                
                vehicEstacionHab.SubestacionesHabilitadas = new SubestacionL();

                while (bLeer &&
                        vehicEstacionHab.Patente == Conversiones.edt_Str(oDR["veh_paten"]) &&
                        vehicEstacionHab.Tag.EmisorTag == Conversiones.edt_Str(oDR["veh_emitg"]) &&
                        vehicEstacionHab.Tag.NumeroTag == Conversiones.edt_Str(oDR["veh_ntag"]))
                {
                    Subestacion subestacion = new Subestacion();

                    subestacion.Descripcion = Conversiones.edt_Str(oDR["sub_descr"]);
                    subestacion.SentidoCardinal = Conversiones.edt_Str(oDR["sub_sencar"]);

                    vehicEstacionHab.SubestacionesHabilitadas.Add(subestacion);
                    bLeer = oDR.Read();
                }
                vehicEstacionesHab.Add(vehicEstacionHab);
            }

            return vehicEstacionesHab;
        }





        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de los últimos tránsitos de una patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="sPatente">string - Patente para la busqueda</param>
        /// <param name="sEmisor">string - Número del emisor de Tag para la busqueda</param>
        /// <param name="sTag">string - Número de Tag para la busqueda</param>
        /// <returns>Devuelve la lista de los últimos tránsitos de una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoUltTransitosL getVehiculoUltTransitos(Conexion oConn, string sPatente, string sEmisor, string sTag, string sCPF,int? top)
        {
            VehiculoInfoUltTransitosL transitos = new VehiculoInfoUltTransitosL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Validacion.usp_Cliente_getUltimosTransitosVehiculo";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = sPatente;
            oCmd.Parameters.Add("@fecha", SqlDbType.DateTime, 8).Value = DateTime.Now;
            oCmd.Parameters.Add("@emiTg", SqlDbType.VarChar, 5).Value = sEmisor;
            oCmd.Parameters.Add("@ntag", SqlDbType.VarChar, 24).Value = sTag;
            oCmd.Parameters.Add("@cpf", SqlDbType.VarChar,8).Value = sCPF;
            oCmd.Parameters.Add("@top", SqlDbType.Int, 10).Value = top;
            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                transitos.Add(CargarTransito(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return transitos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto VehiculoInfoUltTransitos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info del tránsito</param>
        /// <returns>Objeto VehiculoInfoUltTransitos con los datos</returns>
        /// ***********************************************************************************************
        private static VehiculoInfoUltTransitos CargarTransito(System.Data.IDataReader oDR)
        {
            VehiculoInfoUltTransitos oTransito = new VehiculoInfoUltTransitos();



            oTransito.estacionCodigo = Conversiones.edt_Int16(oDR["Estacion"]);
            oTransito.estacionNombre = Conversiones.edt_Str(oDR["Nombre"]);
            oTransito.via = new Via(Conversiones.edt_Int16(oDR["Estacion"]), Conversiones.edt_Byte(oDR["Via"]), Conversiones.edt_Str(oDR["ViaNombre"]));
            oTransito.sentido = Conversiones.edt_Str(oDR["Sentido"]);

            if (oTransito.sentido == "A")
            {
                oTransito.sentido = "Ascendente";
            }
            else
            {
                if (oTransito.sentido == "D")
                {
                    oTransito.sentido = "Descendente";
                }
                else
                {
                    oTransito.sentido = "";
                }
            }

            oTransito.fechaHora = Conversiones.edt_DateTime(oDR["Fecha"]);
            oTransito.catTab = Conversiones.edt_Int16(oDR["Tabulada"]);
            oTransito.catDac = Conversiones.edt_Int16(oDR["Detectada"]);
            oTransito.catCons = Conversiones.edt_Int16(oDR["Consolidada"]);
            oTransito.catTabulada = oDR["ManuaDescr"].ToString();
            oTransito.catDetectada = oDR["DacDescr"].ToString();
            oTransito.catConsolidada = oDR["CatDesCons"].ToString();
            oTransito.formaPago = Conversiones.edt_Str(oDR["FormaPago"]);
            oTransito.tipo = Conversiones.edt_Str(oDR["Agrupacion"]);
            oTransito.clienteNombre = Conversiones.edt_Str(oDR["Cliente"]);
            oTransito.videoFoto1 = Conversiones.edt_Str(oDR["tra_video"]);
            oTransito.videoFoto2 = Conversiones.edt_Str(oDR["tra_video2"]);
            oTransito.Patente = Conversiones.edt_Str(oDR["Placa"]);
            oTransito.NumeroDeEvento = Conversiones.edt_Int32(oDR["Numev"]);
            oTransito.TagTamperizado = Conversiones.edt_Str(oDR["TagTamperizado"]);   
            

            //transito.fechaHora = Conversiones.edt_DateTime(oDR["trc_fecha"]);
            //transito.catTab = Conversiones.edt_Int16(oDR["tra_manua"]);
            //transito.catDac = Conversiones.edt_Int16(oDR["tra_dac"]);
            //transito.catCons = Conversiones.edt_Int16(oDR["dvs_categ"]);
            //transito.formaPago = Conversiones.edt_Str(oDR["for_descr"]);
            //transito.tipo = Conversiones.edt_Str(oDR["ctg_descr"]);
            //transito.clienteNombre = Conversiones.edt_Str(oDR["cli_nombr"]);
            //transito.videoFoto1 = Conversiones.edt_Str(oDR["tra_video"]);
            //transito.videoFoto2 = Conversiones.edt_Str(oDR["tra_video2"]);
            //transito.Patente = Conversiones.edt_Str(oDR["tra_paten"]);
            //transito.NumeroDeEvento = Conversiones.edt_Int32(oDR["eve_ident"]);          
            //transito.cantEjesDobles = Conversiones.edt_Int32(oDR["tra_ejerd"]);
            //transito.cantEjesSimples = Conversiones.edt_Int32(oDR["tra_ejers"]);
            //transito.cantEjesSusp = Conversiones.edt_Int32(oDR["tra_exsus"]);
            //transito.Ticket = Conversiones.edt_Int32(oDR["tra_ticke"]);
            //transito.Altura = Conversiones.edt_Str(oDR["tra_altur"]);

            //transito.Tag = new Tag();
            //transito.Tag.EmisorTag = Conversiones.edt_Str(oDR["trc_emitg"]);
            //transito.Tag.NumeroTag = Conversiones.edt_Str(oDR["trc_ntag"]);

            oTransito.Tag = new Tag();
            oTransito.Tag.EmisorTag = Conversiones.edt_Str(oDR["EmisorTag"]);
            oTransito.Tag.NumeroTag = Conversiones.edt_Str(oDR["NumeroTag"]);
            

            return oTransito;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el detalle de deuda de una patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xsPatente">string - Patente para la busqueda</param>
        /// <param name="cpf">numero de CPF para el caso de que busquemos las deudas.</param>
        /// <returns>Devuelve el detalle de deuda de una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoDeudaDetalleL getVehiculoDeudaDetalle(Conexion oConn, string patente,string cpf)
        {
            VehiculoInfoDeudaDetalleL oDetalleDeuda = new VehiculoInfoDeudaDetalleL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Deudas_getDetalleDeudaVehiculo";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@cpf", SqlDbType.VarChar, 8).Value = cpf;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oDetalleDeuda.Add(CargarDeuda(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oDetalleDeuda;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto VehiculoInfoDeudaDetalle
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info de la deuda</param>
        /// <returns>Objeto VehiculoInfoDeudaDetalle con los datos</returns>
        /// ***********************************************************************************************
        private static VehiculoInfoDeudaDetalle CargarDeuda(System.Data.IDataReader oDR)
        {
            VehiculoInfoDeudaDetalle oDeuda = new VehiculoInfoDeudaDetalle();

            oDeuda.estacionNumero = Conversiones.edt_Byte(oDR["pag_estge"]);
            oDeuda.estacionNombre = Conversiones.edt_Str(oDR["est_nombr"]);
            oDeuda.via = Conversiones.edt_Int16(oDR["pag_viage"]);
            oDeuda.eventoNumero = Conversiones.edt_Int32(oDR["pag_numev"]);
            oDeuda.sentido = Conversiones.edt_Str(oDR["tra_senti"]);

            if (oDeuda.sentido == "A")
            {
                oDeuda.sentido = "Ascendente";
            }
            else
            {
                if (oDeuda.sentido == "D")
                {
                    oDeuda.sentido = "Descendente";
                }
                else
                {
                    oDeuda.sentido = "";
                }
            }

            oDeuda.fechaHora = Conversiones.edt_DateTime(oDR["pag_fecge"]);
            oDeuda.catTab = Conversiones.edt_Int16(oDR["tra_manua"]);
            oDeuda.catDac = Conversiones.edt_Int16(oDR["tra_dac"]);
            oDeuda.catCons = Conversiones.edt_Int16(oDR["dvs_categ"]);
            oDeuda.importe = Conversiones.edt_Str(oDR["pag_monge"]);
            oDeuda.tipo = Conversiones.edt_Str(oDR["Tipo"]);
            oDeuda.marca = Conversiones.edt_Str(oDR["pag_marca"]);
            oDeuda.clienteNombre = Conversiones.edt_Str(oDR["pag_nombr"]);
            oDeuda.numeroPD = Conversiones.edt_Str(oDR["pag_numer"]);
            oDeuda.videoFoto1 = Conversiones.edt_Str(oDR["tra_video"]);
            oDeuda.videoFoto2 = Conversiones.edt_Str(oDR["tra_video2"]);
            
            return oDeuda;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el total de deuda de una patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="xsPatente">string - Patente para la busqueda</param>
        /// <returns>Devuelve el total de deuda de una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoDeudaTotalL getVehiculoDeudaTotal(Conexion oConn, string cpf,string xsPatente)
        {
            VehiculoInfoDeudaTotalL oTotalDeuda = new VehiculoInfoDeudaTotalL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Deudas_getTotalDeudaVehiculo";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = xsPatente;
            oCmd.Parameters.Add("@cpf", SqlDbType.VarChar, 8).Value = cpf;

            oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTotalDeuda.Add(CargarTotalDeuda(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTotalDeuda;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto VehiculoInfoDeudaTotal
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader con info del todal de la deuda</param>
        /// <returns>Objeto VehiculoInfoDeudaTotal con los datos</returns>
        /// ***********************************************************************************************
        private static VehiculoInfoDeudaTotal CargarTotalDeuda(System.Data.IDataReader oDR)
        {
            VehiculoInfoDeudaTotal oTotalDeuda = new VehiculoInfoDeudaTotal();

            oTotalDeuda.tipo = Conversiones.edt_Str(oDR["Tipo"]);
            oTotalDeuda.cantidad = Conversiones.edt_Int(oDR["Cantidad"]);
            oTotalDeuda.importe = Conversiones.edt_Str(oDR["Monto"]);

            return oTotalDeuda;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve el total de deuda de una patente
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="sPatente">string - Patente para la busqueda</param>
        /// <param name="sTag">string - Tag para la busqueda</param>
        /// <returns>Devuelve el total de deuda de una patente</returns>
        /// ***********************************************************************************************
        public static VehiculoInfoCompletaL getVehiculoPatenteTag(Conexion oConn, string sPatente, string sEmisor, string sTag)
        {
            VehiculoInfoCompletaL vehiculos = new VehiculoInfoCompletaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Peaje.usp_Vehiculo_getVehiculoPatenteTag";
            if( sPatente != "" )
                oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = sPatente;
            if( sEmisor != "")
                oCmd.Parameters.Add("@emiTg", SqlDbType.VarChar, 5).Value = sEmisor;
            if( sTag != "")
                oCmd.Parameters.Add("@ntag", SqlDbType.VarChar, 24).Value = sTag;

            //oCmd.CommandTimeout = 120;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                vehiculos.Add(CargarVehiculoPatenteTag(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return vehiculos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga una entidad VehiculoInfoCompleta con los datos traidos de la base de datos a través de un DataReader
        /// </summary>
        /// <param name="oDR"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        private static VehiculoInfoCompleta CargarVehiculoPatenteTag(IDataReader oDR)
        {



            VehiculoInfoCompleta unVehiculo = new VehiculoInfoCompleta();
            unVehiculo.Tag = new Tag();
            unVehiculo.Exento = new ExentoCodigo();

            unVehiculo.patente = Conversiones.edt_Str(oDR["Patente"]);
            unVehiculo.Tag.EmisorTag = Conversiones.edt_Str(oDR["Emisor"]);
            unVehiculo.Tag.NumeroTag = Conversiones.edt_Str(oDR["Tag"]);
            unVehiculo.catCodigo = Conversiones.edt_Int(oDR["Categoria"]);
            if (oDR["Secuencia"] != DBNull.Value)
                unVehiculo.Tag.Secuencia = (int)oDR["Secuencia"];
            if (oDR["SecuenciaTag"] != DBNull.Value)
                unVehiculo.Tag.SecuenciaTag = (int)oDR["SecuenciaTag"];
            unVehiculo.marcaDescripcion = Conversiones.edt_Str(oDR["Marca"]);
            unVehiculo.colorDescripcion = Conversiones.edt_Str(oDR["Color"]);
            unVehiculo.clienteNombre = Conversiones.edt_Str(oDR["Nombre"]);
            if (oDR["Vencimiento"] != DBNull.Value)
                unVehiculo.Tag.FechaVencimiento = Conversiones.edt_DateTime(oDR["Vencimiento"]);
            if (oDR["CodigoExento"] != DBNull.Value)
            {
                unVehiculo.Exento.CodigoExento = Conversiones.edt_Int16(oDR["CodigoExento"]);
                unVehiculo.Exento.DescrExento = Conversiones.edt_Str(oDR["TipoExento"]);
                unVehiculo.CodigoAutorizacion = Conversiones.edt_Str(oDR["CodAutor"]);
            }
            if (oDR["FechaListaNegra"] != DBNull.Value)
                unVehiculo.Tag.FechaListaNegra = Conversiones.edt_DateTime(oDR["FechaListaNegra"]);

            if(unVehiculo.Exento.CodigoExento > 0)
            {
                unVehiculo.Tag.EstadoTag = "Isento";
            }
            else if (Conversiones.edt_Str(oDR["Accion"]) == "01")
            {
                unVehiculo.Tag.EstadoTag = "Lista Nela";
            }
            else if (Conversiones.edt_Str(oDR["Accion"]) == "02")
            {
                unVehiculo.Tag.EstadoTag = "Verificação";
            }
            else if (unVehiculo.Tag.FechaVencimiento != null && unVehiculo.Tag.FechaVencimiento >= DateTime.Now)
            {
                unVehiculo.Tag.EstadoTag = "Isento Vencido";
            }
            else
            {
                unVehiculo.Tag.EstadoTag = "Normal";
            }

            unVehiculo.EsListaBlanca = Conversiones.edt_Str(oDR["ListaBlanca"]);
            //unVehiculo.ListaAValidar = Conversiones.edt_Str(oDR["ListaAValidar"]);  

            return unVehiculo;
        }

        #endregion

        #region VEHICULO: Clase de datos de Vehiculo

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Vehiculos junto con sus datos asociados para una patente y estacion en la que estoy 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="patente">string - Patente a filtrar</param>
        /// <returns>Lista de Vehiculos</returns>
        /// ***********************************************************************************************
        public static VehiculoL getVehiculosPatenteEstacion(Conexion oConn, string patente, int coest, int? tagNuext)
        {
            VehiculoL oVehiculos= new VehiculoL();
            SqlDataReader oDR;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cliente_getVehiculoPatenteEstacion";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt, coest).Value = coest;
            oCmd.Parameters.Add("@tagnuext", SqlDbType.Int, coest).Value = tagNuext;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oVehiculos.Add(CargarVehiculos(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculos;
        }

        public static VehiculoL getVehiculosPatenteEstacionCliente(Conexion oConn, string patente, string nombre, int coest, int? nuext, int xiCantRows, out bool llegoAlTope)
        {
            VehiculoL oVehiculos = new VehiculoL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cliente_getVehiculoPatenteEstacionCliente";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt, coest).Value = coest;
            oCmd.Parameters.Add("@nombre", SqlDbType.VarChar, 20).Value = nombre;
            oCmd.Parameters.Add("@nuext", SqlDbType.Int, coest).Value = nuext;
            oCmd.Parameters.Add("@CantRows", SqlDbType.Int).Value = xiCantRows;
            oDR = oCmd.ExecuteReader();
            int i = 0;
            llegoAlTope = false;
            while (oDR.Read())
            {
                i++;
                oVehiculos.Add(CargarVehiculos(oDR));
                if (i == xiCantRows)
                {
                    llegoAlTope = true;
                    break;
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculos;
        }
        
        public static VehiculoL getVehiculosPatenteEstacionCliente(Conexion oConn, string patente, string nombre,  int coest, int? nuext)
        {
            VehiculoL oVehiculos = new VehiculoL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cliente_getVehiculoPatenteEstacionCliente";
            oCmd.Parameters.Add("@paten", SqlDbType.VarChar, 8).Value = patente;
            oCmd.Parameters.Add("@coest", SqlDbType.TinyInt, coest).Value = coest;
            oCmd.Parameters.Add("@nombre", SqlDbType.VarChar, 20).Value = nombre;
            oCmd.Parameters.Add("@nuext", SqlDbType.Int, coest).Value = nuext;
            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oVehiculos.Add(CargarVehiculos(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vehiculos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Vehiculos y datos asociados</param>
        /// <returns>Lista con el elemento Vehiculo de la base de datos</returns>
        /// ***********************************************************************************************
        private static Vehiculo CargarVehiculos(System.Data.IDataReader oDR)
        {
            Vehiculo oVehiculo = new Vehiculo();


            // AUXILIARES, PARA EL OBJETO FINAL "VEHICULO"

            // Color
            VehiculoColor oColor = null;
            if (oDR["clv_color"] != DBNull.Value)
            {
                oColor = new VehiculoColor((int)oDR["clv_color"],
                                           oDR["col_descr"].ToString(),
                                           "N");
                                           //oDR["col_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }
            
            // Marca
            VehiculoMarca oMarca = null;
            if (oDR["clv_marca"] != DBNull.Value)
            {
                oMarca = new VehiculoMarca((int)oDR["clv_marca"],
                                           oDR["mar_descr"].ToString(),
                                           "N");
                                           //oDR["mar_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }
            
            // Categoria
            CategoriaManual oCategoria = null;
            if (oDR["clv_categ"] != DBNull.Value)
            {
                oCategoria = new CategoriaManual((byte)oDR["clv_categ"], oDR["cat_descr"].ToString());
            }


            // Modelo 
            VehiculoModelo oModelo = null;
            if (oDR["clv_model"] != DBNull.Value)
            {
                oModelo = new VehiculoModelo(oMarca,
                                             (int)oDR["clv_model"],
                                             oDR["mod_descr"].ToString(),
                                             "N");
                                             //oDR["mod_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }

            //patente nueva
             
            //Vehiculo oPatente = null;
            //if (oDR["clv_model"] != DBNull.Value)
            //{
            //    oPatente = new Vehiculo(oPatente,
            //                                 (int)oDR["clv_model"],
            //                                 oDR["mod_descr"].ToString(),
            //                                 "N");
            //  
            //}


            // Tag 
            Tag oTag = null;
            if (oDR["tag_numer"] != DBNull.Value)
            {

                oTag = new Tag();
                oTag.Patente = oDR["clv_paten"].ToString();
                oTag.NumeroTag = oDR["tag_numer"].ToString();
                oTag.Habilitado = "S";
                int? numExt = null;
                if (!String.IsNullOrEmpty(oDR["tag_nuext"].ToString())) numExt = (int)oDR["tag_nuext"];
                oTag.NumeroExterno = numExt;

                // Tag en lista negra
                if (oDR["TagListaNegra"].ToString() == "S")
                {
                    oTag.ListaNegra = new TagListaNegra();
                    oTag.ListaNegra.NumeroTag = oDR["tag_numer"].ToString();
                }

                if (oDR["tag_entre"].ToString() == "S")
                {
                    oTag.Entregado = true;
                }
            }
            
            // Chip
            Chip oChip = null;
            if (oDR["chi_numer"] != DBNull.Value)
            {
                oChip = new Chip();
                oChip.Patente = oDR["clv_paten"].ToString();
                if( oDR["chi_nuint"] != DBNull.Value )
                {
                    oChip.NumeroInterno = (int)oDR["chi_nuint"];
                }
                oChip.NumeroExterno = (int)oDR["chi_nuext"];
                oChip.Habilitado = "S";
                oChip.Dispositivo = oDR["chi_numer"].ToString();
                // Chip en lista negra
                if (oDR["ChipListaNegra"].ToString() == "S")
                {
                    oChip.ListaNegra = new ChipListaNegra();
                    oChip.ListaNegra.NumeroChip = oDR["chi_numer"].ToString();
                }

                if (oDR["chi_entre"].ToString() == "S")
                {
                    oChip.Entregado = true;
                }
            }

            //Vehiculo Existente
            if (oDR["VehiculoExistente"] != DBNull.Value)
            {
                oVehiculo.VehiculoExistente = (oDR["VehiculoExistente"].ToString() == "S" ? true : false);
            }

            // Cliente
            Cliente oCliente = new Cliente();
                        oCliente.NumeroCliente = (int)oDR["cli_numcl"];
                        oCliente.RazonSocial = oDR["cli_nombr"].ToString();
                        oCliente.NumeroDocumento = oDR["cli_docum"].ToString();
                        oCliente.Domicilio = oDR["cli_domic"].ToString();
                        oCliente.Localidad = oDR["cli_local"].ToString();
                        oCliente.Telefono = oDR["cli_telef"].ToString();
                        oCliente.Comentario = oDR["cli_comen"].ToString();
                        oCliente.Expediente = oDR["cli_exped"].ToString();
                        oCliente.Email = oDR["cli_email"].ToString();
                        //oCliente.Eliminado = false;
                        oCliente.Eliminado = (oDR["cli_delet"].ToString() == "S"); 
            
                        // Tipo de documento 
                        TipoDocumento oTipoDocu = null;
                        if (oDR["cli_tidoc"] != DBNull.Value)
                        {
                            oTipoDocu = new TipoDocumento((int)oDR["cli_tidoc"], oDR["tid_descr"].ToString());
                        }

                        // Provincia
                        Provincia oProvincia = null;                        
                        if (oDR["cli_provi"] != DBNull.Value)
                        {
                            oProvincia = new Provincia((int)oDR["cli_provi"], oDR["pro_descr"].ToString());
                        }

                        oCliente.TipoDocumento = oTipoDocu;
                        oCliente.Provincia = oProvincia;
            
            // Cuenta

            TipoCuenta oAuxTipoCuenta = null;
            if (oDR["cta_tipcu"] != DBNull.Value)
            {
                oAuxTipoCuenta = new TipoCuenta((int)oDR["cta_tipcu"], oDR["tic_descr"].ToString());
                oAuxTipoCuenta.TipoBoleto = oDR["tic_tipbo"].ToString();
            }


            AgrupacionCuenta oAuxAgrupacion = null;
            if (oDR["cta_subfp"] != DBNull.Value)
            {
                oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["cta_subfp"], oDR["ctg_descr"].ToString(), null, null, null, null,null, null, null, null);
            }


            Cuenta oCuenta = null;
            if (oDR["cta_numer"] != DBNull.Value)
            {
                oCuenta = new Cuenta((int)oDR["cta_numer"],
                                            oAuxTipoCuenta,
                                            Util.DbValueToNullable<DateTime>(oDR["cta_feegr"]),
                                            oAuxAgrupacion,
                                            oDR["cta_descr"].ToString(),
                                            "N",
                                            oCliente);
                oCuenta.Eliminado = oDR["cta_delet"].ToString();     
                //Estaciones habilitadas
                byte estacion = (byte)oDR["est_codig"];
                CuentaEstacion oCuentaEstacion = new CuentaEstacion();
                oCuentaEstacion.Estacion = new Estacion(estacion, oDR["est_nombr"].ToString());

                if (estacion < ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
                {
                    oCuentaEstacion.Habilitado = (oDR["cte_habil"].ToString() == "S");
                }
                else
                {
                    oCuentaEstacion.Habilitado = true;
                }

                oCuenta.EstacionesHabilitadas = new CuentaEstacionL();
                oCuenta.EstacionesHabilitadas.Add(oCuentaEstacion);
            }


            // ARMAMOS EL OBJETO VEHICULO DEFINITIVO
            oVehiculo.Patente = oDR["clv_paten"].ToString();
            oVehiculo.FechaVencimiento = Util.DbValueToNullable<DateTime>(oDR["clv_fecve"]);
            oVehiculo.Color = oColor;
            oVehiculo.Marca = oMarca;
            oVehiculo.Modelo = oModelo;
            oVehiculo.Cliente = oCliente;
            oVehiculo.Categoria = oCategoria;
            oVehiculo.Tag = oTag;
            oVehiculo.Chip = oChip;
            oVehiculo.Cuenta = oCuenta;
            oVehiculo.CausaInhabilitacion = oDR["CausaInhabilitacion"].ToString();
            //agregado
            //if (oDR["patente_nueva"].ToString() != "" && oDR["patente_nueva"].ToString() != null)
            //{
            //    oVehiculo.Patente = oDR["patente_nueva"].ToString();
            //    oVehiculo.FechaVencimiento = Util.DbValueToNullable<DateTime>(oDR["clv_fecve"]);
            //    oVehiculo.Color = oColor;
            //    oVehiculo.Marca = oMarca;
            //    oVehiculo.Modelo = oPatente;
            //    oVehiculo.Cliente = oCliente;
            //    oVehiculo.Categoria = oCategoria;
            //    oVehiculo.Tag = oTag;
            //    oVehiculo.Chip = oChip;
            //    oVehiculo.Cuenta = oCuenta;
            //    oVehiculo.CausaInhabilitacion = oDR["CausaInhabilitacion"].ToString();
            
            //}
                
            return oVehiculo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Vehiculos de un cliente en particular
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente del que quiero saber sus vehiculos</param>
        /// <returns>Lista de Vehiculos del cliente</returns>
        /// ***********************************************************************************************
        public static VehiculoL getVehiculosCliente(Conexion oConn, int numeroCliente)
        {
            VehiculoL oVehiculos = new VehiculoL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getVehiculosPorCliente";
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = numeroCliente;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oVehiculos.Add(CargarVehiculoCliente(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oVehiculos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Vehiculos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Vehiculos y datos asociados</param>
        /// <returns>Lista con el elemento Vehiculo de la base de datos</returns>
        /// ***********************************************************************************************
        private static Vehiculo CargarVehiculoCliente(System.Data.IDataReader oDR)
        {
            Vehiculo oVehiculo = new Vehiculo();


            // AUXILIARES, PARA EL OBJETO FINAL "VEHICULO"

            // Color
            VehiculoColor oColor = null;
            if (oDR["clv_color"] != DBNull.Value)
            {
                oColor = new VehiculoColor((int)oDR["clv_color"],
                                           oDR["col_descr"].ToString(),
                                           "N");
                                            //oDR["col_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }


            // Marca
            VehiculoMarca oMarca = null;
            if (oDR["clv_marca"] != DBNull.Value)
            {
                oMarca = new VehiculoMarca((int)oDR["clv_marca"],
                                           oDR["mar_descr"].ToString(),
                                           "N");
                                            //oDR["mar_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }


            // Categoria
            CategoriaManual oCategoria = null;
            if (oDR["clv_categ"] != DBNull.Value)
            {
                oCategoria = new CategoriaManual((byte)oDR["clv_categ"],
                                                 oDR["cat_descr"].ToString());
            }


            // Modelo 
            VehiculoModelo oModelo = null;
            if (oDR["clv_model"] != DBNull.Value)
            {
                oModelo = new VehiculoModelo(oMarca,
                                             (int)oDR["clv_model"],
                                             oDR["mod_descr"].ToString(),
                                             "N");
                                             //oDR["mod_delet"].ToString());       // OPEVIAL no tiene todavia baja logica
            }

            // Tag en lista negra
            TagListaNegra oTagLN = null;
            if (oDR["LNTag"].ToString() == "S")
            {
                oTagLN = new TagListaNegra(oDR["lnt_numer"].ToString(), Convert.ToDateTime(oDR["lnt_fecha"]), new Usuario(oDR["lnt_usuar"].ToString(), oDR["nombrUseTag"].ToString()), oDR["lnt_comen"].ToString());
            }

            // Tag 
            Tag oTag = null;
            if (oDR["clv_ntag"] != DBNull.Value)
            {
                int? numExt = null;
                if (oDR["tag_nuext"] != DBNull.Value)
                {
                    numExt = (int)oDR["tag_nuext"];
                }
                oTag = new Tag(null,
                               oDR["clv_paten"].ToString(),
                               oDR["clv_ntag"].ToString(),
                               oDR["tag_habil"].ToString(),
                               DateTime.Now,
                               oTagLN,
                               numExt);
                oTag.EmisorTag = oDR["clv_emitg"].ToString();
            }


            // Chip en lista negra
            ChipListaNegra oChipLN = null;
            if (oDR["LNChip"].ToString() == "S")
            {
                oChipLN = new ChipListaNegra(oDR["lnc_numer"].ToString(), (int)oDR["chi_nuext"], Convert.ToDateTime(oDR["lnc_fecha"]), new Usuario(oDR["lnc_usuar"].ToString(), oDR["nombrUseChip"].ToString()), oDR["lnc_comen"].ToString());
                //oChipLN = new ChipListaNegra(null, DateTime.Now, null, null);

            }

            // Chip
            Chip oChip = null;
            if (oDR["chi_numer"] != DBNull.Value)
            {
                oChip = new Chip(null,
                                 oDR["clv_paten"].ToString(),
                                 oDR["chi_numer"].ToString(),
                                 Convert.ToInt32(oDR["chi_nuext"]),
                                 oDR["chi_habil"].ToString(),
                                 DateTime.Now,
                                 oChipLN,
                                 Util.DbValueToNullable<int>(oDR["chi_nuint"]));
            }



            // ARMAMOS EL OBJETO VEHICULO DEFINITIVO
            oVehiculo.Patente = oDR["clv_paten"].ToString();
            oVehiculo.FechaVencimiento = Util.DbValueToNullable<DateTime>(oDR["clv_fecve"]);
            oVehiculo.Color = oColor;
            oVehiculo.Marca = oMarca;
            oVehiculo.Modelo = oModelo;
            oVehiculo.Categoria = oCategoria;
            oVehiculo.Tag = oTag;
            oVehiculo.Chip = oChip;

            return oVehiculo;
        }
                
        #endregion

        #region CUENTAS: Clase de datos de Cuentas de Clientes

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Cuentas de un cliente determinado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente del que me interesan conocer las cuentas</param>
        /// <returns>Lista de Cuentas del cliente</returns>
        /// ***********************************************************************************************
        public static CuentaL getCuentasCliente(Conexion oConn, int numeroCliente)
        {
            CuentaL oCuentas = new CuentaL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getCuentasPorCliente";
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = numeroCliente;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oCuentas.Add(CargarCuentaCliente(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oCuentas;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Cuentas
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cuentas</param>
        /// <returns>Lista con el elemento Cuenta de la base de datos</returns>
        /// ***********************************************************************************************
        private static Cuenta CargarCuentaCliente(System.Data.IDataReader oDR)
        {
            TipoCuenta oAuxTipoCuenta = null;
            if (oDR["TipoCuenta"] != DBNull.Value)
            {
                oAuxTipoCuenta = new TipoCuenta((int)oDR["TipoCuenta"], oDR["DescTipoCuenta"].ToString());
            }


            AgrupacionCuenta oAuxAgrupacion = null;
            if (oDR["Agrupacion"] != DBNull.Value)
            {
                oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["Agrupacion"], oDR["DescAgrupacion"].ToString(), null, null, null, null, null, null, null, null);
            }

            
            Cuenta oCuenta = new Cuenta((int)oDR["NumCuenta"],
                                        oAuxTipoCuenta,
                                        Util.DbValueToNullable<DateTime>(oDR["FechaEgreso"]),
                                        oAuxAgrupacion,
                                        oDR["Descripcion"].ToString(),
                                        "N",
                                        null);

            return oCuenta;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza los vehiculos habilitados en cada cuenta
        /// Modifica la lista de cuentas del objeto cliente recibido
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oCliente">Cliente - Objeto cliente, debe tener cargados los vehiculos y las cuentas</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void getVehiculosPorCuentas(Conexion oConn, Cliente oCliente)
        {
            if (oCliente.Cuentas == null )
            {
                throw new ArgumentNullException("Cuentas");
            }
            if( oCliente.Vehiculos == null)
            {
                throw new ArgumentNullException("Vehiculos");
            }
            //Generamos el espacio donde registrar en cada cuenta y vehiculo
            foreach (Cuenta item in oCliente.Cuentas)
            {
                if (item.VehiculosHabilitados == null)
                    item.VehiculosHabilitados = new VehiculoHabilitadoL();
            }

            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getVehiculosHabilPorCuenta";
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCliente.NumeroCliente;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                //Buscamos la cuenta y el vehiculo
                string patente = oDR["clv_paten"].ToString();
                int cuenta = (int)oDR["cta_numer"];
                bool habilitado = (oDR["habilitado"].ToString() == "S");
                Vehiculo oVehiculo = oCliente.Vehiculos.FindPatente(patente);
                Cuenta oCuenta = oCliente.Cuentas.FindNumeroCuenta(cuenta);

                if (oVehiculo != null && oCuenta != null)
                {
                    //Agregamos el vehiculo a la cuenta
                    oCuenta.VehiculosHabilitados.Add(new VehiculoHabilitado(oVehiculo, habilitado));
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza las estaciones habilitadas en cada cuenta
        /// Modifica la lista de cuentas del objeto cliente recibido
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="oCliente">Cliente - Objeto cliente, debe tener cargados los vehiculos y las cuentas</param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static void getEstacionesPorCuentas(Conexion oConn, Cliente oCliente)
        {
            if (oCliente.Cuentas == null)
            {
                throw new ArgumentNullException("Cuentas");
            }

            //Generamos el espacio donde registrar en cada cuenta
            foreach (Cuenta item in oCliente.Cuentas)
            {
                if (item.EstacionesHabilitadas == null)
                {
                    item.EstacionesHabilitadas = new CuentaEstacionL();
                }
            }

            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getEstacionesPorCuenta";
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCliente.NumeroCliente;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                //Buscamos la cuenta 
                int cuenta = (int)oDR["Cuenta"];
                bool habilitado = (oDR["Habilitado"].ToString() == "S");
                Cuenta oCuenta = oCliente.Cuentas.FindNumeroCuenta(cuenta);

                if (oCuenta != null)
                {
                    //Agregamos la estacion a la cuenta
                    oCuenta.EstacionesHabilitadas.Add(new CuentaEstacion(new Estacion((byte)oDR["Estacion"], oDR["Nombre"].ToString()), habilitado));
                }
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la Cuenta Prepaga de un cliente determinado
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente del que me interesan conocer la cuenta</param>
        /// <returns>Cuentas prepaga del cliente</returns>
        /// ***********************************************************************************************
        public static Cuenta getCuentaPrepaga(Conexion oConn, int estacion, int numeroCliente)
        {
            Cuenta oCuenta = new Cuenta();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_cuentas_getDatosCuentaPrepaga_Cliente";
            oCmd.Parameters.Add("@Coest", SqlDbType.TinyInt).Value = estacion;
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = numeroCliente;
            oCmd.Parameters.Add("@EsAdmin", SqlDbType.Char, 1).Value = (estacion < ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)?"S":"N";

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oCuenta = CargarCuentaPrepagaCliente(oDR);
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oCuenta;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en el objeto Cuenta
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Cuenta</param>
        /// <returns>Lista con el elemento Cuenta de la base de datos</returns>
        /// ***********************************************************************************************
        private static Cuenta CargarCuentaPrepagaCliente(System.Data.IDataReader oDR)
        {
            TipoCuenta oAuxTipoCuenta = null;
            if (oDR["TipoCuenta"] != DBNull.Value)
            {
                oAuxTipoCuenta = new TipoCuenta((int)oDR["TipoCuenta"], oDR["DescTipoCuenta"].ToString());
            }


            AgrupacionCuenta oAuxAgrupacion = null;
            if (oDR["Agrupacion"] != DBNull.Value)
            {
                oAuxAgrupacion = new AgrupacionCuenta(oAuxTipoCuenta, (byte)oDR["Agrupacion"], oDR["DescAgrupacion"].ToString(), null, null, null, null, null, null, null, null);
            }


            Cuenta oCuenta = new Cuenta((int)oDR["Cuenta"],
                                        oAuxTipoCuenta,
                                        Util.DbValueToNullable<DateTime>(oDR["FechaEgresoCuenta"]),
                                        oAuxAgrupacion,
                                        oDR["cta_descr"].ToString(),
                                        "N",
                                        null);

            // Monto minimo y maximo de recarga
            oCuenta.MontoMinimoRecarga = Convert.ToDecimal(oDR["MinimoRecarga"]);
            oCuenta.MontoMaximoRecarga = Convert.ToDecimal(oDR["MaximoRecarga"]);

            //Estaciones habilitadas
            byte estacion = (byte)oDR["est_codig"];
            CuentaEstacion oCuentaEstacion = new CuentaEstacion();
            oCuentaEstacion.Estacion = new Estacion(estacion, oDR["est_nombr"].ToString());

            if (estacion < ConfiguracionClienteFacturacion.MinimaEstacionAdministrativa)
            {
                oCuentaEstacion.Habilitado = (oDR["cte_habil"].ToString() == "S");
            }
            else
            {
                oCuentaEstacion.Habilitado = true;
            }

            oCuenta.EstacionesHabilitadas = new CuentaEstacionL();
            oCuenta.EstacionesHabilitadas.Add(oCuentaEstacion);

            return oCuenta;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCuenta">Cuenta - Objeto con la informacion de la cuenta a agregar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool addCuenta(Conexion oConn, Cuenta oCuenta)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_Agregar";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCuenta.Cliente.NumeroCliente;
            SqlParameter parCuenta = oCmd.Parameters.Add("@Cuenta", SqlDbType.Int);
            parCuenta.Direction = ParameterDirection.Output;

            oCmd.Parameters.Add("@Tipcu", SqlDbType.Int).Value = oCuenta.TipoCuenta.CodigoTipoCuenta;
            oCmd.Parameters.Add("@Subfp", SqlDbType.Int).Value = oCuenta.Agrupacion.SubTipoCuenta;
            oCmd.Parameters.Add("@Feegr", SqlDbType.DateTime).Value = oCuenta.FechaEgreso;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar,60).Value = oCuenta.Descripcion;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;
            if (parCuenta.Value != DBNull.Value)
            {
                oCuenta.Numero = Convert.ToInt32(parCuenta.Value);
            }

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al insertar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cuenta ya existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCuenta">Cuenta - Objeto con la informacion del cliente a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updCuenta(Conexion oConn, Cuenta oCuenta)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_Guardar";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCuenta.Cliente.NumeroCliente;
            oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oCuenta.Numero;
            oCmd.Parameters.Add("@Tipcu", SqlDbType.Int).Value = oCuenta.TipoCuenta.CodigoTipoCuenta;
            oCmd.Parameters.Add("@Subfp", SqlDbType.Int).Value = oCuenta.Agrupacion.SubTipoCuenta;
            oCmd.Parameters.Add("@Feegr", SqlDbType.DateTime).Value = oCuenta.FechaEgreso;
            oCmd.Parameters.Add("@Descr", SqlDbType.VarChar, 60).Value = oCuenta.Descripcion;

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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cuenta no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Elimina una Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCuenta">Cuenta - Objeto con la informacion de la cuenta a eliminar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool delCuenta(Conexion oConn, Cuenta oCuenta)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_Eliminar";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCuenta.Cliente.NumeroCliente;
            oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oCuenta.Numero;

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al eliminar el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cuenta no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica una Estacion en una Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCuenta">Cuenta - Objeto con la informacion de la cuenta</param>
        /// <param name="oCuentaEstacion">CuentaEstacion - Objeto con la informacion de la estacion a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updCuentaEstacion(Conexion oConn, Cuenta oCuenta, CuentaEstacion oCuentaEstacion)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_GuardarCuentaEst";

            oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oCuenta.Numero;
            oCmd.Parameters.Add("@Estac", SqlDbType.TinyInt).Value = oCuentaEstacion.Estacion.Numero;
            oCmd.Parameters.Add("@Habil", SqlDbType.Char,1).Value = oCuentaEstacion.Habilitado ? "S":"N";

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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cuenta no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un Vehiculo Habilitado en una Cuenta
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCuenta">Cuenta - Objeto con la informacion de la cuenta</param>
        /// <param name="oVehiculo">VehiculoHabilitado - Objeto con la informacion del vehiculo a modificar</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool updCuentaVehiculo(Conexion oConn, Cuenta oCuenta, VehiculoHabilitado oVehiculo)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_GuardarCuentaVeh";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCuenta.Cliente.NumeroCliente;
            oCmd.Parameters.Add("@Cuenta", SqlDbType.Int).Value = oCuenta.Numero;
            oCmd.Parameters.Add("@Paten", SqlDbType.VarChar,8).Value = oVehiculo.Vehiculo.Patente;
            oCmd.Parameters.Add("@Habil", SqlDbType.Char, 1).Value = oVehiculo.Habilitado?"S":"N";

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
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                else if (retval == -102)
                {
                    msg = Traduccion.Traducir("El número de Cuenta no existe");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Saldos de un cliente en particular
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCliente">int - Numero de cliente del que quiero saber sus saldos</param>
        /// <returns>Lista de Saldos del cliente</returns>
        /// ***********************************************************************************************
        public static SaldoPrepagoL getSaldosCliente(Conexion oConn, int numeroCliente, int? zona)
        {
            SaldoPrepagoL oSaldos = new SaldoPrepagoL();
            SqlDataReader oDR;
            
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getSaldosPorCliente";
            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = numeroCliente;
            oCmd.Parameters.Add("@zona", SqlDbType.TinyInt).Value = zona;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oSaldos.Add(CargarSaldoPrepago(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oSaldos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto SaldoPrepago
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de saldos</param>
        /// <returns>elemento SaldoPrepago</returns>
        /// ***********************************************************************************************
        private static SaldoPrepago CargarSaldoPrepago(System.Data.IDataReader oDR)
        {
            SaldoPrepago oSaldo = new SaldoPrepago();

            oSaldo.Zona = new Zona((byte)oDR["zon_zona"], oDR["zon_descr"].ToString());
            if( oDR["pre_saldo"] != DBNull.Value )
            {
                oSaldo.Saldo = (decimal)oDR["pre_saldo"];
            }

            if (oDR["pre_grojo"] != DBNull.Value)
            {
                oSaldo.GiroRojo = (decimal)oDR["pre_grojo"];
            }

            if (oDR["pre_feult"] != DBNull.Value)
            {
                oSaldo.FechaUltimoMovimiento = (DateTime)oDR["pre_feult"];
            }

            return oSaldo;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Actualiza la cuenta prioritaria de cada estacion
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// <param name="oCliente">Cliente - Objeto con la informacion del cliente a recalcular</param>
        /// <returns>true si está OK</returns>
        /// ***********************************************************************************************
        public static bool RecalcularCuentas(Conexion oConn, Cliente oCliente, bool soloBorrar)
        {
            bool ret = false;
            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Cuentas_CuentasPorEstacion";

            oCmd.Parameters.Add("@Client", SqlDbType.Int).Value = oCliente.NumeroCliente;
            oCmd.Parameters.Add("@SoloBorrar", SqlDbType.Char, 1).Value = soloBorrar ? "S" : "N";

            SqlParameter parRetVal = oCmd.Parameters.Add("@RetVal", SqlDbType.Int);
            parRetVal.Direction = ParameterDirection.ReturnValue;

            oCmd.ExecuteNonQuery();
            int retval = (int)parRetVal.Value;

            // Cerramos el objeto
            oCmd = null;

            // Revisamos el retorno del SP. 
            if (retval != 0)
            {
                string msg = Traduccion.Traducir("Error al recalcular el registro ") + retval.ToString();
                if (retval == -101)
                {
                    msg = Traduccion.Traducir("Hay campos que no pueden estar vacíos");
                }
                throw new ErrorSPException(msg);
            }
            else
            {
                ret = true;
            }
            return ret;
        }

        #endregion

        #region TIPODOCUMENTO: Clase de Datos de Tipos de Documento
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de Documentos definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoColor">int - Codigo de Tipo de Documento a filtrar</param>
        /// <returns>Lista de Tipos de Documento</returns>
        /// ***********************************************************************************************
        public static TipoDocumentoL getTiposDocumento(Conexion oConn, int? codigoTipoDoc)
        {
            TipoDocumentoL oTipoDocs = new TipoDocumentoL();
            SqlDataReader oDR;


            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getTiposDocumentos";
            oCmd.Parameters.Add("@Codigo", SqlDbType.Int).Value = codigoTipoDoc;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTipoDocs.Add(CargarTipoDocumentos(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTipoDocs;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Tipos de Documentos
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipos de Documento</param>
        /// <returns>Lista con el elemento Tipos de Documento de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoDocumento CargarTipoDocumentos(System.Data.IDataReader oDR)
        {
            TipoDocumento oTipoDoc = new TipoDocumento((int)oDR["tid_codig"], oDR["tid_descr"].ToString());
            return oTipoDoc;
        }
        
        #endregion

        #region TIPOIVA: Clase de Datos de Tipos de IVA
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Tipos de IVA definidos
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="codigoColor">int - Codigo  a filtrar</param>
        /// <returns>Lista de Tipos de IVA</returns>
        /// ***********************************************************************************************
        public static TipoIVAL getTiposIVA(Conexion oConn, int? codigo)
        {
            TipoIVAL oTipos = new TipoIVAL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getTiposIVA";
            oCmd.Parameters.Add("@Codigo", SqlDbType.Int).Value = codigo;

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oTipos.Add(CargarTipoIVA(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oTipos;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Tipo de IVA
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Tipos de IVA</param>
        /// <returns>Lista con el elemento Tipos de IVA de la base de datos</returns>
        /// ***********************************************************************************************
        private static TipoIVA CargarTipoIVA(System.Data.IDataReader oDR)
        {
            TipoIVA oTipo = new TipoIVA((byte)oDR["tip_codig"],
                                                       oDR["tip_descr"].ToString());
            if( oDR["tip_tipfa"] != DBNull.Value )
            {
                oTipo.TipoFactura = new TipoFactura(oDR["tip_tipfa"].ToString(), oDR["tif_descr"].ToString());
            }

            return oTipo;
        }
        
        #endregion

        #region PROVINCIAS: Clase de Datos de Provincias
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de Provincias
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Provincias</returns>
        /// ***********************************************************************************************
        public static ProvinciaL getProvincias(Conexion oConn)
        {
            ProvinciaL oProvincias = new ProvinciaL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_getProvincias";

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oProvincias.Add(CargarProvincia(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oProvincias;
        }

        public static EstadoL getEstados(Conexion oConn)
        {
            EstadoL oEstados = new EstadoL();
            SqlDataReader oDR;

            // Creamos, cargamos y ejecutamos el comando
            SqlCommand oCmd = new SqlCommand();
            oCmd.Connection = oConn.conection;
            oCmd.Transaction = oConn.transaction;

            oCmd.CommandType = System.Data.CommandType.StoredProcedure;
            oCmd.CommandText = "Facturacion.usp_Clientes_GetEstados";

            Estado oEstado = new Estado("", Traduccion.Traducir("Desconocido"),"","");
            oEstados.Add(oEstado);

            oDR = oCmd.ExecuteReader();
            while (oDR.Read())
            {
                oEstados.Add(CargarEstados(oDR));
            }

            // Cerramos el objeto
            oCmd = null;
            oDR.Close();
            return oEstados;
        }

        private static Estado CargarEstados(SqlDataReader oDR)
        {
            Estado oEstado = new Estado(oDR["ufe_codig"].ToString(), oDR["ufe_estado"].ToString(), oDR["ufe_capital"].ToString(), oDR["ufe_region"].ToString());
            return oEstado;
        }



        
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en un objeto Provincia
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Provincias</param>
        /// <returns>Objecto Provincia</returns>
        /// ***********************************************************************************************
        private static Provincia CargarProvincia(System.Data.IDataReader oDR)
        {
            Provincia oProvincia = new Provincia((int)oDR["pro_codig"], oDR["pro_descr"].ToString());
            return oProvincia;
        }

        #endregion
    }
}
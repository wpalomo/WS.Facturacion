using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Telectronica.Utilitarios;
using Telectronica.Errores;
using System.IO;

namespace Telectronica.Peaje
{
    ///****************************************************************************************************
    /// <summary>
    /// Clase para administracion de las Categorias definidas en el sistema
    /// </summary>
    ///****************************************************************************************************
    public static class CategoriaDt
    {
        #region CATEGORIA: Clase de Datos de Categorias manuales definidas

        /// ***********************************************************************************************
        /// <summary>
        /// Método encargado de cargar los parameters para ejecutar un Add o un Upd
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="tipoCupon"></param>
        /// ***********************************************************************************************
        private static void AddParametersToUpdOrAdd(SqlCommand cmd, CategoriaManual categoriaManual)
        {
            cmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoriaManual.Categoria;
            cmd.Parameters.Add("@Descr", SqlDbType.VarChar, 30).Value = categoriaManual.Descripcion;
            cmd.Parameters.Add("@Imagen", SqlDbType.VarChar, 30).Value = categoriaManual.Imagen;
            cmd.Parameters.Add("@Equiv", SqlDbType.Real).Value = categoriaManual.Equivalente;
            cmd.Parameters.Add("@DescLarga", SqlDbType.VarChar, 50).Value = categoriaManual.DescripcionLarga;
            cmd.Parameters.Add("@EquivCGMP", SqlDbType.TinyInt).Value = categoriaManual.EquivalenteCGMP;
            cmd.Parameters.Add("@EquivANTT", SqlDbType.TinyInt).Value = categoriaManual.EquivalenteANTT;
            cmd.Parameters.Add("@EjesAdicANTT", SqlDbType.TinyInt).Value = categoriaManual.EjesAdicionalesANTT;
            cmd.Parameters.Add("@PrincipalCgmp", SqlDbType.Char,1).Value = categoriaManual.PrincipalCgmp;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="oConn"></param>
        /// <param name="numeroCategoria"></param>
        /// <param name="incluirCategoriaCero"></param>
        /// <param name="pathImagenes"></param>
        /// <returns></returns>
        /// ***********************************************************************************************
        public static DataSet getRptCategorias(Conexion oConn, int? numeroCategoria, bool incluirCategoriaCero, string pathImagenes)
        {
            DataSet dsCategorias = new DataSet();
            dsCategorias.DataSetName = "RptPeaje_CategoriasDetalleDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_getCategorias";
                oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = numeroCategoria;
                oCmd.Parameters.Add("incluirCatCero", SqlDbType.Char, 1).Value = (incluirCategoriaCero == true ? "S" : "N");

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsCategorias, "Categorias");

                // Creamos una nueva columna para almacenar los datos de la imagen leida:
                dsCategorias.Tables[0].Columns.Add("trq_image", typeof(System.Byte[]));

                for (int i = 0; i < dsCategorias.Tables[0].Rows.Count - 1; i++)
                {
                    // Ruta de la imagen
                    string pathImage = string.Concat(pathImagenes, dsCategorias.Tables[0].Rows[i]["cat_codig"]);

                    // Abrimos la imagen, si existe...
                    if (File.Exists(pathImage))
                    {
                        FileStream fs = new FileStream(pathImage, FileMode.Open, FileAccess.Read);
                        BinaryReader br = new BinaryReader(fs);

                        // Cargamos los datos de la imagen en el array
                        byte[] arr = new byte[fs.Length];
                        br.Read(arr, 0, (int)fs.Length);
                        br.Close();
                        fs.Close();

                        // Guardamos los datos leidos en el campo correspondiente....
                        dsCategorias.Tables[0].Rows[i]["trq_image"] = arr;
                    }
                    else
                    {
                        dsCategorias.Tables[0].Rows[i]["trq_image"] = null;
                    }
                }

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsCategorias;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de categorias manuales definidas 
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="numeroCategoria">int - Numero de categoria por la cual filtrar la busqueda</param>
        /// <param name="incluirCategoriaCero">bool - Permite filtrar, trayendo o no la categoria cero</param>
        /// <returns>Lista de Categorias manuales definidas</returns>
        /// ***********************************************************************************************
        public static CategoriaManualL getCategorias(Conexion oConn, int? numeroCategoria, bool incluirCategoriaCero)
        {
            CategoriaManualL oCategorias = new CategoriaManualL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_getCategorias";
                oCmd.Parameters.Add("Categ", SqlDbType.TinyInt).Value = numeroCategoria;
                oCmd.Parameters.Add("incluirCatCero", SqlDbType.Char, 1).Value = (incluirCategoriaCero == true ? "S" : "N");

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCategorias.Add(CargarCategoria(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCategorias;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Categorias manuales
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Categorias manuales</param>
        /// <returns>Lista con los elementos Categoria de la base de datos</returns>
        /// ***********************************************************************************************
        private static CategoriaManual CargarCategoria(System.Data.IDataReader oDR)
        {
            CategoriaManual oCategoria = new CategoriaManual();
            oCategoria.Categoria = (byte)oDR["cat_tarif"];
            oCategoria.Descripcion = oDR["cat_descr"].ToString();
            oCategoria.Imagen = oDR["cat_codig"].ToString();
            if (oDR["trq_equiv"] != DBNull.Value)
            {
                oCategoria.Equivalente = (float)oDR["trq_equiv"];
            }
            oCategoria.DescripcionLarga = oDR["cat_descl"].ToString();
            if (oDR["cat_cgmp"] != DBNull.Value)
            {
                oCategoria.EquivalenteCGMP = (byte)oDR["cat_cgmp"];
            }
            if (oDR["cat_adicartesp"] != DBNull.Value)
            {
                oCategoria.EjesAdicionalesANTT = (byte)oDR["cat_adicartesp"];
            }

            if (oDR["cat_artesp"] != DBNull.Value)
            {
                oCategoria.EquivalenteANTT = (byte)oDR["cat_artesp"];
            }

            if (oDR["cat_defcgmp"] != DBNull.Value)
            {
                oCategoria.PrincipalCgmp = oDR["cat_defcgmp"].ToString();
            }
            else
            {
                oCategoria.PrincipalCgmp = "N";
            }
                   
            return oCategoria;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Agrega una categoria manual en la base de datos
        /// </summary>
        /// <param name="oCategoriaManual">CategoriaManual - Objeto con la informacion de la categoria a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCategoriaManual(CategoriaManual oCategoriaManual, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_addCategoria";

                AddParametersToUpdOrAdd(oCmd, oCategoriaManual);

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
                        msg = Traduccion.Traducir("Este número de categoría ya existe");
                    }
                    else if (retval == (int)EjecucionSP.enmErrorSP.enmBAJA_LOGICA)
                    {
                        msg = Traduccion.Traducir("Este número de categoría fue eliminado");
                    }
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
        /// Modifica una Categoria Manual en la base de datos
        /// </summary>
        /// <param name="oCategoriaManual">CategoriaManual - Objeto con la informacion de la Categoria a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updCategoriaManual(CategoriaManual oCategoriaManual, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_updCategoria";

                AddParametersToUpdOrAdd(oCmd, oCategoriaManual);

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
                        msg = Traduccion.Traducir("No existe el registro de la categoría");
                    }
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
        /// Elimina una Categoria Manual en la base de datos
        /// </summary>
        /// <param name="oCategoriaManual">CategoriaManual - Objeto que contiene la categoria a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCategoriaManual(CategoriaManual oCategoriaManual, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_delCategoria";

                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oCategoriaManual.Categoria;

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
                        msg = Traduccion.Traducir("No existe el registro de la categoría");
                    }
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
        /// Elimina todas las habilitaciones de Categorias por forma de pago para una categoria dada
        /// </summary>
        /// <param name="oCategoriaManual">CategoriaManual - Objeto para saber de que categoria hay 
        ///                                que eliminar las habilitaciones por forma de pago</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCategoriasFormaPago(CategoriaManual oCategoriaManual, Conexion oConn)
        {
            // Creamos y cargamos un objeto CategoriaFormaPago con la forma de pago en null para que 
            // el otro metodo elimine todas las habilitaciones por forma de pago de esta categoria
            CategoriaFormaPago oCatFP = new CategoriaFormaPago();
            oCatFP.Categoria = oCategoriaManual.Categoria;
            oCatFP.FormaPago = null;

            delCategoriaFormaPago(oCatFP, oConn);
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Elimina la habilitacion de una Forma de Pago para la Categoria Manual dada
        /// </summary>
        /// <param name="oCategoriaFormaPago">CategoriaFormaPago - Objeto que contiene la forma de pago a eliminar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void delCategoriaFormaPago(CategoriaFormaPago oCategoriaFormaPago, Conexion oConn)
        {
            try
            {
                string sTipo = null;
                string sSubtipo = null;
                
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_delCategoriaFormaPago";

                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oCategoriaFormaPago.Categoria;

                if (oCategoriaFormaPago.FormaPago != null)
                {
                    sTipo = oCategoriaFormaPago.FormaPago.Tipo;
                    sSubtipo = oCategoriaFormaPago.FormaPago.SubTipo;
                }

                oCmd.Parameters.Add("@Tipo", SqlDbType.Char).Value = sTipo;
                oCmd.Parameters.Add("@SubTipo", SqlDbType.Char).Value = sSubtipo;

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
        /// Inserta la habilitacion de una Forma de Pago para la Categoria Manual dada
        /// </summary>
        /// <param name="oCategoriaFormaPago">CategoriaFormaPago - Objeto que contiene la forma de pago a insertar</param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void addCategoriaFormaPago(CategoriaFormaPago oCategoriaFormaPago, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_addCategoriaFormaPago";

                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = oCategoriaFormaPago.Categoria;
                oCmd.Parameters.Add("@Tipo", SqlDbType.Char).Value = oCategoriaFormaPago.FormaPago.Tipo;
                oCmd.Parameters.Add("@SubTipo", SqlDbType.Char).Value = oCategoriaFormaPago.FormaPago.SubTipo;

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
        
        #region CATEGORIADAC: Clase de Datos de Categorias del DAC (categorias automaticas)
        
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de categorias definidas para el DAC
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <returns>Lista de Categorias definidas para la deteccion del DAC</returns>
        /// ***********************************************************************************************
        public static CategoriaDACL getCategoriasDAC(Conexion oConn)
        {
            CategoriaDACL oCategoriasDAC = new CategoriaDACL();
            try
            {
                SqlDataReader oDR;
                
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CategoriasDAC_getCategoriasDAC";

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCategoriasDAC.Add(CargarCategoriaDAC(oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCategoriasDAC;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la la lista de Categorias del DAC
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de Categorias del DAC</param>
        /// <returns>Lista con el elemento CategoriaDAC de la base de datos</returns>
        /// ***********************************************************************************************
        private static CategoriaDAC CargarCategoriaDAC(System.Data.IDataReader oDR)
        {
            CategoriaDAC oCategoriaDAC = new CategoriaDAC();
            oCategoriaDAC.CantidadEjes = (byte)oDR["cat_nejes"];
            oCategoriaDAC.RuedaDual = oDR["cat_rudob"].ToString();
            oCategoriaDAC.Altura = oDR["cat_altur"].ToString();
            oCategoriaDAC.Categoria = Util.DbValueToNullable<byte>(oDR["cat_tarif"]);
            oCategoriaDAC.Moto = oDR["cat_motos"].ToString();
            oCategoriaDAC.CategoriaEspecial = (oDR["cat_especial"] is DBNull) ? "N" : oDR["cat_especial"].ToString();

            return oCategoriaDAC;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Modifica un registro de definicion de la categoria del DAC
        /// </summary>
        /// <param name="oCategoriaDAC">CategoriaDAC - Objeto con la informacion de la categoria automatica </param>
        /// <param name="oConn">Conexion - Objeto de conexion con la base de datos</param>
        /// ***********************************************************************************************
        public static void updCategoriaDAC(CategoriaDAC oCategoriaDAC, Conexion oConn)
        {
            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_CategoriasDAC_updCategoriaDAC";

                oCmd.Parameters.Add("@Ejes", SqlDbType.TinyInt).Value = oCategoriaDAC.CantidadEjes;
                oCmd.Parameters.Add("@Duales", SqlDbType.Char, 1).Value = oCategoriaDAC.RuedaDual;
                oCmd.Parameters.Add("@Altura", SqlDbType.Char, 1).Value = oCategoriaDAC.Altura;
                oCmd.Parameters.Add("@Categoria", SqlDbType.TinyInt).Value = oCategoriaDAC.Categoria;
                oCmd.Parameters.Add("@Moto", SqlDbType.Char, 1).Value = oCategoriaDAC.Moto;
                oCmd.Parameters.Add("@Especial", SqlDbType.Char, 1).Value = oCategoriaDAC.CategoriaEspecial;

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
                        msg = Traduccion.Traducir("Se modificaron más registros de lo esperado");
                    }
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
        
        #region CATEGORIAFORMAPAGO: Clase de Datos de Categorias habilitadas para cada Forma de Pago
        
        public static DataSet getRptCategoriasFormaPago(Conexion oConn)
        {
            DataSet dsTransitos = new DataSet();
            dsTransitos.DataSetName = "RptPeaje_CategoriasDS";

            try
            {
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_rptCategoriasFormaPago";

                SqlDataAdapter oDA = new SqlDataAdapter(oCmd);
                oDA.Fill(dsTransitos, "Categorias");

                // Cerramos el objeto
                oCmd = null;
                oDA.Dispose();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsTransitos;
        }

        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve la lista de forma de pago (con su estado de habilitacion) para la categoria indicada por parametro
        /// </summary>
        /// <param name="oConn">Conexion - Objeto de conexion a la base de datos correspondiente</param>
        /// <param name="categoria">byte - Categoria de la que se desean obtener las formas de pago habilitadas</param>
        /// <returns>Lista de formas de pago (habilitadas o no) para una categoria dada</returns>
        /// ***********************************************************************************************
        public static CategoriaFormaPagoL getCategoriasFormaPago(Conexion oConn, byte categoria, bool? cobraVia)
        {
            CategoriaFormaPagoL oCategoriaFormaPagoL = new CategoriaFormaPagoL();

            try
            {
                SqlDataReader oDR;
                string sCobraVia = null;
                
                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Peaje.usp_Categorias_getCategoriasFormaPago";
                oCmd.Parameters.Add("@Categ", SqlDbType.TinyInt).Value = categoria;

                if (cobraVia != null)
                {
                    sCobraVia = (bool)cobraVia ? "S" : "N";
                }

                oCmd.Parameters.Add("@CobraVia", SqlDbType.Char, 1).Value = sCobraVia;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    oCategoriaFormaPagoL.Add(CargarCategoriaFormaPago(categoria, oDR));
                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oCategoriaFormaPagoL;
        }
        
        /// ***********************************************************************************************
        /// <summary>
        /// Carga un elemento DataReader en la lista de Formas de pago habilitadas por categoria
        /// </summary>
        /// <param name="oDR">System.Data.IDataReader - Objeto DataReader de la tabla de forma de pago por categoria</param>
        /// <returns>Lista con el elemento CategoriaFormaPago de la base de datos</returns>
        /// ***********************************************************************************************
        private static CategoriaFormaPago CargarCategoriaFormaPago(byte categoria, System.Data.IDataReader oDR)
        {
            // Objeto Forma de Pago que se le anexa al objeto CategoriaFormaPago
            FormaPago oFormaPago = new FormaPago(oDR["for_tipop"].ToString(),
                                                 oDR["for_tipbo"].ToString(),
                                                 oDR["for_descr"].ToString(),
                                                 oDR["for_corto"].ToString(),
                                                 oDR["for_inici"].ToString(),
                                                 oDR["for_autom"].ToString(),
                                                 oDR["for_cobra"].ToString());

            //Objeto CategoriaFormaPago a retornar
            CategoriaFormaPago oCategoriaFormaPago = new CategoriaFormaPago();

            oCategoriaFormaPago.Categoria = categoria;
            oCategoriaFormaPago.FormaPago = oFormaPago;
            oCategoriaFormaPago.Habilitada = (oDR["for_categ"] != DBNull.Value);
            
            return oCategoriaFormaPago;
        }
        
        #endregion
    }
}

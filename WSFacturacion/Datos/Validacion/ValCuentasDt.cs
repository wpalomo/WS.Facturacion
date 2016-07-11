using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using System.Data.SqlClient;
using System.Data;
using Telectronica.Facturacion;

namespace Telectronica.Validacion
{
    public class ValCuentasDt
    {
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de la cuenta por patente
        /// </summary>        
        /// ***********************************************************************************************
        public static VehiculoL GetDatosCuentaPorPatente(Conexion oConn, string patente, int estacion, string sTipop)
        {
            VehiculoL oVehiculos = new VehiculoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Cuentas_GetDatosCuentaPorPatente";
                oCmd.Parameters.Add("@patente", SqlDbType.VarChar, 8).Value = patente;
                oCmd.Parameters.Add("@nuestac", SqlDbType.TinyInt).Value = estacion;
                oCmd.Parameters.Add("@tipop", SqlDbType.VarChar, 1).Value = sTipop;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    VehiculoMarca marca = null;
                    if (oDR["mar_codig"] != DBNull.Value)
                        marca = new VehiculoMarca {Codigo = Convert.ToInt16(oDR["mar_codig"]), Descripcion = Convert.ToString(oDR["mar_descr"])};

                    VehiculoModelo modelo = null;
                    if (oDR["mod_codig"] != DBNull.Value)
                        modelo = new VehiculoModelo { Codigo = Convert.ToInt16(oDR["mod_codig"]), Descripcion = Convert.ToString(oDR["mod_descr"]), Marca = marca };

                    VehiculoColor color = null;
                    if (oDR["col_codig"] != DBNull.Value)
                        color = new VehiculoColor { Codigo = Convert.ToInt16(oDR["col_codig"]), Descripcion = Convert.ToString(oDR["col_descr"]) };

                    Cliente cliente = null;
                    if (oDR["cli_numcl"] != DBNull.Value)
                        cliente = new Cliente { NumeroCliente = Convert.ToInt32(oDR["cli_numcl"]), RazonSocial = Convert.ToString(oDR["cli_nombr"]) };

                    TipoCuenta tipoCuenta = null;
                    if (oDR["cta_tipcu"] != DBNull.Value)
                        tipoCuenta = new TipoCuenta { CodigoTipoCuenta = Convert.ToInt32(oDR["cta_tipcu"]), Descripcion = Convert.ToString(oDR["tic_descr"]), TipoBoleto = oDR["tic_tipbo"].ToString() };

                    TarifaDiferenciada tipoTarifa = null;
                    if (oDR["ctg_titar"] != DBNull.Value)
                        tipoTarifa = new TarifaDiferenciada { CodigoTarifa = Convert.ToInt16(oDR["ctg_titar"]) };

                    AgrupacionCuenta agrupacion = null;
                    if (oDR["cta_subfp"] != DBNull.Value)
                        agrupacion = new AgrupacionCuenta { SubTipoCuenta = Convert.ToInt16(oDR["cta_subfp"]), TipoTarifa = tipoTarifa };

                    Cuenta cuenta = null;
                    if (oDR["cta_numer"] != DBNull.Value)
                        cuenta = new Cuenta { Numero = Convert.ToInt32(oDR["cta_numer"]), Descripcion = Convert.ToString(oDR["cta_descr"]), TipoCuenta =  tipoCuenta, Agrupacion = agrupacion };

                    Tag tag = null;
                    if (oDR["tag_numer"] != DBNull.Value)
                        tag = new Tag { NumeroTag = Convert.ToString(oDR["tag_numer"]) };

                    Chip chip = null;
                    if (oDR["chi_numer"] != DBNull.Value)
                        chip = new Chip { Dispositivo = Convert.ToString(oDR["chi_numer"]) };

                    OSAsTag tagOSA = null;
                    if (oDR["tag_ntag"] != DBNull.Value)
                        tagOSA = new OSAsTag { NumeroTag = Convert.ToString(oDR["tag_ntag"]), EmisorTag = Convert.ToString(oDR["tag_emitg"]), Administradora = Convert.ToInt32(oDR["tag_admtag"]), TipoDispositivo = oDR["tag_tipop"].ToString(), Categoria = Convert.ToByte(oDR["CategoriaTag"]), EstadoDescripcion = Convert.ToString(oDR["EstadoDescripcion"]), CategoriaDescripcion = Convert.ToString(oDR["CategoriaTagDescripcion"]), FormaPago = oDR["tic_tipbo"].ToString(), MedioPago = oDR["tag_tipop"].ToString() };

                    if (oDR["ForpagDescr"] != DBNull.Value)
                        tagOSA.FormaPagoDescr = Convert.ToString(oDR["ForpagDescr"]);
                        
                    CategoriaManual categoria = new CategoriaManual { Categoria = Convert.ToByte(oDR["clv_categ"]) };

                    oVehiculos.Add( new Vehiculo{
                        Patente = Convert.ToString(oDR["clv_paten"]),
                        Marca = marca,
                        Modelo = modelo,
                        Color = color, 
                        Cliente = cliente,
                        Cuenta = cuenta,
                        Tag = tag,
                        Chip = chip,
                        TagOSA = tagOSA,
                        Categoria = categoria,
                        ListaNegra = Convert.ToString(oDR["ListaNegra"])
                    });

                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVehiculos;
        }
        /// ***********************************************************************************************
        /// <summary>
        /// Devuelve los datos de la cuenta por tag
        /// </summary>        
        /// ***********************************************************************************************
        public static VehiculoL GetDatosCuentaPorTag(Conexion oConn, string emisor, string ntag, int estacion)
        {
            VehiculoL oVehiculos = new VehiculoL();
            try
            {
                SqlDataReader oDR;

                // Creamos, cargamos y ejecutamos el comando
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = oConn.conection;
                oCmd.Transaction = oConn.transaction;

                oCmd.CommandType = System.Data.CommandType.StoredProcedure;
                oCmd.CommandText = "Validacion.usp_Cuentas_GetDatosCuentaPorTag";
                oCmd.Parameters.Add("@emitg", SqlDbType.VarChar, 5).Value = emisor==""?null:emisor;
                oCmd.Parameters.Add("@ntag", SqlDbType.VarChar, 10).Value = ntag;
                oCmd.Parameters.Add("@nuestac", SqlDbType.TinyInt).Value = estacion;

                oDR = oCmd.ExecuteReader();
                while (oDR.Read())
                {
                    VehiculoMarca marca = null;
                    if (oDR["mar_codig"] != DBNull.Value)
                        marca = new VehiculoMarca { Codigo = Convert.ToInt16(oDR["mar_codig"]), Descripcion = Convert.ToString(oDR["mar_descr"]) };

                    VehiculoModelo modelo = null;
                    if (oDR["mod_codig"] != DBNull.Value)
                        modelo = new VehiculoModelo { Codigo = Convert.ToInt16(oDR["mod_codig"]), Descripcion = Convert.ToString(oDR["mod_descr"]), Marca = marca };

                    VehiculoColor color = null;
                    if (oDR["col_codig"] != DBNull.Value)
                        color = new VehiculoColor { Codigo = Convert.ToInt16(oDR["col_codig"]), Descripcion = Convert.ToString(oDR["col_descr"]) };

                    Cliente cliente = null;
                    if (oDR["cli_numcl"] != DBNull.Value)
                        cliente = new Cliente { NumeroCliente = Convert.ToInt32(oDR["cli_numcl"]), RazonSocial = Convert.ToString(oDR["cli_nombr"]) };

                    TipoCuenta tipoCuenta = null;
                    if (oDR["cta_tipcu"] != DBNull.Value)
                        tipoCuenta = new TipoCuenta { CodigoTipoCuenta = Convert.ToInt32(oDR["cta_tipcu"]), Descripcion = Convert.ToString(oDR["tic_descr"]), TipoBoleto = oDR["tic_tipbo"].ToString() };

                    TarifaDiferenciada tipoTarifa = null;
                    if (oDR["ctg_titar"] != DBNull.Value)
                        tipoTarifa = new TarifaDiferenciada { CodigoTarifa = Convert.ToInt16(oDR["ctg_titar"]) };

                    AgrupacionCuenta agrupacion = null;
                    if (oDR["cta_subfp"] != DBNull.Value)
                        agrupacion = new AgrupacionCuenta { SubTipoCuenta = Convert.ToInt16(oDR["cta_subfp"]), TipoTarifa = tipoTarifa };

                    Cuenta cuenta = null;
                    if (oDR["cta_numer"] != DBNull.Value)
                        cuenta = new Cuenta { Numero = Convert.ToInt32(oDR["cta_numer"]), Descripcion = Convert.ToString(oDR["cta_descr"]), TipoCuenta = tipoCuenta, Agrupacion = agrupacion };

                    Tag tag = null;
                    if (oDR["tag_numer"] != DBNull.Value)
                        tag = new Tag { NumeroTag = Convert.ToString(oDR["tag_numer"]) };

                    Chip chip = null;
                    if (oDR["chi_numer"] != DBNull.Value)
                        chip = new Chip { Dispositivo = Convert.ToString(oDR["chi_numer"]) };

                    OSAsTag tagOsa = null;
                    if (oDR["tag_ntag"] != DBNull.Value)
                        tagOsa = new OSAsTag { NumeroTag = Convert.ToString(oDR["tag_ntag"]), EmisorTag = Convert.ToString(oDR["tag_emitg"]), Administradora = Convert.ToInt32(oDR["tag_admtag"]), TipoDispositivo = oDR["tag_tipop"].ToString(), Categoria = Convert.ToByte(oDR["CategoriaTag"]), EstadoDescripcion = Convert.ToString(oDR["EstadoDescripcion"]), CategoriaDescripcion = Convert.ToString(oDR["CategoriaTagDescripcion"]), FormaPago = oDR["tic_tipbo"].ToString(), MedioPago = oDR["tag_tipop"].ToString() };

                    if (oDR["ForpagDescr"] != DBNull.Value)
                        tagOsa.FormaPagoDescr = Convert.ToString(oDR["ForpagDescr"]);

                    CategoriaManual categoria = new CategoriaManual { Categoria = Convert.ToByte(oDR["clv_categ"]) };

                    oVehiculos.Add(new Vehiculo
                    {
                        Patente = Convert.ToString(oDR["clv_paten"]),
                        Marca = marca,
                        Modelo = modelo,
                        Color = color,
                        Cliente = cliente,
                        Cuenta = cuenta,
                        Tag = tag,
                        Chip = chip,
                        TagOSA = tagOsa,
                        Categoria = categoria,
                        ListaNegra = Convert.ToString(oDR["ListaNegra"])
                    });

                }

                // Cerramos el objeto
                oCmd = null;
                oDR.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return oVehiculos;
        }
    }
}

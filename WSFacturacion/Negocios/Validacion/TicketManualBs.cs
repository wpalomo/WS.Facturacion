using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telectronica.Peaje;
using Telectronica.Errores;
using Telectronica.Utilitarios;

namespace Telectronica.Validacion
{
    public class TicketManualBs
    {
        public static TicketManualL GetTicket(int tckInicial, int tckFinal, int? categoria, string tipoComprobante)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    //sin transaccion
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    return TicketManualDt.getTicketManual(conn, tckInicial, tckFinal, categoria, tipoComprobante);

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool TicketManualYaUtilizado(int tckInicial, int tckFinal, int? codigoRegistro, int? categoria, string tipoComprobante, DateTime fecha )
        {
            bool tckUtilizado = false;
            TicketManualL tickets = GetTicket( tckInicial, tckFinal, categoria, tipoComprobante);
            TicketManual ticket = null;
            //TODO solo tomar los tickets si son de hasta 3 dias antes de la fecha del ticket buscado
            foreach (TicketManual item in tickets)
            {
                if (item.Fecha > fecha.AddDays(-3)
                    && (codigoRegistro == null || codigoRegistro != ticket.CodigoRegistro))
                {
                    ticket = item;
                    tckUtilizado = true;
                    break;
                }
            }
            /*
            if(tickets.Count > 0)
                ticket = tickets.FirstOrDefault();
            if (ticket != null)
            {
                if (codigoRegistro == null)
                    tckUtilizado = true;
                else if (codigoRegistro == ticket.CodigoRegistro)
                    tckUtilizado = false;
                else
                    tckUtilizado = true;                
            }*/
            return tckUtilizado;
        }

        ///****************************************************************************************************<summary>
        /// Descripcion a grabar con la informacion del registro afectado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaDescripcion(string puntoVenta, long numeroTicket, int cantidad, CategoriaManual categoria)
        {
            var sb = new StringBuilder();

            AuditoriaBs.AppendCampo(sb, "Cat:", categoria.Descripcion);
            AuditoriaBs.AppendCampo(sb, "P.V.:", puntoVenta.ToString());
            AuditoriaBs.AppendCampo(sb, "Tk.Inic.:", numeroTicket.ToString());
            AuditoriaBs.AppendCampo(sb, "Cantidad:", cantidad.ToString());

            return sb.ToString();
        }


        //Retorna mensaje de error para las nuevas violaciones a via abierta
        private static string getStringError(int causa)
        {
            switch(causa) 
            {
                case -1: return "Hay más de un bloque en el período solicitado";     
                //TODO realizar este bloque en  c#
                //Case 0
                //   If MsgBox("No existe un bloque para el horario solicitado, confirma el ingreso?", vbQuestion + vbOKCancel, "Atención") = vbOK Then
                //       bConfirmado = True
                //       bRepetir = True
                //   End If 
                case 2: return "La vía estaba abierta con otro parte";        
                case 3: return "La fecha inicial o fecha final no corresponde a una jornada contable";        
                case 4: return "Ya existe un Tránsito en la misma Hora y Vía que uno de los que se intentó registrar";       
                case 5: return "Error en el SP de Registro de Tickets Manuales";
                //Case 6: return "El parte debe liquidarse antes de registrar nuevos tickets manuales"         
                case 7:  return "El bloque pertenece a otro parte";         
                case 8:  return "Existe solapamiento entre el inicio y fin del turno y del período";             
                case 9:  return "Existe solapamiento entre el inicio y fin del turno abierto actualmente en la vía y del período";             
                case 10: return "El parte del turno abierto actualmente en la vía es diferente";            
                case 11: return "El número de ticket ya existe";
                case 12: return "Debe existir un ticket posterior en el mismo punto de venta";
                default: return "Error " + causa + " al registrar los tickets manuales "; 
            }
        }

        public static void SetNuevosTicketsManuales(ParteValidacion parte, Estacion estacion, Anomalia anomalia, ViaDefinicion via, int numeroTicket, int cantidad, CategoriaManual categoria, DateTime fechaIni, DateTime fechaFin, string tipoComprobante, string observInterna,string vista, CodigoValidacion codigoAceptacion)
        {
            int ticketFinal;

            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), true);

                    // Si la cantidad es uno, el numero inicial y final es el mismo
                    ticketFinal = numeroTicket + (cantidad == 1 ? 0 : cantidad);


                    if (TicketManualBs.TicketManualYaUtilizado(numeroTicket, ticketFinal, null, Convert.ToInt32(categoria.Categoria), tipoComprobante, fechaIni))
                        throw new ErrorSPException("Hay números de tickets manuales que ya habían sido utilizados");

                    //Verifica que el punto de venta se pueda usar para esta estacion, categoria, via o tipo de comprobante
                    //if (!TicketManualBs.esPuntoVentaCompatible(conn, estacion, categoria, via, tipoComprobante, puntoVenta))
                      //  throw new ErrorSPException("No es un punto de venta compatible");

                    int causaError = EventoValidacionDt.getBloquePorFechaParte(conn, estacion, via, fechaIni, fechaFin, parte);

                    switch (causaError)
                    {
                        case 1:
                            // Cae dentro de un bloque del mismo parte
                            break;
                        case 0:
                            //Cae fuera de cualquier bloque
                            // TODO se elimino logica de confimado
                            //if (!confirmado)
                            //throw new ErrorSPException(getStringError(causaError));
                            break;
                        case 2:
                            //El parte del turno is diferente
                            throw new ErrorSPException(getStringError(causaError));
                        default:
                            throw new ErrorSPException(getStringError(causaError));
                    }

                    if (!JornadaBs.esFechaDeJornada(conn, fechaIni, fechaFin, parte.Jornada, estacion))
                        throw new ErrorSPException(getStringError(3));

                    int returnTickManuales = TicketManualDt.SetNuevosTicketsManuales(conn, parte, estacion, anomalia, via, numeroTicket, cantidad, categoria, fechaIni, fechaFin, tipoComprobante,observInterna, vista, ConexionBs.getUsuario(), codigoAceptacion);
                    switch (returnTickManuales)
                    {
                        case -100:
                            throw new ErrorSPException("Error de Parametrización");
                        case -101:
                            throw new ErrorSPException("El intervalo de tiempo no es válido para la cantidad de TM");
                        case -102:
                            throw new ErrorSPException("El parte no existe");
                        case -105:
                            throw new ErrorSPException("Ya existe el ticket");
                        case -104:
                            throw new ErrorSPException("El parte del turno es diferente");
                        case -106:
                            throw new ErrorSPException("Existe solapamiento entre el inicio y fin del turno y del período");
                        case -107:
                            throw new ErrorSPException(getStringError(9));
                        case -108:
                            throw new ErrorSPException("Debe existir algun ticket posterior de este mismo punto de venta");
                    }

                    if (returnTickManuales < 0)
                    {
                        throw new ErrorSPException(getStringError(5));
                    }

                    //Auditoria
                    using (Conexion connAud = new Conexion())
                    {
                        connAud.ConectarGSTPlaza(ConexionBs.getGSToEstacion(), false);

                        AuditoriaBs.addAuditoria(new Auditoria(getAuditoriaCodigoAuditoriaNuevosTicketsManuales(),
                            "A",
                            getAuditoriaCodigoRegistro(parte),
                            getAuditoriaDescripcion("0" , numeroTicket, cantidad, categoria)),
                            connAud);
                    }

                    conn.Commit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void getNombreYEstadoRuc(Int64 ruc, out string nombre, out string estado)
        {
            try
            {
                using (Conexion conn = new Conexion())
                {
                    conn.ConectarConsolidadoPlaza(ConexionBs.getGSToEstacion(), false);
                    TicketManualDt.getNombreYEstadoRuc(conn, ruc, out nombre, out estado);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private static bool esPuntoVentaCompatible(Conexion conn, Estacion estacion, CategoriaManual categoria, ViaDefinicion via, string tipoComprobante, string puntoVenta)
        {
            return TicketManualDt.esPuntoVentaCompatible(conn, estacion, categoria, via, tipoComprobante, puntoVenta);
        }

        private static string getAuditoriaCodigoAuditoriaNuevosTicketsManuales()
        {
            return "RTM";
        }

        ///****************************************************************************************************<summary>
        /// Codigo que identifica a la PK del registro auditado
        /// </summary>****************************************************************************************************
        private static string getAuditoriaCodigoRegistro(ParteValidacion parte)
        {
            return parte.Numero.ToString();
        }

        public static void CalcularMontos(AnomaliaValidacion anomalia)
        {
            #region Ticket Manual
            int nCategoria;

            //Tomo la mayor de las categorias
            nCategoria = Util.Max(Util.Max(anomalia.CategoriaTabulada.Categoria, anomalia.CategoriaConsolidada.Categoria), anomalia.CategoriaTicketManual.Categoria);

            if (nCategoria == anomalia.CategoriaTabulada.Categoria && anomalia.MontoOriginal > 0)
            {
                anomalia.MontoConsolidado = anomalia.MontoOriginal;
            }
            else
            {
                anomalia.MontoConsolidado = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, nCategoria, (int)anomalia.TipoTarifa.CodigoTarifa);
            }
            if (anomalia.MontoConsolidado > anomalia.MontoOriginal)
            {
                anomalia.MontoDiferencia = anomalia.MontoConsolidado - anomalia.MontoOriginal;
            }
            else
            {
                anomalia.MontoDiferencia = 0;
            }
            if (anomalia.CategoriaTicketManual.Categoria > 0)
            {
                //Calculamos el monto del Ticket Manual a tarifa Basica
                anomalia.MontoTicketManual = TarifaBs.getTarifa(anomalia.Estacion.Numero, anomalia.Fecha, anomalia.CategoriaTicketManual.Categoria, 0);
            }
            #endregion
        }
    }
}

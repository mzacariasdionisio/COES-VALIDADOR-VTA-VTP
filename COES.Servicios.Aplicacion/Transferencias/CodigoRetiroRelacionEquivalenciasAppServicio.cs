using COES.Base.Core;
using COES.Base.Tools;
using COES.Dominio.DTO.Transferencias;
using COES.Servicios.Aplicacion.Factory;
using COES.Servicios.Aplicacion.Transferencias.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using COES.Servicios.Aplicacion.IEOD;
using System.Transactions;
using COES.Servicios.Aplicacion.Helper;
using System.Globalization;

namespace COES.Servicios.Aplicacion.Transferencias
{
    public class CodigoRetiroRelacionEquivalenciasAppServicio : AppServicioBase
    {
        /// <summary>
        /// Instancia para el manejo de logs
        /// </summary>
        private static readonly ILog Logger = LogManager.GetLogger(typeof(SolicitudCodigoAppServicio));

        /// <summary>
        /// Lista la relacion de codigos de retiros para casos especiales
        /// </summary>
        /// <param name="nroPagina"></param>
        /// <param name="pageSize"></param>
        /// <param name="genEmprCodi"></param>
        /// <param name="cliEmprCodi"></param>
        /// <param name="barrCodiTra"></param>
        /// <param name="barrCodiSum"></param>
        /// <param name="codigo"></param>
        /// <returns></returns>

        public List<CodigoRetiroRelacionDTO> ListarRelacionCodigoRetiros(int nroPagina, int pageSize,
            int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum, int? tipConCodi, int? tipUsuCodi, string estado, string codigo, int pericodi = 0, int recpotcodi = 0, int optionValorMaximo = 0)
        {
            List<CodigoRetiroRelacionDTO> resultado = new List<CodigoRetiroRelacionDTO>();
            List<CodigoRetiroRelacionDetalleDTO> resultadoDetalle = new List<CodigoRetiroRelacionDetalleDTO>();
            /* if (!string.IsNullOrEmpty(codigo))
             {
                 resultadoDetalle = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().ListarRelacionCodigoRetiros(nroPagina, pageSize, genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);
                 if (result.Count > 0)
                 {
                     resultadoDetalle = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().ListarRelacionCodigoRetirosPorCodigo(result[0].Retrelcodi);
                 }
             }
             else
             {*/
            resultadoDetalle = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().ListarRelacionCodigoRetiros(nroPagina, pageSize, genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);

            //}

            if (resultadoDetalle.Count > 0)
            {
                List<int> idRetRelCodi = resultado.Select(x => x.RetrelCodi).ToList();

                foreach (var itemRel in resultadoDetalle.Select(x => new { x.Retrelcodi, x.Retrelvari }).Distinct())
                {
                    List<String> lstCodVTEA = new List<String>();
                    decimal potecoincidenteVtp = 0;
                    decimal energiavtea = 0;
                    CodigoRetiroRelacionDTO entityRel = new CodigoRetiroRelacionDTO();
                    entityRel.Retrelvari = itemRel.Retrelvari;
                    entityRel.RetrelCodi = itemRel.Retrelcodi;
                    entityRel.ListarRelacion = resultadoDetalle?.FindAll(y => y.Retrelcodi == itemRel.Retrelcodi).OrderBy(x => x.Rerldtcodi).ToList();

                    if (optionValorMaximo == 1)
                    {
                        foreach (var itemDetCod in entityRel.ListarRelacion)
                        {
                            if (lstCodVTEA.Count == 0)
                            {
                                lstCodVTEA.Add(itemDetCod.Codigovtea);
                                if (!String.IsNullOrEmpty(itemDetCod.Genemprnombvtea))
                                {
                                    EmpresaDTO empresa = FactoryTransferencia.GetEmpresaRepository().GetByNombre(itemDetCod.Genemprnombvtea);
                                    energiavtea += this.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, itemDetCod.Codigovtea, empresa.EmprCodi);
                                }
                                else
                                {
                                    energiavtea += 0;

                                }
                            }
                            else
                            {
                                foreach (var itemcodvtea in lstCodVTEA)
                                {
                                    if (itemcodvtea != itemDetCod.Codigovtea)
                                    {
                                        if (!String.IsNullOrEmpty(itemDetCod.Genemprnombvtea))
                                        {
                                            EmpresaDTO empresa = FactoryTransferencia.GetEmpresaRepository().GetByNombre(itemDetCod.Genemprnombvtea);
                                            energiavtea += this.GetMaximaDemandaVTEAEmpresa(pericodi, recpotcodi, itemDetCod.Codigovtea, empresa.EmprCodi);
                                        }
                                        else
                                        {
                                            energiavtea += 0;
                                        }
                                    }
                                }
                            }
                            if (!String.IsNullOrEmpty(itemDetCod.Genemprnombvtp))
                            {
                                EmpresaDTO empresa = FactoryTransferencia.GetEmpresaRepository().GetByNombre(itemDetCod.Genemprnombvtp);
                                potecoincidenteVtp += FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().GetPoteCoincidenteByCodigoVtp(pericodi, recpotcodi, itemDetCod.Codigovtp, empresa.EmprCodi);
                            }
                        }
                        entityRel.EnergiaVtea = Math.Round(energiavtea * 4000, 4);
                        entityRel.PotenciaVtp = Math.Round(potecoincidenteVtp, 4);
                        entityRel.DiferenciaVteaVtp = entityRel.PotenciaVtp - entityRel.EnergiaVtea;
                        entityRel.PorcentajeVariacionCalculado = energiavtea == 0 ? 0 : Math.Round(((energiavtea * 4000) - potecoincidenteVtp) / (energiavtea * 4000), 2);
                        entityRel.PorcentajeVariacionCalculado = entityRel.PorcentajeVariacionCalculado * 100;
                        entityRel.PorcentajeVariacionCalculado = entityRel.PorcentajeVariacionCalculado < 0 ? entityRel.PorcentajeVariacionCalculado * -1 : entityRel.PorcentajeVariacionCalculado;
                        entityRel.PorcentajeVariacionCalculado = Math.Round(entityRel.PorcentajeVariacionCalculado, 2);

                    }
                    if (entityRel.ListarRelacion.Count > 1)
                    {
                        if (entityRel.ListarRelacion[1].Codigovtea == "")
                        {
                            for (int i = 1; i < entityRel.ListarRelacion.Count; i++)
                            {
                                entityRel.ListarRelacion[i].Barrnombvtea = entityRel.ListarRelacion[0].Barrnombvtea;
                                entityRel.ListarRelacion[i].Cliemprnombvtea = entityRel.ListarRelacion[0].Cliemprnombvtea;
                                entityRel.ListarRelacion[i].Codigovtea = entityRel.ListarRelacion[0].Codigovtea;
                                entityRel.ListarRelacion[i].Genemprnombvtea = entityRel.ListarRelacion[0].Genemprnombvtea;
                                entityRel.ListarRelacion[i].Tipocontratovtea = entityRel.ListarRelacion[0].Tipocontratovtea;
                                entityRel.ListarRelacion[i].Tipousuariovtea = entityRel.ListarRelacion[0].Tipousuariovtea;
                            }
                        }
                    }
                    resultado.Add(entityRel);
                }
                //resultadoDetalle = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().ListarRelacionCodigoRetiros(idRetRelCodi);
                //resultadoDetalle = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().ListarRelacionDetalle(idRetRelCodi);

            }
            return resultado;
        }

        //public decimal GetMaximaDemandaVTEA(int pericodi, int recpotcodi,string codvtea)
        //{
        //    VtpRecalculoPotenciaDTO model = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetById(pericodi, recpotcodi);
        //    int dia = model == null ? 0 :Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("dd"));
        //    int hora = model == null ? 0 : Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("HH"));
        //    int minuto = model == null ? 0 : Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("mm"));
        //    decimal totalvtea = 0;
        //    TransferenciaEntregaDetalleDTO transferenciaDetalle = FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().GetDemandaByCodVtea(pericodi, recpotcodi, codvtea, dia);
        //    TransferenciaRetiroDetalleDTO transferenciaRetiroDetalle = FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().GetDemandaRetiroByCodVtea(pericodi, recpotcodi, codvtea, dia);
        //    if (transferenciaDetalle.TranEntrDetaDia != 0)
        //    { 
        //        totalvtea = this.GetTotalDemandaVTEAByDay(transferenciaDetalle, hora, minuto);
        //    }
        //    if(transferenciaRetiroDetalle.TranRetiDetaDia != 0)
        //    {
        //        totalvtea = this.GetTotalDemandaRetiroVTEAByDay(transferenciaRetiroDetalle, hora, minuto);
        //    }
        //    return totalvtea;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pericodi"></param>
        /// <param name="recpotcodi"></param>
        /// <param name="codvtea"></param>
        /// <param name="emprcodi"></param>
        /// <returns></returns>
        public decimal GetMaximaDemandaVTEAEmpresa(int pericodi, int recpotcodi, string codvtea, int emprcodi)
        {
            VtpRecalculoPotenciaDTO model = FactoryTransferencia.GetVtpRecalculoPotenciaRepository().GetById(pericodi, recpotcodi);
            int dia = model == null ? 0 : Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("dd"));
            int hora = model == null ? 0 : Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("HH"));
            int minuto = model == null ? 0 : Int32.Parse(model.Recpotinterpuntames.GetValueOrDefault().ToString("mm"));
            decimal totalvtea = 0;
            TransferenciaEntregaDetalleDTO transferenciaDetalle = FactoryTransferencia.GetTransferenciaEntregaDetalleRepository().GetDemandaByCodVteaEmpresa(pericodi, recpotcodi, codvtea, dia, emprcodi);
            TransferenciaRetiroDetalleDTO transferenciaRetiroDetalle = FactoryTransferencia.GetTransferenciaRetiroDetalleRepository().GetDemandaRetiroByCodVteaEmpresa(pericodi, recpotcodi, codvtea, dia, emprcodi);
            if (transferenciaDetalle.TranEntrDetaDia != 0)
            {
                totalvtea = this.GetTotalDemandaVTEAByDay(transferenciaDetalle, hora, minuto);
            }
            if (transferenciaRetiroDetalle.TranRetiDetaDia != 0)
            {
                totalvtea = this.GetTotalDemandaRetiroVTEAByDay(transferenciaRetiroDetalle, hora, minuto);
            }
            return totalvtea;
        }

        public decimal GetTotalDemandaVTEAByDay(TransferenciaEntregaDetalleDTO transferenciaDetalle, int hora, int minuto)
        {
            decimal totalvtea = 0;
            if (transferenciaDetalle != null)
            {
                if (hora == 0)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah96;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah1;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah2;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah3;
                    }
                }
                else if (hora == 1)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah4;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah5;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah6;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah7;
                    }
                }
                else if (hora == 2)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah8;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah9;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah10;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah11;
                    }
                }
                else if (hora == 3)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah12;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah13;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah14;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah15;
                    }
                }
                else if (hora == 4)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah16;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah17;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah18;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah19;
                    }
                }
                else if (hora == 5)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah20;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah21;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah22;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah23;
                    }
                }
                else if (hora == 6)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah24;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah25;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah26;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah27;
                    }
                }
                else if (hora == 7)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah28;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah29;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah30;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah31;
                    }
                }
                else if (hora == 8)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah32;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah33;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah34;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah35;
                    }
                }
                else if (hora == 9)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah36;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah37;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah38;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah39;
                    }
                }
                else if (hora == 10)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah40;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah41;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah42;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah43;
                    }
                }
                else if (hora == 11)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah44;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah45;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah46;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah47;
                    }
                }
                else if (hora == 12)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah48;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah49;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah50;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah51;
                    }
                }
                else if (hora == 13)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah52;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah53;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah54;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah55;
                    }
                }
                else if (hora == 14)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah56;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah57;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah58;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah59;
                    }
                }
                else if (hora == 15)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah60;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah61;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah62;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah63;
                    }
                }
                else if (hora == 16)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah64;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah65;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah66;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah67;
                    }
                }
                else if (hora == 17)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah68;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah69;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah70;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah71;
                    }
                }
                else if (hora == 18)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah72;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah73;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah74;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah75;
                    }
                }
                else if (hora == 19)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah76;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah77;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah78;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah79;
                    }
                }
                else if (hora == 20)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah80;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah81;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah82;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah83;
                    }
                }
                else if (hora == 21)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah84;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah85;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah86;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah87;
                    }
                }
                else if (hora == 22)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah88;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah89;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah90;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah91;
                    }
                }
                else if (hora == 23)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah92;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah93;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah94;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranEntrDetah95;
                    }
                }
            }
            return totalvtea;
        }

        private decimal GetTotalDemandaRetiroVTEAByDay(TransferenciaRetiroDetalleDTO transferenciaDetalle, int hora, int minuto)
        {
            decimal totalvtea = 0;
            if (transferenciaDetalle != null)
            {
                if (hora == 0)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah96;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah1;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah2;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah3;
                    }
                }
                else if (hora == 1)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah4;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah5;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah6;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah7;
                    }
                }
                else if (hora == 2)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah8;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah9;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah10;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah11;
                    }
                }
                else if (hora == 3)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah12;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah13;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah14;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah15;
                    }
                }
                else if (hora == 4)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah16;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah17;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah18;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah19;
                    }
                }
                else if (hora == 5)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah20;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah21;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah22;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah23;
                    }
                }
                else if (hora == 6)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah24;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah25;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah26;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah27;
                    }
                }
                else if (hora == 7)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah28;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah29;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah30;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah31;
                    }
                }
                else if (hora == 8)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah32;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah33;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah34;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah35;
                    }
                }
                else if (hora == 9)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah36;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah37;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah38;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah39;
                    }
                }
                else if (hora == 10)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah40;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah41;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah42;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah43;
                    }
                }
                else if (hora == 11)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah44;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah45;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah46;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah47;
                    }
                }
                else if (hora == 12)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah48;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah49;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah50;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah51;
                    }
                }
                else if (hora == 13)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah52;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah53;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah54;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah55;
                    }
                }
                else if (hora == 14)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah56;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah57;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah58;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah59;
                    }
                }
                else if (hora == 15)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah60;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah61;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah62;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah63;
                    }
                }
                else if (hora == 16)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah64;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah65;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah66;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah67;
                    }
                }
                else if (hora == 17)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah68;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah69;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah70;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah71;
                    }
                }
                else if (hora == 18)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah72;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah73;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah74;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah75;
                    }
                }
                else if (hora == 19)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah76;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah77;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah78;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah79;
                    }
                }
                else if (hora == 20)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah80;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah81;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah82;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah83;
                    }
                }
                else if (hora == 21)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah84;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah85;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah86;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah87;
                    }
                }
                else if (hora == 22)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah88;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah89;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah90;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah91;
                    }
                }
                else if (hora == 23)
                {
                    if (minuto == 0)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah92;
                    }
                    else if (minuto == 15)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah93;
                    }
                    else if (minuto == 30)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah94;
                    }
                    else if (minuto == 45)
                    {
                        totalvtea = transferenciaDetalle.TranRetiDetah95;
                    }
                }
            }
            return totalvtea;
        }

        /// <summary>
        /// Total registros de codigo retiro relacionados para casos especiales
        /// </summary>
        /// <returns></returns>

        public int TotalRecordsRelacionCodigoRetiros(int? genEmprCodi, int? cliEmprCodi, int? barrCodiTra, int? barrCodiSum,
            int? tipConCodi, int? tipUsuCodi, string estado, string codigo)
        {
            return FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().TotalRecordsRelacionCodigoRetiros(genEmprCodi, cliEmprCodi, barrCodiTra, barrCodiSum, tipConCodi, tipUsuCodi, estado, codigo);
        }

        /// <summary>
        /// Permite grabar equivalencia de codigo VTEA y VTP
        /// </summary>
        /// <param name="entity">Entidad de CodigoRetiroRelacionDTO</param>
        /// <param name="detalle">Entidad de CodigoRetiroRelacionDetalleDTO</param>
        /// <returns>Retorna el RetrelCodi nuevo </returns>
        public int RegistrarEquivalencia(CodigoRetiroRelacionDTO entity, List<CodigoRetiroRelacionDetalleDTO> detalle)
        {
            int id = 0;
            try
            {

                if (entity.RetrelCodi == 0)
                {
                    id = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Save(entity);
                    if (id > 0)
                    {
                        foreach (var item in detalle)
                        {
                            item.Retrelcodi = id;
                            FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(item);
                        }

                    }
                }
                else
                {
                    int result = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Update(entity);
                    if (result > 0)
                    {
                        result = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Delete(entity.RetrelCodi);
                        if (result > 0)
                        {
                            foreach (var item in detalle)
                            {
                                item.Retrelcodi = entity.RetrelCodi;
                                FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().Save(item);
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                id = 0;
                Logger.Error(ConstantesAppServicio.LogError, ex);
            }
            return id;
        }

        /// <summary>
        /// Lista la relacion de codigos de retiros para casos especiales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public CodigoRetiroRelacionDTO ObtenereEquivalencia(int id)
        {
            CodigoRetiroRelacionDTO resultado = new CodigoRetiroRelacionDTO();
            try
            {
                resultado = FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().GetById(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultado = null;
            }

            return resultado;
        }

        /// <summary>
        /// Lista la relacion de codigos de retiros para casos especiales
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CodigoRetiroRelacionDetalleDTO> ObtenereEquivalenciaDetalle(int id)
        {
            List<CodigoRetiroRelacionDetalleDTO> resultado = new List<CodigoRetiroRelacionDetalleDTO>();
            try
            {
                resultado = FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().GetById(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                resultado = null;
            }

            return resultado;
        }

        /// <summary>
        /// Indica si existe un codigo vtea usando en otro registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExisteVTEA(int id)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().ExisteVTEA(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }

        }

        /// <summary>
        /// Indica si existe un codigo vtp usando en otro registro
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ExisteVTP(int id)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroEquivalenciaDetalleRepository().ExisteVTP(id);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return false;
            }

        }

        /// <summary>
        /// Elimina relación de equivalencia
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int EliminarEquivalencia(CodigoRetiroRelacionDTO entity)
        {
            try
            {
                return FactoryTransferencia.GetCodigoRetiroRelacionEquivalenciasRepository().Update(entity);
            }
            catch (Exception ex)
            {
                Logger.Error(ConstantesAppServicio.LogError, ex);
                return 0;
            }

        }

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using static COES.Dominio.DTO.Sic.EqEquipoDTO;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EQ_FAMILIA
    /// </summary>
    public class EqFamiliaRepository: RepositoryBase, IEqFamiliaRepository
    {
        public EqFamiliaRepository(string strConn): base(strConn)
        {
        }

        EqFamiliaHelper helper = new EqFamiliaHelper();

        public int Save(EqFamiliaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Famabrev, DbType.String, entity.Famabrev);
            dbProvider.AddInParameter(command, helper.Tipoecodi, DbType.Int32, entity.Tipoecodi);
            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, entity.Tareacodi);
            dbProvider.AddInParameter(command, helper.Famnomb, DbType.String, entity.Famnomb);
            dbProvider.AddInParameter(command, helper.Famnumconec, DbType.Int32, entity.Famnumconec);
            dbProvider.AddInParameter(command, helper.Famnombgraf, DbType.String, entity.Famnombgraf);
            dbProvider.AddInParameter(command, helper.Famestado, DbType.String, entity.Famestado);
            dbProvider.AddInParameter(command, helper.UsuarioCreacion, DbType.String, entity.UsuarioCreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqFamiliaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Famabrev, DbType.String, entity.Famabrev);
            dbProvider.AddInParameter(command, helper.Tipoecodi, DbType.Int32, entity.Tipoecodi);
            dbProvider.AddInParameter(command, helper.Tareacodi, DbType.Int32, entity.Tareacodi);
            dbProvider.AddInParameter(command, helper.Famnomb, DbType.String, entity.Famnomb);
            dbProvider.AddInParameter(command, helper.Famnumconec, DbType.Int32, entity.Famnumconec);
            dbProvider.AddInParameter(command, helper.Famnombgraf, DbType.String, entity.Famnombgraf);
            dbProvider.AddInParameter(command, helper.Famestado, DbType.String, entity.Famestado);
            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, entity.UsuarioUpdate);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int famcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int famcodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.UsuarioUpdate, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);

            dbProvider.ExecuteNonQuery(command);
        }
 
        public EqFamiliaDTO GetById(int famcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, famcodi);
            EqFamiliaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<TipoSerie> ListTipoSerie()
        {
            List<TipoSerie> entitys = new List<TipoSerie>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTipoSerie);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaSerie(dr));
                }
            }
            return entitys;
        }

        public List<TipoPuntoMedicion> ListTipoPuntoMedicion()
        {
            List<TipoPuntoMedicion> entitys = new List<TipoPuntoMedicion>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListTipoPuntoMedicion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaTipoPuntoMedicion(dr));
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntoMedicionPorEmpresa(int CodEmpresa, int CodTipoSerie, int CodTipoPuntoMedicion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntoMedicionPorEmpresa);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, CodEmpresa);
            dbProvider.AddInParameter(command, helper.TIPOSERIECODI, DbType.Int32, CodTipoSerie);
            dbProvider.AddInParameter(command, helper.PTOMEDICODI, DbType.Int32, CodTipoPuntoMedicion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicion(dr));
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListPuntoMedicionPorCuenca(int cuenca, int tptomedicodi)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntoMedicionPorCuenca);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicion(dr));
                }
            }
            return entitys;
        }
        public List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporado(int cuenca)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntoMedicionPorCuencaNaturalEvaporado);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicion(dr));
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListarPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion(int cuenca, int tipopuntomedicion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPuntoMedicionPorCuencaNaturalEvaporadoPorTipoPuntoMedicion);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.TIPOPUNTOMEDICION, DbType.Int32, tipopuntomedicion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicion(dr));
                }
            }
            return entitys;
        }

        public List<GraficoSeries> ObtenerGraficoAnual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoAnual);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.Int32, anioinicio);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.Int32, aniofin);



            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }

        public List<MeRelacionptoDTO> ObtenerPuntosCalculados(int ptomedicodi)
        {
            List<MeRelacionptoDTO> entitys = new List<MeRelacionptoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPuntosCalculados);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateRelacionPto(dr));
                }
            }
            return entitys;
        }

        public List<GraficoSeries> ObtenerCaudalPuntosCalculados(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int aniofin, string tipoReporte, string anios)
        {
            List<MeRelacionptoDTO> listPuntosCalculados = new List<MeRelacionptoDTO>();
            listPuntosCalculados = ObtenerPuntosCalculados(ptomedicodi);

            List<GraficoSeries> listCaudalFinal = new List<GraficoSeries>();

            if (listPuntosCalculados.Count>0)
            {
                foreach (var puntoCalculado in listPuntosCalculados)
                {
                    List<GraficoSeries> listCaudal = new List<GraficoSeries>();
                    if (tipoReporte=="ANUAL")
                    {
                        listCaudal = ObtenerGraficoAnual(tiposeriecodi, tptomedicodi, puntoCalculado.Ptomedicodi2, anioinicio, aniofin);
                    } else if (tipoReporte == "MENSUAL")
                    {
                        listCaudal = ObtenerGraficoMensual(tiposeriecodi, tptomedicodi, puntoCalculado.Ptomedicodi2, anioinicio);
                    }
                    else if (tipoReporte == "TOTALMENSUAL")
                    {
                        listCaudal = ObtenerGraficoTotal(tiposeriecodi, tptomedicodi, puntoCalculado.Ptomedicodi2, aniofin);
                    }
                    else if (tipoReporte == "COMPARATIVAVOLUMEN")
                    {
                        listCaudal = ObtenerGraficoComparativaVolumen(tiposeriecodi, tptomedicodi, puntoCalculado.Ptomedicodi2, anios);
                    }
                    else if (tipoReporte == "COMPARATIVANATURALEVAPORADA")
                    {
                        listCaudal = ObtenerGraficoComparativaNaturalEvaporada(tiposeriecodi,  puntoCalculado.Ptomedicodi2, anios);
                    }
                    else if (tipoReporte == "TOTALCOMPARATIVANATURALEVAPORADA")
                    {
                        listCaudal = ObtenerGraficoTotalNaturalEvaporada(tiposeriecodi, puntoCalculado.Ptomedicodi2, aniofin);
                    }
                    else if (tipoReporte == "TABLA_VERTICAL")
                    {
                        listCaudal = ListaTablaVerticalPuntosCalculados(puntoCalculado.Ptomedicodi2, tptomedicodi, tiposeriecodi, anioinicio, aniofin);
                    }
                    if (tipoReporte == "ESTADISTICAS_ANUALES")
                    {
                        listCaudal = ObtenerGraficoEstadisticasAnuales(tiposeriecodi, tptomedicodi, puntoCalculado.Ptomedicodi2, anioinicio, 1, aniofin, 12);
                    }

                    if ((!string.IsNullOrEmpty(puntoCalculado.Relptofactor.ToString())) && (!string.IsNullOrEmpty(puntoCalculado.Relptopotencia.ToString())))
                    {
                        foreach (var caudalDetalle in listCaudal)
                        {
                            GraficoSeries caudalDetalleFinal = new GraficoSeries();
                            if (listCaudalFinal.Count>0)
                            {
                                List<GraficoSeries> listCaudalFinalAnio = new List<GraficoSeries>();
                                listCaudalFinalAnio = listCaudalFinal.Where(i => i.Anio == caudalDetalle.Anio).ToList();
                                if (listCaudalFinalAnio.Count > 0)
                                {
                                    caudalDetalleFinal = listCaudalFinalAnio.First();
                                } else
                                {
                                    caudalDetalleFinal.Anio = caudalDetalle.Anio;
                                    caudalDetalleFinal.Ptomedicodi = caudalDetalle.Ptomedicodi;
                                }
                            } else
                            {
                                caudalDetalleFinal.Anio = caudalDetalle.Anio;
                                caudalDetalleFinal.Ptomedicodi = caudalDetalle.Ptomedicodi;
                            }
                            

                            if (caudalDetalle.M1>0)
                            {
                                caudalDetalle.M1 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M1), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M1 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M1));
                                if (caudalDetalleFinal!=null)
                                {
                                    caudalDetalleFinal.Anio = caudalDetalle.Anio;
                                    caudalDetalleFinal.M1 = Convert.ToDecimal(caudalDetalleFinal.M1) + caudalDetalle.M1;
                                    caudalDetalleFinal.Ptomedicodi = caudalDetalle.Ptomedicodi;
                                } 

                            }

                            if (caudalDetalle.M2 > 0)
                            {
                                caudalDetalle.M2 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M2), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M2 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M2));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M2 = Convert.ToDecimal(caudalDetalleFinal.M2) + caudalDetalle.M2;
                                }
                            }

                            if (caudalDetalle.M3 > 0)
                            {
                                caudalDetalle.M3 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M3), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M3 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M3));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M3 = Convert.ToDecimal(caudalDetalleFinal.M3) + caudalDetalle.M3;
                                }
                            }

                            if (caudalDetalle.M4 > 0)
                            {
                                caudalDetalle.M4 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M4), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M4 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M4));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M4 = Convert.ToDecimal(caudalDetalleFinal.M4) + caudalDetalle.M4;
                                }
                            }

                            if (caudalDetalle.M5 > 0)
                            {
                                caudalDetalle.M5 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M5), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M5 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M5));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M5 = Convert.ToDecimal(caudalDetalleFinal.M5) + caudalDetalle.M5;
                                }
                            }

                            if (caudalDetalle.M6 > 0)
                            {
                                caudalDetalle.M6 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M6), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M6 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M6));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M6 = Convert.ToDecimal(caudalDetalleFinal.M6) + caudalDetalle.M6;
                                }
                            }

                            if (caudalDetalle.M7 > 0)
                            {
                                caudalDetalle.M7 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M7), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M7 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M7));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M7 = Convert.ToDecimal(caudalDetalleFinal.M7) + caudalDetalle.M7;
                                }
                            }

                            if (caudalDetalle.M8 > 0)
                            {
                                caudalDetalle.M8 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M8), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M8 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M8));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M8 = Convert.ToDecimal(caudalDetalleFinal.M8) + caudalDetalle.M8;
                                }
                            }

                            if (caudalDetalle.M9 > 0)
                            {
                                caudalDetalle.M9 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M9), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M9 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M9));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M9 = Convert.ToDecimal(caudalDetalleFinal.M9) + caudalDetalle.M9;
                                }
                            }

                            if (caudalDetalle.M10 > 0)
                            {
                                caudalDetalle.M10 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M10), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M10 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M10));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M10 = Convert.ToDecimal(caudalDetalleFinal.M10) + caudalDetalle.M10;
                                }
                            }

                            if (caudalDetalle.M11 > 0)
                            {
                                caudalDetalle.M11 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M11), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M11 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M11));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M11 = Convert.ToDecimal(caudalDetalleFinal.M11) + caudalDetalle.M11;
                                }
                            }

                            if (caudalDetalle.M12 > 0)
                            {
                                caudalDetalle.M12 = Convert.ToDecimal(Math.Pow(Convert.ToDouble(caudalDetalle.M12), Convert.ToDouble(puntoCalculado.Relptopotencia))) * puntoCalculado.Relptofactor;
                                caudalDetalle.M12 = Convert.ToDecimal(String.Format("{0:0.####}", caudalDetalle.M12));
                                if (caudalDetalleFinal != null)
                                {
                                    caudalDetalleFinal.M12 = Convert.ToDecimal(caudalDetalleFinal.M12) + caudalDetalle.M12;
                                }
                            }

                            if (caudalDetalleFinal != null)
                            {
                                List<GraficoSeries> listCaudalFinalAnio = new List<GraficoSeries>();
                                listCaudalFinalAnio = listCaudalFinal.Where(i => i.Anio == caudalDetalle.Anio).ToList();
                                if (listCaudalFinalAnio.Count > 0)
                                {
                                    //caudalDetalleFinal = listCaudalFinalAnio.First();
                                }
                                else
                                {
                                    listCaudalFinal.Add(caudalDetalleFinal);
                                }

                                

                            }
                            
                        }
                    }
                }
            }

            if (tipoReporte=="ESTADISTICAS_ANUALES")
            foreach (var item in listCaudalFinal)
            {
                var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                List<decimal?> valoresList = new List<decimal?>();
                foreach (var itemValor in valores)
                {
                    if (itemValor != null)
                    {
                        valoresList.Add(itemValor);
                    }
                }
                item.maximo = valores.Max();
                item.minimo = valores.Min();
                item.promedio = valores.Average();
                if (valoresList.Count > 0)
                {
                    item.desv = Convert.ToDecimal(this.standardDesviation(valoresList));
                }

            }

            return listCaudalFinal;
        }

        public List<GraficoSeries> ObtenerGraficoMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoMensual);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.Int32, anioinicio);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }

        public List<GraficoSeries> ObtenerCaudalPuntosCalculadosMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            entitys = ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, 0, "MENSUAL", "");
            return entitys;
        }

        public List<GraficoSeries> ObtenerCaudalPuntosCalculadosTotalMensual(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            entitys = ObtenerCaudalPuntosCalculados(tiposeriecodi, tptomedicodi, ptomedicodi, 0, aniofin, "TOTALMENSUAL", "");
            return entitys;
        }



        public List<GraficoSeries> ObtenerGraficoComparativaVolumen(int tiposeriecodi, int tptomedicodi, int ptomedicodi, string anios)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            var query = string.Format(helper.SqlObtenerGraficoComparativaVolumen, tiposeriecodi,
              tptomedicodi,
              ptomedicodi,
              anios);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }
        public List<GraficoSeries> ObtenerGraficoComparativaNaturalEvaporada(int tiposeriecodi, int ptomedicodi, string anioinicio)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            List<int> anios = anioinicio.Split(',').Select(int.Parse).ToList();

            string queryTemplate = helper.SqlObtenerGraficoComparativaNaturalEvaporada;
            string anioPlaceholder = string.Join(", ", anios.Select((_, i) => ":Anio" + i));
            string query = queryTemplate.Replace("{AnioInicioPlaceholder}", anioPlaceholder);

            /*            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoComparativaNaturalEvaporada);
            */
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);
            /*            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.String, anioinicio);
            */
            for (int i = 0; i < anios.Count; i++)
            {
                dbProvider.AddInParameter(command, ":Anio" + i, DbType.Int32, anios[i]);
            }
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }
        public List<GraficoSeries> ObtenerGraficoComparativaLineaTendencia(int tiposeriecodi, int tptomedicodi, string ptomedicodi, int anioinicio, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            List<int> ptomedi = ptomedicodi.Split(',').Select(int.Parse).ToList();

            string queryTemplate = helper.SqlObtenerGraficoComparativaLineaTendencia;
            string ptomedicodiPlaceholder = string.Join(", ", ptomedi.Select((_, i) => ":PtoMediCodi" + i));
            string query = queryTemplate.Replace("{ptomedicodiPlaceholder}", ptomedicodiPlaceholder);

            /*            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoComparativaNaturalEvaporada);
            */
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.Int32, anioinicio);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.String, aniofin);

            for (int i = 0; i < ptomedi.Count; i++)
            {
                dbProvider.AddInParameter(command, ":PtoMediCodi" + i, DbType.Int32, ptomedi[i]);
            }
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGraficoTendencia(dr));
                }
            }
            return entitys;
        }
        
        public List<GraficoSeries> ObtenerGraficoTotal(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoTotal);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.Int32, aniofin);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }
        public List<GraficoSeries> ObtenerGraficoTotalNaturalEvaporada(int tiposeriecodi, int ptomedicodi, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoTotalNaturalEvaporada);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.PtoMediCodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.Int32, aniofin);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }
        public List<GraficoSeries> ObtenerGraficoTotalLineaTendencia(int tiposeriecodi, string ptomedicodi, int aniofin, int tptomedicodi)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            List<int> ptomedi = ptomedicodi.Split(',').Select(int.Parse).ToList();

            string queryTemplate = helper.SqlObtenerGraficoTotalLineaTendencia;
            string ptomedicodiPlaceholder = string.Join(", ", ptomedi.Select((_, i) => ":PtoMediCodi" + i));
            string query = queryTemplate.Replace("{ptomedicodiPlaceholder}", ptomedicodiPlaceholder);

            /*            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerGraficoComparativaNaturalEvaporada);
            */
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.String, aniofin);

            for (int i = 0; i < ptomedi.Count; i++)
            {
                dbProvider.AddInParameter(command, ":PtoMediCodi" + i, DbType.Int32, ptomedi[i]);
            }

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGraficoNombrePtoMedicion(dr));
                }
            }
            return entitys;
        }
        public List<MePtomedicionDTO> ListarPtoMedicionCuenca(int cuenca)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPtoMedicionCuenca);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicionCuenca(dr));
                }
            }
            return entitys;
        }

        public List<MePtomedicionDTO> ListarPtoMedicionCuencaTipoPuntoMedicion(int cuenca, int tipopuntomedicion)
        {
            List<MePtomedicionDTO> entitys = new List<MePtomedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPtoMedicionCuencaTipoPuntoMedicion);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, cuenca);
            dbProvider.AddInParameter(command, helper.TIPOPUNTOMEDICION, DbType.Int32, tipopuntomedicion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaPuntoMedicionCuenca(dr));
                }
            }
            return entitys;
        }
        public List<TablaVertical> ListaTablaVertical(string ptomedicodi, int tptomedicodi, int tiposeriecodi,int anioinicio, int aniofin)
        {
            List<TablaVertical> entitys = new List<TablaVertical>();
            List<int> ptomedi = ptomedicodi.Split(',').Select(int.Parse).ToList();
            string queryTemplate = helper.SqlObtenerListaTablaVertical;
            string ptomedicodiPlaceholder = string.Join(", ", ptomedi.Select((_, i) => ":PtoMediCodi" + i));
            string query = queryTemplate.Replace("{ptomedicodiPlaceholder}", ptomedicodiPlaceholder);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.String, anioinicio);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.String, aniofin);

            for (int i = 0; i < ptomedi.Count; i++)
            {
                dbProvider.AddInParameter(command, ":PtoMediCodi" + i, DbType.Int32, ptomedi[i]);
            }

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaTablaVertical(dr));
                }
            }
            return entitys;
        }

        public List<GraficoSeries> ListaTablaVerticalPuntosCalculados(int ptomedicodi, int tptomedicodi, int tiposeriecodi, int anioinicio, int aniofin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            string queryTemplate = helper.SqlObtenerListaTablaVertical;
            string ptomedicodiPlaceholder = ":PtoMediCodi";
            string query = queryTemplate.Replace("{ptomedicodiPlaceholder}", ptomedicodiPlaceholder);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            dbProvider.AddInParameter(command, helper.TipoPtoMediCodi, DbType.Int32, tptomedicodi);
            dbProvider.AddInParameter(command, helper.TipoSerieCodi, DbType.Int32, tiposeriecodi);
            dbProvider.AddInParameter(command, helper.AnioInicio, DbType.String, anioinicio);
            dbProvider.AddInParameter(command, helper.AnioFin, DbType.String, aniofin);
            dbProvider.AddInParameter(command, helper.PTOMEDICODI, DbType.Int32, ptomedicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateListaGrafico(dr));
                }
            }
            return entitys;
        }

        public List<GraficoSeries> ObtenerGraficoEstadisticasAnuales(int tiposeriecodi, int tptomedicodi, int ptomedicodi, int anioinicio, int mesinicio, int aniofin, int mesfin)
        {
            List<GraficoSeries> entitys = new List<GraficoSeries>();
            entitys = this.ObtenerGraficoAnual(tiposeriecodi, tptomedicodi, ptomedicodi, anioinicio, aniofin);
            if (entitys!=null)
            {
                foreach(var item in entitys)
                {

                    if (item.Anio==anioinicio)
                    {
                        if (mesinicio == 2) {
                            item.M1 = null;
                        } 
                        else if (mesinicio == 3)
                        {
                            item.M1 = null;
                            item.M2 = null;
                        }
                        else if (mesinicio == 4)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                        }
                        else if (mesinicio == 5)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                        }
                        else if (mesinicio == 6)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                        }
                        else if (mesinicio == 7)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                        }
                        else if (mesinicio == 8)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                        }
                        else if (mesinicio == 9)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                        }
                        else if (mesinicio == 10)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                        }
                        else if (mesinicio == 11)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                        }
                        else if (mesinicio == 12)
                        {
                            item.M1 = null;
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                        }

                    } 
                    if (item.Anio==aniofin)
                    {
                        if (mesfin == 1)
                        {
                            item.M2 = null;
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 2)
                        {
                            item.M3 = null;
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 3)
                        {
                            item.M4 = null;
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 4)
                        {
                            item.M5 = null;
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 5)
                        {
                            item.M6 = null;
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 6)
                        {
                            item.M7 = null;
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 7)
                        {
                            item.M8 = null;
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 8)
                        {
                            item.M9 = null;
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 9)
                        {
                            item.M10 = null;
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 10)
                        {
                            item.M11 = null;
                            item.M12 = null;
                        }
                        else if (mesfin == 11)
                        {
                            item.M12 = null;
                        }
                        
                    }

                }

                foreach (var item in entitys)
                {
                    var valores = new decimal?[] { item.M1, item.M2, item.M3, item.M4, item.M5, item.M6, item.M7, item.M8, item.M9, item.M10, item.M11, item.M12 };
                    List<decimal?> valoresList = new List<decimal?>();
                    foreach (var itemValor in valores)
                    {
                        if (itemValor!=null)
                        {
                            valoresList.Add(itemValor);
                        }
                    }
                    item.maximo = valores.Max();
                    item.minimo = valores.Min();
                    item.promedio = valores.Average();
                    if (valoresList.Count>0)
                    {
                        item.desv = Convert.ToDecimal(String.Format("{0:#,0.000}", this.standardDesviation(valoresList)));
                    }
                   
                }
            }
            
            return entitys;
        }

        public double standardDesviation(List<decimal?> valores)
        {
            decimal? average = valores.Average();
            decimal? sum = Convert.ToDecimal(valores.Sum(d => Math.Pow(Convert.ToDouble(d - average), 2)));
            return Math.Sqrt((Convert.ToDouble(sum)) / valores.Count());
        }


        public MePtomedicionDTO GetPtoMedicionById(int CodPuntoMedicion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlInfoPuntoMedicionPorEmpresa);
            dbProvider.AddInParameter(command, helper.PTOMEDICODI, DbType.Int32, CodPuntoMedicion);
            MePtomedicionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {

                    entity = helper.CreatePuntoMedicion(dr);
                
                }
            }

            return entity;
        }

        public List<EqFamiliaDTO> List()
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqFamiliaDTO> GetByCriteria(string strEstado)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Famestado, DbType.String, strEstado);
            dbProvider.AddInParameter(command, helper.Famestado, DbType.String, strEstado);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EqFamiliaDTO> ObtenerFamiliasProcManiobras()
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerFamiliasProcManiobras);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        #region PR5
        public List<EqFamiliaDTO> ListarFamiliaXEmp(int idEmpresa)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();

            string queryEq = string.Format(helper.SqlObtenerFamiliasXEmp, idEmpresa);

            DbCommand command = dbProvider.GetSqlStringCommand(queryEq);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        #endregion

        #region INTERVENCIONES
        public List<EqFamiliaDTO> ListarComboTipoEquiposXUbicaciones(int IdArea)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarComboTipoEquiposXUbicaciones);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, IdArea);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqFamiliaDTO> ListarByTareaIds(string idTareas)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlListarByTareaIds, idTareas));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Mejoras Ieod

        public List<EqFamiliaDTO> ListarFamiliaPorOrigenLecturaEquipo(int origlectcodi,int emprcodi)
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            string queryEq = string.Format(helper.SqlListarFamiliaPorOrigenLecturaEquipo, origlectcodi,emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryEq);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region FICHA TÉCNICA

        public List<EqFamiliaDTO> ListarFamiliasFT()
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarFamiliasFT);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqFamiliaDTO entity = new EqFamiliaDTO();

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iTareaabrev = dr.GetOrdinal(helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToInt32(dr.GetValue(iTareacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
        
        #region GestProtect
        public List<EqFamiliaDTO> ListFamiliaEquipamientoCOES()
        {
            List<EqFamiliaDTO> entitys = new List<EqFamiliaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListFamiliaEquipamientoCOES);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
		#endregion

    }
}

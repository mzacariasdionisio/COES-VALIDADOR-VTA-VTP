using COES.Base.Core;
using COES.Dominio.DTO.ReportesFrecuencia;
using COES.Dominio.Interfaces.ReportesFrecuencia;
using COES.Infraestructura.Datos.Helper.ReportesFrecuencia;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.ReportesFrecuencia
{
    public class ReporteSegundosFaltantesRepository : RepositoryBase, IReporteSegundosFaltantesRepository
    {
        public ReporteSegundosFaltantesRepository(string strConn) : base(strConn)
        {
        }
        ReporteSegundosFaltantesHelper helper = new ReporteSegundosFaltantesHelper();

        public List<ReporteSegundosFaltantesDTO> GetReporteSegundosFaltantes(ReporteSegundosFaltantesParam param)
        {
            List<ReporteSegundosFaltantesDTO> lista = new List<ReporteSegundosFaltantesDTO>();
            try
            {
                string strIndOficial = string.Empty;
                if (string.IsNullOrEmpty(param.IndOficial))
                {
                    strIndOficial = "";
                }
                else
                {
                    strIndOficial = param.IndOficial;
                }
                var query = string.Format(helper.SqlGetReporteSegundosFaltantes,
                                            param.FechaInicial,
                                            param.FechaFinal,
                                            param.IdGPS,
                                            strIndOficial);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        lista.Add(helper.Create(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }

        public List<ReporteSegundosFaltantesDTO> GetReporteTotalSegundosFaltantes(ReporteSegundosFaltantesParam param)
        {
            List<ReporteSegundosFaltantesDTO> lista = new List<ReporteSegundosFaltantesDTO>();
            try
            {
                string strIndOficial = string.Empty;
                if (string.IsNullOrEmpty(param.IndOficial))
                {
                    strIndOficial = "";
                }
                else
                {
                    strIndOficial = param.IndOficial;
                }
                var query = string.Format(helper.SqlGetReporteTotalSegundosFaltantes,
                                            param.FechaInicial,
                                            param.FechaFinal,
                                            param.IdGPS,
                                            strIndOficial);
                DbCommand command = dbProvider.GetSqlStringCommand(query);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        lista.Add(helper.Create(dr));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return lista;
        }


    }
}


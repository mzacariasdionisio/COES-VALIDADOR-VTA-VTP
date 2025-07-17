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
    public class ReporteFrecuenciaAuditRepository : RepositoryBase, IReporteFrecuenciaAuditRepository
    {
        ReportesFrecuenciaAuditHelper helper;
        public ReporteFrecuenciaAuditRepository(string strConn) : base(strConn)
        {
            helper = new ReportesFrecuenciaAuditHelper();
        }


        List<FrecuenciasAudit> IReporteFrecuenciaAuditRepository.GetFrecuenciasAudit(ReporteFrecuenciaParam param)
        {
            List<FrecuenciasAudit> entitys = new List<FrecuenciasAudit>();
            var query = string.Format(helper.GetFrecuenciasAudit, param.IdGPS);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }

        FrecuenciasAudit IReporteFrecuenciaAuditRepository.GetFrecuenciaAudit(int id)
        {
            List<FrecuenciasAudit> entitys = new List<FrecuenciasAudit>();
            var query = string.Format(helper.GetFrecuenciaAudit, id);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    return helper.Create(dr);
                }
            }
            return new FrecuenciasAudit();
        }

        int IReporteFrecuenciaAuditRepository.Grabar(ReporteFrecuenciaParam param)
        {
            var query = string.Format(helper.SQLSave, param.FechaInicial.ToString("dd/MM/yyyy HH:mm:ss")
                , param.FechaFinal.ToString("dd/MM/yyyy HH:mm:ss"), param.IdGPS, param.Usuario);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddOutParameter(command, "P_MENSAJE", DbType.String, 200);
            return dbProvider.ExecuteNonQuery(command);
            //var aa = dbProvider.GetParameterValue(command, "P_MENSAJE") == DBNull.Value ? "" : (string)dbProvider.GetParameterValue(command, "P_MENSAJE");
        }

        int IReporteFrecuenciaAuditRepository.Eliminar(int ID, string Usuario)
        {
            var query = string.Format(helper.SQLEliminar, Usuario, ID);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            //dbProvider.AddOutParameter(command, "P_MENSAJE", DbType.String, 200);
            return dbProvider.ExecuteNonQuery(command);
        }
    }
}

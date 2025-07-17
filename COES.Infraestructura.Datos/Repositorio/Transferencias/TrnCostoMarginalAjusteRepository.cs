using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    public class TrnCostoMarginalAjusteRepository: RepositoryBase, ITrnCostoMarginalAjusteRepository
    {
        public TrnCostoMarginalAjusteRepository(string strConn) : base(strConn)
        {
        }

        TrnCostoMarginalAjusteHelper helper = new TrnCostoMarginalAjusteHelper();

        public void Update(int idPeriodo, int idVersion, string suser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Trncmausumodificacion, DbType.String, suser);
            dbProvider.AddInParameter(command, helper.Trncmafecmodificacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, idPeriodo);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, idVersion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int trncmacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Trncmacodi, DbType.Int32, trncmacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PeriodoDTO GetPeriodo(int idPeriodo)
        {
            PeriodoDTO entity = new PeriodoDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetPeriodo);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, idPeriodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iPericodi = dr.GetOrdinal(helper.Pericodi);
                    if (!dr.IsDBNull(iPericodi)) entity.PeriCodi = Convert.ToInt32(dr.GetValue(iPericodi));

                    int iPerianio = dr.GetOrdinal(helper.Perianio);
                    if (!dr.IsDBNull(iPerianio)) entity.AnioCodi = Convert.ToInt32(dr.GetValue(iPerianio));

                    int iPerimes = dr.GetOrdinal(helper.Perimes);
                    if (!dr.IsDBNull(iPerimes)) entity.MesCodi = Convert.ToInt32(dr.GetValue(iPerimes));

                    int iPerinombre = dr.GetOrdinal(helper.Perinombre);
                    if (!dr.IsDBNull(iPerinombre)) entity.PeriNombre = dr.GetString(iPerinombre);
                }
            }

            return entity;
        }

        public List<TrnCostoMarginalAjusteDTO> ListByPeriodoVersion(int idPeriodo, int idVersion)
        {
            List<TrnCostoMarginalAjusteDTO> entitys = new List<TrnCostoMarginalAjusteDTO>();
            string query = string.Format(helper.SqlListByPeriodoVersion, idPeriodo, idVersion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    TrnCostoMarginalAjusteDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(TrnCostoMarginalAjusteDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Trncmacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, entity.Pericodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, entity.Recacodi);
            dbProvider.AddInParameter(command, helper.Trncmafecha, DbType.DateTime, entity.Trncmafecha);            
            dbProvider.AddInParameter(command, helper.Trncmausucreacion, DbType.String, entity.Trncmausucreacion);
            dbProvider.AddInParameter(command, helper.Trncmafeccreacion, DbType.DateTime, entity.Trncmafeccreacion);
            dbProvider.ExecuteNonQuery(command);
        }

        public void CopiarAjustesCostosMarginales(int iPeriCodi, int iVersionNew, int iVersionOld)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlCopiarAjustesCostosMarginales);
            dbProvider.AddInParameter(command, helper.Trncmacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iVersionNew);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iVersionOld);
            dbProvider.ExecuteNonQuery(command);
        }

        public void DeleteListaAjusteCostoMarginal(int iPeriCodi, int iVersion)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDeleteListaAjusteCostoMarginal);
            dbProvider.AddInParameter(command, helper.Pericodi, DbType.Int32, iPeriCodi);
            dbProvider.AddInParameter(command, helper.Recacodi, DbType.Int32, iVersion);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}

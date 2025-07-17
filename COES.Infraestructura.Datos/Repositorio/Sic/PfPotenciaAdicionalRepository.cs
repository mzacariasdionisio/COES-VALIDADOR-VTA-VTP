using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PF_POTENCIA_ADICIONAL
    /// </summary>
    public class PfPotenciaAdicionalRepository : RepositoryBase, IPfPotenciaAdicionalRepository
    {
        public PfPotenciaAdicionalRepository(string strConn) : base(strConn)
        {
        }

        PfPotenciaAdicionalHelper helper = new PfPotenciaAdicionalHelper();

        public int Save(PfPotenciaAdicionalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfpadicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pfpadipe, DbType.Decimal, entity.Pfpadipe);
            dbProvider.AddInParameter(command, helper.Pfpadifi, DbType.Decimal, entity.Pfpadifi);
            dbProvider.AddInParameter(command, helper.Pfpadipf, DbType.Decimal, entity.Pfpadipf);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfpadiincremental, DbType.Int32, entity.Pfpadiincremental);
            dbProvider.AddInParameter(command, helper.Pfpadiunidadnomb, DbType.String, entity.Pfpadiunidadnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfPotenciaAdicionalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Pfpadipe, DbType.Decimal, entity.Pfpadipe);
            dbProvider.AddInParameter(command, helper.Pfpadifi, DbType.Decimal, entity.Pfpadifi);
            dbProvider.AddInParameter(command, helper.Pfpadipf, DbType.Decimal, entity.Pfpadipf);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfpadiincremental, DbType.Int32, entity.Pfpadiincremental);
            dbProvider.AddInParameter(command, helper.Pfpadiunidadnomb, DbType.String, entity.Pfpadiunidadnomb);
            dbProvider.AddInParameter(command, helper.Pfpadicodi, DbType.Int32, entity.Pfpadicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfpadicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfpadicodi, DbType.Int32, pfpadicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfPotenciaAdicionalDTO GetById(int pfpadicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfpadicodi, DbType.Int32, pfpadicodi);
            PfPotenciaAdicionalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfPotenciaAdicionalDTO> List()
        {
            List<PfPotenciaAdicionalDTO> entitys = new List<PfPotenciaAdicionalDTO>();
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

        public List<PfPotenciaAdicionalDTO> GetByCriteria()
        {
            List<PfPotenciaAdicionalDTO> entitys = new List<PfPotenciaAdicionalDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<PfPotenciaAdicionalDTO> ListarPotAdicionalFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId)
        {
            List<PfPotenciaAdicionalDTO> entitys = new List<PfPotenciaAdicionalDTO>();

            string query = string.Format(helper.SqlListarPotAdicionalFiltro, recacodi, pericodi, emprcodi, centralId, versionId);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

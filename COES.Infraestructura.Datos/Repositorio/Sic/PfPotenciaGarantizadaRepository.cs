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
    /// Clase de acceso a datos de la tabla PF_POTENCIA_GARANTIZADA
    /// </summary>
    public class PfPotenciaGarantizadaRepository : RepositoryBase, IPfPotenciaGarantizadaRepository
    {
        public PfPotenciaGarantizadaRepository(string strConn) : base(strConn)
        {
        }

        PfPotenciaGarantizadaHelper helper = new PfPotenciaGarantizadaHelper();

        public int Save(PfPotenciaGarantizadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Pfpgarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pfpgarpe, DbType.Decimal, entity.Pfpgarpe);
            dbProvider.AddInParameter(command, helper.Pfpgarpg, DbType.Decimal, entity.Pfpgarpg);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfpgarunidadnomb, DbType.String, entity.Pfpgarunidadnomb);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PfPotenciaGarantizadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Equipadre, DbType.Int32, entity.Equipadre);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Pfpgarpe, DbType.Decimal, entity.Pfpgarpe);
            dbProvider.AddInParameter(command, helper.Pfpgarpg, DbType.Decimal, entity.Pfpgarpg);
            dbProvider.AddInParameter(command, helper.Pfpericodi, DbType.Int32, entity.Pfpericodi);
            dbProvider.AddInParameter(command, helper.Pfverscodi, DbType.Int32, entity.Pfverscodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Pfpgarunidadnomb, DbType.String, entity.Pfpgarunidadnomb);
            dbProvider.AddInParameter(command, helper.Pfpgarcodi, DbType.Int32, entity.Pfpgarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int pfpgarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Pfpgarcodi, DbType.Int32, pfpgarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PfPotenciaGarantizadaDTO GetById(int pfpgarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Pfpgarcodi, DbType.Int32, pfpgarcodi);
            PfPotenciaGarantizadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PfPotenciaGarantizadaDTO> List()
        {
            List<PfPotenciaGarantizadaDTO> entitys = new List<PfPotenciaGarantizadaDTO>();
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

        public List<PfPotenciaGarantizadaDTO> GetByCriteria()
        {
            List<PfPotenciaGarantizadaDTO> entitys = new List<PfPotenciaGarantizadaDTO>();
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

        public List<PfPotenciaGarantizadaDTO> ListarPotGarantizadaFiltro(int pericodi, int recacodi, int emprcodi, int centralId, int versionId)
        {
            List<PfPotenciaGarantizadaDTO> entitys = new List<PfPotenciaGarantizadaDTO>();

            string query = string.Format(helper.SqlListarPotGarantizadaFiltro, recacodi, pericodi, emprcodi, centralId, versionId);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    //entitys.Add(helper.Create(dr));
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

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_EQUIPODAT
    /// </summary>
    public class PrEquipodatRepository: RepositoryBase, IPrEquipodatRepository
    {
        public PrEquipodatRepository(string strConn): base(strConn)
        {
        }

        PrEquipodatHelper helper = new PrEquipodatHelper();

        public void Save(PrEquipodatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Formuladat, DbType.String, entity.Formuladat);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.Fechadat);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrEquipodatDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, entity.Concepcodi);
            dbProvider.AddInParameter(command, helper.Formuladat, DbType.String, entity.Formuladat);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, entity.Fechadat);
            dbProvider.AddInParameter(command, helper.Deleted, DbType.Int32, entity.Deleted);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int equicodi, int grupocodi, int concepcodi, DateTime fechadat)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, fechadat);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrEquipodatDTO GetById(int equicodi, int grupocodi, int concepcodi, DateTime fechadat)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Concepcodi, DbType.Int32, concepcodi);
            dbProvider.AddInParameter(command, helper.Fechadat, DbType.DateTime, fechadat);
            PrEquipodatDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrEquipodatDTO> List()
        {
            List<PrEquipodatDTO> entitys = new List<PrEquipodatDTO>();
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

        public List<PrEquipodatDTO> GetByCriteria()
        {
            List<PrEquipodatDTO> entitys = new List<PrEquipodatDTO>();
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


        public List<ConceptoDatoDTO> ListarDatosConceptoEquipoDat(int iEquiCodi, int iGrupoCodi)
        {
            var resultado = new List<ConceptoDatoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValoresModoOperacionEquipoDat);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, iGrupoCodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, iEquiCodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oDatoConc = new ConceptoDatoDTO();
                    oDatoConc.CONCEPCODI = dr.IsDBNull(dr.GetOrdinal("CONCEPCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("CONCEPCODI"));
                    oDatoConc.CONCEPUNID = dr.IsDBNull(dr.GetOrdinal("CONCEPUNID")) ? "" : dr.GetString(dr.GetOrdinal("CONCEPUNID"));
                    oDatoConc.VALOR = dr.IsDBNull(dr.GetOrdinal("VALOR")) ? "" : dr.GetString(dr.GetOrdinal("VALOR"));
                    resultado.Add(oDatoConc);
                }
            }

            return resultado;
        }
    }
}

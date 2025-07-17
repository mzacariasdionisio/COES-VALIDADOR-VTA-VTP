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
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_TERMSUPERAVIT
    /// </summary>
    public class VcrTermsuperavitRepository: RepositoryBase, IVcrTermsuperavitRepository
    {
        public VcrTermsuperavitRepository(string strConn): base(strConn)
        {
        }

        VcrTermsuperavitHelper helper = new VcrTermsuperavitHelper();

        public int Save(VcrTermsuperavitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrtscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrtsfecha, DbType.DateTime, entity.Vcrtsfecha);
            dbProvider.AddInParameter(command, helper.Vcrtsmpa, DbType.Decimal, entity.Vcrtsmpa);
            dbProvider.AddInParameter(command, helper.Vcrtsdefmwe, DbType.Decimal, entity.Vcrtsdefmwe);
            dbProvider.AddInParameter(command, helper.Vcrtssupmwe, DbType.Decimal, entity.Vcrtssupmwe);
            dbProvider.AddInParameter(command, helper.Vcrtsrnsmwe, DbType.Decimal, entity.Vcrtsrnsmwe);
            dbProvider.AddInParameter(command, helper.Vcrtsdeficit, DbType.Decimal, entity.Vcrtsdeficit);
            dbProvider.AddInParameter(command, helper.Vcrtssuperavit, DbType.Decimal, entity.Vcrtssuperavit);
            dbProvider.AddInParameter(command, helper.Vcrtsresrvnosumn, DbType.Decimal, entity.Vcrtsresrvnosumn);
            dbProvider.AddInParameter(command, helper.Vcrtsusucreacion, DbType.String, entity.Vcrtsusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrtsfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrTermsuperavitDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            //dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            //dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            //dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            //dbProvider.AddInParameter(command, helper.Vcrtsfecha, DbType.DateTime, entity.Vcrtsfecha);
            //dbProvider.AddInParameter(command, helper.Vcrtsmpa, DbType.Decimal, entity.Vcrtsmpa);
            dbProvider.AddInParameter(command, helper.Vcrtsdefmwe, DbType.Decimal, entity.Vcrtsdefmwe);
            dbProvider.AddInParameter(command, helper.Vcrtssupmwe, DbType.Decimal, entity.Vcrtssupmwe);
            dbProvider.AddInParameter(command, helper.Vcrtsrnsmwe, DbType.Decimal, entity.Vcrtsrnsmwe);
            dbProvider.AddInParameter(command, helper.Vcrtsdeficit, DbType.Decimal, entity.Vcrtsdeficit);
            dbProvider.AddInParameter(command, helper.Vcrtssuperavit, DbType.Decimal, entity.Vcrtssuperavit);
            dbProvider.AddInParameter(command, helper.Vcrtsresrvnosumn, DbType.Decimal, entity.Vcrtsresrvnosumn);
            //dbProvider.AddInParameter(command, helper.Vcrtsusucreacion, DbType.String, entity.Vcrtsusucreacion);
            //dbProvider.AddInParameter(command, helper.Vcrtsfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrtscodi, DbType.Int32, entity.Vcrtscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrtscodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrTermsuperavitDTO GetById(int vcrtscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrtscodi, DbType.Int32, vcrtscodi);
            VcrTermsuperavitDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrTermsuperavitDTO GetByIdDia(int vcrecacodi, int grupocodi, DateTime vcrtsfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdDia);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrtsfecha, DbType.DateTime, vcrtsfecha);
            VcrTermsuperavitDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrTermsuperavitDTO> List()
        {
            List<VcrTermsuperavitDTO> entitys = new List<VcrTermsuperavitDTO>();
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

        public List<VcrTermsuperavitDTO> GetByCriteria()
        {
            List<VcrTermsuperavitDTO> entitys = new List<VcrTermsuperavitDTO>();
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

        public List<VcrTermsuperavitDTO> ListPorMesURS(int vcrecacodi, int grupocodi)
        {
            List<VcrTermsuperavitDTO> entitys = new List<VcrTermsuperavitDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPorMesURS);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}

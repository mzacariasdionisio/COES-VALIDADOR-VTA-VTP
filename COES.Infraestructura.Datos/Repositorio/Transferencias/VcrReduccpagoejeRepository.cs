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
    /// Clase de acceso a datos de la tabla VCR_REDUCCPAGOEJE
    /// </summary>
    public class VcrReduccpagoejeRepository: RepositoryBase, IVcrReduccpagoejeRepository
    {
        public VcrReduccpagoejeRepository(string strConn): base(strConn)
        {
        }

        VcrReduccpagoejeHelper helper = new VcrReduccpagoejeHelper();

        public int Save(VcrReduccpagoejeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrpecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Vcrpecumplmes, DbType.Decimal, entity.Vcrpecumplmes);
            dbProvider.AddInParameter(command, helper.Vcrpereduccpagomax, DbType.Decimal, entity.Vcrpereduccpagomax);
            dbProvider.AddInParameter(command, helper.Vcrpereduccpagoeje, DbType.Decimal, entity.Vcrpereduccpagoeje);
            dbProvider.AddInParameter(command, helper.Vcrpeusucreacion, DbType.String, entity.Vcrpeusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrpefeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrReduccpagoejeDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrpereduccpagoeje, DbType.Decimal, entity.Vcrpereduccpagoeje);
            dbProvider.AddInParameter(command, helper.Vcrpecodi, DbType.Int32, entity.Vcrpecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrReduccpagoejeDTO GetById(int vcrecacodi, int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            VcrReduccpagoejeDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrReduccpagoejeDTO> List(int vcrecacodi)
        {
            List<VcrReduccpagoejeDTO> entitys = new List<VcrReduccpagoejeDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrReduccpagoejeDTO> GetByCriteria()
        {
            List<VcrReduccpagoejeDTO> entitys = new List<VcrReduccpagoejeDTO>();
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
    }
}

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
    /// Clase de acceso a datos de la tabla VCR_MEDBORNECARGOINCP
    /// </summary>
    public class VcrMedbornecargoincpRepository: RepositoryBase, IVcrMedbornecargoincpRepository
    {
        public VcrMedbornecargoincpRepository(string strConn): base(strConn)
        {
        }

        VcrMedbornecargoincpHelper helper = new VcrMedbornecargoincpHelper();

        public int Save(VcrMedbornecargoincpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcmbcicodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcmbciconsiderar, DbType.String, entity.Vcmbciconsiderar);
            dbProvider.AddInParameter(command, helper.Vcmbciusucreacion, DbType.String, entity.Vcmbciusucreacion);
            dbProvider.AddInParameter(command, helper.Vcmbcifeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrMedbornecargoincpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicen, DbType.Int32, entity.Equicodicen);
            dbProvider.AddInParameter(command, helper.Equicodiuni, DbType.Int32, entity.Equicodiuni);
            dbProvider.AddInParameter(command, helper.Vcmbciconsiderar, DbType.String, entity.Vcmbciconsiderar);
            dbProvider.AddInParameter(command, helper.Vcmbciusucreacion, DbType.String, entity.Vcmbciusucreacion);
            dbProvider.AddInParameter(command, helper.Vcmbcifeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcmbcicodi, DbType.Int32, entity.Vcmbcicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateVersionNO(int vcrecacodi, string sUser)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVersionNO);
            dbProvider.AddInParameter(command, helper.Vcmbciusucreacion, DbType.String, sUser);
            dbProvider.AddInParameter(command, helper.Vcmbcifeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateVersionSI(int vcrecacodi, string sUser, int vcmbcicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateVersionSI);

            dbProvider.AddInParameter(command, helper.Vcmbciusucreacion, DbType.String, sUser);
            dbProvider.AddInParameter(command, helper.Vcmbcifeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcmbcicodi, DbType.Int32, vcmbcicodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrMedbornecargoincpDTO GetById(int vcmbcicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcmbcicodi, DbType.Int32, vcmbcicodi);
            VcrMedbornecargoincpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrMedbornecargoincpDTO> List(int vcrecacodi)
        {
            List<VcrMedbornecargoincpDTO> entitys = new List<VcrMedbornecargoincpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrMedbornecargoincpDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinombcen = dr.GetOrdinal(this.helper.Equinombcen);
                    if (!dr.IsDBNull(iEquinombcen)) entity.Equinombcen = dr.GetString(iEquinombcen);

                    int iEquinombuni = dr.GetOrdinal(this.helper.Equinombuni);
                    if (!dr.IsDBNull(iEquinombuni)) entity.Equinombuni = dr.GetString(iEquinombuni);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<VcrMedbornecargoincpDTO> GetByCriteria()
        {
            List<VcrMedbornecargoincpDTO> entitys = new List<VcrMedbornecargoincpDTO>();
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

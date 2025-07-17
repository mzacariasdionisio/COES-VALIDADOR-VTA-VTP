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
    /// Clase de acceso a datos de la tabla SI_MENUREPORTE_GRAFICO
    /// </summary>
    public class SiMenureporteGraficoRepository: RepositoryBase, ISiMenureporteGraficoRepository
    {
        public SiMenureporteGraficoRepository(string strConn): base(strConn)
        {
        }

        SiMenureporteGraficoHelper helper = new SiMenureporteGraficoHelper();

        public int Save(SiMenureporteGraficoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);
            dbProvider.AddInParameter(command, helper.Mrgrcodi, DbType.Int32, id);            
            dbProvider.AddInParameter(command, helper.Mrgrestado, DbType.Int32, entity.Mrgrestado);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenureporteGraficoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);            
            dbProvider.AddInParameter(command, helper.Mrgrestado, DbType.Int32, entity.Mrgrestado);
            dbProvider.AddInParameter(command, helper.Reporcodi, DbType.Int32, entity.Reporcodi);
            dbProvider.AddInParameter(command, helper.Mrgrcodi, DbType.Int32, entity.Mrgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mrgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mrgrcodi, DbType.Int32, mrgrcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenureporteGraficoDTO GetById(int mrgrcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mrgrcodi, DbType.Int32, mrgrcodi);
            SiMenureporteGraficoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenureporteGraficoDTO> List()
        {
            List<SiMenureporteGraficoDTO> entitys = new List<SiMenureporteGraficoDTO>();
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

        public List<SiMenureporteGraficoDTO> GetByCriteria(int tmrepcodi)
        {
            List<SiMenureporteGraficoDTO> entitys = new List<SiMenureporteGraficoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, tmrepcodi);
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

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
    /// Clase de acceso a datos de la tabla SI_MENUREPORTE_HOJA
    /// </summary>
    public class SiMenureporteHojaRepository : RepositoryBase, ISiMenureporteHojaRepository
    {
        public SiMenureporteHojaRepository(string strConn) : base(strConn)
        {
        }

        SiMenureporteHojaHelper helper = new SiMenureporteHojaHelper();

        public int Save(SiMenureporteHojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Mrephcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Mrephnombre, DbType.String, entity.Mrephnombre);
            dbProvider.AddInParameter(command, helper.Mrephestado, DbType.Int32, entity.Mrephestado);
            dbProvider.AddInParameter(command, helper.Mrephvisible, DbType.Int32, entity.Mrephvisible);
            dbProvider.AddInParameter(command, helper.Mrephorden, DbType.Int32, entity.Mrephorden);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SiMenureporteHojaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Mrephnombre, DbType.String, entity.Mrephnombre);
            dbProvider.AddInParameter(command, helper.Mrephestado, DbType.Int32, entity.Mrephestado);
            dbProvider.AddInParameter(command, helper.Mrephvisible, DbType.Int32, entity.Mrephvisible);
            dbProvider.AddInParameter(command, helper.Mrephorden, DbType.Int32, entity.Mrephorden);
            dbProvider.AddInParameter(command, helper.Mrepcodi, DbType.Int32, entity.Mrepcodi);

            dbProvider.AddInParameter(command, helper.Mrephcodi, DbType.Int32, entity.Mrephcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int mrephcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Mrephcodi, DbType.Int32, mrephcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SiMenureporteHojaDTO GetById(int mrephcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Mrephcodi, DbType.Int32, mrephcodi);
            SiMenureporteHojaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SiMenureporteHojaDTO> List()
        {
            List<SiMenureporteHojaDTO> entitys = new List<SiMenureporteHojaDTO>();
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

        public List<SiMenureporteHojaDTO> GetByCriteria(int tmrepcodi)
        {
            List<SiMenureporteHojaDTO> entitys = new List<SiMenureporteHojaDTO>();

            string sql = string.Format(helper.SqlGetByCriteria, tmrepcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iRepdescripcion = dr.GetOrdinal(helper.Repdescripcion);
                    if (!dr.IsDBNull(iRepdescripcion)) entity.Repdescripcion = dr.GetString(iRepdescripcion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

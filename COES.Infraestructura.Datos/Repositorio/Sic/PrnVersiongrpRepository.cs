using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnVersiongrpRepository : RepositoryBase, IPrnVersiongrpReporsitory
    {
        public PrnVersiongrpRepository(string strConn) : base(strConn) { }

        PrnVersiongrpHelper helper = new PrnVersiongrpHelper();

        public void Delete(int vergrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, vergrpcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public PrnVersiongrpDTO GetById(int vergrpcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, vergrpcodi);
            PrnVersiongrpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public PrnVersiongrpDTO GetByName(string vergrpnomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByName);

            dbProvider.AddInParameter(command, helper.Vergrpnomb, DbType.String, vergrpnomb);
            PrnVersiongrpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnVersiongrpDTO> List()
        {
            List<PrnVersiongrpDTO> entitys = new List<PrnVersiongrpDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersiongrpDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrnVersiongrpDTO> ListVersionByArea(string area)
        {
            List<PrnVersiongrpDTO> entitys = new List<PrnVersiongrpDTO>();
            string query = string.Format(helper.SqlListVersionByArea, area);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersiongrpDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void Save(PrnVersiongrpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vergrpnomb, DbType.String, entity.Vergrpnomb);
            dbProvider.AddInParameter(command, helper.Vergrpareausuaria, DbType.String, entity.Vergrpareausuaria);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnVersiongrpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vergrpnomb, DbType.String, entity.Vergrpnomb);
            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, entity.Vergrpcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int SaveGetId(PrnVersiongrpDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vergrpcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vergrpnomb, DbType.String, entity.Vergrpnomb);
            dbProvider.AddInParameter(command, helper.Vergrpareausuaria, DbType.String, entity.Vergrpareausuaria);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<PrnVersiongrpDTO> ListVersionesPronosticoPorFecha(string fechaIni, string fechaFin)
        {
            List<PrnVersiongrpDTO> entitys = new List<PrnVersiongrpDTO>();
            string query = string.Format(helper.SqlListVersionesPronosticoPorFecha,
                fechaIni,
                fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersiongrpDTO entity = new PrnVersiongrpDTO();

                    int iVergrpcodi = dr.GetOrdinal(helper.Vergrpcodi);
                    if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

                    int iVergrpnomb = dr.GetOrdinal(helper.Vergrpnomb);
                    if (!dr.IsDBNull(iVergrpnomb)) entity.Vergrpnomb = dr.GetString(iVergrpnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrnVersiongrpDTO> ListVersionByAreaFecha(
            string vergrpareausuaria, string fechaIni,
            string fechaFin)
        {
            List<PrnVersiongrpDTO> entitys = new List<PrnVersiongrpDTO>();
            string query = string.Format(helper.SqlListVersionByAreaFecha,
                vergrpareausuaria, fechaIni,
                fechaFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnVersiongrpDTO entity = new PrnVersiongrpDTO();

                    int iVergrpcodi = dr.GetOrdinal(helper.Vergrpcodi);
                    if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

                    int iVergrpnomb = dr.GetOrdinal(helper.Vergrpnomb);
                    if (!dr.IsDBNull(iVergrpnomb)) entity.Vergrpnomb = dr.GetString(iVergrpnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public PrnVersiongrpDTO GetByNameArea(string vergrpnomb,
            string area)
        {
            string query = string.Format(helper.SqlGetByNameArea,
                vergrpnomb, area);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            PrnVersiongrpDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new PrnVersiongrpDTO();

                    int iVergrpcodi = dr.GetOrdinal(helper.Vergrpcodi);
                    if (!dr.IsDBNull(iVergrpcodi)) entity.Vergrpcodi = Convert.ToInt32(dr.GetValue(iVergrpcodi));

                    int iVergrpnomb = dr.GetOrdinal(helper.Vergrpnomb);
                    if (!dr.IsDBNull(iVergrpnomb)) entity.Vergrpnomb = dr.GetString(iVergrpnomb);
                }
            }

            return entity;
        }
    }
}

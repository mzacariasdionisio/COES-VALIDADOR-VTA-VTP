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
    public class DpoFeriadosRepository : RepositoryBase, IDpoFeriadosRepository
    {
        public DpoFeriadosRepository(string strConn) : base(strConn)
        {
        }

        DpoFeriadosHelper helper = new DpoFeriadosHelper();

        public void Save(DpoFeriadosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Dpofercodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Dpoferanio, DbType.Int32, entity.Dpoferanio);
            dbProvider.AddInParameter(command, helper.Dpoferfecha, DbType.DateTime, entity.Dpoferfecha);
            dbProvider.AddInParameter(command, helper.Dpoferdescripcion, DbType.String, entity.Dpoferdescripcion);
            dbProvider.AddInParameter(command, helper.Dpoferspl, DbType.String, entity.Dpoferspl);
            dbProvider.AddInParameter(command, helper.Dpofersco, DbType.String, entity.Dpofersco);
            dbProvider.AddInParameter(command, helper.Dpoferusucreacion, DbType.String, entity.Dpoferusucreacion);
            dbProvider.AddInParameter(command, helper.Dpoferfeccreacion, DbType.DateTime, entity.Dpoferfeccreacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(DpoFeriadosDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, helper.Dpoferdescripcion, DbType.String, entity.Dpoferdescripcion);
            dbProvider.AddInParameter(command, helper.Dpoferspl, DbType.String, entity.Dpoferspl);
            dbProvider.AddInParameter(command, helper.Dpofersco, DbType.String, entity.Dpofersco);
            dbProvider.AddInParameter(command, helper.Dpofercodi, DbType.Int32, entity.Dpofercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int dpofercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Dpofercodi, DbType.Int32, dpofercodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public DpoFeriadosDTO GetById(int dpofercodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Dpofercodi, DbType.Int32, dpofercodi);
            DpoFeriadosDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<DpoFeriadosDTO> List()
        {
            List<DpoFeriadosDTO> entitys = new List<DpoFeriadosDTO>();
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
        public List<DpoFeriadosDTO> GetByAnio(int dpoferanio)
        {
            List<DpoFeriadosDTO> entitys = new List<DpoFeriadosDTO>();

            string query = string.Format(helper.SqlGetByAnhio, dpoferanio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<DpoFeriadosDTO> GetByFecha(string dpoferfecha)
        {
            List<DpoFeriadosDTO> entitys = new List<DpoFeriadosDTO>();

            string query = string.Format(helper.SqlGetByFecha, dpoferfecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public void UpdateById(int id, string descripcion, string spl, string sco, string fecha)
        {
            string query = string.Format(helper.SqlUpdateById, id, descripcion, spl, sco, fecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<DateTime> ObtenerFeriadosSpl()
        {
            List<DateTime> entities = new List<DateTime>();

            string query = string.Format(helper.SqlObtenerFeriadosSpl);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iDpoferfecha = dr.GetOrdinal(this.helper.Dpoferfecha);
                    if (!dr.IsDBNull(iDpoferfecha)) entities.Add(dr.GetDateTime(iDpoferfecha));
                }
            }

            return entities;
        }

        public List<DateTime> ObtenerFeriadosSco()
        {
            List<DateTime> entities = new List<DateTime>();

            string query = string.Format(helper.SqlObtenerFeriadosSco);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iDpoferfecha = dr.GetOrdinal(this.helper.Dpoferfecha);
                    if (!dr.IsDBNull(iDpoferfecha)) entities.Add(dr.GetDateTime(iDpoferfecha));
                }
            }

            return entities;
        }

      

        public List<DateTime> ObtenerFeriadosPorAnio(int dpoferanio)
        {
            List<DateTime> entitys = new List<DateTime>();

            string query = string.Format(helper.SqlObtenerFeriadosPorAnio, dpoferanio);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iDpoferfecha = dr.GetOrdinal(this.helper.Dpoferfecha);
                    if (!dr.IsDBNull(iDpoferfecha)) entitys.Add(dr.GetDateTime(iDpoferfecha));
                }
            }

            return entitys;
        }

        public List<DpoFeriadosDTO> GetByAnioRango(int anioIni, int anioFin)
        {
            List<DpoFeriadosDTO> entitys = new List<DpoFeriadosDTO>();

            string query = string.Format(helper.SqlGetByAnioRango, anioIni, anioFin);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

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

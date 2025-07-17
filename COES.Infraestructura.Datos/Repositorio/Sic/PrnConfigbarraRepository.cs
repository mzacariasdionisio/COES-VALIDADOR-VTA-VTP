using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Dominio.DTO.Sic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnConfigbarraRepository : RepositoryBase, IPrnConfigbarraRepository
    {
        public PrnConfigbarraRepository(string strConn) : base(strConn)
        {
        }

        PrnConfigbarraHelper helper = new PrnConfigbarraHelper();

        public void Save(PrnConfigbarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cfgbarfecha, DbType.DateTime, entity.Cfgbarfecha);
            dbProvider.AddInParameter(command, helper.Cfgbarpse, DbType.Decimal, entity.Cfgbarpse);
            dbProvider.AddInParameter(command, helper.Cfgbarfactorf, DbType.Decimal, entity.Cfgbarfactorf);
            dbProvider.AddInParameter(command, helper.Cfgbarusucreacion, DbType.String, entity.Cfgbarusucreacion);
            dbProvider.AddInParameter(command, helper.Cfgbarfeccreacion, DbType.DateTime, entity.Cfgbarfeccreacion);
            dbProvider.AddInParameter(command, helper.Cfgbarusumodificacion, DbType.String, entity.Cfgbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cfgbarfecmodificacion, DbType.DateTime, entity.Cfgbarfecmodificacion);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnConfigbarraDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            
            dbProvider.AddInParameter(command, helper.Cfgbarpse, DbType.Decimal, entity.Cfgbarpse);
            dbProvider.AddInParameter(command, helper.Cfgbarfactorf, DbType.Decimal, entity.Cfgbarfactorf);
            dbProvider.AddInParameter(command, helper.Cfgbarusucreacion, DbType.String, entity.Cfgbarusucreacion);
            dbProvider.AddInParameter(command, helper.Cfgbarfeccreacion, DbType.DateTime, entity.Cfgbarfeccreacion);
            dbProvider.AddInParameter(command, helper.Cfgbarusumodificacion, DbType.String, entity.Cfgbarusumodificacion);
            dbProvider.AddInParameter(command, helper.Cfgbarfecmodificacion, DbType.DateTime, entity.Cfgbarfecmodificacion);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Cfgbarfecha, DbType.DateTime, entity.Cfgbarfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grupocodi, DateTime cfgbarfecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Cfgbarfecha, DbType.DateTime, cfgbarfecha);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnConfigbarraDTO> List()
        {
            List<PrnConfigbarraDTO> entitys = new List<PrnConfigbarraDTO>();
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

        public PrnConfigbarraDTO GetById(int grupocodi, DateTime cfgbarfecha)
        {
            PrnConfigbarraDTO entity = new PrnConfigbarraDTO();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Cfgbarfecha, DbType.DateTime, cfgbarfecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PrnConfigbarraDTO> ParametrosList(string fecdesde, string fechasta, string lstbarras)
        {
            List<PrnConfigbarraDTO> entitys = new List<PrnConfigbarraDTO>();
            string query = String.Format(helper.SqlParametrosList, lstbarras, fecdesde, fechasta);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrnConfigbarraDTO entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PrnConfigbarraDTO GetConfiguracion(int grupocodi, string fecha, int defid, string deffecha)
        {
            PrnConfigbarraDTO entity = new PrnConfigbarraDTO();
            string query = String.Format(helper.SqlGetConfiguracion, grupocodi, fecha, defid, deffecha);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }
    }
}

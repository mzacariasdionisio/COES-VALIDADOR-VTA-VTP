using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Dominio.DTO.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using COES.Dominio.Interfaces.Sic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PrnAreamedicionRepository : RepositoryBase, IPrnAreamedicionRepository
    {
        public PrnAreamedicionRepository(string strConn)
            : base(strConn)
        {
        }

        PrnAreamedicionHelper helper = new PrnAreamedicionHelper();

        public void Save(PrnAreamedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Areamedcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Areamedfecinicial, DbType.DateTime, entity.Areamedfecinicial);
            dbProvider.AddInParameter(command, helper.Areamedfecfinal, DbType.DateTime, entity.Areamedfecfinal);
            dbProvider.AddInParameter(command, helper.Areamedestado, DbType.String, entity.Areamedestado);
            dbProvider.AddInParameter(command, helper.Areamedfeccreacion, DbType.DateTime, entity.Areamedfeccreacion);
            dbProvider.AddInParameter(command, helper.Areamedestado, DbType.String, entity.Areamedestado);
            dbProvider.AddInParameter(command, helper.Areamedfecmodificacion, DbType.DateTime, entity.Areamedfecmodificacion);
            dbProvider.AddInParameter(command, helper.Areamedusumodificacion, DbType.String, entity.Areamedusumodificacion);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(PrnAreamedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Areamedfecinicial, DbType.DateTime, entity.Areamedfecinicial);
            dbProvider.AddInParameter(command, helper.Areamedfecfinal, DbType.DateTime, entity.Areamedfecfinal);
            dbProvider.AddInParameter(command, helper.Areamedestado, DbType.String, entity.Areamedestado);
            dbProvider.AddInParameter(command, helper.Areamedfecmodificacion, DbType.DateTime, entity.Areamedfecmodificacion);
            dbProvider.AddInParameter(command, helper.Areamedusumodificacion, DbType.String, entity.Areamedusumodificacion);

            dbProvider.AddInParameter(command, helper.Areamedcodi, DbType.Int32, entity.Areamedcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int aremedcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Areamedcodi, DbType.Int32, aremedcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrnAreamedicionDTO> List()
        {
            List<PrnAreamedicionDTO> entitys = new List<PrnAreamedicionDTO>();
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

        public List<PrnAreamedicionDTO> ListVarexoCiudad()
        {
            PrnAreamedicionDTO entity = new PrnAreamedicionDTO();
            List<PrnAreamedicionDTO> entitys = new List<PrnAreamedicionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListVarexoCiudad);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrnAreamedicionDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iAremedestado = dr.GetOrdinal(helper.Areamedestado);
                    if (!dr.IsDBNull(iAremedestado)) entity.Areamedestado = dr.GetString(iAremedestado);

                    int iAremedcodi = dr.GetOrdinal(helper.Areamedcodi);
                    if (!dr.IsDBNull(iAremedcodi)) entity.Areamedcodi = Convert.ToInt32(dr.GetValue(iAremedcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void UpdateEstado(PrnAreamedicionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);

            dbProvider.AddInParameter(command, helper.Areamedestado, DbType.String, entity.Areamedestado);
            dbProvider.AddInParameter(command, helper.Areamedfecmodificacion, DbType.DateTime, entity.Areamedfecmodificacion);
            dbProvider.AddInParameter(command, helper.Areamedusumodificacion, DbType.String, entity.Areamedusumodificacion);

            dbProvider.AddInParameter(command, helper.Areamedcodi, DbType.Int32, entity.Areamedcodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}

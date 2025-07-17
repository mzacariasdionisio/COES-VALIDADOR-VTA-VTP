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
    /// Clase de acceso a datos de la tabla SEG_COORDENADA
    /// </summary>
    public class SegCoordenadaRepository: RepositoryBase, ISegCoordenadaRepository
    {
        public SegCoordenadaRepository(string strConn): base(strConn)
        {
        }

        SegCoordenadaHelper helper = new SegCoordenadaHelper();

        public int Save(SegCoordenadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Segcocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Segconro, DbType.Int32, entity.Segconro);
            dbProvider.AddInParameter(command, helper.Segcoflujo1, DbType.Decimal, entity.Segcoflujo1);
            dbProvider.AddInParameter(command, helper.Segcoflujo2, DbType.Decimal, entity.Segcoflujo2);
            dbProvider.AddInParameter(command, helper.Segcogener1, DbType.Decimal, entity.Segcogener1);
            dbProvider.AddInParameter(command, helper.Segcogener2, DbType.Decimal, entity.Segcogener2);
            dbProvider.AddInParameter(command, helper.Zoncodi, DbType.Int32, entity.Zoncodi);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi); //Movisoft 2022-04-26
            dbProvider.AddInParameter(command, helper.Segcousucreacion, DbType.String, entity.Segcousucreacion);
            dbProvider.AddInParameter(command, helper.Segcousumodificacion, DbType.String, entity.Segcousumodificacion);
            dbProvider.AddInParameter(command, helper.Segcofeccreacion, DbType.DateTime, entity.Segcofeccreacion);
            dbProvider.AddInParameter(command, helper.Segcofecmodificacion, DbType.DateTime, entity.Segcofecmodificacion);
            dbProvider.AddInParameter(command, helper.Segcoestado, DbType.String, entity.Segcoestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(SegCoordenadaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Segconro, DbType.Int32, entity.Segconro);
            dbProvider.AddInParameter(command, helper.Segcoflujo1, DbType.Decimal, entity.Segcoflujo1);
            dbProvider.AddInParameter(command, helper.Segcoflujo2, DbType.Decimal, entity.Segcoflujo2);
            dbProvider.AddInParameter(command, helper.Segcogener1, DbType.Decimal, entity.Segcogener1);
            dbProvider.AddInParameter(command, helper.Segcogener2, DbType.Decimal, entity.Segcogener2);
            dbProvider.AddInParameter(command, helper.Zoncodi, DbType.Int32, entity.Zoncodi);
            dbProvider.AddInParameter(command, helper.Regcodi, DbType.Int32, entity.Regcodi);
            dbProvider.AddInParameter(command, helper.Segcotipo, DbType.Int32, entity.Segcotipo);
            dbProvider.AddInParameter(command, helper.Regsegcodi, DbType.Int32, entity.Regsegcodi);
            dbProvider.AddInParameter(command, helper.Segcousumodificacion, DbType.String, entity.Segcousumodificacion);
            dbProvider.AddInParameter(command, helper.Segcofecmodificacion, DbType.DateTime, entity.Segcofecmodificacion);
            dbProvider.AddInParameter(command, helper.Segcoestado, DbType.String, entity.Segcoestado);
            dbProvider.AddInParameter(command, helper.Segcocodi, DbType.Int32, entity.Segcocodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int segcocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);
            dbProvider.AddInParameter(command, helper.Segcocodi, DbType.Int32, segcocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public SegCoordenadaDTO GetById( int segcocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Segcocodi, DbType.Int32, segcocodi);
            SegCoordenadaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<SegCoordenadaDTO> List()
        {
            List<SegCoordenadaDTO> entitys = new List<SegCoordenadaDTO>();
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

        public List<SegCoordenadaDTO> GetByCriteria(int regcodi, int idtipo)
        {
            List<SegCoordenadaDTO> entitys = new List<SegCoordenadaDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, regcodi, idtipo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SegCoordenadaDTO entity = helper.Create(dr);

                    int iZonnombre = dr.GetOrdinal(helper.Zonnombre);
                    if (!dr.IsDBNull(iZonnombre)) entity.Zonnombre = dr.GetString(iZonnombre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SegCoordenadaDTO> Totalrestriccion()
        {
            List<SegCoordenadaDTO> entitys = new List<SegCoordenadaDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlTotalrestriccion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SegCoordenadaDTO entity = new SegCoordenadaDTO();

                    int iTotalrestriccion = dr.GetOrdinal(helper.Totalrestriccion);
                    if (!dr.IsDBNull(iTotalrestriccion)) entity.Totalrestriccion = Convert.ToInt32(dr.GetValue(iTotalrestriccion));
                    int iRegcodi = dr.GetOrdinal(helper.Regcodi);
                    if (!dr.IsDBNull(iRegcodi)) entity.Regcodi = Convert.ToInt32(dr.GetValue(iRegcodi));
                    int iSegcotipo = dr.GetOrdinal(helper.Segcotipo);
                    if (!dr.IsDBNull(iSegcotipo)) entity.Segcotipo = Convert.ToInt32(dr.GetValue(iSegcotipo));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

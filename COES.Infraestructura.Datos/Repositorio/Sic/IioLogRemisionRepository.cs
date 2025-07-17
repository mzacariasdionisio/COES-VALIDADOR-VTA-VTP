using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla IIO_LOG_REMISION
    /// </summary>
    public class IioLogRemisionRepository : RepositoryBase, IIioLogRemisionRepository
    {
        private readonly IioLogRemisionHelper helper = new IioLogRemisionHelper();

        public IioLogRemisionRepository(string strConn) : base(strConn)
        {
            
        }

        public List<IioLogRemisionDTO> List(IioControlCargaDTO scoControlCargaDto, int minRowToFetch, int maxRowToFetch)
        {
            var entities = new List<IioLogRemisionDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, scoControlCargaDto.RccaCodi);
            dbProvider.AddInParameter(command, IioControlCargaHelper.Enviocodi, DbType.Int32, scoControlCargaDto.Enviocodi);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    var entity = helper.Create(dataReader);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public int Save(IioLogRemisionDTO scoLogRemisionDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            var result = dbProvider.ExecuteScalar(command);
            int id = (result != null ? Convert.ToInt32(result) : 1);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, IioLogRemisionHelper.RlogCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, scoLogRemisionDto.RccaCodi);
            dbProvider.AddInParameter(command, IioLogRemisionHelper.RlogNroLinea, DbType.Int32, scoLogRemisionDto.RlogNroLinea);
            dbProvider.AddInParameter(command, IioLogRemisionHelper.RlogDescripcionError, DbType.String, scoLogRemisionDto.RlogDescripcionError);
            dbProvider.AddInParameter(command, IioLogRemisionHelper.Enviocodi, DbType.Int32, scoLogRemisionDto.Enviocodi);
            dbProvider.ExecuteNonQuery(command);

            return id;
        }

        public IioLogRemisionDTO GetById(IioLogRemisionDTO scoLogRemisionDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, IioLogRemisionHelper.RlogCodi, DbType.Int32, 
                scoLogRemisionDto.RlogCodi);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return (dataReader.Read() ? helper.Create(dataReader) : null);
            }
        }

        public void Delete(int rccacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, rccacodi);

            dbProvider.ExecuteNonQuery(command);
        }

    }
}
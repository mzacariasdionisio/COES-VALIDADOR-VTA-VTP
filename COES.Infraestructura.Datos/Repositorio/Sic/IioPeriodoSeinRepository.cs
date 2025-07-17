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
    /// Clase de acceso a datos de la tabla IIO_PERIODO_SEIN
    /// </summary>
    public class IioPeriodoSeinRepository : RepositoryBase, IIioPeriodoSeinRepository
    {
        private readonly IioPeriodoSeinHelper helper = new IioPeriodoSeinHelper();

        public IioPeriodoSeinRepository(string strConn) : base(strConn)
        {
            
        }

        public List<IioPeriodoSeinDTO> GetByCriteria(string anio)
        {
            var entities = new List<IioPeriodoSeinDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.Anio, DbType.String, anio);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    entities.Add(helper.Create(dataReader));
                }
            }
            return entities;
        }

        public IioPeriodoSeinDTO GetById(string periodo)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinAnioMesPerrem, DbType.String, periodo);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                return (dataReader.Read() ? helper.Create(dataReader) : null);
            }
        }

        public void Save(IioPeriodoSeinDTO iioPeriodoSeinDto)
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlInsert);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinAnioMesPerrem, DbType.String, iioPeriodoSeinDto.PseinAnioMesPerrem);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinConfirmado, DbType.String, iioPeriodoSeinDto.PseinConfirmado);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinEstado, DbType.String, iioPeriodoSeinDto.PseinEstado);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinEstRegistro, DbType.String, iioPeriodoSeinDto.PseinEstRegistro);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinUsuCreacion, DbType.String, iioPeriodoSeinDto.PseinUsuCreacion);
            dbProvider.ExecuteNonQuery(command);
        }
        
        public List<string> ListAnios()
        {
            var anios = new List<string>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlListAnios);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    anios.Add(dataReader.GetString(0));
                }
            }
            return anios;
        }

        public void Update(IioPeriodoSeinDTO iioPeriodoSeinDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);            
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinAnioMesPerrem, DbType.String, iioPeriodoSeinDto.PseinAnioMesPerrem);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinFecPriEnvio, DbType.DateTime, iioPeriodoSeinDto.PseinFecPriEnvio);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinFecUltEnvio, DbType.DateTime, iioPeriodoSeinDto.PseinFecUltEnvio);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinEstado, DbType.String, iioPeriodoSeinDto.PseinEstado);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinEstRegistro, DbType.String, iioPeriodoSeinDto.PseinEstRegistro);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinUsuModificacion, DbType.String, iioPeriodoSeinDto.PseinUsuModificacion);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.Int32, iioPeriodoSeinDto.PseinCodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}
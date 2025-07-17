using System;
using System.Collections.Generic;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SCO_CONTROL_CARGA
    /// </summary>
    public class IioControlCargaRepository : RepositoryBase, IIioControlCargaRepository
    {
        private readonly IioControlCargaHelper helper = new IioControlCargaHelper();

        public IioControlCargaRepository(string strConn)
            : base(strConn)
        {

        }

        public void Update(IioControlCargaDTO iioControlCargaDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.String, iioControlCargaDto.PseinCodi);
            dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, iioControlCargaDto.RtabCodi);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaNroRegistros, DbType.Int32, iioControlCargaDto.RccaNroRegistros);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaFecHorEnvio, DbType.DateTime, iioControlCargaDto.RccaFecHorEnvio ?? DateTime.Now);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaEstadoEnvio, DbType.String, iioControlCargaDto.RccaEstadoEnvio);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaEstRegistro, DbType.String, iioControlCargaDto.RccaEstRegistro);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaUsuModificacion, DbType.String, iioControlCargaDto.RccaUsuModificacion);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaFecModificacion, DbType.DateTime, iioControlCargaDto.RccaFecModificacion ?? DateTime.Now);
            dbProvider.AddInParameter(command, IioControlCargaHelper.Enviocodi, DbType.Int32, iioControlCargaDto.Enviocodi);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, iioControlCargaDto.RccaCodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(IioControlCargaDTO iioControlCargaDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            var result = dbProvider.ExecuteScalar(command);
            int id = (result != null ? Convert.ToInt32(result) : 1);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.String, iioControlCargaDto.PseinCodi);
            dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, iioControlCargaDto.RtabCodi);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaNroRegistros, DbType.Int32, iioControlCargaDto.RccaNroRegistros);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaFecHorEnvio, DbType.DateTime, iioControlCargaDto.RccaFecHorEnvio ?? DateTime.Now);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaEstadoEnvio, DbType.String, iioControlCargaDto.RccaEstadoEnvio);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaEstRegistro, DbType.String, iioControlCargaDto.RccaEstRegistro);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaUsuCreacion, DbType.String, iioControlCargaDto.RccaUsuCreacion);
            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaFecCreacion, DbType.DateTime, iioControlCargaDto.RccaFecCreacion ?? DateTime.Now);
            dbProvider.AddInParameter(command, IioControlCargaHelper.Enviocodi, DbType.Int32, iioControlCargaDto.Enviocodi);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        /// <summary>
        /// Devuelve un registro de la tabla IioControlCargaDTO en base al criterio de los parametros ingresados. 
        /// En caso el registro no exista, devuelve una nueva instancia del objeto IioControlCargaDTO (es decir, con sus valores por default).
        /// </summary>
        /// <param name="iioControlCargaDto"></param>
        /// <returns></returns>
        public IioControlCargaDTO GetByCriteria(IioControlCargaDTO iioControlCargaDto)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.Int32, iioControlCargaDto.PseinCodi);
            dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, iioControlCargaDto.RtabCodi);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                IioControlCargaDTO iioControlCargaDTO = new IioControlCargaDTO();
                iioControlCargaDTO = (dataReader.Read() ? helper.Create(dataReader) : new IioControlCargaDTO());
                return iioControlCargaDTO;
            }
        }

        public IioControlCargaDTO GetById(int rccaCodi)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, IioControlCargaHelper.RccaCodi, DbType.Int32, rccaCodi);
            IioControlCargaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        #region SIOSEIN-PRIE-2021
        /// <summary>
        /// Devuelve un listado de la tabla IIO_CONTROL_CARGA en base al periodo.
        /// </summary>
        /// <param name="PseinCodi"></param>
        /// <returns></returns>
        public List<IioControlCargaDTO> GetByPeriodo(int PseinCodi)
        {
            var entities = new List<IioControlCargaDTO>();
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodo);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.String, PseinCodi);

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
        #endregion

        public List<IioControlCargaDTO> GetByCriteriaXTabla(int pseinCodi, string rtabcodi)
        {
            var entities = new List<IioControlCargaDTO>();

            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, IioPeriodoSeinHelper.PseinCodi, DbType.Int32, pseinCodi);
            dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, rtabcodi);

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
    }
}
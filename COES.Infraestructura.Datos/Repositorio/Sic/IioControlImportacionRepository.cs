using System;
using System.Collections.Generic;
using System.Data;
using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla SCO_CONTROL_IMPORTACION
    /// </summary>
    public class IioControlImportacionRepository : RepositoryBase, IIioControlImportacionRepository
    {
        private readonly IioControlImportacionHelper helper = new IioControlImportacionHelper();

        public IioControlImportacionRepository(string strConn) : base(strConn)
        {
            
        }

        public void BulkInsert(List<IioControlImportacionDTO> entitys)
        {
            dbProvider.AddColumnMapping(helper.RcimCodi, DbType.Int32);
            dbProvider.AddColumnMapping(IioPeriodoSicliHelper.PsicliCodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.RtabCodi, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimNroRegistros, DbType.Int32);
            dbProvider.AddColumnMapping(helper.RcimFecHorImportacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.RcimEstadoImportacion, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimEstRegistro, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimUsuCreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimFecCreacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.RcimUsuModificacion, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimFecModificacion, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.RcimNroRegistrosCoes, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Enviocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.RcimEmpresa, DbType.String);
            dbProvider.AddColumnMapping(helper.RcimEmpresaDesc, DbType.String);
                                   
            dbProvider.BulkInsert<IioControlImportacionDTO>(entitys, helper.TableName);
        }

        public void Update(IioControlImportacionDTO iioControlImportacionDTO)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.String, iioControlImportacionDTO.Psiclicodi);
            dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, iioControlImportacionDTO.Rtabcodi.Trim());
            dbProvider.AddInParameter(command, helper.RcimNroRegistros, DbType.Int32, iioControlImportacionDTO.Rcimnroregistros);
            dbProvider.AddInParameter(command, helper.RcimNroRegistrosCoes, DbType.Int32, iioControlImportacionDTO.Rcimnroregistroscoes);
            dbProvider.AddInParameter(command, helper.RcimEstadoImportacion, DbType.String, iioControlImportacionDTO.Rcimestadoimportacion);
            dbProvider.AddInParameter(command, helper.RcimEstRegistro, DbType.String, iioControlImportacionDTO.Rcimestregistro);
            dbProvider.AddInParameter(command, helper.RcimUsuModificacion, DbType.String, iioControlImportacionDTO.Rcimusumodificacion);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, iioControlImportacionDTO.Enviocodi);
            dbProvider.AddInParameter(command, helper.RcimEmpresa, DbType.String, iioControlImportacionDTO.Rcimempresa);
            dbProvider.AddInParameter(command, helper.RcimCodi, DbType.Int32, iioControlImportacionDTO.Rcimcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public int Save(IioControlImportacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            //var command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //var result = dbProvider.ExecuteScalar(command);
            //int id = (result != null ? Convert.ToInt32(result) : 1);

            //command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            //dbProvider.AddInParameter(command, helper.RcimCodi, DbType.Int32, id);
            //dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.String, iioControlImportacionDTO.Psiclicodi);
            //dbProvider.AddInParameter(command, IioTablaSyncHelper.RtabCodi, DbType.String, iioControlImportacionDTO.Rtabcodi);
            //dbProvider.AddInParameter(command, helper.RcimNroRegistros, DbType.Int32, iioControlImportacionDTO.Rcimnroregistros);
            //dbProvider.AddInParameter(command, helper.RcimNroRegistrosCoes, DbType.Int32, iioControlImportacionDTO.Rcimnroregistroscoes);
            //dbProvider.AddInParameter(command, helper.RcimEstadoImportacion, DbType.String, iioControlImportacionDTO.Rcimestadoimportacion);
            //dbProvider.AddInParameter(command, helper.RcimEstRegistro, DbType.String, iioControlImportacionDTO.Rcimestregistro);
            //dbProvider.AddInParameter(command, helper.RcimUsuCreacion, DbType.String, iioControlImportacionDTO.Rcimusucreacion);
            //dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, iioControlImportacionDTO.Enviocodi);
            //dbProvider.AddInParameter(command, helper.RcimEmpresa, DbType.Int32, iioControlImportacionDTO.Rcimempresa);
            //dbProvider.AddInParameter(command, helper.RcimEmpresaDesc, DbType.Int32, iioControlImportacionDTO.Rcimempresadesc);
            //dbProvider.ExecuteNonQuery(command);
            //return id;

            dbProvider.AddInParameter(command, helper.RcimCodi, DbType.Int32, entity.Rcimcodi);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, entity.Psiclicodi);
            dbProvider.AddInParameter(command, helper.RtabCodi, DbType.String, entity.Rtabcodi);
            dbProvider.AddInParameter(command, helper.RcimNroRegistros, DbType.Int32, entity.Rcimnroregistros);
            dbProvider.AddInParameter(command, helper.RcimNroRegistrosCoes, DbType.Int32,entity.Rcimnroregistroscoes);
            dbProvider.AddInParameter(command, helper.RcimFecHorImportacion, DbType.DateTime, entity.Rcimfechorimportacion);
            dbProvider.AddInParameter(command, helper.RcimEstadoImportacion, DbType.String, entity.Rcimestadoimportacion);
            dbProvider.AddInParameter(command, helper.RcimEstRegistro, DbType.String, entity.Rcimestregistro);
            dbProvider.AddInParameter(command, helper.RcimUsuCreacion, DbType.String, entity.Rcimusucreacion);
            dbProvider.AddInParameter(command, helper.RcimFecCreacion, DbType.DateTime, entity.Rcimfeccreacion);
            dbProvider.AddInParameter(command, helper.RcimUsuModificacion, DbType.String, entity.Rcimusumodificacion);
            dbProvider.AddInParameter(command, helper.RcimFecModificacion, DbType.DateTime, entity.Rcimfecmodificacion);
            dbProvider.AddInParameter(command, helper.Enviocodi, DbType.Int32, entity.Enviocodi);
            dbProvider.AddInParameter(command, helper.RcimEmpresa, DbType.String, entity.Rcimempresa);
            dbProvider.AddInParameter(command, helper.RcimEmpresaDesc, DbType.String, entity.Rcimempresadesc);

            dbProvider.ExecuteNonQuery(command);

            return 0;
        }

        public IioControlImportacionDTO GetByCriteria(IioControlImportacionDTO iioControlImportacionDTO)
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, iioControlImportacionDTO.Psiclicodi);
            dbProvider.AddInParameter(command, helper.RcimEmpresa, DbType.String, iioControlImportacionDTO.Rcimempresa);

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                IioControlImportacionDTO iioControl = new IioControlImportacionDTO();
                iioControl = (dataReader.Read() ? helper.Create(dataReader) : new IioControlImportacionDTO());
                return iioControl;
            }
        }


        public int GetCantidadRegistros(int periodo)
        {
            int cant = 0;
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetByPeriodo);
            dbProvider.AddInParameter(command, IioPeriodoSicliHelper.PsicliCodi, DbType.Int32, periodo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    cant = dr.GetInt32(dr.GetOrdinal("cantidad"));
                }
            }

            return cant;
        }

        public int GetMaxId()
        {
            var command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            var result = dbProvider.ExecuteScalar(command);
            int id = (result != null ? Convert.ToInt32(result) : 1);

            return id;
        }


        public List<IioControlImportacionDTO> ListByTabla(int periodo, string tabla)
        {
            var entities = new List<IioControlImportacionDTO>();

            string queryString = string.Format(helper.SqlListByTabla, periodo, tabla);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

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

        public IioControlImportacionDTO GetByEmpresaTabla(int periodo, string tabla, string empresa)
        {
            string queryString = string.Format(helper.SqlGetByEmpresaTabla, periodo, tabla, empresa);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.ExecuteNonQuery(command);

            IioControlImportacionDTO entity = new IioControlImportacionDTO();

            using (var dataReader = dbProvider.ExecuteReader(command))
            {
                while (dataReader.Read())
                {
                    entity = helper.Create(dataReader);
                }
            }

            return entity;
        }
       

    }
}
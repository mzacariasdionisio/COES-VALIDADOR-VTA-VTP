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
    /// Clase de acceso a datos de la tabla AUD_PROGRAMAAUDITORIA
    /// </summary>
    public class AudProgramaauditoriaRepository: RepositoryBase, IAudProgramaauditoriaRepository
    {
        public AudProgramaauditoriaRepository(string strConn): base(strConn)
        {
        }

        AudProgramaauditoriaHelper helper = new AudProgramaauditoriaHelper();

        public int Save(AudProgramaauditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, entity.Audicodi);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoactividad, DbType.Int32, entity.Tabcdcoditipoactividad);
            dbProvider.AddInParameter(command, helper.Progafecha, DbType.DateTime, entity.Progafecha);
            dbProvider.AddInParameter(command, helper.Progahorainicio, DbType.String, entity.Progahorainicio);
            dbProvider.AddInParameter(command, helper.Progahorafin, DbType.String, entity.Progahorafin);
            dbProvider.AddInParameter(command, helper.Progaactivo, DbType.String, entity.Progaactivo);
            dbProvider.AddInParameter(command, helper.Progahistorico, DbType.String, entity.Progahistorico);
            dbProvider.AddInParameter(command, helper.Progausucreacion, DbType.String, entity.Progausucreacion);
            dbProvider.AddInParameter(command, helper.Progafeccreacion, DbType.DateTime, entity.Progafeccreacion);
            dbProvider.AddInParameter(command, helper.Progausumodificacion, DbType.String, entity.Progausumodificacion);
            dbProvider.AddInParameter(command, helper.Progafecmodificacion, DbType.DateTime, entity.Progafecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestadoactividad, DbType.Int32, entity.Tabcdcodiestadoactividad);
            
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(AudProgramaauditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, entity.Audicodi);
            dbProvider.AddInParameter(command, helper.Tabcdcoditipoactividad, DbType.Int32, entity.Tabcdcoditipoactividad);
            dbProvider.AddInParameter(command, helper.Progafecha, DbType.DateTime, entity.Progafecha);
            dbProvider.AddInParameter(command, helper.Progahorainicio, DbType.String, entity.Progahorainicio);
            dbProvider.AddInParameter(command, helper.Progahorafin, DbType.String, entity.Progahorafin);
            dbProvider.AddInParameter(command, helper.Progahistorico, DbType.String, entity.Progahistorico);
            dbProvider.AddInParameter(command, helper.Progausumodificacion, DbType.String, entity.Progausumodificacion);
            dbProvider.AddInParameter(command, helper.Progafecmodificacion, DbType.DateTime, entity.Progafecmodificacion);
            dbProvider.AddInParameter(command, helper.Tabcdcodiestadoactividad, DbType.Int32, entity.Tabcdcodiestadoactividad);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(AudProgramaauditoriaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Progausumodificacion, DbType.String, entity.Progausumodificacion);
            dbProvider.AddInParameter(command, helper.Progafecmodificacion, DbType.DateTime, entity.Progafecmodificacion);
            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, entity.Progacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public AudProgramaauditoriaDTO GetById(int progacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Progacodi, DbType.Int32, progacodi);
            AudProgramaauditoriaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoelementocodi = dr.GetOrdinal(helper.Tipoelementocodi);
                    if (!dr.IsDBNull(iTipoelementocodi)) entity.Tipoelementocodi = Convert.ToInt32(dr.GetValue(iTipoelementocodi));

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iTabcdcodiestadoactividad = dr.GetOrdinal(helper.Tabcdcodiestadoactividad);
                    if (!dr.IsDBNull(iTabcdcodiestadoactividad)) entity.Tabcdcodiestadoactividad = Convert.ToInt32(dr.GetValue(iTabcdcodiestadoactividad));
                    
                }
            }

            return entity;
        }

        public List<AudProgramaauditoriaDTO> List()
        {
            List<AudProgramaauditoriaDTO> entitys = new List<AudProgramaauditoriaDTO>();
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

        public List<AudProgramaauditoriaDTO> GetByCriteria(int audicodi)
        {
            List<AudProgramaauditoriaDTO> entitys = new List<AudProgramaauditoriaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Audicodi, DbType.Int32, audicodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgramaauditoriaDTO entity = helper.Create(dr);

                    int iTipoactividad = dr.GetOrdinal(helper.Tipoactividad);
                    if (!dr.IsDBNull(iTipoactividad)) entity.Tipoactividad = dr.GetString(iTipoactividad);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    //int iProgaecodi = dr.GetOrdinal(helper.Progaecodi);
                    //if (!dr.IsDBNull(iProgaecodi)) entity.Progaecodi = Convert.ToInt32(dr.GetValue(iProgaecodi));
                    
                    //int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    //if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

                    //int iElemcodi = dr.GetOrdinal(helper.Elemcodi);
                    //if (!dr.IsDBNull(iElemcodi)) entity.Elemcod = Convert.ToInt32(dr.GetValue(iElemcodi));

                    int iEquipo = dr.GetOrdinal(helper.Equipo);
                    if (!dr.IsDBNull(iEquipo)) entity.Equipo = dr.GetString(iEquipo);

                    int iResponsables = dr.GetOrdinal(helper.Responsables);
                    if (!dr.IsDBNull(iResponsables)) entity.Responsables = dr.GetString(iResponsables);

                    int iTabcdcodiestadoactividad = dr.GetOrdinal(helper.Tabcdcodiestadoactividad);
                    if (!dr.IsDBNull(iTabcdcodiestadoactividad)) entity.Tabcdcodiestadoactividad = Convert.ToInt32(dr.GetValue(iTabcdcodiestadoactividad));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<AudProgramaauditoriaDTO> GetByCriteriaEjecucion(AudProgramaauditoriaDTO programa)
        {
            List<AudProgramaauditoriaDTO> entitys = new List<AudProgramaauditoriaDTO>();

            string sql = string.Format(helper.SqlGetByCriteriaEjecucion, programa.Audicodi, programa.Tabcdcodiestadoactividad, programa.Equipo);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    AudProgramaauditoriaDTO entity = helper.Create(dr);

                    int iTipoactividad = dr.GetOrdinal(helper.Tipoactividad);
                    if (!dr.IsDBNull(iTipoactividad)) entity.Tipoactividad = dr.GetString(iTipoactividad);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iElemdescripcion = dr.GetOrdinal(helper.Elemdescripcion);
                    if (!dr.IsDBNull(iElemdescripcion)) entity.Elemdescripcion = dr.GetString(iElemdescripcion);

                    int iProgaecodi = dr.GetOrdinal(helper.Progaecodi);
                    if (!dr.IsDBNull(iProgaecodi)) entity.Progaecodi = Convert.ToInt32(dr.GetValue(iProgaecodi));

                    int iElemcodigo = dr.GetOrdinal(helper.Elemcodigo);
                    if (!dr.IsDBNull(iElemcodigo)) entity.Elemcodigo = dr.GetString(iElemcodigo);

                    int iElemcodi = dr.GetOrdinal(helper.Elemcodi);
                    if (!dr.IsDBNull(iElemcodi)) entity.Elemcod = Convert.ToInt32(dr.GetValue(iElemcodi));

                    int iEquipo = dr.GetOrdinal(helper.Equipo);
                    if (!dr.IsDBNull(iEquipo)) entity.Equipo = dr.GetString(iEquipo);

                    int iResponsables = dr.GetOrdinal(helper.Responsables);
                    if (!dr.IsDBNull(iResponsables)) entity.Responsables = dr.GetString(iResponsables);

                    int iTabcdcodiestadoactividad = dr.GetOrdinal(helper.Tabcdcodiestadoactividad);
                    if (!dr.IsDBNull(iTabcdcodiestadoactividad)) entity.Tabcdcodiestadoactividad = Convert.ToInt32(dr.GetValue(iTabcdcodiestadoactividad));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

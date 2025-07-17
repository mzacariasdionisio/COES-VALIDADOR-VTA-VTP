using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPR_AREA_
    /// </summary>
    public class EprAreaRepository : RepositoryBase, IEprAreaRepository
    {
        public EprAreaRepository(string strConn) : base(strConn)
        {
        }

        EprAreaHelper helper = new EprAreaHelper();

        public int Save(EprAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Epareacodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Areacodizona, DbType.String, entity.Areacodizona);
            dbProvider.AddInParameter(command, helper.Epareanomb, DbType.String, entity.Epareanomb);
            dbProvider.AddInParameter(command, helper.Epareaestregistro, DbType.String, entity.Epareaestregistro);
            dbProvider.AddInParameter(command, helper.Epareausucreacion, DbType.String, entity.Epareausucreacion);
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EprAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Areacodizona, DbType.Int32, entity.Areacodizona);
            dbProvider.AddInParameter(command, helper.Epareanomb, DbType.String, entity.Epareanomb);
            dbProvider.AddInParameter(command, helper.Epareaestregistro, DbType.String, entity.Epareaestregistro);
            dbProvider.AddInParameter(command, helper.Epareausumodificacion, DbType.String, entity.Epareausumodificacion);
            dbProvider.AddInParameter(command, helper.Epareacodi, DbType.Int32, entity.Epareacodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(EprAreaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Epareaestregistro, DbType.String, entity.Epareaestregistro);
            dbProvider.AddInParameter(command, helper.Epareausumodificacion, DbType.String, entity.Epareausumodificacion);
            dbProvider.AddInParameter(command, helper.Epareacodi, DbType.Int32, entity.Epareacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EprAreaDTO GetById(int epareacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Epareacodi, DbType.Int32, epareacodi);
            EprAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public EprAreaDTO GetCantidadUbicacionSGOCOESEliminar(int epareacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCantidadUbicacionSGOCOESEliminar);

            dbProvider.AddInParameter(command, helper.Epareacodi, DbType.Int32, epareacodi);
            EprAreaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new EprAreaDTO();
                    int iNroEquipos = dr.GetOrdinal(helper.NroEquipos);
                    if (!dr.IsDBNull(iNroEquipos)) entity.NroEquipos = Convert.ToInt32(dr.GetValue(iNroEquipos));
                }
            }

            return entity;
        }

        

        public List<EprAreaDTO> ListSubEstacion()
        {
            List<EprAreaDTO> entitys = new List<EprAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListSubEstacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprAreaDTO entity = new EprAreaDTO();

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = Convert.ToString(dr.GetValue(iAreanomb));

                    int iZona = dr.GetOrdinal(helper.Zona);
                    if (!dr.IsDBNull(iZona)) entity.Zona = Convert.ToString(dr.GetValue(iZona));

                    int iEpareafeccreacion = dr.GetOrdinal(helper.Epareafeccreacion);
                    if (!dr.IsDBNull(iEpareafeccreacion)) entity.Epareafeccreacion = Convert.ToString(dr.GetValue(iEpareafeccreacion));

                    int iEpareafecmodificacion = dr.GetOrdinal(helper.Epareafecmodificacion);
                    if (!dr.IsDBNull(iEpareafecmodificacion)) entity.Epareafecmodificacion = Convert.ToString(dr.GetValue(iEpareafecmodificacion));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EprAreaDTO> ListAreaxCelda(string celda1, string celda2)
        {
            List<EprAreaDTO> entitys = new List<EprAreaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListAreaxCelda);
            dbProvider.AddInParameter(command, helper.Idcelda1, DbType.String, celda1);
            dbProvider.AddInParameter(command, helper.Idcelda2, DbType.String, celda2);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EprAreaDTO entity = new EprAreaDTO();
                    
                    int iAreaCodi = dr.GetOrdinal(helper.AreaCodi);
                    if (!dr.IsDBNull(iAreaCodi)) entity.AreaCodi = Convert.ToInt32(dr.GetValue(iAreaCodi));

                    int iTareacodi = dr.GetOrdinal(helper.Tareacodi);
                    if (!dr.IsDBNull(iTareacodi)) entity.Tareacodi = Convert.ToString(dr.GetValue(iTareacodi));

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = Convert.ToString(dr.GetValue(iAreaabrev));

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = Convert.ToString(dr.GetValue(iAreaNomb));

                    int iAreapadre = dr.GetOrdinal(helper.Areapadre);
                    if (!dr.IsDBNull(iAreapadre)) entity.Areapadre = Convert.ToString(dr.GetValue(iAreapadre));

                    int iAreaestado = dr.GetOrdinal(helper.Areaestado);
                    if (!dr.IsDBNull(iAreaestado)) entity.Areaestado = Convert.ToString(dr.GetValue(iAreaestado));

                    int iUsuariocreacion = dr.GetOrdinal(helper.Usuariocreacion);
                    if (!dr.IsDBNull(iUsuariocreacion)) entity.Usuariocreacion = Convert.ToString(dr.GetValue(iUsuariocreacion));

                    int iFechacreacion = dr.GetOrdinal(helper.Fechacreacion);
                    if (!dr.IsDBNull(iFechacreacion)) entity.Fechacreacion = Convert.ToString(dr.GetValue(iFechacreacion));

                    int iUsuarioupdate = dr.GetOrdinal(helper.Usuarioupdate);
                    if (!dr.IsDBNull(iUsuarioupdate)) entity.Usuarioupdate = Convert.ToString(dr.GetValue(iUsuarioupdate));

                    int iFechaupdate = dr.GetOrdinal(helper.Fechaupdate);
                    if (!dr.IsDBNull(iFechaupdate)) entity.Fechaupdate = Convert.ToString(dr.GetValue(iFechaupdate));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}

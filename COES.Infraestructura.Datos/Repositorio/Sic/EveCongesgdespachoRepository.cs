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
    /// Clase de acceso a datos de la tabla EVE_CONGESGDESPACHO
    /// </summary>
    public class EveCongesgdespachoRepository : RepositoryBase, IEveCongesgdespachoRepository
    {
        public EveCongesgdespachoRepository(string strConn)
            : base(strConn)
        {
        }

        EveCongesgdespachoHelper helper = new EveCongesgdespachoHelper();

        public int Save(EveCongesgdespachoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Congdecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Congdefechaini, DbType.DateTime, entity.Congdefechaini);
            dbProvider.AddInParameter(command, helper.Congdefechafin, DbType.DateTime, entity.Congdefechafin);
            dbProvider.AddInParameter(command, helper.Congdeusucreacion, DbType.String, entity.Congdeusucreacion);
            dbProvider.AddInParameter(command, helper.Congdefeccreacion, DbType.DateTime, entity.Congdefeccreacion);
            dbProvider.AddInParameter(command, helper.Congdeusumodificacion, DbType.String, entity.Congdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Congdefecmodificacion, DbType.DateTime, entity.Congdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Congdeestado, DbType.Int32, entity.Congdeestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveCongesgdespachoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Congdecodi, DbType.Int32, entity.Congdecodi);
            dbProvider.AddInParameter(command, helper.Congdefechaini, DbType.DateTime, entity.Congdefechaini);
            dbProvider.AddInParameter(command, helper.Congdefechafin, DbType.DateTime, entity.Congdefechafin);
            dbProvider.AddInParameter(command, helper.Congdeusucreacion, DbType.String, entity.Congdeusucreacion);
            dbProvider.AddInParameter(command, helper.Congdefeccreacion, DbType.DateTime, entity.Congdefeccreacion);
            dbProvider.AddInParameter(command, helper.Congdeusumodificacion, DbType.String, entity.Congdeusumodificacion);
            dbProvider.AddInParameter(command, helper.Congdefecmodificacion, DbType.DateTime, entity.Congdefecmodificacion);
            dbProvider.AddInParameter(command, helper.Congdeestado, DbType.Int32, entity.Congdeestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateEstado(EveCongesgdespachoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateEstado);


            dbProvider.AddInParameter(command, helper.Congdeestado, DbType.Int32, entity.Congdeestado);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int Iccodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, Iccodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<EveCongesgdespachoDTO> GetById(int congdecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Congdecodi, DbType.Int32, congdecodi);
            List<EveCongesgdespachoDTO> entitys = new List<EveCongesgdespachoDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveCongesgdespachoDTO> List()
        {
            List<EveCongesgdespachoDTO> entitys = new List<EveCongesgdespachoDTO>();
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

        public List<EveCongesgdespachoDTO> GetByCriteria()
        {
            List<EveCongesgdespachoDTO> entitys = new List<EveCongesgdespachoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveCongesgdespachoDTO> BuscarOperacionesCongestion(DateTime fechaInicio, DateTime fechaFinal)
        {
            List<EveCongesgdespachoDTO> entitys = new List<EveCongesgdespachoDTO>();
            String sql = String.Format(this.helper.ObtenerListadoCongestion, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveCongesgdespachoDTO entity = new EveCongesgdespachoDTO();

                    int iIccodi = dr.GetOrdinal(this.helper.Iccodi);
                    if (!dr.IsDBNull(iIccodi)) entity.Iccodi = dr.GetInt32(iIccodi);

                    int iEmprCodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt16(dr.GetValue(iEmprCodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iIchorini = dr.GetOrdinal(this.helper.Ichorini);
                    if (!dr.IsDBNull(iIchorini)) entity.Ichorini = dr.GetDateTime(iIchorini);

                    int iIchorfin = dr.GetOrdinal(this.helper.Ichorfin);
                    if (!dr.IsDBNull(iIchorfin)) entity.Ichorfin = dr.GetDateTime(iIchorfin);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iDescrip2 = dr.GetOrdinal(this.helper.Icdescrip2);
                    if (!dr.IsDBNull(iDescrip2)) entity.Icdescrip2 = dr.GetString(iDescrip2);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iCatecodi = dr.GetOrdinal(this.helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iCongdefeccreacion = dr.GetOrdinal(this.helper.Congdefeccreacion);
                    if (!dr.IsDBNull(iCongdefeccreacion)) entity.Congdefeccreacion = dr.GetDateTime(iCongdefeccreacion);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

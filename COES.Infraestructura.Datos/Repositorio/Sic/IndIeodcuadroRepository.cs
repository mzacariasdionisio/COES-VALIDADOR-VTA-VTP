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
    /// Clase de acceso a datos de la tabla IND_IEODCUADRO
    /// </summary>
    public class IndIeodcuadroRepository : RepositoryBase, IIndIeodcuadroRepository
    {
        public IndIeodcuadroRepository(string strConn) : base(strConn)
        {
        }

        IndIeodcuadroHelper helper = new IndIeodcuadroHelper();

        public int Save(IndIeodcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Iiccocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Iiccotipoindisp, DbType.String, entity.Iiccotipoindisp);
            dbProvider.AddInParameter(command, helper.Iiccopr, DbType.Decimal, entity.Iiccopr);
            dbProvider.AddInParameter(command, helper.Iiccousucreacion, DbType.String, entity.Iiccousucreacion);
            dbProvider.AddInParameter(command, helper.Iiccofeccreacion, DbType.DateTime, entity.Iiccofeccreacion);
            dbProvider.AddInParameter(command, helper.Iiccousumodificacion, DbType.String, entity.Iiccousumodificacion);
            dbProvider.AddInParameter(command, helper.Iiccofecmodificacion, DbType.DateTime, entity.Iiccofecmodificacion);
            dbProvider.AddInParameter(command, helper.Iiccocomentario, DbType.String, entity.Iiccocomentario);
            dbProvider.AddInParameter(command, helper.Iccodi, DbType.Int32, entity.Iccodi);
            dbProvider.AddInParameter(command, helper.Iiccoestado, DbType.String, entity.Iiccoestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndIeodcuadroDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Iiccousumodificacion, DbType.String, entity.Iiccousumodificacion);
            dbProvider.AddInParameter(command, helper.Iiccofecmodificacion, DbType.DateTime, entity.Iiccofecmodificacion);
            dbProvider.AddInParameter(command, helper.Iiccoestado, DbType.String, entity.Iiccoestado);
            dbProvider.AddInParameter(command, helper.Iiccocodi, DbType.Int32, entity.Iiccocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int iiccocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Iiccocodi, DbType.Int32, iiccocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndIeodcuadroDTO GetById(int iiccocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Iiccocodi, DbType.Int32, iiccocodi);
            IndIeodcuadroDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<IndIeodcuadroDTO> List()
        {
            List<IndIeodcuadroDTO> entitys = new List<IndIeodcuadroDTO>();
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

        public List<IndIeodcuadroDTO> ListHistoricoByIccodi(int iccodi)
        {
            List<IndIeodcuadroDTO> entitys = new List<IndIeodcuadroDTO>();
            var sql = string.Format(helper.SqlListHistoricoByIccodi, iccodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndIeodcuadroDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndIeodcuadroDTO> GetByCriteria(DateTime fechaIni, DateTime fechaFin, string idsEmpresa, string idsTipoEquipo, string idstipoMantto)
        {
            List<IndIeodcuadroDTO> entitys = new List<IndIeodcuadroDTO>();
            var sql = string.Format(helper.SqlGetByCriteria, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha)
                                                            , idsEmpresa, idsTipoEquipo, idstipoMantto);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndIeodcuadroDTO entity = helper.Create(dr);

                    int iEvenini = dr.GetOrdinal(helper.Ichorini);
                    if (!dr.IsDBNull(iEvenini)) entity.Ichorini = dr.GetDateTime(iEvenini);
                    int iEvenfin = dr.GetOrdinal(helper.Ichorfin);
                    if (!dr.IsDBNull(iEvenfin)) entity.Ichorfin = dr.GetDateTime(iEvenfin);
                    int iIcdescrip1 = dr.GetOrdinal(helper.Icdescrip1);
                    if (!dr.IsDBNull(iIcdescrip1)) entity.Icdescrip1 = dr.GetString(iIcdescrip1);
                    int iIcdescrip2 = dr.GetOrdinal(helper.Icdescrip2);
                    if (!dr.IsDBNull(iIcdescrip2)) entity.Icdescrip2 = dr.GetString(iIcdescrip2);
                    int iIcdescrip3 = dr.GetOrdinal(helper.Icdescrip3);
                    if (!dr.IsDBNull(iIcdescrip3)) entity.Icdescrip3 = dr.GetString(iIcdescrip3);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = Convert.ToInt32(dr.GetValue(iEvenclasecodi));
                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

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
    /// Clase de acceso a datos de la tabla EQ_EQUICANAL
    /// </summary>
    public class EqEquicanalRepository : RepositoryBase, IEqEquicanalRepository
    {
        public EqEquicanalRepository(string strConn)
            : base(strConn)
        {
        }

        EqEquicanalHelper helper = new EqEquicanalHelper();

        public int Save(EqEquicanalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Ecancodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ecanestado, DbType.String, entity.Ecanestado);
            dbProvider.AddInParameter(command, helper.Ecanfactor, DbType.Decimal, entity.Ecanfactor);
            dbProvider.AddInParameter(command, helper.Ecanusucreacion, DbType.String, entity.Ecanusucreacion);
            dbProvider.AddInParameter(command, helper.Ecanfecmodificacion, DbType.DateTime, entity.Ecanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ecanusumodificacion, DbType.String, entity.Ecanusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecanfeccreacion, DbType.DateTime, entity.Ecanfeccreacion);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqEquicanalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Ecanestado, DbType.String, entity.Ecanestado);
            dbProvider.AddInParameter(command, helper.Ecanfactor, DbType.Decimal, entity.Ecanfactor);
            dbProvider.AddInParameter(command, helper.Ecanusucreacion, DbType.String, entity.Ecanusucreacion);
            dbProvider.AddInParameter(command, helper.Ecanfecmodificacion, DbType.DateTime, entity.Ecanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Ecanusumodificacion, DbType.String, entity.Ecanusumodificacion);
            dbProvider.AddInParameter(command, helper.Ecanfeccreacion, DbType.DateTime, entity.Ecanfeccreacion);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, entity.Areacode);

            dbProvider.AddInParameter(command, helper.Ecancodi, DbType.Int32, entity.Ecancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int areacode, int canalcodi, int equicodi, int tipoinfocodi, string user)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);
            dbProvider.AddInParameter(command, helper.Ecanusumodificacion, DbType.String, user);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqEquicanalDTO GetById(int areacode, int canalcodi, int equicodi, int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);
            dbProvider.AddInParameter(command, helper.Areacode, DbType.Int32, areacode);

            EqEquicanalDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iCanalnomb = dr.GetOrdinal(this.helper.Canalnomb);
                    if (!dr.IsDBNull(iCanalnomb)) entity.Canalnomb = dr.GetValue(iCanalnomb).ToString();

                    int iCanalabrev = dr.GetOrdinal(this.helper.Canalabrev);
                    if (!dr.IsDBNull(iCanalabrev)) entity.Canalabrev = dr.GetString(iCanalabrev);

                    int iZonanomb = dr.GetOrdinal(this.helper.Zonanomb);
                    if (!dr.IsDBNull(iZonanomb)) entity.Zonanomb = dr.GetString(iZonanomb);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);
                }
            }

            return entity;
        }

        public List<EqEquicanalDTO> List()
        {
            List<EqEquicanalDTO> entitys = new List<EqEquicanalDTO>();
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

        public List<EqEquicanalDTO> GetByCriteria()
        {
            List<EqEquicanalDTO> entitys = new List<EqEquicanalDTO>();
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

        public List<EqEquicanalDTO> ListarEquivalencia(int areacode, int idEmpresa, int idFamilia, int medida)
        {
            List<EqEquicanalDTO> entitys = new List<EqEquicanalDTO>();
            string sql = string.Format(helper.SqlListarEquivalencia, areacode, idEmpresa, idFamilia, medida);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquicanalDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iAreaabrev = dr.GetOrdinal(helper.Areaabrev);
                    if (!dr.IsDBNull(iAreaabrev)) entity.Areaabrev = dr.GetString(iAreaabrev);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

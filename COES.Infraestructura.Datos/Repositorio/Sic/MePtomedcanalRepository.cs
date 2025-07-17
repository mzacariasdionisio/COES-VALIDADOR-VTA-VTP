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
    /// Clase de acceso a datos de la tabla ME_PTOMEDCANAL
    /// </summary>
    public class MePtomedcanalRepository : RepositoryBase, IMePtomedcanalRepository
    {
        public MePtomedcanalRepository(string strConn)
            : base(strConn)
        {
        }

        MePtomedcanalHelper helper = new MePtomedcanalHelper();

        public void Save(MePtomedcanalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);

            dbProvider.AddInParameter(command, helper.Pcanestado, DbType.String, entity.Pcanestado);
            dbProvider.AddInParameter(command, helper.Pcanusucreacion, DbType.String, entity.Pcanusucreacion);
            dbProvider.AddInParameter(command, helper.Pcanfeccreacion, DbType.DateTime, entity.Pcanfeccreacion);
            dbProvider.AddInParameter(command, helper.Pcanfactor, DbType.Decimal, entity.Pcanfactor);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Update(MePtomedcanalDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Pcanestado, DbType.String, entity.Pcanestado);
            dbProvider.AddInParameter(command, helper.Pcanusumodificacion, DbType.String, entity.Pcanusumodificacion);
            dbProvider.AddInParameter(command, helper.Pcanfecmodificacion, DbType.DateTime, entity.Pcanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Pcanfactor, DbType.Decimal, entity.Pcanfactor);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, entity.Ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, entity.Tipoinfocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public MePtomedcanalDTO GetById(int canalcodi, int ptomedicodi, int tipoinfocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, canalcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodi, DbType.Int32, ptomedicodi);
            dbProvider.AddInParameter(command, helper.Tipoinfocodi, DbType.Int32, tipoinfocodi);

            MePtomedcanalDTO entity = null;

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

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);
                }
            }

            return entity;
        }

        public List<MePtomedcanalDTO> List()
        {
            List<MePtomedcanalDTO> entitys = new List<MePtomedcanalDTO>();
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

        public List<MePtomedcanalDTO> GetByCriteria()
        {
            List<MePtomedcanalDTO> entitys = new List<MePtomedcanalDTO>();
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

        public List<MePtomedcanalDTO> ListarEquivalencia(string idEmpresa, int origlectcodi, int medida, string famcodis)
        {
            List<MePtomedcanalDTO> entitys = new List<MePtomedcanalDTO>();
            string sql = string.Format(helper.SqlListarEquivalencia, idEmpresa, origlectcodi, medida, famcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    MePtomedcanalDTO entity = new MePtomedcanalDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iPtomedicodi = dr.GetOrdinal(this.helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iFamcodi = dr.GetOrdinal(this.helper.Famcodi);
                    if (!dr.IsDBNull(iEquinomb)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(this.helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iCanalcodi = dr.GetOrdinal(this.helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iPmedclfestado = dr.GetOrdinal(this.helper.Pcanestado);
                    if (!dr.IsDBNull(iPmedclfestado)) entity.Pcanestado = dr.GetString(iPmedclfestado);

                    int iPcanfactor = dr.GetOrdinal(helper.Pcanfactor);
                    if (!dr.IsDBNull(iPcanfactor)) entity.Pcanfactor = dr.GetDecimal(iPcanfactor);

                    int iTipoinfocodi = dr.GetOrdinal(this.helper.Tipoinfocodi);
                    if (!dr.IsDBNull(iTipoinfocodi)) entity.Tipoinfocodi = Convert.ToInt32(dr.GetValue(iTipoinfocodi));

                    int iTipoinfoabrev = dr.GetOrdinal(this.helper.Tipoinfoabrev);
                    if (!dr.IsDBNull(iTipoinfoabrev)) entity.Tipoinfoabrev = dr.GetString(iTipoinfoabrev);

                    int iOriglectnombre = dr.GetOrdinal(this.helper.Origlectnombre);
                    if (!dr.IsDBNull(iOriglectnombre)) entity.Origlectnombre = dr.GetString(iOriglectnombre);

                    int iPtomedielenomb = dr.GetOrdinal(this.helper.Ptomedielenomb);
                    if (!dr.IsDBNull(iPtomedielenomb)) entity.Ptomedielenomb = dr.GetString(iPtomedielenomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<MePtomedcanalDTO> ObtenerEquivalencia(string ptomedicodis, int tipoinfocodi)
        {
            List<MePtomedcanalDTO> entitys = new List<MePtomedcanalDTO>();
            string sql = string.Format(helper.SqlObtenerEquivalencia, ptomedicodis, tipoinfocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
    }
}

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
    /// Clase de acceso a datos de la tabla WB_MEDIDORES_VALIDACION
    /// </summary>
    public class WbMedidoresValidacionRepository: RepositoryBase, IWbMedidoresValidacionRepository
    {
        public WbMedidoresValidacionRepository(string strConn): base(strConn)
        {
        }

        WbMedidoresValidacionHelper helper = new WbMedidoresValidacionHelper();

        public int Save(WbMedidoresValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Medivalcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Ptomedicodimed, DbType.Int32, entity.Ptomedicodimed);
            dbProvider.AddInParameter(command, helper.Ptomedicodidesp, DbType.Int32, entity.Ptomedicodidesp);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(WbMedidoresValidacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Medivalcodi, DbType.Int32, entity.Medivalcodi);
            dbProvider.AddInParameter(command, helper.Ptomedicodimed, DbType.Int32, entity.Ptomedicodimed);
            dbProvider.AddInParameter(command, helper.Ptomedicodidesp, DbType.Int32, entity.Ptomedicodidesp);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Indestado, DbType.String, entity.Indestado);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int medivalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Medivalcodi, DbType.Int32, medivalcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public WbMedidoresValidacionDTO GetById(int medivalcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Medivalcodi, DbType.Int32, medivalcodi);
            WbMedidoresValidacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<WbMedidoresValidacionDTO> List()
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquipo = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquipo)) entity.Equinomb = dr.GetString(iEquipo);

                    int iEmpresa = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmpresa)) entity.Emprnomb = dr.GetString(iEmpresa);

                    int iPtomediestado = dr.GetOrdinal(helper.Ptomediestado);
                    if (!dr.IsDBNull(iPtomediestado)) entity.Ptomediestado = dr.GetString(iPtomediestado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbMedidoresValidacionDTO> GetByCriteria()
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlGetByCriteria));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iEmprCodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprCodi));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoNomb = dr.GetOrdinal(this.helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    int iGrupoAbrev = dr.GetOrdinal(this.helper.GrupoAbrev);
                    if (!dr.IsDBNull(iGrupoAbrev)) entity.Grupoabrev = dr.GetString(iGrupoAbrev);

                    int iPtoMediCodiDesp = dr.GetOrdinal(this.helper.Ptomedicodidesp);
                    if (!dr.IsDBNull(iPtoMediCodiDesp)) entity.Ptomedicodidesp = Convert.ToInt32(dr.GetValue(iPtoMediCodiDesp));

                    int iPtoMediCodiMed = dr.GetOrdinal(this.helper.Ptomedicodimed);
                    if (!dr.IsDBNull(iPtoMediCodiMed)) entity.Ptomedicodimed = Convert.ToInt32(dr.GetValue(iPtoMediCodiMed));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<WbMedidoresValidacionDTO> ObtenerPuntosPorEmpresa(int origLectCodi, int emprCodi)
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();
            string sql = String.Format(helper.SqlObtenerPuntosPorEmpresa, origLectCodi, emprCodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ValidarExistencia(int idMedicion, int idDespacho)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarExistencia);
            dbProvider.AddInParameter(command, helper.Ptomedicodimed, DbType.Int32, idMedicion);
            dbProvider.AddInParameter(command, helper.Ptomedicodidesp, DbType.Int32, idDespacho);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<WbMedidoresValidacionDTO> ObtenerRelaciones(int idEmpresa)
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();
            string query = string.Format(helper.SqlObtenerRelaciones, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }


            return entitys;
        }

        public List<WbMedidoresValidacionDTO> ObtenerEmpresasGrafico()
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();
           
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerEmpresaGrafico);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }


            return entitys;

        }

        public List<WbMedidoresValidacionDTO> ObtenerGruposGrafico(int idEmpresa)
        {
            List<WbMedidoresValidacionDTO> entitys = new List<WbMedidoresValidacionDTO>();

            string query = string.Format(helper.SqlObtenerGrupoGrafico, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    WbMedidoresValidacionDTO entity = new WbMedidoresValidacionDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.GrupoNomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }


            return entitys;

        }


    }
}

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
    /// Clase de acceso a datos de la tabla EVE_INTERRUPCION
    /// </summary>
    public class EveInterrupcionRepository: RepositoryBase, IEveInterrupcionRepository
    {
        public EveInterrupcionRepository(string strConn): base(strConn)
        {
        }

        EveInterrupcionHelper helper = new EveInterrupcionHelper();

        public int Save(EveInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Interrupcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.InterrmwDe, DbType.Decimal, entity.InterrmwDe);
            dbProvider.AddInParameter(command, helper.InterrmwA, DbType.Decimal, entity.InterrmwA);
            dbProvider.AddInParameter(command, helper.Interrminu, DbType.Decimal, entity.Interrminu);
            dbProvider.AddInParameter(command, helper.Interrmw, DbType.Decimal, entity.Interrmw);
            dbProvider.AddInParameter(command, helper.Interrdesc, DbType.String, entity.Interrdesc);
            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, entity.Ptointerrcodi);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Interrnivel, DbType.String, entity.Interrnivel);
            dbProvider.AddInParameter(command, helper.Interrracmf, DbType.String, entity.Interrracmf);
            dbProvider.AddInParameter(command, helper.Interrmfetapa, DbType.Int32, entity.Interrmfetapa);
            dbProvider.AddInParameter(command, helper.Interrmanualr, DbType.String, entity.Interrmanualr);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);            

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveInterrupcionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.InterrmwDe, DbType.Decimal, entity.InterrmwDe);
            dbProvider.AddInParameter(command, helper.InterrmwA, DbType.Decimal, entity.InterrmwA);
            dbProvider.AddInParameter(command, helper.Interrminu, DbType.Decimal, entity.Interrminu);
            dbProvider.AddInParameter(command, helper.Interrmw, DbType.Decimal, entity.Interrmw);
            dbProvider.AddInParameter(command, helper.Interrdesc, DbType.String, entity.Interrdesc);            
            dbProvider.AddInParameter(command, helper.Ptointerrcodi, DbType.Int32, entity.Ptointerrcodi);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Interrnivel, DbType.String, entity.Interrnivel);
            dbProvider.AddInParameter(command, helper.Interrracmf, DbType.String, entity.Interrracmf);
            dbProvider.AddInParameter(command, helper.Interrmfetapa, DbType.Int32, entity.Interrmfetapa);
            dbProvider.AddInParameter(command, helper.Interrmanualr, DbType.String, entity.Interrmanualr);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Interrupcodi, DbType.Int32, entity.Interrupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int interrupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Interrupcodi, DbType.Int32, interrupcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveInterrupcionDTO GetById(int interrupcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Interrupcodi, DbType.Int32, interrupcodi);
            EveInterrupcionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iPtointerrupnomb = dr.GetOrdinal(helper.PtoInterrupNomb);
                    if (!dr.IsDBNull(iPtointerrupnomb)) entity.PtoInterrupNomb = dr.GetString(iPtointerrupnomb);

                    int iPtoentrenomb = dr.GetOrdinal(helper.PtoEntreNomb);
                    if (!dr.IsDBNull(iPtoentrenomb)) entity.PtoEntreNomb = dr.GetString(iPtoentrenomb);
                }
            }

            return entity;
        }

        public List<EveInterrupcionDTO> List()
        {
            List<EveInterrupcionDTO> entitys = new List<EveInterrupcionDTO>();
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

        public List<EveInterrupcionDTO> GetByCriteria(string idEvento)
        {
            List<EveInterrupcionDTO> entitys = new List<EveInterrupcionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEvento);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInterrupcionDTO entity = helper.Create(dr);

                    int iPtoInterrupNomb = dr.GetOrdinal(helper.PtoInterrupNomb);
                    if (!dr.IsDBNull(iPtoInterrupNomb)) entity.PtoInterrupNomb = dr.GetString(iPtoInterrupNomb);

                    int iPtoEntreNomb = dr.GetOrdinal(helper.PtoEntreNomb);
                    if (!dr.IsDBNull(iPtoEntreNomb)) entity.PtoEntreNomb = dr.GetString(iPtoEntreNomb);

                    int iAreaNomb = dr.GetOrdinal(helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iEquiAbrev = dr.GetOrdinal(helper.EquiAbrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EquiAbrev = dr.GetString(iEquiAbrev);

                    int iEquiTension = dr.GetOrdinal(helper.EquiTension);
                    if (!dr.IsDBNull(iEquiTension)) entity.EquiTension = dr.GetDecimal(iEquiTension);

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region MigracionSGOCOES-GrupoB
        public List<EveInterrupcionDTO> ListaCalidadSuministro(DateTime fecIni)
        {
            List<EveInterrupcionDTO> entitys = new List<EveInterrupcionDTO>();
            string query = string.Format(helper.SqlListaCalidadSuministro, fecIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInterrupcionDTO entity = new EveInterrupcionDTO();

                    int iPtoInterrupNomb = dr.GetOrdinal(helper.PtoInterrupNomb);
                    if (!dr.IsDBNull(iPtoInterrupNomb)) entity.PtoInterrupNomb = dr.GetString(iPtoInterrupNomb);

                    int iEmprNomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iPtoEntreNomb = dr.GetOrdinal(helper.PtoEntreNomb);
                    if (!dr.IsDBNull(iPtoEntreNomb)) entity.PtoEntreNomb = dr.GetString(iPtoEntreNomb);

                    int iInterrmw = dr.GetOrdinal(helper.Interrmw);
                    if (!dr.IsDBNull(iInterrmw)) entity.Interrmw = dr.GetDecimal(iInterrmw);

                    int iEvenini = dr.GetOrdinal(helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iInterrminu = dr.GetOrdinal(helper.Interrminu);
                    if (!dr.IsDBNull(iInterrminu)) entity.Interrminu = dr.GetDecimal(iInterrminu);

                    int iInterrdesc = dr.GetOrdinal(helper.Interrdesc);
                    if (!dr.IsDBNull(iInterrdesc)) entity.Interrdesc = dr.GetString(iInterrdesc);

                    int iInterrracmf = dr.GetOrdinal(helper.Interrracmf);
                    if (!dr.IsDBNull(iInterrracmf)) entity.Interrracmf = dr.GetString(iInterrracmf);

                    int iInterrmfetapa = dr.GetOrdinal(helper.Interrmfetapa);
                    if (!dr.IsDBNull(iInterrmfetapa)) entity.InterrmfetapaDesc = dr.GetString(iInterrmfetapa);

                    int iInterrmanualr = dr.GetOrdinal(helper.Interrmanualr);
                    if (!dr.IsDBNull(iInterrmanualr)) entity.Interrmanualr = dr.GetString(iInterrmanualr);

                    int iEvencodi = dr.GetOrdinal(helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = dr.GetInt32(iEvencodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion
    }
}

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
    /// Clase de acceso a datos de la tabla EQ_RELACION
    /// </summary>
    public class EqRelacionRepository : RepositoryBase, IEqRelacionRepository
    {
        public EqRelacionRepository(string strConn)
            : base(strConn)
        {
        }

        EqRelacionHelper helper = new EqRelacionHelper();

        public int Save(EqRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codbarra, DbType.String, entity.Codbarra);
            dbProvider.AddInParameter(command, helper.Idgener, DbType.String, entity.Idgener);
            dbProvider.AddInParameter(command, helper.Descripcion, DbType.String, entity.Descripcion);
            dbProvider.AddInParameter(command, helper.Nombarra, DbType.String, entity.Nombarra);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Indfuente, DbType.String, entity.Indfuente);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna, DbType.String, entity.Nombretna);
            //- Linea agregada movisoft 25.02.2021
            dbProvider.AddInParameter(command, helper.Indgeneracionrer, DbType.String, entity.Indgeneracionrer);

            #region Ticket 2022-004245
            dbProvider.AddInParameter(command, helper.Indnomodeladatna, DbType.String, entity.Indnomodeladatna);
            #endregion
            dbProvider.AddInParameter(command, helper.Indtnaadicional, DbType.String, entity.Indtnaadicional);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }


        public int SaveReservaRotante(EqRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSaveReservaRotante);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Idgener, DbType.String, entity.Idgener);
            dbProvider.AddInParameter(command, helper.Nombarra, DbType.String, entity.Nombarra);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Indrvarte, DbType.String, entity.Indrvarte);
            dbProvider.AddInParameter(command, helper.Estadorvarte, DbType.String, entity.Estadorvarte);
            dbProvider.AddInParameter(command, helper.Canaliccpint, DbType.String, entity.Canaliccpint);
            dbProvider.AddInParameter(command, helper.Canalsigno, DbType.String, entity.Canalsigno);
            dbProvider.AddInParameter(command, helper.Canaluso, DbType.String, entity.Canaluso);
            dbProvider.AddInParameter(command, helper.Canalcero, DbType.String, entity.Canalcero);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna, DbType.String, entity.Nombretna);
            //- Linea agregada movisoft 25.02.2021
            dbProvider.AddInParameter(command, helper.Indgeneracionrer, DbType.String, entity.Indgeneracionrer);
            dbProvider.AddInParameter(command, helper.Indtnaadicional, DbType.String, entity.Indtnaadicional);

            #region Ticket 2022-004245
            dbProvider.AddInParameter(command, helper.Indnomodeladatna, DbType.String, entity.Indnomodeladatna);
            #endregion

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EqRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Codincp, DbType.Int32, entity.Codincp);
            dbProvider.AddInParameter(command, helper.Nombrencp, DbType.String, entity.Nombrencp);
            dbProvider.AddInParameter(command, helper.Codbarra, DbType.String, entity.Codbarra);
            dbProvider.AddInParameter(command, helper.Idgener, DbType.String, entity.Idgener);
            dbProvider.AddInParameter(command, helper.Descripcion, DbType.String, entity.Descripcion);
            dbProvider.AddInParameter(command, helper.Nombarra, DbType.String, entity.Nombarra);
            dbProvider.AddInParameter(command, helper.Estado, DbType.String, entity.Estado);
            dbProvider.AddInParameter(command, helper.Indfuente, DbType.String, entity.Indfuente);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna, DbType.String, entity.Nombretna);
            //- Linea agregada movisoft 25.02.2021
            dbProvider.AddInParameter(command, helper.Indgeneracionrer, DbType.String, entity.Indgeneracionrer);

            #region Ticket 2022-004245
            dbProvider.AddInParameter(command, helper.Indnomodeladatna, DbType.String, entity.Indnomodeladatna);
            #endregion

            dbProvider.AddInParameter(command, helper.Indtnaadicional, DbType.String, entity.Indtnaadicional);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);


            dbProvider.ExecuteNonQuery(command);
        }

        public void UpdateReservaRotante(EqRelacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateReservaRotante);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Idgener, DbType.String, entity.Idgener);
            dbProvider.AddInParameter(command, helper.Nombarra, DbType.String, entity.Nombarra);
            dbProvider.AddInParameter(command, helper.Canalcodi, DbType.Int32, entity.Canalcodi);
            dbProvider.AddInParameter(command, helper.Canaliccp, DbType.String, entity.Canaliccp);
            dbProvider.AddInParameter(command, helper.Indrvarte, DbType.String, entity.Indrvarte);
            dbProvider.AddInParameter(command, helper.Estadorvarte, DbType.String, entity.Estadorvarte);
            dbProvider.AddInParameter(command, helper.Canaliccpint, DbType.String, entity.Canaliccpint);
            dbProvider.AddInParameter(command, helper.Canalsigno, DbType.String, entity.Canalsigno);
            dbProvider.AddInParameter(command, helper.Canaluso, DbType.String, entity.Canaluso);
            dbProvider.AddInParameter(command, helper.Canalcero, DbType.String, entity.Canalcero);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Nombretna, DbType.String, entity.Nombretna);
            //- Linea agregada movisoft 25.02.2021
            dbProvider.AddInParameter(command, helper.Indgeneracionrer, DbType.String, entity.Indgeneracionrer);

            #region Ticket 2022-004245
            dbProvider.AddInParameter(command, helper.Indnomodeladatna, DbType.String, entity.Indnomodeladatna);
            #endregion
            dbProvider.AddInParameter(command, helper.Indtnaadicional, DbType.String, entity.Indtnaadicional);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, entity.Relacioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int relacioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, relacioncodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EqRelacionDTO GetById(int relacioncodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Relacioncodi, DbType.Int32, relacioncodi);
            EqRelacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iIndrvarte = dr.GetOrdinal(helper.Indrvarte);
                    if (!dr.IsDBNull(iIndrvarte)) entity.Indrvarte = dr.GetString(iIndrvarte);

                    int iEstadorvarte = dr.GetOrdinal(helper.Estadorvarte);
                    if (!dr.IsDBNull(iEstadorvarte)) entity.Estadorvarte = dr.GetString(iEstadorvarte);

                    int iCanaliccpint = dr.GetOrdinal(helper.Canaliccpint);
                    if (!dr.IsDBNull(iCanaliccpint)) entity.Canaliccpint = dr.GetString(iCanaliccpint);

                    int iCanalsigno = dr.GetOrdinal(helper.Canalsigno);
                    if (!dr.IsDBNull(iCanalsigno)) entity.Canalsigno = dr.GetString(iCanalsigno);

                    int iCanaluso = dr.GetOrdinal(helper.Canaluso);
                    if (!dr.IsDBNull(iCanaluso)) entity.Canaluso = dr.GetString(iCanaluso);

                    int iCanalcero = dr.GetOrdinal(helper.Canalcero);
                    if (!dr.IsDBNull(iCanalcero)) entity.Canalcero = dr.GetString(iCanalcero);
                }
            }

            return entity;
        }

        public List<EqRelacionDTO> List(string fuente)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlList, fuente);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ListHidraulico(string fuente)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlListHidraulico, fuente);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> GetByCriteria(int idEmpresa, string estado, string fuente)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlGetByCriteria, idEmpresa, estado, fuente);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iDesubicacion = dr.GetOrdinal(helper.Desubicacion);
                    if (!dr.IsDBNull(iDesubicacion)) entity.Desubicacion = dr.GetString(iDesubicacion);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> GetByCriteriaReservaRotante(int idEmpresa, string estado, int idGrupo)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlGetByCriteriaReservaRotante, idEmpresa, estado, idGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTgenercodi = dr.GetOrdinal(helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iTgenernomb = dr.GetOrdinal(helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListarEmpresas(string fuente)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresas);
            dbProvider.AddInParameter(command, helper.Indfuente, DbType.String, fuente);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListarEmpresasReservaRotante()
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEmpresasReservaRotante);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerPorEquipo(int equicodi, string fuente)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorEquipo);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Indfuente, DbType.String, fuente);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public int ObtenerPorEquipoReservaRotante(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPorEquipoReservaRotante);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public List<EqEquipoDTO> ObtenerEquiposRelacion(int idEmpresa)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();
            string sql = string.Format(helper.SqlObtenerEquipoRelacion, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<EqRelacionDTO> ObtenerConfiguracionProceso(string fuente, string famcodis)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionProceso, fuente, famcodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iIndtipo = dr.GetOrdinal(helper.Indtipo);
                    if (!dr.IsDBNull(iIndtipo)) entity.IndTipo = dr.GetString(iIndtipo);

                    int iIndcc = dr.GetOrdinal(helper.Indcc);
                    if (!dr.IsDBNull(iIndcc)) entity.Indcc = Convert.ToInt32(dr.GetValue(iIndcc));

                    int iModosoperacion = dr.GetOrdinal(helper.Modosoperacion);
                    if (!dr.IsDBNull(iModosoperacion)) entity.Modosoperacion = dr.GetString(iModosoperacion);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    //int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    //if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

                    int iIndtvcc = dr.GetOrdinal(helper.Indtvcc);
                    if (!dr.IsDBNull(iIndtvcc)) entity.Indtvcc = dr.GetString(iIndtvcc);

                    //if (entity.Indtvcc == "S")
                    //{
                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));
                    //}

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iIndcoes = dr.GetOrdinal(helper.Indcoes);
                    if (!dr.IsDBNull(iIndcoes)) entity.Indcoes = dr.GetString(iIndcoes);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iIndnoforzada = dr.GetOrdinal(helper.Indnoforzada);
                    if (!dr.IsDBNull(iIndnoforzada)) entity.Indnoforzada = dr.GetString(iIndnoforzada);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerContadorGrupo()
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerContadorGrupo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iCodicnp = dr.GetOrdinal(helper.Codincp);
                    if (!dr.IsDBNull(iCodicnp)) entity.Codincp = Convert.ToInt32(dr.GetValue(iCodicnp));

                    int iNombrencp = dr.GetOrdinal(helper.Nombrencp);
                    if (!dr.IsDBNull(iNombrencp)) entity.Nombrencp = dr.GetString(iNombrencp);

                    int iContador = dr.GetOrdinal(helper.Contador);
                    if (!dr.IsDBNull(iContador)) entity.Contador = Convert.ToInt32(dr.GetValue(iContador));

                    if (entity.Codincp == 9176) entity.Contador = 6;

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<int> ObtenerModosOperacion(DateTime fecha)
        {
            List<int> result = new List<int>();
            string sql = string.Format(helper.SqlObtenerModosOperacion, fecha.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) result.Add(Convert.ToInt32(dr.GetValue(iGrupocodi)));
                }
            }

            return result;
        }


        public List<EqRelacionDTO> ObtenerModosOperacionLimiteTransmision(DateTime fecha)
        {
            List<EqRelacionDTO> result = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerModosOperacionLimiteTransmision, fecha.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iHoplimtrans = dr.GetOrdinal(helper.Hoplimtrans);
                    if (!dr.IsDBNull(iHoplimtrans)) entity.LimiteTransmision = dr.GetString(iHoplimtrans);

                    result.Add(entity);
                }
            }

            return result;
        }

        public List<int> ObtenerModosOperacionEspeciales()
        {
            List<int> result = new List<int>();
            string sql = string.Format(helper.SqlObtenerModosOperacionEspeciales);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) result.Add(Convert.ToInt32(dr.GetValue(iGrupocodi)));
                }
            }

            return result;
        }

        public List<int> ObtenerUnidadesEnOperacion(DateTime fechaProceso, int idModoOperacion)
        {
            List<int> result = new List<int>();
            string sql = string.Format(helper.SqlObtenerUnidadesEnOperacion, fechaProceso.ToString(ConstantesBase.FormatoFechaHora), idModoOperacion);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) result.Add(Convert.ToInt32(dr.GetValue(iEquicodi)));
                }
            }

            return result;
        }

        public List<EqRelacionDTO> ObtenerCalificacionUnidades(DateTime fecha)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();

            string sql = string.Format(helper.SqlObtenerCalificacionUnidades, fecha.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iSubcausacmg = dr.GetOrdinal(helper.Subcausacmg);
                    if (!dr.IsDBNull(iSubcausacmg)) entity.Subcausacmg = dr.GetString(iSubcausacmg);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerRestriccionOperativa(DateTime fecha)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();

            string sql = string.Format(helper.SqlObtenerRestricionOperativa, fecha.ToString(ConstantesBase.FormatoFechaHora));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = Convert.ToInt32(dr.GetValue(iSubcausacodi));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Valor = dr.GetDecimal(iValor);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerModoOperacionUnidad(int grupoCodi)
        {
            string sql = string.Format(helper.SqlObtenerModoOperacionUnidad, grupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            int idResultado = 0;
            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                if (int.TryParse(result.ToString(), out idResultado)) { }
            }

            return idResultado;
        }

        public List<EqRelacionDTO> ObtenerConfiguracionProcesoDemanda(string fuente, int origenlectura)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionProcesoDemanda, fuente, origenlectura);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerPropiedadHidraulicos()
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPropiedadHidraulicos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPropiedad = dr.GetOrdinal(helper.Propiedad);
                    if (!dr.IsDBNull(iPropiedad)) entity.Propiedad = dr.GetString(iPropiedad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerPropiedadHidraulicosCentral()
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerPropiedadHidraulicosCentral);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO(); ;

                    int iPropcodi = dr.GetOrdinal(helper.Propcodi);
                    if (!dr.IsDBNull(iPropcodi)) entity.Propcodi = Convert.ToInt32(dr.GetValue(iPropcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iPropiedad = dr.GetOrdinal(helper.Propiedad);
                    if (!dr.IsDBNull(iPropiedad)) entity.Propiedad = dr.GetString(iPropiedad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerPropiedadesConfiguracion(int indicador)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = (indicador == 0) ? helper.SqlObtenerPropiedadesHidroCM : helper.SqlObtenerPropiedadesTermoCM;
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    string tomacarga = string.Empty;
                    int iTomacarga = dr.GetOrdinal(helper.VelTomaCarga);
                    if (!dr.IsDBNull(iTomacarga)) tomacarga = dr.GetString(iTomacarga);

                    string reduccioncarga = string.Empty;
                    int iReduccioncarga = dr.GetOrdinal(helper.VelReduccionCarga);
                    if (!dr.IsDBNull(iReduccioncarga)) reduccioncarga = dr.GetString(iReduccioncarga);

                    decimal valtomacarga = 0;
                    decimal valreduccioncarga = 0;

                    if (decimal.TryParse(tomacarga, out valtomacarga))
                    {
                        entity.VelocidadCarga = (double)valtomacarga;
                    }

                    if (decimal.TryParse(reduccioncarga, out valreduccioncarga))
                    {
                        entity.VelocidadDescarga = (double)valreduccioncarga;
                    }

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroUnidades(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerNroUnidades);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }


        public List<EqRelacionDTO> ObtenerConfiguracionReservaRotante()
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerConfiguracionReservaRotante);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iIndtipo = dr.GetOrdinal(helper.Indtipo);
                    if (!dr.IsDBNull(iIndtipo)) entity.IndTipo = dr.GetString(iIndtipo);

                    int iIndcc = dr.GetOrdinal(helper.Indcc);
                    if (!dr.IsDBNull(iIndcc)) entity.Indcc = Convert.ToInt32(dr.GetValue(iIndcc));

                    int iModosoperacion = dr.GetOrdinal(helper.Modosoperacion);
                    if (!dr.IsDBNull(iModosoperacion)) entity.Modosoperacion = dr.GetString(iModosoperacion);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iIndtvcc = dr.GetOrdinal(helper.Indtvcc);
                    if (!dr.IsDBNull(iIndtvcc)) entity.Indtvcc = dr.GetString(iIndtvcc);

                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iIndcoes = dr.GetOrdinal(helper.Indcoes);
                    if (!dr.IsDBNull(iIndcoes)) entity.Indcoes = dr.GetString(iIndcoes);

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iIndrvarte = dr.GetOrdinal(helper.Indrvarte);
                    if (!dr.IsDBNull(iIndrvarte)) entity.Indrvarte = dr.GetString(iIndrvarte);

                    int iCanaliccpint = dr.GetOrdinal(helper.Canaliccpint);
                    if (!dr.IsDBNull(iCanaliccpint)) entity.Canaliccpint = dr.GetString(iCanaliccpint);

                    int iCanalsigno = dr.GetOrdinal(helper.Canalsigno);
                    if (!dr.IsDBNull(iCanalsigno)) entity.Canalsigno = dr.GetString(iCanalsigno);

                    int iCanaluso = dr.GetOrdinal(helper.Canaluso);
                    if (!dr.IsDBNull(iCanaluso)) entity.Canaluso = dr.GetString(iCanaluso);

                    int iCanalcero = dr.GetOrdinal(helper.Canalcero);
                    if (!dr.IsDBNull(iCanalcero)) entity.Canalcero = dr.GetString(iCanalcero);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqRelacionDTO> ObtenerListadoReservaRotante(int idEmpresa, string estado)
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();
            string sql = string.Format(helper.SqlObtenerListadoReservaRotante, idEmpresa, estado);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = helper.Create(dr);

                    int iCanalcodi = dr.GetOrdinal(helper.Canalcodi);
                    if (!dr.IsDBNull(iCanalcodi)) entity.Canalcodi = Convert.ToInt32(dr.GetValue(iCanalcodi));

                    int iCanaliccp = dr.GetOrdinal(helper.Canaliccp);
                    if (!dr.IsDBNull(iCanaliccp)) entity.Canaliccp = dr.GetString(iCanaliccp);

                    int iIndrvarte = dr.GetOrdinal(helper.Indrvarte);
                    if (!dr.IsDBNull(iIndrvarte)) entity.Indrvarte = dr.GetString(iIndrvarte);

                    int iEstadorvarte = dr.GetOrdinal(helper.Estadorvarte);
                    if (!dr.IsDBNull(iEstadorvarte)) entity.Estadorvarte = dr.GetString(iEstadorvarte);

                    int iCanaliccpint = dr.GetOrdinal(helper.Canaliccpint);
                    if (!dr.IsDBNull(iCanaliccpint)) entity.Canaliccpint = dr.GetString(iCanaliccpint);

                    int iCanalsigno = dr.GetOrdinal(helper.Canalsigno);
                    if (!dr.IsDBNull(iCanalsigno)) entity.Canalsigno = dr.GetString(iCanalsigno);

                    int iCanaluso = dr.GetOrdinal(helper.Canaluso);
                    if (!dr.IsDBNull(iCanaluso)) entity.Canaluso = dr.GetString(iCanaluso);

                    int iCanalcero = dr.GetOrdinal(helper.Canalcero);
                    if (!dr.IsDBNull(iCanalcero)) entity.Canalcero = dr.GetString(iCanalcero);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iDesubicacion = dr.GetOrdinal(helper.Desubicacion);
                    if (!dr.IsDBNull(iDesubicacion)) entity.Desubicacion = dr.GetString(iDesubicacion);

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Mejoras CMgN

        public List<EqRelacionDTO> ObtenerUnidadComparativoCM()
        {
            List<EqRelacionDTO> entitys = new List<EqRelacionDTO>();           
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerUnidadComparativoCM);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqRelacionDTO entity = new EqRelacionDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                                      
                    int iIndcc = dr.GetOrdinal(helper.Indcc);
                    if (!dr.IsDBNull(iIndcc)) entity.Indcc = Convert.ToInt32(dr.GetValue(iIndcc));                   

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                   
                    int iIndtvcc = dr.GetOrdinal(helper.Indtvcc);
                    if (!dr.IsDBNull(iIndtvcc)) entity.Indtvcc = dr.GetString(iIndtvcc);

                    int iCcombcodi = dr.GetOrdinal(helper.Ccombcodi);
                    if (!dr.IsDBNull(iCcombcodi)) entity.Ccombcodi = Convert.ToInt32(dr.GetValue(iCcombcodi));                   

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion
    }
}

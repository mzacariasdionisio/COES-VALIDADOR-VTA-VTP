using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Base.Core;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla PR_GRUPO
    /// </summary>
    public class PrGrupoRepository : RepositoryBase, IPrGrupoRepository
    {
        public PrGrupoRepository(string strConn)
            : base(strConn)
        {
        }

        PrGrupoHelper helper = new PrGrupoHelper();

        public int Save(PrGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Barracodi, DbType.Int32, entity.Barracodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Grupoabrev, DbType.String, entity.Grupoabrev);
            dbProvider.AddInParameter(command, helper.Grupovmax, DbType.Decimal, entity.Grupovmax);
            dbProvider.AddInParameter(command, helper.Grupovmin, DbType.Decimal, entity.Grupovmin);
            dbProvider.AddInParameter(command, helper.Grupoorden, DbType.Int32, entity.Grupoorden);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupotipo, DbType.String, entity.Grupotipo);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);
            dbProvider.AddInParameter(command, helper.Grupotipoc, DbType.Int32, entity.Grupotipoc);
            dbProvider.AddInParameter(command, helper.Grupopadre, DbType.Int32, entity.Grupopadre);
            dbProvider.AddInParameter(command, helper.Grupoactivo, DbType.String, entity.Grupoactivo);
            dbProvider.AddInParameter(command, helper.Grupocomb, DbType.String, entity.Grupocomb);
            dbProvider.AddInParameter(command, helper.Osicodi, DbType.String, entity.Osicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi2, DbType.Int32, entity.Grupocodi2);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Gruposub, DbType.String, entity.Gruposub);
            dbProvider.AddInParameter(command, helper.Barracodi2, DbType.Int32, entity.Barracodi2);
            dbProvider.AddInParameter(command, helper.Barramw1, DbType.Decimal, entity.Barramw1);
            dbProvider.AddInParameter(command, helper.Barramw2, DbType.Decimal, entity.Barramw2);
            dbProvider.AddInParameter(command, helper.Gruponombncp, DbType.String, entity.Gruponombncp);
            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, entity.Tipogrupocodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.TipoGrupoCodi2, DbType.Int32, entity.TipoGrupoCodi2);
            dbProvider.AddInParameter(command, helper.TipoGenerRER, DbType.String, entity.TipoGenerRer);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            dbProvider.AddInParameter(command, helper.Grupotipocogen, DbType.String, entity.Grupotipocogen);
            dbProvider.AddInParameter(command, helper.Grupointegrante, DbType.String, entity.Grupointegrante);
            dbProvider.AddInParameter(command, helper.GrupoTension, DbType.Decimal, entity.Grupotension);
            dbProvider.AddInParameter(command, helper.GrupoNodoEnergetico, DbType.Int32, entity.Gruponodoenergetico);
            dbProvider.AddInParameter(command, helper.GrupoReservaFria, DbType.Int32, entity.Gruporeservafria);
            dbProvider.AddInParameter(command, helper.GrupoEstado, DbType.String, entity.GrupoEstado);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Curvcodi, DbType.Int32, entity.Curvcodi);
            dbProvider.AddInParameter(command, helper.GrupoUsuModificacion, DbType.String, entity.Grupousumodificacion);
            dbProvider.AddInParameter(command, helper.GrupoFecModificacion, DbType.DateTime, entity.Grupofecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupotipomodo, DbType.String, entity.Grupotipomodo);
            dbProvider.AddInParameter(command, helper.Grupousucreacion, DbType.String, entity.Grupousucreacion);
            dbProvider.AddInParameter(command, helper.Grupofeccreacion, DbType.DateTime, entity.Grupofeccreacion);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(PrGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Barracodi, DbType.Int32, entity.Barracodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Grupoabrev, DbType.String, entity.Grupoabrev);
            dbProvider.AddInParameter(command, helper.Grupovmax, DbType.Decimal, entity.Grupovmax);
            dbProvider.AddInParameter(command, helper.Grupovmin, DbType.Decimal, entity.Grupovmin);
            dbProvider.AddInParameter(command, helper.Grupoorden, DbType.Int32, entity.Grupoorden);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupotipo, DbType.String, entity.Grupotipo);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);
            dbProvider.AddInParameter(command, helper.Grupotipoc, DbType.Int32, entity.Grupotipoc);
            dbProvider.AddInParameter(command, helper.Grupopadre, DbType.Int32, entity.Grupopadre);
            dbProvider.AddInParameter(command, helper.Grupoactivo, DbType.String, entity.Grupoactivo);
            dbProvider.AddInParameter(command, helper.Grupocomb, DbType.String, entity.Grupocomb);
            dbProvider.AddInParameter(command, helper.Osicodi, DbType.String, entity.Osicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi2, DbType.Int32, entity.Grupocodi2);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Gruposub, DbType.String, entity.Gruposub);
            dbProvider.AddInParameter(command, helper.Barracodi2, DbType.Int32, entity.Barracodi2);
            dbProvider.AddInParameter(command, helper.Barramw1, DbType.Decimal, entity.Barramw1);
            dbProvider.AddInParameter(command, helper.Barramw2, DbType.Decimal, entity.Barramw2);
            dbProvider.AddInParameter(command, helper.Gruponombncp, DbType.String, entity.Gruponombncp);
            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, entity.Tipogrupocodi);
            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.TipoGrupoCodi2, DbType.Int32, entity.TipoGrupoCodi2);
            dbProvider.AddInParameter(command, helper.TipoGenerRER, DbType.String, entity.TipoGenerRer);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            dbProvider.AddInParameter(command, helper.Grupotipocogen, DbType.String, entity.Grupotipocogen);
            dbProvider.AddInParameter(command, helper.Grupointegrante, DbType.String, entity.Grupointegrante);
            dbProvider.AddInParameter(command, helper.GrupoTension, DbType.Decimal, entity.Grupotension);
            dbProvider.AddInParameter(command, helper.GrupoNodoEnergetico, DbType.Int32, entity.Gruponodoenergetico);
            dbProvider.AddInParameter(command, helper.GrupoReservaFria, DbType.Int32, entity.Gruporeservafria);
            dbProvider.AddInParameter(command, helper.GrupoEstado, DbType.String, entity.GrupoEstado);
            dbProvider.AddInParameter(command, helper.Areacodi, DbType.Int32, entity.Areacodi);
            dbProvider.AddInParameter(command, helper.Curvcodi, DbType.Int32, entity.Curvcodi);
            dbProvider.AddInParameter(command, helper.GrupoUsuModificacion, DbType.String, entity.Grupousumodificacion);
            dbProvider.AddInParameter(command, helper.GrupoFecModificacion, DbType.DateTime, entity.Grupofecmodificacion);
            dbProvider.AddInParameter(command, helper.Grupotipomodo, DbType.String, entity.Grupotipomodo);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete_UpdateAuditoria(int grupocodi, string username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete_UpdateAuditoria);

            dbProvider.AddInParameter(command, helper.GrupoUsuModificacion, DbType.String, username);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public PrGrupoDTO GetById(int grupocodi) 
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            PrGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iGrupoTension = dr.GetOrdinal(this.helper.GrupoTension);
                    if (!dr.IsDBNull(iGrupoTension)) entity.Grupotension = dr.GetDecimal(iGrupoTension);

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iGrupoCentral = dr.GetOrdinal(this.helper.GrupoCentral);
                    if (!dr.IsDBNull(iGrupoCentral)) entity.GrupoCentral = Convert.ToInt32(dr.GetValue(iGrupoCentral));

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iGrupotipomodo = dr.GetOrdinal(this.helper.Grupotipomodo);
                    if (!dr.IsDBNull(iGrupotipomodo)) entity.Grupotipomodo = dr.GetString(iGrupotipomodo);

                    int iGrupousucreacion = dr.GetOrdinal(this.helper.Grupousucreacion);
                    if (!dr.IsDBNull(iGrupousucreacion)) entity.Grupousucreacion = dr.GetString(iGrupousucreacion);

                    int iGrupofeccreacion = dr.GetOrdinal(this.helper.Grupofeccreacion);
                    if (!dr.IsDBNull(iGrupofeccreacion)) entity.Grupofeccreacion = dr.GetDateTime(iGrupofeccreacion);

                    int iGrupoUsuModificacion = dr.GetOrdinal(this.helper.GrupoUsuModificacion);
                    if (!dr.IsDBNull(iGrupoUsuModificacion)) entity.Grupousumodificacion = dr.GetString(iGrupoUsuModificacion);

                    int iGrupoFecModificacion = dr.GetOrdinal(this.helper.GrupoFecModificacion);
                    if (!dr.IsDBNull(iGrupoFecModificacion)) entity.Grupofecmodificacion = dr.GetDateTime(iGrupoFecModificacion);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iCateNomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCateNomb)) entity.Catenomb = dr.GetString(iCateNomb);
                }
            }

            return entity;
        }

        public List<PrGrupoDTO> List(string catecodis)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string sql = string.Format(helper.SqlList, catecodis);
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iTipoGenerRer = dr.GetOrdinal(helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListByEmprCodiAndCatecodi(int emprCodi, int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByEmprCodiAndCatecodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprCodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, catecodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iTipoGenerRer = dr.GetOrdinal(helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> GetByCriteria(int idTipoGrupo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);
            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, idTipoGrupo);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = helper.Create(dr);

                    int iDesTipoGrupo = dr.GetOrdinal(this.helper.DesTipoGrupo);
                    if (!dr.IsDBNull(iDesTipoGrupo)) entity.DesTipoGrupo = dr.GetString(iDesTipoGrupo);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iTipoGrupoCodi2 = dr.GetOrdinal(this.helper.TipoGrupoCodi2);
                    if (!dr.IsDBNull(iTipoGrupoCodi2)) entity.TipoGrupoCodi2 = Convert.ToInt32(dr.GetValue(iTipoGrupoCodi2));

                    int iTipoGenerRer = dr.GetOrdinal(this.helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public void CambiarTipoGrupo(int idGrupo, int idTipoGrupo, int idTipoGrupo2, string tipoGenerRER, string userName, DateTime fecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlCambiarTipoGrupo);

            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, idTipoGrupo);
            dbProvider.AddInParameter(command, helper.TipoGrupoCodi2, DbType.Int32, idTipoGrupo2);
            dbProvider.AddInParameter(command, helper.TipoGenerRER, DbType.String, tipoGenerRER);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, userName);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, fecha);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, idGrupo);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<GrupoGeneracionDTO> ListarGeneradoresDespachoOsinergmin()
        {
            var entitys = new List<GrupoGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarGeneradoresDespachoOsinergmin);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oGenerador = new GrupoGeneracionDTO();
                    oGenerador.Equifechfinopcom = dr.IsDBNull(dr.GetOrdinal("EQUIFECHFINOPCOM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("EQUIFECHFINOPCOM"));
                    oGenerador.Equifechiniopcom = dr.IsDBNull(dr.GetOrdinal("EQUIFECHINIOPCOM")) ? (DateTime?)null : dr.GetDateTime(dr.GetOrdinal("EQUIFECHINIOPCOM"));
                    oGenerador.Gruponomb = dr.GetString(dr.GetOrdinal("GRUPONOMB"));
                    oGenerador.Grupopadre = dr.IsDBNull(dr.GetOrdinal("GRUPOPADRE")) ? -1 : dr.GetInt32(dr.GetOrdinal("GRUPOPADRE"));
                    oGenerador.Grupocodi = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? -1 : dr.GetInt32(dr.GetOrdinal("GRUPOCODI"));
                    entitys.Add(oGenerador);
                }
            }
            return entitys;
        }


        public List<PrGrupoDTO> ListaModosOperacionActivos()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaModosOperacionActivos);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iTipoGenerRer = dr.GetOrdinal(this.helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int IGrupoCentral = dr.GetOrdinal(this.helper.GrupoCentral);
                    if (!dr.IsDBNull(IGrupoCentral)) entity.GrupoCentral = dr.GetInt32(IGrupoCentral);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListaModosOperacion()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaModosOperacion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int IGrupoCentral = dr.GetOrdinal(this.helper.GrupoCentral);
                    if (!dr.IsDBNull(IGrupoCentral)) entity.GrupoCentral = dr.GetInt32(IGrupoCentral);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iGrupotipomodo = dr.GetOrdinal(this.helper.Grupotipomodo);
                    if (!dr.IsDBNull(iGrupotipomodo)) entity.Grupotipomodo = dr.GetString(iGrupotipomodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListaModosOperacionActivosXCategoria(string catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(string.Format(helper.SqlListaModosOperacionActivosXCategoria, catecodi));

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        public int GruposxDespachoXFiltro(string empresas)
        {
            var strComando = string.Format(helper.SqlListGrupoDespachoXFiltro, empresas );
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<PrGrupoDTO> ListarGrupoDespacho()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            var strComando = string.Format(helper.SqlListarGrupoPadre);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iGrupoPadreNomr = dr.GetOrdinal("GRUPONOMBPADRE");
                    if (!dr.IsDBNull(iGrupoPadreNomr)) entity.GruponombPadre = dr.GetString(iGrupoPadreNomr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }



        public List<PrGrupoDTO> ListaGruposxDespacho(string empresas, int nroPaginas, int nroFilas)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            var strComando = string.Format(helper.SqlListGrupoDespacho, empresas, nroPaginas, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
    
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                     var entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmpresanomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmpresanomb)) entity.Emprnomb = dr.GetString(iEmpresanomb);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);

                    int iGrupopadre = dr.GetOrdinal("GRUPOCODIPADRE");
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iGrupoPadreNomr = dr.GetOrdinal("GRUPONOMBPADRE");
                    if (!dr.IsDBNull(iGrupoPadreNomr)) entity.GruponombPadre = dr.GetString(iGrupoPadreNomr);

                    int iLastuser = dr.GetOrdinal(this.helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    int iLastdate = dr.GetOrdinal(this.helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    int iCateNomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCateNomb)) entity.Catenomb = dr.GetString(iCateNomb);

                    int iGrupoEstado = dr.GetOrdinal(helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ModoOperacionDTO> ModoOperacionCentral1(int iCentral)
        {
            List<ModoOperacionDTO> lsResultado = new List<ModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlModosOperacionCentral1);
            dbProvider.AddInParameter(command, "idCentral", DbType.Int32, iCentral);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oModo = new ModoOperacionDTO();
                    oModo.EQUICODI = dr.IsDBNull(dr.GetOrdinal("EQUICODI")) ? 0 : dr.GetDecimal(dr.GetOrdinal("EQUICODI"));
                    oModo.GRUPOABREV = dr.IsDBNull(dr.GetOrdinal("GRUPOABREV")) ? "" : dr.GetString(dr.GetOrdinal("GRUPOABREV"));
                    oModo.GRUPOCODI = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? 0 : dr.GetDecimal(dr.GetOrdinal("GRUPOCODI"));
                    oModo.GRUPONOM = dr.IsDBNull(dr.GetOrdinal("MODONOM")) ? "" : dr.GetString(dr.GetOrdinal("MODONOM"));
                    //oModo.IDCENTRAL = dr.IsDBNull(dr.GetOrdinal("IDCENTRAL")) ? "" : dr.GetString(dr.GetOrdinal("IDCENTRAL"));
                    oModo.MODONOM = dr.IsDBNull(dr.GetOrdinal("GRUPONOMB")) ? "" : dr.GetString(dr.GetOrdinal("GRUPONOMB"));
                    lsResultado.Add(oModo);
                }
            }

            return lsResultado;
        }

        public List<ModoOperacionDTO> ModoOperacionCentral2(int iCentral)
        {
            List<ModoOperacionDTO> lsResultado = new List<ModoOperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlModosOperacionCentral2);
            dbProvider.AddInParameter(command, "idCentral", DbType.Int32, iCentral);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var oModo = new ModoOperacionDTO();
                    oModo.EQUICODI = dr.IsDBNull(dr.GetOrdinal("EQUICODI")) ? 0 : dr.GetDecimal(dr.GetOrdinal("EQUICODI"));
                    //oModo.GRUPOABREV = dr.IsDBNull(dr.GetOrdinal("GRUPOABREV")) ? "" : dr.GetString(dr.GetOrdinal("GRUPOABREV"));
                    oModo.GRUPOCODI = dr.IsDBNull(dr.GetOrdinal("GRUPOCODI")) ? 0 : dr.GetDecimal(dr.GetOrdinal("GRUPOCODI"));
                    //oModo.GRUPONOM = dr.IsDBNull(dr.GetOrdinal("GRUPONOM")) ? "" : dr.GetString(dr.GetOrdinal("GRUPONOM"));
                    //oModo.IDCENTRAL = dr.IsDBNull(dr.GetOrdinal("IDCENTRAL")) ? "" : dr.GetString(dr.GetOrdinal("IDCENTRAL"));
                    //oModo.MODONOM = dr.IsDBNull(dr.GetOrdinal("MODONOM")) ? "" : dr.GetString(dr.GetOrdinal("MODONOM"));
                    lsResultado.Add(oModo);
                }
            }

            return lsResultado;
        }

        public int ObtenerCodigoModoOperacionPadre(int iPrGrupo)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCodigoModoOperacionPadre);
            dbProvider.AddInParameter(command, "grupocodi", DbType.Int32, iPrGrupo);
            object result = dbProvider.ExecuteScalar(command);
            int id = -1;
            if (result != null) id = Convert.ToInt32(result);
            return id;
        }
        public List<PrGrupoDTO> ModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo, int nroPagina, int nroFilas)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            if (string.IsNullOrEmpty(sNombreModo)) sNombreModo = "";
            var strComando = string.Format(helper.SqlModosOperacionxFiltro, sEstado, idEmpresa, sNombreModo, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CantidadModosOperacionxFiltro(int idEmpresa, string sEstado, string sNombreModo)
        {
            if (string.IsNullOrEmpty(sNombreModo)) sNombreModo = "";
            var strComando = string.Format(helper.SqlCantidadModosOperacionxFiltro, sEstado, idEmpresa, sNombreModo);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public PrGrupoDTO ObtenerModoOperacion(int iGrupoCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerModoOperacion);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, iGrupoCodi);
            PrGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);
                }
            }

            return entity;
        }


        public List<PrGrupoConceptoDato> ListarDatosVigentesPorModoOperacion(string sGrupoCodi, DateTime dtFecha, bool bFichaTecnica)
        {
            var entitys = new List<PrGrupoConceptoDato>();
            string sFicha = "";
            if (bFichaTecnica)
                sFicha = "S";
            else
                sFicha = "*";
            var sCommand = string.Format(helper.SqlDatosVigentesPorModoOperacion, sGrupoCodi,
                dtFecha.ToString("yyyy-MM-dd hh:mm:ss"), sFicha);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoConceptoDato();
                    int iCONCEPDESC = dr.GetOrdinal("CONCEPDESC");
                    if (!dr.IsDBNull(iCONCEPDESC)) entity.Concepdesc = Convert.ToString(dr.GetValue(iCONCEPDESC));
                    int iCONCEPABREV = dr.GetOrdinal("CONCEPABREV");
                    if (!dr.IsDBNull(iCONCEPABREV)) entity.Concepabrev = Convert.ToString(dr.GetValue(iCONCEPABREV));
                    int iCONCEPORDEN = dr.GetOrdinal("CONCEPORDEN");
                    if (!dr.IsDBNull(iCONCEPORDEN)) entity.Conceporden = Convert.ToInt32(dr.GetValue(iCONCEPORDEN));
                    int iCONCEPCODI = dr.GetOrdinal("CONCEPCODI");
                    if (!dr.IsDBNull(iCONCEPCODI)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iCONCEPCODI));
                    int iCONCEPUNID = dr.GetOrdinal("CONCEPUNID");
                    if (!dr.IsDBNull(iCONCEPUNID)) entity.Concepunid = Convert.ToString(dr.GetValue(iCONCEPUNID));
                    int iFORMULADAT = dr.GetOrdinal("FORMULADAT");
                    if (!dr.IsDBNull(iFORMULADAT)) entity.Formuladat = Convert.ToString(dr.GetValue(iFORMULADAT));
                    int iGRUPOCODI = dr.GetOrdinal("GRUPOCODI");
                    if (!dr.IsDBNull(iGRUPOCODI)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGRUPOCODI));
                    int iGRUPOCATECODI = dr.GetOrdinal("GRUPOCATECODI");
                    if (!dr.IsDBNull(iGRUPOCATECODI)) entity.GrupoCatecodi = Convert.ToInt32(dr.GetValue(iGRUPOCATECODI));
                    int iLASTUSER = dr.GetOrdinal("LASTUSER");
                    if (!dr.IsDBNull(iLASTUSER)) entity.Lastuser = Convert.ToString(dr.GetValue(iLASTUSER));
                    int iFECHAACT = dr.GetOrdinal("FECHAACT");
                    if (!dr.IsDBNull(iFECHAACT)) entity.Fechaact = Convert.ToDateTime(dr.GetValue(iFECHAACT));
                    int iFECHADAT = dr.GetOrdinal("FECHADAT");
                    if (!dr.IsDBNull(iFECHADAT)) entity.Fechadat = Convert.ToDateTime(dr.GetValue(iFECHADAT));
                    int iCONCEPTOCATECODI = dr.GetOrdinal("CONCEPTOCATECODI");
                    if (!dr.IsDBNull(iCONCEPTOCATECODI)) entity.ConceptoCatecodi = Convert.ToInt32(dr.GetValue(iCONCEPTOCATECODI));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupoDTO> ListCentrales(string tipocombustible, string emprcodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListCentrales, tipocombustible, emprcodi);
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


        public List<ModoOperacionParametrosDTO> ListarModosOperacionParametros(int iGrupoCodi)
        {
            List<ModoOperacionParametrosDTO> entitys = new List<ModoOperacionParametrosDTO>();
            string query = string.Format(helper.sqlParametrosModoOperacionCompensacion, iGrupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new ModoOperacionParametrosDTO();
                    int iGCodi = dr.GetOrdinal("GRUPOCODI");
                    if (!dr.IsDBNull(iGCodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGCodi));
                    int iGRUPONOMB = dr.GetOrdinal("GRUPONOMB");
                    if (!dr.IsDBNull(iGRUPONOMB)) entity.Gruponomb = Convert.ToString(dr.GetValue(iGRUPONOMB));
                    int iCOMBUSTIBLE = dr.GetOrdinal("COMBUSTIBLE");
                    if (!dr.IsDBNull(iCOMBUSTIBLE)) entity.Combustible = Convert.ToString(dr.GetValue(iCOMBUSTIBLE));
                    int iPOT_MIN = dr.GetOrdinal("POT_MIN");
                    if (!dr.IsDBNull(iPOT_MIN)) entity.PotenciaMinima = Convert.ToDecimal(dr.GetValue(iPOT_MIN));
                    int iPOT_EFEC = dr.GetOrdinal("POT_EFEC");
                    if (!dr.IsDBNull(iPOT_EFEC)) entity.PotenciaEfectiva = Convert.ToDecimal(dr.GetValue(iPOT_EFEC));
                    int iA = dr.GetOrdinal("A");
                    if (!dr.IsDBNull(iA)) entity.A = Convert.ToDecimal(dr.GetValue(iA));
                    int iB = dr.GetOrdinal("B");
                    if (!dr.IsDBNull(iB)) entity.B = Convert.ToDecimal(dr.GetValue(iB));
                    int iC = dr.GetOrdinal("C");
                    if (!dr.IsDBNull(iC)) entity.B = Convert.ToDecimal(dr.GetValue(iC));
                    int iLHV = dr.GetOrdinal("LHV");
                    if (!dr.IsDBNull(iLHV)) entity.Lhv = Convert.ToDecimal(dr.GetValue(iLHV));
                    int iEFIC_NOM = dr.GetOrdinal("EFIC_NOM");
                    if (!dr.IsDBNull(iEFIC_NOM)) entity.RendimientoNominal = Convert.ToDecimal(dr.GetValue(iEFIC_NOM));
                    int iCE_CALOR = dr.GetOrdinal("CE_CALOR");
                    if (!dr.IsDBNull(iCE_CALOR)) entity.RendimientoNominal = Convert.ToDecimal(dr.GetValue(iCE_CALOR));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<ModoOperacionCostosDTO> ListarModosOperacionCostos()
        {
            throw new NotImplementedException();
        }

        public List<PrGrupoDTO> ObtenerTipoCombustiblePorCentral()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerTipoCombustiblePorCentral);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Método que devuelve el listado de modos funcionales térmicos activos
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListadoModosFuncionalesTermicosActivos()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListadoModosFuncionalesTermicosActivos);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGrupoactivo = dr.GetOrdinal(helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);
                    int iGrupovmax = dr.GetOrdinal(helper.Grupovmax);
                    if (!dr.IsDBNull(iGrupovmax)) entity.Grupovmax = dr.GetDecimal(iGrupovmax);
                    int iGrupovmin = dr.GetOrdinal(helper.Grupovmin);
                    if (!dr.IsDBNull(iGrupovmin)) entity.Grupovmin = dr.GetDecimal(iGrupovmin);
                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));
                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);
                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);
                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));
                    int iGrupoCentral = dr.GetOrdinal("GRUPOCENTRAL");
                    if (!dr.IsDBNull(iGrupoCentral)) entity.GrupoCentral = Convert.ToInt32(dr.GetValue(iGrupoCentral));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrGrupoDTO> ObtenerArbolGrupoDespacho(string empresas, string tipoCentral)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlObtenerArbolGrupoDespacho, empresas, tipoCentral);
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

        public List<PrGrupoDTO> ObtenerCentralesPorTipo(string tipoGrupo, string empresas)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlObtenerCentralesPorGrupo, tipoGrupo, empresas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrGrupoDTO> ListadoModosFuncionalesCostosVariables(DateTime fecha, string flagGrupoActivo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string sConsulta = string.Format(helper.SqlListadoModosFuncionalesCostosVariables, fecha.ToString("yyyy-MM-dd hh:mm:ss"), flagGrupoActivo);
            DbCommand command = dbProvider.GetSqlStringCommand(sConsulta);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iGrupoactivo = dr.GetOrdinal(helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);
                    int iGrupovmax = dr.GetOrdinal(helper.Grupovmax);
                    if (!dr.IsDBNull(iGrupovmax)) entity.Grupovmax = dr.GetDecimal(iGrupovmax);
                    int iGrupovmin = dr.GetOrdinal(helper.Grupovmin);
                    if (!dr.IsDBNull(iGrupovmin)) entity.Grupovmin = dr.GetDecimal(iGrupovmin);
                    int iGrupoorden = dr.GetOrdinal(helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));
                    int iGrupotipo = dr.GetOrdinal(helper.Grupotipo);
                    if (!dr.IsDBNull(iGrupotipo)) entity.Grupotipo = dr.GetString(iGrupotipo);
                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);
                    int iBarracodi = dr.GetOrdinal(helper.Barracodi);
                    if (!dr.IsDBNull(iBarracodi)) entity.Barracodi = Convert.ToInt32(dr.GetValue(iBarracodi));
                    int iGrupoCentral = dr.GetOrdinal("GRUPOCENTRAL");
                    if (!dr.IsDBNull(iGrupoCentral)) entity.GrupoCentral = Convert.ToInt32(dr.GetValue(iGrupoCentral));
                    int iGrupoPadreNomr = dr.GetOrdinal("GRUPONOMBPADRE");
                    if (!dr.IsDBNull(iGrupoPadreNomr)) entity.GruponombPadre = dr.GetString(iGrupoPadreNomr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<PrGrupoDTO> ListarModosOperacionNoConfigurados(int idEmpresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string sConsulta = string.Format(helper.SqlModosOperacionNoConfigurados, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sConsulta);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListarModosOperacionConfigurados(int idEmpresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string sConsulta = string.Format(helper.SqlModosOperacionConfigurados, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(sConsulta);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);
                    int iEmprnomb = dr.GetOrdinal("EMPRNOMB");
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }


        public List<DetalleReportePotenciaDTO> DatosReportePotenciaEfectivaTermicas(int iEmpresa, int iCentral, DateTime fechaIni, DateTime fechaFin, string tipo)
        {
            List<DetalleReportePotenciaDTO> resultado = new List<DetalleReportePotenciaDTO>();
            string sSubQuery = "";
            if (iCentral != -2)
            {
                sSubQuery = " and ppp.grupocodi in (select eq.grupocodi from eq_equipo eq where eq.equicodi=" + iCentral + ")";
            }
            if (tipo.Trim().ToUpper() == "C")// En caso de ser consolidado filtar por tabla de configuración.
            {
                sSubQuery = sSubQuery + " and p.grupocodi in (select cpe.grupocodi from PR_CONFIGURACION_POT_EFECTIVA cpe)";
            }
            string sConsultaFechas = "";
            var lPeriodos = EPDate.GetPeriodos(fechaIni, fechaFin);
            int iCantPeriodos = 0;
            foreach (var oPeriodo in lPeriodos)
            {
                sConsultaFechas = sConsultaFechas + ",fn_sdatoconceptofechas(p.grupocodi,14,to_date('" + oPeriodo.FechaInicio.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'),to_date('" + oPeriodo.FechaFin.ToString("YYYY-MM-DD") + "','YYYY-MM-DD'))as pe" + iCantPeriodos;
                iCantPeriodos++;
            }
            string sQuery = string.Format(helper.SqlReportePotenciaEfectivaTermicas, iEmpresa, sSubQuery, sConsultaFechas);
            var command = dbProvider.GetSqlStringCommand(sQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DetalleReportePotenciaDTO oAux = new DetalleReportePotenciaDTO();
                    oAux.Central = dr.GetString(dr.GetOrdinal("Central"));
                    oAux.Empresa = dr.GetString(dr.GetOrdinal("emprnomb"));
                    oAux.GeneradorModo = dr.GetString(dr.GetOrdinal("equinomb"));
                    oAux.Combustible = dr.GetString(dr.GetOrdinal("grupocomb"));
                    oAux.Potencia = new List<decimal?>();
                    for (int i = 0; i < lPeriodos.Count; i++)
                    {
                        oAux.Potencia.Add(dr.GetDecimal(dr.GetOrdinal("pe" + i)));
                    }
                    resultado.Add(oAux);
                }
            }
            return resultado;
        }

        public List<PrGrupoDTO> ListarModoOperacionCategoriaTermico()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarModoOperacionCategoriaTermico);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = Convert.ToString(dr.GetValue(iGruponomb));


                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        //- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public List<PrGrupoDTO> ListarGruposPorCategoria(List<int> lIdCategori)
        {
            string listaInCategoria = string.Empty;
            foreach (int idCategoria in lIdCategori)
            {
                if (listaInCategoria == string.Empty)
                {
                    listaInCategoria = idCategoria.ToString();
                }
                else
                {
                    listaInCategoria = listaInCategoria + ", " + idCategoria.ToString();
                }
            }

            List<PrGrupoDTO> entities = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlGruposXCategoria, listaInCategoria);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            PrGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iFueosinergcodi = dr.GetOrdinal(this.helper.Fueosinergcodi);
                    if (!dr.IsDBNull(iFueosinergcodi)) entity.Fueosinergcodi = dr.GetString(iFueosinergcodi);

                    entities.Add(entity);
                }
            }

            return entities;
        }

        //- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public void UpdateOsinergmin(PrGrupoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateOsinergmin);

            dbProvider.AddInParameter(command, helper.Fenergcodi, DbType.Int32, entity.Fenergcodi);
            dbProvider.AddInParameter(command, helper.Barracodi, DbType.Int32, entity.Barracodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Grupoabrev, DbType.String, entity.Grupoabrev);
            dbProvider.AddInParameter(command, helper.Grupovmax, DbType.Decimal, entity.Grupovmax);
            dbProvider.AddInParameter(command, helper.Grupovmin, DbType.Decimal, entity.Grupovmin);
            dbProvider.AddInParameter(command, helper.Grupoorden, DbType.Int32, entity.Grupoorden);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Grupotipo, DbType.String, entity.Grupotipo);
            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, entity.Catecodi);
            dbProvider.AddInParameter(command, helper.Grupotipoc, DbType.Int32, entity.Grupotipoc);
            dbProvider.AddInParameter(command, helper.Grupopadre, DbType.Int32, entity.Grupopadre);
            dbProvider.AddInParameter(command, helper.Grupoactivo, DbType.String, entity.Grupoactivo);
            dbProvider.AddInParameter(command, helper.Grupocomb, DbType.String, entity.Grupocomb);
            dbProvider.AddInParameter(command, helper.Osicodi, DbType.String, entity.Osicodi);
            dbProvider.AddInParameter(command, helper.Grupocodi2, DbType.Int32, entity.Grupocodi2);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Gruposub, DbType.String, entity.Gruposub);
            dbProvider.AddInParameter(command, helper.Barracodi2, DbType.Int32, entity.Barracodi2);
            dbProvider.AddInParameter(command, helper.Barramw1, DbType.Decimal, entity.Barramw1);
            dbProvider.AddInParameter(command, helper.Barramw2, DbType.Decimal, entity.Barramw2);
            dbProvider.AddInParameter(command, helper.Gruponombncp, DbType.String, entity.Gruponombncp);
            dbProvider.AddInParameter(command, helper.Tipogrupocodi, DbType.Int32, entity.Tipogrupocodi);
            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, entity.Osinergcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        //- alpha.HDT - 30/10/2016: Cambio para atender el requerimiento. 
        public PrGrupoDTO GetByIdOsinergmin(int grupocodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.GetByIdOsinergmin);

            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            PrGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);
                }
            }

            return entity;
        }

        public List<PrGrupoDTO> ListarModoOperacionSubModulo(int subModulo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarModoOperacionSubModulo, subModulo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = Convert.ToString(dr.GetValue(iGrupoabrev));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = Convert.ToString(dr.GetValue(iGruponomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region "COSTO OPORTUNIDAD"
        public PrGrupoDTO GetByIdNCP(int grupocodincp)
        {
            string query = string.Format(helper.SqlGetByIdNCP, grupocodincp);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            PrGrupoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public List<PrGrupoDTO> GetByListaModosOperacionNCP()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.sqlGetByListaModosOperacionNCP);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    int iPtomedicodi = dr.GetOrdinal("PTOMEDICODI");
                    if (!dr.IsDBNull(iPtomedicodi)) entity.PtoMediCodi = Convert.ToInt32(dr.GetValue(iPtomedicodi));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupodatDTO> GetListaPotenciaEfectiva(DateTime dfecha, int porcentajerpf, int origlectcodi)
        {
            PrGrupodatDTO entity = null;
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();
            string query = string.Format(helper.sqlGetListaPotenciaEfectiva, dfecha.ToString(ConstantesBase.FormatoFecha), porcentajerpf, origlectcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupodatDTO();
                    entity.Centralnomb = dr.GetString(dr.GetOrdinal("gruponombncp"));
                    entity.Fechadat = dr.GetDateTime(dr.GetOrdinal("fechadat"));
                    entity.Formuladat = dr.GetString(dr.GetOrdinal("formuladat"));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupoDTO> ListaCompensacionGrupo(int pecacodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListaCompensacionGrupo, pecacodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<PrGrupoDTO> GetByIdGrupoPadre(int grupourspadre)
        {
            string query = string.Format(helper.SqlGetByIdGrupoPadre, grupourspadre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<PrGrupoDTO> entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                entity = new List<PrGrupoDTO>();
                while (dr.Read())
                {
                    entity.Add(helper.Create(dr));
                }
            }
            return entity;
        }
        #endregion

        #region PR5

        /// <summary>
        /// Listar todas las unidades termoeléctricas indicando si la central es especial o no
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllUnidadTermoelectrica()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarAllUnidadTermoelectrica);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFlagModoEspecial = dr.GetOrdinal(helper.FlagModoEspecial);
                    if (!dr.IsDBNull(iFlagModoEspecial)) entity.FlagModoEspecial = dr.GetString(iFlagModoEspecial);
                    int iGrupocodimodo = dr.GetOrdinal(helper.Grupocodimodo);
                    if (!dr.IsDBNull(iGrupocodimodo)) entity.Grupocodimodo = dr.GetInt32(iGrupocodimodo);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListarAllUnidadTermoelectricaModoEspecial()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarAllUnidadTermoelectrica);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprNomb = dr.GetOrdinal(this.helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.EmprNomb = dr.GetString(iEmprNomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iFlagModoEspecial = dr.GetOrdinal(helper.FlagModoEspecial);
                    if (!dr.IsDBNull(iFlagModoEspecial)) entity.FlagModoEspecial = dr.GetString(iFlagModoEspecial);
                    int iGrupocodimodo = dr.GetOrdinal(helper.Grupocodimodo);
                    if (!dr.IsDBNull(iGrupocodimodo)) entity.Grupocodimodo = dr.GetInt32(iGrupocodimodo);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Listar todas los grupo RER, tipogenerrer = 'S'
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllGrupoRER(DateTime fechaPeriodo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarAllGrupoRER, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iGrupoactivo = dr.GetOrdinal(this.helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Listar todas los grupo CoGeneracion, GRUPOTIPOCOGEN = 'S'
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllGrupoCoGeneracion(DateTime fechaPeriodo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarAllGrupoCoGeneracion, fechaPeriodo.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt16(dr.GetValue(iTgenercodi));
                    int iTgenernomb = dr.GetOrdinal(this.helper.Tgenernomb);
                    if (!dr.IsDBNull(iTgenernomb)) entity.Tgenernomb = dr.GetString(iTgenernomb);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iGrupoactivo = dr.GetOrdinal(this.helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        /// <summary>
        /// Listar Todos los grupos de generacion GR.CATECODI in (3,5) and grupoactivo = 'S', y si es Integrante o no segun la fecha
        /// </summary>
        /// <returns></returns>
        public List<PrGrupoDTO> ListarAllGrupoGeneracion(DateTime fechaConsulta, string grupoactivo, string emprestado)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarAllGrupoGeneracion, fechaConsulta.ToString(ConstantesBase.FormatoFecha), grupoactivo, emprestado);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(this.helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprorden = dr.GetOrdinal(this.helper.Emprorden);
                    if (!dr.IsDBNull(iEmprorden)) entity.Emprorden = Convert.ToInt32(dr.GetValue(iEmprorden));

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGrupoorden = dr.GetOrdinal(this.helper.Grupoorden);
                    if (!dr.IsDBNull(iGrupoorden)) entity.Grupoorden = Convert.ToInt32(dr.GetValue(iGrupoorden));

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iTipoGenerRer = dr.GetOrdinal(this.helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iTgenercodi = dr.GetOrdinal(this.helper.Tgenercodi);
                    if (!dr.IsDBNull(iTgenercodi)) entity.Tgenercodi = Convert.ToInt32(dr.GetValue(iTgenercodi));

                    int iGrupoactivo = dr.GetOrdinal(this.helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iGrupointegrante = dr.GetOrdinal(this.helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        //-Pruebas aleatorias
        public List<PrGrupoDTO> ListarModoOperacionDeEquipo(int equicodi, int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListaModoOperacionDeEquipo, equicodi, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = Convert.ToString(dr.GetValue(iGruponomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region INDISPONIBILIDADES

        public List<PrGrupoDTO> ListaPrGrupoCC()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListaPrGrupoCC);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();
                    entity.Grupocodicc = dr.GetInt32(dr.GetOrdinal("grupocodicc"));
                    entity.Grupocodidet = dr.GetInt32(dr.GetOrdinal("grupocodidet"));
                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        public List<PrGrupoDTO> GetListaUnidadesXModoOperacionIndisponibilidad(int IdCentral)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListaUnidadesXModoOperacionIndisponibilidad, IdCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iIdtv = dr.GetOrdinal("IDTV");
                    if (!dr.IsDBNull(iIdtv)) entity.Idtv = Convert.ToInt32(dr.GetValue(iIdtv));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergpadre = dr.GetOrdinal(helper.Fenergpadre);
                    if (!dr.IsDBNull(iFenergpadre)) entity.Fenergpadre = Convert.ToInt32(dr.GetValue(iFenergpadre));
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    entitys.Add(entity);
                }
            }
            return entitys;

        }

        public List<PrGrupoDTO> ListaByGrupopadre(int equipadre)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            PrGrupoDTO entity = null;
            string query = string.Format(helper.SqlListaByGrupopadre, equipadre);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();
                    entity = helper.Create(dr);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = Convert.ToString(dr.GetValue(iEquinomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Barras Modeladas
        public List<PrGrupoDTO> ListaPrGruposPorCategoriaPaginado(int iCatecodi, string sEstado, int nroPagina, int nroFilas)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListGrupoPorCategoriaPaginado, iCatecodi, sEstado, nroPagina, nroFilas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGrupoTension = dr.GetOrdinal("GRUPOTENSION");
                    if (!dr.IsDBNull(iGrupoTension)) entity.Grupotension = Convert.ToDecimal(dr.GetValue(iGrupoTension));

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        public List<PrGrupoDTO> ListarModoOperacionXFamiliaAndEmpresa(string iCodFamilias, string iEmpresas)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarModoOperacionXFamiliaAndEmpresa, iCodFamilias, iEmpresas);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #region Transferencia de Equipos
        public List<PrGrupoDTO> ListarPrGruposXEmpresa(int idEmpresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string strComando = string.Format(helper.SqlListadoPrGrupoByEmpresa, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }
            return entitys;
        }
        #endregion

        #region Curva de consumo de combustible

        public List<PrGrupoDTO> ObtenerCentralesPorTipoCurva(string tipoGrupo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlObtenerCentralesPorGrupoCurva, tipoGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ObtenerGruposPorCodigo(string codigoGrupo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlObtenerGruposPorCodigo, codigoGrupo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListarDetalleGrupoCurva(int CurvCodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarDetalleGrupoCurva, CurvCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = Convert.ToString(dr.GetValue(iGruponomb));

                    int iCurvcodi = dr.GetOrdinal(helper.Curvcodi);
                    if (!dr.IsDBNull(iCurvcodi)) entity.Curvcodi = Convert.ToInt32(dr.GetValue(iCurvcodi));

                    int iCurvGrupocodiPrincipal = dr.GetOrdinal(helper.CURVGRUPOCODIPRINCIPAL);
                    if (!dr.IsDBNull(iCurvGrupocodiPrincipal)) entity.CurvGrupocodiPrincipal = Convert.ToInt32(dr.GetValue(iCurvGrupocodiPrincipal));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNCP(int CurvCodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlObtenerNCP, CurvCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {

                    int iGrupocodiNCP = dr.GetOrdinal(helper.GRUPOCODINCP);
                    if (!dr.IsDBNull(iGrupocodiNCP))
                        return Convert.ToInt32(dr.GetValue(iGrupocodiNCP));
                    else
                        return 0;
                }
            }

            return 0;
        }

        public void UpdateNCP(int grupoCodiNCP, int grupoCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateNCP);

            dbProvider.AddInParameter(command, helper.GRUPOCODINCP, DbType.Int32, grupoCodiNCP);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupoCodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        #region MigracionSGOCOES-GrupoB

        /// <summary>
        /// Listar los grupos por filtros, segun estado a la fecha
        /// </summary>
        /// <param name="iEmpresa"></param>
        /// <param name="iCatecodi"></param>
        /// <param name="nombre"></param>
        /// <param name="sEstado"></param>
        /// <param name="nroPagina"></param>
        /// <param name="nroFilas"></param>
        /// <param name="fechaDat"></param>
        /// <returns></returns>
        public List<PrGrupoDTO> ListaPrGruposPaginado(int iEmpresa, string iCatecodi, string nombre, string sEstado, int nroPagina, int nroFilas, DateTime fechaDat, int esReservaFria, int esNodoEnergetico)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListaPrGruposPaginado, iEmpresa, iCatecodi, nombre, sEstado, nroPagina, nroFilas, esReservaFria, esNodoEnergetico);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);
                    int iGrupoTension = dr.GetOrdinal(this.helper.GrupoTension);
                    if (!dr.IsDBNull(iGrupoTension)) entity.Grupotension = Convert.ToDecimal(dr.GetValue(iGrupoTension));

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iCatenomb = dr.GetOrdinal(this.helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iOsinergcodi = dr.GetOrdinal(this.helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int TotalPrGrupos(int iEmpresa, string iCatecodi, string nombre, string sEstado, DateTime fechaDat, int esReservaFria, int esNodoEnergetico)
        {
            string query = String.Format(helper.SqlTotalPrGrupos, iEmpresa, iCatecodi, nombre, sEstado, esReservaFria, esNodoEnergetico);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 1;
        }

        #endregion
        //Compensaciones
        public List<PrGrupoDTO> GetListaModosOperacion(int perianiomes, int pecacodi, int pecacodianterior)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlGetListModosOperacion, perianiomes, pecacodi, pecacodianterior);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCalificacion = dr.GetOrdinal(helper.Calificacion);
                    if (!dr.IsDBNull(iCalificacion)) entity.Calificacion = dr.GetString(iCalificacion);

                    int iAccionCalculo = dr.GetOrdinal(helper.AccionCalculo);
                    if (!dr.IsDBNull(iAccionCalculo)) entity.AccionCalculo = dr.GetString(iAccionCalculo);

                    int iHorasOperacion = dr.GetOrdinal(helper.HorasOperacion);
                    if (!dr.IsDBNull(iHorasOperacion)) entity.HorasOperacion = dr.GetDecimal(iHorasOperacion);

                    int iGrupoactivo = dr.GetOrdinal(helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoactivo)) entity.Grupoactivo = dr.GetString(iGrupoactivo);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupoDTO> GetListaModosIds(string fechadatos)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlGetListModosIds, fechadatos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<PrGrupoDTO> ListarMOXensayo(int idEnsayo)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            String sql = String.Format(helper.SqlListarMOXEnsayo, idEnsayo);
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

        public List<PrGrupoDTO> ListarUnidadesWithModoOperacionXCentralYEmpresa(int idCentral, string idEmpresa)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string query = string.Format(helper.SqlListarUnidadesWithModoOperacionXCentralYEmpresa, idCentral, idEmpresa);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new PrGrupoDTO();
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));
                    int iIdtv = dr.GetOrdinal(helper.Idtv);
                    if (!dr.IsDBNull(iIdtv)) entity.Idtv = Convert.ToInt32(dr.GetValue(iIdtv));
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));
                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);
                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));
                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));
                    int iFenergpadre = dr.GetOrdinal(helper.Fenergpadre);
                    if (!dr.IsDBNull(iFenergpadre)) entity.Fenergpadre = Convert.ToInt32(dr.GetValue(iFenergpadre));
                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #region Titularidad-Instalaciones-Empresas

        public List<PrGrupoDTO> ListarGrupoByMigracodi(int idMigracion)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string strComando = string.Format(helper.SqlListarGrupoByMigracodi, idMigracion);
            DbCommand command = dbProvider.GetSqlStringCommand(strComando);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprnombOrigen = dr.GetOrdinal(this.helper.EmprnombOrigen);
                    if (!dr.IsDBNull(iEmprnombOrigen)) entity.EmprnombOrigen = dr.GetString(iEmprnombOrigen);

                    int iGrupoEstado = dr.GetOrdinal(this.helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    int iCatenomb = dr.GetOrdinal(this.helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    entitys.Add(entity);
                }

            }

            return entitys;
        }

        #endregion

        #region Numerales Datos Base
        public List<PrGrupoDTO> ListaNumerales_DatosBase_5_6_3()
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_3);

            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);


                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iComb = dr.GetOrdinal(helper.Comb);
                    if (!dr.IsDBNull(iComb)) entity.Comb = dr.GetString(iComb);

                    int iLastdate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastdate)) entity.Lastdate = dr.GetDateTime(iLastdate);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region SIOSEIN

        public List<PrGrupoDTO> ListByIds(string grupocodi)
        {
            var query = string.Format(helper.SqlListByIds, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            PrGrupoDTO entity;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoGenerRer = dr.GetOrdinal(helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerRer)) entity.TipoGenerRer = dr.GetString(iTipoGenerRer);

                    int iGrupoCentral = dr.GetOrdinal(helper.GrupoCentral);
                    if (!dr.IsDBNull(iGrupoCentral)) entity.GrupoCentral = dr.GetInt32(iGrupoCentral);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region Pronóstico de la demanda

        //ASSETEC - 20200106
        public List<PrGrupoDTO> ListaBarraCategoria(int catecodi)
        {
            PrGrupoDTO entity = new PrGrupoDTO();
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlBarraCategoria);

            dbProvider.AddInParameter(command, helper.Catecodi, DbType.Int32, catecodi);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        #region Cálculo de Consumo de Combustible

        public void UpdateOsinergcodi(int grupocodi, string osinergcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateOsinergcodi);

            dbProvider.AddInParameter(command, helper.Osinergcodi, DbType.String, osinergcodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        #endregion

        #region Mejoras Yupana

        public List<PrGrupoDTO> ListaEquiposXModoOperacion(string idsEquipos)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            PrGrupoDTO entity;
            string query = string.Format(helper.SqlListaEquiposXModoOperacion, idsEquipos);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));
                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #endregion

        public void UpdateSddp(int grupocodi, int grupocodisddp)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdateSddp);

            dbProvider.AddInParameter(command, helper.GrupoCodiSDDP, DbType.Int32, grupocodisddp);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public List<PrGrupoDTO> ListaBarrasByCodigos(string codigos)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            string query = string.Format(helper.SqlListaBarrasByCodigos, codigos);

            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Notificacion de Grupos Despacho

        public List<PrGrupoDTO> ListadoGruposModificados(DateTime dtFechaInicio, DateTime dtFechaFin)
        {
            string query = string.Format(helper.SqlGruposModificados, dtFechaInicio.ToString("dd-MM-yyyy HH:mm"), dtFechaFin.ToString("dd-MM-yyyy HH:mm"));
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iGrupoPadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupoPadre)) entity.GruponombPadre = dr.GetString(iGrupoPadre);

                    int iEmprnomb = dr.GetOrdinal(helper.EmprNomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.EmprNomb = dr.GetString(iEmprnomb);

                    int iCatenomb = dr.GetOrdinal(helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iTipogruponomb = dr.GetOrdinal(helper.DesTipoGrupo);
                    if (!dr.IsDBNull(iTipogruponomb)) entity.DesTipoGrupo = dr.GetString(iTipogruponomb);

                    int iTipoGenerrer = dr.GetOrdinal(helper.TipoGenerRER);
                    if (!dr.IsDBNull(iTipoGenerrer)) entity.TipoGenerRer = dr.GetString(iTipoGenerrer);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    int iGrupointegrante = dr.GetOrdinal(helper.Grupointegrante);
                    if (!dr.IsDBNull(iGrupointegrante)) entity.Grupointegrante = dr.GetString(iGrupointegrante);

                    int iGrupoTipocogen = dr.GetOrdinal(helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupoTipocogen)) entity.Grupotipocogen = dr.GetString(iGrupoTipocogen);

                    int iGrupoNodoEnergetico = dr.GetOrdinal(helper.GrupoNodoEnergetico);
                    if (!dr.IsDBNull(iGrupoNodoEnergetico)) entity.Gruponodoenergetico = dr.GetInt32(iGrupoNodoEnergetico);

                    int iGrupoReservaFria = dr.GetOrdinal(helper.GrupoReservaFria);
                    if (!dr.IsDBNull(iGrupoNodoEnergetico)) entity.Gruporeservafria = dr.GetInt32(iGrupoReservaFria);

                    int iGrupoActivo = dr.GetOrdinal(helper.Grupoactivo);
                    if (!dr.IsDBNull(iGrupoActivo)) entity.Grupoactivo = dr.GetString(iGrupoActivo);

                    int iGrupoEstado = dr.GetOrdinal(helper.GrupoEstado);
                    if (!dr.IsDBNull(iGrupoEstado)) entity.GrupoEstado = dr.GetString(iGrupoEstado);

                    int iLastDate = dr.GetOrdinal(helper.Lastdate);
                    if (!dr.IsDBNull(iLastDate)) entity.Lastdate = dr.GetDateTime(iLastDate);

                    int iLastUser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastUser)) entity.Lastuser = dr.GetString(iLastUser);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion

        #region Ficha tecnica 2023

        public PrGrupoDTO ObtenerDatosGrupo(int grupocodi)
        {            
            PrGrupoDTO entity = null;
            string query = string.Format(helper.SqlGetDatosGrupo, grupocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);


            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprNomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprNomb)) entity.Emprnomb = dr.GetString(iEmprNomb);

                    int iCatenomb = dr.GetOrdinal(this.helper.Catenomb);
                    if (!dr.IsDBNull(iCatenomb)) entity.Catenomb = dr.GetString(iCatenomb);

                    int iAreadesc = dr.GetOrdinal(this.helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                }
            }

            return entity;
        }

        public List<PrGrupoDTO> ListarPorEmpresaPropietaria(int emprcodi, int catecodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            string query = string.Format(helper.SqlListarPorEmpresaPropietaria, emprcodi, catecodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = Convert.ToInt32(dr.GetValue(iCatecodi));

                    int iIdelemento = dr.GetOrdinal(helper.Idelemento);
                    if (!dr.IsDBNull(iIdelemento)) entity.Idelemento = Convert.ToInt32(dr.GetValue(iIdelemento));

                    int iIdempresaelemento = dr.GetOrdinal(helper.Idempresaelemento);
                    if (!dr.IsDBNull(iIdempresaelemento)) entity.Idempresaelemento = Convert.ToInt32(dr.GetValue(iIdempresaelemento));

                    int iNombempresaelemento = dr.GetOrdinal(helper.Nombempresaelemento);
                    if (!dr.IsDBNull(iNombempresaelemento)) entity.Nombempresaelemento = dr.GetString(iNombempresaelemento);

                    int iIdempresacopelemento = dr.GetOrdinal(helper.Idempresacopelemento);
                    if (!dr.IsDBNull(iIdempresacopelemento)) entity.Idempresacopelemento = Convert.ToInt32(dr.GetValue(iIdempresacopelemento));

                    int iNombempresacopelemento = dr.GetOrdinal(helper.Nombempresacopelemento);
                    if (!dr.IsDBNull(iNombempresacopelemento)) entity.Nombempresacopelemento = dr.GetString(iNombempresacopelemento);

                    int iNombreelemento = dr.GetOrdinal(helper.Nombreelemento);
                    if (!dr.IsDBNull(iNombreelemento)) entity.Nombreelemento = dr.GetString(iNombreelemento);

                    int iTipoelemento = dr.GetOrdinal(helper.Tipoelemento);
                    if (!dr.IsDBNull(iTipoelemento)) entity.Tipoelemento = dr.GetString(iTipoelemento);

                    int iAreaelemento = dr.GetOrdinal(helper.Areaelemento);
                    if (!dr.IsDBNull(iAreaelemento)) entity.Areaelemento = dr.GetString(iAreaelemento);

                    int iEstadoelemento = dr.GetOrdinal(helper.Estadoelemento);
                    if (!dr.IsDBNull(iEstadoelemento)) entity.Estadoelemento = dr.GetString(iEstadoelemento);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        //MERGE ENEL
        public List<ActualizacionCVDTO> ObtenerActualizacionesCostos(DateTime dtFechaInicio, DateTime dtFechaFin)
        {

            var lista = new List<ActualizacionCVDTO>();
            string query = string.Format(helper.SqlActualizacionesCostosVariables, dtFechaInicio.ToString("yyyy-MM-dd"),
                dtFechaFin.ToString("yyyy-MM-dd"));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new ActualizacionCVDTO();
                    entity.Codigo = Convert.ToInt32(dr.GetValue(dr.GetOrdinal("REPCODI")));
                    entity.Detalle = Convert.ToString(dr.GetValue(dr.GetOrdinal("REPDETALLE")));
                    entity.Fecha = Convert.ToDateTime(dr.GetValue(dr.GetOrdinal("REPFECHA")));
                    entity.Nombre = Convert.ToString(dr.GetValue(dr.GetOrdinal("REPNOMB")));
                    entity.Tipo = Convert.ToString(dr.GetValue(dr.GetOrdinal("REPTIPO")));
                    lista.Add(entity);
                }
            }
            return lista;
        }

        public List<CostoVariableDTO> ObtenerCostosVariablesPorActualizacion(int iCodActualizacion)
        {
            String query = String.Format(helper.SqlCostosVariablesxActualizacion, iCodActualizacion);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            List<CostoVariableDTO> lista = new List<CostoVariableDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = new CostoVariableDTO();
                    entity.CCOMB = dr.IsDBNull(dr.GetOrdinal("CCOMB")) ? 0 : dr.GetDecimal(dr.GetOrdinal("CCOMB"));
                    entity.CVC = dr.IsDBNull(dr.GetOrdinal("CVC")) ? 0 : dr.GetDecimal(dr.GetOrdinal("CVC"));
                    entity.CVNC = dr.IsDBNull(dr.GetOrdinal("CVNC")) ? 0 : dr.GetDecimal(dr.GetOrdinal("CVNC"));
                    entity.EFICBTUKWH = dr.IsDBNull(dr.GetOrdinal("EFICBTUKWH")) ? 0 : dr.GetDecimal(dr.GetOrdinal("EFICBTUKWH"));
                    entity.EFICTERM = dr.IsDBNull(dr.GetOrdinal("EFICTERM")) ? 0 : dr.GetDecimal(dr.GetOrdinal("EFICTERM"));
                    entity.ESCENARIO = dr.GetString(dr.GetOrdinal("ESCENARIO"));
                    entity.MODO_OPERACION = dr.GetString(dr.GetOrdinal("GRUPO"));
                    entity.PE = dr.IsDBNull(dr.GetOrdinal("PE")) ? 0 : dr.GetDecimal(dr.GetOrdinal("PE"));
                    lista.Add(entity);
                }
            }

            return lista;
        }

        #endregion
    }
}

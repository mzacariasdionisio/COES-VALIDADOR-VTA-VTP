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
    public class PmoConfIndispEquipoRepository : RepositoryBase, IPmoConfIndispEquipoRepository
    {
        public PmoConfIndispEquipoRepository(string strConn)
            : base(strConn)
        {
        }

        PmoConfIndispEquipoHelper helper = new PmoConfIndispEquipoHelper();

        public int Save(PmoConfIndispEquipoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.PmCindCodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.EquiCodi, DbType.Int32, entity.EquiCodi);
            dbProvider.AddInParameter(command, helper.PmCindPorcentaje, DbType.Decimal, entity.PmCindPorcentaje);
            dbProvider.AddInParameter(command, helper.PmCindConJuntoEqp, DbType.String, entity.PmCindConJuntoEqp.ToUpper());//NET 20190228
            dbProvider.AddInParameter(command, helper.PmCindRelInversa, DbType.Int32, entity.PmCindRelInversa);//NET 20190402
            dbProvider.AddInParameter(command, helper.PmCindEstRegistro, DbType.String, entity.PmCindEstRegistro);
            dbProvider.AddInParameter(command, helper.PmCindUsuCreacion, DbType.String, entity.PmCindUsuCreacion);
            dbProvider.AddInParameter(command, helper.PmCindFecCreacion, DbType.DateTime, entity.PmCindFecCreacion);
            dbProvider.AddInParameter(command, helper.PmCindUsuModificacion, DbType.String, entity.PmCindUsuModificacion);
            dbProvider.AddInParameter(command, helper.PmCindFecModificacion, DbType.DateTime, entity.PmCindFecModificacion);
            dbProvider.AddInParameter(command, helper.Grupocodimodo, DbType.Int32, entity.Grupocodimodo);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int Update(PmoConfIndispEquipoDTO entity)
        {
            string queryString = string.Format(helper.SqlUpdate);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Sddpcodi, DbType.Int32, entity.Sddpcodi);
            dbProvider.AddInParameter(command, helper.EquiCodi, DbType.Int32, entity.EquiCodi);
            dbProvider.AddInParameter(command, helper.PmCindPorcentaje, DbType.Decimal, entity.PmCindPorcentaje);
            dbProvider.AddInParameter(command, helper.PmCindConJuntoEqp, DbType.String, entity.PmCindConJuntoEqp.ToUpper());//NET 20190228
            dbProvider.AddInParameter(command, helper.PmCindUsuModificacion, DbType.String, entity.PmCindUsuModificacion);
            dbProvider.AddInParameter(command, helper.PmCindFecModificacion, DbType.DateTime, entity.PmCindFecModificacion);
            dbProvider.AddInParameter(command, helper.PmCindRelInversa, DbType.Int32, entity.PmCindRelInversa);//NET 20190402
            dbProvider.AddInParameter(command, helper.PmCindCodi, DbType.Int32, entity.PmCindCodi);
            dbProvider.AddInParameter(command, helper.Grupocodimodo, DbType.Int32, entity.Grupocodimodo);

            var id = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public PmoConfIndispEquipoDTO GetById(int pmcindcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.PmCindCodi, DbType.Int32, pmcindcodi);
            PmoConfIndispEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iFamCodi = dr.GetOrdinal(this.helper.FamCodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.FamCodi = dr.GetInt32(iFamCodi);

                    int iTsddpcodi = dr.GetOrdinal(helper.Tsddpcodi);
                    if (!dr.IsDBNull(iTsddpcodi)) entity.Tsddpcodi = Convert.ToInt32(dr.GetValue(iTsddpcodi));
                }
            }

            return entity;
        }

        public void EliminarCorrelacion(PmoConfIndispEquipoDTO entity)
        {            
            string queryString = string.Format(helper.SqlEliminarCorrelacion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.PmCindUsuModificacion, DbType.String, entity.PmCindUsuModificacion);
            dbProvider.AddInParameter(command, helper.PmCindFecModificacion, DbType.DateTime, entity.PmCindFecModificacion);
            dbProvider.AddInParameter(command, helper.PmCindCodi, DbType.Int32, entity.PmCindCodi);
            var id = dbProvider.ExecuteNonQuery(command);
        }

        public List<PmoConfIndispEquipoDTO> List(int emprcodi, int tsddpcodi, int famcodi)
        {
            List<PmoConfIndispEquipoDTO> entitys = new List<PmoConfIndispEquipoDTO>();
            string queryString = string.Format(helper.SqlList, emprcodi, tsddpcodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            PmoConfIndispEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprCodi = dr.GetOrdinal(this.helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iEquiAbrev = dr.GetOrdinal(this.helper.EquiAbrev);
                    if (!dr.IsDBNull(iEquiAbrev)) entity.EquiAbrev = dr.GetString(iEquiAbrev);
                    int iGrupoNomb = dr.GetOrdinal(this.helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.GrupoNomb = dr.GetString(iGrupoNomb);

                    int iAreaNomb = dr.GetOrdinal(this.helper.AreaNomb);
                    if (!dr.IsDBNull(iAreaNomb)) entity.AreaNomb = dr.GetString(iAreaNomb);

                    int iFamCodi = dr.GetOrdinal(this.helper.FamCodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.FamCodi = dr.GetInt32(iFamCodi);
                    int iFamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iUsuarioMod = dr.GetOrdinal(this.helper.UsuarioMod);
                    if (!dr.IsDBNull(iUsuarioMod)) entity.UsuarioMod = dr.GetString(iUsuarioMod);

                    int iFechaMod = dr.GetOrdinal(this.helper.FechaMod);
                    if (!dr.IsDBNull(iFechaMod)) entity.FechaMod = dr.GetDateTime(iFechaMod);

                    int iTsddpcodi = dr.GetOrdinal(helper.Tsddpcodi);
                    if (!dr.IsDBNull(iTsddpcodi)) entity.Tsddpcodi = Convert.ToInt32(dr.GetValue(iTsddpcodi));

                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

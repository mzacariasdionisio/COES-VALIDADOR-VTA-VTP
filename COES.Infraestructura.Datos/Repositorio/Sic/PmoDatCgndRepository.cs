using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    public class PmoDatCgndRepository : RepositoryBase, IPmoDatCgndRepository
    {
        public PmoDatCgndRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatCgndHelper helper = new PmoDatCgndHelper();

        public List<PmoDatCgndDTO> List()
        {
            List<PmoDatCgndDTO> entitys = new List<PmoDatCgndDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            PmoDatCgndDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatCgndDTO();

                    int iPmCgndCodi = dr.GetOrdinal(this.helper.PmCgndCodi);
                    if (!dr.IsDBNull(iPmCgndCodi)) entity.PmCgndCodi = dr.GetInt32(iPmCgndCodi);

                    int iCodCentral = dr.GetOrdinal(this.helper.CodCentral);
                    if (!dr.IsDBNull(iCodCentral)) entity.CodCentral = dr.GetInt32(iCodCentral);

                    int iNombCentral = dr.GetOrdinal(this.helper.NombCentral);
                    if (!dr.IsDBNull(iNombCentral)) entity.NombCentral = dr.GetString(iNombCentral);

                    int iCodBarra = dr.GetOrdinal(this.helper.CodBarra);
                    if (!dr.IsDBNull(iCodBarra)) entity.CodBarra = dr.GetInt32(iCodBarra);

                    int iNombBarra = dr.GetOrdinal(this.helper.NombBarra);
                    if (!dr.IsDBNull(iNombBarra)) entity.NombBarra = dr.GetString(iNombBarra);

                    int iPmCgndTipoPlanta = dr.GetOrdinal(this.helper.PmCgndTipoPlanta);
                    if (!dr.IsDBNull(iPmCgndTipoPlanta)) entity.PmCgndTipoPlanta = dr.GetString(iPmCgndTipoPlanta);

                    int iPmCgndNroUnidades = dr.GetOrdinal(this.helper.PmCgndNroUnidades);
                    if (!dr.IsDBNull(iPmCgndNroUnidades)) entity.PmCgndNroUnidades = dr.GetInt32(iPmCgndNroUnidades);

                    int iPmCgndPotInstalada = dr.GetOrdinal(this.helper.PmCgndPotInstalada);
                    if (!dr.IsDBNull(iPmCgndPotInstalada)) entity.PmCgndPotInstalada = dr.GetDecimal(iPmCgndPotInstalada);

                    int iPmCgndFactorOpe = dr.GetOrdinal(this.helper.PmCgndFactorOpe);
                    if (!dr.IsDBNull(iPmCgndFactorOpe)) entity.PmCgndFactorOpe = dr.GetDecimal(iPmCgndFactorOpe);

                    int iPmCgndProbFalla = dr.GetOrdinal(this.helper.PmCgndProbFalla);
                    if (!dr.IsDBNull(iPmCgndProbFalla)) entity.PmCgndProbFalla = dr.GetDecimal(iPmCgndProbFalla);

                    int iPmCgndCorteOFalla = dr.GetOrdinal(this.helper.PmCgndCorteOFalla);
                    if (!dr.IsDBNull(iPmCgndCorteOFalla)) entity.PmCgndCorteOFalla = dr.GetDecimal(iPmCgndCorteOFalla);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int Update(PmoDatCgndDTO entity)
        {
            string queryString = string.Format(helper.SqlUpdate);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmCgndTipoPlanta, DbType.String, entity.PmCgndTipoPlanta);
            dbProvider.AddInParameter(command, helper.PmCgndNroUnidades, DbType.Int32, entity.PmCgndNroUnidades);
            dbProvider.AddInParameter(command, helper.PmCgndPotInstalada, DbType.Decimal, entity.PmCgndPotInstalada);
            dbProvider.AddInParameter(command, helper.PmCgndFactorOpe, DbType.Decimal, entity.PmCgndFactorOpe);
            dbProvider.AddInParameter(command, helper.PmCgndProbFalla, DbType.Decimal, entity.PmCgndProbFalla);
            dbProvider.AddInParameter(command, helper.PmCgndCorteOFalla, DbType.Decimal, entity.PmCgndCorteOFalla);
            dbProvider.AddInParameter(command, helper.PmCgndCodi, DbType.Int32, entity.PmCgndCodi);
            var id = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public List<PrGrupoDTO> ListBarra()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetBarra);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PrGrupoDTO entity = null;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PrGrupoDTO();

                    int iGrupoCodi = dr.GetOrdinal(this.helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.Grupocodi = dr.GetInt32(iGrupoCodi);

                    int iGrupoNomb = dr.GetOrdinal(this.helper.GrupoNomb);
                    if (!dr.IsDBNull(iGrupoNomb)) entity.Gruponomb = dr.GetString(iGrupoNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public PmoDatCgndDTO GetById(int id)
        {
            PmoDatCgndDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.PmCgndCodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<PmoDatCgndDTO> ListDatCgnd()
        {
            List<PmoDatCgndDTO> entitys = new List<PmoDatCgndDTO>();
            string queryString = string.Format(helper.SqlGetDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            PmoDatCgndDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatCgndDTO();

                    int iNum = dr.GetOrdinal(helper.Num);
                    if (!dr.IsDBNull(iNum)) entity.Num = dr.GetString(iNum);

                    int iName = dr.GetOrdinal(helper.Name);
                    if (!dr.IsDBNull(iName)) entity.Name = dr.GetString(iName);

                    int iBus = dr.GetOrdinal(helper.Bus);
                    if (!dr.IsDBNull(iBus)) entity.Bus = dr.GetString(iBus);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);

                    int iUni = dr.GetOrdinal(helper.Uni);
                    if (!dr.IsDBNull(iUni)) entity.Uni = dr.GetString(iUni);

                    int iPotIns = dr.GetOrdinal(helper.PotIns);
                    if (!dr.IsDBNull(iPotIns)) entity.PotIns = dr.GetString(iPotIns);

                    int iFatOpe = dr.GetOrdinal(helper.FatOpe);
                    if (!dr.IsDBNull(iFatOpe)) entity.FatOpe = dr.GetString(iFatOpe);

                    int iProbFal = dr.GetOrdinal(helper.ProbFal);
                    if (!dr.IsDBNull(iProbFal)) entity.ProbFal = dr.GetString(iProbFal);

                    int iSFal = dr.GetOrdinal(helper.SFal);
                    if (!dr.IsDBNull(iSFal)) entity.SFal = dr.GetString(iSFal);

                    #region 20190308 - NET: Adecuaciones a los archivos .DAT

                    int iPmCgndProbFalla = dr.GetOrdinal(this.helper.PmCgndProbFalla);
                    if (!dr.IsDBNull(iPmCgndProbFalla)) entity.PmCgndProbFalla = dr.GetDecimal(iPmCgndProbFalla);

                    int iPmCgndFactorOpe = dr.GetOrdinal(this.helper.PmCgndFactorOpe);
                    if (!dr.IsDBNull(iPmCgndFactorOpe)) entity.PmCgndFactorOpe = dr.GetDecimal(iPmCgndFactorOpe);

                    int iPmCgndPotInstalada = dr.GetOrdinal(this.helper.PmCgndPotInstalada);
                    if (!dr.IsDBNull(iPmCgndPotInstalada)) entity.PmCgndPotInstalada = dr.GetDecimal(iPmCgndPotInstalada);

                    #endregion

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int CountDatCgnd(int periCodi)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();
            string queryString = string.Format(helper.SqlGetCount);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            int Cantidad = 0;
            //dbProvider.AddInParameter(command, helper.PmPeriCodi, DbType.Int32, periCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    int iCant = dr.GetOrdinal(helper.Cant);
                    if (!dr.IsDBNull(iCant)) Cantidad = dr.GetInt32(iCant);
                }
            }

            return Cantidad;
        }

        public List<PmoDatCgndDTO> ListGrupoCodig()
        {
            List<PmoDatCgndDTO> entitys = new List<PmoDatCgndDTO>();
            string queryString = string.Format(helper.SqlGetGrupoCodi);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            PmoDatCgndDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new PmoDatCgndDTO();
                    int iGrupoCodi = dr.GetOrdinal(helper.GrupoCodi);
                    if (!dr.IsDBNull(iGrupoCodi)) entity.GrupoCodi = dr.GetInt32(iGrupoCodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

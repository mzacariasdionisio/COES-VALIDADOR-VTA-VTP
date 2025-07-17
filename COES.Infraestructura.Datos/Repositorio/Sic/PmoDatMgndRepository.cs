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
    public class PmoDatMgndRepository : RepositoryBase, IPmoDatMgndRepository
    {
        public PmoDatMgndRepository(string strConn)
            : base(strConn)
        {
        }

        PmoDatMgndHelper helper = new PmoDatMgndHelper();
        public List<PmoDatMgndDTO> List()
        {
            List<PmoDatMgndDTO> entitys = new List<PmoDatMgndDTO>();
            string queryString = string.Format(helper.SqlList);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatMgndDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PmoDatMgndDTO> ListDatMgnd()
        {
            List<PmoDatMgndDTO> entitys = new List<PmoDatMgndDTO>();
            string queryString = string.Format(helper.SqlGetDat);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PmoDatMgndDTO entity = new PmoDatMgndDTO();

                    int iFecha = dr.GetOrdinal(helper.Fecha);
                    if (!dr.IsDBNull(iFecha)) entity.Fecha = dr.GetString(iFecha);

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

                    #region 20190308 - NET: Adecuaciones a los archivos .DAT
                    int iPmMgndPotInstalada = dr.GetOrdinal(helper.PmMgndPotInstalada);
                    if (!dr.IsDBNull(iPmMgndPotInstalada)) entity.PmMgndPotInstalada = dr.GetDecimal(iPmMgndPotInstalada);

                    int iPmMgndFactorOpe = dr.GetOrdinal(helper.PmMgndFactorOpe);
                    if (!dr.IsDBNull(iPmMgndFactorOpe)) entity.PmMgndFactorOpe = dr.GetDecimal(iPmMgndFactorOpe);

                    int iPmMgndProbFalla = dr.GetOrdinal(helper.PmMgndProbFalla);
                    if (!dr.IsDBNull(iPmMgndProbFalla)) entity.PmMgndProbFalla = dr.GetDecimal(iPmMgndProbFalla);
                    #endregion

                    int iSFal = dr.GetOrdinal(helper.SFal);
                    if (!dr.IsDBNull(iSFal)) entity.SFal = dr.GetString(iSFal);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListBarraMgnd()
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

        public PmoDatMgndDTO GetById(int id)
        {
            PmoDatMgndDTO entity = null;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.PmMgndCodi, DbType.Int32, id);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public int Update(PmoDatMgndDTO entity)
        {
            string queryString = string.Format(helper.SqlUpdate);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            //dbProvider.AddInParameter(command, helper.GrupoCodi, DbType.Int32, entity.GrupoCodi);
            dbProvider.AddInParameter(command, helper.PmMgndFecha, DbType.DateTime, entity.PmMgndFecha);
            dbProvider.AddInParameter(command, helper.PmMgndTipoPlanta, DbType.Int32, entity.PmMgndTipoPlanta);
            dbProvider.AddInParameter(command, helper.PmMgndNroUnidades, DbType.Decimal, entity.PmMgndNroUnidades);
            dbProvider.AddInParameter(command, helper.PmMgndFactorOpe, DbType.Decimal, entity.PmMgndFactorOpe);
            dbProvider.AddInParameter(command, helper.PmMgndProbFalla, DbType.Decimal, entity.PmMgndProbFalla);
            dbProvider.AddInParameter(command, helper.PmMgndPotInstalada, DbType.Decimal, entity.PmMgndPotInstalada);//20190317 - NET: Corrección
            dbProvider.AddInParameter(command, helper.CodBarra, DbType.Decimal, entity.CodBarra);//20190317 - NET: Corrección
            dbProvider.AddInParameter(command, helper.PmMgndCorteOFalla, DbType.Decimal, entity.PmMgndCorteOFalla);//20190713 - NET: Corrección

            dbProvider.AddInParameter(command, helper.PmMgndCodi, DbType.Int32, entity.PmMgndCodi);
            var id = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public int CountDatMgnd(int periCodi)
        {
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
    }
}

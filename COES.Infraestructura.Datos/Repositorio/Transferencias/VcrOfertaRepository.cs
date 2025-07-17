using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_OFERTA
    /// </summary>
    public class VcrOfertaRepository: RepositoryBase, IVcrOfertaRepository
    {
        public VcrOfertaRepository(string strConn): base(strConn)
        {
        }

        VcrOfertaHelper helper = new VcrOfertaHelper();

        public int Save(VcrOfertaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrofecodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Vcrofecodigoenv, DbType.String, entity.Vcrofecodigoenv);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, entity.Vcrofefecha);
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, entity.Vcrofehorinicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, entity.Vcrofehorfinal);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrofemodoperacion, DbType.String, entity.Vcrofemodoperacion);
            dbProvider.AddInParameter(command, helper.Vcrofepotofertada, DbType.Decimal, entity.Vcrofepotofertada);
            dbProvider.AddInParameter(command, helper.Vcrofeprecio, DbType.Decimal, entity.Vcrofeprecio);
            dbProvider.AddInParameter(command, helper.Vcrofeusucreacion, DbType.String, entity.Vcrofeusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrofefeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrofetipocarga, DbType.Int32, entity.Vcrofetipocarga); 
            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrOfertaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Usercode, DbType.Int32, entity.Usercode);
            dbProvider.AddInParameter(command, helper.Vcrofecodigoenv, DbType.String, entity.Vcrofecodigoenv);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, entity.Vcrofefecha);
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, entity.Vcrofehorinicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, entity.Vcrofehorfinal);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrofemodoperacion, DbType.String, entity.Vcrofemodoperacion);
            dbProvider.AddInParameter(command, helper.Vcrofepotofertada, DbType.Decimal, entity.Vcrofepotofertada);
            dbProvider.AddInParameter(command, helper.Vcrofeprecio, DbType.Decimal, entity.Vcrofeprecio);
            dbProvider.AddInParameter(command, helper.Vcrofeusucreacion, DbType.String, entity.Vcrofeusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrofefeccreacion, DbType.DateTime, entity.Vcrofefeccreacion);
            dbProvider.AddInParameter(command, helper.Vcrofetipocarga, DbType.Int32, entity.Vcrofetipocarga);
            dbProvider.AddInParameter(command, helper.Vcrofecodi, DbType.Int32, entity.Vcrofecodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrOfertaDTO GetById(int vcrofecodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrofecodi, DbType.Int32, vcrofecodi);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrOfertaDTO GetByIdMaxDia(int vcrecacodi, DateTime dFecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMaxDia);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrOfertaDTO GetByIdMaxDiaUrs(int vcrecacodi, DateTime dFecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMaxDiaUrs);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrOfertaDTO GetByIdMaxDiaGrupoCodi(int vcrecacodi, int grupocodi, DateTime dFecha)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMaxDiaGrupoCodi);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrOfertaDTO> List()
        {
            List<VcrOfertaDTO> entitys = new List<VcrOfertaDTO>();
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
        
        //ASSETEC 20190115
        public List<VcrOfertaDTO> ListSinDuplicados(int vcrecacodi)
        {
            List<VcrOfertaDTO> entitys = new List<VcrOfertaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListSinDuplicados);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrOfertaDTO entity = new VcrOfertaDTO();

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iUsercode = dr.GetOrdinal(this.helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iVcrofecodigoenv = dr.GetOrdinal(this.helper.Vcrofecodigoenv);
                    if (!dr.IsDBNull(iVcrofecodigoenv)) entity.Vcrofecodigoenv = dr.GetString(iVcrofecodigoenv);

                    int iVcrofefecha = dr.GetOrdinal(this.helper.Vcrofefecha);
                    if (!dr.IsDBNull(iVcrofefecha)) entity.Vcrofefecha = dr.GetDateTime(iVcrofefecha);

                    int iVcrofehorinicio = dr.GetOrdinal(this.helper.Vcrofehorinicio);
                    if (!dr.IsDBNull(iVcrofehorinicio)) entity.Vcrofehorinicio = dr.GetDateTime(iVcrofehorinicio);

                    int iVcrofehorfinal = dr.GetOrdinal(this.helper.Vcrofehorfinal);
                    if (!dr.IsDBNull(iVcrofehorfinal)) entity.Vcrofehorfinal = dr.GetDateTime(iVcrofehorfinal);

                    int iGrupocodi = dr.GetOrdinal(this.helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iVcrofetipocarga = dr.GetOrdinal(this.helper.Vcrofetipocarga);
                    if (!dr.IsDBNull(iVcrofetipocarga)) entity.Vcrofetipocarga = Convert.ToInt32(dr.GetValue(iVcrofetipocarga));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public VcrOfertaDTO GetByCriteriaOferta(int vcrecacodi, int grupocodi, DateTime vcrofefecha, string vcrofecodigoenv, DateTime vcrofehorinicio, int vcrofetipocarga)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaVcrOferta);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, vcrofefecha);
            dbProvider.AddInParameter(command, helper.Vcrofecodigoenv, DbType.String, vcrofecodigoenv);
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, vcrofehorinicio);
            dbProvider.AddInParameter(command, helper.Vcrofetipocarga, DbType.Int32, vcrofetipocarga);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public decimal GetOfertaMaxDiaGrupoCodiHiHf(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetOfertaMaxDiaGrupoCodiHiHf);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            dbProvider.AddInParameter(command, helper.Vcrofetipocarga, DbType.Int32, vcrofetipocarga);
            
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            //dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal.AddMinutes(-1));
            //dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal);
            //dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            //dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal);
            decimal dVcrofeprecio = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iVcrofeprecio = dr.GetOrdinal(this.helper.Vcrofeprecio);
                    if (!dr.IsDBNull(iVcrofeprecio)) dVcrofeprecio = dr.GetDecimal(iVcrofeprecio);
                }
            }

            return dVcrofeprecio;
        }

        public decimal GetOfertaMaxDiaGrupoCodiHiHf2020(int vcrecacodi, int grupocodi, DateTime dFecha, DateTime dHoraInicio, DateTime dHoraFinal, int vcrofetipocarga)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetOfertaMaxDiaGrupoCodiHiHf2020);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            dbProvider.AddInParameter(command, helper.Vcrofetipocarga, DbType.Int32, vcrofetipocarga);

            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal);
            dbProvider.AddInParameter(command, helper.Vcrofehorinicio, DbType.DateTime, dHoraInicio);
            dbProvider.AddInParameter(command, helper.Vcrofehorfinal, DbType.DateTime, dHoraFinal);
            decimal dVcrofeprecio = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iVcrofeprecio = dr.GetOrdinal(this.helper.Vcrofeprecio);
                    if (!dr.IsDBNull(iVcrofeprecio)) dVcrofeprecio = dr.GetDecimal(iVcrofeprecio);
                }
            }

            return dVcrofeprecio;
        }
        //-------------------------------------------------------------------------------------------

        public List<VcrOfertaDTO> GetByCriteria()
        {
            List<VcrOfertaDTO> entitys = new List<VcrOfertaDTO>();
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

        public VcrOfertaDTO GetByIdMaxMes(int vcrecacodi, DateTime dFecha)//, int grupocodi
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdMaxMes);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            //dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Vcrofefecha, DbType.DateTime, dFecha);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        //Funciones para la tabla FW_USER
        public VcrOfertaDTO GetByFwUserByNombre(string Username)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByFwUserByNombre);

            dbProvider.AddInParameter(command, helper.Username, DbType.String, Username);
            VcrOfertaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrOfertaDTO();

                    int iUsercode = dr.GetOrdinal(this.helper.Usercode);
                    if (!dr.IsDBNull(iUsercode)) entity.Usercode = Convert.ToInt32(dr.GetValue(iUsercode));

                    int iUsername = dr.GetOrdinal(this.helper.Username);
                    if (!dr.IsDBNull(iUsername)) entity.Username = Convert.ToString(dr.GetValue(iUsername));
                }
            }

            return entity;
        }
    }
}

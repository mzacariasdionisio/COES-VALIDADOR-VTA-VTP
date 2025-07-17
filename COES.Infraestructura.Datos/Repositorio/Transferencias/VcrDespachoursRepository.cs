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
    /// Clase de acceso a datos de la tabla VCR_DESPACHOURS
    /// </summary>
    public class VcrDespachoursRepository: RepositoryBase, IVcrDespachoursRepository
    {
        public VcrDespachoursRepository(string strConn): base(strConn)
        {
        }

        VcrDespachoursHelper helper = new VcrDespachoursHelper();

        public int Save(VcrDespachoursDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcdurscodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcdurstipo, DbType.String, entity.Vcdurstipo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Vcdursfecha, DbType.DateTime, entity.Vcdursfecha);
            dbProvider.AddInParameter(command, helper.Vcdursdespacho, DbType.Decimal, entity.Vcdursdespacho);
            dbProvider.AddInParameter(command, helper.Vcdursusucreacion, DbType.String, entity.Vcdursusucreacion);
            dbProvider.AddInParameter(command, helper.Vcdursfeccreacion, DbType.DateTime, DateTime.Now);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrDespachoursDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcdurstipo, DbType.String, entity.Vcdurstipo);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Vcdursfecha, DbType.DateTime, entity.Vcdursfecha);
            dbProvider.AddInParameter(command, helper.Vcdursdespacho, DbType.Decimal, entity.Vcdursdespacho);
            dbProvider.AddInParameter(command, helper.Vcdursusucreacion, DbType.String, entity.Vcdursusucreacion);
            dbProvider.AddInParameter(command, helper.Vcdursfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcdurscodi, DbType.Int32, entity.Vcdurscodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrDespachoursDTO GetById(int vcdurscodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcdurscodi, DbType.Int32, vcdurscodi);
            VcrDespachoursDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<VcrDespachoursDTO> List(int vcrecacodi)
        {
            List<VcrDespachoursDTO> entitys = new List<VcrDespachoursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrDespachoursDTO> ListUnidadByUrsTipo(int vcrecacodi, int GrupoCodi, string Vcdurstipo)
        {
            List<VcrDespachoursDTO> entitys = new List<VcrDespachoursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListUnidadByUrsTipo);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, GrupoCodi);
            dbProvider.AddInParameter(command, helper.Vcdurstipo, DbType.String, Vcdurstipo);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrDespachoursDTO dtoDespachours = new VcrDespachoursDTO();

                    int iEquicodi = dr.GetOrdinal(this.helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) dtoDespachours.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(this.helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) dtoDespachours.Equinomb = dr.GetString(iEquinomb);


                    entitys.Add(dtoDespachours);
                }
            }

            return entitys;
        }

        public List<VcrDespachoursDTO> ListByUrsUnidadTipoDia(int vcrecacodi, int grupocodi, int equicodi, string vcdurstipo, DateTime vcdursfecha)
        {
            List<VcrDespachoursDTO> entitys = new List<VcrDespachoursDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByUrsUnidadTipoDia);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.Vcdurstipo, DbType.String, vcdurstipo);
            dbProvider.AddInParameter(command, helper.Vcdursfecha, DbType.DateTime, vcdursfecha);
            
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<VcrDespachoursDTO> GetByCriteria()
        {
            List<VcrDespachoursDTO> entitys = new List<VcrDespachoursDTO>();
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

        public List<VcrDespachoursDTO> ListByRangeDatetime(DateTime fechaInicio, DateTime fechaFin)
        {
            List<VcrDespachoursDTO> entitys = new List<VcrDespachoursDTO>();
            var query = string.Format(helper.SqlGetByRangeDatetime, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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

        #region Costos de Oportunidad

        public int GetMaxId()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }

        public void GrabarBulk(List<VcrDespachoursDTO> ListaDespacho)
        {  
            dbProvider.AddColumnMapping(helper.Vcdurscodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Vcrecacodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Vcdurstipo, DbType.String);
            dbProvider.AddColumnMapping(helper.Grupocodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Gruponomb, DbType.String);
            dbProvider.AddColumnMapping(helper.Equicodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Emprcodi, DbType.Int32);
            dbProvider.AddColumnMapping(helper.Vcdursfecha, DbType.DateTime);
            dbProvider.AddColumnMapping(helper.Vcdursdespacho, DbType.Decimal);
            dbProvider.AddColumnMapping(helper.Vcdursusucreacion, DbType.String);
            dbProvider.AddColumnMapping(helper.Vcdursfeccreacion, DbType.DateTime);

            dbProvider.BulkInsert<VcrDespachoursDTO>(ListaDespacho, "VCR_DESPACHOURS");
        }

        #endregion
    }
}

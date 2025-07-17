using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;
using COES.Dominio.DTO.Sic;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VCR_ASIGNACIONRESERVA
    /// </summary>
    public class VcrAsignacionreservaRepository: RepositoryBase, IVcrAsignacionreservaRepository
    {
        public VcrAsignacionreservaRepository(string strConn): base(strConn)
        {
        }

        VcrAsignacionreservaHelper helper = new VcrAsignacionreservaHelper();

        public int Save(VcrAsignacionreservaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null)id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Vcrarcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrarfecha, DbType.DateTime, entity.Vcrarfecha);
            dbProvider.AddInParameter(command, helper.Vcrarrapbf, DbType.Decimal, entity.Vcrarrapbf);
            dbProvider.AddInParameter(command, helper.Vcrarprbf, DbType.Decimal, entity.Vcrarprbf);
            dbProvider.AddInParameter(command, helper.Vcrarrama, DbType.Decimal, entity.Vcrarrama);
            dbProvider.AddInParameter(command, helper.Vcrarmpa, DbType.Decimal, entity.Vcrarmpa);
            dbProvider.AddInParameter(command, helper.Vcrarramaursra, DbType.Decimal, entity.Vcrarramaursra);
            dbProvider.AddInParameter(command, helper.Vcrarasignreserva, DbType.Decimal, entity.Vcrarasignreserva);
            dbProvider.AddInParameter(command, helper.Vcrarusucreacion, DbType.String, entity.Vcrarusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrarfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrarrapbfbajar, DbType.Decimal, entity.Vcrarrapbfbajar);
            dbProvider.AddInParameter(command, helper.Vcrarprbfbajar, DbType.Decimal, entity.Vcrarprbfbajar);
            dbProvider.AddInParameter(command, helper.Vcrarramabajar, DbType.Decimal, entity.Vcrarramabajar);
            dbProvider.AddInParameter(command, helper.Vcrarmpabajar, DbType.Decimal, entity.Vcrarmpabajar);
            dbProvider.AddInParameter(command, helper.Vcrarramaursrabajar, DbType.Decimal, entity.Vcrarramaursrabajar);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(VcrAsignacionreservaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, entity.Vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Gruponomb, DbType.String, entity.Gruponomb);
            dbProvider.AddInParameter(command, helper.Vcrarfecha, DbType.DateTime, entity.Vcrarfecha);
            dbProvider.AddInParameter(command, helper.Vcrarrapbf, DbType.Decimal, entity.Vcrarrapbf);
            dbProvider.AddInParameter(command, helper.Vcrarprbf, DbType.Decimal, entity.Vcrarprbf);
            dbProvider.AddInParameter(command, helper.Vcrarrama, DbType.Decimal, entity.Vcrarrama);
            dbProvider.AddInParameter(command, helper.Vcrarmpa, DbType.Decimal, entity.Vcrarmpa);
            dbProvider.AddInParameter(command, helper.Vcrarramaursra, DbType.Decimal, entity.Vcrarramaursra);
            dbProvider.AddInParameter(command, helper.Vcrarasignreserva, DbType.Decimal, entity.Vcrarasignreserva);
            dbProvider.AddInParameter(command, helper.Vcrarusucreacion, DbType.String, entity.Vcrarusucreacion);
            dbProvider.AddInParameter(command, helper.Vcrarfeccreacion, DbType.DateTime, DateTime.Now);
            dbProvider.AddInParameter(command, helper.Vcrarrapbfbajar, DbType.Decimal, entity.Vcrarrapbfbajar);
            dbProvider.AddInParameter(command, helper.Vcrarprbfbajar, DbType.Decimal, entity.Vcrarprbfbajar);
            dbProvider.AddInParameter(command, helper.Vcrarramabajar, DbType.Decimal, entity.Vcrarramabajar);
            dbProvider.AddInParameter(command, helper.Vcrarmpabajar, DbType.Decimal, entity.Vcrarmpabajar);
            dbProvider.AddInParameter(command, helper.Vcrarramaursrabajar, DbType.Decimal, entity.Vcrarramaursrabajar);
            dbProvider.AddInParameter(command, helper.Vcrarcodi, DbType.Int32, entity.Vcrarcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public VcrAsignacionreservaDTO GetById(int vcrarcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Vcrarcodi, DbType.Int32, vcrarcodi);
            VcrAsignacionreservaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public VcrAsignacionreservaDTO GetByIdEmpresa(int vcrecacodi, int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEmpresa);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);
            VcrAsignacionreservaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrAsignacionreservaDTO();

                    int iVcrarasignreserva = dr.GetOrdinal(this.helper.Vcrarasignreserva);
                    if (!dr.IsDBNull(iVcrarasignreserva)) entity.Vcrarasignreserva = dr.GetDecimal(iVcrarasignreserva);
                }
            }

            return entity;
        }

        public List<VcrAsignacionreservaDTO> List()
        {
            List<VcrAsignacionreservaDTO> entitys = new List<VcrAsignacionreservaDTO>();
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

        public List<VcrAsignacionreservaDTO> GetByCriteria()
        {
            List<VcrAsignacionreservaDTO> entitys = new List<VcrAsignacionreservaDTO>();
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

        public List<VcrAsignacionreservaDTO> ListPorMesURS(int vcrecacodi, int grupocodi)
        {
            List<VcrAsignacionreservaDTO> entitys = new List<VcrAsignacionreservaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListPorMesURS);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, grupocodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EveRsfhoraDTO> ExportarReservaAsignadaSEV2020(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveRsfhoraDTO> entitys = new List<EveRsfhoraDTO>();
            string sql = string.Format(helper.SqlExportarReservaAsignadaSEV2020, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveRsfhoraDTO entity = new EveRsfhoraDTO();
                    string Rsfhorfecha = "RSFHORFECHA";
                    string Rsfhorinicio = "RSFHORINICIO";
                    string Rsfhorfin = "RSFHORFIN";
                    string Ursnomb = "GRUPONOMB";
                    string ValorAutomatico = "RSFDETVALAUT";

                    int iRsfhorfecha = dr.GetOrdinal(Rsfhorfecha);
                    if (!dr.IsDBNull(iRsfhorfecha)) entity.Rsfhorfecha = dr.GetDateTime(iRsfhorfecha);

                    int iRsfhorinicio = dr.GetOrdinal(Rsfhorinicio);
                    if (!dr.IsDBNull(iRsfhorinicio)) entity.Rsfhorinicio = dr.GetDateTime(iRsfhorinicio);

                    int iRsfhorfin = dr.GetOrdinal(Rsfhorfin);
                    if (!dr.IsDBNull(iRsfhorfin)) entity.Rsfhorfin = dr.GetDateTime(iRsfhorfin);

                    int iUrsnomb = dr.GetOrdinal(Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iValorautomatico = dr.GetOrdinal(ValorAutomatico);
                    if (!dr.IsDBNull(iValorautomatico)) entity.Valorautomatico = dr.GetDecimal(iValorautomatico);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VcrEveRsfhoraDTO> ExportarReservaAsignadaSEV(DateTime fechaInicio, DateTime fechaFin)
        {
            List<VcrEveRsfhoraDTO> entitys = new List<VcrEveRsfhoraDTO>();
            string sql = string.Format(helper.SqlExportarReservaAsignadaSEV, fechaInicio.ToString(ConstantesBase.FormatoFecha),
                fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VcrEveRsfhoraDTO entity = new VcrEveRsfhoraDTO();
                    string Rsfhorfecha = "RSFHORFECHA";
                    string Rsfhorinicio = "RSFHORINICIO";
                    string Rsfhorfin = "RSFHORFIN";
                    string Ursnomb = "GRUPONOMB";
                    //string ValorAutomatico = "RSFDETVALAUT";
                    string Rsfdetsub = "RSFDETSUB";
                    string Rsfdetbaj = "RSFDETBAJ";

                    int iRsfhorfecha = dr.GetOrdinal(Rsfhorfecha);
                    if (!dr.IsDBNull(iRsfhorfecha)) entity.Rsfhorfecha = dr.GetDateTime(iRsfhorfecha);

                    int iRsfhorinicio = dr.GetOrdinal(Rsfhorinicio);
                    if (!dr.IsDBNull(iRsfhorinicio)) entity.Rsfhorinicio = dr.GetDateTime(iRsfhorinicio);

                    int iRsfhorfin = dr.GetOrdinal(Rsfhorfin);
                    if (!dr.IsDBNull(iRsfhorfin)) entity.Rsfhorfin = dr.GetDateTime(iRsfhorfin);

                    int iUrsnomb = dr.GetOrdinal(Ursnomb);
                    if (!dr.IsDBNull(iUrsnomb)) entity.Ursnomb = dr.GetString(iUrsnomb);

                    int iRsfdetsub = dr.GetOrdinal(Rsfdetsub);
                    if (!dr.IsDBNull(iRsfdetsub)) entity.Rsfdetsub = dr.GetDecimal(iRsfdetsub);

                    int iRsfdetbaj = dr.GetOrdinal(Rsfdetbaj);
                    if (!dr.IsDBNull(iRsfdetbaj)) entity.Rsfdetbaj = dr.GetDecimal(iRsfdetbaj);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<VcrAsignacionreservaDTO> GetByCriteriaVcrAsignacionReservaOferta(int vcrecacodi, DateTime fecha)
        {
            List<VcrAsignacionreservaDTO> entitys = new List<VcrAsignacionreservaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteriaOferta);
            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrarfecha, DbType.DateTime, fecha);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        //ASSETEC 20190115 -> MODIFICADO 202010
        public decimal GetBydMPA2020(int vcrecacodi, DateTime dDia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByMPA2020);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrarfecha, DbType.DateTime, dDia);
            decimal dVcrarmpa = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iVcrarmpa = dr.GetOrdinal(this.helper.Vcrarmpa);
                    if (!dr.IsDBNull(iVcrarmpa)) dVcrarmpa = dr.GetDecimal(iVcrarmpa);
                }
            }
            return dVcrarmpa;
        }

        public VcrAsignacionreservaDTO GetBydMPA(int vcrecacodi, DateTime dDia)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByMPA);

            dbProvider.AddInParameter(command, helper.Vcrecacodi, DbType.Int32, vcrecacodi);
            dbProvider.AddInParameter(command, helper.Vcrarfecha, DbType.DateTime, dDia);
            VcrAsignacionreservaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = new VcrAsignacionreservaDTO();

                    int iVcrarmpa = dr.GetOrdinal(this.helper.Vcrarmpa);
                    if (!dr.IsDBNull(iVcrarmpa)) entity.Vcrarmpa = dr.GetDecimal(iVcrarmpa);

                    int iVcrarmpabajar = dr.GetOrdinal(this.helper.Vcrarmpabajar);
                    if (!dr.IsDBNull(iVcrarmpabajar)) entity.Vcrarmpabajar = dr.GetDecimal(iVcrarmpabajar);
                }
            }
            return entity;
        }
    }
}

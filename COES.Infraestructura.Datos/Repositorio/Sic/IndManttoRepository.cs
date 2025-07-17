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
    /// <summary>
    /// Clase de acceso a datos de la tabla IND_MANTTO
    /// </summary>
    public class IndManttoRepository : RepositoryBase, IIndManttoRepository
    {
        public IndManttoRepository(string strConn)
            : base(strConn)
        {
        }

        IndManttoHelper helper = new IndManttoHelper();

        public int Save(IndManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Indmancodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Indmanfecini, DbType.DateTime, entity.Indmanfecini);
            dbProvider.AddInParameter(command, helper.Indmanfecfin, DbType.DateTime, entity.Indmanfecfin);
            dbProvider.AddInParameter(command, helper.Indmantipoindisp, DbType.String, entity.Indmantipoindisp);
            dbProvider.AddInParameter(command, helper.Indmanpr, DbType.Decimal, entity.Indmanpr);
            dbProvider.AddInParameter(command, helper.Indmanasocproc, DbType.String, entity.Indmanasocproc);
            dbProvider.AddInParameter(command, helper.Indmanusarencalculo, DbType.String, entity.Indmanusarencalculo);
            dbProvider.AddInParameter(command, helper.Indmancomentario, DbType.String, entity.Indmancomentario);
            dbProvider.AddInParameter(command, helper.Indmanestado, DbType.String, entity.Indmanestado);
            dbProvider.AddInParameter(command, helper.Indmantipoaccion, DbType.String, entity.Indmantipoaccion);
            dbProvider.AddInParameter(command, helper.Indmanindispo, DbType.String, entity.Indmanindispo);
            dbProvider.AddInParameter(command, helper.Indmaninterrup, DbType.String, entity.Indmaninterrup);
            dbProvider.AddInParameter(command, helper.Indmandescripcion, DbType.String, entity.Indmandescripcion);
            dbProvider.AddInParameter(command, helper.Indmanusucreacion, DbType.String, entity.Indmanusucreacion);
            dbProvider.AddInParameter(command, helper.Indmanfeccreacion, DbType.DateTime, entity.Indmanfeccreacion);
            dbProvider.AddInParameter(command, helper.Indmanusumodificacion, DbType.String, entity.Indmanusumodificacion);
            dbProvider.AddInParameter(command, helper.Indmanfecmodificacion, DbType.DateTime, entity.Indmanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Indmancodiold, DbType.Int32, entity.Indmancodiold);
            dbProvider.AddInParameter(command, helper.Indmanomitir7d, DbType.String, entity.Indmanomitir7d);
            dbProvider.AddInParameter(command, helper.Indmanomitirexcesopr, DbType.String, entity.Indmanomitirexcesopr);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(IndManttoDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Manttocodi, DbType.Int32, entity.Manttocodi);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Evenclasecodi, DbType.Int32, entity.Evenclasecodi);
            dbProvider.AddInParameter(command, helper.Tipoevencodi, DbType.Int32, entity.Tipoevencodi);
            dbProvider.AddInParameter(command, helper.Indmanfecini, DbType.DateTime, entity.Indmanfecini);
            dbProvider.AddInParameter(command, helper.Indmanfecfin, DbType.DateTime, entity.Indmanfecfin);
            dbProvider.AddInParameter(command, helper.Indmantipoindisp, DbType.String, entity.Indmantipoindisp);
            dbProvider.AddInParameter(command, helper.Indmanpr, DbType.Decimal, entity.Indmanpr);
            dbProvider.AddInParameter(command, helper.Indmanasocproc, DbType.String, entity.Indmanasocproc);
            dbProvider.AddInParameter(command, helper.Indmanusarencalculo, DbType.String, entity.Indmanusarencalculo);
            dbProvider.AddInParameter(command, helper.Indmancomentario, DbType.String, entity.Indmancomentario);
            dbProvider.AddInParameter(command, helper.Indmanestado, DbType.String, entity.Indmanestado);
            dbProvider.AddInParameter(command, helper.Indmantipoaccion, DbType.String, entity.Indmantipoaccion);
            dbProvider.AddInParameter(command, helper.Indmanindispo, DbType.String, entity.Indmanindispo);
            dbProvider.AddInParameter(command, helper.Indmaninterrup, DbType.String, entity.Indmaninterrup);
            dbProvider.AddInParameter(command, helper.Indmandescripcion, DbType.String, entity.Indmandescripcion);
            dbProvider.AddInParameter(command, helper.Indmanusucreacion, DbType.String, entity.Indmanusucreacion);
            dbProvider.AddInParameter(command, helper.Indmanfeccreacion, DbType.DateTime, entity.Indmanfeccreacion);
            dbProvider.AddInParameter(command, helper.Indmanusumodificacion, DbType.String, entity.Indmanusumodificacion);
            dbProvider.AddInParameter(command, helper.Indmanfecmodificacion, DbType.DateTime, entity.Indmanfecmodificacion);
            dbProvider.AddInParameter(command, helper.Indmancodiold, DbType.Int32, entity.Indmancodiold);
            dbProvider.AddInParameter(command, helper.Indmanomitir7d, DbType.String, entity.Indmanomitir7d);
            dbProvider.AddInParameter(command, helper.Indmanomitirexcesopr, DbType.String, entity.Indmanomitirexcesopr);

            dbProvider.AddInParameter(command, helper.Indmancodi, DbType.Int32, entity.Indmancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int indmancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Indmancodi, DbType.Int32, indmancodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndManttoDTO GetById(int indmancodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Indmancodi, DbType.Int32, indmancodi);
            IndManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public IndManttoDTO GetById2(int manttocodi)
        {
            String query = String.Format(helper.SqlGetById2, manttocodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            IndManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);

                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);

                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                }
            }

            return entity;
        }

        public List<IndManttoDTO> List()
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
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

        public List<IndManttoDTO> GetByCriteria()
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
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

        public List<IndManttoDTO> GetIndisponibilidadesIndmanto(DateTime fechaInicio, DateTime fechaFin, string famcodi, int famcodiCentral, int famcodiUnidad)
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            string query = string.Empty;
            if (famcodi == "-1") { query = string.Format(helper.SqlIndisponibilidadesIndmantoCuadro4, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), famcodiCentral, famcodiUnidad); }
            else { query = string.Format(helper.SqlIndisponibilidadesIndmanto, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), famcodi); }
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            IndManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndManttoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);
                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = dr.GetInt32(iEvenclasecodi);
                    int iIndmanfecini = dr.GetOrdinal(helper.Indmanfecini);
                    if (!dr.IsDBNull(iIndmanfecini)) entity.Indmanfecini = dr.GetDateTime(iIndmanfecini);
                    int iIndmanfecfin = dr.GetOrdinal(helper.Indmanfecfin);
                    if (!dr.IsDBNull(iIndmanfecfin)) entity.Indmanfecfin = dr.GetDateTime(iIndmanfecfin);
                    int iManttocodi = dr.GetOrdinal(helper.Manttocodi);
                    if (!dr.IsDBNull(iManttocodi)) entity.Manttocodi = dr.GetInt32(iManttocodi);
                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<IndManttoDTO> BuscarMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto, int nroPagina, int nroFilas)
        {
            String query = String.Format(helper.SqlGetByCriteria, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPagina,
                nroFilas, idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            IndManttoDTO entity = new IndManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);
                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);
                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroRegistros(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
           string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {

            String query = String.Format(helper.SqlTotalRecords, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                    fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                     idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);

            return 0;
        }

        public List<IndManttoDTO> ObtenerReporteMantenimientos(string idsTipoMantenimiento, DateTime fechaInicio, DateTime fechaFin, string indispo,
            string idsTipoEmpresa, string idsEmpresa, string idsTipoEquipo, string indInterrupcion, string idstipoMantto)
        {
            String query = String.Format(helper.SqlReporteIndMantto, idsTipoMantenimiento, idsEmpresa, idsTipoEquipo,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha),
                idsTipoEmpresa, indInterrupcion, idstipoMantto, indispo);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            IndManttoDTO entity = new IndManttoDTO();
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iTipoemprcodi = dr.GetOrdinal(helper.Tipoemprcodi);
                    if (!dr.IsDBNull(iTipoemprcodi)) entity.Tipoemprcodi = Convert.ToInt32(iTipoemprcodi);
                    int iTipoemprdesc = dr.GetOrdinal(helper.Tipoemprdesc);
                    if (!dr.IsDBNull(iTipoemprdesc)) entity.Tipoemprdesc = dr.GetString(iTipoemprdesc);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEvenclasedesc = dr.GetOrdinal(helper.Evenclasedesc);
                    if (!dr.IsDBNull(iEvenclasedesc)) entity.Evenclasedesc = dr.GetString(iEvenclasedesc);
                    int iEvenclaseabrev = dr.GetOrdinal(helper.Evenclaseabrev);
                    if (!dr.IsDBNull(iEvenclaseabrev)) entity.Evenclaseabrev = dr.GetString(iEvenclaseabrev);

                    int iTipoevenabrev = dr.GetOrdinal(helper.Tipoevenabrev);
                    if (!dr.IsDBNull(iTipoevenabrev)) entity.Tipoevenabrev = dr.GetString(iTipoevenabrev);
                    int iTipoevendesc = dr.GetOrdinal(helper.Tipoevendesc);
                    if (!dr.IsDBNull(iTipoevendesc)) entity.Tipoevendesc = dr.GetString(iTipoevendesc);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = dr.GetInt32(iAreacodi);
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);
                    int iAreadesc = dr.GetOrdinal(helper.Areadesc);
                    if (!dr.IsDBNull(iAreadesc)) entity.Areadesc = dr.GetString(iAreadesc);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));
                    int iFamnomb = dr.GetOrdinal(helper.Famnomb);
                    if (!dr.IsDBNull(iFamnomb)) entity.Famnomb = dr.GetString(iFamnomb);
                    int ifamabrev = dr.GetOrdinal(helper.Famabrev);
                    if (!dr.IsDBNull(ifamabrev)) entity.Famabrev = dr.GetString(ifamabrev);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquitension = dr.GetOrdinal(helper.Equitension);
                    if (!dr.IsDBNull(iEquitension)) entity.Equitension = dr.GetDecimal(iEquitension);

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);

                    int iGrupotipocogen = dr.GetOrdinal(this.helper.Grupotipocogen);
                    if (!dr.IsDBNull(iGrupotipocogen)) entity.Grupotipocogen = dr.GetString(iGrupotipocogen);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndManttoDTO> ListarIndManttoByEveMantto(string manttocodi)
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            string query = string.Format(helper.SqlListarIndManttoByEveMantto, manttocodi);

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

        public List<IndManttoDTO> ListHistoricoByIndmacodi(string indmacodi)
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            string query = string.Format(helper.SqlListHistoricoByIndmacodi, indmacodi);

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

        public List<IndManttoDTO> ListarIndManttoAppPR25(DateTime fechaInicio, DateTime fechaFin, string famcodi)
        {
            List<IndManttoDTO> entitys = new List<IndManttoDTO>();
            string query = string.Format(helper.SqlListarIndManttoAppPR25, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            IndManttoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new IndManttoDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);
                    int iCentral = dr.GetOrdinal(helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iFamcodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamcodi)) entity.Famcodi = Convert.ToInt32(dr.GetValue(iFamcodi));

                    int iEvenclasecodi = dr.GetOrdinal(helper.Evenclasecodi);
                    if (!dr.IsDBNull(iEvenclasecodi)) entity.Evenclasecodi = dr.GetInt32(iEvenclasecodi);

                    int iIndmanfecini = dr.GetOrdinal(helper.Indmanfecini);
                    if (!dr.IsDBNull(iIndmanfecini)) entity.Indmanfecini = dr.GetDateTime(iIndmanfecini);
                    int iIndmanfecfin = dr.GetOrdinal(helper.Indmanfecfin);
                    if (!dr.IsDBNull(iIndmanfecfin)) entity.Indmanfecfin = dr.GetDateTime(iIndmanfecfin);
                    int iManttocodi = dr.GetOrdinal(helper.Manttocodi);
                    if (!dr.IsDBNull(iManttocodi)) entity.Manttocodi = dr.GetInt32(iManttocodi);

                    int iIndmandescripcion = dr.GetOrdinal(helper.Indmandescripcion);
                    if (!dr.IsDBNull(iIndmandescripcion)) entity.Indmandescripcion = dr.GetString(iIndmandescripcion);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

    }
}

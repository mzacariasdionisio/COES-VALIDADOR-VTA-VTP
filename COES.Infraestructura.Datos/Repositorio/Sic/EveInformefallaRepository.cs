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
    /// Clase de acceso a datos de la tabla EVE_INFORMEFALLA
    /// </summary>
    public class EveInformefallaRepository : RepositoryBase, IEveInformefallaRepository
    {
        public EveInformefallaRepository(string strConn) : base(strConn)
        {
        }

        EveInformefallaHelper helper = new EveInformefallaHelper();

        public int Save(EveInformefallaDTO entity)
        {
            //DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            //object result = dbProvider.ExecuteScalar(command);
            //int id = 1;
            //if (result != null) id = Convert.ToInt32(result);

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evencorr, DbType.Int32, entity.Evencorr);
            dbProvider.AddInParameter(command, helper.Eveninffechemis, DbType.DateTime, entity.Eveninffechemis);
            dbProvider.AddInParameter(command, helper.Eveninfelab, DbType.String, entity.Eveninfelab);
            dbProvider.AddInParameter(command, helper.Eveninfrevs, DbType.String, entity.Eveninfrevs);
            dbProvider.AddInParameter(command, helper.Eveninflastuser, DbType.String, entity.Eveninflastuser);
            dbProvider.AddInParameter(command, helper.Eveninflastdate, DbType.DateTime, entity.Eveninflastdate);
            dbProvider.AddInParameter(command, helper.Eveninfemitido, DbType.String, entity.Eveninfemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpfechemis, DbType.DateTime, entity.Eveninfpfechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpelab, DbType.String, entity.Eveninfpelab);
            dbProvider.AddInParameter(command, helper.Eveninfprevs, DbType.String, entity.Eveninfprevs);
            dbProvider.AddInParameter(command, helper.Eveninfpifechemis, DbType.DateTime, entity.Eveninfpifechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpielab, DbType.String, entity.Eveninfpielab);
            dbProvider.AddInParameter(command, helper.Eveninfpirevs, DbType.String, entity.Eveninfpirevs);
            dbProvider.AddInParameter(command, helper.Eveninfpemitido, DbType.String, entity.Eveninfpemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpiemitido, DbType.String, entity.Eveninfpiemitido);
            dbProvider.AddInParameter(command, helper.Eveninfmem, DbType.String, entity.Eveninfmem);
            dbProvider.AddInParameter(command, helper.Eveninfpiemit, DbType.String, entity.Eveninfpiemit);
            dbProvider.AddInParameter(command, helper.Eveninfpemit, DbType.String, entity.Eveninfpemit);
            dbProvider.AddInParameter(command, helper.Eveninfemit, DbType.String, entity.Eveninfemit);
            dbProvider.AddInParameter(command, helper.Evencorrmem, DbType.Int32, entity.Evencorrmem);
            dbProvider.AddInParameter(command, helper.Eveninfmemfechemis, DbType.DateTime, entity.Eveninfmemfechemis);
            dbProvider.AddInParameter(command, helper.Eveninfmemelab, DbType.String, entity.Eveninfmemelab);
            dbProvider.AddInParameter(command, helper.Eveninfmemrevs, DbType.String, entity.Eveninfmemrevs);
            dbProvider.AddInParameter(command, helper.Eveninfmememit, DbType.String, entity.Eveninfmememit);
            dbProvider.AddInParameter(command, helper.Eveninfmememitido, DbType.String, entity.Eveninfmememitido);
            dbProvider.AddInParameter(command, helper.EvencorrSco, DbType.Int32, entity.EvencorrSco);
            dbProvider.AddInParameter(command, helper.Eveninfactuacion, DbType.String, entity.Eveninfactuacion);
            dbProvider.AddInParameter(command, helper.Eveninfactllamado, DbType.String, entity.Eveninfactllamado);
            dbProvider.AddInParameter(command, helper.Eveninfactelab, DbType.String, entity.Eveninfactelab);
            dbProvider.AddInParameter(command, helper.Eveninfactfecha, DbType.DateTime, entity.Eveninfactfecha);

            dbProvider.ExecuteNonQuery(command);
            return entity.Eveninfcodi;
        }

        public int SaveEvento(EveInformefallaDTO entity)
        {

            int id = entity.Eveninfcodi;

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSaveEvento);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evencorr, DbType.Int32, entity.Evencorr);
            dbProvider.AddInParameter(command, helper.Eveninflastuser, DbType.String, entity.Eveninflastuser);
            dbProvider.AddInParameter(command, helper.Eveninflastdate, DbType.DateTime, entity.Eveninflastdate);
            dbProvider.AddInParameter(command, helper.Eveninfemitido, DbType.String, entity.Eveninfemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpemitido, DbType.String, entity.Eveninfpemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpiemitido, DbType.String, entity.Eveninfpiemitido);
            dbProvider.AddInParameter(command, helper.Eveninfmem, DbType.String, entity.Eveninfmem);
            dbProvider.AddInParameter(command, helper.Evencorrmem, DbType.Int32, entity.Evencorrmem);
            dbProvider.AddInParameter(command, helper.Eveninfmememitido, DbType.String, entity.Eveninfmememitido);
            dbProvider.AddInParameter(command, helper.EvencorrSco, DbType.Int32, entity.EvencorrSco);
            dbProvider.AddInParameter(command, helper.Eveninfactuacion, DbType.String, entity.Eveninfactuacion);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasipi, DbType.Int32, entity.Eveninfplazodiasipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasif, DbType.Int32, entity.Eveninfplazodiasif);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraipi, DbType.Int32, entity.Eveninfplazohoraipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraif, DbType.Int32, entity.Eveninfplazohoraif);
            dbProvider.AddInParameter(command, helper.Eveninfplazominipi, DbType.Int32, entity.Eveninfplazominipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazominif, DbType.Int32, entity.Eveninfplazominif);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveInformefallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, entity.Evenanio);
            dbProvider.AddInParameter(command, helper.Evencorr, DbType.Int32, entity.Evencorr);
            dbProvider.AddInParameter(command, helper.Eveninffechemis, DbType.DateTime, entity.Eveninffechemis);
            dbProvider.AddInParameter(command, helper.Eveninfelab, DbType.String, entity.Eveninfelab);
            dbProvider.AddInParameter(command, helper.Eveninfrevs, DbType.String, entity.Eveninfrevs);
            dbProvider.AddInParameter(command, helper.Eveninflastuser, DbType.String, entity.Eveninflastuser);
            dbProvider.AddInParameter(command, helper.Eveninflastdate, DbType.DateTime, entity.Eveninflastdate);
            dbProvider.AddInParameter(command, helper.Eveninfemitido, DbType.String, entity.Eveninfemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpfechemis, DbType.DateTime, entity.Eveninfpfechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpelab, DbType.String, entity.Eveninfpelab);
            dbProvider.AddInParameter(command, helper.Eveninfprevs, DbType.String, entity.Eveninfprevs);
            dbProvider.AddInParameter(command, helper.Eveninfpifechemis, DbType.DateTime, entity.Eveninfpifechemis);
            dbProvider.AddInParameter(command, helper.Eveninfpielab, DbType.String, entity.Eveninfpielab);
            dbProvider.AddInParameter(command, helper.Eveninfpirevs, DbType.String, entity.Eveninfpirevs);
            dbProvider.AddInParameter(command, helper.Eveninfpemitido, DbType.String, entity.Eveninfpemitido);
            dbProvider.AddInParameter(command, helper.Eveninfpiemitido, DbType.String, entity.Eveninfpiemitido);
            dbProvider.AddInParameter(command, helper.Eveninfmem, DbType.String, entity.Eveninfmem);
            dbProvider.AddInParameter(command, helper.Eveninfpiemit, DbType.String, entity.Eveninfpiemit);
            dbProvider.AddInParameter(command, helper.Eveninfpemit, DbType.String, entity.Eveninfpemit);
            dbProvider.AddInParameter(command, helper.Eveninfemit, DbType.String, entity.Eveninfemit);
            dbProvider.AddInParameter(command, helper.Evencorrmem, DbType.Int32, entity.Evencorrmem);
            dbProvider.AddInParameter(command, helper.Eveninfmemfechemis, DbType.DateTime, entity.Eveninfmemfechemis);
            dbProvider.AddInParameter(command, helper.Eveninfmemelab, DbType.String, entity.Eveninfmemelab);
            dbProvider.AddInParameter(command, helper.Eveninfmemrevs, DbType.String, entity.Eveninfmemrevs);
            dbProvider.AddInParameter(command, helper.Eveninfmememit, DbType.String, entity.Eveninfmememit);
            dbProvider.AddInParameter(command, helper.Eveninfmememitido, DbType.String, entity.Eveninfmememitido);
            dbProvider.AddInParameter(command, helper.EvencorrSco, DbType.Int32, entity.EvencorrSco);
            dbProvider.AddInParameter(command, helper.Eveninfactuacion, DbType.String, entity.Eveninfactuacion);
            dbProvider.AddInParameter(command, helper.Eveninfactllamado, DbType.String, entity.Eveninfactllamado);
            dbProvider.AddInParameter(command, helper.Eveninfactelab, DbType.String, entity.Eveninfactelab);
            dbProvider.AddInParameter(command, helper.Eveninfactfecha, DbType.DateTime, entity.Eveninfactfecha);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int eveninfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, eveninfcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveInformefallaDTO GetById(int eveninfcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, eveninfcodi);
            EveInformefallaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {

                if (dr.Read())
                {
                    entity = new EveInformefallaDTO();

                    int iEveninfcodi = dr.GetOrdinal(this.helper.Eveninfcodi);
                    if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = Convert.ToInt32(dr.GetValue(iEvencodi));

                    int iEvenanio = dr.GetOrdinal(this.helper.Evenanio);
                    if (!dr.IsDBNull(iEvenanio)) entity.Evenanio = Convert.ToInt32(dr.GetValue(iEvenanio));

                    int iEvencorr = dr.GetOrdinal(this.helper.Evencorr);
                    if (!dr.IsDBNull(iEvencorr)) entity.Evencorr = Convert.ToInt32(dr.GetValue(iEvencorr));

                    int iEveninffechemis = dr.GetOrdinal(this.helper.Eveninffechemis);
                    if (!dr.IsDBNull(iEveninffechemis)) entity.Eveninffechemis = dr.GetDateTime(iEveninffechemis);

                    int iEveninfelab = dr.GetOrdinal(this.helper.Eveninfelab);
                    if (!dr.IsDBNull(iEveninfelab)) entity.Eveninfelab = dr.GetString(iEveninfelab);

                    int iEveninfrevs = dr.GetOrdinal(this.helper.Eveninfrevs);
                    if (!dr.IsDBNull(iEveninfrevs)) entity.Eveninfrevs = dr.GetString(iEveninfrevs);

                    int iEveninflastuser = dr.GetOrdinal(this.helper.Eveninflastuser);
                    if (!dr.IsDBNull(iEveninflastuser)) entity.Eveninflastuser = dr.GetString(iEveninflastuser);

                    int iEveninflastdate = dr.GetOrdinal(this.helper.Eveninflastdate);
                    if (!dr.IsDBNull(iEveninflastdate)) entity.Eveninflastdate = dr.GetDateTime(iEveninflastdate);

                    int iEveninfemitido = dr.GetOrdinal(this.helper.Eveninfemitido);
                    if (!dr.IsDBNull(iEveninfemitido)) entity.Eveninfemitido = dr.GetString(iEveninfemitido);

                    int iEveninfpfechemis = dr.GetOrdinal(this.helper.Eveninfpfechemis);
                    if (!dr.IsDBNull(iEveninfpfechemis)) entity.Eveninfpfechemis = dr.GetDateTime(iEveninfpfechemis);

                    int iEveninfpelab = dr.GetOrdinal(this.helper.Eveninfpelab);
                    if (!dr.IsDBNull(iEveninfpelab)) entity.Eveninfpelab = dr.GetString(iEveninfpelab);

                    int iEveninfprevs = dr.GetOrdinal(this.helper.Eveninfprevs);
                    if (!dr.IsDBNull(iEveninfprevs)) entity.Eveninfprevs = dr.GetString(iEveninfprevs);

                    int iEveninfpifechemis = dr.GetOrdinal(this.helper.Eveninfpifechemis);
                    if (!dr.IsDBNull(iEveninfpifechemis)) entity.Eveninfpifechemis = dr.GetDateTime(iEveninfpifechemis);

                    int iEveninfpielab = dr.GetOrdinal(this.helper.Eveninfpielab);
                    if (!dr.IsDBNull(iEveninfpielab)) entity.Eveninfpielab = dr.GetString(iEveninfpielab);

                    int iEveninfpirevs = dr.GetOrdinal(this.helper.Eveninfpirevs);
                    if (!dr.IsDBNull(iEveninfpirevs)) entity.Eveninfpirevs = dr.GetString(iEveninfpirevs);

                    int iEveninfpemitido = dr.GetOrdinal(this.helper.Eveninfpemitido);
                    if (!dr.IsDBNull(iEveninfpemitido)) entity.Eveninfpemitido = dr.GetString(iEveninfpemitido);

                    int iEveninfpiemitido = dr.GetOrdinal(this.helper.Eveninfpiemitido);
                    if (!dr.IsDBNull(iEveninfpiemitido)) entity.Eveninfpiemitido = dr.GetString(iEveninfpiemitido);

                    int iEveninfmem = dr.GetOrdinal(this.helper.Eveninfmem);
                    if (!dr.IsDBNull(iEveninfmem)) entity.Eveninfmem = dr.GetString(iEveninfmem);

                    int iEveninfpiemit = dr.GetOrdinal(this.helper.Eveninfpiemit);
                    if (!dr.IsDBNull(iEveninfpiemit)) entity.Eveninfpiemit = dr.GetString(iEveninfpiemit);

                    int iEveninfpemit = dr.GetOrdinal(this.helper.Eveninfpemit);
                    if (!dr.IsDBNull(iEveninfpemit)) entity.Eveninfpemit = dr.GetString(iEveninfpemit);

                    int iEveninfemit = dr.GetOrdinal(this.helper.Eveninfemit);
                    if (!dr.IsDBNull(iEveninfemit)) entity.Eveninfemit = dr.GetString(iEveninfemit);

                    int iEvencorrmem = dr.GetOrdinal(this.helper.Evencorrmem);
                    if (!dr.IsDBNull(iEvencorrmem)) entity.Evencorrmem = Convert.ToInt32(dr.GetValue(iEvencorrmem));

                    int iEveninfmemfechemis = dr.GetOrdinal(this.helper.Eveninfmemfechemis);
                    if (!dr.IsDBNull(iEveninfmemfechemis)) entity.Eveninfmemfechemis = dr.GetDateTime(iEveninfmemfechemis);

                    int iEveninfmemelab = dr.GetOrdinal(this.helper.Eveninfmemelab);
                    if (!dr.IsDBNull(iEveninfmemelab)) entity.Eveninfmemelab = dr.GetString(iEveninfmemelab);

                    int iEveninfmemrevs = dr.GetOrdinal(this.helper.Eveninfmemrevs);
                    if (!dr.IsDBNull(iEveninfmemrevs)) entity.Eveninfmemrevs = dr.GetString(iEveninfmemrevs);

                    int iEveninfmememit = dr.GetOrdinal(this.helper.Eveninfmememit);
                    if (!dr.IsDBNull(iEveninfmememit)) entity.Eveninfmememit = dr.GetString(iEveninfmememit);

                    int iEveninfmememitido = dr.GetOrdinal(this.helper.Eveninfmememitido);
                    if (!dr.IsDBNull(iEveninfmememitido)) entity.Eveninfmememitido = dr.GetString(iEveninfmememitido);

                    int iEvencorrSco = dr.GetOrdinal(this.helper.EvencorrSco);
                    if (!dr.IsDBNull(iEvencorrSco)) entity.EvencorrSco = Convert.ToInt32(dr.GetValue(iEvencorrSco));

                    int iEveninfactuacion = dr.GetOrdinal(this.helper.Eveninfactuacion);
                    if (!dr.IsDBNull(iEveninfactuacion)) entity.Eveninfactuacion = dr.GetString(iEveninfactuacion);

                    int iEveninfactllamado = dr.GetOrdinal(this.helper.Eveninfactllamado);
                    if (!dr.IsDBNull(iEveninfactllamado)) entity.Eveninfactllamado = dr.GetString(iEveninfactllamado);

                    int iEveninfactelab = dr.GetOrdinal(this.helper.Eveninfactelab);
                    if (!dr.IsDBNull(iEveninfactelab)) entity.Eveninfactelab = dr.GetString(iEveninfactelab);

                    int iEveninfactfecha = dr.GetOrdinal(this.helper.Eveninfactfecha);
                    if (!dr.IsDBNull(iEveninfactfecha)) entity.Eveninfactfecha = dr.GetDateTime(iEveninfactfecha);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iEvenmwindisp = dr.GetOrdinal(this.helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = Convert.ToDecimal(dr.GetValue(iEvenmwindisp));

                    int iTareaabrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);
                }

            }

            return entity;
        }

        public List<EveInformefallaDTO> List()
        {
            List<EveInformefallaDTO> entitys = new List<EveInformefallaDTO>();
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

        public List<EveInformefallaDTO> GetByCriteria()
        {
            List<EveInformefallaDTO> entitys = new List<EveInformefallaDTO>();
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

        public int ValidarInformeFallaN1(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlValidarInformeFallaN1);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);

            object result = dbProvider.ExecuteScalar(command);

            if (result != null)
            {
                return Convert.ToInt32(result);
            }

            return 0;
        }

        public void EliminarInformeFallaN1(int idEvento)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlEliminarInformeFallaN1);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, idEvento);
            dbProvider.ExecuteNonQuery(command);
        }

        public void ObtenerCorrelativoInformeFalla(int nroAnio, out int correlativoMen, out int correlativo, out int correlativoSco)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlObtenerCorrelativoInformeFalla);
            dbProvider.AddInParameter(command, helper.Evenanio, DbType.Int32, nroAnio);
            correlativoMen = 0;
            correlativo = 0;
            correlativoSco = 0;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    int iEvencorrmen = dr.GetOrdinal(helper.Evencorrmem);
                    if (!dr.IsDBNull(iEvencorrmen)) correlativoMen = Convert.ToInt32(dr.GetValue(iEvencorrmen));

                    int iEvencorr = dr.GetOrdinal(helper.Evencorr);
                    if (!dr.IsDBNull(iEvencorr)) correlativo = Convert.ToInt32(dr.GetValue(iEvencorr));

                    int iEvencorrsco = dr.GetOrdinal(helper.EvencorrSco);
                    if (!dr.IsDBNull(iEvencorrsco)) correlativoSco = Convert.ToInt32(dr.GetValue(iEvencorrsco));
                }
            }
        }

        public List<EveInformefallaDTO> BuscarOperaciones(string infMem, string infEmitido, int emprCodi, string equiAbrev, DateTime fechaIni, DateTime fechaFin,
            int nroPage, int pageSize)
        {
            List<EveInformefallaDTO> entitys = new List<EveInformefallaDTO>();
            String sql = String.Format(this.helper.ObtenerListado, infMem, infEmitido, emprCodi,
                equiAbrev, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformefallaDTO entity = new EveInformefallaDTO();

                    int iEveninfcodi = dr.GetOrdinal(this.helper.Eveninfcodi);
                    if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

                    int iCorrmem = dr.GetOrdinal(this.helper.Corrmem);
                    if (!dr.IsDBNull(iCorrmem)) entity.Corrmem = dr.GetString(iCorrmem);

                    int iEvencorrSco = dr.GetOrdinal(this.helper.EvencorrSco);
                    if (!dr.IsDBNull(iEvencorrSco)) entity.EvencorrSco = Convert.ToInt32(dr.GetValue(iEvencorrSco));

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iTareaabrev = dr.GetOrdinal(this.helper.Tareaabrev);
                    if (!dr.IsDBNull(iTareaabrev)) entity.Tareaabrev = dr.GetString(iTareaabrev);

                    int iAreanomb = dr.GetOrdinal(this.helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iFamabrev = dr.GetOrdinal(this.helper.Famabrev);
                    if (!dr.IsDBNull(iFamabrev)) entity.Famabrev = dr.GetString(iFamabrev);

                    int iEquiabrev = dr.GetOrdinal(this.helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEvenmwindisp = dr.GetOrdinal(this.helper.Evenmwindisp);
                    if (!dr.IsDBNull(iEvenmwindisp)) entity.Evenmwindisp = Convert.ToDecimal(dr.GetValue(iEvenmwindisp));

                    int iEvenini = dr.GetOrdinal(this.helper.Evenini);
                    if (!dr.IsDBNull(iEvenini)) entity.Evenini = dr.GetDateTime(iEvenini);

                    int iExtosinerg = dr.GetOrdinal(this.helper.ExtOsinerg);
                    if (!dr.IsDBNull(iExtosinerg)) entity.Extosinerg = dr.GetString(iExtosinerg);

                    int iObsprelimini = dr.GetOrdinal(this.helper.ObsPrelimIni);
                    if (!dr.IsDBNull(iObsprelimini)) entity.Obsprelimini = dr.GetString(iObsprelimini);

                    int iObsprelim = dr.GetOrdinal(this.helper.ObsPrelim);
                    if (!dr.IsDBNull(iObsprelim)) entity.Obsprelim = dr.GetString(iObsprelim);

                    int iObsfinal = dr.GetOrdinal(this.helper.ObsFinal);
                    if (!dr.IsDBNull(iObsfinal)) entity.Obsfinal = dr.GetString(iObsfinal);

                    int iObsmem = dr.GetOrdinal(this.helper.ObsMem);
                    if (!dr.IsDBNull(iObsmem)) entity.Obsmem = dr.GetString(iObsmem);

                    int iEveninflastuser = dr.GetOrdinal(this.helper.Eveninflastuser);
                    if (!dr.IsDBNull(iEveninflastuser)) entity.Eveninflastuser = dr.GetString(iEveninflastuser);

                    int iEveninflastdate = dr.GetOrdinal(this.helper.Eveninflastdate);
                    if (!dr.IsDBNull(iEveninflastdate)) entity.Eveninflastdate = dr.GetDateTime(iEveninflastdate);

                    int iEveninfmem = dr.GetOrdinal(this.helper.Eveninfmem);
                    if (!dr.IsDBNull(iEveninfmem)) entity.Eveninfmem = dr.GetString(iEveninfmem);

                    int iEvencodi = dr.GetOrdinal(this.helper.Evencodi);
                    if (!dr.IsDBNull(iEvencodi)) entity.Evencodi = dr.GetInt32(iEvencodi);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public int ObtenerNroFilas(string infMem, string infEmitido, int emprCodi, string equiAbrev, DateTime fechaIni, DateTime fechaFin)
        {
            List<EveInformefallaDTO> entitys = new List<EveInformefallaDTO>();
            String sql = String.Format(this.helper.TotalRegistros, infMem, infEmitido, emprCodi,
              equiAbrev, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));

            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            object count = dbProvider.ExecuteScalar(command);

            if (count != null) return Convert.ToInt32(count);
            return 0;
        }

        public List<EveInformefallaDTO> ObtenerAlertaInformeFalla()
        {
            List<EveInformefallaDTO> entitys = new List<EveInformefallaDTO>();
            string sql = string.Format(helper.SqlObtenerAlertaInformeFalla);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveInformefallaDTO entity = new EveInformefallaDTO();

                    int iCorrelativo = dr.GetOrdinal(helper.Correlativo);
                    if (!dr.IsDBNull(iCorrelativo)) entity.Correlativo = dr.GetString(iCorrelativo);

                    int iPlazo = dr.GetOrdinal(helper.Plazo);
                    if (!dr.IsDBNull(iPlazo)) entity.Plazo = dr.GetDecimal(iPlazo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public EveInformefallaDTO MostrarEventoInformeFalla(int evencodi)
        {
            EveInformefallaDTO entity = new EveInformefallaDTO();
            String sql = String.Format(this.helper.SqlMostrarEventoInformeFalla, evencodi);

            DbCommand command = dbProvider.GetSqlStringCommand(sql);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    int iEveninfcodi = dr.GetOrdinal(this.helper.Eveninfcodi);
                    if (!dr.IsDBNull(iEveninfcodi)) entity.Eveninfcodi = Convert.ToInt32(dr.GetValue(iEveninfcodi));

                    int iEveninfplazodiasipi = dr.GetOrdinal(this.helper.Eveninfplazodiasipi);
                    if (!dr.IsDBNull(iEveninfplazodiasipi)) entity.Eveninfplazodiasipi = Convert.ToInt32(dr.GetValue(iEveninfplazodiasipi));

                    int iEveninfplazodiasif = dr.GetOrdinal(this.helper.Eveninfplazodiasif);
                    if (!dr.IsDBNull(iEveninfplazodiasif)) entity.Eveninfplazodiasif= Convert.ToInt32(dr.GetValue(iEveninfplazodiasif));

                    int iEveninfplazohoraipi = dr.GetOrdinal(this.helper.Eveninfplazohoraipi);
                    if (!dr.IsDBNull(iEveninfplazohoraipi)) entity.Eveninfplazohoraipi = Convert.ToInt32(dr.GetValue(iEveninfplazohoraipi));

                    int iEveninfplazohoraif = dr.GetOrdinal(this.helper.Eveninfplazohoraif);
                    if (!dr.IsDBNull(iEveninfplazohoraif)) entity.Eveninfplazohoraif = Convert.ToInt32(dr.GetValue(iEveninfplazohoraif));

                    int iEveninfplazominipi = dr.GetOrdinal(this.helper.Eveninfplazominipi);
                    if (!dr.IsDBNull(iEveninfplazominipi)) entity.Eveninfplazominipi = Convert.ToInt32(dr.GetValue(iEveninfplazominipi));

                    int iEveninfplazominif = dr.GetOrdinal(this.helper.Eveninfplazominif);
                    if (!dr.IsDBNull(iEveninfplazominif)) entity.Eveninfplazominif = Convert.ToInt32(dr.GetValue(iEveninfplazominif));

                    int iEvencorr = dr.GetOrdinal(this.helper.Evencorr);
                    if (!dr.IsDBNull(iEvencorr)) entity.Evencorr = Convert.ToInt32(dr.GetValue(iEvencorr));
                }
            }
   
            return entity;
        }


        public void ActualizarAmpliacion(EveInformefallaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlActualizarAmpliacion);

           
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasipi, DbType.Int32, entity.Eveninfplazodiasipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazodiasif, DbType.Int32, entity.Eveninfplazodiasif);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraipi, DbType.Int32, entity.Eveninfplazohoraipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazohoraif, DbType.Int32, entity.Eveninfplazohoraif);
            dbProvider.AddInParameter(command, helper.Eveninfplazominipi, DbType.Int32, entity.Eveninfplazominipi);
            dbProvider.AddInParameter(command, helper.Eveninfplazominif, DbType.Int32, entity.Eveninfplazominif);
            dbProvider.AddInParameter(command, helper.Eveninfcodi, DbType.Int32, entity.Eveninfcodi);
            dbProvider.ExecuteNonQuery(command);
        }
    }
}

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
    /// Clase de acceso a datos de la tabla EVE_HORAOPERACION
    /// </summary>
    public class EveHoraoperacionRepository : RepositoryBase, IEveHoraoperacionRepository
    {
        public EveHoraoperacionRepository(string strConn)
            : base(strConn)
        {
        }

        EveHoraoperacionHelper helper = new EveHoraoperacionHelper();


        public int Save(EveHoraoperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlSave);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;
            int id = entity.Hopcodi;
            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Hophorini, DbType.DateTime, entity.Hophorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Hophorfin, DbType.DateTime, entity.Hophorfin);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hopdesc, DbType.String, entity.Hopdesc);
            dbProvider.AddInParameter(command, helper.Hophorordarranq, DbType.DateTime, entity.Hophorordarranq);
            dbProvider.AddInParameter(command, helper.Hophorparada, DbType.DateTime, entity.Hophorparada);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopsaislado, DbType.Int32, entity.Hopsaislado);
            dbProvider.AddInParameter(command, helper.Hoplimtrans, DbType.String, entity.Hoplimtrans);
            dbProvider.AddInParameter(command, helper.Hopfalla, DbType.String, entity.Hopfalla);
            dbProvider.AddInParameter(command, helper.Hopcompordarrq, DbType.String, entity.Hopcompordarrq);
            dbProvider.AddInParameter(command, helper.Hopcompordpard, DbType.String, entity.Hopcompordpard);
            dbProvider.AddInParameter(command, helper.Hopcausacodi, DbType.Int32, entity.Hopcausacodi);
            dbProvider.AddInParameter(command, helper.Hopcodipadre, DbType.Int32, entity.Hopcodipadre);
            dbProvider.AddInParameter(command, helper.Hopestado, DbType.String, entity.Hopestado);
            dbProvider.AddInParameter(command, helper.Hopnotifuniesp, DbType.Int32, entity.Hopnotifuniesp);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Hopobs, DbType.String, entity.Hopobs);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hoparrqblackstart, DbType.String, entity.Hoparrqblackstart);
            dbProvider.AddInParameter(command, helper.Hopensayope, DbType.String, entity.Hopensayope);
            dbProvider.AddInParameter(command, helper.Hopensayopmin, DbType.String, entity.Hopensayopmin);

            dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(EveHoraoperacionDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            entity.Emprcodi = entity.Emprcodi > 0 ? entity.Emprcodi : -1;

            dbProvider.AddInParameter(command, helper.Hophorini, DbType.DateTime, entity.Hophorini);
            dbProvider.AddInParameter(command, helper.Subcausacodi, DbType.Int32, entity.Subcausacodi);
            dbProvider.AddInParameter(command, helper.Hophorfin, DbType.DateTime, entity.Hophorfin);
            dbProvider.AddInParameter(command, helper.Equicodi, DbType.Int32, entity.Equicodi);
            dbProvider.AddInParameter(command, helper.Hopdesc, DbType.String, entity.Hopdesc);
            dbProvider.AddInParameter(command, helper.Hophorordarranq, DbType.DateTime, entity.Hophorordarranq);
            dbProvider.AddInParameter(command, helper.Hophorparada, DbType.DateTime, entity.Hophorparada);
            dbProvider.AddInParameter(command, helper.Lastuser, DbType.String, entity.Lastuser);
            dbProvider.AddInParameter(command, helper.Lastdate, DbType.DateTime, entity.Lastdate);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Hopsaislado, DbType.Int32, entity.Hopsaislado);
            dbProvider.AddInParameter(command, helper.Hoplimtrans, DbType.String, entity.Hoplimtrans);
            dbProvider.AddInParameter(command, helper.Hopfalla, DbType.String, entity.Hopfalla);
            dbProvider.AddInParameter(command, helper.Hopcompordarrq, DbType.String, entity.Hopcompordarrq);
            dbProvider.AddInParameter(command, helper.Hopcompordpard, DbType.String, entity.Hopcompordpard);
            dbProvider.AddInParameter(command, helper.Hopcausacodi, DbType.Int32, entity.Hopcausacodi);
            dbProvider.AddInParameter(command, helper.Hopcodipadre, DbType.Int32, entity.Hopcodipadre);
            dbProvider.AddInParameter(command, helper.Hopestado, DbType.String, entity.Hopestado);
            dbProvider.AddInParameter(command, helper.Hopnotifuniesp, DbType.Int32, entity.Hopnotifuniesp);
            dbProvider.AddInParameter(command, helper.Evencodi, DbType.Int32, entity.Evencodi);
            dbProvider.AddInParameter(command, helper.Hopobs, DbType.String, entity.Hopobs);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Hoparrqblackstart, DbType.String, entity.Hoparrqblackstart);
            dbProvider.AddInParameter(command, helper.Hopensayope, DbType.String, entity.Hopensayope);
            dbProvider.AddInParameter(command, helper.HopPruebaExitosa, DbType.Int32, entity.HopPruebaExitosa);
            dbProvider.AddInParameter(command, helper.Hopensayopmin, DbType.String, entity.Hopensayopmin);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, entity.Hopcodi);
            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int hopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, hopcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public EveHoraoperacionDTO GetById(int hopcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Hopcodi, DbType.Int32, hopcodi);

            EveHoraoperacionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }
            return entity;
        }

        public List<EveHoraoperacionDTO> List()
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
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

        public List<EveHoraoperacionDTO> GetByCriteria(DateTime fecha)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria,
                fecha.ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));
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

        public List<EveHoraoperacionDTO> GetByDetalleHO(DateTime fecha)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetByDetalleHO,
                fecha.ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            EveHoraoperacionDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);
                    entity.Unidad = dr.IsDBNull(dr.GetOrdinal(helper.Unidad)) ? -1 : dr.GetInt32(dr.GetOrdinal(helper.Unidad));
                    entity.Grupopadre = dr.IsDBNull(dr.GetOrdinal(helper.Grupopadre)) ? -1 : dr.GetInt32(dr.GetOrdinal(helper.Grupopadre));
                    entity.Gruponomb = dr.GetString(dr.GetOrdinal(helper.Gruponomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> GetByCriteria(DateTime fechaInicio, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetByCriteria,
                fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFin.AddDays(1).ToString(ConstantesBase.FormatoFecha));
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

        public List<EveHoraoperacionDTO> GetByCriteriaXEmpresaxFecha(int emprcodi, DateTime fecha, DateTime fechafin, int idCentral)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetByCriteriaXEmpresaxFecha, emprcodi,
                fecha.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iEquipoNombre = dr.GetOrdinal(this.helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(this.helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iCodipadre = dr.GetOrdinal(this.helper.Codipadre);
                    if (!dr.IsDBNull(iCodipadre)) entity.CodiPadre = dr.GetInt32(iCodipadre);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> GetByCriteriaUnidadesXEmpresaxFecha(int emprcodi, DateTime fecha, DateTime fechafin, int idCentral)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetByCriteriaUnidadesXEmpresaxFecha, emprcodi,
                fecha.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    var entity = helper.Create(dr);

                    int iEquipoNombre = dr.GetOrdinal(this.helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(this.helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListEquiposHorasOperacionxFormato(int formatcodi, int emprcodi, DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListEquiposHorasOperacionxFormato, formatcodi, emprcodi,
                fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
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

        public List<EveHoraoperacionDTO> ListarHorasOperacxEmpresaxFechaxTipoOPxFam(int emprcodi, DateTime fechaini, DateTime fechafin, string idTipoOperacion, int famcodi)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListarHorasOperacxEmpresaxFechaxTipoOP, emprcodi,
                fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), idTipoOperacion, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquipoNombre = dr.GetOrdinal(helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);
                    int iPadreNombre = dr.GetOrdinal(helper.Padrenombre);
                    if (!dr.IsDBNull(iPadreNombre)) entity.PadreNombre = dr.GetString(iPadreNombre);
                    int iFamCodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.Famcodi = dr.GetInt32(iFamCodi);

                    int iFenergCodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergCodi)) entity.Fenergcodi = dr.GetInt32(iFenergCodi);

                    int iFenergNomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergNomb)) entity.Fenergnomb = dr.GetString(iFenergNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam(int emprcodi, DateTime fechaini, DateTime fechafin, string idTipoOperacion, int famcodi)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListarHorasOperacxEquiposXEmpXTipoOPxFam, emprcodi,
                fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), idTipoOperacion, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquipoNombre = dr.GetOrdinal(helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);
                    int iPadreNombre = dr.GetOrdinal(helper.Padrenombre);
                    if (!dr.IsDBNull(iPadreNombre)) entity.PadreNombre = dr.GetString(iPadreNombre);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iFenergCodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergCodi)) entity.Fenergcodi = dr.GetInt32(iFenergCodi);

                    int iFenergNomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergNomb)) entity.Fenergnomb = dr.GetString(iFenergNomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListarHorasOperacxEquiposXEmpXTipoOPxFam2(int emprcodi, DateTime fechaini, DateTime fechafin, string idTipoOperacion, int idCentral)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListarHorasOperacxEquiposXEmpXTipoOPxFam2, emprcodi,
                fechaini.ToString(ConstantesBase.FormatoFecha), fechafin.ToString(ConstantesBase.FormatoFecha), idTipoOperacion, idCentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = helper.Create(dr);

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);
                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);
                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iEquipoNombre = dr.GetOrdinal(helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);
                    int iPadreNombre = dr.GetOrdinal(helper.Padrenombre);
                    if (!dr.IsDBNull(iPadreNombre)) entity.PadreNombre = dr.GetString(iPadreNombre);
                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    int iFenergCodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergCodi)) entity.Fenergcodi = dr.GetInt32(iFenergCodi);

                    int iFenergNomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergNomb)) entity.Fenergnomb = dr.GetString(iFenergNomb);

                    int iOsinergcodi = dr.GetOrdinal(helper.Osinergcodi);
                    if (!dr.IsDBNull(iOsinergcodi)) entity.Osinergcodi = dr.GetString(iOsinergcodi);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> GetCriteriaxPKCodis(string pkCodis)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetCriteriaxPKCodis, pkCodis);
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = helper.Create(dr);

                    int iEquipadre = dr.GetOrdinal(helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = dr.GetInt32(iEquipadre);

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        public List<EveHoraoperacionDTO> GetCriteriaUnidadesxPKCodis(string pkCodis)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetCriteriaUnidadesxPKCodis, pkCodis);
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

        public List<EveHoraoperacionDTO> GetHorasURS(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlGetHorasURS, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = helper.Create(dr);
                    int iGrupourspadre = dr.GetOrdinal(helper.Grupourspadre);
                    if (!dr.IsDBNull(iGrupourspadre)) entity.Grupourspadre = dr.GetInt32(iGrupourspadre);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        #region Horas Operacion EMS
        public List<EveHoraoperacionDTO> ListarHorasOperacionByCriteria(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales, int tipoListado)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();

            string query = string.Format(helper.SqlListarHorasOperacionByCriteria, dfechaIni.ToString(ConstantesBase.FormatoFecha), dfechaFin.ToString(ConstantesBase.FormatoFecha)
                , empresas, centrales, tipoListado);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = this.helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc).Trim();

                    int iEquipoNombre = dr.GetOrdinal(this.helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iFamCodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.Famcodi = dr.GetInt32(iFamCodi);

                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iFlagTipoHo = dr.GetOrdinal(this.helper.FlagTipoHo);
                    if (!dr.IsDBNull(iFlagTipoHo)) entity.FlagTipoHo = Convert.ToInt32(dr.GetValue(iFlagTipoHo));

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(this.helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iCodipadre = dr.GetOrdinal(this.helper.Codipadre);
                    if (!dr.IsDBNull(iCodipadre)) entity.CodiPadre = Convert.ToInt32(dr.GetValue(iCodipadre));

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    int iPruebaExitosa = dr.GetOrdinal(this.helper.HopPruebaExitosa);
                    if (!dr.IsDBNull(iPruebaExitosa)) entity.HopPruebaExitosa = Convert.ToInt32(dr.GetValue(iPruebaExitosa));

                    int iGrupotipomodo = dr.GetOrdinal(this.helper.Grupotipomodo);
                    if (!dr.IsDBNull(iGrupotipomodo)) entity.Grupotipomodo = dr.GetString(iGrupotipomodo);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListarHorasOperacionByCriteriaUnidades(DateTime dfechaIni, DateTime dfechaFin, string empresas, string centrales)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();

            string query = string.Format(helper.SqlListarHorasOperacionByCriteriaUnidades, dfechaIni.ToString(ConstantesBase.FormatoFecha), dfechaFin.ToString(ConstantesBase.FormatoFecha)
                , empresas, centrales);

            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = this.helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(this.helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iSubcausadesc = dr.GetOrdinal(this.helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    int iEquipoNombre = dr.GetOrdinal(this.helper.Equiponombre);
                    if (!dr.IsDBNull(iEquipoNombre)) entity.EquipoNombre = dr.GetString(iEquipoNombre);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);
                    int iFamCodi = dr.GetOrdinal(helper.Famcodi);
                    if (!dr.IsDBNull(iFamCodi)) entity.Famcodi = dr.GetInt32(iFamCodi);

                    int iGrupoabrev = dr.GetOrdinal(this.helper.Grupoabrev);
                    if (!dr.IsDBNull(iGrupoabrev)) entity.Grupoabrev = dr.GetString(iGrupoabrev);

                    int iCentral = dr.GetOrdinal(this.helper.Central);
                    if (!dr.IsDBNull(iCentral)) entity.Central = dr.GetString(iCentral);

                    int iEquipadre = dr.GetOrdinal(this.helper.Equipadre);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    int iEmprcodi = dr.GetOrdinal(this.helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iGruponomb = dr.GetOrdinal(this.helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iFlagTipoHo = dr.GetOrdinal(this.helper.FlagTipoHo);
                    if (!dr.IsDBNull(iFlagTipoHo)) entity.FlagTipoHo = Convert.ToInt32(dr.GetValue(iFlagTipoHo));

                    int iFenergcodi = dr.GetOrdinal(this.helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = Convert.ToInt32(dr.GetValue(iFenergcodi));

                    int iFenergnomb = dr.GetOrdinal(this.helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iFenercolor = dr.GetOrdinal(this.helper.Fenercolor);
                    if (!dr.IsDBNull(iFenercolor)) entity.Fenercolor = dr.GetString(iFenercolor);

                    int iGrupopadre = dr.GetOrdinal(this.helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region MigracionSGOCOES-GrupoB
        public List<EveHoraoperacionDTO> ListaEstadoOperacion(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListaEstadoOperacion, fechaIni.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);
                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListaEstadoOperacion90(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListaEstadoOperacion90, fechaIni.ToString(ConstantesBase.FormatoFechaExtendido), fechaFin.ToString(ConstantesBase.FormatoFechaExtendido));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);
                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iAreacodi = dr.GetOrdinal(helper.Areacodi);
                    if (!dr.IsDBNull(iAreacodi)) entity.Areacodi = Convert.ToInt32(dr.GetValue(iAreacodi));
                    int iAreanomb = dr.GetOrdinal(helper.Areanomb);
                    if (!dr.IsDBNull(iAreanomb)) entity.Areanomb = dr.GetString(iAreanomb);

                    int iEquiabrev = dr.GetOrdinal(helper.Equiabrev);
                    if (!dr.IsDBNull(iEquiabrev)) entity.Equiabrev = dr.GetString(iEquiabrev);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iSubcausacodi = dr.GetOrdinal(helper.Subcausacodi);
                    if (!dr.IsDBNull(iSubcausacodi)) entity.Subcausacodi = dr.GetInt32(iSubcausacodi);
                    int iSubcausadesc = dr.GetOrdinal(helper.Subcausadesc);
                    if (!dr.IsDBNull(iSubcausadesc)) entity.Subcausadesc = dr.GetString(iSubcausadesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListaProdTipCombustible(DateTime fechaIni, DateTime fechaFin)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListaProdTipCombustible, fechaIni.ToString(ConstantesBase.FormatoFecha), fechaFin.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = dr.GetInt32(iEquicodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = dr.GetInt32(iGrupopadre);

                    int iPtomedicodi = dr.GetOrdinal(helper.Ptomedicodi);
                    if (!dr.IsDBNull(iPtomedicodi)) entity.Ptomedicodi = dr.GetInt32(iPtomedicodi);

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iGrupocomb = dr.GetOrdinal(helper.Grupocomb);
                    if (!dr.IsDBNull(iGrupocomb)) entity.Grupocomb = dr.GetString(iGrupocomb);

                    int iFenergcodi = dr.GetOrdinal(helper.Fenergcodi);
                    if (!dr.IsDBNull(iFenergcodi)) entity.Fenergcodi = dr.GetInt32(iFenergcodi);

                    int iFenergnomb = dr.GetOrdinal(helper.Fenergnomb);
                    if (!dr.IsDBNull(iFenergnomb)) entity.Fenergnomb = dr.GetString(iFenergnomb);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = dr.GetInt32(iEmprcodi);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EveHoraoperacionDTO> ListaOperacionTension(DateTime fecIni)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            string query = string.Format(helper.SqlListaOperacionTension, fecIni.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iEmprabrev = dr.GetOrdinal(helper.Emprabrev);
                    if (!dr.IsDBNull(iEmprabrev)) entity.Emprabrev = dr.GetString(iEmprabrev);

                    int iGruponomb = dr.GetOrdinal(helper.Gruponomb);
                    if (!dr.IsDBNull(iGruponomb)) entity.Gruponomb = dr.GetString(iGruponomb);

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iHopdesc = dr.GetOrdinal(helper.Hopdesc);
                    if (!dr.IsDBNull(iHopdesc)) entity.Hopdesc = dr.GetString(iHopdesc);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

        #region Numerales Datos Base
        public List<EveHoraoperacionDTO> ListaNumerales_DatosBase_5_1_2(string fechaini, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_1_2, fechaini, fechaFin);

            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);


                    int iOsicodi = dr.GetOrdinal(helper.Osicodi);
                    if (!dr.IsDBNull(iOsicodi)) entity.Osicodi = dr.GetString(iOsicodi);

                    int iGrupocomb = dr.GetOrdinal(helper.Grupocomb);
                    if (!dr.IsDBNull(iGrupocomb)) entity.Grupocomb = dr.GetString(iGrupocomb);

                    int iDia = dr.GetOrdinal(helper.Dia);
                    if (!dr.IsDBNull(iDia)) entity.Dia = dr.GetString(iDia);

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }


        public List<EveHoraoperacionDTO> ListaNumerales_DatosBase_5_6_2(string fechaini, string fechaFin)
        {
            string sqlQuery = string.Format(this.helper.SqlDatosBase_5_6_2, fechaini, fechaFin);

            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();

                    int iOsigrupocodi = dr.GetOrdinal(helper.Osigrupocodi);
                    if (!dr.IsDBNull(iOsigrupocodi)) entity.Osigrupocodi = dr.GetString(iOsigrupocodi);


                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGrupocomb = dr.GetOrdinal(helper.Grupocomb);
                    if (!dr.IsDBNull(iGrupocomb)) entity.Grupocomb = dr.GetString(iGrupocomb);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }
        #endregion

        #region Mejoras CMgN

        public List<EveHoraoperacionDTO> ObtenerHorasOperacionCompartivoCM(DateTime fecha)
        {
            List<EveHoraoperacionDTO> entitys = new List<EveHoraoperacionDTO>();

            string sqlQuery = string.Format(this.helper.SqlHorasOperacionComparativoCM, 
                fecha.ToString(ConstantesBase.FormatoFecha), fecha.AddDays(1).ToString(ConstantesBase.FormatoFecha));            
            DbCommand command = dbProvider.GetSqlStringCommand(sqlQuery);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EveHoraoperacionDTO entity = new EveHoraoperacionDTO();                    

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iHophorini = dr.GetOrdinal(helper.Hophorini);
                    if (!dr.IsDBNull(iHophorini)) entity.Hophorini = dr.GetDateTime(iHophorini);

                    int iHophorfin = dr.GetOrdinal(helper.Hophorfin);
                    if (!dr.IsDBNull(iHophorfin)) entity.Hophorfin = dr.GetDateTime(iHophorfin);

                    int iGrupopadre = dr.GetOrdinal(helper.Grupopadre);
                    if (!dr.IsDBNull(iGrupopadre)) entity.Grupopadre = Convert.ToInt32(dr.GetValue(iGrupopadre));

                    entitys.Add(entity);
                }
            }
            return entitys;
        }

        #endregion
    }
}

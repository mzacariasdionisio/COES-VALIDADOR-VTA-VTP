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
    /// Clase de acceso a datos de la tabla IND_RELACION_EMPRESA
    /// </summary>
    public class IndRelacionEmpresaRepository : RepositoryBase, IIndRelacionEmpresaRepository
    {
        public IndRelacionEmpresaRepository(string strConn) : base(strConn)
        {
        }

        IndRelacionEmpresaHelper helper = new IndRelacionEmpresaHelper();

        public int GetMaxId()
        {

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            int id = 1;

            object result = dbProvider.ExecuteScalar(command);
            if (result != null) id = Convert.ToInt32(result);

            return id;
        }
        public int Save(IndRelacionEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Relempcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.Emprcodi);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, entity.Equicodicentral);
            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, entity.Equicodiunidad);
            dbProvider.AddInParameter(command, helper.Grupocodi, DbType.Int32, entity.Grupocodi);
            dbProvider.AddInParameter(command, helper.Famcodi, DbType.Int32, entity.Famcodi);
            dbProvider.AddInParameter(command, helper.Relempunidadnomb, DbType.String, entity.Relempunidadnomb);
            dbProvider.AddInParameter(command, helper.Gaseoductoequicodi, DbType.Int32, entity.Gaseoductoequicodi);
            dbProvider.AddInParameter(command, helper.Grupocodicn2, DbType.Int32, entity.Grupocodicn2);
            dbProvider.AddInParameter(command, helper.Relempcuadro1, DbType.String, entity.Relempcuadro1);
            dbProvider.AddInParameter(command, helper.Relempcuadro2, DbType.String, entity.Relempcuadro2);
            dbProvider.AddInParameter(command, helper.Relempsucad, DbType.String, entity.Relempsucad);
            dbProvider.AddInParameter(command, helper.Relempsugad, DbType.String, entity.Relempsugad);
            dbProvider.AddInParameter(command, helper.Relempestado, DbType.String, entity.Relempestado);
            dbProvider.AddInParameter(command, helper.Relemptecnologia, DbType.Int32, entity.Relemptecnologia);
            dbProvider.AddInParameter(command, helper.Relempusucreacion, DbType.String, entity.Relempusucreacion);
            dbProvider.AddInParameter(command, helper.Relempfeccreacion, DbType.DateTime, entity.Relempfeccreacion);
            dbProvider.AddInParameter(command, helper.Relempusumodificacion, DbType.String, entity.Relempusumodificacion);
            dbProvider.AddInParameter(command, helper.Relempfecmodificacion, DbType.DateTime, entity.Relempfecmodificacion);

            return dbProvider.ExecuteNonQuery(command);
        }

        public void Update(IndRelacionEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);

            dbProvider.AddInParameter(command, helper.Gaseoductoequicodi, DbType.Int32, entity.Gaseoductoequicodi);
            dbProvider.AddInParameter(command, helper.Grupocodicn2, DbType.Int32, entity.Grupocodicn2);
            dbProvider.AddInParameter(command, helper.Relempcuadro1, DbType.String, entity.Relempcuadro1);
            dbProvider.AddInParameter(command, helper.Relempcuadro2, DbType.String, entity.Relempcuadro2);
            dbProvider.AddInParameter(command, helper.Relempsucad, DbType.String, entity.Relempsucad);
            dbProvider.AddInParameter(command, helper.Relempsugad, DbType.String, entity.Relempsugad);
            dbProvider.AddInParameter(command, helper.Relempestado, DbType.String, entity.Relempestado);
            dbProvider.AddInParameter(command, helper.Relemptecnologia, DbType.Int32, entity.Relemptecnologia);
            dbProvider.AddInParameter(command, helper.Relempusumodificacion, DbType.String, entity.Relempusumodificacion);
            dbProvider.AddInParameter(command, helper.Relempfecmodificacion, DbType.DateTime, entity.Relempfecmodificacion);
            dbProvider.AddInParameter(command, helper.Relempcodi, DbType.Int32, entity.Relempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public void Delete(int relempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Relempcodi, DbType.Int32, relempcodi);

            dbProvider.ExecuteNonQuery(command);
        }

        public IndRelacionEmpresaDTO GetById(int relempcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Relempcodi, DbType.Int32, relempcodi);
            IndRelacionEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGaseoductoequinomb = dr.GetOrdinal(helper.Gaseoductoequinomb);
                    if (!dr.IsDBNull(iGaseoductoequinomb)) entity.Gaseoductoequinomb = dr.GetString(iGaseoductoequinomb);

                }
            }

            return entity;
        }

        public List<IndRelacionEmpresaDTO> List()
        {
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlList);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iGaseoductoequinomb = dr.GetOrdinal(helper.Gaseoductoequinomb);
                    if (!dr.IsDBNull(iGaseoductoequinomb)) entity.Gaseoductoequinomb = dr.GetString(iGaseoductoequinomb);

                    int iGruporeservafria = dr.GetOrdinal(helper.Gruporeservafria);
                    if (!dr.IsDBNull(iGruporeservafria)) entity.Gruporeservafria = Convert.ToInt32(dr.GetValue(iGruporeservafria));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListByIdEmpresa(int emprcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListByIdEmpresa);

            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, emprcodi);

            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);

                    //int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    //if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEquinombcentral = dr.GetOrdinal(helper.Equinombcentral);
                    if (!dr.IsDBNull(iEquinombcentral)) entity.Equinombcentral = dr.GetString(iEquinombcentral);

                    int iEquinombunidad = dr.GetOrdinal(helper.Equinombunidad);
                    if (!dr.IsDBNull(iEquinombunidad)) entity.Equinombunidad = dr.GetString(iEquinombunidad);

                    int iGaseoductoequinomb = dr.GetOrdinal(helper.Gaseoductoequinomb);
                    if (!dr.IsDBNull(iGaseoductoequinomb)) entity.Gaseoductoequinomb = dr.GetString(iGaseoductoequinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> GetByIdCentral(int equicodicentral)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdCentral);

            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, equicodicentral);
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<IndRelacionEmpresaDTO> GetByIdUnidad(int equicodiunidad)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdUnidad);

            dbProvider.AddInParameter(command, helper.Equicodiunidad, DbType.Int32, equicodiunidad);
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListCentral()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCentral);

            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodicentral = dr.GetInt32(iEquicodicentral);

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListUnidad(int equicodicentral)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListCentral);
            dbProvider.AddInParameter(command, helper.Equicodicentral, DbType.Int32, equicodicentral);

            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodiunidad = dr.GetInt32(iEquicodiunidad);

                    int iEquinombunidad = dr.GetOrdinal(helper.Relempunidadnomb);
                    if (!dr.IsDBNull(iEquinombunidad)) entity.Relempunidadnomb = dr.GetString(iEquinombunidad);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListGaseoducto()
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListGaseoducto);

            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

                    int iGaseoductoequicodi = dr.GetOrdinal(helper.Gaseoductoequicodi);
                    if (!dr.IsDBNull(iGaseoductoequicodi)) entity.Gaseoductoequicodi = dr.GetInt32(iGaseoductoequicodi);

                    int iGaseoductoequinomb = dr.GetOrdinal(helper.Gaseoductoequinomb);
                    if (!dr.IsDBNull(iGaseoductoequinomb)) entity.Gaseoductoequinomb = dr.GetString(iGaseoductoequinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListEmpresas(string tipoemprcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListEmpresas, tipoemprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<SiEmpresaDTO> ListEmpresasConGaseoducto(string tipoemprcodi)
        {
            List<SiEmpresaDTO> entitys = new List<SiEmpresaDTO>();

            string query = string.Format(helper.SqlListEmpresasConGaseoducto, tipoemprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    SiEmpresaDTO entity = new SiEmpresaDTO();

                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = dr.GetString(iEmprnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListCentrales(string emprcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListCentrales, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListCentralesConGaseoducto(string emprcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListCentralesConGaseoducto, emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodicentral = dr.GetOrdinal(helper.Equicodicentral);
                    if (!dr.IsDBNull(iEquicodicentral)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodicentral));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<EqEquipoDTO> ListUnidades(string emprcodi, string equicodicentral)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            string query = string.Format(helper.SqlListUnidades, emprcodi, equicodicentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodiunidad = dr.GetOrdinal(helper.Equicodiunidad);
                    if (!dr.IsDBNull(iEquicodiunidad)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodiunidad));

                    int iEquinomb = dr.GetOrdinal(helper.Equinomb);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListGrupos(string emprcodi, string equicodicentral, string equicodiunidad)
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            string query = string.Format(helper.SqlListGrupos, emprcodi, equicodicentral, equicodiunidad);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoDTO entity = new PrGrupoDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iRelempunidadnomb = dr.GetOrdinal(helper.Relempunidadnomb);
                    if (!dr.IsDBNull(iRelempunidadnomb)) entity.Gruponomb = dr.GetString(iRelempunidadnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListUnidadNombres(string emprcodi, string equicodicentral)
        {
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            string query = string.Format(helper.SqlListUnidadNombres, emprcodi, equicodicentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

                    int iRelempcodi = dr.GetOrdinal(helper.Relempcodi);
                    if (!dr.IsDBNull(iRelempcodi)) entity.Relempcodi = Convert.ToInt32(dr.GetValue(iRelempcodi));

                    int iRelempunidadnomb = dr.GetOrdinal(helper.Relempunidadnomb);
                    if (!dr.IsDBNull(iRelempunidadnomb)) entity.Relempunidadnomb = dr.GetString(iRelempunidadnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> ListUnidadNombresConGaseoducto(string emprcodi, string equicodicentral)
        {
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            string query = string.Format(helper.SqlListUnidadNombresConGaseoducto, emprcodi, equicodicentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = new IndRelacionEmpresaDTO();

                    int iRelempcodi = dr.GetOrdinal(helper.Relempcodi);
                    if (!dr.IsDBNull(iRelempcodi)) entity.Relempcodi = Convert.ToInt32(dr.GetValue(iRelempcodi));

                    int iRelempunidadnomb = dr.GetOrdinal(helper.Relempunidadnomb);
                    if (!dr.IsDBNull(iRelempunidadnomb)) entity.Relempunidadnomb = dr.GetString(iRelempunidadnomb);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> GetByCriteria(string relempcodi, string emprcodi, string equicodicentral)
        {
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            string query = string.Format(helper.SqlGetByCriteria, relempcodi, emprcodi, equicodicentral);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<IndRelacionEmpresaDTO> GetByCriteria2(string emprcodi, string equicodicentral, string equicodiunidad, string grupocodi, string famcodi)
        {
            List<IndRelacionEmpresaDTO> entitys = new List<IndRelacionEmpresaDTO>();

            string query = string.Format(helper.SqlGetByCriteria2, emprcodi, equicodicentral, equicodiunidad, grupocodi, famcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    IndRelacionEmpresaDTO entity = helper.Create(dr);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoDTO> ListPrGrupoForCN2()
        {
            List<PrGrupoDTO> entitys = new List<PrGrupoDTO>();

            string query = string.Format(helper.SqlListPrGrupoForCN2);
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

        public List<PrGrupodatDTO> ListPrGrupodatByCriteria(string grupocodi, string concepcodi, DateTime fechadat)
        {
            List<PrGrupodatDTO> entitys = new List<PrGrupodatDTO>();

            string query = string.Format(helper.SqlListPrGrupodatByCriteria, grupocodi, concepcodi, fechadat.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(query);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupodatDTO entity = new PrGrupodatDTO();

                    int iConcepdesc = dr.GetOrdinal(this.helper.Concepdesc);
                    if (!dr.IsDBNull(iConcepdesc)) entity.ConcepDesc = dr.GetString(iConcepdesc);

                    int iConcepabrev = dr.GetOrdinal(this.helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConcepunid = dr.GetOrdinal(this.helper.Concepunid);
                    if (!dr.IsDBNull(iConcepunid)) entity.ConcepUni = dr.GetString(iConcepunid);

                    int iConcepcodi = dr.GetOrdinal(this.helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = dr.GetInt32(iConcepcodi);

                    int iFormulaDat = dr.GetOrdinal(helper.Formuladat);
                    if (!dr.IsDBNull(iFormulaDat)) entity.Formuladat = dr.GetString(iFormulaDat);

                    int iFechaDat = dr.GetOrdinal(helper.Fechadat);
                    if (!dr.IsDBNull(iFechaDat)) entity.Fechadat = dr.GetDateTime(iFechaDat);

                    int iCatecodi = dr.GetOrdinal(helper.Catecodi);
                    if (!dr.IsDBNull(iCatecodi)) entity.Catecodi = dr.GetInt32(iCatecodi);

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = dr.GetInt32(iGrupocodi);

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Tipocombustible = dr.GetString(iValor);

                    int iLastuser = dr.GetOrdinal(helper.Lastuser);
                    if (!dr.IsDBNull(iLastuser)) entity.Lastuser = dr.GetString(iLastuser);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<PrGrupoEquipoValDTO> ListPrGrupoEquipoValByCriteria(string concepcodi, string equicodi, string grupocodi, DateTime greqvafechadat)
        {
            List<PrGrupoEquipoValDTO> entitys = new List<PrGrupoEquipoValDTO>();
            string sql = string.Format(helper.SqlListPrGrupoEquipoValByCriteria, concepcodi, equicodi, grupocodi, greqvafechadat.ToString(ConstantesBase.FormatoFecha));
            DbCommand command = dbProvider.GetSqlStringCommand(sql);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    PrGrupoEquipoValDTO entity = new PrGrupoEquipoValDTO();

                    int iGrupocodi = dr.GetOrdinal(helper.Grupocodi);
                    if (!dr.IsDBNull(iGrupocodi)) entity.Grupocodi = Convert.ToInt32(dr.GetValue(iGrupocodi));

                    int iConcepcodi = dr.GetOrdinal(helper.Concepcodi);
                    if (!dr.IsDBNull(iConcepcodi)) entity.Concepcodi = Convert.ToInt32(dr.GetValue(iConcepcodi));

                    int iEquicodi = dr.GetOrdinal(helper.Equicodi);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iGreqvafechadat = dr.GetOrdinal(helper.Greqvafechadat);
                    if (!dr.IsDBNull(iGreqvafechadat)) entity.Greqvafechadat = dr.GetDateTime(iGreqvafechadat);

                    int iGreqvaformuladat = dr.GetOrdinal(helper.Greqvaformuladat);
                    if (!dr.IsDBNull(iGreqvaformuladat)) entity.Greqvaformuladat = dr.GetString(iGreqvaformuladat);

                    int iGreqvadeleted = dr.GetOrdinal(helper.Greqvadeleted);
                    if (!dr.IsDBNull(iGreqvadeleted)) entity.Greqvadeleted = Convert.ToInt32(dr.GetValue(iGreqvadeleted));

                    int iGreqvausucreacion = dr.GetOrdinal(helper.Greqvausucreacion);
                    if (!dr.IsDBNull(iGreqvausucreacion)) entity.Greqvausucreacion = dr.GetString(iGreqvausucreacion);

                    int iGreqvafeccreacion = dr.GetOrdinal(helper.Greqvafeccreacion);
                    if (!dr.IsDBNull(iGreqvafeccreacion)) entity.Greqvafeccreacion = dr.GetDateTime(iGreqvafeccreacion);

                    int iGreqvausumodificacion = dr.GetOrdinal(helper.Greqvausumodificacion);
                    if (!dr.IsDBNull(iGreqvausumodificacion)) entity.Greqvausumodificacion = dr.GetString(iGreqvausumodificacion);

                    int iGreqvafecmodificacion = dr.GetOrdinal(helper.Greqvafecmodificacion);
                    if (!dr.IsDBNull(iGreqvafecmodificacion)) entity.Greqvafecmodificacion = dr.GetDateTime(iGreqvafecmodificacion);

                    int iGreqvacomentario = dr.GetOrdinal(helper.Greqvacomentario);
                    if (!dr.IsDBNull(iGreqvacomentario)) entity.Greqvacomentario = dr.GetString(iGreqvacomentario);

                    int iGreqvasustento = dr.GetOrdinal(helper.Greqvasustento);
                    if (!dr.IsDBNull(iGreqvasustento)) entity.Greqvasustento = dr.GetString(iGreqvasustento);

                    int iGreqvacheckcero = dr.GetOrdinal(helper.Greqvacheckcero);
                    if (!dr.IsDBNull(iGreqvacheckcero)) entity.Greqvacheckcero = Convert.ToInt32(dr.GetValue(iGreqvacheckcero));

                    int iConcepabrev = dr.GetOrdinal(helper.Concepabrev);
                    if (!dr.IsDBNull(iConcepabrev)) entity.Concepabrev = dr.GetString(iConcepabrev);

                    int iConceppadre = dr.GetOrdinal(helper.Conceppadre);
                    if (!dr.IsDBNull(iConceppadre)) entity.Conceppadre = Convert.ToInt32(dr.GetValue(iConceppadre));

                    int iValor = dr.GetOrdinal(helper.Valor);
                    if (!dr.IsDBNull(iValor)) entity.Tipocombustible = dr.GetString(iValor);

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

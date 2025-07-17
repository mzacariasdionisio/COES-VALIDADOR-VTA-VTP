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
    public class GmmAgenteRepository : RepositoryBase, IGmmAgenteRepository
    {
        public GmmAgenteRepository(string strConn)
            : base(strConn)
        {

        }

        GmmAgenteHelper helper = new GmmAgenteHelper();

        public int Save(GmmEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetMaxId);
            object result = dbProvider.ExecuteScalar(command);
            int id = 1;
            if (result != null) id = Convert.ToInt32(result);

            command = dbProvider.GetSqlStringCommand(helper.SqlSave);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, id);
            dbProvider.AddInParameter(command, helper.Empgfecingreso, DbType.String,Convert.ToDateTime(entity.EMPGFECINGRESO).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Empgusucreacion, DbType.String, entity.EMPGUSUCREACION);
            dbProvider.AddInParameter(command, helper.Empgtipopart, DbType.String, entity.EMPGTIPOPART); // > 0 ? entity.Equicodi : (object)DBNull.Value);
            dbProvider.AddInParameter(command, helper.Empgestado, DbType.String, entity.EMPGESTADO);
            dbProvider.AddInParameter(command, helper.Empgcomentario, DbType.String, entity.EMPGCOMENTARIO);
            dbProvider.AddInParameter(command, helper.Emprcodi, DbType.Int32, entity.EMPRCODI);

            var iRslt = dbProvider.ExecuteNonQuery(command);
            return id;
        }

        public void Update(GmmEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUpdate);
            int id = entity.EMPGCODI;
            dbProvider.AddInParameter(command, helper.Empgfecingreso, DbType.String,Convert.ToDateTime(entity.EMPGFECINGRESO).ToString("yyyy-MM-dd"));
            dbProvider.AddInParameter(command, helper.Empgtipopart, DbType.String, entity.EMPGTIPOPART); 
            dbProvider.AddInParameter(command, helper.Empgestado, DbType.String, entity.EMPGESTADO);
            dbProvider.AddInParameter(command, helper.Empgcomentario, DbType.String, entity.EMPGCOMENTARIO);
            dbProvider.AddInParameter(command, helper.Empgusumodificacion, DbType.String, entity.EMPGUSUMODIFICACION);
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, id);
            
            dbProvider.ExecuteNonQuery(command);
        }

        public bool Delete(GmmEmpresaDTO entity)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlDelete);

            dbProvider.AddInParameter(command, helper.Empgusumodificacion, DbType.String, entity.EMPGUSUMODIFICACION);
            //dbProvider.AddInParameter(command, helper.Empgfecmodificacion, DbType.String, Convert.ToDateTime(entity.EMPGFECMODIFICACION).ToString("yyyy-MM-dd"));       
            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, entity.EMPGCODI);
            int numberOfRecords = dbProvider.ExecuteNonQuery(command);

            if (numberOfRecords > 0)
                return true;
            else
                return false;
        }

        public List<GmmEmpresaDTO> ListarFiltroAgentes(string razonSocial, string documento, string tipoParticipante, string tipoModalidad, string fecIni, string fecFin, string estado, bool dosMasIncumplimientos)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string queryString = string.Format(helper.SqlListarFiltroAgentes,
                razonSocial, documento, tipoParticipante == "0" ? "-" : tipoParticipante, string.IsNullOrEmpty(estado) ? "-" : estado, 
                tipoModalidad == "0" ? "-" : tipoModalidad,
                string.IsNullOrEmpty(fecIni) ? "-" : fecIni, string.IsNullOrEmpty(fecFin) ? "-" : fecFin, dosMasIncumplimientos ? 2 : -1);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaAgentes(dr));
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarModalidades(int empgcodi)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string queryString = string.Format(helper.SqlListarModalidades, empgcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaModalidades(dr));
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarEstados(int empgcodi)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string queryString = string.Format(helper.SqlListarEstados, empgcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaEstados(dr));
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarIncumplimientos(int empgcodi)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string queryString = string.Format(helper.SqlListarIncumplimientos, empgcodi);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaIncumplimientos(dr));
                }
            }
            return entities;
        }

        public GmmEmpresaDTO GetById(int empgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, empgcodi);
            GmmEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public GmmEmpresaDTO GetByIdEdit(int empgcodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByIdEdit);

            dbProvider.AddInParameter(command, helper.Empgcodi, DbType.Int32, empgcodi);
            GmmEmpresaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.SqlGetByIdEditDTO(dr);
                }
            }

            return entity;
        }

        public List<GmmEmpresaDTO> ListarAgentes(string razonsocial)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string queryString = string.Format(helper.SqlListarAgentes, razonsocial);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateListaAgentesSelect(dr));
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarAgentesParaCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string sPeriodo = anio.ToString();
            if (mes < 10) sPeriodo += "0" + mes.ToString();
            else
                sPeriodo += mes.ToString();
            string queryString = string.Format(helper.SqlListarAgentesParaCalculo, anio, mes, sPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmEmpresaDTO gmmEmpresa = helper.CreateListaParaCalculo(dr);
                    gmmEmpresa.gmmValEnergiaDTOs = ListarCabeceraValoresEnergia(formatotrimestre, formatomes, gmmEmpresa.EmprcodiCal, tipoEnergia);
                    entities.Add(gmmEmpresa);
                    
                }
            }
            return entities;
        }

        

        public List<GmmEmpresaDTO> ListarAgentesEntregaParaCalculo(int anio, int mes, int formatotrimestre, int formatomes, int tipoEnergia)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string sPeriodo = anio.ToString();
            if (mes < 10) sPeriodo += "0" + mes.ToString();
            else
                sPeriodo += mes.ToString();
            string queryString = string.Format(helper.SqlListarAgentesEntregaParaCalculo, anio, mes, sPeriodo);

            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmEmpresaDTO gmmEmpresa = helper.CreateListaParaCalculo(dr);
                    gmmEmpresa.gmmValEnergiaEntregaDTOs = ListarCabeceraValoresEnergiaEntrega(formatotrimestre, formatomes, gmmEmpresa.EmprcodiCal, tipoEnergia);
                    entities.Add(gmmEmpresa);

                }
            }
            return entities;
        }

        private List<GmmValEnergiaDTO> ListarCabeceraValoresEnergia(int formatotrimestre, int formatomes, int emprcodi, int tipoEnergia)
        {
            List<GmmValEnergiaDTO> entities = new List<GmmValEnergiaDTO>();
            string queryString = string.Format(helper.SqlListarCabeceraValoresEnergia, formatotrimestre, formatomes, emprcodi, tipoEnergia);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateCabeceraValoresEnergia(dr));
                }
            }
            return entities;
        }
        private List<GmmValEnergiaEntregaDTO> ListarCabeceraValoresEnergiaEntrega(int formatotrimestre, int formatomes, int emprcodi, int tipoEnergia)
        {
            List<GmmValEnergiaEntregaDTO> entities = new List<GmmValEnergiaEntregaDTO>();
            string queryString = string.Format(helper.SqlListarCabeceraValoresEnergia, formatotrimestre, formatomes, emprcodi, tipoEnergia);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entities.Add(helper.CreateCabeceraValoresEnergiaEntrega(dr));
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarMaestroEmpresas(string razonsocial, string estadoRegistro)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string condicion = "";

            if (!string.IsNullOrEmpty(razonsocial))
            {
                condicion = condicion + " AND NVL(EMPRRAZSOCIAL, EMPRNOMB) LIKE '%" + razonsocial.ToUpper() + "%' ";
            }
            string queryString = string.Format(helper.SqlListarMaestroEmpresas, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmEmpresaDTO entity = new GmmEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);
                    int iEmprRuc = dr.GetOrdinal(helper.EmprRuc);
                    if (!dr.IsDBNull(iEmprRuc)) entity.EmprRuc = dr.GetString(iEmprRuc);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarMaestroEmpresasCliente(string razonsocial, string estadoRegistro)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string condicion = "";

            if (!string.IsNullOrEmpty(razonsocial))
            {
                condicion = condicion + " AND NVL(EMPRRAZSOCIAL, EMPRNOMB) LIKE '%" + razonsocial.ToUpper() + "%' ";
            }
            string queryString = string.Format(helper.SqlListarMaestroEmpresasCliente, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmEmpresaDTO entity = new GmmEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);
                    int iEmprRuc = dr.GetOrdinal(helper.EmprRuc);
                    if (!dr.IsDBNull(iEmprRuc)) entity.EmprRuc = dr.GetString(iEmprRuc);
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public List<GmmEmpresaDTO> ListarEmpresasParticipantes(string razonsocial, string estadoRegistro)
        {
            List<GmmEmpresaDTO> entities = new List<GmmEmpresaDTO>();
            string condicion = "";

            if (!string.IsNullOrEmpty(razonsocial))
            {
                condicion = condicion + " AND NVL(EMPRRAZSOCIAL, EMPRNOMB) LIKE '%" + razonsocial.ToUpper() + "%' ";
            }
            string queryString = string.Format(helper.SqlListarEmpresasParticipantes, estadoRegistro, condicion);
            DbCommand command = dbProvider.GetSqlStringCommand(queryString);

            //dbProvider.AddInParameter(command, helper.Emprestado, DbType.String, estadoRegistro);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    GmmEmpresaDTO entity = new GmmEmpresaDTO();
                    int iEmprcodi = dr.GetOrdinal(helper.Emprcodi);
                    if (!dr.IsDBNull(iEmprcodi)) entity.EMPRCODI = Convert.ToInt32(dr.GetValue(iEmprcodi));
                    int iEmpgcodi = dr.GetOrdinal(helper.Empgcodi);
                    if (!dr.IsDBNull(iEmpgcodi)) entity.EMPGCODI = Convert.ToInt32(dr.GetValue(iEmpgcodi));
                    int iEmprrazsocial = dr.GetOrdinal(helper.Emprrazsocial);
                    if (!dr.IsDBNull(iEmprrazsocial)) entity.Emprrazsocial = dr.GetString(iEmprrazsocial);
                    int iEmprRuc = dr.GetOrdinal(helper.EmprRuc);
                    if (!dr.IsDBNull(iEmprRuc)) entity.EmprRuc = dr.GetString(iEmprRuc);
                    entities.Add(entity);
                }
            }
            return entities;
        }


        #region Mantenimiento Garantia - Modalidad
        public GmmGarantiaDTO ObtieneGarantiaById(int garacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetGarantiaById);

            dbProvider.AddInParameter(command, helper.GaraCodi, DbType.Int32, garacodi);
            GmmGarantiaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.SqlGetGarantiaByIdEditarDTO(dr);
                }
            }

            return entity;
        }
        #endregion
    }
}

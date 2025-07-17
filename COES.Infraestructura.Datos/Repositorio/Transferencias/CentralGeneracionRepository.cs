using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
    public class CentralGeneracionRepository : RepositoryBase, ICentralGeneracionRepository
    {
        public CentralGeneracionRepository(string strConn)
            : base(strConn)
        {
        }

        CentralGeneracionHelper helper = new CentralGeneracionHelper();

        public List<CentralGeneracionDTO> List()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
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

        public List<CentralGeneracionDTO> Unidad()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUnidad);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CentralGeneracionDTO> UnidadCentral(int equicodicen)
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlUnidadCentral);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, equicodicen);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    CentralGeneracionDTO entity = helper.Create(dr);
                    int iFAMCODI = dr.GetOrdinal(this.helper.FAMCODI);
                    if (!dr.IsDBNull(iFAMCODI)) entity.FamCodi = dr.GetInt32(iFAMCODI);
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

        public List<CentralGeneracionDTO> ListInfoBase()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListInfoBase);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CentralGeneracionDTO> ListaInterCodEnt()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodEnt);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #region PrimasRER.2023
        public List<CentralGeneracionDTO> ListaCentralByEmpresa(int emprcodi)
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCentralByEmpresa);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<CentralGeneracionDTO> ListaCentralUnidadByEmpresa(int emprcodi)
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaCentralUnidadByEmpresa);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        #endregion
        public List<CentralGeneracionDTO> ListaInterCodInfoBase()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListaInterCodInfoBase);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public CentralGeneracionDTO GetByCentGeneNombre(string sCentGeneNombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneNombre);

            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, sCentGeneNombre);
            CentralGeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public List<CentralGeneracionDTO> ListEmpresaCentralGeneracion()
        {
            List<CentralGeneracionDTO> entitys = new List<CentralGeneracionDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListEmpresaCentralGeneracion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }
        
        public CentralGeneracionDTO GetByCentGeneNombVsEN(string sCentGeneNombre, int strecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneNombVsEN);

            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, sCentGeneNombre);
            dbProvider.AddInParameter(command, helper.CENTGENECODI, DbType.Int32, strecacodi);
            CentralGeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public CentralGeneracionDTO GetByCentGeneTermoelectricaNombre(string sCentGeneNombre)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneTermoelectricaNombre);

            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, sCentGeneNombre);
            CentralGeneracionDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                }
            }

            return entity;
        }

        public EqEquipoDTO GetByCentGeneNombreEquipo(string equinomb, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneNombreEquipo);

            dbProvider.AddInParameter(command, helper.EQUINOMB, DbType.String, equinomb);
            dbProvider.AddInParameter(command, helper.VCRECACODI, DbType.Int32, vcrecacodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateEquipo(dr);
                }
            }

            return entity;
        }

        public EqEquipoDTO GetByCentGeneNombreEquipoCenUni(string equinomb, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneNombreEquipoCenUni);

            dbProvider.AddInParameter(command, helper.EQUINOMB, DbType.String, equinomb);
            dbProvider.AddInParameter(command, helper.VCRECACODI, DbType.Int32, vcrecacodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateEquipo(dr);
                }
            }

            return entity;
        }

        public EqEquipoDTO GetByCentGeneUniNombreEquipo(string sCentGeneNombre, int equicodi, int vcrecacodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCentGeneUniNombreEquipo);

            dbProvider.AddInParameter(command, helper.CENTGENENOMBRE, DbType.String, sCentGeneNombre);
            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, equicodi);
            dbProvider.AddInParameter(command, helper.VCRECACODI, DbType.Int32, vcrecacodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateEquipo(dr);
                }
            }

            return entity;
        }

        public EqEquipoDTO GetByEquicodi(int equicodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEquicodi);

            dbProvider.AddInParameter(command, helper.EQUICODI, DbType.Int32, equicodi);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateEquipo(dr);
                }
            }

            return entity;
        }
        
        public EqEquipoDTO GetByEquiNomb(string equinomb)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByEquiNomb);
            dbProvider.AddInParameter(command, helper.EQUINOMB, DbType.String, equinomb);
            EqEquipoDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.CreateEquipo(dr);
                }
            }

            return entity;
        }

        #region SIOSEIN-PRIE-2021
        public List<EqEquipoDTO> ListarEquiposPorEmpresa(int emprcodi)
        {
            List<EqEquipoDTO> entitys = new List<EqEquipoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlListarEquiposPorEmpresa);
            dbProvider.AddInParameter(command, helper.EMPRCODI, DbType.Int32, emprcodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    EqEquipoDTO entity = new EqEquipoDTO();

                    int iEquicodi = dr.GetOrdinal(helper.EQUICODI);
                    if (!dr.IsDBNull(iEquicodi)) entity.Equicodi = Convert.ToInt32(dr.GetValue(iEquicodi));

                    int iEquinomb = dr.GetOrdinal(helper.EQUINOMB);
                    if (!dr.IsDBNull(iEquinomb)) entity.Equinomb = dr.GetString(iEquinomb);

                    int iEmprcodi = dr.GetOrdinal(helper.EMPRCODI);
                    if (!dr.IsDBNull(iEmprcodi)) entity.Emprcodi = Convert.ToInt32(dr.GetValue(iEmprcodi));

                    int iEquipadre = dr.GetOrdinal(helper.EQUIPADRE);
                    if (!dr.IsDBNull(iEquipadre)) entity.Equipadre = Convert.ToInt32(dr.GetValue(iEquipadre));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        #endregion

    }
}

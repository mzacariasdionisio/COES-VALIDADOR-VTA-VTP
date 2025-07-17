using COES.Base.Core;
using COES.Dominio.DTO.Sic;
using COES.Dominio.Interfaces.Sic;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper.Sic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace COES.Infraestructura.Datos.Repositorio.Sic
{
    /// <summary>
    /// Clase de acceso a datos de la tabla EPR_REP_LIMIT_CAP
    /// </summary>
    public class EprRepLimitCapRepository : RepositoryBase, IEprRepLimitCapRepository
    {
        public EprRepLimitCapRepository(string strConn) : base(strConn)
        {
        }

        EprRepLimitCapHelper helper = new EprRepLimitCapHelper();

        public List<EprRepLimitCapDTO> ListCapacidadTransmision(int idAreaExcel)
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCapacidadTransmision);
            dbProvider.AddInParameter(command, helper.IdAreaExcel, DbType.Int32, idAreaExcel);
            dbProvider.AddInParameter(command, helper.IdAreaExcel, DbType.Int32, idAreaExcel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<EprRepLimitCapDTO> ListaEmpresaSigla()
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListaEmpresaSigla);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateEpresaSigla(dr));
                }
            }

            return entitys;
        }

        public List<EprRepLimitCapDTO> ListCapacidadTransformador(int idAreaExcel)
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCapacidadTransformador);
            dbProvider.AddInParameter(command, helper.IdAreaExcel, DbType.Int32, idAreaExcel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateTransformador(dr));
                }
            }

            return entitys;
        }

        public List<EprRepLimitCapDTO> ListCapacidadAcoplaminento(int idAreaExcel, int tension)
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListCapacidadAcoplaminento);
            dbProvider.AddInParameter(command, helper.IdAreaExcel, DbType.Int32, idAreaExcel);
            dbProvider.AddInParameter(command, helper.IdAreaExcel, DbType.Int32, idAreaExcel);
            dbProvider.AddInParameter(command, helper.Tension, DbType.Int32, tension);
            dbProvider.AddInParameter(command, helper.Tension, DbType.Int32, tension);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.CreateAcoplamiento(dr));
                }
            }

            return entitys;
        }
        

        public List<EprRepLimitCapDTO> ListActualizaciones()
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListActualizaciones);
            EprRepLimitCapDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprRepLimitCapDTO();
                    int iEpProyNomb = dr.GetOrdinal(helper.EpProyNomb);
                    if (!dr.IsDBNull(iEpProyNomb)) entity.EpProyNomb = Convert.ToString(dr.GetValue(iEpProyNomb));

                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<EprRepLimitCapDTO> ListRevisiones()
        {
            List<EprRepLimitCapDTO> entitys = new List<EprRepLimitCapDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(helper.ListRevisiones);
            EprRepLimitCapDTO entity;
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = new EprRepLimitCapDTO();
                    int iEprtlcfecemision = dr.GetOrdinal(helper.Eprtlcfecemision);
                    if (!dr.IsDBNull(iEprtlcfecemision)) entity.Eprtlcfecemision = Convert.ToString(dr.GetValue(iEprtlcfecemision));
                    int iEprtlcrevision = dr.GetOrdinal(helper.Eprtlcrevision);
                    if (!dr.IsDBNull(iEprtlcrevision)) entity.Eprtlcrevision = Convert.ToString(dr.GetValue(iEprtlcrevision));
                    int iEprtlcdescripcion = dr.GetOrdinal(helper.Eprtlcdescripcion);
                    if (!dr.IsDBNull(iEprtlcdescripcion)) entity.Eprtlcdescripcion = Convert.ToString(dr.GetValue(iEprtlcdescripcion));
                    int iEprtlcusuelabora = dr.GetOrdinal(helper.Eprtlcusuelabora);
                    if (!dr.IsDBNull(iEprtlcusuelabora)) entity.Eprtlcusuelabora = Convert.ToString(dr.GetValue(iEprtlcusuelabora));
                    int iEprtlcusurevisa = dr.GetOrdinal(helper.Eprtlcusurevisa);
                    if (!dr.IsDBNull(iEprtlcusurevisa)) entity.Eprtlcusurevisa = Convert.ToString(dr.GetValue(iEprtlcusurevisa));
                    int iEprtlcusuaprueba = dr.GetOrdinal(helper.Eprtlcusuaprueba);
                    if (!dr.IsDBNull(iEprtlcusuaprueba)) entity.Eprtlcusuaprueba = Convert.ToString(dr.GetValue(iEprtlcusuaprueba));
                   
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

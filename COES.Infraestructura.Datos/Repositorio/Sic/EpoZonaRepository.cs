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
    public class EpoZonaRepository:RepositoryBase, IEpoZonaRepository
    {
        public EpoZonaRepository(string strConn) : base(strConn)
        {
        }

        EpoZonaHelper helper = new EpoZonaHelper();

        public List<EpoZonaDTO> List()
        {
            List<EpoZonaDTO> entitys = new List<EpoZonaDTO>();
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


        public EpoZonaDTO GetById(int ZonCodi)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetById);

            dbProvider.AddInParameter(command, helper.ZonCod, DbType.Int32, ZonCodi);
            EpoZonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iZonCodi = dr.GetOrdinal(helper.ZonCod);
                    if (!dr.IsDBNull(iZonCodi)) entity.Zoncodi = Convert.ToInt32(dr.GetValue(iZonCodi));

                    int iZonDescrip = dr.GetOrdinal(helper.ZonaDescripcion);
                    if (!dr.IsDBNull(iZonDescrip)) entity.ZonDescripcion = dr.GetString(iZonDescrip);
                }
            }

            return entity;
        }
        public EpoZonaDTO GetByCriteria(int PuntCode)
        {
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PuntCodi, DbType.Int32, PuntCode);
            EpoZonaDTO entity = null;

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                if (dr.Read())
                {
                    entity = helper.Create(dr);
                    int iZonCodi = dr.GetOrdinal(helper.ZonCod);
                    if (!dr.IsDBNull(iZonCodi)) entity.Zoncodi = Convert.ToInt32(dr.GetValue(iZonCodi));

                    int iZonDescrip = dr.GetOrdinal(helper.ZonaDescripcion);
                    if (!dr.IsDBNull(iZonDescrip)) entity.ZonDescripcion = dr.GetString(iZonDescrip);
                }
            }

            return entity;
        }

        


    }
}

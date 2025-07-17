using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Framework.Base.Tools;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamCatalogoRepository : RepositoryBase, ICamCatalogoRepository

    {
        public CamCatalogoRepository(string strConn) : base(strConn) { }

        CamCatalogoHelper Helper = new CamCatalogoHelper();

        public List<CatalogoDTO> GetCatalogoXdesc(string descortaCat)
        {
            List<CatalogoDTO> entitys = new List<CatalogoDTO>();

            using (DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetCatalogoXDesc))
            {
                dbProvider.AddInParameter(command, "DESCORTACAT", DbType.String, descortaCat);

                using (IDataReader dr = dbProvider.ExecuteReader(command))
                {
                    while (dr.Read())
                    {
                        entitys.Add(new CatalogoDTO
                        {
                            CatCodi = Convert.ToInt32(dr["CATCODI"]),
                            DesCat = dr["DESCAT"].ToString(),
                            DesCortaCat = dr["DESCORTACAT"].ToString()
                        });
                    }
                }
            }
            return entitys;
        }

        public List<DataCatalogoDTO> GetCatalogoParametria(int catCodi)
        {
            throw new NotImplementedException();
        }
    }
}

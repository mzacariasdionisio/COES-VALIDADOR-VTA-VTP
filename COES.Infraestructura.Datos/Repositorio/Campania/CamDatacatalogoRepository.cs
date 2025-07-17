using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Framework.Base.Tools;
using COES.Infraestructura.Datos.Helper;
using COES.Infraestructura.Datos.Helper.Campania;
using COES.Infraestructura.Datos.Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamDatacatalogoRepository : RepositoryBase, ICamDatacatalogoRepository
    {

        public CamDatacatalogoRepository(string strConn) : base (strConn) { }

        CamDatacatalogoHelper Helper = new CamDatacatalogoHelper();

        public List<DataCatalogoDTO> GetParametria(int catcodi)
        {
            List<DataCatalogoDTO> entitys = new List<DataCatalogoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetParametria);
            dbProvider.AddInParameter(command, "CATCODI", DbType.Int32, catcodi);
            dbProvider.AddInParameter(command, "INDDEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DataCatalogoDTO ob = new DataCatalogoDTO();
                    ob.CatCodi = Int32.Parse(dr["CATCODI"].ToString());
                    ob.DataCatCodi = Int32.Parse(dr["DATACATCODI"].ToString());
                    ob.DesDataCat = dr["DESDATACAT"].ToString();
                    ob.DescortaDatacat = dr["DESCORTADATACAT"].ToString();
                    ob.Valor = dr["VALOR"].ToString();
                    entitys.Add(ob);
                }
            }
            return entitys;
        }
        public List<DataSubestacionDTO> GetParamSubestacion()
        {
            List<DataSubestacionDTO> entitys = new List<DataSubestacionDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetParamSubestacion);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DataSubestacionDTO ob = new DataSubestacionDTO();
                    ob.Equicodi = Int32.Parse(dr["ZONACODI"].ToString());
                    ob.Equinomb = dr["DESCRIP"].ToString();
                    entitys.Add(ob);
                }
            }
            return entitys;
        }
         public List<DataCatalogoDTO> GetParametriaAll()
        {
            List<DataCatalogoDTO> entitys = new List<DataCatalogoDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetParametriaAll);
            dbProvider.AddInParameter(command, "INDDEL", DbType.String, Constantes.IndDel);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DataCatalogoDTO ob = new DataCatalogoDTO();
                    ob.CatCodi = Int32.Parse(dr["CATCODI"].ToString());
                    ob.DataCatCodi = Int32.Parse(dr["DATACATCODI"].ToString());
                    ob.DesDataCat = dr["DESDATACAT"].ToString();
                    ob.DescortaDatacat = dr["DESCORTADATACAT"].ToString();
                    ob.Valor = dr["VALOR"].ToString();
                    entitys.Add(ob);
                }
            }
            return entitys;
        }
    }
}

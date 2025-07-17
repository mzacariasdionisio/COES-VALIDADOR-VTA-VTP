using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
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
    public class CamProvinciaRepository : RepositoryBase, ICamProvinciaRepository
    {
        public CamProvinciaRepository(string strConn) : base(strConn)
        { }

        CamProvinciaHelper Helper = new CamProvinciaHelper();

        public List<ProvinciaDTO> GetListProvByDepId(string IdDepartamento)
        {
            List<ProvinciaDTO> provinciaDTOs = new List<ProvinciaDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetListProvByDepId);
            dbProvider.AddInParameter(command, "DEPARTAMENTO_ID", DbType.String, IdDepartamento);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    ProvinciaDTO ob = new ProvinciaDTO();
                    ob.Id = dr["ID"].ToString();
                    ob.Nombre = dr["NOMBRE"].ToString();
                    ob.DepartamentoId = dr["DEPARTAMENTO_ID"].ToString();
                    provinciaDTOs.Add(ob);
                }
            }
            return provinciaDTOs;
        }


    }
}

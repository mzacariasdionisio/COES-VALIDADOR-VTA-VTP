using COES.Base.Core;
using COES.Dominio.DTO.Campania;
using COES.Dominio.Interfaces.Campania;
using COES.Infraestructura.Datos.Helper.Campania;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Infraestructura.Datos.Repositorio.Campania
{
    public class CamDistritoRepository: RepositoryBase, ICamDistritoRepository
    {

        public CamDistritoRepository(string strConn): base(strConn)
        {
        }

        CamDistritoHelper Helper = new CamDistritoHelper();

        public List<DistritoDTO> GetListDistByProvDepId(string IdProvincia)
        {
            List<DistritoDTO> distritoDTOs = new List<DistritoDTO>();
            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlGetListDistByProvDepId);
            dbProvider.AddInParameter(command, "PROVINCIAID", DbType.String, IdProvincia);
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    DistritoDTO ob = new DistritoDTO();
                    ob.Id = dr["ID"].ToString();
                    ob.Nombre = dr["NOMBRE"].ToString();
                    ob.DepartamentoId = dr["DEPARTAMENTO_ID"].ToString();
                    ob.ProvinciaId = dr["PROVINCIA_ID"].ToString();
                    distritoDTOs.Add(ob);
                }
            }
            return distritoDTOs;


        }
        public UbicacionDTO GetUbicacionByProvDepId(string id)
        {
            Console.WriteLine("ID Distrito recibido: " + id);
            UbicacionDTO ubicacionDTO = new UbicacionDTO();

            DbCommand command = dbProvider.GetSqlStringCommand(Helper.SqlDepProvDistByDistritotId);
            dbProvider.AddInParameter(command, "DISTRITOID", DbType.String, id);
           
            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                Console.WriteLine("Distrito ID: " + ubicacionDTO.DistritoId);
                if (dr.Read()) // Verifica si hay resultados
                {
                    ubicacionDTO = new UbicacionDTO
                    {
                        // Asignación de campos de acuerdo a las tablas
                        DistritoId = dr["DISTRITOID"].ToString(),
                        Distrito = dr["DISTRITONOMBRE"].ToString(),
                        ProvinciaId = dr["PROVINCIAID"].ToString(),
                        Provincia = dr["PROVINCIANOMBRE"].ToString(),
                        DepartamentoId = dr["DEPARTAMENTOID"].ToString(),
                        Departamento = dr["DEPARTAMENTONOMBRE"].ToString()
                    };
                }
            }

            return ubicacionDTO; // Devuelve los datos obtenidos en el DTO
        }



    }
}

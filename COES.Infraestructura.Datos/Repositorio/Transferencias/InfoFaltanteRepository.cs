using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Infraestructura.Datos.Helper.Transferencias;
using System.Data;
using System.Data.Common;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase que contiene las operaciones con la base de datos
    /// </summary>
   public class InfoFaltanteRepository: RepositoryBase, IInfoFaltanteRepository
    {
       public InfoFaltanteRepository(string strConn)
            : base(strConn)
        {
        }

       InfoFaltanteHelper helper = new InfoFaltanteHelper();

        public List<InfoFaltanteDTO> GetByCriteria(Int32 PeriCodi)
        {
            List<InfoFaltanteDTO> entitys = new List<InfoFaltanteDTO>();
            
            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByCriteria);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, PeriCodi);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entitys.Add(helper.Create(dr));
                }
            }

            return entitys;
        }

        public List<InfoFaltanteDTO> ListByPeriodoVersion(int iPericodi, int iVersion)
        {
            List<InfoFaltanteDTO> entitys = new List<InfoFaltanteDTO>();

            DbCommand command = dbProvider.GetSqlStringCommand(helper.SqlGetByListaPeriodoVersion);

            dbProvider.AddInParameter(command, helper.PeriCodi, DbType.Int32, iPericodi);
            dbProvider.AddInParameter(command, helper.Version, DbType.Int32, iVersion);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {

                    InfoFaltanteDTO entity = new InfoFaltanteDTO();

                    int iEmpresa = dr.GetOrdinal(helper.Empresa);
                    if (!dr.IsDBNull(iEmpresa)) entity.Empresa = dr.GetString(iEmpresa);

                    int iBarra = dr.GetOrdinal(helper.Barra);
                    if (!dr.IsDBNull(iBarra)) entity.Barra = dr.GetString(iBarra);

                    int iCliente = dr.GetOrdinal(helper.Cliente);
                    if (!dr.IsDBNull(iCliente)) entity.Cliente = dr.GetString(iCliente);

                    int iCodigo = dr.GetOrdinal(helper.Codigo);
                    if (!dr.IsDBNull(iCodigo)) entity.Codigo = dr.GetString(iCodigo);

                    int iEmprCodi = dr.GetOrdinal(helper.EmprCodi);
                    if (!dr.IsDBNull(iEmprCodi)) entity.EmprCodi = dr.GetInt32(iEmprCodi);


                    int iFechaInicio = dr.GetOrdinal(helper.FechaInicio);
                    if (!dr.IsDBNull(iFechaInicio)) entity.FechaInicio = dr.GetDateTime(iFechaInicio);

                    int iFechaFin= dr.GetOrdinal(helper.FechaFin);
                    if (!dr.IsDBNull(iFechaFin)) entity.FechaFin = dr.GetDateTime(iFechaFin);

                    int iTipo = dr.GetOrdinal(helper.Tipo);
                    if (!dr.IsDBNull(iTipo)) entity.Tipo = dr.GetString(iTipo);


                    entitys.Add(entity);
                   
                }
            }

            return entitys;
        }


       
    
   }
}

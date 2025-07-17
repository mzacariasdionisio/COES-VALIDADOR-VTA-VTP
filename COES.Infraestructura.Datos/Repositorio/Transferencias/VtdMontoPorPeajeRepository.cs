using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using COES.Dominio.DTO.Transferencias;
using COES.Dominio.Interfaces.Transferencias;
using COES.Base.Core;
using COES.Infraestructura.Datos.Helper.Transferencias;

namespace COES.Infraestructura.Datos.Repositorio.Transferencias
{
    /// <summary>
    /// Clase de acceso a datos de la tabla VtdMontoPorPeaje
    /// </summary>
    public class VtdMontoPorPeajeRepository: RepositoryBase, IVtdMontoPorPeajeRepository
    {
        public VtdMontoPorPeajeRepository(string strConn): base(strConn)
        {
        }
        VtdMontoPorPeajeHelper helper = new VtdMontoPorPeajeHelper();
        //filtro por rango de fechas -Fit
        public VtdMontoPorPeajeDTO GetListFullByDateRange(int emprcodi,DateTime fechaInicio, DateTime fechaFinal)
        {
            VtdMontoPorPeajeDTO entity = new VtdMontoPorPeajeDTO();

            string sCommand = string.Format(helper.SqlListByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                }
            }

            return entity;
        }
        public List<VtdMontoPorPeajeDTO> GetListPageByDateRange(DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<VtdMontoPorPeajeDTO> entitys = new List<VtdMontoPorPeajeDTO>();

            string sCommand = string.Format(helper.SqlListPageByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorPeajeDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }

    }
}

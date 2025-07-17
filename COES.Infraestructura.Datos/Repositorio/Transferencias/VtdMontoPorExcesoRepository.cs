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
    /// Clase de acceso a datos de la tabla VTD_MONEXCESO
    /// </summary>
    public class VtdMontoPorExcesoRepository: RepositoryBase, IVtdMontoPorExcesoRepository
    {
        public VtdMontoPorExcesoRepository(string strConn): base(strConn)
        {
        }

        VtdMontoPorExcesoHelper helper = new VtdMontoPorExcesoHelper();

        //filtro por rango de fechas -Fit
        public List<VtdMontoPorExcesoDTO> GetListFullByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal)
        {
            List<VtdMontoPorExcesoDTO> entitys = new List<VtdMontoPorExcesoDTO>();

            string sCommand = string.Format(helper.SqlListByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha),emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorExcesoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
        public List<VtdMontoPorExcesoDTO> GetListPageByDateRange(int emprcodi, DateTime fechaInicio, DateTime fechaFinal, int nroPage, int pageSize)
        {
            List<VtdMontoPorExcesoDTO> entitys = new List<VtdMontoPorExcesoDTO>();

            string sCommand = string.Format(helper.SqlListPageByDateRange, fechaInicio.ToString(ConstantesBase.FormatoFecha), fechaFinal.ToString(ConstantesBase.FormatoFecha), nroPage, pageSize,emprcodi);
            DbCommand command = dbProvider.GetSqlStringCommand(sCommand);

            using (IDataReader dr = dbProvider.ExecuteReader(command))
            {
                while (dr.Read())
                {
                    VtdMontoPorExcesoDTO entity = helper.Create(dr);

                    int iEmprnomb = dr.GetOrdinal(helper.Emprnomb);
                    if (!dr.IsDBNull(iEmprnomb)) entity.Emprnomb = Convert.ToString(dr.GetValue(iEmprnomb));
                    entitys.Add(entity);
                }
            }

            return entitys;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COES.Base.Core;
using COES.Dominio.DTO.Transferencias;

namespace COES.Dominio.Interfaces.Transferencias
{
    public interface ITrnConfiguracionPmmeRepository : IRepositoryBase
    {
        int Save(TrnConfiguracionPmmeDTO entity);
        void Update(TrnConfiguracionPmmeDTO entity);
        List<TrnConfiguracionPmmeDTO> List(int emprcodi, DateTime fechaInicio, DateTime fechaFin);
        bool ValidarExistencia(int emprcodi, int ptomedicodi, string vigencia);
        List<TrnConfiguracionPmmeDTO> ListaConfPtosMMExEmpresa(int emprcodi);
        List<TrnConfiguracionPmmeDTO> ListaTrnConfiguracionPmme(int emprcodi, int ptomedicodi, string vigencia);
        TrnConfiguracionPmmeDTO GetById(int confconcodi);
        void Delete(int confconcodi, string estado);
    }
}

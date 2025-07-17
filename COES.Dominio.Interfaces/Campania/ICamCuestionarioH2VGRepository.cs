using COES.Dominio.DTO.Campania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COES.Dominio.Interfaces.Campania
{
    public interface ICamCuestionarioH2VGRepository
    {
        List<CuestionarioH2VGDTO> GetCamCuestionarioH2VG(int id);
        bool SaveCamCuestionarioH2VG(CuestionarioH2VGDTO camCuestionarioH2VGDto);
        bool UpdateCamCuestionarioH2VG(CuestionarioH2VGDTO camCuestionarioH2VGDto);
        int GetLastCamCuestionarioH2VGCodi();
        bool DeleteCamCuestionarioH2VGById(int id, string usuario);
        List<CuestionarioH2VGDTO> GetCamCuestionarioH2VGById(int id);
    }
}

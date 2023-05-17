using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Scripts.Persistence.DAO.Specification
{
    public interface IBossDAO
    {
        ISQliteConnectionProvider ConnectionProvider { get; }
        bool SetBoss(BossModel bossModel);
        bool UpdateBoss(BossModel bossModel);
        bool DeleteBoss(int id);
        BananaModel GetBoss(int id);
    }
}

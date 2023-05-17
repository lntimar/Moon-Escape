using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Scripts.Persistence.DAO.Specification
{
    public interface IBananaDAO
    {
        ISQliteConnectionProvider ConnectionProvider { get; }
        bool SetBanana(BananaModel bananaModel);
        bool UpdateBanana(BananaModel bananaModel);
        bool DeleteBanana(int id);
        BananaModel GetBanana(int id);
    }
}
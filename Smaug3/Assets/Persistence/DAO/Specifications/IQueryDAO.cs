using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Assets.Scripts.Persistence.DAO.Specification
{
    public interface IQueryDAO
    {
        ISQliteConnectionProvider ConnectionProvider { get; }
        void BananaLEFTBoss();
        void BananaINNERBoss();
        void BananaFULLBoss();
    }
}

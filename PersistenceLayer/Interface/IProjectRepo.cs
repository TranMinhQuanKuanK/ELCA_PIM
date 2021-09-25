using DomainLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersistenceLayer.Interface
{
    public interface IProjectRepo
    {
       IList<Project> GetProjectList();
    }
}

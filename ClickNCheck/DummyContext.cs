using Dummy.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ClickNCheck
{
    class DummyContext: System.Data.Entity.DbContext
    {
        public DummyContext(): base("mycs")// will add string connection to this
        {

        }
        public virtual System.Data.Entity.DbSet<DummyModel> DummyModel { get; set; }
    }
}

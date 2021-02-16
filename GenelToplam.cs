using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSatis.Entities.Tables
{
  public  class GenelToplam
    {

        public string Bilgi { get; set; }
        public Nullable<int> KayitSayisi { get; set; }

        public Nullable<decimal>  Tutar { get; set; }
    }
}

using NetSatis.Entities.Context;
using NetSatis.Entities.Repository;
using NetSatis.Entities.Tables;
using NetSatis.Entities.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSatis.Entities.DataAccess
{
   public class KasaDAL : EntityRepositoryBase<NetSatisContext, Kasa , KasaValidator>
    {

        public object KasaListele(NetSatisContext context)
        {

            var result = context.Kasalar.GroupJoin(context.KasaHareketleri, c => c.KasaKodu, c => c.KasaKodu, (kasa, kasahareket) => new
            {
                kasa.Id,
                kasa.KasaKodu,
                kasa.KasaAdi,
                kasa.YetkiliKodu,
                kasa.YetkiliAdi,
                kasa.Aciklama,
                KasaGiris=kasahareket.Where(c=>c.KasaKodu==kasa.KasaKodu && c.Hareket=="Kasa Giriş").Sum(c=>c.Tutar) ?? 0,
                KasaCikis = kasahareket.Where(c => c.KasaKodu == kasa.KasaKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0,
                Bakiye=(kasahareket.Where(c => c.KasaKodu == kasa.KasaKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0) -(kasahareket.Where(c => c.KasaKodu == kasa.KasaKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0)

            }).ToList();

            return result;
        }
    }
}

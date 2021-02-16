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
    public class KasaHareketDAL : EntityRepositoryBase<NetSatisContext, KasaHareket, KasaHareketValidator>
    {
        public object OdemeTuruToplamListele(NetSatisContext context, string kasaKodu)
        {

            var result = (from c in context.KasaHareketleri.Where(c => c.KasaKodu == kasaKodu)
                          group c by new { c.OdemeTuruAdi } into grp
                          select new
                          {

                              grp.Key.OdemeTuruAdi,
                              KasaGiris = grp.Where(c => c.OdemeTuruAdi == grp.Key.OdemeTuruAdi && c.OdemeTuruAdi == "Kasa Giriş").Sum(c => c.Tutar) ?? 0,
                              KasaCikis = grp.Where(c => c.OdemeTuruAdi == grp.Key.OdemeTuruAdi && c.OdemeTuruAdi == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0,
                              Bakiye = (grp.Where(c => c.OdemeTuruAdi == grp.Key.OdemeTuruAdi && c.OdemeTuruAdi == "Kasa Giriş").Sum(c => c.Tutar) ?? 0) - (grp.Where(c => c.OdemeTuruAdi == grp.Key.OdemeTuruAdi && c.OdemeTuruAdi == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0)
                          }
            ).ToList();

            return result;

        }

        public object GenelToplamListele(NetSatisContext context, string kasaKodu)
        {

            decimal? KasaGiris = (decimal?)context.KasaHareketleri.Where(c => c.KasaKodu == kasaKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0;
            decimal? KasaCikis = (decimal?)context.KasaHareketleri.Where(c => c.KasaKodu == kasaKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0;
            int? kasagiriskayitsayisi =(int?) context.KasaHareketleri.Where(c => c.KasaKodu == kasaKodu && c.Hareket == "Kasa Giriş").Count() ?? 0;
            int? kasacikiskayitsayisi =(int?) context.KasaHareketleri.Where(c => c.KasaKodu == kasaKodu && c.Hareket == "Kasa Çıkış").Count() ?? 0;




            List<GenelToplam> genelToplamlar = new List<GenelToplam>()
         {
             new GenelToplam
             {
                 Bilgi="Kasa Giriş",
                 KayitSayisi=kasagiriskayitsayisi,
                 Tutar=KasaGiris
             },
             new GenelToplam
              {
                 Bilgi="Kasa Çıkış",
                 KayitSayisi=kasacikiskayitsayisi,
                 Tutar=KasaCikis
             },
              new GenelToplam
              {
                 Bilgi="Bakiye",
                 KayitSayisi= kasagiriskayitsayisi+kasacikiskayitsayisi,
                 Tutar=KasaGiris-KasaCikis
             }

         };


            return genelToplamlar;

        }
    }
}

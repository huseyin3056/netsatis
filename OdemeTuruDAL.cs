﻿using NetSatis.Entities.Context;
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
    public class OdemeTuruDAL : EntityRepositoryBase<NetSatisContext, OdemeTuru, OdemeTuruValidator>
    {

        public object OdemeTuruListele(NetSatisContext context)
        {

            var result = context.OdemeTurleri.GroupJoin(context.KasaHareketleri, c => c.OdemeTuruKodu, c => c.OdemeTuruKodu, (odemeturu, kasahareket) => new
            {
                odemeturu.Id,
                odemeturu.OdemeTuruKodu,
                odemeturu.OdemeTuruAdi,
                odemeturu.Aciklama,
                KasaGiris = kasahareket.Where(c => c.OdemeTuruKodu == odemeturu.OdemeTuruKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0,
                KasaCikis = kasahareket.Where(c => c.OdemeTuruKodu == odemeturu.OdemeTuruKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0,
                Bakiye = (kasahareket.Where(c => c.OdemeTuruKodu == odemeturu.OdemeTuruKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0) - (kasahareket.Where(c => c.OdemeTuruKodu == odemeturu.OdemeTuruKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0)

            }).ToList();

            return result;
        }


        public object KasaToplamListele(NetSatisContext context, string odemeturuKodu)
        {

            var result = (from c in context.KasaHareketleri.Where(c => c.OdemeTuruKodu == odemeturuKodu)
                          group c by new { c.KasaKodu, c.KasaAdi } into grp
                          select new
                          {

                              grp.Key.KasaKodu,
                              grp.Key.KasaAdi,

                              KasaGiris = grp.Where(c => c.KasaKodu == grp.Key.KasaKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0,
                              KasaCikis = grp.Where(c => c.KasaKodu == grp.Key.KasaKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0,

                              Bakiye = (grp.Where(c => c.KasaKodu == grp.Key.KasaKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0) - (grp.Where(c => c.KasaKodu == grp.Key.KasaKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0)
                          }
                 ).ToList();

            return result;

        }


        public object GenelToplamListele(NetSatisContext context, string odemeturuKodu)
        {

            decimal? KasaGiris = (decimal?)context.KasaHareketleri.Where(c => c.OdemeTuruKodu == odemeturuKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0;
            decimal? KasaCikis = (decimal?)context.KasaHareketleri.Where(c => c.OdemeTuruKodu == odemeturuKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0;
            int? kasagiriskayitsayisi = (int?)context.KasaHareketleri.Where(c => c.OdemeTuruKodu == odemeturuKodu && c.Hareket == "Kasa Giriş").Count() ?? 0;
            int? kasacikiskayitsayisi = (int?)context.KasaHareketleri.Where(c => c.OdemeTuruKodu == odemeturuKodu && c.Hareket == "Kasa Çıkış").Count() ?? 0;




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

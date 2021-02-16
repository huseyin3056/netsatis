using NetSatis.Entities.Context;
using NetSatis.Entities.Interfaces;
using NetSatis.Entities.Repository;
using NetSatis.Entities.Tables;
using NetSatis.Entities.Validations;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NetSatis.Entities.DataAccess
{
    public class CariDAL : EntityRepositoryBase<NetSatisContext, Cari, CariValidator>
    {

        public object GetCariler(NetSatisContext context)
        {

            var result = context.Cariler.GroupJoin(context.Fisler, c => c.CariKodu, c => c.CariKodu, (cariler, fisler) => new
            {
                cariler.Id,
                cariler.CariKodu,
                cariler.Durumu,
                cariler.CariAdi,
                cariler.CariTuru,
                cariler.YetkiliKisi,
                cariler.FaturaUnvani,
                cariler.CepTelefonu,
                cariler.Telefon,
                cariler.Fax,
                cariler.Email,
                cariler.Web,
                cariler.Il,
                cariler.Ilce,
                cariler.Semt,
                cariler.Adres,
                cariler.CariGrubu,
                cariler.CariAltGrubu,
                cariler.OzelKod1,
                cariler.OzelKod2,
                cariler.OzelKod3,
                cariler.OzelKod4,
                cariler.VergiDairesi,
                cariler.VergiNo,
                cariler.IskontoOrani,
                cariler.RiskLimiti,
                cariler.AlisOzelFiyati,
                cariler.SatisOzelFiyati,
                cariler.Aciklama,

                AlisToplam = fisler.Where(c => c.FisTuru == "Alış Faturası").Sum(c => c.ToplamTutar) ?? 0,
                SatisToplam = fisler.Where(c => c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar) ?? 0,
                Bakiye = (fisler.Where(c => c.FisTuru == "Alış Faturası").Sum(c => c.ToplamTutar)) - (fisler.Where(c => c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar)) ?? 0

            }).GroupJoin(context.KasaHareketleri, c => c.CariKodu, c => c.CariKodu, (cariler, kasahareket) => new
            {
                cariler.Id,
                cariler.CariKodu,
                cariler.Durumu,
                cariler.CariAdi,
                cariler.CariTuru,
                cariler.YetkiliKisi,
                cariler.FaturaUnvani,
                cariler.CepTelefonu,
                cariler.Telefon,
                cariler.Fax,
                cariler.Email,
                cariler.Web,
                cariler.Il,
                cariler.Ilce,
                cariler.Semt,
                cariler.Adres,
                cariler.CariGrubu,
                cariler.CariAltGrubu,
                cariler.OzelKod1,
                cariler.OzelKod2,
                cariler.OzelKod3,
                cariler.OzelKod4,
                cariler.VergiDairesi,
                cariler.VergiNo,
                cariler.IskontoOrani,
                cariler.RiskLimiti,
                cariler.AlisOzelFiyati,
                cariler.SatisOzelFiyati,
                cariler.Aciklama,

                Alacak = cariler.AlisToplam + kasahareket.Where(c => c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0,
                Borc = cariler.SatisToplam + kasahareket.Where(c => c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0,
                Bakiye = (cariler.AlisToplam + kasahareket.Where(c => c.Hareket == "Kasa Giriş").Sum(c => c.Tutar)) - (cariler.SatisToplam + kasahareket.Where(c => c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar)) ?? 0

            }).ToList();

            return result;
        }


        public object CariFisAyrinti(NetSatisContext context, string cariKodu)
        {
            var result = context.Fisler.Where(c => c.CariKodu == cariKodu).GroupJoin(context.KasaHareketleri, c => c.CariKodu, c => c.CariKodu, (fisler, kasahareket) => new
            {
                fisler.Id,
                fisler.FisKodu,
                fisler.FisTuru,
                fisler.PlasiyerKodu,
                fisler.PlasiyerAdi,
                fisler.BelgeNo,
                fisler.Tarih,
                fisler.IskontoOrani,
                fisler.IskontoTutar,
                fisler.Aciklama,
                fisler.ToplamTutar,
                Odenen = context.KasaHareketleri.Where(c => c.FisKodu == fisler.FisKodu ).Sum(c => c.Tutar) ?? 0,
                KalanOdeme = (fisler.ToplamTutar) - (context.KasaHareketleri.Where(c => c.FisKodu == fisler.FisKodu).Sum(c => c.Tutar)) ?? 0



            }).ToList();

            return result;
        }

        public object CariFisGenelToplam(NetSatisContext context, string cariKodu)
        {
            var result = (from c in context.Fisler
                          where (c.CariKodu == cariKodu)
                          group c by new
                          {
                              c.FisTuru

                          } into grp
                          select new
                          {
                              Bilgi = grp.Key.FisTuru,
                              KayitSayisi = grp.Count(),
                              Tutar = grp.Sum(c => c.ToplamTutar) ?? 0

                          }).ToList();
            return result;

        }


        public object CariGenelToplam(NetSatisContext context, string cariKodu)
        {
            Nullable<decimal> f1 = (decimal?)context.Fisler.Where(c => c.CariKodu == cariKodu && c.FisTuru == "Alış Faturası").Sum(c => c.ToplamTutar) ;
            Nullable<decimal> f2 = (decimal?)context.KasaHareketleri.Where(c => c.CariKodu == cariKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar);

            Nullable<decimal> f3 = (decimal?)context.Fisler.Where(c => c.CariKodu == cariKodu && c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar);
            Nullable<decimal> f4 = (decimal?)context.KasaHareketleri.Where(c => c.CariKodu == cariKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar);

            decimal? alacak = f1 + f2;
            decimal? borc = f3 + f4;
                       

          

            List<GenelToplam> genelToplamlar = new List<GenelToplam>()
            {
                new GenelToplam
                {
                    Bilgi="Alacak",
                    Tutar=alacak
                },
                   new GenelToplam
                {
                    Bilgi="Borç",
                    Tutar=borc
                },
                      new GenelToplam
                {
                    Bilgi="Bakiye",
                    Tutar=alacak-borc
                }
            };

            return genelToplamlar;

        }

        public CariBakiye CariBakiyesi(NetSatisContext context, string cariKodu)
        {
            Nullable<decimal> alacak = (decimal?)context.Fisler.Where(c => c.CariKodu == cariKodu && c.FisTuru == "Alış Faturası").Sum(c => c.ToplamTutar) ?? 0+   (decimal?)context.KasaHareketleri.Where(c => c.CariKodu == cariKodu && c.Hareket == "Kasa Giriş").Sum(c => c.Tutar) ?? 0;

            Nullable<decimal> borc = (decimal?)context.Fisler.Where(c => c.CariKodu == cariKodu && c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar) ?? 0+   (decimal?)context.KasaHareketleri.Where(c => c.CariKodu == cariKodu && c.Hareket == "Kasa Çıkış").Sum(c => c.Tutar) ?? 0;


            CariBakiye entity = new CariBakiye
            {
                CariKodu=cariKodu,
                RiskLimiti=Convert.ToDecimal( context.Cariler.Where(c=>c.CariKodu==cariKodu).SingleOrDefault().RiskLimiti),
                Alacak= (decimal)alacak,
                Borc= (decimal)borc,
                Bakiye= (decimal)(alacak -borc)

            };

            return entity;
        }

    }

   
}

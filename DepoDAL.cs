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
    public class DepoDAL : EntityRepositoryBase<NetSatisContext, Depo, DepoValidator>

    {
        public object DepoStokListele(NetSatisContext context, string depoKodu)
        {
            var tablo = context.Stoklar.GroupJoin(context.StokHareketleri.Where(c => c.DepoKodu == depoKodu), c => c.StokKodu, c => c.StokKodu, (Stoklar, StokHareketleri) => new
            {

                Stoklar.StokAdi,
                Stoklar.Barkod,

                StokGiris = StokHareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar),
                StokCikis = StokHareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar),
                MevcutStok = (StokHareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar)) - (StokHareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar))


            }).ToList();


            return tablo;
        }

        public object DepoIstatistikListele(NetSatisContext context, string depoKodu)
        {
           

            List<GenelToplam> genelToplamlar = new List<GenelToplam>()
            {
                new GenelToplam
                {
                    Bilgi="Stok Giriş",
                    KayitSayisi=context.StokHareketleri.Where(c=>c.DepoKodu==depoKodu && c.Hareket=="Stok Giriş").Count(),
                    Tutar=context.StokHareketleri.Where(c=>c.DepoKodu==depoKodu && c.Hareket=="Stok Giriş").Sum(c=>c.Miktar) ?? 0
                },
                   new GenelToplam
                {
                   Bilgi="Stok Çıkış",
                    KayitSayisi=context.StokHareketleri.Where(c=>c.DepoKodu==depoKodu && c.Hareket=="Stok Çıkış").Count(),
                    Tutar=context.StokHareketleri.Where(c=>c.DepoKodu==depoKodu && c.Hareket=="Stok Çıkış").Sum(c=>c.Miktar) ?? 0
                }


            };

            return genelToplamlar;

        }
   
    
    public object DepoBazindaStokListele(NetSatisContext context,string stokKodu)
        {

            var result = context.Depolar.GroupJoin(context.StokHareketleri.Where(c => c.StokKodu == stokKodu), c => c.DepoKodu, c => c.DepoKodu, (depolar, stokhareketleri)

                    => new
                    {
                        depolar.Id,
                        depolar.DepoKodu,
                        depolar.DepoAdi,
                        depolar.YetkiliKodu,
                        depolar.YetkiliAdi,
                        depolar.Il,depolar.Ilce,depolar.Semt,depolar.Adres,depolar.Telefon,
                        StokGiris = stokhareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar),
                        StokCikis = stokhareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar),
                        MevcutStok = (stokhareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar)) - (stokhareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar))

                    }
                ).ToList();



            return result;
        }

    
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetSatis.Entities.Context;
using NetSatis.Entities.Repository;
using NetSatis.Entities.Tables;
using NetSatis.Entities.Validations;

namespace NetSatis.Entities.DataAccess
{
    public class StokDAL : EntityRepositoryBase<NetSatisContext, Stok, StokValidator>
    {
        public object GetAllJoin(NetSatisContext context)
        {
            var tablo = context.Stoklar.GroupJoin(context.StokHareketleri, c => c.StokKodu, c => c.StokKodu, (Stoklar, StokHareketleri) => new
            {
                Stoklar.StokKodu,
                Stoklar.StokAdi,
                Stoklar.Durumu,
                Stoklar.Barkod,
                Stoklar.BarkodTuru,
                Stoklar.Birimi,
                Stoklar.StokAltGrubu,
                Stoklar.StokGrubu,
                Stoklar.Marka,
                Stoklar.Modeli,
                Stoklar.OzelKod1,
                Stoklar.OzelKod2,
                Stoklar.OzelKod3,
                Stoklar.OzelKod4,
                Stoklar.AlisFiyati1,
                Stoklar.AlisFiyati2,
                Stoklar.AlisFiyati3,
                Stoklar.SatisFiyati1,
                Stoklar.SatisFiyati2,
                Stoklar.SatisFiyati3,
                Stoklar.AlisKdv,
                Stoklar.SatisKdv,
                Stoklar.GarantiSuresi,
                Stoklar.UreticiKodu,
                Stoklar.MinStokMiktari,
                Stoklar.MaxStokMiktari,
                Stoklar.Aciklama,

                StokGiris = StokHareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar),
                StokCikis = StokHareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar),
                MevcutStok = (StokHareketleri.Where(c => c.Hareket == "Stok Giriş").Sum(c => c.Miktar)) - (StokHareketleri.Where(c => c.Hareket == "Stok Çıkış").Sum(c => c.Miktar))


            }).ToList();


            return tablo;
        }

       

    }
}

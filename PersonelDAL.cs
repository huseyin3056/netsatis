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
    public class PersonelDAL : EntityRepositoryBase<NetSatisContext, Personel, PersonelValidator>
    {

        public object PersonelListele(NetSatisContext context)
        {
            var result = context.Personeller.GroupJoin(context.Fisler, c => c.PersonelKodu, c => c.PlasiyerKodu, (personel, fis) => new
            {
                personel.Id,
                personel.Calisiyor,
                personel.PersonelKodu,
                personel.PersonelAdi,
                personel.Unvani,
                personel.TcKimlikNo,
                personel.IseGirisTarihi,
                personel.IstenCikisTarihi,
                personel.VergiDairesi,
                personel.VergiNo,
                personel.CepTelefonu,
                personel.Telefon,
                personel.Il,
                personel.Ilce,
                personel.Fax,
                personel.Semt,
                personel.Adres,
                personel.Email,
                personel.Web,
                personel.PrimOrani,
                personel.AylikMaasi,
                personel.Aciklama,
                ToplamSatis = fis.Where(c => c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar) ?? 0,
                PrimTutari = (fis.Where(c => c.FisTuru == "Perakende Satış Faturası").Sum(c => c.ToplamTutar) ?? 0) / 100 * personel.PrimOrani



            }).ToList();

            return result;
        }


        public object PersonelFisToplam(NetSatisContext context, string personelKodu)
        {
            var result = (from c in context.Fisler.Where(c => c.PlasiyerKodu == personelKodu)
                          group c by new
                          {
                              c.FisTuru

                          } into grp
                          select new
                          {
                              Bilgi=grp.Key.FisTuru,
                              KayitSayisi=grp.Count(),
                              ToplamTutar=grp.Sum(c=>c.ToplamTutar)
                              
                          }).ToList();

            return result;
        }

    }
}

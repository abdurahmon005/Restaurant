using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp.Domain.Enums
{
    public enum OtpStatus
    {
        Created = 0,        // kod yaratildi, foydalanuvchiga hali yuborilmadi
        Sent = 1,           // kod foydalanuvchiga yuborildi (Telegram/SMS orqali)
        Verified = 2,       // foydalanuvchi kodni to‘g‘ri kiritdi
        Expired = 3,        // kodning amal qilish muddati o‘tdi
        Used = 4,           // kod bir marta ishlatildi va endi qayta ishlatilmaydi
        Cancelled = 5        // kod bekor qilindi yoki yanlış so‘rov tufayli bekor qilindi
    }
}

﻿

namespace Auth_Core
{
    public static class AppExceptions
    {
        public static IDictionary<int, string[]> ExceptionMessages = new Dictionary<int, string[]>()
        {
            {-1,new string[]{ "Error Happen Contact With Customer Service", "حدث خطأ يرجي التواصل مع خدمة العملاء "}  },
            {1,new string[]{ "Record Not Existed", "هذا السجل غير موجود "}  },
            {2,new string[]{ "Model Not Valid", "المدخلات غير صحيحة"}  },
            {3,new string[]{ "File Not Exist", "الملف غير موجود"}  },
            {4,new string[]{ "Property NotAccess ", "لا يمكن الوصول للعنصر"}  },
            {35,new string[]{ "user is Locked", "العميل موقوف "}  },
            {37,new string[]{ "user Phone not confirmed ", "لم يتم تأكيد رقم الجوال "}  },
            {53,new string[]{ "Error Get Vehicle Data From Yakeen", "حدث خطأ اثناء جلب بيانات اللوحة من يقين"} },
            {54,new string[]{ "Error Get Vehicle Data From Yakeen", "حدث خطأ اثناء جلب بيانات المركبة من يقين"}  },
            {55,new string[]{ "Reqest Model Is Invalid ", "المدخلات غير صحيحة"}  },
            {56,new string[]{ "Uploaded File Is damaged", "الملف غير صالح"}  },
            {57,new string[]{ "Maker code can't be less than 1", "ماركة المركبة غير صحيحة "}  },
            {58,new string[]{ "Policy Effective Date Not Correct", "تاريخ تفعيل الوثيقة غير صحيح"}  },
            {59,new string[]{ "Policy effective date should be within 30 days starts from toworrow ", "تاريخ اصدار الوثيقة لابد ان يبدأ من غدا وخلال 30  يوم"}  },
             {60,new string[]{ "Error Happen Save File", "حدث خطأ اثناء حفظ الملف"}  },
             {61,new string[]{ "Captcha not correct", "كلمة التحقق غير صحيحة" }  },
             {62,new string[]{ "Provider Id Not Found", " مزود الخدمة غير موجود" }  },
             {63,new string[]{ "No product To show", "لا يوجد عرض سعر" }  },
             {64,new string[]{ "Error happen Get Quotaion request", "حدث خطأ اثناء جلب بيانات الطلب" }  },
             {66,new string[]{ "Invalid UserName", "الاسم غير صحيح" }  },
             {67,new string[]{ "The request Not Clear", "البيانات المدخلة غير متطابقة" }  },
             {68,new string[]{ "Error Get Shopping Cart Data ", "سلة مشتريات العميل فارغة" }  },
             {69,new string[]{ "Quotaion Expire With in 16 h ", "عرض السعر انتهي" }  },
             {70,new string[]{ "Request policy effective date is less than 16hr ", "خطأ في تاريخ تفعيل الوثيقة" }  },
             {71,new string[]{ "CrInfo Data Not Found ", "بيانات الشركة غير موجودة " }  },
             {72,new string[]{ "Wrong Capcha Input", "رقم التحقق خطأ"}  },
        };
    }
}

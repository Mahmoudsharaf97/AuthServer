

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
            {38,new string[]{ "Email is not confirmed ", "البريد الألكترونى غير مفعل لهذا الحساب " }  },
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
             {82,new string[]{ "Cached Refresh Token With Email Not Found", "رمز التحديث المخزن مؤقتًا المربوط بالبريد الإلكتروني غير موجود" }  },
             {83,new string[]{ "Refresh Token Is Wrong", "رمز التحديث خطأ" }  },
             {84,new string[]{ "NationalId  is Empty", "من فضلك ادخل رقم الهويه" }  },
             {85,new string[]{ "Birth year is empty or invalid value", "سنة الميلاد فارغة أو قيمة غير صحيحة" }  },
             {86,new string[]{ "Please enter email", "البريد الإلكتروني مطلوب" }  },
             {87,new string[]{ "Please enter password", "كلمة المرور مطلوبة" }  },
             {88,new string[]{ "Mobile is empty", "من فضلك ادخل رقم الجوال" }  },
             {89,new string[]{ "Something wrong with Mobile Number", "هناك خطأ في رقم الجوال" }  },
             {90,new string[]{ "Empty OTP", "OTP فارغ" }  },
             {91,new string[]{ "Empty input parameter", "برجاء استكمال البيانات" }  },
             {92,new string[]{ "No Account is Linked With This Id Please Login With The Email", "لا يوجد حساب مربوط بالهوية ,الرجاء تسجيل الدخول بالبريد الألكتروني " }  },
             {93,new string[]{ "Incorrect username or password", "اسم المستخدم أو كلمة المرور غير صحيحة " }  },
             {94,new string[]{ "Error In OTP", "خطأ في رمز التحقق " }  },
             {95,new string[]{ "OTP Expired", "انتهت صلاحيه رمز التحقق " }  },
             {96,new string[]{ "model is empty", "المدخلات ناقصه او فارغه" }  },
        };
    }
}

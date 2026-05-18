namespace BLL.Utility
{
    public class NumberConverter
    {
        public static string BanglaWords(int number)
        {
            string banglaWords = string.Empty;
            string[] banglaSpecialCases = new string[]
            {
                "শূন্য", "এক", "দুই", "তিন", "চার", "পাঁচ", "ছয়", "সাত", "আট", "নয়",
                "দশ", "এগারো", "বারো", "তেরো", "চৌদ্দ", "পনেরো", "ষোল", "সতেরো", "আঠারো", "ঊনিশ",
                "বিশ", "একুশ", "বাইশ", "তেইশ", "চব্বিশ", "পঁচিশ", "ছাব্বিশ", "সাতাশ", "আটাশ", "ঊনত্রিশ",
                "ত্রিশ", "একত্রিশ", "বত্রিশ", "তেত্রিশ", "চৌত্রিশ", "পঁয়ত্রিশ", "ছত্রিশ", "সাইত্রিশ", "আটত্রিশ", "ঊনচল্লিশ",
                "চল্লিশ", "একচল্লিশ", "বিয়াল্লিশ", "তেতাল্লিশ", "চুয়াল্লিশ", "পঁয়তাল্লিশ", "ছেচল্লিশ", "সাতচল্লিশ", "আটচল্লিশ", "ঊনপঞ্চাশ",
                "পঞ্চাশ", "একান্ন", "বাহান্ন", "তিপ্পান্ন", "চুয়ান্ন", "পঞ্চান্ন", "ছাপ্পান্ন", "সাতান্ন", "আটান্ন", "ঊনষাট",
                "ষাট", "একষট্টি", "বাষট্টি", "তেষট্টি", "চৌষট্টি", "পঁষট্টি", "ছেষট্টি", "সাতষট্টি", "আটষট্টি", "ঊনসত্তর",
                "সত্তর", "একাত্তর", "বাহাত্তর", "তিয়াত্তর", "চুয়াত্তর", "পঁচাত্তর", "ছিয়াত্তর", "সাতাত্তর", "আটাত্তর", "ঊনআশি",
                "আশি", "একাশি", "বিরাশি", "তিরাশি", "চুরাশি", "পঁচাশি", "ছিয়াশি", "সাতাশি", "অষ্টআশি", "ঊননব্বই",
                "নব্বই", "একানব্বই", "বিরানব্বই", "তিরানব্বই", "চুরানব্বই", "পঁচানব্বই", "ছিয়ানব্বই", "সাতানব্বই", "আটানব্বই", "নিরানব্বই"
            };

            if (number == 0)
            {
                banglaWords = "শূন্য";
                return banglaWords;
            }
            if (number >= 10000000)
            {
                int crores = number / 10000000;
                banglaWords += string.Concat(BanglaWords(crores), " কোটি ");
                number %= 10000000;
            }
            if (number >= 100000)
            {
                int lakhs = number / 100000;
                banglaWords += string.Concat(BanglaWords(lakhs), " লাখ ");
                number %= 100000;
            }
            if (number >= 1000)
            {
                int thousands = number / 1000;
                banglaWords += string.Concat(BanglaWords(thousands), " হাজার ");
                number %= 1000;
            }
            if (number >= 100)
            {
                int hundreds = number / 100;
                banglaWords += string.Concat(banglaSpecialCases[hundreds], "শো ");
                number %= 100;
            }
            if (number > 0)
            {
                banglaWords += banglaSpecialCases[number];
            }
            return banglaWords.Trim();
        }
        public static string EnglishWords(int number)
        {
            string englishWords = string.Empty;
            string[] englishSpecialCases = new string[]
            {
                "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
                "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen",
                "Twenty", "Twenty-One", "Twenty-Two", "Twenty-Three", "Twenty-Four", "Twenty-Five", "Twenty-Six", "Twenty-Seven", "Twenty-Eight", "Twenty-Nine",
                "Thirty", "Thirty-One", "Thirty-Two", "Thirty-Three", "Thirty-Four", "Thirty-Five", "Thirty-Six", "Thirty-Seven", "Thirty-Eight", "Thirty-Nine",
                "Forty", "Forty-One", "Forty-Two", "Forty-Three", "Forty-Four", "Forty-Five", "Forty-Six", "Forty-Seven", "Forty-Eight", "Forty-Nine",
                "Fifty", "Fifty-One", "Fifty-Two", "Fifty-Three", "Fifty-Four", "Fifty-Five", "Fifty-Six", "Fifty-Seven", "Fifty-Eight", "Fifty-Nine",
                "Sixty", "Sixty-One", "Sixty-Two", "Sixty-Three", "Sixty-Four", "Sixty-Five", "Sixty-Six", "Sixty-Seven", "Sixty-Eight", "Sixty-Nine",
                "Seventy", "Seventy-One", "Seventy-Two", "Seventy-Three", "Seventy-Four", "Seventy-Five", "Seventy-Six", "Seventy-Seven", "Seventy-Eight", "Seventy-Nine",
                "Eighty", "Eighty-One", "Eighty-Two", "Eighty-Three", "Eighty-Four", "Eighty-Five", "Eighty-Six", "Eighty-Seven", "Eighty-Eight", "Eighty-Nine",
                "Ninety", "Ninety-One", "Ninety-Two", "Ninety-Three", "Ninety-Four", "Ninety-Five", "Ninety-Six", "Ninety-Seven", "Ninety-Eight", "Ninety-Nine"
            };
            if (number == 0)
            {
                englishWords = "Zero";
                return englishWords;
            }
            if (number >= 10000000)
            {
                int crores = number / 10000000;
                englishWords += string.Concat(EnglishWords(crores), " Crore ");
                number %= 10000000;
            }
            if (number >= 100000)
            {
                int lakhs = number / 100000;
                englishWords += string.Concat(EnglishWords(lakhs), " Lakh ");
                number %= 100000;
            }
            if (number >= 1000)
            {
                int thousands = number / 1000;
                englishWords += string.Concat(EnglishWords(thousands), " Thousand ");
                number %= 1000;
            }
            if (number >= 100)
            {
                int hundreds = number / 100;
                englishWords += string.Concat(englishSpecialCases[hundreds], " Hundred ");
                number %= 100;
            }
            if (number > 0)
            {
                englishWords += englishSpecialCases[number];
            }
            return englishWords.Trim();
        }
    }
}
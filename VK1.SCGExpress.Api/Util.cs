using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VK1.SCGExpress.Api {
    public class Util {
        public string CorrectMobileFormat(string mobile) {

            //1 prepend 0
            mobile = mobile.Substring(0, 1) == "0" ? mobile : $"0{mobile}";

            //2 get rid of character
            string[] array = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < mobile.Length; i++) {
                var member = mobile.Substring(i, 1);
                bool isNumber = array.Any(x => x == member);
                if (isNumber) {
                    stringBuilder.Append(member);
                }
            }
            var newNumber = stringBuilder.ToString();

            var length = newNumber.Length;
            //3
            if (length > 10) {
                return newNumber.Substring(0, 10);
            }
            if (length < 10) {
                return newNumber.PadRight(10, '0');
            } else {
                return newNumber;
            }
        }
    }
}

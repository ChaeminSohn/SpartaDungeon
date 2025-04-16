using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpartaDungeon
{
    internal static class Utils
    {
        // 실제 출력 너비 기준으로 문자열 오른쪽 패딩
        public static string PadToWidth(string str, int totalWidth)
        {
            if(str == null)
            {
                str = "";   //null 이면 빈 문자열로 처리
            }
            int realLength = str.Sum(c => c > 127 ? 2 : 1); // 한글은 너비 2, 영어는 1로 계산
            int padding = Math.Max(0, totalWidth - realLength);
            return str + new string(' ', padding);
        }
    }
}

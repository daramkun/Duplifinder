using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Duplifinder
{
	public static class HangulDestroyer
	{
		// ㄱ ㄲ ㄴ ㄷ ㄸ ㄹ ㅁ ㅂ ㅃ ㅅ ㅆ ㅇ ㅈ ㅉ ㅊ ㅋ ㅌ ㅍ ㅎ
		private static readonly int[] chosung =
		{
			0x3131, 0x3132, 0x3134, 0x3137, 0x3138, 0x3139, 0x3141, 0x3142, 0x3143,
			0x3145, 0x3146, 0x3147, 0x3148, 0x3149, 0x314a, 0x314b, 0x314c, 0x314d,
			0x314e
		};

		// ㅏ ㅐ ㅑ ㅒ ㅓ ㅔ ㅕ ㅖ ㅗ ㅘ ㅙ ㅚ ㅛ ㅜ ㅝ ㅞ ㅟ ㅠ ㅡ ㅢ ㅣ
		private static readonly int [] jungsung =
		{
			0x314f, 0x3150, 0x3151, 0x3152, 0x3153, 0x3154, 0x3155, 0x3156, 0x3157,
			0x3158, 0x3159, 0x315a, 0x315b, 0x315c, 0x315d, 0x315e, 0x315f, 0x3160,
			0x3161, 0x3162, 0x3163
		};

		// ㄱ ㄲ ㄳ ㄴ ㄵ ㄶ ㄷ ㄹ ㄺ ㄻ ㄼ ㄽ ㄾ ㄿ ㅀ ㅁ ㅂ ㅄ ㅅ ㅆ ㅇ ㅈ ㅊ ㅋ ㅌ ㅍ ㅎ
		private static readonly int [] jongsung =
		{
			0x0000, 0x3131, 0x3132, 0x3133, 0x3134, 0x3135, 0x3136, 0x3137, 0x3139,
			0x313a, 0x313b, 0x313c, 0x313d, 0x313e, 0x313f, 0x3140, 0x3141, 0x3142,
			0x3144, 0x3145, 0x3146, 0x3147, 0x3148, 0x314a, 0x314b, 0x314c, 0x314d,
			0x314e
		};

		public static string DestroyedHangulCharacters(string str)
		{
			StringBuilder builder = new StringBuilder();
			foreach (var ch in str)
			{
				if (ch >= 0xAC00 && ch <= 0xD7A3)
				{
					var jongsungCh = ch - 0xAC00;
					var chosungCh = jongsungCh / (21 * 28);
					jongsungCh %= (21 * 28);
					var jungsungCh = jongsungCh / 28;
					jongsungCh %= 28;

					builder.Append((char) chosung[chosungCh]);
					builder.Append((char) jungsung[jungsungCh]);
					if (jongsungCh > 0)
						builder.Append((char) jongsung[jongsungCh]);
				}
				else
					builder.Append(ch);
			}

			return builder.ToString();
		}
	}
}

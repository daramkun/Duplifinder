using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using F23.StringSimilarity;

namespace Duplifinder
{
	public static class StringExtensions
	{
		private static readonly F23.StringSimilarity.JaroWinkler winkler = new JaroWinkler();

		public static double CalculateSimilarity(this string str1, string str2)
		{
			var longer = HangulDestroyer.DestroyedHangulCharacters(str1);
			var shorter = HangulDestroyer.DestroyedHangulCharacters(str2);

			if (longer.Length < shorter.Length)
			{
				var temp = longer;
				longer = shorter;
				shorter = temp;
			}

			var longerLength = longer.Length;
			return longerLength == 0 ? 1.0 : winkler.Similarity(longer, shorter);
		}

		public static double FilenameSimilarity(this string str1, string str2)
		{
			return CalculateSimilarity(
				Path.GetFileNameWithoutExtension(str1),
				Path.GetFileNameWithoutExtension(str2)
			);
		}
	}
}

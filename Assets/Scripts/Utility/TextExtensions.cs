using System;
using System.Collections.Generic;

namespace Surreily.SomeWords.Scripts.Utility {
    public static class TextExtensions {
        public static IEnumerable<string> SplitIntoLines(this string text, int length) {
            if (length <= 0) {
                throw new ArgumentException("Length must be a positive integer.", nameof(length));
            }

            int start = 0;

            while (text[start] == ' ' && start < text.Length) {
                start++;
            }

            int end = start + length;

            while (end <= text.Length) {
                while (text[end] != ' ' && end > start) {
                    end--;
                }

                if (end == start) {
                    yield return text.Substring(start, length);
                    start += length;
                } else {
                    yield return text.Substring(start, (end - start));
                    start = end + 1;
                }

                while (text[start] == ' ' && start < text.Length) {
                    start++;
                }

                end = start + length;
            }

            while (text[start] == ' ' && start < text.Length) {
                start++;
            }

            yield return text.Substring(start);
        }
    }
}

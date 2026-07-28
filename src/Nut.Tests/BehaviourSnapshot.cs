using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Nut.Tests
{
    /// <summary>
    /// Renders every language against every currency across a spread of amounts, and
    /// compares the whole lot to a checked-in snapshot. The per-language fixtures assert
    /// what the wording should be; this asserts that nothing else moved with it.
    ///
    /// It exists because green tests are not enough on their own: while the shared-state
    /// fix was being written, 234 passing tests hid 44 wrong outputs, because no test
    /// happened to combine Russian with the hryvnia. A full dump has no such blind spots.
    ///
    /// When a change is meant to alter output, regenerate the snapshot with
    /// <c>UPDATE_SNAPSHOT=1 dotnet test</c> and the diff of this file becomes the
    /// human-readable record of exactly what changed.
    /// </summary>
    [TestFixture]
    public class BehaviourSnapshot
    {
        private static readonly string[] Languages =
        {
            Language.English, Language.French, Language.German, Language.Spanish,
            Language.Portuguese, Language.Turkish, Language.Russian, Language.Ukrainian,
            Language.Belarusian, Language.Bulgarian, Language.Polish, Language.Amharic,
        };

        private static readonly string[] Currencies =
        {
            Currency.EUR, Currency.USD, Currency.RUB, Currency.TRY, Currency.UAH,
            Currency.BGN, Currency.ETB, Currency.PLN, Currency.BYN, Currency.ARS, Currency.BRL,
        };

        private static readonly decimal[] Amounts =
        {
            0m, 0.01m, 0.02m, 1m, 1.01m, 1.02m, 2m, 2.02m, 3m, 5m, 11m, 21m, 22m, 42m, 100m,
            101m, 121m, 999m, 1000m, 1001m, 2000m, 2001m, 5000m, 21000m, 22000m, 41000m,
            42000m, 100000m, 1000000m, 2000000m, 1000000000m, 123.45m,
        };

        private static readonly long[] PlainNumbers =
        {
            0, 1, 2, 3, 5, 11, 15, 20, 21, 22, 42, 100, 101, 121, 200, 999, 1000, 1001, 2000,
            5000, 21000, 41000, 100000, 1000000, 2000000, 999999999,
        };

        [Test]
        public void OutputMatchesTheCheckedInSnapshot()
        {
            var actual = Render();
            var path = SnapshotPath();

            if (Environment.GetEnvironmentVariable("UPDATE_SNAPSHOT") == "1")
            {
                File.WriteAllText(path, actual);
                // Fail rather than skip: if this variable is ever set in CI by accident,
                // a skipped test would quietly disable the check, while a failure is loud.
                Assert.Fail("Snapshot rewritten. Review the diff of behaviour-snapshot.tsv, " +
                            "then re-run without UPDATE_SNAPSHOT to verify.");
            }

            var expected = File.ReadAllText(path);
            if (expected == actual) return;

            Assert.Fail($"Output differs from the snapshot.{Environment.NewLine}" +
                        $"{Describe(expected, actual)}{Environment.NewLine}" +
                        "If the change is intended, rerun with UPDATE_SNAPSHOT=1 and commit " +
                        "behaviour-snapshot.tsv so the diff records it.");
        }

        private static string Render()
        {
            var sb = new StringBuilder();
            foreach (var lang in Languages)
            {
                foreach (var n in PlainNumbers)
                    sb.Append("plain\t").Append(lang).Append('\t').Append(n).Append('\t')
                      .Append(n.ToText(lang)).Append('\n');

                foreach (var currency in Currencies)
                    foreach (var amount in Amounts)
                    {
                        string text;
                        try { text = amount.ToText(currency, lang); }
                        catch (Exception e) { text = "EX:" + e.GetType().Name; }
                        sb.Append("money\t").Append(lang).Append('\t').Append(currency)
                          .Append('\t').Append(amount).Append('\t').Append(text).Append('\n');
                    }
            }
            return sb.ToString();
        }

        /// <summary>First few differing lines, so a failure is readable without opening the file.</summary>
        private static string Describe(string expected, string actual)
        {
            var want = expected.Split('\n');
            var got = actual.Split('\n');
            var lines = new List<string>();
            var differing = 0;

            for (var i = 0; i < Math.Max(want.Length, got.Length); i++)
            {
                var a = i < want.Length ? want[i] : "<missing>";
                var b = i < got.Length ? got[i] : "<missing>";
                if (a == b) continue;
                differing++;
                if (lines.Count < 20) lines.Add($"  - {a}{Environment.NewLine}  + {b}");
            }

            var shown = string.Join(Environment.NewLine, lines);
            return differing > 20
                ? $"{differing} lines differ, first 20:{Environment.NewLine}{shown}"
                : $"{differing} lines differ:{Environment.NewLine}{shown}";
        }

        private static string SnapshotPath([CallerFilePath] string thisFile = "")
        {
            return Path.Combine(Path.GetDirectoryName(thisFile) ?? ".", "behaviour-snapshot.tsv");
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace zxcvbn_test
{
    [TestClass]
    public class ZxcvbnTest
    {
        private static string[] testPasswords = new string[] {
            "zxcvbn",
            "qwER43@!",
            "Tr0ub4dour&3",
            "correcthorsebatterystaple",
            "coRrecth0rseba++ery9.23.2007staple$",
            "D0g..................",
            "abcdefghijk987654321",
            "neverforget13/3/1997",
            "1qaz2wsx3edc",
            "temppass22",
            "briansmith",
            "briansmith4mayor",
            "password1",
            "viking",
            "thx1138",
			"ScoRpi0ns",
			"do you know",
			"ryanhunter2000",
			"rianhunter2000",
			"asdfghju7654rewq",
			"AOEUIDHG&*()LS_",
			"12345678",
			"defghi6789",
			"rosebud",
			"Rosebud",
			"ROSEBUD",
			"rosebuD",
			"ros3bud99",
			"r0s3bud99",
			"R0$38uD99",
			"verlineVANDERMARK",
			"eheuczkqyq",
			"rWibMFACxAUGZmxhVncy",
			"Ba9ZyWABu99[BK#6MBgbH88Tofv)vs$w"
        };

        // Entropies for the above passwords, in order, as generated by zxcvbn
        private static double[] expectedEntropies = new double[] {
            6.845,
            26.44,
            30.435,
            45.212,
            66.018,
            20.678,
            11.951,
            32.628,
            19.314,
            22.179,
            4.322,
            18.64,
            2,
            7.531,
            7.426,
            20.621,
            20.257,
            14.506,
            21.734,
            29.782,
            33.254,
            1.585,
            12.607,
            7.937,
            8.937,
            8.937,
            8.937,
            19.276,
            19.276,
            25.076,
            26.293,
            42.813,
            104.551,
            167.848
        };

        [TestMethod]
        public void RunAllTestPasswords()
        {
            var zx = new Zxcvbn.Zxcvbn(new Zxcvbn.DefaultMatcherFactory());

            for (int i = 0; i < testPasswords.Length; ++i)
            {
                var password = testPasswords[i];

                var result = zx.EvaluatePassword(password);

                O("");
                O("Password:        {0}", result.Password);
                O("Entropy:         {0}", result.Entropy);
                O("Crack Time (s):  {0}", result.CrackTime);
                O("Crack Time (d):  {0}", result.CrackTimeDisplay);
                O("Score (0 to 4):  {0}", result.Score);
                O("Calc time (ms):  {0}", result.CalcTime);
                O("--------------------");

                foreach (var match in result.MatchSequence)
                {
                    if (match != result.MatchSequence.First()) O("+++++++++++++++++");

                    O(match.Token);
                    O("Pattern:      {0}", match.Pattern);
                    O("Entropy:      {0}", match.Entropy);

                    if (match is Zxcvbn.Matcher.DictionaryMatch)
                    {
                        var dm = match as Zxcvbn.Matcher.DictionaryMatch;
                        O("Dict. Name:   {0}", dm.DictionaryName);
                        O("Rank:         {0}", dm.Rank);
                        O("Base Entropy: {0}", dm.BaseEntropy);
                        O("Upper Entpy:  {0}", dm.UppercaseEntropy);
                    }

                    if (match is Zxcvbn.Matcher.L33tDictionaryMatch)
                    {
                        var lm = match as Zxcvbn.Matcher.L33tDictionaryMatch;
                        O("L33t Entpy:   {0}", lm.L33tEntropy);
                        O("Unleet:       {0}", lm.MatchedWord);
                    }

                    if (match is Zxcvbn.Matcher.SpatialMatch)
                    {
                        var sm = match as Zxcvbn.Matcher.SpatialMatch;
                        O("Graph:        {0}", sm.Graph);
                        O("Turns:        {0}", sm.Turns);
                        O("Shifted Keys: {0}", sm.ShiftedCount);
                    }

                    if (match is Zxcvbn.Matcher.RepeatMatch)
                    {
                        var rm = match as Zxcvbn.Matcher.RepeatMatch;
                        O("Repeat char:  {0}", rm.RepeatChar);
                    }

                    if (match is Zxcvbn.Matcher.SequenceMatch)
                    {
                        var sm = match as Zxcvbn.Matcher.SequenceMatch;
                        O("Seq. name:    {0}", sm.SequenceName);
                        O("Seq. size:    {0}", sm.SequenceSize);
                        O("Ascending:    {0}", sm.Ascending);
                    }

                    if (match is Zxcvbn.Matcher.DateMatch)
                    {
                        var dm = match as Zxcvbn.Matcher.DateMatch;
                        O("Day:          {0}", dm.Day);
                        O("Month:        {0}", dm.Month);
                        O("Year:         {0}", dm.Year);
                        O("Separator:    {0}", dm.Separator);
                    }
                }

                O("");
                O("=========================================");

                Assert.AreEqual(expectedEntropies[i], result.Entropy);
            }
        }

        private void O(string format, params object[] args)
        {
            System.Diagnostics.Debug.WriteLine(format, args);
        }


        [TestMethod]
        public void BruteForceCardinalityTest()
        {
            Assert.AreEqual(26, Zxcvbn.PasswordScoring.PasswordCardinality("asdf"));
            Assert.AreEqual(26, Zxcvbn.PasswordScoring.PasswordCardinality("ASDF"));
            Assert.AreEqual(52, Zxcvbn.PasswordScoring.PasswordCardinality("aSDf"));
            Assert.AreEqual(10, Zxcvbn.PasswordScoring.PasswordCardinality("124890"));
            Assert.AreEqual(62, Zxcvbn.PasswordScoring.PasswordCardinality("aS159Df"));
            Assert.AreEqual(33, Zxcvbn.PasswordScoring.PasswordCardinality("!@<%:{$:#<@}{+&)(*%"));
            Assert.AreEqual(100, Zxcvbn.PasswordScoring.PasswordCardinality("©"));
            Assert.AreEqual(95, Zxcvbn.PasswordScoring.PasswordCardinality("ThisIs@T3stP4ssw0rd!"));
        }

        [TestMethod]
        public void TimeDisplayStrings()
        {
            // Note that the time strings should be + 1
            Assert.AreEqual("11 minutes", Zxcvbn.Utility.DisplayTime(60 * 10));
            Assert.AreEqual("2 days", Zxcvbn.Utility.DisplayTime(60 * 60 * 24));
            Assert.AreEqual("17 years", Zxcvbn.Utility.DisplayTime(60 * 60 * 24 * 365 * 15.4));
        }

        [TestMethod]
        public void RepeatMatcher()
        {
            var repeat = new Zxcvbn.Matcher.RepeatMatcher();

            var res = repeat.MatchPassword("aaasdffff");
            Assert.AreEqual(2, res.Count());

            var m1 = res.ElementAt(0);
            Assert.AreEqual(0, m1.i);
            Assert.AreEqual(2, m1.j);
            Assert.AreEqual("aaa", m1.Token);

            var m2 = res.ElementAt(1);
            Assert.AreEqual(5, m2.i);
            Assert.AreEqual(8, m2.j);
            Assert.AreEqual("ffff", m2.Token);


            res = repeat.MatchPassword("asdf");
            Assert.AreEqual(0, res.Count());
        }

        [TestMethod]
        public void SequenceMatcher()
        {
            var seq = new Zxcvbn.Matcher.SequenceMatcher();

            var res = seq.MatchPassword("abcd");
            Assert.AreEqual(1, res.Count());
            var m1 = res.First();
            Assert.AreEqual(0, m1.i);
            Assert.AreEqual(3, m1.j);
            Assert.AreEqual("abcd", m1.Token);

            res = seq.MatchPassword("asdfabcdhujzyxwhgjj");
            Assert.AreEqual(2, res.Count());

            m1 = res.ElementAt(0);
            Assert.AreEqual(4, m1.i);
            Assert.AreEqual(7, m1.j);
            Assert.AreEqual("abcd", m1.Token);

            var m2 = res.ElementAt(1);
            Assert.AreEqual(11, m2.i);
            Assert.AreEqual(14, m2.j);
            Assert.AreEqual("zyxw", m2.Token);

            res = seq.MatchPassword("dfsjkhfjksdh");
            Assert.AreEqual(0, res.Count());
        }

        [TestMethod]
        public void DigitsRegexMatcher()
        {
            var re = new Zxcvbn.Matcher.RegexMatcher("\\d{3,}", 10);

            var res = re.MatchPassword("abc123def");
            Assert.AreEqual(1, res.Count());
            var m1 = res.First();
            Assert.AreEqual(3, m1.i);
            Assert.AreEqual(5, m1.j);
            Assert.AreEqual("123", m1.Token);

            res = re.MatchPassword("123456789a12345b1234567");
            Assert.AreEqual(3, res.Count());
            var m3 = res.ElementAt(2);
            Assert.AreEqual("1234567", m3.Token);

            res = re.MatchPassword("12");
            Assert.AreEqual(0, res.Count());

            res = re.MatchPassword("dfsdfdfhgjkdfngjl");
            Assert.AreEqual(0, res.Count());
        }

        [TestMethod]
        public void DateMatcher()
        {
            var dm = new Zxcvbn.Matcher.DateMatcher();

            var res = dm.MatchPassword("1297");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("98123");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("221099");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("352002");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("2011157");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("11222015");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("2013/06/1");
            Assert.AreEqual(2, res.Count()); // 2 since 2013 is a valid date without separators in its own right

            res = dm.MatchPassword("13-05-08");
            Assert.AreEqual(2, res.Count()); // 2 since prefix and suffix year sep matcher valid, so counts twice

            res = dm.MatchPassword("17 8 1992");
            Assert.AreEqual(3, res.Count()); // 3 since 1992 is a valid date without separators in its own right, and a partial match is valid prefix year

            res = dm.MatchPassword("10.16.16");
            Assert.AreEqual(1, res.Count());
        }

        [TestMethod]
        public void SpatialMatcher()
        {
            var sm = new Zxcvbn.Matcher.SpatialMatcher();

            var res = sm.MatchPassword("qwert");
            Assert.AreEqual(1, res.Count());
            var m1 = res.First();
            Assert.AreEqual("qwert", m1.Token);
            Assert.AreEqual(0, m1.i);
            Assert.AreEqual(4, m1.j);

            res = sm.MatchPassword("plko14569852pyfdb");
            Assert.AreEqual(6, res.Count()); // Multiple matches from different keyboard types
        }

        [TestMethod]
        public void BinomialTest()
        {
            Assert.AreEqual(1, Zxcvbn.PasswordScoring.Binomial(0, 0));
            Assert.AreEqual(1, Zxcvbn.PasswordScoring.Binomial(1, 0));
            Assert.AreEqual(0, Zxcvbn.PasswordScoring.Binomial(0, 1));
            Assert.AreEqual(1, Zxcvbn.PasswordScoring.Binomial(1, 1));
            Assert.AreEqual(56, Zxcvbn.PasswordScoring.Binomial(8, 3));
            Assert.AreEqual(2598960, Zxcvbn.PasswordScoring.Binomial(52, 5));
        }

        [TestMethod]
        public void DictionaryTest()
        {
            var dm = new Zxcvbn.Matcher.DictionaryMatcher("test", "test_dictionary.txt");

            var res = dm.MatchPassword("NotInDictionary");
            Assert.AreEqual(0, res.Count());

            res = dm.MatchPassword("choreography");
            Assert.AreEqual(1, res.Count());

            res = dm.MatchPassword("ChOrEograPHy");
            Assert.AreEqual(1, res.Count());


            var leet = new Zxcvbn.Matcher.L33tMatcher(dm);
            res = leet.MatchPassword("3mu");
            Assert.AreEqual(1, res.Count());

            res = leet.MatchPassword("3mupr4nce|egume");
        }

        [TestMethod]
        public void L33tTest()
        {
            var l = new Zxcvbn.Matcher.L33tMatcher(new Zxcvbn.Matcher.DictionaryMatcher("test", new List<string> {"password"}));

            l.MatchPassword("password");
            l.MatchPassword("p@ssword");
            l.MatchPassword("p1ssword");
            l.MatchPassword("p1!ssword");
            l.MatchPassword("p1!ssw0rd");
            l.MatchPassword("p1!ssw0rd|");
        }

        [TestMethod]
        public void EmptyPassword()
        {
            var res = Zxcvbn.Zxcvbn.MatchPassword("");
            Assert.AreEqual(0, res.Entropy);
        }

        [TestMethod]
        public void SinglePasswordTest()
        {
            var res = Zxcvbn.Zxcvbn.MatchPassword("||ke");
        }
    }
}

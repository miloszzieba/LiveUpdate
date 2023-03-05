using LiveUpdatePerformance.Models;
using Newtonsoft.Json.Linq;
using System;

namespace LiveUpdatePerformance.Services
{
    public class DataService
    {
        private Row[] _rows;

        public DataService()
        {
            _rows = InitRows();
        }

        public Row[] GetCurrentRows()
        {
            return this._rows;
        }

        public Row[] GetLatestChanges()
        {
            foreach (var row in _rows)
            {
                var valueChanged = _random.NextDouble() > 0.5;
                if (valueChanged)
                {
                    row.LastValueDate = DateTime.UtcNow;
                    var valueIncreased = _random.NextDouble() > 0.5;
                    if (valueIncreased)
                    {
                        row.LastValue = row.LowestSellValue;
                        row.LowestSellValue = Math.Floor(row.LastValue * 110) / 100;
                        row.LowestSellVolume = _random.Next(1, 100);
                    }
                    else
                    {
                        row.LastValue = row.HighestBuyValue;
                        row.HighestBuyValue = Math.Floor(row.LastValue * 90) / 100;
                        row.HighestBuyVolume = _random.Next(1, 100);
                    }
                }
                else
                {
                    var buyChanged = _random.NextDouble() > 0.5;
                    var sellChanged = _random.NextDouble() > 0.5;
                    if(buyChanged)
                    {
                        row.HighestBuyValue = Math.Min(Math.Floor(row.HighestBuyValue * 110) / 100, row.LastValue);
                        row.HighestBuyVolume = _random.Next(1, 100);
                    }
                    if(sellChanged)
                    {
                        row.LowestSellValue = Math.Max(Math.Floor(row.LowestSellValue * 90) / 100, row.LastValue);
                        row.LowestSellVolume = _random.Next(1, 100);
                    }
                }
            }

            return this._rows;
        }

        private static Row[] InitRows()
        {
            return Enumerable.Range(1, 500).Select(x =>
            {
                var value = new decimal(_random.Next(0, 100)) + (decimal)(_random.Next(0,100)) / 100;
                return new Row()
                {
                    Id = x,
                    Ticker = RandomString(_random.Next(1, 5)),
                    LastValue = value,
                    LastValueDate = DateTime.UtcNow,
                    HighestBuyValue = Math.Floor(value * 90) / 100,
                    HighestBuyVolume = _random.Next(1, 100),
                    LowestSellValue = Math.Floor(value * 110) / 100,
                    LowestSellVolume = _random.Next(1, 100),
                };
            }).ToArray();
        }

        private static Random _random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Range(1, length).Select(_ => chars[_random.Next(chars.Length)]).ToArray());
        }
    }
}

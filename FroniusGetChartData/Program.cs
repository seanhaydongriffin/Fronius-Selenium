using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Toolkit;
using Toolkit.Selenium;

namespace FroniusGetChartData
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFilename = $"{Assembly.GetCallingAssembly().GetName().Name}.json";

            string username = "seangriffin@internode.on.net";
            string password = "";

            double controlledLoad1Rate = 0.19459;
            double dailyChargeRate = 1.14818;
            double dailyChargeControlledLoad1Rate = 0.02915;
            double standardSolarRate = 0.08;
            double peakRate = 0.29733;

            // period
            var startDate = new System.DateTime(2024, 5, 14);
            var endDate = Toolkit.DateTime.Now();

            string report = "";
            
            Console.Clear();
            Log.Initialise();

            //if (args.Length < 1 || args.Contains("/?", StringComparer.OrdinalIgnoreCase) || args.Contains("/help", StringComparer.OrdinalIgnoreCase) || args.Contains("/h", StringComparer.OrdinalIgnoreCase))
            //{
            //    Log.WriteLine("-------------------------------------------------------------------------------");
            //    Log.WriteLine("   FroniusGetChartData");
            //    Log.WriteLine("-------------------------------------------------------------------------------");
            //    Log.WriteLine("");
            //    Log.WriteLine("   Gets chart data from Fronius Solar Web generates a chart page.");
            //    Log.WriteLine("");
            //    Log.WriteLine("              Usage      :: FroniusGetChartData [options]");
            //    Log.WriteLine("");
            //    Log.WriteLine("::");
            //    Log.WriteLine(":: options :");
            //    Log.WriteLine("::");
            //    Log.WriteLine("");
            //    Log.WriteLine("/config:filename         :: the config filename to use.");
            //    Log.WriteLine("                            Optional.");
            //    Log.WriteLine("");

            //    //Log.WriteLine("\nPress any key to continue...");
            //    //Console.ReadKey();

            //    Environment.Exit(0);
            //}


            // data load

            List<DateValues> dailyData = new List<DateValues>();

            if (File.exists(dataFilename))
            {
                dailyData = JSON.FromFile<List<DateValues>>(dataFilename);
            }

            // Browser Login

            Log.WriteLine("visiting the Solar Web login page.");

            BrowserTestObject.start(
                "",
                true,  // headless
                "latest",
                "windows",
                "8.1",
                "1600x1200",
                "https://www.solarweb.com/Account/ExternalLogin",
                false,
                ""
            );

            Log.WriteLine("logging in.");
            UI.Fronius.Login.Login.testMain(username, password);

            var dates = GetDateRange(startDate, endDate);

            foreach (System.DateTime date in dates)
            {
                // get data only if it does not already exist from the data file or it's today's date

                var matchingDailyData = dailyData.Where(e => e.Date == date).FirstOrDefault();

                if (matchingDailyData.Equals(null) || date.Equals(Toolkit.DateTime.NowDate()))
                {
                    dailyData.Remove(matchingDailyData);

                    Log.WriteLine($"visiting the chart for {date.Day}/{date.Month}/{date.Year}.");

                    BrowserTestObject.CurrentDriver.Navigate().GoToUrl($"https://www.solarweb.com/Chart/GetChartNew?pvSystemId=3dddea18-0c48-4618-a47c-3b4e8704cca8&year={date.Year}&month={date.Month}&day={date.Day}&interval=day&view=production");
                    Log.WriteLine("getting the chart data.");
                    var jsonStr = UI.Fronius.Chart.GetChart.testMain();
                    var json = JSON.Deserialize(jsonStr);
                    //File.overwrite("C:\\dwn\\fronius_chart.json", jsonStr);
                    //var json = JSON.FromFile("C:\\dwn\\fronius_chart.json");

                    // Energy export - Power to grid

                    var powerToGridData = json["settings"]["series"][0]["data"];

                    double powerToGridDataDailyEnergy = 0.0;
                    double powerToGridDataDailyEarnings = 0.0;

                    foreach (var dataPart in powerToGridData)
                    {
                        double energy = (double)dataPart[1] * ((double)5 / (double)60) / (double)1000;
                        double earnings = energy * standardSolarRate;

                        powerToGridDataDailyEnergy += energy;
                        powerToGridDataDailyEarnings += earnings;
                    }

                    // Energy import - Consumption

                    // consumed from all sources
                    var consumptionData = json["settings"]["series"][1]["data"];

                    // consumed from solar only
                    var consumedDirectlyData = json["settings"]["series"][2]["data"];

                    double consumptionEnergyTotal = 0.0;
                    double consumedDirectlyEnergyTotal = 0.0;
                    double consumedFromGridEnergyTotal = 0.0;
                    double consumedFromGridPriceTotal = 0.0;
                    double controlledLoadQty = 0.0;
                    double peakQty = 0.0;

                    for (int i = 0; i < ((JArray)consumptionData).Count; i++)
                    {
                        long consumptionTime = (long)consumptionData[i][0];
                        double consumptionEnergy = (double)consumptionData[i][1];
                        double consumedDirectlyEnergy = (double)consumedDirectlyData[i][1];
                        double consumedFromGridEnergy = (consumptionEnergy - consumedDirectlyEnergy) * ((double)5 / (double)60) / (double)1000;
                        double consumedFromGridPrice;

                        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(consumptionTime);
                        System.DateTime localConsumptionDateTime = dateTimeOffset.ToLocalTime().DateTime;
                        System.DateTime peakStart = new System.DateTime(localConsumptionDateTime.Year, localConsumptionDateTime.Month, localConsumptionDateTime.Day, 16, 0, 0);
                        System.DateTime peakEnd = new System.DateTime(localConsumptionDateTime.Year, localConsumptionDateTime.Month, localConsumptionDateTime.Day, 20, 0, 0);

                        if (localConsumptionDateTime < peakStart || localConsumptionDateTime > peakEnd)
                        {
                            consumedFromGridPrice = consumedFromGridEnergy * controlledLoad1Rate;
                            controlledLoadQty += consumedFromGridEnergy;
                        }
                        else
                        {
                            consumedFromGridPrice = consumedFromGridEnergy * peakRate;
                            peakQty += consumedFromGridEnergy;
                        }

                        consumptionEnergyTotal += consumptionEnergy;
                        consumedDirectlyEnergyTotal += consumedDirectlyEnergy;
                        consumedFromGridEnergyTotal += consumedFromGridEnergy;
                        consumedFromGridPriceTotal += consumedFromGridPrice;
                    }

                    consumedFromGridPriceTotal += dailyChargeRate + dailyChargeControlledLoad1Rate;

                    double profitLossTotal = powerToGridDataDailyEarnings - consumedFromGridPriceTotal;

                    // report

                    string profitLossMsg;

                    if (profitLossTotal < 0)

                        profitLossMsg = $"loss of ${Math.Abs(profitLossTotal).ToString("F2")}";
                    else

                        profitLossMsg = $"profit of ${Math.Abs(profitLossTotal).ToString("F2")}";

                    report += Environment.NewLine + $"{date.Day}/{date.Month}/{date.Year} - energy to grid = {powerToGridDataDailyEnergy} kWh / ${powerToGridDataDailyEarnings.ToString("F2")}, energy from grid = {consumedFromGridEnergyTotal} kWh / ${consumedFromGridPriceTotal.ToString("F2")}, {profitLossMsg}";

                    // json

                    dailyData.Add(
                        new DateValues
                        {
                            Date = date,
                            dailyEarnings = profitLossTotal,
                            totalEarnings = dailyData.Count == 0 ? profitLossTotal : dailyData.Last().totalEarnings + profitLossTotal,
                            controlledLoad = controlledLoadQty,
                            solar = powerToGridDataDailyEnergy,
                            peak = peakQty
                        }
                    );
                } else

                    Log.WriteLine($"already have the chart for {date.Day}/{date.Month}/{date.Year}, skipped.");
            }

            Log.WriteLine("Done.");
            BrowserTestObject.CloseAll();
            Log.WriteLine(report);


            // json

            var jsonString = dailyData.ToJSONString();
            File.overwrite(dataFilename, jsonString);

            // chart

            var chartLabels = dailyData.Select(s => s.Date.ToString("dd MMM")).ToList();
            var dailyEarningsPerDay = dailyData.Select(s => s.dailyEarnings).ToList();
            var totalEarningsPerDay = dailyData.Select(s => s.totalEarnings).ToList();
            var controlledLoadQtyPerPeriod = dailyData.Sum(d => d.controlledLoad);
            var standardSolarQtyPerPeriod = dailyData.Sum(d => d.solar);
            var peakQtyPerPeriod = dailyData.Sum(d => d.peak);

            var totalCharges = (controlledLoadQtyPerPeriod * controlledLoad1Rate) + (dates.Count * dailyChargeRate) + (dates.Count * dailyChargeControlledLoad1Rate) - (standardSolarQtyPerPeriod * standardSolarRate) + (peakQtyPerPeriod * peakRate);
            var totalChargesDrCr = totalCharges >= 0 ? "dr" : "cr";
            var indexHtml = $@"<html>

<head>
<style>
table, th, td {{
  font-family: arial, sans-serif;
  padding: 8px;
}}
td:first-child, th:first-child {{
  text-align: left;
}}
td:nth-child(n+2), th:nth-child(n+2) {{
  text-align: right;
}}
.header-row {{
  background-color: #D6EEEE;
}}
</style>
</head>

<h2>My Electricity</h2>

<h3>Earnings</h3>

<div style=""height: 300px"">
  <canvas id=""myChart""></canvas>
</div>

<h3>Charges</h3>

<div>
  <table>
    <tr class=""header-row""><th>Charge</th><th>Quantity</th><th>Rate</th><th>Total</th><th></th></tr>
    <tr><td>Controlled Load 1</td><td>{controlledLoadQtyPerPeriod:F3} kWh</td><td>$ {controlledLoad1Rate}</td><td>$ {controlledLoadQtyPerPeriod * controlledLoad1Rate:F2}</td><td>dr</td></tr>
    <tr><td>Daily Charge</td><td>{dates.Count} days</td><td>$ {dailyChargeRate}</td><td>$ {dates.Count * dailyChargeRate:F2}</td><td>dr</td></tr>
    <tr><td>Daily Charge - Controlled Load 1</td><td>{dates.Count} days</td><td>$ {dailyChargeControlledLoad1Rate}</td><td>$ {dates.Count * dailyChargeControlledLoad1Rate:F2}</td><td>dr</td></tr>
    <tr><td>Standard Solar</td><td>{standardSolarQtyPerPeriod:F3} kWh</td><td>-$ {standardSolarRate}</td><td>$ {standardSolarQtyPerPeriod * standardSolarRate:F2}</td><td>cr</td></tr>
    <tr><td>Peak</td><td>{peakQtyPerPeriod:F3} kWh</td><td>$ {peakRate}</td><td>$ {peakQtyPerPeriod * peakRate:F2}</td><td>dr</td></tr>
    <tr class=""header-row""><td><b>Total Charges</b></td><td></td><td></td><td><b>$ {totalCharges:F2}</b></td><td>{totalChargesDrCr}</td></tr>
  </table>
</div>

<script src=""https://cdn.jsdelivr.net/npm/chart.js""></script>

<script>
  const ctx = document.getElementById('myChart');

  new Chart(ctx, {{
    type: 'bar',
    data: {{
      labels: {chartLabels.ToJSONString().Replace("\"", "'")},
      datasets: [{{
        label: 'Daily Earnings',
        data: {dailyEarningsPerDay.ToJSONString()},
        borderWidth: 1
      }},{{
        label: 'Total Earnings',
        data: {totalEarningsPerDay.ToJSONString()},
        borderWidth: 1,
        type: 'line'
      }}]
    }},
    options: {{
      scales: {{
        y: {{
          beginAtZero: true,
          ticks: {{
            callback: function(value, index, values) {{
              return '$ ' + value.toFixed(2);
            }}
          }}
        }}
      }},
      plugins: {{
        tooltip: {{
          callbacks: {{
            label: function(context) {{
              return context.parsed.y >= 0 ? '$ ' + Math.abs(context.parsed.y).toFixed(2) + ' cr' : '$ ' + Math.abs(context.parsed.y).toFixed(2) + ' dr';
            }}
          }}
        }}
      }}
    }}
  }});
</script>
</html>";

            File.overwrite("electricity.html", indexHtml);


        }

        public class DateValues
        {
            public System.DateTime Date { get; set; }
            public double dailyEarnings { get; set; }
            public double totalEarnings { get; set; }
            public double controlledLoad { get; set; }
            public double solar { get; set; }
            public double peak { get; set; }
        }

        public static List<System.DateTime> GetDateRange(System.DateTime startDate, System.DateTime endDate)
        {
            List<System.DateTime> dateList = new List<System.DateTime>();

            for (System.DateTime date = startDate; date <= endDate; date = date.AddDays(1))
            {
                dateList.Add(date);
            }

            return dateList;
        }
    }
}

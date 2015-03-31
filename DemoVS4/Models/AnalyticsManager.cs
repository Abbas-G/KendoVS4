using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;

using System.Xml.Linq;
using Google.GData.Analytics;
using Google.GData.Client;
using System.Collections.Generic;
using System.Text;

namespace Report.Web.Models
{
    public class AnalyticsManager
    {
        private string baseUrl = "https://www.google.com/analytics/feeds/data";
        private AnalyticsService asv = new AnalyticsService("gaExportAPI_acctSample_v2.0");
        private string GetSegment(string segment)
        {
            return "dynamic::ga:pagePath=@" + segment;
        }

        public string InitializeToken()
        {
            string token = string.Empty;
            try
            {
                string stUserName = "ecatalogues.analytics@gmail.com";
                string stPassword = "eCatAnalytics";
                string stToken = "ga:83972661";

                asv.setUserCredentials(stUserName, stPassword);
                token = stToken;

                //asv.setUserCredentials("fnn.vint@gmail.com", "Fnn.vint44");

                //AccountQuery query = new AccountQuery();
                //AccountFeed accountFeed = asv.Query(query);
                //AccountEntry entry = (AccountEntry)accountFeed.Entries[0];

                //token = entry.ProfileId.Value;
            }
            catch (Exception ex) { }
            return token;
        }

        public DataFeed GetReportData(string token, string segment, string startDate, string endDate, string sortBy)
        {
            DataFeed feed = null;
            try
            {

                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                query.Dimensions = "ga:eventCategory,ga:eventAction";
                query.Metrics = "ga:pageviews,ga:totalEvents,ga:uniqueEvents,ga:eventValue";
                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;
                query.Sort = "-ga:" + sortBy;

                feed = asv.Query(query);
            }
            catch { }
            return feed;
        }
        public DataFeed GetReportA(string token, string segment, string startDate, string endDate, string sortBy)
        {
            DataFeed feed = null;
            try
            {

                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                query.Dimensions = "ga:date";
                query.Metrics = "ga:totalEvents";
                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;
                query.Sort = "-ga:" + sortBy;

                feed = asv.Query(query);
            }
            catch { }
            return feed;
        }

        public AtomEntryCollection GetGraphData(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                //query.Dimensions = "ga:date";
                //query.Metrics = "ga:visits,ga:" + sortBy;

                // query.Metrics = "ga:pageviews";

                query.Dimensions = "ga:eventCategory,ga:eventAction,ga:eventLabel,ga:city";
                query.Metrics = "ga:pageviews,ga:totalEvents,ga:uniqueEvents,ga:eventValue";

                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;
        }

        public AtomEntryCollection GetGraphDataForChart(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                query.Dimensions = "ga:eventLabel,ga:city";
                query.Metrics = "ga:totalEvents,ga:uniqueEvents";
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;
        }
        public string GetGraphURL(AtomEntryCollection entries, string startDate, string endDate)
        {
            Decimal maxValue = 0;
            List<decimal> actualDataPoints = new List<decimal>();
            List<string> xAxisDataPoints = new List<string>();

            AtomEntryCollection pointEntries = entries;

            foreach (DataEntry pointEntry in pointEntries)
            {
                actualDataPoints.Add(Convert.ToDecimal(pointEntry.Metrics[1].Value));

                if (actualDataPoints[actualDataPoints.Count - 1] > maxValue)
                    maxValue = actualDataPoints[actualDataPoints.Count - 1];
            }

            foreach (Decimal point in actualDataPoints)
            {
                decimal relativeValue = 0;
                if (point != 0) relativeValue = (point / maxValue * 100);
                xAxisDataPoints.Add(relativeValue.ToString("0.00"));
            }

            StringBuilder chartUrl = new StringBuilder("http://chart.apis.google.com/chart?");

            chartUrl.AppendFormat("cht={0}", "lc");
            chartUrl.AppendFormat("&chs={0}", "800x150");
            chartUrl.AppendFormat("&chd=t:{0}", String.Join(",", xAxisDataPoints.ToArray()));
            chartUrl.Append("&chxt=x,y,r");
            chartUrl.AppendFormat("&chxr=1,0,{0}", maxValue.ToString("0"));
            chartUrl.AppendFormat("|2,0,{0}", maxValue.ToString("0"));
            chartUrl.AppendFormat("&chxl=0:|{0}", String.Join("|", GetYAxisGraphLabel(startDate, endDate).ToArray()));
            chartUrl.Append("&chco=0077CC");
            chartUrl.Append("&chm=B,F5FAFD,0,0,0");
            chartUrl.Append("&chls=5");
            chartUrl.Append("&chg=20,50,1,5");

            return chartUrl.ToString();
        }
        public List<AxisPoint> GetGraphDataforAction(AtomEntryCollection entries, string startDate, string endDate)
        {

            List<AxisPoint> itemList = new List<AxisPoint>();

            AtomEntryCollection pointEntries = entries;

            foreach (DataEntry pointEntry in pointEntries)
            {
                if (pointEntry.Dimensions[1].Value.ToString() != "Surat")
                {
                AxisPoint item = new AxisPoint();

                item.X = pointEntry.Dimensions[0].Value;
                item.Y = pointEntry.Metrics[0].Value;
                item.Y1 = pointEntry.Metrics[1].Value;
                itemList.Add(item);
                }
            }

            return itemList;
        }

        public AtomEntryCollection GetReportDataforCountry(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                //query.Dimensions = "ga:date";
                //query.Metrics = "ga:visits,ga:" + sortBy;

                // query.Metrics = "ga:pageviews";

                //query.Dimensions = "ga:date,ga:country,ga:city,ga:eventCategory";
                query.Dimensions = "ga:date,ga:country,ga:city,ga:eventAction,ga:eventCategory";
                query.Metrics = "ga:totalEvents";//ga:visits for session

                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;
            
        }
        public List<AxisPoint2> GetGraphDataforCountry(AtomEntryCollection entriesCountry, string startDate, string endDate)
        {
           
            List<AxisPoint2> itemList = new List<AxisPoint2>();

            AtomEntryCollection pointEntries = entriesCountry;
            string sd = startDate;
            foreach (DataEntry pointEntry in pointEntries)
            {
                if (pointEntry.Dimensions[2].Value.ToString() != "Surat")
                {
                    AxisPoint2 item = new AxisPoint2();

                    //item.X = pointEntry.Dimensions[3].Value;
                    item.Y = Convert.ToInt32(pointEntry.Metrics[0].Value);
                    item.Date = pointEntry.Dimensions[0].Value;
                    //string a = Convert.ToInt32(item.Date).ToString("####/##/##");
                    itemList.Add(item);
                }
            }
            
            var list = itemList.GroupBy(i => i.Date)
                    .Select(g => new { Date = g.Key,
                                       Y = g.Sum(i => i.Y)
                    });

            itemList = new List<AxisPoint2>();
            foreach (var m  in list)
            {
                AxisPoint2 item = new AxisPoint2();

                item.Y = m.Y;
                item.Date = m.Date;
                //string a = Convert.ToInt32(item.Date).ToString("####/##/##");
                itemList.Add(item);
            }

            return itemList;
        }



        public AtomEntryCollection GetReportDataDocCount(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                //query.Dimensions = "ga:date";
                //query.Metrics = "ga:visits,ga:" + sortBy;

                // query.Metrics = "ga:pageviews";

                //query.Dimensions = "ga:eventCategory";
                query.Dimensions = "ga:eventAction,ga:eventLabel,ga:eventCategory,ga:city";
                query.Metrics = "ga:totalEvents,ga:timeOnPage,ga:timeOnScreen,ga:timeOnSite";

                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;

        }
        public AtomEntryCollection GetReportDataDocCountForChart(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                //query.Dimensions = "ga:date";
                //query.Metrics = "ga:visits,ga:" + sortBy;

                // query.Metrics = "ga:pageviews";

                //query.Dimensions = "ga:eventCategory";
                query.Dimensions = "ga:eventAction";
                query.Metrics = "ga:totalEvents";

                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;

        }
        public List<AxisPoint> GetGraphDataforDocCount(AtomEntryCollection entriesDocCount, string startDate, string endDate)
        {

            List<AxisPoint> itemList = new List<AxisPoint>();

            AtomEntryCollection pointEntries = entriesDocCount;

            foreach (DataEntry pointEntry in pointEntries)
            {
               // if(pointEntry.Dimensions[1].Value.ToString() != "Surat"){
                AxisPoint item = new AxisPoint();

                item.X = (pointEntry.Dimensions[0].Value.ToString().Length >= 20) ? pointEntry.Dimensions[0].Value.ToString().Substring(0, 17)+"..." : pointEntry.Dimensions[0].Value.ToString();
                item.Y = pointEntry.Metrics[0].Value;
                //item.Date = pointEntry.Metrics[3].Value; //date variable is used to store timespan
                itemList.Add(item);
               // }
            }

            return itemList;
        }

        public AtomEntryCollection GetReportDataDiveInfoCount(string token, string segment, string startDate, string endDate, string sortBy)
        {
            AtomEntryCollection entries = null;
            try
            {
                DataQuery query = new DataQuery(baseUrl);
                query.Ids = token;
                //query.Dimensions = "ga:date";
                //query.Metrics = "ga:visits,ga:" + sortBy;

                // query.Metrics = "ga:pageviews";

                query.Dimensions = "ga:mobileDeviceInfo,ga:appVersion";
                query.Metrics = "ga:totalEvents";

                //query.Segment = GetSegment(segment);
                query.GAStartDate = startDate;
                query.GAEndDate = endDate;

                DataFeed feed = asv.Query(query);
                entries = feed.Entries;
            }
            catch { }
            return entries;

        }
        public List<AxisPoint> GetGraphDataforDiveInfoCountt(AtomEntryCollection entriesDiveInfoCount, string startDate, string endDate)
        {

            List<AxisPoint> itemList = new List<AxisPoint>();

            AtomEntryCollection pointEntries = entriesDiveInfoCount;

            foreach (DataEntry pointEntry in pointEntries)
            {
                //if (pointEntry.Dimensions[2].Value.ToString() != "Surat")
                //{
                    AxisPoint item = new AxisPoint();

                    item.X = pointEntry.Dimensions[0].Value + " " + pointEntry.Dimensions[1].Value;
                    item.Y = pointEntry.Metrics[0].Value;
                    item.Date = "";
                    itemList.Add(item);
                //}
            }

            return itemList;
        }

        private List<string> GetYAxisGraphLabel(string startDate, string endDate)
        {
            List<string> yAxisLabels = new List<string>();

            DateTime sDate = Convert.ToDateTime(startDate);
            DateTime eDate = Convert.ToDateTime(endDate);

            yAxisLabels.Add(sDate.ToString("MMM dd"));
            sDate = sDate.AddDays(-1);

            while (sDate <= eDate)
            {
                sDate = sDate.AddDays(7);
                if (sDate <= eDate)
                    yAxisLabels.Add(sDate.ToString("MMM dd"));
                else
                    yAxisLabels.Add(eDate.ToString("MMM dd"));
            }

            return yAxisLabels;
        }
    }

    public class AxisPoint {
        public string X { get;set; }
        public string Y { get; set; }
        public string Y1 { get; set; }
        public string Date { get; set; }
    }

    public class AxisPoint2
    {
        public int Y { get; set; }
        public string Date { get; set; }
    }
}

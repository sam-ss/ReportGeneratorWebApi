using Newtonsoft.Json.Linq;
using Selenium.Axe;
using System;


namespace ReportGeneratorWebApi.Models
{
    public class AxeResultModel
    {
        public AxeResultItem[] Violations { get; }
        public AxeResultItem[] Passes { get; }
        public AxeResultItem[] Inapplicable { get; }
        public AxeResultItem[] Incomplete { get; }
        public DateTimeOffset? Timestamp { get; }
        public string Url { get; }
        public string Error { get; }
    }
}

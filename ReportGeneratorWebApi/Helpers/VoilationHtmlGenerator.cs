using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReportGeneratorWebApi.Models;
using Selenium.Axe;

namespace ReportGeneratorWebApi.Helpers
{
    public class VoilationHtmlGenerator
    {
        public static string GenerateHtml(Dictionary<string, AxeResultModel> pagesDetails, string FilePath = null)
        {
            string FolderLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string path = FolderLocation + "\\Voaliation.html";

            //string path = FilePath;// FolderLocation + "\\Voaliation.html"; 

            try
            {
                using (FileStream fs = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(ConvertDataTableToHtml(pagesDetails));

                    fs.Write(info, 0, info.Length);
                }
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    while ((s = sr.ReadLine()) != null)
                    {
                        Console.WriteLine(s);
                    }
                }
                return path;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
                return null;
            }
        }
        public static string ConvertDataTableToHtml(Dictionary<string, AxeResultModel> pagesDetails)
        {
            try
            {
                string htmlString = "";
                StringBuilder jsonItems = new StringBuilder();
                //Dictionary<string, AxeResult> pagesDetails = new Dictionary<string, AxeResult>();
                //pagesDetails.Add("https://www.amazon.in/gp/bestsellers/?ref_=nav_cs_bestsellers", axeResult);
                //pagesDetails.Add("https://www.google.com/", axeResult);


                string pageMapDetail = getPageMapping(pagesDetails);

                StringBuilder htmlBuilder = new StringBuilder();
                htmlBuilder.Append("<html>");
                htmlBuilder.Append("<head>");
                htmlBuilder.Append("<title>");
                htmlBuilder.Append("</title>");
                htmlBuilder.Append(@"
                      <style>
                                .accordion33{

	                              width:1100px;
                                }
                      </style>
                  ");
                htmlBuilder.Append("</head>");
                htmlBuilder.Append("<body>");

                htmlBuilder.Append(@"
                <nav class='navbar navbar-expand-md navbar-dark fixed-top bg-dark'>
                    <img src='../../../Framework/Common/Config/kibologo.png' /img>
                    < span class='report-name'>  Test Automation Report  </ span >
               </nav>
                
                  <main role='main'>
                      <div class='container'>        
                            <div class='row'>
                        
                            
            ");


                var mainHTMLBody = getMainHtml(pagesDetails);

                var includedScript = @"
                                     <link rel='canonical' href='https://getbootstrap.com/docs/4.1/examples/jumbotron/'>
                                     <link href = 'https://getbootstrap.com/docs/4.1/dist/css/bootstrap.min.css' rel = 'stylesheet' >  
                                     <link href = 'https://getbootstrap.com/docs/4.1/examples/jumbotron/jumbotron.css' rel = 'stylesheet' >   
                                     <script src = 'https://code.jquery.com/jquery-3.1.1.min.js' ></script>
                                    <script src = 'https://code.jquery.com/ui/1.12.1/jquery-ui.min.js' ></script>
                                    <link href = 'https://code.jquery.com/ui/1.12.1/themes/smoothness/jquery-ui.css' rel = 'stylesheet'>   
                        
                                   ";


                jsonItems = new StringBuilder();

                jsonItems.Append("[");
                foreach (var page in pagesDetails)
                {
                    jsonItems.Append("{");
                    jsonItems.Append("pageName:" + "'" + page.Key + "'" + ",");

                    //add violation json details.. 
                    jsonItems.Append("violations: [");
                    foreach (var item in page.Value.Violations)
                    {
                        jsonItems.Append("{");

                        jsonItems.Append("id:" + "'" + item.Id + "'" + "," +
                                         "description:" + "'" + item.Description + "'" + "," +
                                         "help:" + "'" + item.Help + "'" + "," +
                                         "helpUrl:" + "'" + item.HelpUrl + "'" + "," +
                                         "impact:" + "'" + item.Impact + "'" + "," +
                                         "tags:[" + getTagsDetails(item.Tags) + @"]");

                        jsonItems.Append("}");
                        jsonItems.Append(",");

                    }
                    jsonItems.Append("]");
                    jsonItems.Append(",");

                    //add inapplicables json details..
                    jsonItems.Append("inApplicables: [");
                    foreach (var item in page.Value.Inapplicable)
                    {
                        jsonItems.Append("{");

                        jsonItems.Append("id:" + "'" + item.Id + "'" + "," +
                                         "description:" + "'" + item.Description.Replace("'", string.Empty) + "'" + "," +
                                         "help:" + "'" + item.Help.Replace("'", string.Empty) + "'" + "," +
                                         "helpUrl:" + "'" + item.HelpUrl.Replace("'", string.Empty) + "'" + "," +
                                         "impact:" + "'" + item.Impact + "'" + "," +
                                         "tags:[" + getTagsDetails(item.Tags) + @"]");

                        jsonItems.Append("}");
                        jsonItems.Append(",");

                    }
                    jsonItems.Append("]");
                    jsonItems.Append(",");

                    //add incomplete json details..
                    jsonItems.Append("inComplete: [");
                    foreach (var item in page.Value.Incomplete)
                    {
                        jsonItems.Append("{");

                        jsonItems.Append("id:" + "'" + item.Id + "'" + "," +
                                         "description:" + "'" + item.Description.Replace("'", string.Empty) + "'" + "," +
                                         "help:" + "'" + item.Help.Replace("'", string.Empty) + "'" + "," +
                                         "helpUrl:" + "'" + item.HelpUrl.Replace("'", string.Empty) + "'" + "," +
                                         "impact:" + "'" + item.Impact + "'" + "," +
                                         "tags:[" + getTagsDetails(item.Tags) + @"]");

                        jsonItems.Append("}");
                        jsonItems.Append(",");

                    }
                    jsonItems.Append("]");
                    jsonItems.Append(",");

                    //add passes json details..
                    jsonItems.Append("passes: [");
                    foreach (var item in page.Value.Passes)
                    {
                        jsonItems.Append("{");

                        jsonItems.Append("id:" + "'" + item.Id + "'" + "," +
                                         "description:" + "'" + item.Description.Replace("'", string.Empty) + "'" + "," +
                                         "help:" + "'" + item.Help.Replace("'", string.Empty) + "'" + "," +
                                         "helpUrl:" + "'" + item.HelpUrl.Replace("'", string.Empty) + "'" + "," +
                                         "impact:" + "'" + item.Impact + "'" + "," +
                                         "tags:[" + getTagsDetails(item.Tags) + @"]");

                        jsonItems.Append("}");
                        jsonItems.Append(",");

                    }
                    jsonItems.Append("]");

                    jsonItems.Append("}");
                    jsonItems.Append(",");
                }
                jsonItems.Append("]");

                var str = Convert.ToString(jsonItems);
                var documentReady = @"
                                        <script>

                                                
                                                $(document).ready(function() {
                                                  
                                                       var pageDetails = " + pageMapDetail + @"                                                 
                                                       var violationData=" + str + @"
                                                        $( '.accordion33' ).accordion({                                                           
                                                            collapsible: true,
                                                            active:false,
                                                            heightStyle: 'content'                                                                
                                                        });    

                                                        //alert(JSON.stringify(violationData));
                                                       
	                                                for(var j=0; j< pageDetails.length; j++){
														
															var pageUniqueID= pageDetails[j].uniqueID;
															var pageName= pageDetails[j].pageName;
															var violation= violationData.find(x => x.pageName === pageName).violations;
                                                            var inApplicables = violationData.find(x => x.pageName === pageName).inApplicables;
                                                            var inCompletes = violationData.find(x => x.pageName === pageName).inComplete;
                                                            var passes = violationData.find(x => x.pageName === pageName).passes;
															getViolationDetailsForPage(violation,pageUniqueID);	
                                                            getInApplicaleDetailsForPage(inApplicables,pageUniqueID);
                                                            getInCompletsDetailsForPage(inCompletes,pageUniqueID);	
                                                            getPassesDetailsForPage(passes,pageUniqueID);		
														}

                                                       	$('.badge').addClass('badge-pill badge-info');
														$('.badge').css('margin-left', '5px')

                                                });


                                                function htmlEntities(str) {
                                                         return String(str).replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g,'&gt;');
                                                 }

                                                //For setting up page violations ..
			                                   function getViolationDetailsForPage(violationData,pageName){
													
												   for(var i=0; i<violationData.length; i++){
															var trData=`
                                                                    <tr>   
                                                                      <th scope ='row'>1</th>    
                                                                      <td> ViolationID </td> <td class='baseHtml'>`+ violationData[i].id+ `</td>     
                                                                    </tr>  
                                                                    <tr>   
                                                                      <th scope = 'row' >2</th>    
                                                                      <td> Description </td> <td class='baseHtml'>`+ htmlEntities(violationData[i].description) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >3</th>    
                                                                      <td> Help </td> <td class='baseHtml'>`+ htmlEntities(violationData[i].help) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >4</th>    
                                                                      <td> HelpUrl </td> <td class='baseHtml'>`+ htmlEntities(violationData[i].helpUrl) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >5</th>    
                                                                      <td> Impact </td> <td class='baseHtml'>`+ htmlEntities(violationData[i].impact) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >6</th>    
                                                                      <td> Tags </td> <td class='baseHtml'>`+ getTagsData(violationData[i].tags) +`</td>     
                                                                    </tr>
                                                             `;															 
															 
																$('.'+ pageName +'_violation_'+ violationData[i].id).append(trData);
													   }
												 }

                                                         function getTagsData(tagData){
															   var allTr='';
															         for(var i=0; i<tagData.length; i++){
																          allTr= allTr + '<span class='+ 'badge'+ '>'+ tagData[i] +'</span>';
                                                                     }
                                                               return allTr;
                                                          }

                                //For setting up page inapplicables..
                                function getInApplicaleDetailsForPage(inApplicableData,pageName){
													
												   for(var i=0; i< inApplicableData.length; i++){
															var trData=`
                                                                    <tr>   
                                                                      <th scope ='row'>1</th>    
                                                                      <td> ViolationID </td> <td class='baseHtml'>`+ inApplicableData[i].id+ `</td>     
                                                                    </tr>  
                                                                    <tr>   
                                                                      <th scope = 'row' >2</th>    
                                                                      <td> Description </td> <td class='baseHtml'>`+ htmlEntities(inApplicableData[i].description) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >3</th>    
                                                                      <td> Help </td> <td class='baseHtml'>`+ htmlEntities(inApplicableData[i].help) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >4</th>    
                                                                      <td> HelpUrl </td> <td class='baseHtml'>`+ htmlEntities(inApplicableData[i].helpUrl) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >5</th>    
                                                                      <td> Impact </td> <td class='baseHtml'>`+ htmlEntities(inApplicableData[i].impact) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >6</th>    
                                                                      <td> Tags </td> <td class='baseHtml'>`+ getTagsData(inApplicableData[i].tags) +`</td>     
                                                                    </tr>
                                                             `;															 
															 
																$('.'+ pageName +'_inapplicable_'+ inApplicableData[i].id).append(trData);
													   }
												 }

                                                    //For setting up page incompletes..
                                                 function getInCompletsDetailsForPage(inCompleteData,pageName){
													
												   for(var i=0; i< inCompleteData.length; i++){
															var trData=`
                                                                    <tr>   
                                                                      <th scope ='row'>1</th>    
                                                                      <td> ViolationID </td> <td class='baseHtml'>`+ inCompleteData[i].id+ `</td>     
                                                                    </tr>  
                                                                    <tr>   
                                                                      <th scope = 'row' >2</th>    
                                                                      <td> Description </td> <td class='baseHtml'>`+ htmlEntities(inCompleteData[i].description) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >3</th>    
                                                                      <td> Help </td> <td class='baseHtml'>`+ htmlEntities(inCompleteData[i].help) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >4</th>    
                                                                      <td> HelpUrl </td> <td class='baseHtml'>`+ htmlEntities(inCompleteData[i].helpUrl) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >5</th>    
                                                                      <td> Impact </td> <td class='baseHtml'>`+ inCompleteData[i].impact + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >6</th>    
                                                                      <td> Tags </td> <td class='baseHtml'>`+ getTagsData(inCompleteData[i].tags) +`</td>     
                                                                    </tr>
                                                             `;															 
															 
																$('.'+ pageName +'_incomplete_'+ inCompleteData[i].id).append(trData);
													   }
												 }

                                                 //For setting up page passes..
                                                 function getPassesDetailsForPage(passes,pageName){													
												   for(var i=0; i< passes.length; i++){
															var trData=`
                                                                    <tr>   
                                                                      <th scope ='row'>1</th>    
                                                                      <td> ViolationID </td> <td class='baseHtml'>`+ passes[i].id+ `</td>     
                                                                    </tr>  
                                                                    <tr>   
                                                                      <th scope = 'row' >2</th>    
                                                                      <td> Description </td> <td class='baseHtml'>`+ htmlEntities(passes[i].description) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >3</th>    
                                                                      <td> Help </td> <td class='baseHtml'>`+ htmlEntities(passes[i].help) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >4</th>    
                                                                      <td> HelpUrl </td> <td class='baseHtml'>`+ htmlEntities(passes[i].helpUrl) + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >5</th>    
                                                                      <td> Impact </td> <td class='baseHtml'>`+ passes[i].impact + `</td>     
                                                                    </tr>
                                                                    <tr>   
                                                                      <th scope = 'row' >6</th>    
                                                                      <td> Tags </td> <td class='baseHtml'>`+ getTagsData(passes[i].tags) +`</td>     
                                                                    </tr>
                                                             `;														 
															 
																$('.'+ pageName +'_passes_'+ passes[i].id).append(trData);
													   }
												 }
                                                
                                        </script>
                                    ";

                htmlBuilder.Append(mainHTMLBody);
                htmlBuilder.Append(@"
                    </div>
                     <hr>
                     </div> 
                    </main>
                     <footer class='container'>
                          <p> &copy; Company 2019 - 2020 </p>
                     </footer >
                ");
                htmlBuilder.Append(includedScript);
                htmlBuilder.Append(documentReady);
                htmlBuilder.Append("</body>");
                htmlBuilder.Append("</html>");
                htmlString = htmlBuilder.ToString();

                return htmlString;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public static string getTagsDetails(string[] tags)
        {
            StringBuilder tagsBUilder = new StringBuilder();
            for (int i = 0; i < tags.Length; i++)
            {
                tagsBUilder.Append("'");
                tagsBUilder.Append(tags[i]);
                tagsBUilder.Append("'");
                tagsBUilder.Append(",");
            }

            return tagsBUilder.ToString();
        }
        public static string getMainHtml(Dictionary<string, AxeResultModel> axeData)
        {
            StringBuilder tagBuilder = new StringBuilder();
            tagBuilder.Append(getMainPageName(axeData));


            return @"
                                 <div id='accordionrr4' class='accordion33'>
                                  " + tagBuilder.ToString() + @"
                                 </div>
  
                        ";
        }

        /// <summary>
        /// Set html for each pages...
        /// </summary>
        /// <param name="axePageData"></param>
        /// <returns></returns>
        public static string getMainPageName(Dictionary<string, AxeResultModel> axePageData)
        {
            StringBuilder pageBuilder = new StringBuilder();
            int count = 0;
            foreach (var pageData in axePageData)
            {

                pageBuilder.Append(@"
                                      <h3>" + pageData.Key + @"</h3> 
                                      <div id='accordionData' class='accordion33'>" + getAllTabsForPage(pageData.Value.Violations, pageData.Value.Inapplicable, pageData.Value.Incomplete, pageData.Value.Passes, pageData.Value.Error, "pageNumber" + count)
                                     + @" </div>
                                  ");

                int temp = count + 1;
                count = temp;

            }

            return pageBuilder.ToString();
        }

        public static string getAllTabsForPage(AxeResultItem[] violations, AxeResultItem[] inApplicables, AxeResultItem[] inCompletes, AxeResultItem[] passes, string error, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();
            nestedTags.Append(getErrorDetails(error));
            nestedTags.Append(getInapplicableDetails(inApplicables, pageName));
            nestedTags.Append(getPassesDetails(passes, pageName));
            nestedTags.Append(getIncompleteDetails(inCompletes, pageName));
            nestedTags.Append(getViolationDetails(violations, pageName));
            return Convert.ToString(nestedTags);
        }

        public static string getErrorDetails(string error)
        {
            return (@"
                                      <h3> Errors </h3> 
                                      <div class='innerContent'>" + error + @" </div>
                  ");
        }

        #region Set InApplicable Details For Page
        public static string getInapplicableDetails(AxeResultItem[] inApplicables, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();
            return (@"
                                      <h3> InApplicable <span class='badge' style='background-color:grey;color:white;margin-left:5px;'>" + inApplicables.Length + @"</span></h3> 
                                      <div class='innerContent' style='height:400px;'>" + getIndividualInApplicables(inApplicables, pageName) + @"</div>
                  ");
        }

        public static string getIndividualInApplicables(AxeResultItem[] inApplicables, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();

            for (int i = 0; i < inApplicables.Length; i++)
            {
                string violatedData = @" 
                                        <div class='accordion33' style='width:950px;'>
                                          <h3>" + inApplicables[i].Id + @"</h3> 
                                          <div class='innerContent' style='height:400px;'>
                                            <table class='table table-bordered  table-hover'>
                                               <thead  class='thead-light'>
                                                    <tr>
                                                        <th scope = 'col' >#</th>
                                                        <th scope = 'col' > Category </th> 
                                                        <th scope = 'col' > Details </th>                                           
                                                    </tr>   
                                              </thead>  
                                              <tbody class='" + pageName + "_inapplicable_" + inApplicables[i].Id + "'> </tbody></table>" + @"

                                          </div>
                                        </div>
                                      ";
                nestedTags.Append(violatedData);
            }
            return Convert.ToString(nestedTags);
        }

        #endregion       

        #region Set Passes Details For Page...
        public static string getPassesDetails(AxeResultItem[] passes, string pageName)
        {
            return (@"
                                      <h3> Passes <span class='badge' style='background-color:grey;color:white;margin-left:5px;'>" + passes.Length + @" </span></h3> 
                                      <div class='innerContent' style='height:400px;'>" + getIndividualPasses(passes, pageName) + @" </div>
                  ");
        }

        public static string getIndividualPasses(AxeResultItem[] passes, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();

            for (int i = 0; i < passes.Length; i++)
            {
                string violatedData = @" 
                                        <div class='accordion33' style='width:950px;'>
                                          <h3>" + passes[i].Id + @"</h3> 
                                          <div class='innerContent' style='height:400px;'>
                                            <table class='table table-bordered  table-hover'>
                                               <thead  class='thead-light'>
                                                    <tr>
                                                        <th scope = 'col' >#</th>
                                                        <th scope = 'col' > Category </th> 
                                                        <th scope = 'col' > Details </th>                                           
                                                    </tr>   
                                              </thead>  
                                              <tbody class='" + pageName + "_passes_" + passes[i].Id + "'> </tbody></table>" + @"

                                          </div>
                                        </div>
                                      ";
                nestedTags.Append(violatedData);
            }
            return Convert.ToString(nestedTags);
        }

        #endregion

        #region Set Incomplete Details For Page...
        public static string getIncompleteDetails(AxeResultItem[] inCompletes, string pageName)
        {
            return (@"
                                      <h3> Incomplete <span class='badge' style='background-color:grey;color:white;margin-left:5px;'>" + inCompletes.Length + @"</span></h3> 
                                      <div class='innerContent' style='height:400px;'>" + getIndividualInComplete(inCompletes, pageName) + @" </div>
                  ");
        }

        public static string getIndividualInComplete(AxeResultItem[] inCompletes, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();

            for (int i = 0; i < inCompletes.Length; i++)
            {
                string violatedData = @" 
                                        <div class='accordion33' style='width:950px;'>
                                          <h3>" + inCompletes[i].Id + @"</h3> 
                                          <div class='innerContent' style='height:400px;'>
                                            <table class='table table-bordered  table-hover'>
                                               <thead  class='thead-light'>
                                                    <tr>
                                                        <th scope = 'col' >#</th>
                                                        <th scope = 'col' > Category </th> 
                                                        <th scope = 'col' > Details </th>                                           
                                                    </tr>   
                                              </thead>  
                                              <tbody class='" + pageName + "_incomplete_" + inCompletes[i].Id + "'> </tbody></table>" + @"

                                          </div>
                                        </div>
                                      ";
                nestedTags.Append(violatedData);
            }
            return Convert.ToString(nestedTags);
        }

        #endregion

        #region Set Violation Details For Page..
        public static string getViolationDetails(AxeResultItem[] violations, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();
            return (@"
                                      <h3> Violations <span class='badge' style='background-color:grey;color:white;margin-left:5px;'>" + violations.Length + @"</span></h3> 
                                      <div class='innerContent' style='height:400px;'>" + getIndividualViolations(violations, pageName) + @" </div>
                  ");
        }
        public static string getIndividualViolations(AxeResultItem[] violations, string pageName)
        {
            StringBuilder nestedTags = new StringBuilder();

            for (int i = 0; i < violations.Length; i++)
            {
                string violatedData = @" 
                                        <div class='accordion33' style='width:950px;'>
                                          <h3>" + violations[i].Id + @"</h3> 
                                          <div class='innerContent' style='height:400px;'>" +
                                           createTableForIndividualViolation(violations[i], pageName) + @"
                                          </div>
                                        </div>
                                      ";
                nestedTags.Append(violatedData);
            }
            return Convert.ToString(nestedTags);
        }
        public static string createTableForIndividualViolation(AxeResultItem violations, string pageName)
        {
            return @"                    
                                 <table class='table table-bordered  table-hover'>
                                    <thead  class='thead-light'>
                                        <tr>
                                            <th scope = 'col' >#</th>
                                            <th scope = 'col' > Category </th> 
                                            <th scope = 'col' > Details </th>                                           
                                        </tr>   
                                    </thead>   
                                    <tbody class='" + pageName + "_violation_" + violations.Id + "'> </tbody></table>";
        }

        #endregion

        public static string getPageMapping(Dictionary<string, AxeResultModel> pagesDetails)
        {
            StringBuilder jsonItems = new StringBuilder();
            int count = 0;
            jsonItems.Append("[");
            foreach (var item in pagesDetails)
            {
                jsonItems.Append("{");

                jsonItems.Append("pageName:" + "'" + item.Key + "'" + "," +
                                 "uniqueID:" + "'" + "pageNumber" + count + "'");

                jsonItems.Append("}");
                jsonItems.Append(",");

                count += 1;

            }
            jsonItems.Append("]");

            return Convert.ToString(jsonItems);
        }
    }
}

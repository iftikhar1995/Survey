
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using PCSW.Models;
using System.Configuration;
using System.IO;
using DAL;
using System.Text;

namespace PCSW.Controllers
{
    public class GMISController : Controller
    {

        public static List<SelectListItem> GetDropDownListForYears()
        {
            List<SelectListItem> ls = new List<SelectListItem>();

            int currYear = DateTime.Now.Year;
            for (int i = currYear ; i <= currYear+13 ; i++)
            {
                ls.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString() });
            }

            return ls;
        }

        // GET: GMIS
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Part1()
        {            
            return View();
        }

        public ActionResult Part2()
        {
            //@Html.DropDownList("yearPickerPart2" , (IEnumerable<SelectListItem>)ViewBag.Years , "Select Year" , new { @class = "dropdown-menu" })
            return View();
        }

        public ActionResult Part3()
        {

            //return View();
            return View();

        }

        public ActionResult Part3V2()
        {

            //return View();
            return View();

        }
        

        [HttpPost]
        public ActionResult DownloadProvincial()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Type,Departments,No.of Employees Male,No.of Employees Female,No.of Employees Male From 2012,No.of Employees Female From 2012,Gazetted Officer Male,Gazetted Officer Female,Gazetted Officer Male from 2012,Gazetted Officer Female from 2012,NonGazetted Officer Male,NonGazetted Officer Female,NonGazetted Officer Male from 2012,NonGazetted Officer Female from 2012,Contract Basis Male,Contract Basis Female,Contract Basis Male from 2012,Contract Basis Female from 2012,Female Washrooms,Female Prayer Rooms,Number of women appointed to whom age relaxation of up to 3 years was allowed,Number of women who availed maternity leave,Number of men who availed paternity leave,Number of Selection and Recruitment Committees for regular and contractual employment,Selection and Recruitment Committees for regular and contractual employment fulfilling the condition of at least one woman representative,Establishment of Gender Mainstreaming Committee,Code of Conduct Implemented under Punjab Protection Against Harassment of Women at Workplace Act 2012,Establishment of Workplace Harassment Committees,No. of Complaints Received,No. of Actions Taken,No. Of Boards,Name,Male,Female,No. Of Committee,Name,Male,Female,No. of Taskforce,Name,Male,Female,No. of Trainings for Boards,No. of Trainings for Committes,No. of Trainings for TaskForce\r\n");
            try
            {
                using (PCSWEntities entities = new PCSWEntities())
                {
                    var dataList = entities.ProvincialDatas.ToList();
                    List<ProvincialBoard> boardData = null;
                    List<ProvincialCommittee> comitteeData = null;
                    List<ProvincialTaskForce> taskforceData = null;
                    int maxCount = -1;

                    foreach (var d in dataList)
                    {
                        sb.Append(d.P_Type + ","+ d.Departments + ",");
                        sb.Append(d.NumEmployeeMale + "," + d.NumEmployeeFemale + "," + d.NumEmployeeMaleFrom2012 + "," + d.NumEmployeeFemaleFrom2012 + ",");
                        sb.Append(d.NumGazettedMale + "," + d.NumGazettedFemale + "," + d.NumGazettedMaleFrom2012 + "," + d.NumGazettedFemaleFrom2012 + ",");
                        sb.Append(d.NumNonGazettedMale + "," + d.NumNonGazettedFemale + "," + d.NumNonGazettedMaleFrom2012 + "," + d.NumNonGazettedFemaleFrom2012 + ",");
                        sb.Append(d.NumContractMale + "," + d.NumContractFemale + "," + d.NumContractMaleFrom2012 + "," + d.NumContractFemaleFrom2012 + ",");
                        sb.Append(d.NumWomenWashrooms + "," + d.NumWomenPrayerRooms + ",");
                        sb.Append(d.NumAgeRelaxation3 + "," + d.NumMaternityLeave + "," + d.NumPaternityLeave + ",");
                        sb.Append(d.NumSRCForRegular + "," + d.NumSRCForRegularWithOneWomen + ",");
                        sb.Append(d.GMCEstablishment + "," + d.COCImplementation + ",");
                        sb.Append(d.EstablishmentWHC + "," + d.NumComplaints + "," + d.NumActionsTaken + ","); /////////////////////

                        boardData = entities.ProvincialBoards.Where(pb => pb.P_Id == d.P_Id).ToList();
                        if (boardData != null && boardData.Count > 0)
                        {
                            sb.Append("1)" + "," + boardData[0].Name + "," + boardData[0].NumMale + "," + boardData[0].NumFemale + ",");
                        }
                        else
                        {
                            sb.Append("---,---,---,---,");
                        }

                        comitteeData = entities.ProvincialCommittees.Where(pc => pc.P_Id == d.P_Id).ToList();
                        if (comitteeData != null && comitteeData.Count > 0)
                        {
                            sb.Append("1)" + "," + comitteeData[0].Name + "," + comitteeData[0].NumMale + "," + comitteeData[0].NumFemale + ",");
                        }
                        else
                        {
                            sb.Append("---,---,---,---,");
                        }

                        taskforceData = entities.ProvincialTaskForces.Where(pt => pt.P_Id == d.P_Id).ToList();
                        if (taskforceData != null && taskforceData.Count > 0)
                        {
                            sb.Append("1)" + "," + taskforceData[0].Name + "," + taskforceData[0].NumMale + "," + taskforceData[0].NumFemale + ",");
                        }
                        else
                        {
                            sb.Append("---,---,---,---,");
                        }
                        sb.Append(d.NumBoardBCT + "," + d.NumCommitteeBCT + "," + d.NumTaskForceBCT);
                        sb.Append("\r\n");

                        maxCount = Math.Max(boardData.Count, Math.Max(comitteeData.Count, taskforceData.Count));
                        if (maxCount > 1)
                        {
                            for (int i = 1; i < maxCount; i++)
                            {
                                sb.Append(",,,,,,,,,,,,,,,,,,,,,,,,,,,,,,");
                                if (i < boardData.Count)
                                {
                                    sb.Append((i + 1).ToString() + ")," + boardData[i].Name + "," + boardData[i].NumMale + "," + boardData[i].NumFemale + ",");
                                }
                                else
                                {
                                    sb.Append(",,,,");
                                }

                                if (i < comitteeData.Count)
                                {
                                    sb.Append((i + 1).ToString() + ")," + comitteeData[i].Name + "," + comitteeData[i].NumMale + "," + comitteeData[i].NumFemale + ",");
                                }
                                else
                                {
                                    sb.Append(",,,,");
                                }

                                if (i < taskforceData.Count)
                                {
                                    sb.Append((i + 1).ToString() + ")," + taskforceData[i].Name + "," + taskforceData[i].NumMale + "," + taskforceData[i].NumFemale + ",");
                                }
                                else
                                {
                                    sb.Append(",,,,");
                                }
                                sb.Append(",,,\r\n");
                            }
                        }
                    }

                    StreamWriter file = new StreamWriter(Server.MapPath("~/Exports/ProvincialReport.csv"));
                    file.WriteLine(sb.ToString());
                    file.Close();

                    string path = Server.MapPath("~/Exports/ProvincialReport.csv");
                    return File(path, "text/csv", "ProvincialReport.csv");
                    
                }
            }
            catch (Exception e)
            {
                Response.Write(e);
                return RedirectToAction("ErrorView", "GMIS", null);
            }
        }

        [HttpPost]
        public ActionResult DownloadDistrict()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Type,District,Departments,No.of Employees Male,No.of Employees Female,No.of Employees Male From 2012,No.of Employees Female From 2012,Gazetted Officer Male,Gazetted Officer Female,Gazetted Officer Male from 2012,Gazetted Officer Female from 2012,NonGazetted Officer Male,NonGazetted Officer Female,NonGazetted Officer Male from 2012,NonGazetted Officer Female from 2012,Contract Basis Male,Contract Basis Female,Contract Basis Male from 2012,Contract Basis Female from 2012,Female Washrooms,Female Prayer Rooms,Number of women appointed to whom age relaxation of up to 3 years was allowed,Number of women who availed maternity leave,Number of men who availed paternity leave,Number of Selection and Recruitment Committees for regular and contractual employment,Selection and Recruitment Committees for regular and contractual employment fulfilling the condition of at least one woman representative,Establishment of Gender Mainstreaming Committee,Code of Conduct Implemented under Punjab Protection Against Harassment of Women at Workplace Act 2012,Establishment of Workplace Harassment Committees,No. of Complaints Received,No. of Actions Taken\r\n");
            try
            {
                using (PCSWEntities entities = new PCSWEntities())
                {
                    var dataList = entities.DistrictDatas.ToList();
                    
                    foreach (var d in dataList)
                    {
                        sb.Append(d.D_Type + "," + d.District + "," + d.Departments + ",");
                        sb.Append(d.NumEmployeeMale + "," + d.NumEmployeeFemale + "," + d.NumEmployeeMaleFrom2012 + "," + d.NumEmployeeFemaleFrom2012 + ",");
                        sb.Append(d.NumGazettedMale + "," + d.NumGazettedFemale + "," + d.NumGazettedMaleFrom2012 + "," + d.NumGazettedFemaleFrom2012 + ",");
                        sb.Append(d.NumNonGazettedMale + "," + d.NumNonGazettedFemale + "," + d.NumNonGazettedMaleFrom2012 + "," + d.NumNonGazettedFemaleFrom2012 + ",");
                        sb.Append(d.NumContractMale + "," + d.NumContractFemale + "," + d.NumContractMaleFrom2012 + "," + d.NumContractFemaleFrom2012 + ",");
                        sb.Append(d.NumWomenWashrooms + "," + d.NumWomenPrayerRooms + ",");
                        sb.Append(d.NumAgeRelaxation3 + "," + d.NumMaternityLeave + "," + d.NumPaternityLeave + ",");
                        sb.Append(d.NumSRCForRegular + "," + d.NumSRCForRegularWithOneWomen + ",");
                        sb.Append(d.GMCEstablishment + "," + d.COCImplementation + ",");
                        sb.Append(d.EstablishmentWHC + "," + d.NumComplaints + "," + d.NumActionsTaken);
                        sb.Append("\r\n");
                        
                    }

                    StreamWriter file = new StreamWriter(Server.MapPath("~/Exports/DistrictReport.csv"));
                    file.WriteLine(sb.ToString());
                    file.Close();

                    string path = Server.MapPath("~/Exports/DistrictReport.csv");
                    return File(path, "text/csv", "DistrictReport.csv");
                    
                }
            }
            catch (Exception e)
            {
                Response.Write(e);
                return RedirectToAction("ErrorView", "GMIS", null);
            }
        }

        //[HttpPost]
        //public ActionResult ExportToXLS(DataModel d)
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult SaveDistrict(DistrictData d)
        {
            //try
            //{
                using(PCSWEntities entites = new PCSWEntities())
                {
                    entites.DistrictDatas.Add(d);
                    entites.SaveChanges();   
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            //}
            //catch (Exception e)
            //{
                return RedirectToAction("ErrorView", "GMIS", null);
            //}

            
        }

        [HttpPost]
        public ActionResult SaveProvincial(DataModel d)
        {
            try
            {
                
                using (PCSWEntities entites = new PCSWEntities())
                {
                    entites.ProvincialDatas.Add(d.data);
                    entites.SaveChanges();

                    foreach(var board in d.boardData)
                    {
                        board.P_Id = d.data.P_Id;
                        entites.ProvincialBoards.Add(board);

                    }

                    entites.SaveChanges();

                    foreach (var committee in d.committeeData)
                    {
                        committee.P_Id = d.data.P_Id;
                        entites.ProvincialCommittees.Add(committee);
                    }

                    entites.SaveChanges();

                    foreach (var taskforce in d.taskforceData)
                    {
                        taskforce.P_Id = d.data.P_Id;
                        entites.ProvincialTaskForces.Add(taskforce);
                        
                    }
                    entites.SaveChanges();
                }

                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return RedirectToAction("ErrorView", "GMIS", null);
            }


        }

        public ActionResult DownloadReport()
        {
            return View();
        }
        
        public ActionResult ErrorView()
        {
            return View();
        }    
        
        public ActionResult SSNotSupported()
        {
            return View();
        }
    }
}
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace project_motorcycle
{
    /// <summary>
    /// DataHandler 的摘要描述
    /// </summary>
    public class DataHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //接到Web.config的ConnectionString
            string sAction = (context.Request.Form["action"] != null) ? context.Request.Form["action"] : context.Request["action"];
            switch (sAction)
            {
                case "forpepper":
                {
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sSql = @"select ""all""
                                        from tbl_info_list il
                                       GROUP BY ""all""";
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            //cmd.Parameters.AddWithValue("Vendor_Id", "%" + sVendor_Id + "%");
                            //cmd.Parameters.AddWithValue("Moto_Id", "%" + sMoto_Id + "%");
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();

                            //string aa = "";
                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    aa += dt.Rows[i]["MotoName"] + ";";
                            //}

                            conn.Close();
                            conn.Dispose();
                            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
                            context.Response.Write(strJSON);

                        }
                        catch (Exception ex)
                        {
                            string sResultJson = "";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            sResultJson = "{\"result\":\"error\"}";
                            context.Response.Write(sResultJson);
                        }
                        break;
                    }//傳給pepper要用聽覺接收的值
                case "GetMoto_NameList":
                    {
                        string sVendor_Id = (context.Request.Form["Vendor_Id"] != null) ? context.Request.Form["Vendor_Id"] : context.Request["Vendor_Id"];
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sSql = @"select * from tbl_info_list il where il.""Vendor_Id"" like @Vendor_Id";
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("Vendor_Id", "%" + sVendor_Id + "%");
                            //cmd.Parameters.AddWithValue("Moto_Id", "%" + sMoto_Id + "%");
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();

                            //string aa = "";
                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    aa += dt.Rows[i]["MotoName"] + ",";
                            //}

                            conn.Close();
                            conn.Dispose();
                            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
                            context.Response.Write(strJSON);

                        }
                        catch (Exception ex)
                        {
                            string sResultJson = "";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            sResultJson = "{\"result\":\"error\"}";
                            context.Response.Write(sResultJson);
                        }
                        break;
                    }//抓取機車名稱資訊
                case "GetCC_TypeList":
                    {
                        //string sCC_Type = (context.Request.Form["CC_Type"] != null) ? context.Request.Form["CC_Type"] : context.Request["CC_Type"];
                        string sVendor_Id = (context.Request.Form["Vendor_Id"] != null) ? context.Request.Form["Vendor_Id"] : context.Request["Vendor_Id"];
                        string sMoto_Id = (context.Request.Form["Moto_Id"] != null) ? context.Request.Form["Moto_Id"] : context.Request["Moto_Id"];
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);

                        string sqlwhere = "";
                        if (sMoto_Id == "" && sVendor_Id == "")
                        {
                            sqlwhere = @"where pl.""Moto_Id""like @Moto_IdL and il.""Vendor_Id"" like @Vendor_IdL";
                        }

                        else if (sMoto_Id != "" && sVendor_Id == "")
                        {
                            sqlwhere = @"where pl.""Moto_Id"" =@Moto_Id and il.""Vendor_Id"" like @Vendor_IdL";
                        }

                        else if (sMoto_Id != "" && sVendor_Id != "")
                        {
                            sqlwhere = @"where pl.""Moto_Id"" =@Moto_Id and il.""Vendor_Id"" = @Vendor_Id";
                        }

                        else if (sMoto_Id == "" && sVendor_Id != "")
                        {
                            sqlwhere = @"where pl.""Moto_Id"" like @Moto_IdL and il.""Vendor_Id"" = @Vendor_Id";
                        }
                        string Sql = @"select ""CC_Type""
                        from tbl_info_list il 
						left join 
                        tbl_vendor_list vl on il.""Vendor_Id""=vl.""Vendor_Id""
                        left join
                        tbl_power_list pl on il.""Moto_Id"" = pl.""Moto_Id""";
                        string sqlGB = @" GROUP BY ""CC_Type""";
                        string sSql = Sql + sqlwhere + sqlGB;
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("Vendor_IdL", "%" + sVendor_Id + "%");
                            cmd.Parameters.AddWithValue("Moto_IdL", "%" + sMoto_Id + "%");
                            cmd.Parameters.AddWithValue("Vendor_Id", sVendor_Id);
                            cmd.Parameters.AddWithValue("Moto_Id", sMoto_Id);
                            //cmd.Parameters.AddWithValue("CC_Type", "%" + sCC_Type + "%");
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            conn.Close();
                            conn.Dispose();
                            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
                            context.Response.Write(strJSON);
                        }

                        catch (Exception ex)
                        {
                            string sResultJson = "";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            sResultJson = "{\"result\":\"error\"}";
                            context.Response.Write(sResultJson);
                        }
                        break;
                    }//抓取CC數資訊
                case "GetVendor_confidence_GridData":
                    {
                        string sRows = (context.Request.Form["rows"] != null) ? context.Request.Form["rows"] : context.Request["rows"];
                        string sTime1 = (context.Request.Form["Time1"] != null) ? context.Request.Form["Time1"] : context.Request["Time1"];
                        string sTime2 = (context.Request.Form["Time2"] != null) ? context.Request.Form["Time2"] : context.Request["Time2"];
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sSql = @"select vdl.""Vendor_Name"",il.""Moto_Name"",il.""Moto_Id"",max(vl.""Sales_Volume"") AS Sales_Volume
	                                    from tbl_vendor_list vdl
	                                    left join tbl_info_list il on il.""Vendor_Id""= vdl.""Vendor_Id""
	                                    left join tbl_sales_volume_list vl on il.""Moto_Id""= vl.""Moto_Id""
	                                    where ""Sales_Date"" BETWEEN @time1 and @time2
	                                    GROUP BY vdl.""Vendor_Name"",il.""Moto_Name"",il.""Moto_Id"",vl.""Sales_Date"",vl.""Sales_Volume""
	                                    order by vl.""Sales_Volume"" DESC
	                                    limit 5";
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("time1",sTime1 );
                            cmd.Parameters.AddWithValue("time2",sTime2 );
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            conn.Close();
                            conn.Dispose();
                            var jsonData = new
                            {
                                total = dt.Rows.Count,
                                page = 1,
                                records = Convert.ToInt32(dt.Rows.Count / Convert.ToInt32(sRows)),
                                rows = dt
                            };
                            string strJSON = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                            context.Response.Write(strJSON);
                        }
                        catch (Exception ex)
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            context.Response.Write("{\"Error\"}");
                        }
                        break;
                    }//抓取最高銷量資訊傳回前端
                case "GetVendorList":
                    {
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sSql = @"select * from tbl_vendor_list";
                        try
                        {
                            string sSQL = "";
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            conn.Close();
                            conn.Dispose();
                            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
                            context.Response.Write(strJSON);
                        }
                        catch (Exception ex)
                        {

                        }
                        break;

                    }//抓取廠商資訊
                case "GetMotoGrid_MotoGridData":
                    {
                        string sRows = (context.Request.Form["rows"] != null) ? context.Request.Form["rows"] : context.Request["rows"];
                        string sVendor_Id = (context.Request.Form["Vendor_Id"] != null) ? context.Request.Form["Vendor_Id"] : context.Request["Vendor_Id"];
                        string sMoto_Id = (context.Request.Form["Moto_Id"] != null) ? context.Request.Form["Moto_Id"] : context.Request["Moto_Id"];
                        string sCC_Type = (context.Request.Form["CC_Type"] != null) ? context.Request.Form["CC_Type"] : context.Request["CC_Type"];
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sqlwhere = "";
                        if (sMoto_Id == "") {
                            sqlwhere = @"where ""CC_Type"" like @CC_Type AND il.""Vendor_Id"" like @Vendor_Id AND il.""Moto_Id"" like @Moto_Id";
                        }
                        else
                        {
                            sqlwhere = @"where ""CC_Type"" like @CC_Type AND il.""Vendor_Id"" like @Vendor_Id AND il.""Moto_Id"" = @Moto_Id";
                        }
                        string Sql = @"select il.""Moto_Id"",""Brake_Type"",""Moto_Name"",""Price"",vl.""Vendor_Id"",""Vendor_Name"",""Sales_Volume"",""Sales_Date"",""Engine_Form"",""Front_Suspension"",""Back_Suspension"",""Max_Power"",""Max_Torque"",""Transfer_Method"",""Supply_Oil"",""Usb"",""Size_L"",""Size_W"",""Size_H"",""Oil_Consumption"",""Oil_Capacity"",""Sit_Height"",""Weight"",""Wheel_Base"",pl.""CC_Type""
                                        from tbl_info_list il 
                                        left join 
                                        tbl_power_list pl on il.""Moto_Id"" = pl.""Moto_Id""
                                        left join 
                                        tbl_shape_list sl on pl.""Moto_Id""= sl.""Moto_Id"" and sl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_sales_volume_list svl on sl.""Moto_Id""=svl.""Moto_Id"" and svl.""Moto_Id""=pl.""Moto_Id"" and svl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_public_time_list ptl on ptl.""Moto_Id""=svl.""Moto_Id"" and ptl.""Moto_Id""=sl.""Moto_Id"" and ptl.""Moto_Id""=pl.""Moto_Id"" and ptl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl__oil_info_list  oil on oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=svl.""Moto_Id"" and oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=sl.""Moto_Id"" and oil.""Moto_Id""=pl.""Moto_Id"" and oil.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_vendor_list vl on il.""Vendor_Id""=vl.""Vendor_Id""";
                        string sSql = Sql + sqlwhere;

                        //
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("CC_Type", "%" + sCC_Type + "%");
                            cmd.Parameters.AddWithValue("Vendor_Id", "%" + sVendor_Id + "%");
                            if (sMoto_Id == "")
                            {
                                cmd.Parameters.AddWithValue("Moto_Id", "%" + sMoto_Id + "%");
                            }
                            else
                            {
                                cmd.Parameters.AddWithValue("Moto_Id", sMoto_Id);
                            }

                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            conn.Close();
                            conn.Dispose();
                            var jsonData = new
                            {
                                total = dt.Rows.Count,
                                page = 1,
                                records = Convert.ToInt32(dt.Rows.Count / Convert.ToInt32(sRows)),
                                rows = dt
                            };
                            string strJSON = JsonConvert.SerializeObject(jsonData, Formatting.Indented);
                            context.Response.Write(strJSON);
                        }
                        catch (Exception ex)
                        {
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            context.Response.Write("{\"Error\"}");
                        }
                        break;
                    }//回傳全機車資訊給前端
                case "GetStockInfo":
                    {
                        sAction = (context.Request.Form["action"] != null) ? context.Request.Form["action"] : context.Request["action"];
                        String sStock = (context.Request.Form["stock"] != null) ? context.Request.Form["stock"] : context.Request["stock"];

                        string sqlwhere = "";
                        string result = "";
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        if (sStock == "")
                        {
                            sqlwhere = @"where  il.""Moto_Name"" like @Moto_Name1";
                        }
                        else
                        {
                            sqlwhere = @"where  il.""Moto_Name"" like @Moto_Name1";
                        }
                        string Sql = @"select il.""Moto_Id"",""Brake_Type"",""Moto_Name"",""Price"",vl.""Vendor_Id"",""Vendor_Name"",""Sales_Volume"",""Sales_Date"",""Engine_Form"",""Front_Suspension"",""Back_Suspension"",""Max_Power"",""Max_Torque"",""Transfer_Method"",""Supply_Oil"",""Usb"",""Size_L"",""Size_W"",""Size_H"",""Oil_Consumption"",""Oil_Capacity"",""Sit_Height"",""Weight"",""Wheel_Base"",pl.""CC_Type""
                                        from tbl_info_list il 
                                        left join 
                                        tbl_power_list pl on il.""Moto_Id"" = pl.""Moto_Id""
                                        left join 
                                        tbl_shape_list sl on pl.""Moto_Id""= sl.""Moto_Id"" and sl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_sales_volume_list svl on sl.""Moto_Id""=svl.""Moto_Id"" and svl.""Moto_Id""=pl.""Moto_Id"" and svl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_public_time_list ptl on ptl.""Moto_Id""=svl.""Moto_Id"" and ptl.""Moto_Id""=sl.""Moto_Id"" and ptl.""Moto_Id""=pl.""Moto_Id"" and ptl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl__oil_info_list  oil on oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=svl.""Moto_Id"" and oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=sl.""Moto_Id"" and oil.""Moto_Id""=pl.""Moto_Id"" and oil.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_vendor_list vl on il.""Vendor_Id""=vl.""Vendor_Id""";
                        string sSql = Sql + sqlwhere;
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("Moto_Name1","%"+sStock+"%");
                            cmd.Parameters.AddWithValue("Moto_Name2",sStock);
                            //cmd.Parameters.AddWithValue("Moto_Id", "%" + sMoto_Id + "%");
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            conn.Close();
                            conn.Dispose();
                            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
                            context.Response.Write(strJSON);
                        }
                        catch (Exception ex)
                        {
                            string sResultJson = "";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            sResultJson = "{\"result\":\"error\"}";
                            context.Response.Write(sResultJson);
                        }

                        context.Response.Write(result);//回傳前端
                        break;
                    }/*抓取收尋值*/
                case "forpeppersay":
                    {
                        sAction = (context.Request.Form["action"] != null) ? context.Request.Form["action"] : context.Request["action"];
                        String sStock = (context.Request.Form["stock"] != null) ? context.Request.Form["stock"] : context.Request["stock"];

                        string sqlwhere = "";
                        string result = "";
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        if (sStock == "")
                        {
                            sqlwhere = @"where  il.""Moto_Name"" like @Moto_Name1";
                        }
                        else
                        {
                            sqlwhere = @"where  il.""Moto_Name"" like @Moto_Name1";
                        }
                        string Sql = @"select il.""Moto_Id"",""Brake_Type"",""Moto_Name"",""Price"",vl.""Vendor_Id"",""Vendor_Name"",""Sales_Volume"",""Sales_Date"",""Engine_Form"",""Front_Suspension"",""Back_Suspension"",""Max_Power"",""Max_Torque"",""Transfer_Method"",""Supply_Oil"",""Usb"",""Size_L"",""Size_W"",""Size_H"",""Oil_Consumption"",""Oil_Capacity"",""Sit_Height"",""Weight"",""Wheel_Base"",pl.""CC_Type""
                                        from tbl_info_list il 
                                        left join 
                                        tbl_power_list pl on il.""Moto_Id"" = pl.""Moto_Id""
                                        left join 
                                        tbl_shape_list sl on pl.""Moto_Id""= sl.""Moto_Id"" and sl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_sales_volume_list svl on sl.""Moto_Id""=svl.""Moto_Id"" and svl.""Moto_Id""=pl.""Moto_Id"" and svl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_public_time_list ptl on ptl.""Moto_Id""=svl.""Moto_Id"" and ptl.""Moto_Id""=sl.""Moto_Id"" and ptl.""Moto_Id""=pl.""Moto_Id"" and ptl.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl__oil_info_list  oil on oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=svl.""Moto_Id"" and oil.""Moto_Id""=ptl.""Moto_Id"" and oil.""Moto_Id""=sl.""Moto_Id"" and oil.""Moto_Id""=pl.""Moto_Id"" and oil.""Moto_Id""=il.""Moto_Id""
                                        left join 
                                        tbl_vendor_list vl on il.""Vendor_Id""=vl.""Vendor_Id""";
                        string sSql = Sql + sqlwhere;
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("Moto_Name1", "%" + sStock + "%");
                            cmd.Parameters.AddWithValue("Moto_Name2", sStock);
                            //cmd.Parameters.AddWithValue("Moto_Id", "%" + sMoto_Id + "%");
                            dr = cmd.ExecuteReader();
                            DataTable dt = new DataTable();
                            dt.Load(dr);
                            dr.Close();
                            string sSay = @"煞車系統" + dt.Rows[0]["Brake_Type"] + " 價錢" + dt.Rows[0]["Price"] + "廠牌 " + dt.Rows[0]["Vendor_Name"] ;
                            conn.Close();
                            conn.Dispose();
                            context.Response.Write(sSay);
                        }
                        catch (Exception ex)
                        {
                            string sResultJson = "";
                            if (conn.State == ConnectionState.Open)
                            {
                                conn.Close();
                            }
                            conn.Dispose();
                            sResultJson = "{\"result\":\"error\"}";
                            context.Response.Write(sResultJson);
                        }

                        context.Response.Write(result);//回傳前端
                        break;
                    }//給pepper說的話
                case "DelMember":
                    {
                        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
                        string sMemberID = (context.Request.Form["member_id"] != null) ? context.Request.Form["member_id"] : context.Request["member_id"];
                        string sSql = @"select * from tbl_picture_list
                        where member_id=@member_id";
                        try
                        {
                            conn.Open();
                            NpgsqlCommand cmd = new NpgsqlCommand();
                            cmd.Connection = conn;
                            NpgsqlDataReader dr;
                            cmd.CommandText = sSql;
                            cmd.Parameters.AddWithValue("member_id", sMemberID);
                            int iDeleCount = cmd.ExecuteNonQuery();
                            string sResultJson = "";
                            if (iDeleCount > 0)
                            {
                                sResultJson = "{\"result\":\"true\"}";
                            }
                            else
                            {
                                sResultJson = "{\"result\":\"false\"}";
                            }
                            context.Response.Write(sResultJson);
                        }
                        catch (Exception ex)
                        {

                        }
                        break;

                    }//抓取圖片
            }
        }
        //case "GetVendor_confidence":
        //    {
        //        NpgsqlConnection conn = new NpgsqlConnection(ConnectionString);
        //        string sSql = @"select*from tbl_vendor_list";
        //        try
        //        {
        //            string sSQL = "";
        //            conn.Open();
        //            NpgsqlCommand cmd = new NpgsqlCommand();
        //            cmd.Connection = conn;
        //            NpgsqlDataReader dr;
        //            cmd.CommandText = sSql;
        //            dr = cmd.ExecuteReader();
        //            DataTable dt = new DataTable();
        //            dt.Load(dr);
        //            dr.Close();
        //            conn.Close();
        //            conn.Dispose();
        //            string strJSON = JsonConvert.SerializeObject(dt, Formatting.Indented);
        //            context.Response.Write(strJSON);
        //        }
        //        catch (Exception ex)
        //        {

        //        }
        //        break;

        //    }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
﻿using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;

namespace AutoMapBankCard
{
    public partial class Search : System.Web.UI.Page
    {
        DBHelper _dBHelper = new DBHelper();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbCount.Text = BankCardHelper.CardNumber.ToString();
                BindDDLPageIndex();
                BindGV();
            }
        }
        private void BindGV()
        {
            var pageSize = int.Parse(ddlPageSize.SelectedValue);
            var pageIndex = int.Parse(ddlPageIndex.SelectedValue);
            var startIndex = (pageIndex - 1) * pageSize + 1;
            var endIndex = pageIndex * pageSize;
            var sourceTable = _dBHelper.GetBankCardList(startIndex, endIndex);
            ProcessData(sourceTable);
            gvShow.DataSource = sourceTable;
            gvShow.DataBind();
        }
        private void BindDDLPageIndex()
        {
            double total = BankCardHelper.CardNumber;
            double pageSize = double.Parse(ddlPageSize.SelectedValue);
            var pageIndex = Convert.ToInt16(Math.Ceiling(total / pageSize));
            for (int i = 1; i <= pageIndex; i++)
                ddlPageIndex.Items.Add(new ListItem(i.ToString(), i.ToString()));
        }
        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlPageIndex.Items.Clear();
            BindDDLPageIndex();
            ddlPageIndex.SelectedIndex = 0;
            BindGV();
        }
        protected void ddlPageIndex_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGV();
        }
        private void ProcessData(DataTable dtSource)
        {
            foreach (DataRow dr in dtSource.Rows)
            {
                dr["AccountName"] = ProcessStringToStar(dr["AccountName"].ToString(), "back", 1);
                dr["AccountNumber"] = ProcessStringToStar(dr["AccountNumber"].ToString(), "front", 7);
            }
        }
        private string ProcessStringToStar(string source, string mode, int remainingLength)
        {
            if (source.Length == 1) return source;

            var replaceLength = source.Length > remainingLength ? source.Length - remainingLength : 1;
            if (mode == "front")
                return new string('*', replaceLength) + source.Substring(replaceLength, source.Length - replaceLength);
            else
                return source.Substring(0, source.Length - replaceLength) + new string('*', replaceLength);
        }
    }
}